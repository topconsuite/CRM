/* TC-2974 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_trib_mun' AND column_name='ret_iss'
) > 0, 'SELECT 1;', 'ALTER TABLE con_obras_trib_mun ADD COLUMN `ret_iss` char(1) NOT NULL DEFAULT ''X'';'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;