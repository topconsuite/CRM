/* TC-1170 | TC-1024 | TC-1227 */

CREATE TABLE IF NOT EXISTS topsys.con_chtel_cobranca (
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
   PRIMARY KEY (`usina`,`ano_chamada`,`num_chamada`)
);

CREATE TABLE IF NOT EXISTS topsys.con_chtel_faturamento LIKE topsys.con_chtel_cobranca;
CREATE TABLE IF NOT EXISTS topsys.con_chtel_resp_solid LIKE topsys.con_chtel_cobranca;

INSERT IGNORE INTO topsys.con_chtel_faturamento
SELECT usina,ano_chamada,num_chamada
,fat_idem_client,fat_cnpj_cpf,fat_razao,fat_nome,fat_email
,fat_ie,fat_ccm,fat_rg,fat_org_uf_emi
,fat_cep,fat_end,fat_num,fat_compl,fat_bairro,fat_mun
FROM topsys.con_chtel
WHERE fat_cnpj_cpf<>'' OR fat_end<>'' or fat_email<>'';

REPLACE INTO topsys.con_chtel_faturamento
SELECT p.usina,p.ano_chamada,p.num_chamada
,IF(LENGTH(l.cnpj_cpf)=14,'J','F'),l.cnpj_cpf,l.razao,l.nome,l.email
,l.ie,l.ccm,l.rg,l.emissor
,l.cep,l.end,l.num,l.compl,l.bairro,l.mun
FROM con_chtel p
INNER JOIN con_obras o ON o.usina=p.usina AND o.ano_chamada=p.ano_chamada AND o.no_chamada=p.num_chamada
INNER JOIN con_contrato c ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
INNER JOIN ger_local l ON c.interv=l.interv AND c.local_fatur=l.seq;

REPLACE INTO topsys.con_chtel_cobranca
SELECT p.usina,p.ano_chamada,p.num_chamada
,IF(LENGTH(l.cnpj_cpf)=14,'J','F'),l.cnpj_cpf,l.razao,l.nome,l.email
,l.ie,l.ccm,l.rg,l.emissor
,l.cep,l.end,l.num,l.compl,l.bairro,l.mun
FROM con_chtel p
INNER JOIN con_obras o ON o.usina=p.usina AND o.ano_chamada=p.ano_chamada AND o.no_chamada=p.num_chamada
INNER JOIN con_contrato c ON c.usina=o.usina AND c.ano_contrato=o.ano_contrato AND c.num_contrato=o.no_contrato
INNER JOIN ger_local l ON c.interv=l.interv AND c.local_cobranca=l.seq;