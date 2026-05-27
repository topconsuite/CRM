/* OP-4456 */
CREATE TABLE IF NOT EXISTS `topsys`.`ger_grupo_economico` (
    codigo INT AUTO_INCREMENT PRIMARY KEY,
    descricao VARCHAR(60) NOT NULL UNIQUE,
	id_cadast char(19) NOT NULL DEFAULT '',
	id_atual char(19) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='ger_interv' AND column_name='grupo_economico'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`ger_interv` ADD COLUMN grupo_economico INT;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;	

INSERT IGNORE INTO `topsys`.`usr_programa` SET
`Aplicativo`='WEB',`num`=6007,`Titulo`='Cadastro Grupo Econômico',`Funcao`='IAER',`menu`=1,`seq_menu`=7;