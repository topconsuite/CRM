SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='obra_km_usina_via_google'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `obra_km_usina_via_google` SMALLINT(3) UNSIGNED DEFAULT 0 AFTER obra_km_usina;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
