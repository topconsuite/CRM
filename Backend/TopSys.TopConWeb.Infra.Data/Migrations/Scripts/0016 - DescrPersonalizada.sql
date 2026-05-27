/* TC-2187 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_def_familiar' AND column_name='slump_inicial'
) > 0, 'SELECT 1;', 'ALTER TABLE con_def_familiar ADD `slump_inicial` smallint(3) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;


/* TC-2245 */
CREATE TABLE IF NOT EXISTS `topsys`.`con_formula_dt_especificacao` (
  `usina` smallint(3) NOT NULL,
  `uso` smallint(4) NOT NULL,
  `pedra` tinyint(2) NOT NULL,
  `slump` smallint(3) NOT NULL,
  `versao` char(6) NOT NULL,
  `tp_resist` smallint(3) NOT NULL,
  `fck` float(3,1) NOT NULL,
  `consumo` smallint(3) NOT NULL,
  `especificacao` char(120) NOT NULL,
  PRIMARY KEY (`usina`,`uso`,`pedra`,`slump`,`versao`,`tp_resist`,`fck`,`consumo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/* TC-2188 */
CREATE TABLE IF NOT EXISTS `topsys`.`con_uso_desc_person` (
  `uso` smallint(4) unsigned NOT NULL default '0',
  `descr` char(150) NOT NULL default ''  ,
PRIMARY KEY  (`uso`)
) ENGINE=Innodb DEFAULT CHARSET=latin1;

INSERT IGNORE INTO con_uso_desc_person(uso, descr) SELECT cod, '' FROM con_uso;
UPDATE con_uso_desc_person set descr = '#TIPORESISTENCIA #MPA/CONSUMO #BRITA #SLUMP #USO' WHERE descr = '';