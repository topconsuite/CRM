SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='codigo_cib'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras ADD COLUMN codigo_cib VARCHAR(8) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='construcao_civil_tipo_alvara'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras ADD COLUMN construcao_civil_tipo_alvara TINYINT(1) UNSIGNED NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='codigo_cib'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras_versao ADD COLUMN codigo_cib VARCHAR(8) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='construcao_civil_tipo_alvara'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras_versao ADD COLUMN construcao_civil_tipo_alvara TINYINT(1) UNSIGNED NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;