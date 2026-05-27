/* OP-4337 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='primeira_testemunha'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `primeira_testemunha` tinyint(4) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='segunda_testemunha'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `segunda_testemunha` tinyint(4) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;