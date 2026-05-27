INSERT IGNORE topsys.ger_parametro (grupo, chave, valor) VALUES ('web', 'BloqueiaCampoTaxaMinimaNoPrecoDeBomba', '0');

UPDATE topsys.con_aprovacao_comercial_hierarquia_condicao SET tipo_valor = 'VendaTracos' WHERE tipo_valor = 'Venda';

INSERT IGNORE topsys.con_aprovacao_comercial_hierarquia_condicao (id, aprovacao_comercial_hierarquia_id, valor_de, valor_ate, percentual_de, percentual_ate, tipo_pessoa_id, tipo_valor)
SELECT UUID(), aprovacao_comercial_hierarquia_id, valor_de, valor_ate, percentual_de, percentual_ate, tipo_pessoa_id, 'VendaBomba'
FROM topsys.con_aprovacao_comercial_hierarquia_condicao
WHERE tipo_valor = 'VendaTracos';