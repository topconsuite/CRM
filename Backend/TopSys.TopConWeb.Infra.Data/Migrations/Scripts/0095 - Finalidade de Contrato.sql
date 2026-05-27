SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel' AND column_name='finalidade_ctr'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel` ADD COLUMN `finalidade_ctr` INT(11) NULL DEFAULT "1";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_versao' AND column_name='finalidade_ctr'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_versao` ADD COLUMN `finalidade_ctr` INT(11) NULL DEFAULT "1";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato' AND column_name='finalidade_ctr'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato` ADD COLUMN `finalidade_ctr` INT(11) NULL DEFAULT "1";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_versao' AND column_name='finalidade_ctr'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_versao` ADD COLUMN `finalidade_ctr` INT(11) NULL DEFAULT "1";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

CREATE TABLE IF NOT EXISTS `topsys`.`con_finalidade_ctr` (
  `codigo` INT NOT NULL AUTO_INCREMENT,
  `descricao` VARCHAR(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`codigo`)
);
  
REPLACE INTO `topsys`.`con_finalidade_ctr` (`codigo`,`descricao`) VALUES (1, 'Prestação de Serviço');
REPLACE INTO `topsys`.`con_finalidade_ctr` (`codigo`,`descricao`) VALUES (2, 'Amostra Grátis');
REPLACE INTO `topsys`.`con_finalidade_ctr` (`codigo`,`descricao`) VALUES (3, 'Doação');
REPLACE INTO `topsys`.`con_finalidade_ctr` (`codigo`,`descricao`) VALUES (4, 'Reposição');
