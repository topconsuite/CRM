using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ObraVersao : ObraBase<ObraVersao, ContratoVersao, PropostaVersao, ObraFrente, ObraTracoVersao, ObraBombaVersao, ObraTaxaVersao, ObraLogVersao, ObraDemaisServicosVersao, PropostaPagamentoVersao, ContratoPagamentoVersao, ContratoPagamentoDetalheVersao, ObraTributacaoMunicipalVersao, ObraMensagemPadraoVersao, ObraReajusteVersao, ObraIndicadorVersao>
    {
        public int NumeroVersao { get; set; }

        public string getChaveVersaoToString()
        {
            return NumeroVersao + "-" + UsinaCodigo + "-" + Numero;
        }
    }
}
