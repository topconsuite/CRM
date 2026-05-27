/* OP-4317 - Lucas Matheus - 17/01/2024 */

CREATE TABLE IF NOT EXISTS `topsys`.`con_obras_frente` (

	`id` CHAR(36) NOT NULL,
	
	`obra_usina` SMALLINT(5) UNSIGNED NOT NULL,
	`obra_numero` MEDIUMINT(8) UNSIGNED NOT NULL,
	`obra_versao` INT(10) UNSIGNED NOT NULL,
	`obra_sequencia` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
	
	`obra_nome` CHAR(30) NOT NULL DEFAULT '',
	
	`obra_cep` CHAR(8) NOT NULL DEFAULT '',
	`obra_end` CHAR(40) NOT NULL DEFAULT '',
	`obra_num` MEDIUMINT(6) UNSIGNED NOT NULL DEFAULT '0',
	`obra_compl` CHAR(30) NOT NULL DEFAULT '',
	`obra_bairro` CHAR(20) NOT NULL DEFAULT '',
	
	PRIMARY KEY(id),
	INDEX `obra` (`obra_usina`, `obra_numero`, `obra_versao`) USING BTREE,
	INDEX `obra_frente` (`obra_usina`, `obra_numero`, `obra_versao`, `obra_sequencia`) USING BTREE
	
);