/* TD-66 */
CREATE TABLE IF NOT EXISTS topsys.`con_demais_servicos` (
  `cod` smallint(3) unsigned NOT NULL AUTO_INCREMENT,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `merc` char(20) NOT NULL DEFAULT '',
  `Unid_cobranca` char(3) NOT NULL DEFAULT '',
  `Casas_decimais` smallint(3) unsigned DEFAULT NULL,
  `Preco_Sugerido` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Preco_Minimo` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Frequencia_Cobranca` enum('Contrato','Programacao','Remessa','M3','M3Bombeado','Bombeamento') DEFAULT NULL,
  `Forma_Cobranca` enum('NaRemessa','FinalConcretagem') DEFAULT NULL,
  `atualiza_estoque` tinyint(1) DEFAULT '0',
  KEY(`cod`,`usina`),
  PRIMARY KEY (`cod`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/* DA-39 */
CREATE TABLE IF NOT EXISTS topsys.`con_obras_dem_serv` (
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `cod` smallint(3) unsigned NOT NULL DEFAULT '0',
  `usina_entrega` smallint(3) unsigned NOT NULL DEFAULT '0',
  `merc` char(20) NOT NULL DEFAULT '',
  `Unid_cobranca` char(3) NOT NULL DEFAULT '',
  `Casas_decimais` smallint(3) unsigned DEFAULT NULL,
  `Preco_Sugerido` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Preco_Minimo` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Frequencia_Cobranca` enum('Contrato','Programacao','Remessa','M3','M3Bombeado','Bombeamento') DEFAULT NULL,
  `Forma_Cobranca` enum('NaRemessa','FinalConcretagem') DEFAULT NULL,
  `atualiza_estoque` tinyint(1) DEFAULT '0',
  `Preco_Proposto` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Quantidade` double(14,6) unsigned NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`usina`,`obra`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/* DA-40 */
CREATE TABLE IF NOT EXISTS topsys.`con_programacao_dem_serv` (
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq_prog` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `Quantidade` double(14,6) unsigned NOT NULL DEFAULT '0.00',
  `valor_total` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `valor_cobrado` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`usina`,`obra`,`seq_prog`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='vlr_demais_servicos'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras ADD COLUMN `vlr_demais_servicos` double(8,2) unsigned not null default 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;