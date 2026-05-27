SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_ponto_relogio' AND column_name='ultimo_nsr_data'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_ponto_relogio` ADD COLUMN `ultimo_nsr_data` DATETIME DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;