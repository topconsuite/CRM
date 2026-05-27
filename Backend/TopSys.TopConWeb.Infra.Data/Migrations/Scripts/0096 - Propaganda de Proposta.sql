CREATE TABLE IF NOT EXISTS `topsys`.`con_propaganda` (

	`id` CHAR(36) NOT NULL,
	`data` DATE NULL DEFAULT NULL,
	`data_hora` DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
	
	`nome` CHAR(70) NOT NULL DEFAULT '',
	`usuario` CHAR(10) NOT NULL DEFAULT '',
	`arquivo` LONGBLOB NOT NULL,

	PRIMARY KEY (`id`),	
	UNIQUE KEY (`nome`, `data_hora`)

);