SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
                                     WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='tribcontrib_nfs'
                                    ) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `tribcontrib_nfs` CHAR(4) NOT NULL DEFAULT '''';'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
                                     WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='tribcontrib_nfs'
                                    ) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras_versao` ADD COLUMN `tribcontrib_nfs` CHAR(4) NOT NULL DEFAULT '''';'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;