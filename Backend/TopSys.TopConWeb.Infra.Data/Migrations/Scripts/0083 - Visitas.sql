/* OP-4283 */

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7003
, Titulo='Cadastro Tipos de Visita'
, Funcao='IAE'
, menu=1
, seq_menu=11;

CREATE TABLE IF NOT EXISTS `topsys`.`con_tipo_visita`
(
	`codigo` INT(11) NOT NULL AUTO_INCREMENT,
    `descricao` VARCHAR(50) NOT NULL DEFAULT "",
	`ativo` TINYINT(1) NOT NULL DEFAULT '0',
	`id_cadast` CHAR(19) NOT NULL DEFAULT '',
	`id_atual` CHAR(19) NOT NULL DEFAULT '',
    PRIMARY KEY(`codigo`)
)
ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `topsys`.`con_visita_cliente` (

	`usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`num_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',
	
	`data_visita` DATE NOT NULL,
	`hora_visita` TIME NOT NULL,

	`cliente` CHAR(40) NOT NULL DEFAULT '',
	`obra_nome` CHAR(30) NOT NULL DEFAULT '',

	`ddd_telefone` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`telefone` INT(9) UNSIGNED NOT NULL DEFAULT '0',
	`ddd_celular` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`celular` INT(9) UNSIGNED NOT NULL DEFAULT '0',
	`email` CHAR(255) NOT NULL DEFAULT '',

	`vendedor` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`tipo_visita` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	
	`cep` CHAR(8) NOT NULL DEFAULT '',
	`endereco` CHAR(40) NOT NULL DEFAULT '',
	`numero` MEDIUMINT(6) UNSIGNED NOT NULL DEFAULT '0',
	`complemento` CHAR(20) NOT NULL DEFAULT '',
	`bairro` CHAR(20) NOT NULL DEFAULT '',
	`municipio` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`obs_interna` CHAR(100) NOT NULL DEFAULT '',

	`ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`num_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',

	PRIMARY KEY (`usina`, `ano_visita`, `num_visita`),
	INDEX(`ano_lead`, `num_lead`)

);

CREATE TABLE IF NOT EXISTS `topsys`.`con_visita_contato` (

	`usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`num_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',
	`seq` INT(11) NOT NULL DEFAULT '0',
	
	`funcao` SMALLINT(4) UNSIGNED NOT NULL DEFAULT '0',

	`nome` CHAR(40) NOT NULL DEFAULT '',

	`ddd_telefone` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`telefone` INT(9) UNSIGNED NOT NULL DEFAULT '0',
	`ddd_celular` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`celular` INT(9) UNSIGNED NOT NULL DEFAULT '0',
	`email` CHAR(255) NOT NULL DEFAULT '',

	PRIMARY KEY (`usina`, `ano_visita`, `num_visita`, `seq`)

);

