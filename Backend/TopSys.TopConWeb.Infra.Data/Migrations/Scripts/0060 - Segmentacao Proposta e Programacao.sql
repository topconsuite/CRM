/* OP-4265 - Lucas Matheus */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel' AND column_name='segmentacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel` ADD COLUMN `segmentacao` INT DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_versao' AND column_name='segmentacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_versao` ADD COLUMN `segmentacao` INT DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
