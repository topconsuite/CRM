/*OP-4514 - Livia */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel' AND column_name='condicoes_gerais'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_chtel ADD COLUMN condicoes_gerais VARCHAR(1000) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_versao' AND column_name='condicoes_gerais'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_chtel_versao ADD COLUMN condicoes_gerais VARCHAR(1000) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists; 