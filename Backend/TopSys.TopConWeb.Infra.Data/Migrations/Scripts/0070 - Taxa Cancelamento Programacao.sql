/* OP-3644 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_taxa_extra' AND column_name='antecedencia'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_taxa_extra` ADD `antecedencia` char(12) NOT NULL DEFAULT "" AFTER `acima_de`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_taxa_extra' AND column_name='quantidade'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_taxa_extra` ADD `quantidade` smallint(4) NOT NULL DEFAULT "0" AFTER `antecedencia`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_taxa_extra' AND column_name='external_id'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_taxa_extra` ADD `external_id` char(100) NOT NULL DEFAULT "" AFTER `quantidade`;;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_taxa_extra_versao' AND column_name='antecedencia'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_taxa_extra_versao` ADD `antecedencia` char(12) NOT NULL DEFAULT "" AFTER `acima_de`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_taxa_extra_versao' AND column_name='quantidade'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_taxa_extra_versao` ADD `quantidade` smallint(4) NOT NULL DEFAULT "0" AFTER `antecedencia`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_taxa_extra_versao' AND column_name='external_id'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_taxa_extra_versao` ADD `external_id` char(100) NOT NULL DEFAULT "" AFTER `quantidade`;;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;