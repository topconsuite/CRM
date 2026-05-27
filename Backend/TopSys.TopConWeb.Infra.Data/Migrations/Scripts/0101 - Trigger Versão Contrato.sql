/* OP-5508 */
DROP TRIGGER IF EXISTS `topsys`.`con_contrato_AFTER_UPDATE`;

DELIMITER $$
USE `topsys`$$
CREATE DEFINER=`root`@`localhost` TRIGGER `topsys`.`con_contrato_AFTER_UPDATE` AFTER UPDATE ON `con_contrato` FOR EACH ROW
BEGIN

	DECLARE ParametroNome CHAR(40);
	DECLARE VersaoContrato INT;
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
	SET @stCtrEncerrado = 9145;
    
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
    
    select valor into @DesativaObApCad
    from ger_parametro where chave = 'DesativarObrigatoriedadeAprovacaoCadastro'
	and grupo = 'web';
    
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
    
    IF @DesativaObApCad = 1 Then
    SET @stCadastro = @stCadAprovado;
    END IF;
	
	IF NEW.status = @stCtrAguardandoAprovacaoComercial AND OLD.status <> @stCtrAguardandoAprovacaoComercial THEN
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


	SELECT IFNULL(parametro_nome, '') INTO ParametroNome FROM con_integracoes WHERE parametro_nome='usuario' AND integracao_tipo='customer';

	IF ParametroNome <> '' THEN
		INSERT INTO ger_db_events SET `table` = 'con_contrato', `column` = '', `key_Values` = CONCAT(NEW.usina, ';', NEW.ano_contrato, ';', NEW.num_contrato),`value_Old` = '',`value_New` = NULL, `event_Date` = now(), `event_Type` = 'update';
	END IF;
    
	SELECT IFNULL(parametro_nome, '') INTO ParametroNome FROM con_integracoes WHERE parametro_nome='usuario' AND integracao_tipo='ConcrebrasProtheus';

	SELECT IFNULL(MAX(num_versao), 1) INTO VersaoContrato FROM con_contrato_versao WHERE usina=NEW.usina AND ano_contrato=NEW.ano_contrato AND num_contrato=NEW.num_contrato AND status=@stCtrAprovado;

	IF ParametroNome <> '' THEN
		INSERT INTO ger_db_events SET `table` = 'con_contrato', `column` = 'ConcrebrasProtheus', `key_Values` = CONCAT(NEW.usina, ';', NEW.ano_contrato, ';', NEW.num_contrato, ';', VersaoContrato),`value_Old` = '',`value_New` = NULL, `event_Date` = now(), `event_Type` = 'update';
	END IF;

	SELECT IFNULL(parametro_nome, '') INTO ParametroNome FROM con_integracoes WHERE parametro_nome='IntegracaoConfigurada' AND integracao_tipo='topcon_integrator';

	IF ParametroNome <> '' THEN
		INSERT INTO ger_db_events SET `table` = 'con_contrato', `column` = '', `key_Values` = CONCAT(NEW.usina, ';', NEW.ano_contrato, ';', NEW.num_contrato),`value_Old` = '',`value_New` = NULL, `event_Date` = now(), `event_Type` = 'update';
	END IF;

END$$
DELIMITER ;

DROP TRIGGER IF EXISTS `topsys`.`con_contrato_versao_AFTER_UPDATE`;

DELIMITER $$
DROP TRIGGER IF EXISTS `topsys`.`con_contrato_versao_AFTER_UPDATE`$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_contrato_versao_AFTER_UPDATE` AFTER UPDATE ON `con_contrato_versao` FOR EACH ROW
BEGIN
	DECLARE ParametroNome CHAR(40);
	DECLARE VersaoContrato INT;
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
    SET @stCtrEncerrado = 9145;
    
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
    FROM con_obras_versao o
    WHERE o.usina=NEW.usina
    AND o.ano_contrato=NEW.ano_contrato
    AND o.no_contrato=NEW.num_contrato
    AND o.num_versao=NEW.num_versao
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
	
	IF NEW.status = @stCtrAguardandoAprovacaoComercial AND OLD.status <> @stCtrAguardandoAprovacaoComercial THEN
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
    
    UPDATE con_obras_versao o
    SET o.status_cadastro=@stCadastro,
    o.status_comercial=@stComercial,
    o.status_engenharia=@stEngenharia,
    o.status_financeiro=@stFinanceiro
    WHERE o.usina=NEW.usina
    AND o.ano_contrato=NEW.ano_contrato
    AND o.no_contrato=NEW.num_contrato
    AND o.num_versao=NEW.num_versao;	

	SELECT IFNULL(parametro_nome, '') INTO ParametroNome FROM con_integracoes WHERE parametro_nome='usuario' AND integracao_tipo='ConcrebrasProtheus';

	SELECT IFNULL(MAX(num_versao), 1) INTO VersaoContrato FROM con_contrato_versao WHERE usina=NEW.usina AND ano_contrato=NEW.ano_contrato AND num_contrato=NEW.num_contrato AND status=@stCtrAprovado;

	IF ParametroNome <> '' AND NEW.status = @stCtrAprovado THEN
		INSERT INTO ger_db_events SET `table` = 'con_contrato', `column` = 'ConcrebrasProtheus', `key_Values` = CONCAT(NEW.usina, ';', NEW.ano_contrato, ';', NEW.num_contrato, ';', VersaoContrato),`value_Old` = '',`value_New` = NULL, `event_Date` = now(), `event_Type` = 'update';
	END IF;

END$$
DELIMITER ;


DROP TRIGGER IF EXISTS `topsys`.`con_contrato_versao_AFTER_INSERT`;

DELIMITER $$
DROP TRIGGER IF EXISTS `topsys`.`con_contrato_versao_AFTER_INSERT`$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_contrato_versao_AFTER_INSERT` AFTER INSERT ON `con_contrato_versao` FOR EACH ROW
BEGIN
	DECLARE ParametroNome CHAR(40);
	DECLARE VersaoContrato INT;
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
    SET @stCtrEncerrado = 9145;

	SELECT IFNULL(parametro_nome, '') INTO ParametroNome FROM con_integracoes WHERE parametro_nome='usuario' AND integracao_tipo='ConcrebrasProtheus';

	SELECT IFNULL(MAX(num_versao), 1) INTO VersaoContrato FROM con_contrato_versao WHERE usina=NEW.usina AND ano_contrato=NEW.ano_contrato AND num_contrato=NEW.num_contrato AND status=@stCtrAprovado;

	IF ParametroNome <> '' AND NEW.status = @stCtrAprovado THEN
		INSERT INTO ger_db_events SET `table` = 'con_contrato', `column` = 'ConcrebrasProtheus', `key_Values` = CONCAT(NEW.usina, ';', NEW.ano_contrato, ';', NEW.num_contrato, ';', VersaoContrato),`value_Old` = '',`value_New` = NULL, `event_Date` = now(), `event_Type` = 'update';
	END IF;

END$$
DELIMITER ;