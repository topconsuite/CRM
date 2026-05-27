/* DA-100 */
CREATE TABLE IF NOT EXISTS `topsys`.`sequence_contrato` (
   `usina` smallint(6) NOT NULL DEFAULT '0',
   `ano` tinyint(4) NOT NULL DEFAULT '0',
   `numero` mediumint(9) NOT NULL AUTO_INCREMENT,
   PRIMARY KEY (`usina`,`ano`,`numero`)
 ) ENGINE=MyISAM DEFAULT CHARSET=latin1;

REPLACE INTO topsys.sequence_contrato (usina, ano, numero)
SELECT usina, ano_contrato, MAX(num_contrato)
FROM con_contrato
GROUP BY usina, ano_contrato;

DELIMITER $$
DROP TRIGGER IF EXISTS topsys.con_contrato_BEFORE_INSERT$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_contrato_BEFORE_INSERT` BEFORE INSERT ON `topsys`.`con_contrato` FOR EACH ROW
BEGIN
	IF ( ISNULL(NEW.num_contrato) OR NEW.num_contrato = 0 ) THEN 
		INSERT INTO topsys.sequence_contrato (usina, ano) VALUES (NEW.usina, NEW.ano_contrato);
		SET NEW.num_contrato = (SELECT last_insert_id());
    ELSE
		REPLACE INTO topsys.sequence_contrato (usina, ano, numero) VALUES (NEW.usina, NEW.ano_contrato, NEW.num_contrato);
    END IF;
    DELETE FROM topsys.sequence_contrato WHERE usina=NEW.usina AND ano=NEW.ano_contrato AND numero<NEW.num_contrato;
    SET @NUMERO_CONTRATO_INSERIDO = NEW.num_contrato;
END$$
DELIMITER ;

ALTER TABLE `topsys`.`ger_interv` CHANGE COLUMN `Cod` `Cod` MEDIUMINT(6) UNSIGNED NOT NULL AUTO_INCREMENT;

INSERT INTO topsys.ger_versao_script (script)
VALUES('DROP TRIGGER IF EXISTS topsys.con_contrato_BEFORE_INSERT;');