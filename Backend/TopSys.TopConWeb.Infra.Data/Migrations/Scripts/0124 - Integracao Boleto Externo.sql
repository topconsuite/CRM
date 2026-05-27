SET @preparedStatement = (SELECT IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema='topsysarquivos' AND table_name='arquivos' AND column_name='nome_arquivo'
) > 0, 'SELECT 1;', 'ALTER TABLE topsysarquivos.arquivos ADD COLUMN `nome_arquivo` CHAR(70) NOT NULL DEFAULT "" AFTER `caminho_arquivo`;'));
PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;