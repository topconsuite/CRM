REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `Funcao`, `menu`, `seq_menu`) 
VALUES 
	('WEB', 7009, 'Acesso por Grupo Agenda', 'IAE', 10, 1), 
	('WEB', 7010, 'Acesso Geral Agenda', 'IAE', 10, 1);
	

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_tarefa' AND column_name='id_agrupamento'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_tarefa ADD COLUMN id_agrupamento VARCHAR(36) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SELECT COUNT(1)
INTO @index_exists
FROM INFORMATION_SCHEMA.STATISTICS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME   = 'con_tarefa'
  AND INDEX_NAME   = 'AGRUPAMENTO_TAREFA';
SET @sql_statement = IF(
    @index_exists = 0,
    'ALTER TABLE topsys.con_tarefa ADD INDEX `AGRUPAMENTO_TAREFA` (`id_agrupamento`);',
    'SELECT 1;'
);
PREPARE stmt FROM @sql_statement;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_compromisso' AND column_name='id_agrupamento'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_compromisso ADD COLUMN id_agrupamento VARCHAR(36) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SELECT COUNT(1)
INTO @index_exists
FROM INFORMATION_SCHEMA.STATISTICS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME   = 'con_compromisso'
  AND INDEX_NAME   = 'AGRUPAMENTO_COMPROMISSO';
SET @sql_statement = IF(
    @index_exists = 0,
    'ALTER TABLE topsys.con_compromisso ADD INDEX `AGRUPAMENTO_COMPROMISSO` (`id_agrupamento`);',
    'SELECT 1;'
);
PREPARE stmt FROM @sql_statement;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;