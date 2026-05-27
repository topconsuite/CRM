SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='lim_aumento_valor_mcc'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_parametro` ADD COLUMN `lim_aumento_valor_mcc` FLOAT(4,2) UNSIGNED NOT NULL DEFAULT "0.00";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='lim_aumento_percentual_mcc'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_parametro` ADD COLUMN `lim_aumento_percentual_mcc` FLOAT(4,2) UNSIGNED NOT NULL DEFAULT "0.00";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;