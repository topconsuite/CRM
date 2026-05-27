/*CREATE TABLE IF NOT EXISTS `topsys`.`con_aprovacao_comercial_hierarquia_condicao_pagamento` (

	`id` CHAR(36) NOT NULL,
	`aprovacao_comercial_hierarquia_id` CHAR(36) NOT NULL,
	`tipo_pessoa_id` CHAR(36) NOT NULL,
	`condicao_pagamento_id` SMALLINT(3) UNSIGNED NOT NULL,
	`necessita_aprovacao` TINYINT NOT NULL DEFAULT '0',
	
	PRIMARY KEY(id),
	UNIQUE INDEX demais_cond_consulta(aprovacao_comercial_hierarquia_id, tipo_pessoa_id, condicao_pagamento_id),
	
	FOREIGN KEY (`aprovacao_comercial_hierarquia_id`) 
		REFERENCES `con_aprovacao_comercial_hierarquia` (`id`) 
		ON UPDATE RESTRICT ON DELETE RESTRICT,
		
	FOREIGN KEY (`tipo_pessoa_id`) 
		REFERENCES `con_aprovacao_comercial_tipo_pessoa` (`id`) 
		ON UPDATE RESTRICT ON DELETE RESTRICT
		
); REMOVIDO NA TASK OP-5335 */

CREATE TABLE IF NOT EXISTS `topsys`.`con_aprovacao_comercial_pendente_cond_pagto` (

	`id` CHAR(36) NOT NULL,
	`id_aprovacao` CHAR(36) NOT NULL,
	
	`obra_versao` INT(10) UNSIGNED NOT NULL,
	`obra_usina` SMALLINT(5) UNSIGNED NOT NULL,
	`obra_numero` MEDIUMINT(8) UNSIGNED NOT NULL,
	
	`nivel_hierarquia` SMALLINT(5) UNSIGNED NOT NULL,
	
	`aprovacao_status` TINYINT(1) NOT NULL DEFAULT '0',
	`aprovacao_data` DATETIME NULL DEFAULT NULL,
	`aprovacao_usuario` CHAR(10) NOT NULL DEFAULT '',
	`aprovacao_sequencia` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
	
	PRIMARY KEY (`id`),
	INDEX (`id_aprovacao`),
	FOREIGN KEY (`id_aprovacao`) 
		REFERENCES `con_aprovacao_comercial_pendente` (`id`)
);

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_usina' AND column_name='show_demais_cond_pagamento'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_usina ADD COLUMN show_demais_cond_pagamento CHAR(1) NOT NULL DEFAULT "N";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;


SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='condicao_pagamento_status_comercial'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras ADD COLUMN condicao_pagamento_status_comercial TINYINT NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='condicao_pagamento_status_comercial'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras_versao ADD COLUMN condicao_pagamento_status_comercial TINYINT NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;