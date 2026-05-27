/* TC-4058 */
CREATE TABLE IF NOT EXISTS `topsys`.`con_obras_reajuste` (
`usina` SMALLINT(3) unsigned NOT NULL DEFAULT '0',
`obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
`men_reajuste` VARCHAR(1000) DEFAULT NULL,
PRIMARY KEY (`usina`,`obra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `topsys`.`con_obras_reajuste_versao` (
`num_versao` INT(11) NOT NULL,
`usina` SMALLINT(3) unsigned NOT NULL DEFAULT '0',
`obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
`men_reajuste` VARCHAR(1000) DEFAULT NULL,
PRIMARY KEY (`num_versao`,`usina`,`obra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
