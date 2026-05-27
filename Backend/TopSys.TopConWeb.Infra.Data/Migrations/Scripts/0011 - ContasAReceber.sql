/* DA-100 */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='fin_car' AND column_name='alocado'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.fin_car ADD COLUMN alocado tinyint NOT NULL DEFAULT 1;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_cartao_transacao' AND column_name='motivo_erro'
) > 0, 'SELECT 1;', 'ALTER TABLE con_cartao_transacao ADD COLUMN motivo_erro varchar(200) NOT NULL DEFAULT \'\';'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_cartao_transacao' AND column_name='status_processo'
) > 0, 'SELECT 1;', 'ALTER TABLE con_cartao_transacao ADD COLUMN status_processo tinyint NOT NULL DEFAULT 0;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS
WHERE table_schema='topsys' AND table_name='con_chtel' AND index_name='IDX_CNPJCPF'
) > 0, 'SELECT 1;', 'ALTER TABLE con_chtel ADD INDEX `IDX_CNPJCPF` (`cnpj_cpf` ASC);'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;


