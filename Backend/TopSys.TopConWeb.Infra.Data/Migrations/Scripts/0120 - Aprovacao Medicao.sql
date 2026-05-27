SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato' AND column_name='aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato` ADD COLUMN `aprov_medicao` CHAR(1) NOT NULL DEFAULT "N" AFTER `finalidade_ctr`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato' AND column_name='tempo_aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato` ADD COLUMN `tempo_aprov_medicao` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER `aprov_medicao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_versao' AND column_name='aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_versao` ADD COLUMN `aprov_medicao` CHAR(1) NOT NULL DEFAULT "N" AFTER `finalidade_ctr`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_versao' AND column_name='tempo_aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_versao` ADD COLUMN `tempo_aprov_medicao` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER `aprov_medicao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel' AND column_name='aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel` ADD COLUMN `aprov_medicao` CHAR(1) NOT NULL DEFAULT "N" AFTER `finalidade_ctr`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel' AND column_name='tempo_aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel` ADD COLUMN `tempo_aprov_medicao` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER `aprov_medicao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_versao' AND column_name='aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_versao` ADD COLUMN `aprov_medicao` CHAR(1) NOT NULL DEFAULT "N" AFTER `finalidade_ctr`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_versao' AND column_name='tempo_aprov_medicao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_versao` ADD COLUMN `tempo_aprov_medicao` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER `aprov_medicao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `menu`, `seq_menu`) VALUES ('WEB', '7008', 'Medição de Contrato', '2', '6');