CREATE TABLE IF NOT EXISTS `topsys`.`con_prensa_carga` (
  `prensa_nome` CHAR(20) NOT NULL DEFAULT '',
  `carga` CHAR(6) NOT NULL DEFAULT '',
  PRIMARY KEY (`prensa_nome`));