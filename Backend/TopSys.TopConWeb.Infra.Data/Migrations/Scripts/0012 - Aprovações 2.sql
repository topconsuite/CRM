UPDATE con_obras o
LEFT JOIN view_obras_pendentes_aprov pa ON o.usina=pa.usina AND o.numero=pa.numero
SET o.status_comercial=0
WHERE o.status_comercial=1 AND ISNULL(pa.usina);

UPDATE view_obras_pendentes_aprov pa
INNER JOIN con_obras o ON o.usina=pa.usina AND o.numero=pa.numero
SET o.status_comercial=1
WHERE o.status_comercial<>1;

CREATE OR REPLACE VIEW view_pendentes_aprov_finan_wg AS
(SELECT o.usina, o.numero, cp.ano_contrato, cp.num_contrato
FROM con_contrato_pag cp
INNER JOIN con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato
INNER JOIN con_contrato c ON cp.usina=c.usina AND cp.ano_contrato=c.ano_contrato AND cp.num_contrato=c.num_contrato
INNER JOIN con_contrato_boleto fp ON fp.usina=cp.usina AND fp.num_contrato=cp.num_contrato AND fp.ano_contrato=cp.ano_contrato AND fp.seq_pgto=cp.seq
WHERE cp.id_aprovacao='' AND cp.necessita_aprov='S'
AND cp.ativo='S'
AND c.status<>9138
AND fp.nosso_num <>'' AND ISNULL(fp.dt_liq))

UNION

(SELECT o.usina, o.numero, cp.ano_contrato, cp.num_contrato
FROM con_contrato_pag cp
INNER JOIN con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato
INNER JOIN con_contrato c ON cp.usina=c.usina AND cp.ano_contrato=c.ano_contrato AND cp.num_contrato=c.num_contrato
INNER JOIN con_contrato_dinheir fp ON fp.usina=cp.usina AND fp.num_contrato=cp.num_contrato AND fp.ano_contrato=cp.ano_contrato AND fp.seq_pgto=cp.seq
WHERE cp.id_aprovacao='' AND cp.necessita_aprov='S'
AND cp.ativo='S'
AND c.status<>9138)

UNION
 
(SELECT o.usina, o.numero, cp.ano_contrato, cp.num_contrato
FROM con_contrato_pag cp
INNER JOIN con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato
INNER JOIN con_contrato c ON cp.usina=c.usina AND cp.ano_contrato=c.ano_contrato AND cp.num_contrato=c.num_contrato
INNER JOIN con_contrato_dep fp ON fp.usina=cp.usina AND fp.num_contrato=cp.num_contrato AND fp.ano_contrato=cp.ano_contrato AND fp.seq_pgto=cp.seq
WHERE cp.id_aprovacao='' AND cp.necessita_aprov='S'
AND cp.ativo='S'
AND c.status<>9138
AND fp.valor_dep<>0)

UNION

(SELECT o.usina, o.numero, cp.ano_contrato, cp.num_contrato
FROM con_contrato_pag cp
INNER JOIN con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato
INNER JOIN con_contrato c ON cp.usina=c.usina AND cp.ano_contrato=c.ano_contrato AND cp.num_contrato=c.num_contrato
INNER JOIN con_contrato_ccredit fp ON fp.usina=cp.usina AND fp.num_contrato=cp.num_contrato AND fp.ano_contrato=cp.ano_contrato AND fp.seq_pgto=cp.seq
WHERE cp.id_aprovacao='' AND cp.necessita_aprov='S'
AND cp.ativo='S'
AND c.status<>9138)

UNION
 
(SELECT o.usina, o.numero, cp.ano_contrato, cp.num_contrato
FROM con_contrato_pag cp
INNER JOIN con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato
INNER JOIN con_contrato c ON cp.usina=c.usina AND cp.ano_contrato=c.ano_contrato AND cp.num_contrato=c.num_contrato
INNER JOIN con_contrato_cheque fp ON fp.usina=cp.usina AND fp.num_contrato=cp.num_contrato AND fp.ano_contrato=cp.ano_contrato AND fp.seq_pgto=cp.seq
WHERE cp.id_aprovacao='' AND cp.necessita_aprov='S'
AND cp.ativo='S'
AND c.status<>9138
AND fp.vlr<>0)

