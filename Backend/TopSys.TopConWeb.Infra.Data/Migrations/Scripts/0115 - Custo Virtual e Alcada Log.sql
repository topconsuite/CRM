CREATE TABLE IF NOT EXISTS `topsys`.`con_aprovacao_comercial_log` (
	`id` CHAR(32) NOT NULL,
	`obra_usina` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
	`obra_numero` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0',
	`obra_versao` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
	`tabela` VARCHAR(55) NOT NULL DEFAULT '',
	`data` DATETIME NOT NULL,
	`source` VARCHAR(120) NOT NULL DEFAULT '',
	`script` TEXT NULL DEFAULT NULL,
	`payload` TEXT NULL DEFAULT NULL,
	PRIMARY KEY (`id`) USING BTREE,
	INDEX (`obra_usina`, `obra_numero`) USING BTREE,
	INDEX (`data`) USING BTREE
);

REPLACE INTO topsys.ger_geral (cod,descr,descr_reduzida,id_cadast,id_atual,fixo,external_id) 
	VALUES (7105,'CUSTO VIRTUAL','CUSTO VIRTUAL','ADMIN 10/07/25','','S','');