/*OP-4326 Livia*/
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='email_resp_tecnico'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `email_resp_tecnico` VARCHAR(255) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='email_resp_tecnico'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras_versao` ADD COLUMN `email_resp_tecnico` VARCHAR(255) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;