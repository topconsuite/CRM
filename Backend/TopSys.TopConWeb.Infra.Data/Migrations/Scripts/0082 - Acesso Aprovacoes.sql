/* OP-4107 */

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7002
, Titulo='Conf. Acesso Aprovacoes'
, Funcao='IAE'
, menu=1
, seq_menu=10;

CREATE TABLE IF NOT EXISTS `topsys`.`usr_grupo_liberacao_acesso`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
	`usina` SMALLINT(5) NOT NULL,
	`descricao` VARCHAR(30) NOT NULL,
	`criado_em` DATETIME NOT NULL,
	`atualizado_em` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`codigo`),
	UNIQUE KEY `usina_descricao` (`usina`, `descricao`)
)
ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `topsys`.`usr_liberacao_acesso`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
	`usuario` VARCHAR(10) DEFAULT NULL,
	`grupo` INT(11) DEFAULT NULL,
    `tipo_liberacao` VARCHAR(20) NOT NULL DEFAULT "",
	`dia_semana` VARCHAR(20) NOT NULL DEFAULT "",
	`turno` INT(1) NOT NULL DEFAULT 0,
	`hora_entrada` TIME,
	`hora_saida` TIME,
	`bloquear` TINYINT(1) NOT NULL DEFAULT 0,
	`criado_em` DATETIME NOT NULL,
	`atualizado_em` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY(`codigo`)
)
ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `topsys`.`usr_periodo_ausencia`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
    `usuario` VARCHAR(10) NOT NULL,
    `tipo_liberacao` VARCHAR(20) NOT NULL DEFAULT "",
	`tipo_ausencia` VARCHAR(20) NOT NULL DEFAULT "",
	`inicio_periodo` DATE DEFAULT NULL,
	`fim_periodo` DATE DEFAULT NULL,
	`criado_em` DATETIME NOT NULL,
	`atualizado_em` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY(`codigo`)
)
ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `usr_liberacao_acesso_log` (
  `tipo_liberacao` VARCHAR(20) NOT NULL DEFAULT "",
  `dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` CHAR(10) NOT NULL DEFAULT '',
  `usuario_modificado` CHAR(10) DEFAULT NULL,
  `usina_grupo` SMALLINT(5) NOT NULL,
  `descricao_grupo` VARCHAR(30) NOT NULL,
  `evento` CHAR(40) NOT NULL DEFAULT '',
  `complemento` CHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`tipo_liberacao`,`dt_hora_evento`,`usuario`, `evento`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;