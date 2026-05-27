SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reajuste_item' AND column_name='id_aprov_versao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reajuste_item` ADD COLUMN `id_aprov_versao` CHAR(19) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reajuste_item_versao' AND column_name='id_aprov_versao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reajuste_item_versao` ADD COLUMN `id_aprov_versao` CHAR(19) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reaj_bomba' AND column_name='id_aprov_versao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reaj_bomba` ADD COLUMN `id_aprov_versao` CHAR(19) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reaj_bomba_versao' AND column_name='id_aprov_versao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reaj_bomba_versao` ADD COLUMN `id_aprov_versao` CHAR(19) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `Funcao`, `menu`, `seq_menu`) VALUES ('WEB', '6008', 'Aprovação Versão Cont./ Reaj.', '', '1', '10');