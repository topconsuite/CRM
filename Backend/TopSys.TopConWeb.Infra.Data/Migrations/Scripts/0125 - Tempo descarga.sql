SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras' AND column_name='tempo_descarga'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras` ADD COLUMN `tempo_descarga` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `condicao_pagamento_status_comercial`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_obras_versao' AND column_name='tempo_descarga'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_obras_versao` ADD COLUMN `tempo_descarga` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `condicao_pagamento_status_comercial`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_programacao' AND column_name='temp_ate_a_obra'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_programacao` ADD COLUMN `temp_ate_a_obra` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `external_id`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_programacao' AND column_name='temp_bt_na_obra'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_programacao` ADD COLUMN `temp_bt_na_obra` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `temp_ate_a_obra`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_programacao' AND column_name='tempo_descarga'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_programacao` ADD COLUMN `tempo_descarga` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `temp_bt_na_obra`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

UPDATE con_programacao prog
INNER JOIN con_obras obra
on obra.usina=prog.usina
AND obra.no_contrato=prog.no_contrato
AND obra.ano_contrato=prog.ano_contrato
SET prog.temp_ate_a_obra=obra.temp_ate_a_obra,
prog.temp_bt_na_obra=obra.temp_bt_na_obra
WHERE prog.temp_ate_a_obra=0 AND prog.temp_bt_na_obra=0;