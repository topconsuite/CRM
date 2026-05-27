SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_contrato_clausu' AND column_name='usina'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_clausu` ADD COLUMN `usina` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0 FIRST, DROP PRIMARY KEY, ADD PRIMARY KEY (`usina`, `clausula`);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

CREATE TABLE IF NOT EXISTS `topsys`.`con_modelo_contrato_usina` (
  `usina` MEDIUMINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `segmento` INT(11) UNSIGNED NOT NULL DEFAULT 0,
  `valor` CHAR(250) NOT NULL DEFAULT '',
  PRIMARY KEY (`usina`, `segmento`));