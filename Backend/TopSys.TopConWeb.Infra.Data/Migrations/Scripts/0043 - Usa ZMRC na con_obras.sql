/* OP-4462 */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='usa_adicional_zmrc'
) > 0, 'SELECT 1;', 'ALTER TABLE con_obras ADD COLUMN `usa_adicional_zmrc` char(1) NOT NULL DEFAULT "N";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='usa_adicional_zmrc'
) > 0, 'SELECT 1;', 'ALTER TABLE con_obras_versao ADD COLUMN `usa_adicional_zmrc` char(1) NOT NULL DEFAULT "N";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;