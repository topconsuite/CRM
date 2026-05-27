/* Ticket 371669 - Lucas Matheus */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_aprovacao_comercial_usina' AND column_name='fluxo_aprovacao'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_aprovacao_comercial_usina ADD COLUMN fluxo_aprovacao TINYINT UNSIGNED NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;