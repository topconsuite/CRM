/* DA-84 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_bandeira' AND column_name='tipo_integracao'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_bandeira ADD COLUMN tipo_integracao CHAR(20) NOT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

CREATE OR REPLACE VIEW topsys.view_cartao_bandeira AS
SELECT b.*, g.descr, g.descr_reduzida
FROM topsys.ger_geral g
INNER JOIN topsys.con_bandeira b ON g.cod=b.cod_bandeira;

CREATE TABLE IF NOT EXISTS topsys.`con_bandeira_integracoes` (
   `cod_bandeira` smallint(4) unsigned NOT NULL DEFAULT '0',
   `parametro_nome` char(40) NOT NULL DEFAULT '',
   `parametro_valor` char(255) NOT NULL DEFAULT '',
   PRIMARY KEY (`cod_bandeira`,`parametro_nome`)
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 CREATE TABLE IF NOT EXISTS topsys.`con_cartao_transacao` (
   `id` bigint not null auto_increment,
   `origem` CHAR(20) NOT NULL DEFAULT '',
   `id_transacao` char(36) NOT NULL DEFAULT '',
   `id_pedido` char(36) NOT NULL DEFAULT '',
   `status` char(45) NOT NULL DEFAULT '',
   `cod_estabelecimento` char(45) NOT NULL DEFAULT '',
   `id_merchant` char(36) NOT NULL DEFAULT '',
   `num_terminal` char(20) NOT NULL DEFAULT '',
   `num_autorizacao` char(20) NOT NULL DEFAULT '',
   `data_hora_transacao` datetime NOT NULL,
   `valor_transacao` double(10,2) NOT NULL DEFAULT 0.0,
   `tipo_transacao` char(45) NOT NULL DEFAULT '',
   `nome_produto` char(70) NOT NULL DEFAULT '',
   `nome_sub_produto` char(70) NOT NULL DEFAULT '',
   `qtde_parcelas` smallint(4) NOT NULL DEFAULT 0,
   `nome_bandeira` char(70) NOT NULL DEFAULT '',
   `num_cartao` char(20) NOT NULL DEFAULT '',
   PRIMARY KEY (`id`),
   UNIQUE KEY (`origem`,`id_transacao`,`id_pedido`)
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1;

  CREATE TABLE IF NOT EXISTS topsys.con_bandeira_integracoes_envios (
	`id` bigint not null auto_increment,
	`cod_bandeira` smallint(4) unsigned NOT NULL DEFAULT 0,
	`id_integracao` char(36) NOT NULL DEFAULT '',
	`valor_total` double(10, 2) NOT NULL DEFAULT 0.0,
    `cpf_cnpj` char(14) NOT NULL DEFAULT '',
    `nome_cliente` char(100) NOT NULL DEFAULT '',
    `tipo_cobranca` char(2) NOT NULL DEFAULT '',
    `qtde_parcelas` int(3) NOT NULL DEFAULT 0,
    `obra_usina` smallint(3) NOT NULL DEFAULT 0,
    `obra_numero` mediumint(6) NOT NULL DEFAULT 0,
    `observacao` char(255) NOT NULL DEFAULT '',
    `solicitante` char(10) NOT NULL DEFAULT '',
    PRIMARY KEY (`id`)
);