CREATE TABLE IF NOT EXISTS `topsys`.`con_visita_log` (

	`usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`num_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',

	`tipo` VARCHAR(20) NOT NULL DEFAULT '',

	`dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
	`usuario` CHAR(10) NOT NULL DEFAULT '',
	`evento` CHAR(30) NOT NULL DEFAULT '',
	`complemento` CHAR(255) NOT NULL DEFAULT '',
	
	PRIMARY KEY (`tipo`,`dt_hora_evento`,`usuario`, `evento`),
	INDEX (`usina`, `ano_visita`, `num_visita`) 
);


CREATE TABLE IF NOT EXISTS `topsys`.`sequence_visita` (
	`usina` SMALLINT(6) NOT NULL DEFAULT '0',
	`ano` TINYINT(4) NOT NULL DEFAULT '0',
	`numero` MEDIUMINT(9) NOT NULL AUTO_INCREMENT,
	PRIMARY KEY (`usina`, `ano`, `numero`)
) ENGINE=MyISAM;

DELIMITER $$
DROP TRIGGER IF EXISTS topsys.con_visita_cliente_BEFORE_INSERT$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_visita_cliente_BEFORE_INSERT` BEFORE INSERT ON `topsys`.`con_visita_cliente` FOR EACH ROW
BEGIN
	IF ( ISNULL(NEW.num_visita) OR NEW.num_visita = 0 ) THEN 
		INSERT INTO topsys.sequence_visita (usina, ano) VALUES (NEW.usina, NEW.ano_visita);
		SET NEW.num_visita = (SELECT last_insert_id());
    ELSE
		REPLACE INTO topsys.sequence_visita (usina, ano, numero) VALUES (NEW.usina, NEW.ano_visita, NEW.num_visita);
    END IF;
    DELETE FROM topsys.sequence_visita WHERE usina=NEW.usina AND ano=NEW.ano_visita AND numero<NEW.num_visita;
    SET @NUMERO_VISITA_INSERIDA = NEW.num_visita;
END$$
DELIMITER ;

CREATE TABLE IF NOT EXISTS `topsys`.`con_visita_anexo` (

	`id` CHAR(36) NOT NULL,
	
	`usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`num_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',

	`descricao` CHAR(70) NOT NULL DEFAULT '',
	`nome` CHAR(70) NOT NULL DEFAULT '',
	`usuario` CHAR(10) NOT NULL DEFAULT '',
	`data` DATE NULL DEFAULT NULL,
	`data_hora` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
	`arquivo` LONGBLOB NOT NULL,

	PRIMARY KEY (`id`),
	INDEX `visita_anex` (`usina`, `ano_visita`, `num_visita`)
) ENGINE=InnoDB;

REPLACE INTO `topsys`.`usr_programa`
	SET Aplicativo='WEB'
		, num=6104
		, Titulo='Visitas'
		, Funcao='IA'
		, menu=5
		, seq_menu=1;

CREATE TABLE IF NOT EXISTS `topsys`.`con_visita_cliente_hist` (

	`id` CHAR(36) NOT NULL,

	`usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`num_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',
	
	`tipo` CHAR(9),
	`descricao` VARCHAR(500) NOT NULL DEFAULT '',
	
	`data` DATE NOT NULL,
	`hora` TIME NOT NULL,
	`data_retorno` DATE DEFAULT NULL,
	`hora_retorno` TIME DEFAULT NULL,
	
	`id_cadast` CHAR(19) NOT NULL DEFAULT '',
	
	PRIMARY KEY(id),
	INDEX(usina, ano_visita, num_visita)
	
);

REPLACE INTO `topsys`.`usr_menu` (`aplicativo`, `cod`, `descr`) VALUES ('WEB', '5', 'Visitas');

REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `Funcao`, `menu`, `seq_menu`) VALUES ('WEB', '6108', 'Interação Visitas', 'I', '5', '2');

INSERT IGNORE usr_dir_grupou
	SELECT 'WEB' Sigla,  Nome_Grupo, 6104 Num_Prog, Usuario, 
	'S' Acesso, 
	'S' Dir_Inc, 
	'S' Dir_Alt, 
	'' Dir_Exc, 
	''  Dir_Rel
	FROM usr_dir_grupou u 
	WHERE sigla = 'WEB'
	GROUP BY nome_grupo, usuario
	HAVING 
			GROUP_CONCAT(num_prog) NOT LIKE '%6104%';
		
INSERT IGNORE usr_dir_grupou
	SELECT 'WEB' Sigla,  Nome_Grupo, 6108 Num_Prog, Usuario, 
	'S' Acesso, 
	'S' Dir_Inc, 
	'' Dir_Alt, 
	'' Dir_Exc, 
	''  Dir_Rel
	FROM usr_dir_grupou u 
	WHERE sigla = 'WEB'
	GROUP BY nome_grupo, usuario
	HAVING 
			GROUP_CONCAT(num_prog) NOT LIKE '%6108%';