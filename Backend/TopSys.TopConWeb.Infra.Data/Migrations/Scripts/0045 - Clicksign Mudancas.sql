SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='envia_assinatura_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `prazo_limite_assinatura` tinyint(2) unsigned NOT NULL DEFAULT "30";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='envia_assinatura_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `envia_assinatura_contratada` TINYINT(1) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='envia_assinatura_responsavel_solidario'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `envia_assinatura_responsavel_solidario` TINYINT(1) NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='email_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `email_contratada` TEXT NULL DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='ddd_wpp_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `ddd_wpp_contratada` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

 /* OP-3983 */
SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='ddd_wpp_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `ddd_wpp_contratada` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='telefone_wpp_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `telefone_wpp_contratada` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_resp_solid' AND column_name='ddd'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_resp_solid` ADD COLUMN `ddd` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_resp_solid' AND column_name='telefone'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_resp_solid` ADD COLUMN `telefone` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_resp_solid_versao' AND column_name='ddd'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_resp_solid_versao` ADD COLUMN `ddd` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_chtel_resp_solid_versao' AND column_name='telefone'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_chtel_resp_solid_versao` ADD COLUMN `telefone` INT DEFAULT NULL;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='metodo_envio_assinatura_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `metodo_envio_assinatura_contratada` TINYINT NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='metodo_autenticacao_contratada'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `metodo_autenticacao_contratada` TINYINT NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_integracao_clicksign' AND column_name='permite_assinatura_whatsapp'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_integracao_clicksign` ADD COLUMN `permite_assinatura_whatsapp` TINYINT DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_clicksign_envios' AND column_name='qtd_assinatura_whatsapp'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_clicksign_envios` ADD COLUMN `qtd_assinatura_whatsapp` TINYINT NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;


REPLACE INTO topsys.con_chtel_resp_solid
SELECT p.usina,p.ano_chamada,p.num_chamada
,IF(LENGTH(l.cnpj_cpf)=14,'J','F'),l.cnpj_cpf,l.razao,l.nome,l.email
,l.ie,l.ccm,l.rg,l.emissor
,l.cep,l.end,l.num,l.compl,l.bairro,l.mun, COALESCE(csr.ddd, '') AS ddd, COALESCE(csr.telefone, '') AS telefone
FROM con_chtel p
INNER JOIN con_obras o ON o.usina=p.usina AND o.ano_chamada=p.ano_chamada AND o.no_chamada=p.num_chamada
INNER JOIN con_contrato c ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
INNER JOIN ger_local l ON c.interv=l.interv AND c.resp_solidario=l.seq
LEFT JOIN topsys.con_chtel_resp_solid csr ON p.usina = csr.usina AND p.ano_chamada = csr.ano_chamada AND p.num_chamada = csr.num_chamada;