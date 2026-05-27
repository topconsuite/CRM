/* OP-2212 - Livia - 04/03/2024 */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_nf_complemento' AND column_name='adic_zmrc'
) > 0, 'SELECT 1;', 'ALTER TABLE con_nf_complemento ADD COLUMN `adic_zmrc` float(7,2) unsigned NOT NULL DEFAULT "0.00";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_nf_complemento' AND column_name='pct_adic_zmrc'
) > 0, 'SELECT 1;', 'ALTER TABLE con_nf_complemento ADD COLUMN `pct_adic_zmrc` float(5,2) unsigned NOT NULL DEFAULT "0.00";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

INSERT IGNORE INTO `topsys`.`ger_diverso` SET
`aplic`='topcon',`prog`=6022,`campo`='taxa_adicional',`cod`='ZMRC',`descr`=N'ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO';

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='necessita_aprov_zmrc'
) > 0, 'SELECT 1;', 'ALTER TABLE con_obras ADD COLUMN `necessita_aprov_zmrc`  CHAR(1) NOT NULL DEFAULT "N";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='necessita_aprov_zmrc'
) > 0, 'SELECT 1;', 'ALTER TABLE con_obras_versao ADD COLUMN `necessita_aprov_zmrc`  CHAR(1) NOT NULL DEFAULT "N";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
