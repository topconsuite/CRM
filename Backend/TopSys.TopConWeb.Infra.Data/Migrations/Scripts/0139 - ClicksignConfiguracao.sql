/* TC - Múltiplas Contas ClickSign */

-- 1. Criação da tabela configuracoes_clicksign
SET @preparedStatement = (SELECT IF(
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES
     WHERE table_schema = 'topsys' AND table_name = 'configuracoes_clicksign') > 0,
    'SELECT 1;',
    'CREATE TABLE `topsys`.`configuracoes_clicksign` (
        `id`           INT NOT NULL AUTO_INCREMENT,
        `email`        VARCHAR(200) NOT NULL DEFAULT \'\',
        `token`        VARCHAR(200) NOT NULL DEFAULT \'\',
        `base_url`     VARCHAR(200) NOT NULL DEFAULT \'\',
        `alias`        VARCHAR(100) NOT NULL DEFAULT \'\',
        `sha256_secret` VARCHAR(200) NOT NULL DEFAULT \'\',
        `active`       TINYINT(1)   NOT NULL DEFAULT 1,
        PRIMARY KEY (`id`)
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8;'
));
PREPARE createIfNotExists FROM @preparedStatement;
EXECUTE createIfNotExists;
DEALLOCATE PREPARE createIfNotExists;

-- 2. Adição da FK clicksign_config_id na tabela con_usina
SET @preparedStatement = (SELECT IF(
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
     WHERE table_schema = 'topsys'
       AND table_name   = 'con_usina'
       AND column_name  = 'clicksign_config_id') > 0,
    'SELECT 1;',
    'ALTER TABLE `topsys`.`con_usina`
     ADD COLUMN `clicksign_config_id` INT NULL DEFAULT NULL;'
));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;
