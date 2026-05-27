CREATE TABLE IF NOT EXISTS `topsys`.`con_fase_lead` (
  `codigo` INT NOT NULL AUTO_INCREMENT,
  `descricao` VARCHAR(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`codigo`));
  
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (1,'Identificação');
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (2,'Qualificação');
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (3,'Trabalhando');
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (4,'Nutrindo');
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (5,'Apresentação');
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (6,'Oportunidade');
REPLACE INTO `topsys`.`con_fase_lead` (`codigo`,`descricao`) VALUES (7,'Fechado Perdido');

CREATE TABLE IF NOT EXISTS `topsys`.`con_lead` (
  `usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `ano_oportunidade` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_oportunidade` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `data` DATE NULL DEFAULT NULL,
  `cliente` CHAR(40) NOT NULL DEFAULT '',
  `ddd` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `telefone` INT(9) UNSIGNED NOT NULL DEFAULT 0,
  `ddd_celular` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `celular` INT(9) UNSIGNED NOT NULL DEFAULT 0,
  `email` CHAR(255) NOT NULL DEFAULT '',
  `vendedor` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `via_captacao` SMALLINT(4) UNSIGNED NOT NULL DEFAULT 0,
  `fase` INT(11) UNSIGNED NOT NULL DEFAULT 0,
  `classificacao` INT(11) UNSIGNED NOT NULL DEFAULT 0,
  `proxima_etapa` CHAR(255) NOT NULL DEFAULT '',
  `obra_nome` CHAR(30) NOT NULL DEFAULT '',
  `cep` CHAR(8) NOT NULL DEFAULT '',
  `endereco` CHAR(40) NOT NULL DEFAULT '',
  `numero` MEDIUMINT(6) UNSIGNED NOT NULL DEFAULT 0,
  `complemento` CHAR(20) NOT NULL DEFAULT '',
  `bairro` CHAR(20) NOT NULL DEFAULT '',
  `municipio` SMALLINT(3) NOT NULL DEFAULT 0,
  `motivo_perda` INT(11) NOT NULL DEFAULT 0,
  `observacao_interna` CHAR(255) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  PRIMARY KEY (`usina`, `ano_lead`, `numero_lead`));

CREATE TABLE IF NOT EXISTS `topsys`.`con_lead_contato` (
  `usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `sequencia` TINYINT(1) UNSIGNED NOT NULL DEFAULT 0,
  `nome` CHAR(30) NOT NULL DEFAULT '',
  `funcao` SMALLINT(4) NOT NULL DEFAULT 0,
  `ddd` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `telefone` INT(9) UNSIGNED NOT NULL DEFAULT 0,
  `ddd_celular` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `celular` INT(9) UNSIGNED NOT NULL DEFAULT 0,
  `email` char(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`usina`, `ano_lead`, `numero_lead`, `sequencia`));

CREATE TABLE IF NOT EXISTS `topsys`.`con_lead_log` (
  `usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
  `ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
  `numero_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',
  `sequencia` INT(11) NOT NULL DEFAULT '0',
  `tipo` CHAR(20) NOT NULL DEFAULT '',
  `dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` CHAR(10) NOT NULL DEFAULT '',
  `evento` CHAR(30) NOT NULL DEFAULT '',
  `complemento` CHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`usina`, `ano_lead`, `numero_lead`, `sequencia`));
 
CREATE TABLE IF NOT EXISTS `topsys`.`sequence_lead` (
   `usina` smallint(6) NOT NULL DEFAULT '0',
   `ano` tinyint(4) NOT NULL DEFAULT '0',
   `numero` mediumint(9) NOT NULL AUTO_INCREMENT,
   PRIMARY KEY (`usina`,`ano`,`numero`)
 ) ENGINE=MyISAM DEFAULT CHARSET=latin1;

REPLACE INTO topsys.sequence_lead (usina, ano, numero)
SELECT usina, ano_lead, MAX(numero_lead)
FROM con_lead
GROUP BY usina, ano_lead;

DELIMITER $$
DROP TRIGGER IF EXISTS topsys.con_lead_BEFORE_INSERT$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_lead_BEFORE_INSERT` BEFORE INSERT ON `topsys`.`con_lead` FOR EACH ROW
BEGIN
	IF ( ISNULL(NEW.numero_lead) OR NEW.numero_lead = 0 ) THEN 
		INSERT INTO topsys.sequence_lead (usina, ano) VALUES (NEW.usina, NEW.ano_lead);
		SET NEW.numero_lead = (SELECT last_insert_id());
    ELSE
		REPLACE INTO topsys.sequence_lead (usina, ano, numero) VALUES (NEW.usina, NEW.ano_lead, NEW.numero_lead);
    END IF;
    DELETE FROM topsys.sequence_lead WHERE usina=NEW.usina AND ano=NEW.ano_lead AND numero<NEW.numero_lead;
    SET @NUMERO_LEAD_INSERIDA = NEW.numero_lead;
END$$
DELIMITER ;

CREATE TABLE IF NOT EXISTS `topsys`.`con_lead_anexo` (
	`id` CHAR(36) NOT NULL,
	`usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT '0',
	`ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT '0',
	`numero_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT '0',
	`descricao` CHAR(70) NOT NULL DEFAULT '',
	`nome` CHAR(70) NOT NULL DEFAULT '',
	`usuario` CHAR(10) NOT NULL DEFAULT '',
	`data` DATE NULL DEFAULT NULL,
	`data_hora` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
	`arquivo` LONGBLOB NOT NULL,
	PRIMARY KEY (`id`),
	INDEX `lead_index` (`usina`, `ano_lead`, `numero_lead`)
) ENGINE=InnoDB;

REPLACE INTO `topsys`.`usr_menu` (`aplicativo`, `cod`, `descr`) VALUES ('WEB', '6', 'Leads');

REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `Funcao`, `menu`, `seq_menu`) VALUES ('WEB', '6105', 'Leads', 'IA', '6', '1');


		
INSERT IGNORE usr_dir_grupou
	SELECT 'WEB' Sigla,  Nome_Grupo, 6105 Num_Prog, Usuario, 
	'S' Acesso, 
	'S' Dir_Inc, 
	'S' Dir_Alt, 
	'' Dir_Exc, 
	''  Dir_Rel
	FROM usr_dir_grupou u 
	WHERE sigla = 'WEB'
	GROUP BY nome_grupo, usuario
	HAVING 
			GROUP_CONCAT(num_prog) NOT LIKE '%6105%';