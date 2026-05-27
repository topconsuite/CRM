 CREATE TABLE IF NOT EXISTS `topsys`.`parametros_sso` (
    id char(36) NOT NULL DEFAULT '',
    sso_habilitado BOOLEAN NOT NULL DEFAULT false,               
    dominio VARCHAR(90) NOT NULL DEFAULT '',        
    tipo_provedor INT NOT NULL DEFAULT 0,                    
    tenant_id char(36) NOT NULL DEFAULT '',                           
    client_id char(36) NOT NULL DEFAULT '',                           
    url_redirecionamento VARCHAR(255) NOT NULL DEFAULT '',
    PRIMARY KEY (id)
);