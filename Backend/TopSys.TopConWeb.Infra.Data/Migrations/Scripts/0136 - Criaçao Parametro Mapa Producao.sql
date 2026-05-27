SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_usina' AND column_name='modelo_mapa_producao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_usina` ADD COLUMN `modelo_mapa_producao` CHAR(1) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;