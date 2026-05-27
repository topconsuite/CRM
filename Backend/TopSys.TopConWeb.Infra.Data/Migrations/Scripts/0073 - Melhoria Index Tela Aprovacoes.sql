 /* OP-4953 / TKT-438766 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.statistics 
	WHERE table_schema='topsys' AND table_name='con_aprovacao_comercial_pendente' and index_name = 'IDX_OBRA'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_aprovacao_comercial_pendente` ADD INDEX `IDX_OBRA` (`aprovacao_status` ASC, `obra_usina` ASC, `obra_numero` ASC, `obra_versao` ASC);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.statistics 
	WHERE table_schema='topsys' AND table_name='con_programacao' and index_name = 'IDX_CRM_APROV_PROG'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_programacao` ADD INDEX `IDX_CRM_APROV_PROG` (`status` ASC, `dt_concretagem` ASC, `usina_entrega` ASC);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.statistics 
	WHERE table_schema='topsys' AND table_name='con_contrato_versao' and index_name = 'IDX_CRM_APROV_CV'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_contrato_versao` ADD INDEX `IDX_CRM_APROV_CV` (`usina` ASC, `ano_contrato` ASC, `num_contrato` ASC, `status` ASC);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
