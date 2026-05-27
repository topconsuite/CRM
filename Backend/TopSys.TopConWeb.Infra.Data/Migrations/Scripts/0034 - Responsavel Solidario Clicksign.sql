/* TC-4099 */

ALTER TABLE `topsys`.`con_integracao_clicksign` 
ADD COLUMN `envia_assinatura_responsavel_solidario` TINYINT(1) NOT NULL DEFAULT '0' AFTER `envia_assinatura_contratada`;