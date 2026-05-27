ALTER TABLE `topsys`.`ger_interv` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '' ,
CHANGE COLUMN `email_cobranca` `email_cobranca` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`ger_local` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '' ,
CHANGE COLUMN `fat_email` `fat_email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_versao` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '' ,
CHANGE COLUMN `fat_email` `fat_email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_obras` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '' ,
CHANGE COLUMN `email_resp_tecnico` `email_resp_tecnico` VARCHAR(5000) NULL DEFAULT '';

ALTER TABLE `topsys`.`con_obras_versao` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '' ,
CHANGE COLUMN `email_resp_tecnico` `email_resp_tecnico` VARCHAR(5000) NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_resp_solid` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_resp_solid_versao` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_cobranca` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_cobranca_versao` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_faturamento` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_faturamento_versao` 
CHANGE COLUMN `email` `email` VARCHAR(5000) NOT NULL DEFAULT '';