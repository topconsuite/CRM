/* TC-2643 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_proposta_item' AND column_name='preco_ajustado'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_proposta_item` ADD COLUMN `preco_ajustado` float(6,2) unsigned NOT NULL default 0.00;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

UPDATE con_proposta_item SET preco_ajustado=preco_unit_tab WHERE preco_ajustado=0;

/* TC-2222 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_proposta_item' AND column_name='qtde_m3_bombeado'
) > 0, 'SELECT 1;', 'ALTER TABLE con_proposta_item ADD COLUMN qtde_m3_bombeado float(7,2) NOT NULL DEFAULT -1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

UPDATE con_proposta_item SET qtde_m3_bombeado = if(slump >= 9, qtde_m3, 0) where qtde_m3_bombeado < 0;