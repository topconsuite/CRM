/*TKT-451286 Livia 23/08/2024*/
ALTER TABLE `topsys`.`con_tab_preco_pre` 
CHANGE COLUMN `custo_material` `custo_material` FLOAT(6,2) UNSIGNED NOT NULL DEFAULT '0.00' ,
CHANGE COLUMN `valor_servico` `valor_servico` FLOAT(6,2) UNSIGNED NOT NULL DEFAULT '0.00' ;