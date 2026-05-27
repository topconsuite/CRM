SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topfixo' AND table_name='ger_cep' AND column_name='confiavel'
) > 0, 'SELECT 1;', 'ALTER TABLE `topfixo`.`ger_cep` ADD COLUMN `confiavel` BIT(1) NOT NULL DEFAULT 0 AFTER `intinerante`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

ALTER TABLE `topsys`.`con_aprov` CHANGE COLUMN `chave` `chave` CHAR(36) NOT NULL DEFAULT '' ;

ALTER TABLE `topsys`.`con_aprov_script` CHANGE COLUMN `chave_email` `chave_email` CHAR(36) NOT NULL DEFAULT '' ;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='obriga_email_pf'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_parametro` ADD COLUMN `obriga_email_pf` tinyint(1) unsigned NOT NULL default \'0\';'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

CREATE TABLE IF NOT EXISTS `topsys`.`ger_parametro` (
	`grupo` char(50) NOT NULL DEFAULT '',
	`chave` char(100) NOT NULL DEFAULT '',
	`valor` char(250) NOT NULL DEFAULT '',
	PRIMARY KEY (`grupo`,`chave`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE OR REPLACE VIEW topsys.view_endereco AS
SELECT c.CEP cep,c.BAIRRO bairro,m.cod municipio,0 numero,'' complemento
,IF(((c.LOGRADOURO <> c.CIDADE) AND (c.LOGRADOURO <> '')),convert(concat(c.TIPON,' ',c.LOGRADOURO) using latin1),'') logradoudo,c.confiavel
FROM topfixo.ger_cep c
INNER JOIN topsys.ger_municipio m ON c.CIDADE=m.mun AND c.ESTADO=m.uf;

CREATE OR REPLACE VIEW topsys.view_slump_real AS
SELECT amplitude_de cod, variavao, amplitude_de FROM topsys.con_slump
GROUP BY amplitude_de;

CREATE OR REPLACE VIEW topsys.view_con_chtel_dep AS
SELECT * FROM topsys.con_contrato_dep;

CREATE OR REPLACE VIEW topsys.view_con_chtel_ccredit AS
SELECT * FROM topsys.con_contrato_ccredit;

CREATE OR REPLACE VIEW topsys.view_con_contrato_pag AS
SELECT cp.*,o.numero no_obra
FROM topsys.con_contrato_pag cp
INNER JOIN topsys.con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato;

CREATE OR REPLACE VIEW topsys.view_cartao_bandeira AS
SELECT b.*, g.descr, g.descr_reduzida
FROM topsys.ger_geral g
INNER JOIN topsys.con_bandeira b ON g.cod=b.cod_bandeira;

CREATE OR REPLACE VIEW topsys.view_tipo_cobranca AS
SELECT tc.*, g.descr, g.descr_reduzida
FROM topsys.ger_geral g
INNER JOIN topsys.con_tipo_cobranca tc ON g.cod=tc.tipo_cobranca;

CREATE OR REPLACE VIEW topsys.view_obras_pendentes_aprov AS
/* TRAÇOS PENDENTES */
(SELECT t.usina, t.no_obra numero
FROM topsys.con_proposta_item t
WHERE aprov_verbal='N'
GROUP BY t.usina, t.no_obra)
	UNION
/* BOMBAS PENDENTES */
(SELECT b.usina, b.no_obra numero
FROM topsys.con_prop_bomba b
WHERE b.aprov_verbal='S'
GROUP BY b.usina, b.no_obra)
	UNION
/* TAXAS EXTRAS PENDENTES */
(SELECT obras.usina, obras.numero
FROM topsys.con_obras_tx tx
INNER JOIN topsys.con_obras obras
	ON tx.usina=obras.obra_usina AND tx.obra=obras.numero
WHERE tx.aprov_desc='N'
GROUP BY obras.usina, obras.numero)
	UNION
/* DEMAIS APROVAÇÕES PENDENTES */
(SELECT obras.usina, obras.numero
FROM topsys.con_aprov aprovacaoComercial
INNER JOIN topsys.con_obras obras
ON SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 1 )=obras.usina
AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', -1 ),'-',1)=obras.ano_chamada
AND SUBSTRING_INDEX(SUBSTRING_INDEX( aprovacaoComercial.complemento , '/', 2 ),'/',-1)=obras.no_chamada
WHERE (aprovacaoComercial.dt_hora_exec='1000-01-01 00:00:00' OR ISNULL(aprovacaoComercial.dt_hora_exec))
AND aprovacaoComercial.tipo_aprov IN(1,2)
GROUP BY obras.usina, obras.numero);

# *************************************************************************
CREATE TABLE IF NOT EXISTS `topsys`.`sequence_proposta` (
   `usina` smallint(6) NOT NULL DEFAULT '0',
   `ano` tinyint(4) NOT NULL DEFAULT '0',
   `numero` mediumint(9) NOT NULL AUTO_INCREMENT,
   PRIMARY KEY (`usina`,`ano`,`numero`)
 ) ENGINE=MyISAM DEFAULT CHARSET=latin1;

REPLACE INTO topsys.sequence_proposta (usina, ano, numero)
SELECT usina, ano_chamada, MAX(num_chamada)
FROM con_chtel
GROUP BY usina, ano_chamada;

DELIMITER $$
DROP TRIGGER IF EXISTS topsys.con_chtel_BEFORE_INSERT$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_chtel_BEFORE_INSERT` BEFORE INSERT ON `topsys`.`con_chtel` FOR EACH ROW
BEGIN
	IF ( ISNULL(NEW.num_chamada) OR NEW.num_chamada = 0 ) THEN 
		INSERT INTO topsys.sequence_proposta (usina, ano) VALUES (NEW.usina, NEW.ano_chamada);
		SET NEW.num_chamada = (SELECT last_insert_id());
    ELSE
		REPLACE INTO topsys.sequence_proposta (usina, ano, numero) VALUES (NEW.usina, NEW.ano_chamada, NEW.num_chamada);
    END IF;
    DELETE FROM topsys.sequence_proposta WHERE usina=NEW.usina AND ano=NEW.ano_chamada AND numero<NEW.num_chamada;
    SET @NUMERO_PROPOSTA_INSERIDA = NEW.num_chamada;
