CREATE TABLE IF NOT EXISTS topsys.con_webhooks (
  evento varchar(128) NOT NULL,
  url varchar(1024) NOT NULL,
  payload text NOT NULL,
  filePath varchar(2048) DEFAULT NULL,
  alertEmails varchar(2048) DEFAULT NULL,
  dtSendAfter datetime DEFAULT NULL,
  headers text,
  id int(11) NOT NULL AUTO_INCREMENT,
  localGuid varchar(36) DEFAULT NULL,
  attempt int(11) NOT NULL DEFAULT '0',
  dtLastAttempt datetime DEFAULT NULL,
  status varchar(32) NOT NULL DEFAULT 'Pending',
  dtEnvio datetime DEFAULT NULL,
  httpResult int(11) DEFAULT NULL,
  error varchar(2048) DEFAULT NULL,
  webHookId varchar(256) DEFAULT NULL,
  dtCriacao datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


CREATE TABLE IF NOT EXISTS topsys.con_webhook_events (
  evento varchar(128) NOT NULL,
  url varchar(1024) NOT NULL,
  alertEmails varchar(2048) DEFAULT NULL,
  dtCriacao datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  dtAtualizacao datetime DEFAULT NULL,
  PRIMARY KEY (evento)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;