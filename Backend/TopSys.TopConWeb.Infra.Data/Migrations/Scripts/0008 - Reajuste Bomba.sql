/* DA-29 */
INSERT IGNORE INTO `topsys`.`ger_parametro` (`grupo`, `chave`, `valor`) VALUES ('web', 'NaoConsideraTodasBombas', '0');

CREATE OR REPLACE VIEW topsys.view_prog_reaj_bomba_vigen AS
SELECT r.usina, r.ano_contrato, r.num_contrato, p.seq_prog, r.seq, r.tipo_bomba, MAX(r.dt_vigencia) dt_vigencia
FROM con_programacao p
INNER JOIN con_prop_bomba b ON p.usina=b.usina AND p.no_obra=b.no_obra AND p.tipo_bomba=b.seq
INNER JOIN con_reaj_bomba r ON p.usina=r.usina AND p.ano_contrato=r.ano_contrato AND p.no_contrato=r.num_contrato AND b.seq=r.seq AND b.tipo_bomba=r.tipo_bomba
WHERE p.dt_concretagem>=r.dt_vigencia
GROUP BY r.usina, r.ano_contrato, r.num_contrato, p.seq_prog, r.seq, r.tipo_bomba;