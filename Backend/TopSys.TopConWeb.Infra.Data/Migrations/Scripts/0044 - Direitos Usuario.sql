/* Ticket-373350 */

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=6999
, Titulo='Solicita Pagamento'
, Funcao='IAER'
, menu=2
, seq_menu=4;

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7000
, Titulo='Versionamento de Contrato'
, Funcao='IAER'
, menu=1
, seq_menu=8;

REPLACE INTO topsys.usr_programa
SET Aplicativo='WEB'
, num=7001
, Titulo='Conf. Aprovacao Comercial'
, Funcao='IAER'
, menu=1
, seq_menu=9;
