/* -- OP-4503 - Lucas Matheus */


CREATE TABLE IF NOT EXISTS topsys.con_aprovacao_comercial_pendente_volume (

	id							CHAR(36) NOT NULL,
	id_aprovacao			CHAR(36) NOT NULL,
	
	obra_versao				INT       UNSIGNED NOT NULL,
	obra_usina				SMALLINT  UNSIGNED NOT NULL,
	obra_numero				MEDIUMINT UNSIGNED NOT NULL,
	
	nivel_hierarquia		SMALLINT UNSIGNED NOT NULL,
	
	aprovacao_status		TINYINT(1) NOT NULL DEFAULT '0',
	aprovacao_data			DATETIME NULL,	
	aprovacao_usuario		CHAR(10) NOT NULL DEFAULT '',
	aprovacao_sequencia	SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	
	PRIMARY KEY(id),
	CONSTRAINT aprov_com_pendente_volume
		 FOREIGN KEY (id_aprovacao) 
		 REFERENCES topsys.con_aprovacao_comercial_pendente (id)
		 
);

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='volume_status_comercial'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras ADD COLUMN volume_status_comercial TINYINT NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='volume_status_comercial'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras_versao ADD COLUMN volume_status_comercial TINYINT NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;