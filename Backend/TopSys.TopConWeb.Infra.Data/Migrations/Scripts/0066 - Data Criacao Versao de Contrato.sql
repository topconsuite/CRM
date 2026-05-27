SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_versao' AND column_name='dt_versao_criada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_versao` ADD COLUMN `dt_versao_criada` DATE NULL DEFAULT NULL AFTER `num_versao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;