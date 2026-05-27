/* OP-4170 - Lucas Matheus */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel' AND column_name='dt_ult_versao_gerada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel` ADD COLUMN `dt_ult_versao_gerada` DATE DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_versao' AND column_name='dt_ult_versao_gerada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_versao` ADD COLUMN `dt_ult_versao_gerada` DATE DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;