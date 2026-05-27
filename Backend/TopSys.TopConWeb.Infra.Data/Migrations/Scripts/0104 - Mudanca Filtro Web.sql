/* OP-5609 */
CREATE TABLE IF NOT EXISTS `topsys`.`usr_web_filtros` (
	`usuario` CHAR(10) NOT NULL DEFAULT '',
	`aplicativo` VARCHAR(50) NOT NULL DEFAULT '',
	`json` MEDIUMTEXT NOT NULL DEFAULT '',
	`filter_string` MEDIUMTEXT NOT NULL DEFAULT '',
	PRIMARY KEY (`usuario`, `aplicativo`) USING BTREE
);

DELETE FROM topsys.con_aprovacao_comercial_pendente_traco WHERE aprovacao_status = 3;
DELETE FROM topsys.con_aprovacao_comercial_pendente_bomba WHERE aprovacao_status = 3;
DELETE FROM topsys.con_aprovacao_comercial_pendente_volume WHERE aprovacao_status = 3;
DELETE FROM topsys.con_aprovacao_comercial_pendente_cond_pagto WHERE aprovacao_status = 3;