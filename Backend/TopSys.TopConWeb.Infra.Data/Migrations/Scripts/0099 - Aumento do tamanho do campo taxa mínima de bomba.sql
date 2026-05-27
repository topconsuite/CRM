ALTER TABLE `topsys`.`con_preco_bomba` 
CHANGE COLUMN `taxa_minima` `taxa_minima` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ,
CHANGE COLUMN `taxa_minima_des` `taxa_minima_des` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ,
CHANGE COLUMN `vlr_tx_min_h_tab` `vlr_tx_min_h_tab` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ,
CHANGE COLUMN `vlr_tx_hora_min` `vlr_tx_hora_min` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ;

ALTER TABLE `topsys`.`con_prop_bomba` 
CHANGE COLUMN `taxa_minima_tab` `taxa_minima_tab` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ,
CHANGE COLUMN `tx_min_hora_tab` `tx_min_hora_tab` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ;

ALTER TABLE `topsys`.`con_prop_bomba_versao` 
CHANGE COLUMN `taxa_minima_tab` `taxa_minima_tab` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ,
CHANGE COLUMN `tx_min_hora_tab` `tx_min_hora_tab` FLOAT(7,2) UNSIGNED NOT NULL DEFAULT '0.00' ;