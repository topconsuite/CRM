/*OP-6224 - Lucas Matheus */

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_ponto_registro' AND column_name='cpf'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_ponto_registro ADD COLUMN cpf CHAR(11) NOT NULL DEFAULT "";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsys' AND table_name='con_ponto_registro' AND column_name='tipo'
) > 0, 'SELECT 1;', 'ALTER TABLE topsys.con_ponto_registro ADD COLUMN tipo CHAR(1) NOT NULL DEFAULT "0";'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

INSERT IGNORE INTO `topsys`.`con_ponto_relogio_modelo` (`id_modelo`, `marca`, `modelo`) VALUES (8, 'Henry', 'Hexa Advanced Novo (CPF)');

REPLACE INTO `topsys`.`usr_programa` (`Aplicativo`, `num`, `Titulo`, `Funcao`, `menu`, `seq_menu`) VALUES ('CON', 7036, 'ID Externo Interveniente', 'A', 1, 2);