END$$
DELIMITER ;

CREATE TABLE IF NOT EXISTS `topsys`.`sequence_obra` (
   `usina` smallint(6) NOT NULL DEFAULT '0',
   `numero` mediumint(9) NOT NULL AUTO_INCREMENT,
   PRIMARY KEY (`usina`,`numero`)
 ) ENGINE=MyISAM DEFAULT CHARSET=latin1;

REPLACE INTO topsys.sequence_obra (usina, numero)
SELECT usina, MAX(numero)
FROM con_obras
GROUP BY usina;

DELIMITER $$
DROP TRIGGER IF EXISTS topsys.con_obras_BEFORE_INSERT$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_obras_BEFORE_INSERT` BEFORE INSERT ON `topsys`.`con_obras` FOR EACH ROW
BEGIN
	IF ( ISNULL(NEW.numero) OR NEW.numero = 0 ) THEN
		INSERT INTO topsys.sequence_obra (usina) VALUES (NEW.usina);
		SET NEW.numero = (SELECT last_insert_id());
    ELSE
		REPLACE INTO topsys.sequence_obra (usina, numero) VALUES (NEW.usina, NEW.numero);
    END IF;
    DELETE FROM topsys.sequence_obra WHERE usina=NEW.usina AND numero<NEW.numero;
    SET @NUMERO_OBRA_INSERIDA = NEW.numero;
END$$
DELIMITER ;

CREATE TABLE IF NOT EXISTS `topsys`.`sequence_programacao` (
   `usina` smallint(6) NOT NULL DEFAULT '0',
   `obra_numero` mediumint(6) NOT NULL DEFAULT '0',
   `sequencia` mediumint(5) NOT NULL AUTO_INCREMENT,
   PRIMARY KEY (`usina`,`obra_numero`,`sequencia`)
 ) ENGINE=MyISAM DEFAULT CHARSET=latin1;

REPLACE INTO topsys.sequence_programacao (usina, obra_numero, sequencia)
SELECT usina, no_obra, MAX(seq_prog)
FROM con_programacao
GROUP BY usina, no_obra;

DELIMITER $$
DROP TRIGGER IF EXISTS topsys.con_programacao_BEFORE_INSERT$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_programacao_BEFORE_INSERT` BEFORE INSERT ON `topsys`.`con_programacao` FOR EACH ROW
BEGIN
	IF ( ISNULL(NEW.seq_prog) OR NEW.seq_prog = 0 ) THEN 
		INSERT INTO topsys.sequence_programacao (usina, obra_numero) VALUES (NEW.usina, NEW.no_obra);
		SET NEW.seq_prog = (SELECT last_insert_id());
    ELSE
		REPLACE INTO topsys.sequence_programacao (usina, obra_numero, sequencia) VALUES (NEW.usina, NEW.no_obra, NEW.seq_prog);
    END IF;
    DELETE FROM topsys.sequence_programacao WHERE usina=NEW.usina AND obra_numero=NEW.no_obra AND sequencia<NEW.seq_prog;
    SET @SEQUENCIA_PROGRAMACAO_INSERIDA = NEW.seq_prog;
END$$
DELIMITER ;
# *************************************************************************

INSERT INTO topsys.ger_versao_script (script)
VALUES('DROP TRIGGER IF EXISTS topsys.con_obras_BEFORE_INSERT;');

INSERT INTO topsys.ger_versao_script (script)
VALUES('DROP TRIGGER IF EXISTS topsys.con_programacao_BEFORE_INSERT;');

INSERT INTO topsys.ger_versao_script (script)
VALUES('DROP TRIGGER IF EXISTS topsys.con_obras_BEFORE_INSERT;');

UPDATE topsys.ger_ver_master_log set master_log_pos=4 WHERE  Master_Log_File<>'';