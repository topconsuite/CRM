/*OP-4512/OP-4663 Livia*/
REPLACE INTO `topsys`.`ger_parametro` (`grupo`, `chave`, `valor`) SELECT 'TopCon', 'RelContratoExpress', valor from `topsys`.`ger_parametro` WHERE grupo='Topcon' AND chave = 'RelContrato';

/*OP-4513/OP-4658 Livia*/
REPLACE INTO `topsys`.`ger_parametro` (`grupo`, `chave`, `valor`) SELECT 'TopCon', 'RelContratoArgamassa', valor from `topsys`.`ger_parametro` WHERE grupo='Topcon' AND chave = 'RelContrato';

