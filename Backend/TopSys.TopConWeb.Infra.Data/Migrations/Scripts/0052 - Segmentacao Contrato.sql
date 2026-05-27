CREATE TABLE IF NOT EXISTS topsys.con_segmentacao (

	id INT NOT NULL, 
	nome VARCHAR(50) NOT NULL DEFAULT '',
	nome_abreviado CHAR(3) NOT NULL DEFAULT '',
	external_id CHAR(100) DEFAULT '',

	PRIMARY KEY(id)

);

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato' AND column_name='segmentacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato` ADD COLUMN `segmentacao` INT DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_versao' AND column_name='segmentacao'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_versao` ADD COLUMN `segmentacao` INT DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;


	
