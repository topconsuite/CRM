SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_parametro' AND column_name='obs_prop_web'
) > 0, 'SELECT 1;', 'ALTER TABLE `topsys`.`con_parametro` ADD COLUMN `obs_prop_web` VARCHAR(1000) NOT NULL DEFAULT "CONFERIR OS DADOS, ASSINAR, CARIMBAR E DEVOLVER VIA E-MAIL";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;