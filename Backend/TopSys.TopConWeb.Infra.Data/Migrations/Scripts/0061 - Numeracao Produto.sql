/* OP-4701 - Livia - 30/04/2024 */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_proposta_item' AND column_name='numeracao_produto'
) > 0, 'SELECT 1;', 'ALTER TABLE con_proposta_item ADD COLUMN `numeracao_produto` int(10) DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_proposta_item_versao' AND column_name='numeracao_produto'
) > 0, 'SELECT 1;', 'ALTER TABLE con_proposta_item_versao ADD COLUMN `numeracao_produto` int(10) DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;