UNION
 
(SELECT o.usina, o.numero, cp.ano_contrato, cp.num_contrato
FROM con_contrato_pag cp
INNER JOIN con_obras o ON cp.usina=o.usina AND cp.ano_contrato=o.ano_contrato AND cp.num_contrato=o.no_contrato
INNER JOIN con_contrato c ON cp.usina=c.usina AND cp.ano_contrato=c.ano_contrato AND cp.num_contrato=c.num_contrato
WHERE cp.id_aprovacao='' AND cp.necessita_aprov='S'
AND cp.ativo='S'
AND c.status<>9138
AND cp.forma='CT');

CREATE OR REPLACE VIEW view_pendentes_aprov_finan AS
SELECT * FROM view_pendentes_aprov_finan_wg
GROUP BY usina, numero, ano_contrato, num_contrato;

UPDATE view_pendentes_aprov_finan p
INNER JOIN con_obras o ON p.usina=o.usina AND p.numero=o.numero AND p.ano_contrato=o.ano_contrato AND p.num_contrato=o.no_contrato
SET o.status_financeiro=1
WHERE o.status_financeiro<>1;

/* Ajuste para deletar trigger que possivelmente foi criada nas usinas em um script anterior */
INSERT INTO topsys.ger_versao_script (script)
VALUES('DROP TRIGGER IF EXISTS topsys.con_contrato_BEFORE_INSERT;');

UPDATE topsys.con_obras o
INNER JOIN
topsys.con_contrato c ON o.ano_contrato = c.ano_contrato
AND o.no_contrato = c.num_contrato
and o.usina = c.usina
SET o.status_engenharia = 1
WHERE
c.aprov_eng = 'S'
AND c.id_aprov_eng = ''
AND c.fechado <> 'R'
and c.dt_encer_cont is null;

UPDATE con_obras o  
INNER JOIN
con_contrato c ON
o.ano_contrato = c.ano_contrato AND
o.no_contrato = c.num_contrato AND
o.usina = c.usina
SET status_cadastro = 
if( c.status =9132,0,
if( c.status =9134,1,
if( c.status =9133,3,
if( c.status =9135,5,
if( c.status =9136,2,
if( c.status =9138,7,
if( c.status =9139,6,
if( c.status =9140,4,2))))))))
WHERE
c.status in (9132,9133,9134,9135,9136,9137,9138,9139,9140,9141,9144);

