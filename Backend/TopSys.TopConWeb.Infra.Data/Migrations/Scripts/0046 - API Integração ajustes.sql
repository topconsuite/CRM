/* TC-4298 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_equipamento' AND column_name='external_id'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_equipamento` ADD COLUMN `external_id` CHAR(100) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

/* TC-4364 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_funcionario' AND column_name='external_id'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_funcionario` ADD COLUMN `external_id` CHAR(100) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

/* TC-4364 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_programacao' AND column_name='external_id'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_programacao` ADD COLUMN `external_id` CHAR(100) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;