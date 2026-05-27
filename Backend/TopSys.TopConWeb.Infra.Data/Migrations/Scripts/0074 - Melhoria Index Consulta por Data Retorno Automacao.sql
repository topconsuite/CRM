 SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.statistics 
	WHERE table_schema='topsys' AND table_name='con_pesagem' and index_name = 'IDX_DATA_RETORNO'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_pesagem` ADD INDEX `IDX_DATA_RETORNO` (`hora_fim` ASC);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
