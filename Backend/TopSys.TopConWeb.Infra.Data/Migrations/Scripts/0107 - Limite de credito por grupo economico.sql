/* OP-4814 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_grupo_economico' AND column_name='Limite_Cred'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_grupo_economico` ADD COLUMN `Limite_Cred` DOUBLE(10,2) NOT NULL DEFAULT 0.00 AFTER `descricao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_grupo_economico' AND column_name='lim_cred_val'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_grupo_economico` ADD COLUMN `lim_cred_val` DATE NULL DEFAULT NULL AFTER `Limite_Cred`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_grupo_economico' AND column_name='Bloq'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_grupo_economico` ADD COLUMN `Bloq` SMALLINT(4) UNSIGNED NOT NULL DEFAULT 0 AFTER `lim_cred_val`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_grupo_economico' AND column_name='obs_bloq'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_grupo_economico` ADD COLUMN `obs_bloq` CHAR(100) NOT NULL DEFAULT "" AFTER `Bloq`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

INSERT IGNORE INTO `topsys`.`ger_parametro` (`grupo`, `chave`, `valor`) VALUES ('web', 'LimiteCreditoPorGrupoEconomico', '0');