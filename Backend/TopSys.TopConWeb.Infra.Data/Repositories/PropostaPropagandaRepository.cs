using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class PropostaPropagandaRepository : IPropostaPropagandaRepository
    {

        private readonly AppDataContext _context;

        public PropostaPropagandaRepository(AppDataContext context)
        {
            _context = context;
        }

        public void Adicionar(PropostaPropaganda propaganda, string anexo)
        {

            var sql = new StringBuilder();

            var anexoFinal = anexo;

            if (anexoFinal.Contains(";base64,"))
                anexoFinal = anexoFinal.Substring(anexoFinal.IndexOf(";base64,") + 8);

            var propagandaAnexoConvertida = Convert.FromBase64String(anexoFinal);

            var parameters = new
            {
                Data = DateTime.Now,
                DataHora = DateTime.Now,
                Nome = propaganda.Nome,
                Usuario = propaganda.Usuario,
                Anexo = propagandaAnexoConvertida,
                Id = Guid.NewGuid(),
                Ativa = propaganda.Ativa
            };

            sql.AppendLine($"INSERT INTO topsys.con_propaganda SET");
            sql.AppendLine($"   data = @{nameof(parameters.Data)}");
            sql.AppendLine($",  data_hora = @{nameof(parameters.DataHora)}");
            sql.AppendLine($",  nome = @{nameof(parameters.Nome)}");
            sql.AppendLine($",  usuario = @{nameof(parameters.Usuario)}");
            sql.AppendLine($",  arquivo = @{nameof(parameters.Anexo)}");
            sql.AppendLine($",  id = @{nameof(parameters.Id)}");
            sql.AppendLine($",  ativa = @{nameof(parameters.Ativa)}");

            _context.Database.Connection.Execute(sql.ToString(), parameters);

        }

        public void Atualizar(PropostaPropaganda propaganda)
        {
            var sql = new StringBuilder();

            var ativa = propaganda.Ativa ? 1 : 0;
            sql.Clear();
            sql.Append($"UPDATE con_propaganda SET ativa={ativa} WHERE id='{propaganda.Id}'");

            _context.Database.Connection.Execute(sql.ToString());
        }

        public IEnumerable<PropostaPropaganda> ListarTodos()
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT");
            sql.AppendLine($"  nome {nameof(PropostaPropaganda.Nome)}");
            sql.AppendLine($",  data {nameof(PropostaPropaganda.Data)}");
            sql.AppendLine($",  data_hora {nameof(PropostaPropaganda.DataHora)}");
            sql.AppendLine($",  usuario {nameof(PropostaPropaganda.Usuario)}");
            sql.AppendLine($",  id {nameof(PropostaPropaganda.Id)}");
            sql.AppendLine($",  ativa {nameof(PropostaPropaganda.Ativa)}");
            sql.AppendLine($"FROM topsys.con_propaganda");

            var resultado = _context.Database.Connection.Query<PropostaPropaganda>(sql.ToString());

            return resultado;

        }

        public byte[] ObterAnexo(Guid id)
        {

            var sql = new StringBuilder();

            sql.Append($"SELECT arquivo");
            sql.Append($" FROM topsys.con_propaganda");
            sql.Append($" WHERE id = @{nameof(id)}");

            return _context.Database.Connection.QueryFirstOrDefault<byte[]>(sql.ToString(), new { id });

        }

        public PropostaPropaganda ObterPorId(Guid id)
        {

            var sql = new StringBuilder();

            sql.AppendLine($"SELECT");
            sql.AppendLine($"  nome {nameof(PropostaPropaganda.Nome)}");
            sql.AppendLine($",  data {nameof(PropostaPropaganda.Data)}");
            sql.AppendLine($",  data_hora {nameof(PropostaPropaganda.DataHora)}");
            sql.AppendLine($",  usuario {nameof(PropostaPropaganda.Usuario)}");
            sql.AppendLine($",  id {nameof(PropostaPropaganda.Id)}");
            sql.AppendLine($",  ativa {nameof(PropostaPropaganda.Ativa)}");
            sql.AppendLine($"FROM topsys.con_propaganda");
            sql.AppendLine($" WHERE id = @{nameof(id)}");

            var resultado = _context.Database.Connection.QueryFirstOrDefault<PropostaPropaganda>(sql.ToString(), new { id });

            return resultado;

        }

        public void Remover(Guid id)
        {
            var sql = new StringBuilder();

            sql.AppendLine($"DELETE FROM topsys.con_propaganda");
            sql.AppendLine($" WHERE id = @{nameof(id)}");

            _context.Database.Connection.Execute(sql.ToString(), new { id });
        }
    }
}
