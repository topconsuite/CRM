using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Helpers
{
    public static class EntityMapHelper
    {
        private static EntitySetMapping getMapping<T>(AppDataContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                          .Single(e => objectItemCollection.GetClrType(e) == typeof(T));

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                      .Single()
                      .EntitySets
                      .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                          .Single()
                          .EntitySetMappings
                          .Single(s => s.EntitySet == entitySet);

            return mapping;
        }

        public static string GetTableName<T>(AppDataContext context)
        {
            var mapping = getMapping<T>(context);

            // Find the storage entity set (table) that the entity is mapped
            var tableEntitySet = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            var tableName = tableEntitySet.MetadataProperties["Table"].Value ?? tableEntitySet.Name;

            // Return the database name from the storage entity set
            var schemaName = tableEntitySet.MetadataProperties["Schema"].Value ?? tableEntitySet.Schema;

            return schemaName + "." + tableName;
        }

        public static string GetTableName<T>(this T source, AppDataContext context)
        {
            return GetTableName<T>(context);
        }

        public static string GetColumnName<T>(string propertyName, AppDataContext context)
        {
            var mapping = getMapping<T>(context);

            // Find the storage entity set (table) that the entity is mapped
            var tableEntitySet = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the table name from the storage entity set
            var tableName = tableEntitySet.MetadataProperties["Table"].Value ?? tableEntitySet.Name;

            // Find the storage property (column) that the property is mapped
            var columnName = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                      .Single(m => m.Property.Name == propertyName)
                .Column
                .Name;

            return tableName + "." + columnName;
        }

        public static string GetColumnName<T>(this T source, string propertyName, AppDataContext context)
        {
            return GetColumnName<T>(propertyName, context);
        }

        public static IEnumerable<string> GetMappedColumnNames<T>(this T source, AppDataContext context) where T : class
        {
            var mapping = getMapping<T>(context);

            // Find the storage property (column) that the property is mapped
            var columnNames = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                      .Select(m => m.Column.Name);

            return columnNames;
        }

        public static IEnumerable<string> GetMappedPropertyNames<T>(this T source, AppDataContext context) where T : class
        {
            var mapping = getMapping<T>(context);

            // Find the storage property (column) that the property is mapped
            var propertyNames = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                      .Select(m => m.Property.Name);

            return propertyNames;
        }

        public static string GetMappedColumnNamesAndPropertyNamesForSelect<T>(AppDataContext context, string prefix) where T : class
        {
            var mapping = getMapping<T>(context);

            var columnNamesAndProperties = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                      .Select(m => $"{prefix}.{m.Column.Name} {m.Property.Name}");

            return String.Join(", ", columnNamesAndProperties);
        }

        public static string GetMappedColumnNamesAndPropertyNamesForSelect<T>(this T source, AppDataContext context, string prefix) where T : class
        {
            return GetMappedColumnNamesAndPropertyNamesForSelect<T>(context, prefix);
        }

        public static string MontarSqlInsert<T>(this T source, AppDataContext context, string[] ignoreProperties = null) where T : class
        {
            var schemaETable = RetornarSchemaETable(source, context);
            var columnsAndValues = RetornarColunasEValores(source, context, ignoreProperties);

            StringBuilder sql = new StringBuilder();

            sql.Clear();
            sql.Append($"INSERT INTO {schemaETable} SET ");
            sql.Append(String.Join(",", columnsAndValues));

            return sql.ToString();
        }

        public static string MontarSqlUpdate<T>(this T source, AppDataContext context, string[] ignoreProperties = null) where T : class
        {
            var schemaETable = RetornarSchemaETable(source, context);
            var columnsAndValues = RetornarColunasEValores(source, context, ignoreProperties, true);
            var where = RetornarWhereComPrimaryKey(source, context);

            StringBuilder sql = new StringBuilder();

            sql.Clear();
            sql.Append($"UPDATE {schemaETable} SET ");
            sql.Append(String.Join(",", columnsAndValues));
            sql.Append($" WHERE {where}");

            return sql.ToString();
        }

        private static List<String> RetornarColunasEValores<T>(T source, AppDataContext context, string[] ignoreProperties = null, bool update = false) where T : class
        {
            var mapping = getMapping<T>(context);

            var propertiesMap = mapping
                .EntityTypeMappings.Where(t => t.EntityType.Name == source.GetType().Name).Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>();

            List<String> columnsAndValues = new List<string>();

            if (!update || context.Entry(source).State == EntityState.Modified)
            {
                var originalValues = update ? context.Entry(source).OriginalValues : null;

                foreach (var propertyMap in propertiesMap)
                {
                    if (ignoreProperties != null && ignoreProperties.Contains(propertyMap.Property.Name)) continue;

                    var propertyName = propertyMap.Property.Name;
                    var currentValue = source.GetType().GetProperty(propertyName).GetValue(source);
                    var originalValue = update ? originalValues?.GetValue<object>(propertyName) : null;

                    if (update && Equals(currentValue, originalValue))
                        continue;

                    if (!update && currentValue == null)
                        continue;

                    columnsAndValues.Add($"{propertyMap.Column.Name}={RetornarCampoFormatadoBanco(currentValue)}");
                }
            }

            return columnsAndValues;
        }

        private static string RetornarSchemaETable<T>(T source, AppDataContext context) where T : class
        {
            var mapping = getMapping<T>(context);

            var tableEntitySet = mapping
                .EntityTypeMappings.Where(t => t.EntityType.Name == source.GetType().Name).Single()
                .Fragments.Single()
                .StoreEntitySet;

            // Return the database name from the storage entity set
            var schemaName = tableEntitySet.MetadataProperties["Schema"].Value ?? tableEntitySet.Schema;

            // Return the table name from the storage entity set
            var tableName = tableEntitySet.MetadataProperties["Table"].Value ?? tableEntitySet.Name;

            return $"{schemaName}.{tableName}";
        }

        private static string RetornarWhereComPrimaryKey<T>(T source, AppDataContext context) where T : class
        {
            var mapping = getMapping<T>(context);

            var entityTypeMapping = mapping
                .EntityTypeMappings.FirstOrDefault(t => t.EntityType.Name == typeof(T).Name);

            if (entityTypeMapping == null)
            {
                throw new Exception("Unmapped entity type.");
            }

            var primaryKeyProperties = entityTypeMapping
                .EntityType
                .KeyMembers
                .Select(k => k.Name)
                .ToList();

            List<string> primaryKeyConditions = new List<string>();

            foreach (var propertyName in primaryKeyProperties)
            {
                var propertyValue = source.GetType().GetProperty(propertyName)?.GetValue(source);
                var propertyColumn = source.GetColumnName(propertyName, context);
                if (propertyValue == null)
                {
                    primaryKeyConditions.Add($"{propertyColumn} IS NULL");
                }
                else
                {
                    primaryKeyConditions.Add($"{propertyColumn}={RetornarCampoFormatadoBanco(propertyValue)}");
                }
            }

            return string.Join(" AND ", primaryKeyConditions);
        }

        private static string RetornarCampoFormatadoBanco(object value)
        {
            if (value is null)
                return "NULL";

            if (value.GetType().BaseType.Name == "Enum")
            {
                return $"{(int)value}";
            }

            switch (value.GetType().Name)
            {
                case "String":
                    return $"'{value.ToString().Replace("'", "''")}'";
                case "DateTime":
                    var d = (DateTime)value;
                    return $"'{d.Year}-{d.Month}-{d.Day} {d.Hour}:{d.Minute}:{d.Second}'";
                case "TimeSpan":
                    var t = (TimeSpan)value;
                    return $"'{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}'";
                case "Guid":
                    return $"'{value.ToString().Replace("'", "''")}'";
                default:
                    return $"{value.ToString().Replace(',', '.')}";
            }
        }
    }
}
