/* OP-5708 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_propaganda' AND column_name='ativa'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_propaganda` ADD COLUMN `ativa` TINYINT(1) NOT NULL DEFAULT 0 AFTER `arquivo`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;