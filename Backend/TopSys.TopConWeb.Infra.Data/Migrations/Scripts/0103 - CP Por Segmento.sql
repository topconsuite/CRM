/* OP-5584 */
CREATE TABLE IF NOT EXISTS topsys.con_usina_ruptura_cp (

	`cod` smallint(5) unsigned NOT NULL,
	`id_segmentacao` int(11) NOT NULL DEFAULT '1',
	`medida_forca` smallint(4) DEFAULT '0',
	`gravidade` float(4,3) DEFAULT '0.000',
	`diametro_padrao_cp` float(4,1) DEFAULT '0.0',
	`altura_padrao_cp` float(4,1) DEFAULT '0.0',
	
	PRIMARY KEY(`cod`, `id_segmentacao`)
);