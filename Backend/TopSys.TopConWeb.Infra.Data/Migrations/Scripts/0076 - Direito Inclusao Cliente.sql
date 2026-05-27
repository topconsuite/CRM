/* OP-4969 */
UPDATE `topsys`.`usr_programa` SET `Funcao` = 'IA' WHERE (`Aplicativo` = 'WEB') and (`num` = '6001');

UPDATE usr_dir_grupo SET dir_inc='S' WHERE sigla='WEB' AND num_prog=6001 AND acesso='S';
UPDATE usr_dir_grupou SET dir_inc='S' WHERE sigla='WEB' AND num_prog=6001 AND acesso='S';