CREATE TABLE IF NOT EXISTS topsys.ger_geral_captacao  (

    `cod` SMALLINT(4) UNSIGNED NOT NULL DEFAULT '0',
    `ativo` CHAR(1) NOT NULL DEFAULT 'S',
    `captacao_tipo_indicador` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
    PRIMARY KEY (cod)
    
);

CREATE TABLE IF NOT EXISTS topsys.con_obras_indicador (

	`obra_usina` SMALLINT(3) UNSIGNED NOT NULL,
	`obra_numero` MEDIUMINT(6) UNSIGNED NOT NULL,
	`interveniente` MEDIUMINT(6) UNSIGNED,
	`vendedor` SMALLINT(3) UNSIGNED,
    PRIMARY KEY (obra_usina, obra_numero)
    
);

CREATE TABLE IF NOT EXISTS topsys.con_obras_indicador_versao (

	`obra_usina` SMALLINT(3) UNSIGNED NOT NULL,
	`obra_numero` MEDIUMINT(6) UNSIGNED NOT NULL,
	`obra_versao` INT(11) NOT NULL,
	`interveniente` MEDIUMINT(6) UNSIGNED,
	`vendedor` SMALLINT(3) UNSIGNED,
    PRIMARY KEY (obra_usina, obra_numero, obra_versao)
    
);