/* TC-3265 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='info_usina_prop'
) > 0, 'SELECT 1;', 'ALTER TABLE con_parametro ADD COLUMN `info_usina_prop` tinyint(1) unsigned NOT NULL DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;