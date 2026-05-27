/* Ajuste */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='rota_padrao'
) > 0, 'SELECT 1;', 'ALTER TABLE con_obras_versao ADD COLUMN `rota_padrao` char(36) DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
