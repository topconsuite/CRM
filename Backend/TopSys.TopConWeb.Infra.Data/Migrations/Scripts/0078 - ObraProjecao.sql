/*OP-5051 Livia 13/09/2024*/
CREATE TABLE IF NOT EXISTS `con_obras_projecao` (
  `usina` smallint(3) NOT NULL,
  `no_obra` mediumint(6) NOT NULL,
  `ano_chamada` tinyint(2) NOT NULL,
  `no_chamada` mediumint(5) NOT NULL,
  `periodo` date NOT NULL,
  `volume_m3` float(5,1) NOT NULL,
  PRIMARY KEY (`usina`,`no_obra`,`ano_chamada`,`no_chamada`,`periodo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;