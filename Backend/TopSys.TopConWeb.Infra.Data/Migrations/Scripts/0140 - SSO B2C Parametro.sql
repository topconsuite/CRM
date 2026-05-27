/* SSO Topcon Identity (Azure AD B2C) — per-deploy feature gate + B2C tenant config.
   Seeds a single row in topsys.parametros_sso with tipo_provedor = 1 (B2C).

   Columns used by HandleB2CGrant / B2CTokenValidator:
     sso_habilitado  feature gate; flip to true at cutover.
     dominio         B2C login subdomain ("topconidentity.b2clogin.com").
     tenant_id       B2C directory GUID; combined with dominio to form
                     the trusted JWT issuer (https://{dominio}/{tenant_id}/v2.0/).
     client_id       App Reg client ID; validated as the token audience.

   Other Sso:* (metadata URL, accepted user flows, identity URL, module name)
   are by-environment / lists and live in Web.config with transforms. */

SET @preparedStatement = (SELECT IF(
    (SELECT COUNT(*) FROM topsys.parametros_sso WHERE tipo_provedor = 1) > 0,
    'SELECT 1;',
    'INSERT INTO topsys.parametros_sso
        (id, sso_habilitado, dominio, tipo_provedor, tenant_id, client_id, url_redirecionamento)
     VALUES
        (UUID(),
         false,
         \'topconidentity.b2clogin.com\',
         1,
         \'eb415800-08f8-4c8c-a912-8e2246ff2408\',
         \'1d8965e0-2e10-45f0-a8dc-a6fb9ac06fc4\',
         \'\');'
));
PREPARE insertIfNotExists FROM @preparedStatement;
EXECUTE insertIfNotExists;
DEALLOCATE PREPARE insertIfNotExists;
