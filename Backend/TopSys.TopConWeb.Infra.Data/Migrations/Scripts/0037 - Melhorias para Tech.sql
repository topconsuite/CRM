/* TC-4128 */
CREATE TABLE IF NOT EXISTS `topsys`.`con_tab_preco_pre` (
`id` Char(36) NOT NULL DEFAULT '',
`usina` smallint(3) unsigned NOT NULL DEFAULT '0',
`uso` smallint(4) unsigned NOT NULL DEFAULT '0',
`tp_resist` smallint(3) unsigned NOT NULL DEFAULT '0',
`fck` float(3,1) unsigned NOT NULL DEFAULT '0.0',
`consumo` smallint(3) unsigned NOT NULL DEFAULT '0',
`pedra` tinyint(2) unsigned NOT NULL DEFAULT '0',
`slump` smallint(3) unsigned NOT NULL DEFAULT '0',
`custo_material` float(5,2) unsigned NOT NULL DEFAULT '0.00',
`valor_servico` float(5,2) unsigned NOT NULL DEFAULT '0.00',
`markup` float(4,2) unsigned NOT NULL DEFAULT '0.00',
`preco_m3` float(6,2) unsigned NOT NULL DEFAULT '0.00',
`id_ciencia` char(19) NOT NULL DEFAULT '',
`data_ciencia`datetime NOT NULL DEFAULT '2000-01-01 00:00:00',
`espec_familia` char(130) NOT NULL DEFAULT '',
`external_id` char(72) NOT NULL DEFAULT '',
`CREATED_AT` datetime DEFAULT NULL,
`UPDATED_AT` datetime DEFAULT NULL,
PRIMARY KEY (`id`),
UNIQUE KEY (`usina`,`uso`,`tp_resist`,`fck`,`consumo`,`pedra`,`slump`, `data_ciencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_tab_preco' AND column_name='numeracao_produto'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_tab_preco ADD COLUMN numeracao_produto INT UNSIGNED;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_tab_preco_pre' AND column_name='numeracao_produto'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_tab_preco_pre ADD COLUMN numeracao_produto INT UNSIGNED;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
