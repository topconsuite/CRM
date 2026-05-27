SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_cheque' AND column_name='ano_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_cheque ADD COLUMN `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_cheque' AND column_name='num_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_cheque ADD COLUMN `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_cheque' AND column_name='no_obra'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_cheque ADD COLUMN `no_obra` mediumint(6) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

ALTER TABLE topsys.con_contrato_cheque
DROP PRIMARY KEY,
ADD PRIMARY KEY (`usina`,`ano_contrato`,`num_contrato`,`seq_pgto`,`seq`,`ano_chamada`,`num_chamada`,`no_obra`);

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_dinheir' AND column_name='ano_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_dinheir ADD COLUMN `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_dinheir' AND column_name='num_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_dinheir ADD COLUMN `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_dinheir' AND column_name='no_obra'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_dinheir ADD COLUMN `no_obra` mediumint(6) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

ALTER TABLE topsys.con_contrato_dinheir
DROP PRIMARY KEY,
ADD PRIMARY KEY (`usina`,`ano_contrato`,`num_contrato`,`seq_pgto`,`seq`,`ano_chamada`,`num_chamada`,`no_obra`);

UPDATE topsys.con_contrato_dinheir c
INNER JOIN topsys.con_obras o
ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
SET c.ano_chamada=o.ano_chamada, c.num_chamada=o.no_chamada;

UPDATE topsys.con_contrato_cheque c
INNER JOIN topsys.con_obras o
ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
SET c.ano_chamada=o.ano_chamada, c.num_chamada=o.no_chamada;

CREATE OR REPLACE VIEW topsys.view_con_chtel_dinheir AS
SELECT c.*
FROM topsys.con_contrato_dinheir c;

CREATE OR REPLACE VIEW topsys.view_con_chtel_cheque AS
SELECT c.*
FROM topsys.con_contrato_cheque c;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS
WHERE table_schema='topsys' AND table_name='con_contrato_pag' AND index_name='IDX_CONTRATO'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_pag ADD INDEX `IDX_CONTRATO` (`usina`, `ano_contrato`, `num_contrato`);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;