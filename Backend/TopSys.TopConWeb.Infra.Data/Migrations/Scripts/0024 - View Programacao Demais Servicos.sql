/* TC-3057 */
CREATE OR REPLACE VIEW `topsys`.`view_prog_dem_serv` AS
    SELECT 
        `con_programacao_dem_serv`.`usina` AS `usina`,
        `con_programacao_dem_serv`.`obra` AS `obra`,
        `con_programacao_dem_serv`.`seq_prog` AS `seq_prog`,
        `con_programacao_dem_serv`.`seq` AS `seq`,
        `con_programacao_dem_serv`.`Quantidade` AS `Quantidade`,
        `con_programacao_dem_serv`.`valor_total` AS `valor_total`,
        `con_programacao_dem_serv`.`valor_cobrado` AS `valor_cobrado`
    FROM
        `con_programacao_dem_serv`;