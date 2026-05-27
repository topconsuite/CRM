SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='informar_bomba_terc'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_parametro` ADD COLUMN `informar_bomba_terc` TINYINT(1) UNSIGNED NOT NULL DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;