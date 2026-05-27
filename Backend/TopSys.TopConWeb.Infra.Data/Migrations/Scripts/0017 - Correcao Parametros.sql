/* TC-2280 */
UPDATE IGNORE `topsys`.`ger_parametro` SET `grupo`='Topcon' WHERE `grupo`='web' and`chave`='NaoConsideraTodasBombas';
UPDATE IGNORE `topsys`.`ger_parametro` SET `grupo`='Topcon' WHERE `grupo`='web' and`chave`='RelContrato';