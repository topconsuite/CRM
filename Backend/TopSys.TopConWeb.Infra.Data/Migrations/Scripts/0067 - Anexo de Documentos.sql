SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_interv_anex' AND column_name='ano_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_interv_anex` ADD COLUMN `ano_chamada` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_interv_anex' AND column_name='num_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_interv_anex` ADD COLUMN `num_chamada` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;