DELIMITER $$
DROP TRIGGER IF EXISTS `topsys`.`con_contrato_AFTER_UPDATE`$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_contrato_AFTER_UPDATE` AFTER UPDATE ON `con_contrato` FOR EACH ROW
BEGIN
    SET @stCtrPreAnalise = 9132;
    SET @stCtrReprovado = 9133;
    SET @stCtrEmAnalise = 9134;
    SET @stCtrPendente = 9135;
    SET @stCtrAprovado = 9136;
    SET @stCtrAguardandoConfirmacaoPagamento = 9137;
    SET @stCtrCancelado = 9138;
    SET @stCtrAguardandoDataProgramacao = 9139;
    SET @stCtrRevalidacaoCadastro = 9140;
    SET @stCtrAguardandoDadosPagamento = 9141;
    SET @stCtrAguardandoAprovacaoComercial = 9144;
    
    SET @stComNaoNecessita = 0;
	SET @stComAguardando = 1;
	SET @stComAprovado = 2;
	SET @stComReprovado = 3;
    
    SET @stCadPreCadastro = 0;
	SET @stCadEmAnalise = 1;
	SET @stCadAprovado = 2;
	SET @stCadReprovado = 3;
	SET @stCadRevalidacao = 4;
	SET @stCadPendente = 5;
	SET @stCadAguardandoProgramacao = 6;
	SET @stCadCancelado = 7;
	SET @stCadEncerrado = 8;
    
    SET @stEngNaoNecessita = 0;
	SET @stEngAguardando = 1;
	SET @stEngAprovado = 2;
	SET @stEngReprovado = 3;
    
    SET @stFinNaoNecessita = 0;
	SET @stFinAguardandoConfirmacao = 1;
	SET @stFinAprovado = 2;
	SET @stFinReprovado = 3;
	SET @stFinAguardandoDadosPagamento = 4;
    
    SELECT o.status_cadastro, o.status_comercial, o.status_engenharia, o.status_financeiro
    INTO @stCadastro, @stComercial, @stEngenharia, @stFinanceiro
    FROM con_obras o
    WHERE o.usina=NEW.usina
    AND o.ano_contrato=NEW.ano_contrato
    AND o.no_contrato=NEW.num_contrato
    LIMIT 1;
    
    IF NEW.status = @stCtrPreAnalise THEN
		SET @stCadastro = @stCadPreCadastro;
	ELSEIF NEW.status = @stCtrReprovado THEN
		SET @stCadastro = @stCadReprovado;
	ELSEIF NEW.status = @stCtrEmAnalise THEN
		SET @stCadastro = @stCadEmAnalise;
	ELSEIF NEW.status = @stCtrAprovado THEN
		SET @stCadastro = @stCadAprovado;
	ELSEIF NEW.status = @stCtrRevalidacaoCadastro THEN
		SET @stCadastro = @stCadRevalidacao;
	ELSEIF NEW.status = @stCtrPendente THEN
		SET @stCadastro = @stCadPendente;
	ELSEIF NEW.status = @stCtrAguardandoDataProgramacao THEN
		SET @stCadastro = @stCadAguardandoProgramacao;
	ELSEIF NEW.status = @stCtrCancelado THEN
		SET @stCadastro = @stCadCancelado;
	ELSE
		SET @stCadastro = @stCadAprovado;
	END IF;
	
	IF NEW.status = @stCtrAguardandoAprovacaoComercial THEN
		SET @stComercial = @stComAguardando;
	ELSEIF NEW.status = @stCtrAguardandoDadosPagamento THEN
		SET @stFinanceiro = @stFinAguardandoDadosPagamento;
	ELSEIF NEW.status = @stCtrAguardandoConfirmacaoPagamento THEN
		SET @stFinanceiro = @stFinAguardandoConfirmacao;
    END IF;
	
    IF OLD.status = @stCtrAguardandoAprovacaoComercial AND NEW.status = @stCtrAprovado THEN
		SET @stComercial = @stComAprovado;
    END IF;
    
    IF OLD.status = @stCtrAguardandoConfirmacaoPagamento AND NEW.status = @stCtrAprovado THEN
		SET @stFinanceiro = @stFinAprovado;
    END IF;
    
    IF NEW.aprov_eng = 'S' AND NEW.id_aprov_eng <> '' THEN
		SET @stEngenharia = @stEngAprovado;
	ELSEIF NEW.aprov_eng = 'S' AND NEW.id_aprov_eng = '' THEN
		SET @stEngenharia = @stEngAguardando;
	ELSEIF NEW.aprov_eng <> 'S' THEN
		SET @stEngenharia = @stEngNaoNecessita;
    END IF;
    
    UPDATE con_obras o
    SET o.status_cadastro=@stCadastro,
    o.status_comercial=@stComercial,
    o.status_engenharia=@stEngenharia,
    o.status_financeiro=@stFinanceiro
    WHERE o.usina=NEW.usina
    AND o.ano_contrato=NEW.ano_contrato
    AND o.no_contrato=NEW.num_contrato;
    
END$$
DELIMITER ;

INSERT INTO topsys.ger_versao_script (script)
VALUES('DROP TRIGGER IF EXISTS topsys.con_contrato_AFTER_UPDATE;');