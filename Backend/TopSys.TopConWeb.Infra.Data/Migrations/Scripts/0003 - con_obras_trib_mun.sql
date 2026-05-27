SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_trib_mun' AND column_name='no_obra'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_obras_trib_mun DROP PRIMARY KEY, ADD COLUMN no_obra mediumint(6) unsigned not null default 0 AFTER usina_contrato, ADD PRIMARY KEY(usina_contrato,no_obra,num_contrato,ano_contrato,usina);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

UPDATE con_obras_trib_mun tm
INNER JOIN con_obras o ON tm.usina_contrato=o.usina AND tm.ano_contrato=o.ano_contrato AND tm.num_contrato=o.no_contrato
SET tm.no_obra=o.numero
WHERE tm.no_obra=0;