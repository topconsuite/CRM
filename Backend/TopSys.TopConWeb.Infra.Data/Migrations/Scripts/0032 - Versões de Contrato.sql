/* TC-3936 */
CREATE TABLE IF NOT EXISTS  `con_chtel_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `dt` date DEFAULT NULL,
  `hora` smallint(4) unsigned NOT NULL DEFAULT '0',
  `vendedor` smallint(3) unsigned NOT NULL DEFAULT '0',
  `represent` smallint(3) unsigned NOT NULL DEFAULT '0',
  `vend_padrinho` smallint(3) unsigned NOT NULL DEFAULT '0',
  `num_tab_preco` mediumint(7) unsigned NOT NULL DEFAULT '0',
  `contato` char(30) NOT NULL DEFAULT '',
  `ddd` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `tel` int(9) unsigned NOT NULL DEFAULT '0',
  `ramal` smallint(4) unsigned NOT NULL DEFAULT '0',
  `celular` int(9) unsigned NOT NULL DEFAULT '0',
  `cod_cliente` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `cliente` char(40) NOT NULL DEFAULT '',
  `cep` char(8) NOT NULL DEFAULT '',
  `end` char(40) NOT NULL DEFAULT '',
  `num` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `compl` char(20) NOT NULL DEFAULT '',
  `bairro` char(20) NOT NULL DEFAULT '',
  `mun` smallint(3) unsigned NOT NULL DEFAULT '0',
  `email` char(255) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `obs` char(100) NOT NULL DEFAULT '',
  `dt_retorno` date DEFAULT NULL,
  `visita` char(1) NOT NULL DEFAULT '',
  `ano_visita` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_visita` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `dt_visita` date DEFAULT NULL,
  `desist` smallint(4) unsigned NOT NULL DEFAULT '0',
  `dt_desist` date DEFAULT NULL,
  `vend_vis` smallint(3) unsigned NOT NULL DEFAULT '0',
  `fis_jur` char(1) NOT NULL DEFAULT '',
  `fator_inicio` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `fator_contato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `fator_valores` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `fator_pgto` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `fator_local` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `avaliacao` float(3,1) unsigned NOT NULL DEFAULT '0.0',
  `ganhou` char(1) NOT NULL DEFAULT '',
  `dt_retorno_efet` date DEFAULT NULL,
  `hora_retor_efet` char(4) NOT NULL DEFAULT '',
  `vlr_concreto` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_bomba` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_extras` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_total_ctr` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `total_m3` double(8,2) unsigned NOT NULL DEFAULT '0.00',
  `id_emissao` char(19) NOT NULL DEFAULT '',
  `cond_pag_desist` char(30) NOT NULL DEFAULT '',
  `tp_contato` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `produto` smallint(4) unsigned NOT NULL DEFAULT '0',
  `tem_obras` char(1) NOT NULL DEFAULT '',
  `qtde_obras` smallint(3) unsigned NOT NULL DEFAULT '0',
  `propos_emitida` char(1) NOT NULL DEFAULT '',
  `vlr_iss_ret` double(9,2) unsigned NOT NULL DEFAULT '0.00',
  `aprov_eng` char(1) NOT NULL DEFAULT 'N',
  `id_aprov_eng` char(19) NOT NULL DEFAULT '',
  `no_obra` mediumint(6) NOT NULL DEFAULT '0',
  `nome_mae` char(40) DEFAULT '',
  `conjuge` char(40) DEFAULT '',
  `status` smallint(4) unsigned NOT NULL DEFAULT '0',
  `cnpj_cpf` char(14) NOT NULL DEFAULT '',
  `nome_cliente` char(20) NOT NULL DEFAULT '',
  `ie` char(20) NOT NULL DEFAULT '',
  `ccm` char(15) NOT NULL DEFAULT '',
  `rg` char(15) NOT NULL DEFAULT '',
  `org_uf_emi` char(15) NOT NULL DEFAULT '',
  `profissao` char(30) NOT NULL DEFAULT '',
  `emp_trabalho` char(50) NOT NULL DEFAULT '',
  `ddd_com` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `tel_com` int(9) unsigned NOT NULL DEFAULT '0',
  `faturamento_ac` char(30) NOT NULL DEFAULT '',
  `fat_idem_client` char(1) NOT NULL DEFAULT '',
  `fat_cnpj_cpf` char(14) NOT NULL DEFAULT '',
  `fat_razao` char(40) NOT NULL DEFAULT '',
  `fat_nome` char(20) NOT NULL DEFAULT '',
  `fat_email` char(255) NOT NULL DEFAULT '',
  `fat_cep` char(8) NOT NULL DEFAULT '',
  `fat_end` char(40) NOT NULL DEFAULT '',
  `fat_num` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `fat_compl` char(20) NOT NULL DEFAULT '',
  `fat_bairro` char(20) NOT NULL DEFAULT '',
  `fat_mun` smallint(3) unsigned NOT NULL DEFAULT '0',
  `fat_ie` char(20) NOT NULL DEFAULT '',
  `fat_ccm` char(15) NOT NULL DEFAULT '',
  `fat_rg` char(15) NOT NULL DEFAULT '',
  `fat_org_uf_emi` char(15) NOT NULL DEFAULT '',
  `just_aprov` char(50) NOT NULL DEFAULT '',
  `Prop_Emi_Aprov` char(1) NOT NULL DEFAULT '',
  `no_obra_pref` char(15) NOT NULL DEFAULT '',
  `status_anterior` smallint(4) unsigned NOT NULL DEFAULT '0',
  `ddd_celular` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `ccm_obra` char(14) NOT NULL DEFAULT '',
  `origem_usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `origem_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `modelo_doc_remessa_concreto` tinyint(4) DEFAULT '0',
  `modelo_doc_remessa_bomba` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_chamada`,`num_chamada`,`no_obra`),
  KEY `IDX_CNPJCPF` (`cnpj_cpf`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_contrato_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `interv` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `dt_contrato` date DEFAULT NULL,
  `dt_encer_cont` date DEFAULT NULL,
  `representante` smallint(3) unsigned NOT NULL DEFAULT '0',
  `vendedor` smallint(3) unsigned NOT NULL DEFAULT '0',
  `vend_padrinho` smallint(3) unsigned NOT NULL DEFAULT '0',
  `num_tab_preco` smallint(3) unsigned NOT NULL DEFAULT '0',
  `local_cobranca` smallint(3) unsigned NOT NULL DEFAULT '0',
  `local_fatur` smallint(3) unsigned NOT NULL DEFAULT '0',
  `dt_carta_reajus` date DEFAULT NULL,
  `faturamento_ac` char(30) NOT NULL DEFAULT '',
  `obs` char(90) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `no_obra` char(25) NOT NULL DEFAULT '',
  `fis_jur` char(1) NOT NULL DEFAULT '',
  `fechado` char(1) NOT NULL DEFAULT '',
  `id_aprov_vend` char(19) NOT NULL DEFAULT '',
  `id_aprov_dir` char(19) NOT NULL DEFAULT '',
  `id_aprov_cad` char(19) NOT NULL DEFAULT '',
  `id_aprov_prog` char(19) NOT NULL DEFAULT '',
  `no_ctr_ant` char(20) NOT NULL DEFAULT '',
  `vlr_concreto` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_bomba` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_extras` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_total_ctr` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `total_m3` double(8,2) unsigned NOT NULL DEFAULT '0.00',
  `usina_principal` smallint(3) unsigned NOT NULL DEFAULT '0',
  `pag_ant_analis` char(1) NOT NULL DEFAULT '',
  `id_analise` char(19) NOT NULL DEFAULT '',
  `cad_aprovado` char(1) NOT NULL DEFAULT '',
  `cheque_analis` char(1) NOT NULL DEFAULT '',
  `id_analise_chq` char(19) NOT NULL DEFAULT '',
  `id_aprov_dinh` char(19) NOT NULL DEFAULT '',
  `dt_pag_dinh` date DEFAULT NULL,
  `vlr_dinheiro` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `aguard_aprov` char(1) NOT NULL DEFAULT '',
  `descr_coincid` text NOT NULL,
  `aprov_coincid` char(19) NOT NULL DEFAULT '',
  `num_cartaocred` smallint(4) unsigned NOT NULL DEFAULT '0',
  `resp_solidario` smallint(3) NOT NULL DEFAULT '0',
  `email_enviado` char(1) NOT NULL DEFAULT '',
  `aprov_eng` char(1) NOT NULL DEFAULT 'N',
  `id_aprov_eng` char(19) NOT NULL DEFAULT '',
  `vlr_iss_ret` double(9,2) unsigned NOT NULL DEFAULT '0.00',
  `id_pre_cadastro` char(19) DEFAULT '',
  `analista` mediumint(4) DEFAULT '0',
  `status` smallint(4) unsigned NOT NULL DEFAULT '0',
  `status_anterior` smallint(4) unsigned NOT NULL DEFAULT '0',
  `inconsistencias` char(50) NOT NULL DEFAULT '',
  `modelo_danfe` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `fat_pendente` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `ccm_obra` char(25) NOT NULL DEFAULT '',
  `trib_iss` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `produto` smallint(4) unsigned NOT NULL DEFAULT '8801',
  `modelo_doc_remessa_concreto` tinyint(4) DEFAULT '0',
  `modelo_doc_remessa_bomba` tinyint(4) DEFAULT '0',
  `retencao_contratual` double(5,2) unsigned NOT NULL,
  `mao_obra_propria` char(1) DEFAULT '',
  `percentual_locacao` double(5,2) unsigned NOT NULL,
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS  `con_chtel_faturamento_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `idem_client` char(1) NOT NULL DEFAULT '',
  `cnpj_cpf` char(14) NOT NULL DEFAULT '',
  `razao` char(40) NOT NULL DEFAULT '',
  `nome` char(20) NOT NULL DEFAULT '',
  `email` char(255) NOT NULL DEFAULT '',
  `ie` char(20) NOT NULL DEFAULT '',
  `ccm` char(15) NOT NULL DEFAULT '',
  `rg` char(15) NOT NULL DEFAULT '',
  `org_uf_emi` char(15) NOT NULL DEFAULT '',
  `cep` char(8) NOT NULL DEFAULT '',
  `end` char(40) NOT NULL DEFAULT '',
  `num` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `compl` char(20) NOT NULL DEFAULT '',
  `bairro` char(20) NOT NULL DEFAULT '',
  `mun` smallint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_chamada`,`num_chamada`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_obras_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `numero` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `ano_levto` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_levto` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `ano_visita` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_visita` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `ano_proposta` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_proposta` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `obra_nome` char(30) NOT NULL DEFAULT '',
  `obra_cep` char(8) NOT NULL DEFAULT '',
  `obra_end` char(40) NOT NULL DEFAULT '',
  `obra_num` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `obra_compl` char(30) NOT NULL DEFAULT '',
  `obra_bairro` char(20) NOT NULL DEFAULT '',
  `obra_mun` smallint(3) unsigned NOT NULL DEFAULT '0',
  `email` char(255) NOT NULL DEFAULT '',
  `obra_usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra_km_usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra_km_usina_via_google` smallint(3) unsigned DEFAULT '0',
  `obra_prev_inic` date DEFAULT NULL,
  `obra_prev_term` date DEFAULT NULL,
  `obra_prazo` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `obra_tipo` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_porte` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_fase` smallint(4) unsigned NOT NULL DEFAULT '0',
  `fase_concorrenc` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_topografia` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_acesso` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_vl_pedagio` float(5,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_zrmc` char(1) NOT NULL DEFAULT '',
  `obra_proib_est` char(1) NOT NULL DEFAULT '',
  `obra_fio_tensao` char(1) NOT NULL DEFAULT '',
  `obra_zona_azul` char(1) NOT NULL DEFAULT '',
  `obra_nova` char(1) NOT NULL DEFAULT '',
  `obra_inic_conc` char(1) NOT NULL DEFAULT '',
  `obra_contato` char(30) NOT NULL DEFAULT '',
  `obra_cont_func` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_cont_fone` int(9) unsigned NOT NULL DEFAULT '0',
  `obra_cont_cel` int(9) unsigned NOT NULL DEFAULT '0',
  `obra_contato1` char(30) NOT NULL DEFAULT '',
  `obra_cont1_func` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_cont1_fone` int(9) unsigned NOT NULL DEFAULT '0',
  `obra_cont1_cel` int(9) unsigned NOT NULL DEFAULT '0',
  `obra_captado_v` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_refer_aces` text,
  `obra_pct_rp_cim` float(4,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_pct_rp_ped` float(4,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_pct_rp_are` float(4,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_pct_rp_obr` float(4,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_pct_rp_die` float(4,2) unsigned NOT NULL DEFAULT '0.00',
  `concorrente` smallint(4) unsigned NOT NULL DEFAULT '0',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `obra_resultado` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_desist` smallint(4) unsigned NOT NULL DEFAULT '0',
  `obra_dt_desist` date DEFAULT NULL,
  `obra_usina_sec` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra_km_usina_s` smallint(3) unsigned NOT NULL DEFAULT '0',
  `volume_concreto` float(7,1) unsigned NOT NULL DEFAULT '0.0',
  `cond_pgto` smallint(3) unsigned NOT NULL DEFAULT '0',
  `tipo_cobranca` smallint(4) unsigned NOT NULL DEFAULT '0',
  `ganhou` char(1) NOT NULL DEFAULT '',
  `reaj_id_solic` char(19) NOT NULL DEFAULT '',
  `reaj_aprov_verb` char(1) NOT NULL DEFAULT '',
  `reaj_id_aprov` char(19) NOT NULL DEFAULT '',
  `obs` varchar(1000) NOT NULL,
  `bco_cobranca` smallint(3) unsigned NOT NULL DEFAULT '0',
  `reaj_sts_aprov` char(1) NOT NULL DEFAULT '',
  `reaj_cien_aprov` char(1) NOT NULL DEFAULT '',
  `cp_prv_tipo` char(8) NOT NULL DEFAULT '',
  `cp_prv_qtde` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `cp_prv_intvl_m3` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `intinerante` char(1) NOT NULL DEFAULT '',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra_cont_ddd` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `obra_cont_ddd1` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `vibr_qtde` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `vibr_vlr_unit` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `volume_pcarga` float(5,1) unsigned NOT NULL DEFAULT '0.0',
  `obra_cel_ddd` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `obra_cel_ddd1` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `obra_radio` char(15) NOT NULL DEFAULT '',
  `temp_ate_a_obra` smallint(3) unsigned NOT NULL DEFAULT '0',
  `temp_bt_na_obra` smallint(3) unsigned NOT NULL DEFAULT '0',
  `url_mapa_obra` char(200) NOT NULL DEFAULT '',
  `status_comercial` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `status_cadastro` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `status_financeiro` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `status_engenharia` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `vlr_demais_servicos` double(8,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_cei` varchar(12) NOT NULL DEFAULT '',
  `obra_obs` varchar(1000) NOT NULL DEFAULT '',
  `tempo_ciclo_prev` smallint(3) unsigned NOT NULL DEFAULT '0',
  `custo_proje_trans` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `obra_benef_fiscal` varchar(45) DEFAULT '',
  `rota_padrao` char(36) DEFAULT NULL,
  PRIMARY KEY (`num_versao`,`usina`,`numero`,`ano_chamada`,`no_chamada`),
  KEY `contrato` (`usina`,`ano_contrato`,`no_contrato`),
  KEY `endereco` (`obra_end`),
  KEY `chamada` (`usina`,`ano_chamada`,`no_chamada`),
  KEY `IDX_TAXA_EXTRA` (`obra_usina`,`numero`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_chtel_cobranca_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `idem_client` char(1) NOT NULL DEFAULT '',
  `cnpj_cpf` char(14) NOT NULL DEFAULT '',
  `razao` char(40) NOT NULL DEFAULT '',
  `nome` char(20) NOT NULL DEFAULT '',
  `email` char(255) NOT NULL DEFAULT '',
  `ie` char(20) NOT NULL DEFAULT '',
  `ccm` char(15) NOT NULL DEFAULT '',
  `rg` char(15) NOT NULL DEFAULT '',
  `org_uf_emi` char(15) NOT NULL DEFAULT '',
  `cep` char(8) NOT NULL DEFAULT '',
  `end` char(40) NOT NULL DEFAULT '',
  `num` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `compl` char(20) NOT NULL DEFAULT '',
  `bairro` char(20) NOT NULL DEFAULT '',
  `mun` smallint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_chamada`,`num_chamada`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_chtel_resp_solid_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `idem_client` char(1) NOT NULL DEFAULT '',
  `cnpj_cpf` char(14) NOT NULL DEFAULT '',
  `razao` char(40) NOT NULL DEFAULT '',
  `nome` char(20) NOT NULL DEFAULT '',
  `email` char(255) NOT NULL DEFAULT '',
  `ie` char(20) NOT NULL DEFAULT '',
  `ccm` char(15) NOT NULL DEFAULT '',
  `rg` char(15) NOT NULL DEFAULT '',
  `org_uf_emi` char(15) NOT NULL DEFAULT '',
  `cep` char(8) NOT NULL DEFAULT '',
  `end` char(40) NOT NULL DEFAULT '',
  `num` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `compl` char(20) NOT NULL DEFAULT '',
  `bairro` char(20) NOT NULL DEFAULT '',
  `mun` smallint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_chamada`,`num_chamada`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_chtel_pag_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `cond_pgto` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `tp_cobranca` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `forma` char(2) NOT NULL DEFAULT '',
  `valor_fixo` char(1) NOT NULL DEFAULT '',
  `valor` double(11,2) NOT NULL DEFAULT '0.00',
  `pct` double(12,6) NOT NULL DEFAULT '0.000000',
  `necessita_aprov` char(1) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `no_obra` mediumint(6) NOT NULL DEFAULT '0',
  `id_aprovacao` char(19) NOT NULL DEFAULT '',
  `ativo` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`num_versao`,`usina`,`ano_chamada`,`num_chamada`,`seq`,`no_obra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_contrato_pag_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `cond_pagto` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `tp_cobranca` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `forma` char(2) NOT NULL DEFAULT '',
  `valor_fixo` char(1) NOT NULL DEFAULT '',
  `valor` double(11,2) NOT NULL DEFAULT '0.00',
  `pct` double(9,6) NOT NULL DEFAULT '0.000000',
  `necessita_aprov` char(1) NOT NULL DEFAULT '',
  `ativo` char(1) NOT NULL DEFAULT 'S',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `id_aprovacao` char(19) NOT NULL DEFAULT '',
  `valor_apropriado` double(11,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`seq`),
  KEY `IDX_CONTRATO` (`usina`,`ano_contrato`,`num_contrato`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_proposta_item_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `tp_resist` smallint(3) unsigned NOT NULL DEFAULT '0',
  `fck` float(3,1) unsigned NOT NULL DEFAULT '0.0',
  `consumo` smallint(3) unsigned NOT NULL DEFAULT '0',
  `uso` smallint(4) unsigned NOT NULL DEFAULT '0',
  `pedra` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `slump` smallint(3) unsigned NOT NULL DEFAULT '0',
  `qtde_m3` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `preco_unit_tab` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `preco_unit_prop` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `pct_descto` float(5,2) NOT NULL DEFAULT '0.00',
  `descto_aprovado` char(1) NOT NULL DEFAULT '',
  `id_aprov_descto` char(19) NOT NULL DEFAULT '',
  `aprov_verbal` char(1) NOT NULL DEFAULT '',
  `obs_aprov` char(15) NOT NULL DEFAULT '',
  `preco_concorren` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `peca_concretar` char(20) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `custo_servico` float(5,2) NOT NULL DEFAULT '0.00',
  `status_aprov` char(1) NOT NULL DEFAULT '',
  `ciente_aprov` char(1) NOT NULL DEFAULT '',
  `tipo_aprov` char(1) NOT NULL DEFAULT '',
  `m3_entregue` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `dt_ult_entrega` date DEFAULT NULL,
  `num_ult_nf_ent` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `pr_reajust_ant` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `dt_ult_reajuste` date DEFAULT NULL,
  `pr_reajustado_a` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `slump_nominal` smallint(3) unsigned NOT NULL DEFAULT '0',
  `custo_serv_ant` double(10,2) NOT NULL DEFAULT '0.00',
  `custo_serv_a` double(10,2) NOT NULL DEFAULT '0.00',
  `obs_traco` char(50) NOT NULL DEFAULT '',
  `pTp_resist` smallint(3) unsigned NOT NULL DEFAULT '0',
  `pfck` float(3,1) unsigned NOT NULL DEFAULT '0.0',
  `pconsumo` smallint(3) unsigned NOT NULL DEFAULT '0',
  `puso` smallint(4) unsigned NOT NULL DEFAULT '0',
  `ppedra` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `pslump` smallint(3) unsigned NOT NULL DEFAULT '0',
  `vl_ressarc` double(6,2) NOT NULL DEFAULT '0.00',
  `id_aprov_ressarc` char(19) NOT NULL DEFAULT '',
  `vl_rel_agua_cim` float(6,3) unsigned NOT NULL DEFAULT '0.000',
  `qtde_cim_p_m3` double(8,4) unsigned NOT NULL DEFAULT '0.0000',
  `temp_lanc` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obs_familiar` char(255) NOT NULL DEFAULT '',
  `cod_concorrente` smallint(4) unsigned NOT NULL DEFAULT '0',
  `desvio_dias` smallint(3) unsigned NOT NULL DEFAULT '0',
  `id_alt_traco_p` char(19) NOT NULL DEFAULT '',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `just_aprov` char(50) NOT NULL DEFAULT '',
  `pedido` char(15) NOT NULL DEFAULT '',
  `pedido_item` tinyint(6) NOT NULL DEFAULT '0',
  `preco_ajustado` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `qtde_m3_bombeado` float(7,2) NOT NULL DEFAULT '-1.00',
  `margem_pos_transporte` float(6,2) NOT NULL DEFAULT '0.00',
  `ebitda` float(6,2) NOT NULL DEFAULT '0.00',
  `iss_aplicado` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `imposto_aplicado_estadual` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `imposto_aplicado_federal` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `custo_bombagem` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `imposto_aplicado_servico` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `imposto_aplicado_faturamento` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `custo_transporte` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `ativo` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`num_versao`,`usina`,`no_obra`,`seq`,`ano_chamada`,`no_chamada`),
  KEY `traso` (`usina`,`no_obra`,`seq`,`fck`,`consumo`,`uso`,`pedra`,`slump`),
  KEY `usina_ano_no_chamada_seq` (`usina`,`ano_chamada`,`no_chamada`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_reajuste_item_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `dt_vigencia` date NOT NULL DEFAULT '0000-00-00',
  `item_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `tp_resist` smallint(3) unsigned NOT NULL DEFAULT '0',
  `fck` float(5,1) unsigned NOT NULL DEFAULT '0.0',
  `consumo` smallint(3) unsigned NOT NULL DEFAULT '0',
  `uso` smallint(4) unsigned NOT NULL DEFAULT '0',
  `pedra` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `slump` smallint(3) unsigned NOT NULL DEFAULT '0',
  `preco_vigente` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `custo_vigente` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `servico_vigente` double(10,2) NOT NULL DEFAULT '0.00',
  `preco_recalc` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `custo_recalc` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `servico_recalc` double(10,2) NOT NULL DEFAULT '0.00',
  `pct_reajuste` float(4,2) NOT NULL DEFAULT '0.00',
  `emite_carta` char(1) NOT NULL DEFAULT '',
  `usina_principal` smallint(3) unsigned NOT NULL DEFAULT '0',
  `data_carta` date DEFAULT NULL,
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `dt_confirmacao` date DEFAULT NULL,
  `dt_calculo` date DEFAULT NULL,
  `tab_custo` mediumint(9) NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`dt_vigencia`,`item_contrato`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


CREATE TABLE IF NOT EXISTS  `con_prop_bomba_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `tipo_bomba` smallint(4) unsigned NOT NULL DEFAULT '0',
  `m3_pr_tab` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `taxa_minima_tab` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `pr_m3_tab` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `m3_pr_prop` smallint(3) unsigned NOT NULL DEFAULT '0',
  `txa_min_pr_prop` float(8,2) unsigned NOT NULL DEFAULT '0.00',
  `pr_m3_bomb_pr_p` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `pct_descto` float(5,2) NOT NULL DEFAULT '0.00',
  `aprovacao` char(1) NOT NULL DEFAULT '',
  `aprovado` char(1) NOT NULL DEFAULT '',
  `id_aprovacao` char(19) NOT NULL DEFAULT '',
  `aprov_verbal` char(1) NOT NULL DEFAULT '',
  `obs_aprov` char(15) NOT NULL DEFAULT '',
  `bomba_propria` char(1) NOT NULL DEFAULT '',
  `terceiro` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `fat_direto` char(1) NOT NULL DEFAULT '',
  `pr_concorrente` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `status_aprov` char(1) NOT NULL DEFAULT '',
  `ciente_aprov` char(1) NOT NULL DEFAULT '',
  `alugadapcliente` char(1) NOT NULL DEFAULT '',
  `tipo_calc` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `dist_tub_bomba` smallint(3) unsigned NOT NULL DEFAULT '0',
  `vlr_adic_tub` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `complex_bomb` char(1) NOT NULL DEFAULT 'N',
  `just_aprov` char(50) NOT NULL DEFAULT '',
  `tipo_calc_hora` tinyint(1) unsigned NOT NULL DEFAULT '9',
  `tx_min_hora_tab` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `tempo_min_h_tab` float(5,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_hora_exc_tab` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `tx_min_hora_prop` float(8,2) unsigned NOT NULL DEFAULT '0.00',
  `tempo_min_h_prop` float(5,2) unsigned NOT NULL DEFAULT '0.00',
  `vlr_hora_exc_prop` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `pct_descto_hora` float(5,2) unsigned NOT NULL DEFAULT '0.00',
  `imposto_aplicado_estadual` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `imposto_aplicado_federal` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `iss_aplicado` float(6,2) unsigned NOT NULL DEFAULT '0.00',
  `ebitda` float(6,2) NOT NULL DEFAULT '0.00',
  `ativo` char(1) NOT NULL DEFAULT 'S',
  PRIMARY KEY (`num_versao`,`usina`,`no_obra`,`seq`,`ano_chamada`,`no_chamada`),
  KEY `usina_ano_no_chamada_seq` (`usina`,`ano_chamada`,`no_chamada`,`seq`),
  KEY `IDX_ID_AP_CIENTE` (`id_aprovacao`,`ciente_aprov`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_obras_dem_serv_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `cod` smallint(3) unsigned NOT NULL DEFAULT '0',
  `usina_entrega` smallint(3) unsigned NOT NULL DEFAULT '0',
  `merc` char(20) NOT NULL DEFAULT '',
  `Unid_cobranca` char(3) NOT NULL DEFAULT '',
  `Casas_decimais` smallint(3) unsigned DEFAULT NULL,
  `Preco_Sugerido` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Preco_Minimo` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Frequencia_Cobranca` enum('Contrato','Programacao','Remessa','M3','M3Bombeado','Bombeamento') DEFAULT NULL,
  `Forma_Cobranca` enum('NaRemessa','FinalConcretagem') DEFAULT NULL,
  `atualiza_estoque` tinyint(1) DEFAULT '0',
  `Preco_Proposto` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `Quantidade` double(14,6) unsigned NOT NULL DEFAULT '0.000000',
  PRIMARY KEY (`num_versao`,`usina`,`obra`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_programacao_dem_serv_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq_prog` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq` smallint(3) unsigned NOT NULL DEFAULT '0',
  `Quantidade` double(14,6) unsigned NOT NULL DEFAULT '0.000000',
  `valor_total` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  `valor_cobrado` double(10,2) unsigned NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`num_versao`,`usina`,`obra`,`seq_prog`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_obras_tx_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `selecionado` char(1) NOT NULL DEFAULT '',
  `id_solic` char(19) NOT NULL DEFAULT '',
  `aprov_desc` char(1) NOT NULL DEFAULT '',
  `id_aprov` char(19) NOT NULL DEFAULT '',
  `ciente` char(1) NOT NULL DEFAULT '',
  PRIMARY KEY (`num_versao`,`usina`,`obra`,`seq`),
  KEY `IDX_PENDENTE` (`aprov_desc`),
  KEY `IDX_CIENTE_ID` (`ciente`,`id_solic`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_contrato_ccredit_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq_pgto` smallint(3) unsigned NOT NULL,
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `bandeira` smallint(4) unsigned NOT NULL DEFAULT '0',
  `no_cartao` smallint(4) unsigned NOT NULL DEFAULT '0',
  `dt_transacao` date DEFAULT NULL,
  `valor` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `no_parcelas` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `no_autorizacao` char(20) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`seq_pgto`,`seq`,`ano_chamada`,`num_chamada`,`no_obra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_contrato_dep_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `dt_deposito` date DEFAULT NULL,
  `portador` smallint(3) unsigned DEFAULT '0',
  `bco_tomad` smallint(3) unsigned NOT NULL DEFAULT '0',
  `agencia_tomad` char(10) CHARACTER SET latin1 NOT NULL DEFAULT '',
  `no_terminal` char(10) CHARACTER SET latin1 NOT NULL DEFAULT '',
  `no_conta` char(12) CHARACTER SET latin1 NOT NULL DEFAULT '',
  `valor_dep` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `id_aprovacao` char(19) CHARACTER SET latin1 NOT NULL DEFAULT '',
  `seq_pgto` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `id_cadast` char(19) CHARACTER SET latin1 NOT NULL DEFAULT '',
  `id_atual` char(19) CHARACTER SET latin1 NOT NULL DEFAULT '',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`seq`,`seq_pgto`,`ano_chamada`,`num_chamada`,`no_obra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS  `con_contrato_dinheir_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq_pgto` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_recibo` char(10) NOT NULL DEFAULT '',
  `dt_pagamento` date DEFAULT NULL,
  `valor` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `id_aprovacao` char(19) NOT NULL DEFAULT '',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`seq_pgto`,`seq`,`ano_chamada`,`num_chamada`,`no_obra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_contrato_cheque_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `bco_cheque` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ag_cheque` smallint(4) unsigned NOT NULL DEFAULT '0',
  `conta_cheque` bigint(10) unsigned NOT NULL DEFAULT '0',
  `dv_conta` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `num_cheque` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `dt_receb` date DEFAULT NULL,
  `vlr` double(11,2) unsigned NOT NULL DEFAULT '0.00',
  `bom_para` date DEFAULT NULL,
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `obs` char(255) NOT NULL DEFAULT '',
  `seq_pgto` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`seq`,`seq_pgto`,`ano_chamada`,`num_chamada`,`no_obra`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=REDUNDANT;

CREATE TABLE IF NOT EXISTS  `con_reaj_bomba_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `dt_vigencia` date NOT NULL DEFAULT '0000-00-00',
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `tipo_bomba` smallint(4) unsigned NOT NULL DEFAULT '0',
  `tx_min_atual` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `vol_min` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `vlr_m3_atual` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `tx_min_recalc` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `vol_min_recalc` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `vlr_m3_recalc` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `pct_reajuste` float(6,2) NOT NULL DEFAULT '0.00',
  `emite_carta` char(1) NOT NULL DEFAULT '',
  `data_carta` date DEFAULT NULL,
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `dt_calculo` date DEFAULT NULL,
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`dt_vigencia`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_contrato_boleto_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `seq_pgto` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `dt_vcto` date DEFAULT NULL,
  `dt_hr_imp` datetime DEFAULT NULL,
  `nosso_num` char(25) NOT NULL DEFAULT '',
  `linha_dig` char(70) NOT NULL DEFAULT '',
  `cod_barra` char(70) NOT NULL DEFAULT '',
  `dt_remessa` date DEFAULT NULL,
  `dt_liq` date DEFAULT NULL,
  `vl_orig` double(9,2) unsigned NOT NULL DEFAULT '0.00',
  `vl_liq` double(9,2) unsigned NOT NULL DEFAULT '0.00',
  `id_liq` char(19) NOT NULL DEFAULT '',
  `ano_chamada` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `num_chamada` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `seq` tinyint(2) unsigned NOT NULL DEFAULT '1',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  PRIMARY KEY (`num_versao`,`usina`,`ano_contrato`,`num_contrato`,`seq_pgto`,`ano_chamada`,`num_chamada`,`no_obra`,`seq`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE OR REPLACE
VIEW `view_con_contrato_pag_versao` AS
    SELECT 	  
	  `cp`.`num_versao` AS `num_versao`,
        `cp`.`usina` AS `usina`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `cp`.`seq` AS `seq`,
        `cp`.`cond_pagto` AS `cond_pagto`,
        `cp`.`tp_cobranca` AS `tp_cobranca`,
        `cp`.`forma` AS `forma`,
        `cp`.`valor_fixo` AS `valor_fixo`,
        `cp`.`valor` AS `valor`,
        `cp`.`pct` AS `pct`,
        `cp`.`necessita_aprov` AS `necessita_aprov`,
        `cp`.`ativo` AS `ativo`,
        `cp`.`id_cadast` AS `id_cadast`,
        `cp`.`id_atual` AS `id_atual`,
        `cp`.`id_aprovacao` AS `id_aprovacao`,
        `cp`.`valor_apropriado` AS `valor_apropriado`,
        `o`.`numero` AS `no_obra`
    FROM
        (`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
		AND (`cp`.`num_versao` = `o`.`num_versao`))));


CREATE TABLE IF NOT EXISTS  `con_obras_log_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `dt_hora_evento` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `usuario` char(20) NOT NULL DEFAULT '',
  `evento` char(30) NOT NULL DEFAULT '',
  `complemento` char(255) NOT NULL DEFAULT '',
  `obs` text NOT NULL,
  `envia_email` char(1) NOT NULL DEFAULT '',
  `email_enviado` char(1) NOT NULL DEFAULT '',
  `dt_hora_email` char(20) NOT NULL DEFAULT '',
  `ano_chamada` tinyint(2) NOT NULL DEFAULT '0',
  `no_chamada` mediumint(5) NOT NULL DEFAULT '0',
  `seq_log` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`obra`,`dt_hora_evento`,`usuario`,`evento`,`ano_chamada`,`no_chamada`,`seq_log`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE OR REPLACE
VIEW `view_con_chtel_ccredit_versao` AS
    SELECT 
	  `con_contrato_ccredit_versao`.`num_versao` AS `num_versao`,
        `con_contrato_ccredit_versao`.`usina` AS `usina`,
        `con_contrato_ccredit_versao`.`ano_contrato` AS `ano_contrato`,
        `con_contrato_ccredit_versao`.`num_contrato` AS `num_contrato`,
        `con_contrato_ccredit_versao`.`seq_pgto` AS `seq_pgto`,
        `con_contrato_ccredit_versao`.`seq` AS `seq`,
        `con_contrato_ccredit_versao`.`bandeira` AS `bandeira`,
        `con_contrato_ccredit_versao`.`no_cartao` AS `no_cartao`,
        `con_contrato_ccredit_versao`.`dt_transacao` AS `dt_transacao`,
        `con_contrato_ccredit_versao`.`valor` AS `valor`,
        `con_contrato_ccredit_versao`.`no_parcelas` AS `no_parcelas`,
        `con_contrato_ccredit_versao`.`no_autorizacao` AS `no_autorizacao`,
        `con_contrato_ccredit_versao`.`id_cadast` AS `id_cadast`,
        `con_contrato_ccredit_versao`.`id_atual` AS `id_atual`,
        `con_contrato_ccredit_versao`.`ano_chamada` AS `ano_chamada`,
        `con_contrato_ccredit_versao`.`num_chamada` AS `num_chamada`,
        `con_contrato_ccredit_versao`.`no_obra` AS `no_obra`
    FROM
        `con_contrato_ccredit_versao`;

CREATE OR REPLACE
VIEW `view_con_chtel_dep_versao` AS
    SELECT 
        `con_contrato_dep_versao`.`num_versao` AS `num_versao`,
        `con_contrato_dep_versao`.`usina` AS `usina`,
        `con_contrato_dep_versao`.`ano_contrato` AS `ano_contrato`,
        `con_contrato_dep_versao`.`num_contrato` AS `num_contrato`,
        `con_contrato_dep_versao`.`seq` AS `seq`,
        `con_contrato_dep_versao`.`dt_deposito` AS `dt_deposito`,
        `con_contrato_dep_versao`.`portador` AS `portador`,
        `con_contrato_dep_versao`.`bco_tomad` AS `bco_tomad`,
        `con_contrato_dep_versao`.`agencia_tomad` AS `agencia_tomad`,
        `con_contrato_dep_versao`.`no_terminal` AS `no_terminal`,
        `con_contrato_dep_versao`.`no_conta` AS `no_conta`,
        `con_contrato_dep_versao`.`valor_dep` AS `valor_dep`,
        `con_contrato_dep_versao`.`id_aprovacao` AS `id_aprovacao`,
        `con_contrato_dep_versao`.`seq_pgto` AS `seq_pgto`,
        `con_contrato_dep_versao`.`id_cadast` AS `id_cadast`,
        `con_contrato_dep_versao`.`id_atual` AS `id_atual`,
        `con_contrato_dep_versao`.`ano_chamada` AS `ano_chamada`,
        `con_contrato_dep_versao`.`num_chamada` AS `num_chamada`,
        `con_contrato_dep_versao`.`no_obra` AS `no_obra`
    FROM
        `con_contrato_dep_versao`;

CREATE OR REPLACE
VIEW `view_con_chtel_boleto_versao` AS
    SELECT 
        `c`.`num_versao` AS `num_versao`,
        `c`.`usina` AS `usina`,
        `c`.`ano_contrato` AS `ano_contrato`,
        `c`.`num_contrato` AS `num_contrato`,
        `c`.`seq_pgto` AS `seq_pgto`,
        `c`.`dt_vcto` AS `dt_vcto`,
        `c`.`dt_hr_imp` AS `dt_hr_imp`,
        `c`.`nosso_num` AS `nosso_num`,
        `c`.`linha_dig` AS `linha_dig`,
        `c`.`cod_barra` AS `cod_barra`,
        `c`.`dt_remessa` AS `dt_remessa`,
        `c`.`dt_liq` AS `dt_liq`,
        `c`.`vl_orig` AS `vl_orig`,
        `c`.`vl_liq` AS `vl_liq`,
        `c`.`id_liq` AS `id_liq`,
        `c`.`ano_chamada` AS `ano_chamada`,
        `c`.`num_chamada` AS `num_chamada`,
        `c`.`no_obra` AS `no_obra`,
        `c`.`seq` AS `seq`,
        `c`.`id_cadast` AS `id_cadast`,
        `c`.`id_atual` AS `id_atual`
    FROM
        `con_contrato_boleto_versao` `c`;

CREATE OR REPLACE
VIEW `view_con_chtel_cheque_versao` AS
    SELECT 
        `c`.`num_versao` AS `num_versao`,
        `c`.`usina` AS `usina`,
        `c`.`ano_contrato` AS `ano_contrato`,
        `c`.`num_contrato` AS `num_contrato`,
        `c`.`seq` AS `seq`,
        `c`.`bco_cheque` AS `bco_cheque`,
        `c`.`ag_cheque` AS `ag_cheque`,
        `c`.`conta_cheque` AS `conta_cheque`,
        `c`.`dv_conta` AS `dv_conta`,
        `c`.`num_cheque` AS `num_cheque`,
        `c`.`dt_receb` AS `dt_receb`,
        `c`.`vlr` AS `vlr`,
        `c`.`bom_para` AS `bom_para`,
        `c`.`id_cadast` AS `id_cadast`,
        `c`.`id_atual` AS `id_atual`,
        `c`.`obs` AS `obs`,
        `c`.`seq_pgto` AS `seq_pgto`,
        `c`.`ano_chamada` AS `ano_chamada`,
        `c`.`num_chamada` AS `num_chamada`,
        `c`.`no_obra` AS `no_obra`
    FROM
        `con_contrato_cheque_versao` `c`;

CREATE OR REPLACE
VIEW `view_con_chtel_dinheir_versao` AS
    SELECT 
        `c`.`num_versao` AS `num_versao`,
        `c`.`usina` AS `usina`,
        `c`.`ano_contrato` AS `ano_contrato`,
        `c`.`num_contrato` AS `num_contrato`,
        `c`.`seq_pgto` AS `seq_pgto`,
        `c`.`seq` AS `seq`,
        `c`.`num_recibo` AS `num_recibo`,
        `c`.`dt_pagamento` AS `dt_pagamento`,
        `c`.`valor` AS `valor`,
        `c`.`id_cadast` AS `id_cadast`,
        `c`.`id_atual` AS `id_atual`,
        `c`.`id_aprovacao` AS `id_aprovacao`,
        `c`.`ano_chamada` AS `ano_chamada`,
        `c`.`num_chamada` AS `num_chamada`,
        `c`.`no_obra` AS `no_obra`
    FROM
        `con_contrato_dinheir_versao` `c`;

CREATE TABLE IF NOT EXISTS  `con_obras_mp_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `cod` smallint(3) unsigned NOT NULL DEFAULT '0',
  `seleciona` char(1) NOT NULL DEFAULT '',
  `id_solic` char(19) NOT NULL DEFAULT '',
  `aprov_desc` char(1) NOT NULL DEFAULT '',
  `id_aprov` char(19) NOT NULL DEFAULT '',
  `ciente` char(1) NOT NULL DEFAULT '',
  PRIMARY KEY (`num_versao`, `usina`,`obra`,`cod`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_obras_trib_mun_versao` (
  `num_versao` int(11) NOT NULL,
  `usina_contrato` smallint(3) unsigned NOT NULL DEFAULT '0',
  `no_obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `num_contrato` mediumint(5) unsigned NOT NULL DEFAULT '0',
  `ano_contrato` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `no_obra_pref` char(25) NOT NULL DEFAULT '',
  `ccm_obra` char(25) NOT NULL DEFAULT '',
  `trib_iss` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `ret_iss` char(1) NOT NULL DEFAULT 'X',
  PRIMARY KEY (`num_versao`,`usina_contrato`,`no_obra`,`num_contrato`,`ano_contrato`,`usina`),
  KEY `IDX_CONTRATO` (`num_versao`,`usina_contrato`,`num_contrato`,`ano_contrato`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS  `con_aprov_versao` (
  `num_versao` int(11) NOT NULL,
  `chave` char(36) NOT NULL DEFAULT '',
  `tipo_aprov` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `usuario_req` char(10) NOT NULL DEFAULT '',
  `usuario_aprov` char(10) NOT NULL DEFAULT '',
  `dt_hora_solic` datetime DEFAULT NULL,
  `dt_hora_exec` datetime DEFAULT NULL,
  `complemento` char(100) NOT NULL DEFAULT '',
  `observacao` char(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`num_versao`,`chave`),
  KEY `IDX_PENDENTE` (`dt_hora_exec`,`tipo_aprov`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE OR REPLACE
VIEW `view_obras_pendentes_aprov_versao` AS
    (SELECT 
        `t`.`usina` AS `usina`, `t`.`no_obra` AS `numero`, `t`.`num_versao` AS `versao`
    FROM
        `con_proposta_item_versao` `t`
    WHERE
        (`t`.`aprov_verbal` = 'N')
    GROUP BY `t`.`usina` , `t`.`no_obra`, `t`.`num_versao`) UNION (SELECT 
        `b`.`usina` AS `usina`, `b`.`no_obra` AS `numero`, `b`.`num_versao` AS `versao`
    FROM
        `con_prop_bomba_versao` `b`
    WHERE
        (`b`.`aprov_verbal` = 'S')
    GROUP BY `b`.`usina` , `b`.`no_obra`, `b`.`num_versao`) UNION (SELECT 
        `obras`.`usina` AS `usina`, `obras`.`numero` AS `numero`, `obras`.`num_versao` AS `versao`
    FROM
        (`con_obras_tx_versao` `tx`
        JOIN `con_obras_versao` `obras` ON (((`tx`.`usina` = `obras`.`obra_usina`)
            AND (`tx`.`obra` = `obras`.`numero`) and (`tx`.`num_versao` = `obras`.`num_versao`))))
    WHERE
        (`tx`.`aprov_desc` = 'N')
    GROUP BY `obras`.`usina` , `obras`.`numero`, `obras`.`num_versao`) UNION (SELECT 
        `obras`.`usina` AS `usina`, `obras`.`numero` AS `numero`, `obras`.`num_versao` AS `versao`
    FROM
        (`con_aprov_versao` `aprovacaocomercial`
        JOIN `con_obras_versao` `obras` ON (((SUBSTRING_INDEX(`aprovacaocomercial`.`complemento`, '/', 1) = `obras`.`usina`)
            AND (SUBSTRING_INDEX(SUBSTRING_INDEX(`aprovacaocomercial`.`complemento`, '/', -(1)), '-', 1) = `obras`.`ano_chamada`)
            AND (SUBSTRING_INDEX(SUBSTRING_INDEX(`aprovacaocomercial`.`complemento`, '/', 2), '/', -(1)) = `obras`.`no_chamada`))
            and (`aprovacaocomercial`.`num_versao` = `obras`.`num_versao`)))
    WHERE
        (((`aprovacaocomercial`.`dt_hora_exec` = '1000-01-01 00:00:00')
            OR ISNULL(`aprovacaocomercial`.`dt_hora_exec`))
            AND (`aprovacaocomercial`.`tipo_aprov` IN (1 , 2)))
    GROUP BY `obras`.`usina` , `obras`.`numero`, `obras`.`num_versao`);

CREATE OR REPLACE
VIEW `view_pendentes_aprov_finan_wg_versao` AS
    (SELECT 
        `o`.`usina` AS `usina`,
        `o`.`numero` AS `numero`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `o`.`num_versao` AS `num_versao`
    FROM
        (((`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
            AND (`cp`.`num_versao` = `o`.`num_versao`))))
        JOIN `con_contrato_versao` `c` ON (((`cp`.`usina` = `c`.`usina`)
            AND (`cp`.`ano_contrato` = `c`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `c`.`num_contrato`)
            AND (`cp`.`num_versao` = `c`.`num_versao`))))
        JOIN `con_contrato_boleto_versao` `fp` ON (((`fp`.`usina` = `cp`.`usina`)
            AND (`fp`.`num_contrato` = `cp`.`num_contrato`)
            AND (`fp`.`ano_contrato` = `cp`.`ano_contrato`)
            AND (`fp`.`seq_pgto` = `cp`.`seq`)
            AND (`fp`.`num_versao` = `cp`.`num_versao`))))
    WHERE
        ((`cp`.`id_aprovacao` = '')
            AND (`cp`.`necessita_aprov` = 'S')
            AND (`cp`.`ativo` = 'S')
            AND (`c`.`status` <> 9138)
            AND (`fp`.`nosso_num` <> '')
            AND ISNULL(`fp`.`dt_liq`))) UNION (SELECT 
        `o`.`usina` AS `usina`,
        `o`.`numero` AS `numero`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `o`.`num_versao` AS `num_versao`
    FROM
        (((`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
            AND (`cp`.`num_versao` = `o`.`num_versao`))))
        JOIN `con_contrato_versao` `c` ON (((`cp`.`usina` = `c`.`usina`)
            AND (`cp`.`ano_contrato` = `c`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `c`.`num_contrato`)
            AND (`cp`.`num_versao` = `c`.`num_versao`))))
        JOIN `con_contrato_dinheir_versao` `fp` ON (((`fp`.`usina` = `cp`.`usina`)
            AND (`fp`.`num_contrato` = `cp`.`num_contrato`)
            AND (`fp`.`ano_contrato` = `cp`.`ano_contrato`)
            AND (`fp`.`seq_pgto` = `cp`.`seq`)
            AND (`fp`.`num_versao` = `cp`.`num_versao`))))
    WHERE
        ((`cp`.`id_aprovacao` = '')
            AND (`cp`.`necessita_aprov` = 'S')
            AND (`cp`.`ativo` = 'S')
            AND (`c`.`status` <> 9138))) UNION (SELECT 
        `o`.`usina` AS `usina`,
        `o`.`numero` AS `numero`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `o`.`num_versao` AS `num_versao`
    FROM
        (((`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
            AND (`cp`.`num_versao` = `o`.`num_versao`))))
        JOIN `con_contrato_versao` `c` ON (((`cp`.`usina` = `c`.`usina`)
            AND (`cp`.`ano_contrato` = `c`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `c`.`num_contrato`)
            AND (`cp`.`num_versao` = `c`.`num_versao`))))
        JOIN `con_contrato_dep_versao` `fp` ON (((`fp`.`usina` = `cp`.`usina`)
            AND (`fp`.`num_contrato` = `cp`.`num_contrato`)
            AND (`fp`.`ano_contrato` = `cp`.`ano_contrato`)
            AND (`fp`.`seq_pgto` = `cp`.`seq`)
            AND (`fp`.`num_versao` = `cp`.`num_versao`))))
    WHERE
        ((`cp`.`id_aprovacao` = '')
            AND (`cp`.`necessita_aprov` = 'S')
            AND (`cp`.`ativo` = 'S')
            AND (`c`.`status` <> 9138)
            AND (`fp`.`valor_dep` <> 0))) UNION (SELECT 
        `o`.`usina` AS `usina`,
        `o`.`numero` AS `numero`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `o`.`num_versao` AS `num_versao`
    FROM
        (((`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
            AND (`cp`.`num_versao` = `o`.`num_versao`))))
        JOIN `con_contrato_versao` `c` ON (((`cp`.`usina` = `c`.`usina`)
            AND (`cp`.`ano_contrato` = `c`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `c`.`num_contrato`)
            AND (`cp`.`num_versao` = `c`.`num_versao`))))
        JOIN `con_contrato_ccredit_versao` `fp` ON (((`fp`.`usina` = `cp`.`usina`)
            AND (`fp`.`num_contrato` = `cp`.`num_contrato`)
            AND (`fp`.`ano_contrato` = `cp`.`ano_contrato`)
            AND (`fp`.`seq_pgto` = `cp`.`seq`)
            AND (`fp`.`num_versao` = `cp`.`num_versao`))))
    WHERE
        ((`cp`.`id_aprovacao` = '')
            AND (`cp`.`necessita_aprov` = 'S')
            AND (`cp`.`ativo` = 'S')
            AND (`c`.`status` <> 9138))) UNION (SELECT 
        `o`.`usina` AS `usina`,
        `o`.`numero` AS `numero`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `o`.`num_versao` AS `num_versao`
    FROM
        (((`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
             AND (`cp`.`num_versao` = `o`.`num_versao`))))
        JOIN `con_contrato_versao` `c` ON (((`cp`.`usina` = `c`.`usina`)
            AND (`cp`.`ano_contrato` = `c`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `c`.`num_contrato`)
            AND (`cp`.`num_versao` = `c`.`num_versao`))))
        JOIN `con_contrato_cheque_versao` `fp` ON (((`fp`.`usina` = `cp`.`usina`)
            AND (`fp`.`num_contrato` = `cp`.`num_contrato`)
            AND (`fp`.`ano_contrato` = `cp`.`ano_contrato`)
            AND (`fp`.`seq_pgto` = `cp`.`seq`)
            AND (`fp`.`num_versao` = `cp`.`num_versao`))))
    WHERE
        ((`cp`.`id_aprovacao` = '')
            AND (`cp`.`necessita_aprov` = 'S')
            AND (`cp`.`ativo` = 'S')
            AND (`c`.`status` <> 9138)
            AND (`fp`.`vlr` <> 0))) UNION (SELECT 
        `o`.`usina` AS `usina`,
        `o`.`numero` AS `numero`,
        `cp`.`ano_contrato` AS `ano_contrato`,
        `cp`.`num_contrato` AS `num_contrato`,
        `o`.`num_versao` AS `num_versao`
    FROM
        ((`con_contrato_pag_versao` `cp`
        JOIN `con_obras_versao` `o` ON (((`cp`.`usina` = `o`.`usina`)
            AND (`cp`.`ano_contrato` = `o`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `o`.`no_contrato`)
            AND (`cp`.`num_versao` = `o`.`num_versao`))))
        JOIN `con_contrato_versao` `c` ON (((`cp`.`usina` = `c`.`usina`)
            AND (`cp`.`ano_contrato` = `c`.`ano_contrato`)
            AND (`cp`.`num_contrato` = `c`.`num_contrato`)
            AND (`cp`.`num_versao` = `c`.`num_versao`))))
    WHERE
        ((`cp`.`id_aprovacao` = '')
            AND (`cp`.`necessita_aprov` = 'S')
            AND (`cp`.`ativo` = 'S')
            AND (`c`.`status` <> 9138)
            AND (`cp`.`forma` = 'CT')));

