SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_boleto' AND column_name='ano_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_boleto ADD COLUMN `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_boleto' AND column_name='num_chamada'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_boleto ADD COLUMN `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_boleto' AND column_name='no_obra'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_boleto ADD COLUMN `no_obra` mediumint(6) unsigned NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_boleto' AND column_name='seq'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_boleto ADD COLUMN `seq` tinyint(2) unsigned NOT NULL DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_boleto' AND column_name='id_cadast'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_boleto ADD COLUMN `id_cadast` char(19) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_boleto' AND column_name='id_atual'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_contrato_boleto ADD COLUMN `id_atual` char(19) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;

ALTER TABLE topsys.con_contrato_boleto
DROP PRIMARY KEY,
ADD PRIMARY KEY (`usina`,`ano_contrato`,`num_contrato`,`seq_pgto`,`seq`,`ano_chamada`,`num_chamada`,`no_obra`);

UPDATE topsys.con_contrato_boleto c
INNER JOIN topsys.con_obras o
ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
SET c.ano_chamada=o.ano_chamada, c.num_chamada=o.no_chamada, c.no_obra=o.numero;

UPDATE topsys.con_contrato_dinheir c
INNER JOIN topsys.con_obras o
ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
SET c.ano_chamada=o.ano_chamada, c.num_chamada=o.no_chamada, c.no_obra=o.numero;

UPDATE topsys.con_contrato_cheque c
INNER JOIN topsys.con_obras o
ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
SET c.ano_chamada=o.ano_chamada, c.num_chamada=o.no_chamada, c.no_obra=o.numero;

CREATE OR REPLACE VIEW topsys.view_con_chtel_boleto AS
SELECT c.*
FROM topsys.con_contrato_boleto c;