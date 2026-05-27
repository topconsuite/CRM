SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='status_engenharia'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `status_engenharia` tinyint(1) unsigned NOT NULL DEFAULT 0 AFTER status_comercial;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='status_financeiro'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `status_financeiro` tinyint(1) unsigned NOT NULL DEFAULT 0 AFTER status_comercial;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='status_cadastro'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `status_cadastro` tinyint(1) unsigned NOT NULL DEFAULT 0 AFTER status_comercial;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;