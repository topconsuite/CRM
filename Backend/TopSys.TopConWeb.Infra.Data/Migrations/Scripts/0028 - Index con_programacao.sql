 /* TC-3434 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.statistics 
	WHERE table_schema='topsys' AND table_name='con_programacao' and index_name = 'IDX_OBRA_PROG'
) > 0, 'SELECT 1;', 'ALTER TABLE con_programacao ADD KEY `IDX_OBRA_PROG` (`usina`,`no_obra`,`seq_prog`);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;