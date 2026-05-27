/* OP-5281 / OP-4282 */

CREATE TABLE IF NOT EXISTS `topsys`.`con_compromisso` (
    codigo INT NOT NULL AUTO_INCREMENT,
    usuario VARCHAR(10) NOT NULL DEFAULT '',
    descricao VARCHAR(30) NOT NULL DEFAULT '',
    dia_inteiro BOOLEAN,
    data_inicio DATE NULL DEFAULT NULL,
    hora_inicio TIME NULL DEFAULT NULL,
    data_fim DATE  NULL DEFAULT NULL,
    hora_fim TIME NULL DEFAULT NULL,
    local VARCHAR(30) NOT NULL DEFAULT '',
    contato VARCHAR(30) NOT NULL DEFAULT '',
    ddd TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    telefone INT(9) UNSIGNED NOT NULL DEFAULT 0,
    ddd_celular TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    celular INT(9) UNSIGNED NOT NULL DEFAULT 0,
    email VARCHAR(255) NOT NULL DEFAULT '',
    observacao VARCHAR(255) NOT NULL DEFAULT '',
    providencia VARCHAR(255) NOT NULL DEFAULT '',
    conclusao VARCHAR(255) NOT NULL DEFAULT '',
    usina SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
    ano_visita TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    numero_visita MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
    ano_lead TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    numero_lead MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
    ano_oportunidade TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    numero_oportunidade MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
    data_criacao TIMESTAMP,
    PRIMARY KEY (codigo)
);

CREATE TABLE IF NOT EXISTS `topsys`.`con_tarefa` (
    codigo INT AUTO_INCREMENT,
    usuario VARCHAR(10) NOT NULL DEFAULT '',
    descricao VARCHAR(30) NOT NULL DEFAULT '',
    dia_inteiro BOOLEAN,
    data DATE  NULL DEFAULT NULL,
    horario TIME,
    contato VARCHAR(30) NOT NULL DEFAULT '',
    ddd TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    telefone INT(9) UNSIGNED NOT NULL DEFAULT 0,
    ddd_celular TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    celular INT(9) UNSIGNED NOT NULL DEFAULT 0,
    email VARCHAR(255) NOT NULL DEFAULT '',
    finalizado BOOLEAN,
	observacao VARCHAR(255) NOT NULL DEFAULT '',
    providencia VARCHAR(255) NOT NULL DEFAULT '',
    conclusao VARCHAR(255) NOT NULL DEFAULT '',
	usina SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
    ano_visita TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    numero_visita MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
    ano_lead TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    numero_lead MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
    ano_oportunidade TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
    numero_oportunidade MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
    data_criacao TIMESTAMP,
    PRIMARY KEY (codigo)
);

CREATE TABLE IF NOT EXISTS `topsys`.`con_tarefa_log` (
  `codigo_tarefa` INT NOT NULL,
  `dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` CHAR(10) NOT NULL DEFAULT '',
  `descricao_tarefa` VARCHAR(30) NOT NULL DEFAULT '',
  `usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `ano_oportunidade` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_oportunidade` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `evento` CHAR(40) NOT NULL DEFAULT '',
  `complemento` CHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`codigo_tarefa`,`dt_hora_evento`,`usuario`, `evento`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `topsys`.`con_compromisso_log` (
  `codigo_compromisso` INT NOT NULL,
  `dt_hora_evento` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` CHAR(10) NOT NULL DEFAULT '',
  `descricao_compromisso` VARCHAR(30) NOT NULL DEFAULT '',
  `usina` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `ano_visita` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_visita` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `ano_lead` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_lead` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `ano_oportunidade` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `numero_oportunidade` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `evento` CHAR(40) NOT NULL DEFAULT '',
  `complemento` CHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`codigo_compromisso`,`dt_hora_evento`,`usuario`, `evento`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

REPLACE INTO `topsys`.`usr_menu` (`aplicativo`, `cod`, `descr`) VALUES ('WEB', '10', 'Agenda');

REPLACE INTO topsys.usr_programa
    SET Aplicativo='WEB'
    , num=7007
    , Titulo='Agenda'
    , Funcao='IAE'
    , menu=10
    , seq_menu=1;

INSERT IGNORE `topsys`.`usr_dir_grupou`
    SELECT 'WEB' Sigla,  Nome_Grupo, 7007 Num_Prog, Usuario, 'S' Acesso, 'S' Dir_Inc, 'S' Dir_Alt, 'S' Dir_Exc, '' Dir_Rel
    FROM `topsys`.`usr_dir_grupou` 
    WHERE sigla = 'WEB'
    GROUP BY nome_grupo, usuario
    HAVING GROUP_CONCAT(num_prog) NOT LIKE '%7007%'
