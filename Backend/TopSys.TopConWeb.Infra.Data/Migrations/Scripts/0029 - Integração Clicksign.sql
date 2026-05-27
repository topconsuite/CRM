 /* TC-2304 */
CREATE TABLE IF NOT EXISTS `con_integracao_clicksign` (
  `id` char(36) NOT NULL,
  `corpo_email` text,
  `obriga_documento_oficial` tinyint(1) NOT NULL DEFAULT '0',
  `obriga_selfie` tinyint(1) NOT NULL DEFAULT '0',
  `obriga_assinatura_manuscrita` tinyint(1) NOT NULL DEFAULT '0',
  `obriga_reconhecimento_facial` tinyint(1) NOT NULL DEFAULT '0',
  `notifica_cliente_confirmacao_assinatura` tinyint(1) NOT NULL DEFAULT '1',
  `id_atual` char(19) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `con_clicksign_envios` (	
`id` char(36) NOT NULL,
`usina` smallint(3) unsigned NOT NULL DEFAULT '0',
`ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
`num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
`id_clicksign` char(36) NOT NULL,
`data_envio` datetime DEFAULT NULL,
`id_envio` char(19) NOT NULL DEFAULT '',
`data_cancelamento` datetime DEFAULT NULL,
`id_cancelamento` char(19) NOT NULL DEFAULT '',
`data_assinatura` datetime DEFAULT NULL,
 PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;