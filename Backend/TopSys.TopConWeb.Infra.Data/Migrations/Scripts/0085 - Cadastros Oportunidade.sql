/* OP-4266 / OP-4314 */

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7005
, Titulo='Cadastro Tipos de Oportunidade'
, Funcao='IAE'
, menu=1
, seq_menu=13;

CREATE TABLE IF NOT EXISTS `topsys`.`con_tipo_oportunidade`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
    `descricao` VARCHAR(50) NOT NULL DEFAULT "",
	`ativo` TINYINT(1) NOT NULL DEFAULT '0',
	`id_cadast` CHAR(19) NOT NULL DEFAULT '',
	`id_atual` CHAR(19) NOT NULL DEFAULT '',
    PRIMARY KEY(`codigo`)
)ENGINE=InnoDB DEFAULT CHARSET=latin1;

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7006
, Titulo='Cadastro Concorrentes'
, Funcao='IAE'
, menu=1
, seq_menu=14;

CREATE TABLE IF NOT EXISTS `topsys`.`con_concorrente`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
    `descricao` VARCHAR(50) NOT NULL DEFAULT "",
	`ativo` TINYINT(1) NOT NULL DEFAULT '0',
	`id_cadast` CHAR(19) NOT NULL DEFAULT '',
	`id_atual` CHAR(19) NOT NULL DEFAULT '',
    PRIMARY KEY(`codigo`)
)
ENGINE=InnoDB DEFAULT CHARSET=latin1;