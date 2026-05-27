/* TC-3936 */
UPDATE IGNORE `topsys`.`usr_programa` SET `Aplicativo` = "WEB" WHERE `num` IN (6998, 6118, 6127, 6309, 6156, 6272, 6268, 6149) AND `Aplicativo` = "CON";

DELETE FROM `topsys`.`usr_programa` WHERE `num` IN (6108,6107,6111,6139,6996,6997,6133,6144,6288) AND `Aplicativo` = "CON";

UPDATE IGNORE `topsys`.`usr_dir_grupou` SET `Sigla` = "WEB" WHERE `Num_Prog` IN (6998, 6118, 6127, 6309, 6156, 6272, 6268, 6149) AND `Sigla` = "CON";

UPDATE IGNORE `topsys`.`usr_dir_grupo` SET `Sigla` = "WEB" WHERE `Num_Prog` IN (6998, 6118, 6127, 6309, 6156, 6272, 6268, 6149) AND `Sigla` = "CON";

DELETE FROM `topsys`.`usr_dir_grupou`
WHERE `Num_Prog` IN (6108,6107,6111,6139,6996,6997,6133,6144,6288) AND `Sigla` = "CON";

DELETE FROM `topsys`.`usr_dir_grupo`
WHERE `Num_Prog` IN (6108,6107,6111,6139,6996,6997,6133,6144,6288) AND `Sigla` = "CON";

DELETE FROM `topsys`.`usr_prog_modulo`
WHERE `num_prog` IN (6108,6107,6111,6139,6156,6127,6118,6996,6998,6268,6272,6997,6149,6133,6144,6288,6309) AND `aplicativo` = "CON";

DELETE FROM `topsys`.`usr_prog_hist_acesso`
WHERE `num_prog` IN (6108,6107,6111,6139,6156,6127,6118,6996,6998,6268,6272,6997,6149,6133,6144,6288,6309) AND `aplicativo` = "CON";

DELETE FROM `topsys`.`usr_prog_favoritos`
WHERE `num_prog` IN (6108,6107,6111,6139,6156,6127,6118,6996,6998,6268,6272,6997,6149,6133,6144,6288,6309) AND `aplicativo` = "CON";