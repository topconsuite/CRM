/* OP-5304 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reajuste_item' AND column_name='id_reprovacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reajuste_item` ADD COLUMN `id_reprovacao` CHAR(19) NOT NULL DEFAULT "" AFTER `id_aprov_versao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reajuste_item_versao' AND column_name='id_reprovacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reajuste_item_versao` ADD COLUMN `id_reprovacao` CHAR(19) NOT NULL DEFAULT "" AFTER `id_aprov_versao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reaj_bomba' AND column_name='id_reprovacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reaj_bomba` ADD COLUMN `id_reprovacao` CHAR(19) NOT NULL DEFAULT "" AFTER `id_aprov_versao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_reaj_bomba_versao' AND column_name='id_reprovacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_reaj_bomba_versao` ADD COLUMN `id_reprovacao` CHAR(19) NOT NULL DEFAULT "" AFTER `id_aprov_versao`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

UPDATE `topsys`.`usr_programa` SET `Titulo` = 'Confirma Reajuste Contrato' WHERE (`Aplicativo` = 'WEB') and (`num` = '6008');

DELETE FROM `topsys`.`usr_programa` WHERE (`Aplicativo` = 'CON') and (`num` = '6117');

DELETE FROM usr_dir_grupou WHERE sigla='CON' AND num_prog=6117;
DELETE FROM usr_dir_grupo WHERE sigla='CON' AND num_prog=6117;

REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `menu`, `seq_menu`) VALUES ('WEB', '6009', 'Aprova Reajuste Contrato', '1', '11');
REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `menu`, `seq_menu`) VALUES ('WEB', '6010', 'Reprova Reajuste Contrato', '1', '12');

CREATE TABLE IF NOT EXISTS `topsys`.`con_reajuste_log` (
  `usina` SMALLINT(3) NOT NULL DEFAULT 0,
  `ano_contrato` TINYINT(2) NOT NULL DEFAULT 0,
  `num_contrato` MEDIUMINT(5) NOT NULL DEFAULT 0,
  `dt_vigencia` DATE NOT NULL DEFAULT '0000-00-00',
  `sequencia` INT(11) NOT NULL DEFAULT 0,
  `tipo` CHAR(20) NOT NULL DEFAULT '',
  `dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` CHAR(10) NOT NULL DEFAULT '',
  `evento` CHAR(30) NOT NULL DEFAULT '',
  `complemento` CHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`usina`, `ano_contrato`, `num_contrato`, `dt_vigencia`, `sequencia`, `tipo`));

CREATE TABLE IF NOT EXISTS `topsys`.`con_contrato_reajuste_versao` (
  `num_versao` INT(11) UNSIGNED NOT NULL DEFAULT 0,
  `usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `ano_contrato` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `num_contrato` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `dt_vigencia` DATE NOT NULL DEFAULT '0000-00-00',
  `tipo` CHAR(5) NOT NULL DEFAULT '',
  PRIMARY KEY (`num_versao`, `usina`, `ano_contrato`, `num_contrato`, `dt_vigencia`, `tipo`));