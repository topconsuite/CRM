SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reaj_bomba' AND column_name='dt_confirmacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reaj_bomba` ADD COLUMN `dt_confirmacao` DATE NULL DEFAULT NULL AFTER `id_atual`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reaj_bomba_versao' AND column_name='dt_confirmacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reaj_bomba_versao` ADD COLUMN `dt_confirmacao` DATE NULL DEFAULT NULL AFTER `id_atual`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='taxa_reajust_ant'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `taxa_reajust_ant` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `pr_m3_bomb_pr_p`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='taxa_reajust_ant'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `taxa_reajust_ant` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `pr_m3_bomb_pr_p`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='m3_pr_reajust_ant'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `m3_pr_reajust_ant` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `taxa_reajust_ant`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='m3_pr_reajust_ant'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `m3_pr_reajust_ant` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `taxa_reajust_ant`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='pr_m3_reajust_ant'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `pr_m3_reajust_ant` FLOAT(6,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `m3_pr_reajust_ant`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='pr_m3_reajust_ant'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `pr_m3_reajust_ant` FLOAT(6,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `m3_pr_reajust_ant`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='taxa_reajustada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `taxa_reajustada` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `pr_m3_reajust_ant`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='taxa_reajustada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `taxa_reajustada` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `pr_m3_reajust_ant`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='m3_pr_reajustada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `m3_pr_reajustada` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `taxa_reajustada`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='m3_pr_reajustada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `m3_pr_reajustada` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `taxa_reajustada`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='pr_m3_reajustada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `pr_m3_reajustada` FLOAT(6,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `m3_pr_reajustada`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='pr_m3_reajustada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `pr_m3_reajustada` FLOAT(6,2) UNSIGNED NOT NULL DEFAULT 0.00 AFTER `m3_pr_reajustada`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba' AND column_name='dt_ult_reajuste'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba` ADD COLUMN `dt_ult_reajuste` DATE NULL DEFAULT NULL AFTER `pr_m3_reajustada`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_prop_bomba_versao' AND column_name='dt_ult_reajuste'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_prop_bomba_versao` ADD COLUMN `dt_ult_reajuste` DATE NULL DEFAULT NULL AFTER `pr_m3_reajustada`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;