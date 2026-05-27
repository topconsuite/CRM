/* OP-5221 */

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7004
, Titulo='Cadastro Motivos da Perda'
, Funcao='IAE'
, menu=1
, seq_menu=12;

CREATE TABLE IF NOT EXISTS `topsys`.`con_motivo_perda`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
    `descricao` VARCHAR(50) NOT NULL DEFAULT "",
	`ativo` TINYINT(1) NOT NULL DEFAULT '0',
	`id_cadast` CHAR(19) NOT NULL DEFAULT '',
	`id_atual` CHAR(19) NOT NULL DEFAULT '',
    PRIMARY KEY(`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `topsys`.`con_motivo_perda_log` (
  `tipo` VARCHAR(20) NOT NULL DEFAULT "",
  `dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` CHAR(10) NOT NULL DEFAULT '',
  `evento` CHAR(40) NOT NULL DEFAULT '',
  `complemento` CHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`tipo`,`dt_hora_evento`,`usuario`, `evento`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;