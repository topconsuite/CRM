/* OP-4317 - Lucas Matheus - 17/01/2024 */

CREATE TABLE IF NOT EXISTS `topsys`.`con_obras_frente` (

	`id` CHAR(36) NOT NULL,
	
	`obra_usina` SMALLINT(5) UNSIGNED NOT NULL,
	`obra_numero` MEDIUMINT(8) UNSIGNED NOT NULL,
	`obra_sequencia` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
	
	`obra_nome` CHAR(30) NOT NULL DEFAULT '',
	
	`obra_cep` CHAR(8) NOT NULL DEFAULT '',
	`obra_end` CHAR(40) NOT NULL DEFAULT '',
	`obra_num` MEDIUMINT(6) UNSIGNED NOT NULL DEFAULT '0',
	`obra_compl` CHAR(30) NOT NULL DEFAULT '',
	`obra_bairro` CHAR(20) NOT NULL DEFAULT '',
	
	PRIMARY KEY(id),
	UNIQUE KEY `obra_frente_key` (`obra_usina`,`obra_numero`,`obra_sequencia`),
	KEY `obra_frente_obra` (`obra_usina`,`obra_numero`)
	
);

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_programacao' AND column_name='obra_frente_sequencia'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_programacao` ADD COLUMN `obra_frente_sequencia` TINYINT NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;