ALTER TABLE `topsys`.`ger_interv` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '' ,
CHANGE COLUMN `email_cobranca` `email_cobranca` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`ger_local` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '' ,
CHANGE COLUMN `fat_email` `fat_email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_versao` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '' ,
CHANGE COLUMN `fat_email` `fat_email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_obras` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '' ,
CHANGE COLUMN `email_resp_tecnico` `email_resp_tecnico` TEXT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_obras_versao` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '' ,
CHANGE COLUMN `email_resp_tecnico` `email_resp_tecnico` TEXT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_resp_solid` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_resp_solid_versao` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_cobranca` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_cobranca_versao` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_faturamento` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';

ALTER TABLE `topsys`.`con_chtel_faturamento_versao` 
CHANGE COLUMN `email` `email` TEXT NOT NULL DEFAULT '';