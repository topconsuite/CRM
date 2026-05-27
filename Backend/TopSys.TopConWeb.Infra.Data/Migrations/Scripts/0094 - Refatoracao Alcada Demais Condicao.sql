SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_cond_pag' AND column_name='media_dias'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_cond_pag` ADD COLUMN `media_dias` FLOAT UNSIGNED NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

/* --- VERIFICA SE TABELA FOI CRIADA NO MODELO ANTIGO, CASO SIM, ELE DROPA A TABELA PARA CRIAR NOVAMENTE --- */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_aprovacao_comercial_hierarquia_condicao_pagamento' AND column_name='media_dias_de'
) > 0, 'SELECT 1;', 'DROP TABLE IF EXISTS `topsys`.`con_aprovacao_comercial_hierarquia_condicao_pagamento`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

CREATE TABLE IF NOT EXISTS `topsys`.`con_aprovacao_comercial_hierarquia_condicao_pagamento` (
	`id` CHAR(36) NOT NULL,

	`aprovacao_comercial_hierarquia_id` CHAR(36) NOT NULL,
	`tipo_pessoa_id` CHAR(36) NOT NULL,
	`segmentacao_id` INT(11) UNSIGNED NOT NULL DEFAULT '1',

	`media_dias_de` FLOAT UNSIGNED NOT NULL DEFAULT '0',
	`media_dias_ate` FLOAT UNSIGNED NOT NULL DEFAULT '0',

	PRIMARY KEY (`id`),
	UNIQUE INDEX `demais_cond_consulta` (`aprovacao_comercial_hierarquia_id`, `tipo_pessoa_id`, `segmentacao_id`),

	FOREIGN KEY (`aprovacao_comercial_hierarquia_id`) REFERENCES `con_aprovacao_comercial_hierarquia` (`id`),
	FOREIGN KEY (`tipo_pessoa_id`) REFERENCES `con_aprovacao_comercial_tipo_pessoa` (`id`)

);