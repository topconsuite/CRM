/* TC-3220 */
CREATE OR REPLACE VIEW topsys.view_cartao_bandeira AS
SELECT b.*, g.descr, g.descr_reduzida
FROM topsys.ger_geral g
INNER JOIN topsys.con_bandeira b ON g.cod=b.cod_bandeira;