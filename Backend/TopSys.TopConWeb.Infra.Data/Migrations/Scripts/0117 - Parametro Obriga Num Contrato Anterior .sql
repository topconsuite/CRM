SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='obriga_num_ctr_anterior'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_parametro` ADD COLUMN `obriga_num_ctr_anterior` TINYINT(1) UNSIGNED NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

