/* OP-5618 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_proposta_item' AND column_name='numeracao_familia'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_proposta_item` ADD COLUMN `numeracao_familia` INT(11) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_proposta_item_versao' AND column_name='numeracao_familia'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_proposta_item_versao` ADD COLUMN `numeracao_familia` INT(11) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;