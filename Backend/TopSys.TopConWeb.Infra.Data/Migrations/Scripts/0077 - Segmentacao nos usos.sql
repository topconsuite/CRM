/* OP-4971 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_uso' AND column_name='id_segmentacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_uso` ADD COLUMN `id_segmentacao` INT(11) UNSIGNED NOT NULL DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;