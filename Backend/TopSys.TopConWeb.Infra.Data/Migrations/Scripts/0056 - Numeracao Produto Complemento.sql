SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_nf_complemento' AND column_name='numeracao_produto'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_nf_complemento` ADD COLUMN `numeracao_produto` INT(10) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;