CREATE OR REPLACE
VIEW `view_pendentes_aprov_finan_versao` AS
    SELECT 
        `view_pendentes_aprov_finan_wg_versao`.`usina` AS `usina`,
        `view_pendentes_aprov_finan_wg_versao`.`numero` AS `numero`,
        `view_pendentes_aprov_finan_wg_versao`.`ano_contrato` AS `ano_contrato`,
        `view_pendentes_aprov_finan_wg_versao`.`num_contrato` AS `num_contrato`,
        `view_pendentes_aprov_finan_wg_versao`.`num_versao` AS `num_versao`
    FROM
        `view_pendentes_aprov_finan_wg_versao`
    GROUP BY `view_pendentes_aprov_finan_wg_versao`.`usina` , `view_pendentes_aprov_finan_wg_versao`.`numero` , `view_pendentes_aprov_finan_wg_versao`.`ano_contrato` , `view_pendentes_aprov_finan_wg_versao`.`num_contrato`, `view_pendentes_aprov_finan_wg_versao`.`num_versao`;

CREATE TABLE IF NOT EXISTS  `con_taxa_extra_versao` (
  `num_versao` int(11) NOT NULL,
  `usina` smallint(3) unsigned NOT NULL DEFAULT '0',
  `seq` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `dt_inicio_valid` date NOT NULL DEFAULT '0000-00-00',
  `data_inicio` date NOT NULL DEFAULT '0000-00-00',
  `data_fim` date NOT NULL DEFAULT '0000-00-00',
  `cod_mun_obra` smallint(3) unsigned NOT NULL DEFAULT '0',
  `obra` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `taxa_adicional` char(50) NOT NULL DEFAULT '',
  `quando_de` char(50) NOT NULL DEFAULT '',
  `quando_oper` char(4) NOT NULL DEFAULT '',
  `quando_ate` char(50) NOT NULL DEFAULT '',
  `horario_antes` char(20) NOT NULL DEFAULT '',
  `horario` char(20) NOT NULL DEFAULT '',
  `cobrar_volume` char(20) NOT NULL DEFAULT '',
  `volume` char(4) NOT NULL DEFAULT '',
  `tipo_valor` char(2) NOT NULL DEFAULT '',
  `tipo_pessoa` char(1) NOT NULL DEFAULT 'T',
  `valor` float(7,2) unsigned NOT NULL DEFAULT '0.00',
  `valor_por` char(12) NOT NULL DEFAULT '',
  `da_pedra` char(20) NOT NULL DEFAULT '',
  `para_pedra` char(20) NOT NULL DEFAULT '',
  `da_resistenc` char(30) NOT NULL DEFAULT '',
  `para_resistenc` char(30) NOT NULL DEFAULT '',
  `do_slump` smallint(3) unsigned NOT NULL DEFAULT '0',
  `para_slump` smallint(3) unsigned NOT NULL DEFAULT '0',
  `texto` char(255) NOT NULL DEFAULT '',
  `texto_mesclado` char(255) NOT NULL DEFAULT '',
  `id_cadast` char(19) NOT NULL DEFAULT '',
  `id_atual` char(19) NOT NULL DEFAULT '',
  `acima_de` smallint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`num_versao`,`usina`,`seq`,`obra`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE table_schema='topsys' AND table_name='con_nf_complemento' AND column_name='versao_contrato'
) > 0, 'SELECT 1;', 'ALTER TABLE con_nf_complemento ADD COLUMN `versao_contrato` SMALLINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `obs_mold_remota`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

DELIMITER $$
DROP TRIGGER IF EXISTS `topsys`.`con_contrato_versao_AFTER_UPDATE`$$
CREATE DEFINER = CURRENT_USER TRIGGER `topsys`.`con_contrato_versao_AFTER_UPDATE` AFTER UPDATE ON `con_contrato_versao` FOR EACH ROW
BEGIN
	DECLARE ParametroNome CHAR(40);
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
END$$
DELIMITER ;
