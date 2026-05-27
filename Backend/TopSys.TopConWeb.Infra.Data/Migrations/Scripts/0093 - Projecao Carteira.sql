SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_projecao' AND column_name='saldo_m3'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras_projecao` ADD COLUMN `saldo_m3` FLOAT(5,1) NOT NULL AFTER `volume_m3`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;