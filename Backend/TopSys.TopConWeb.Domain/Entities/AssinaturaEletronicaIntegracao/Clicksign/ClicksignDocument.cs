using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.SharedKernel.Helpers;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign
{
    public class ClicksignDocument
    {
        public ClicksignDocument(SolicitacaoAssinaturaEletronicaClicksign solicitacaoAssinaturaEletronicaClicksign, ClicksignParametros clicksignParametro)
        {
            Path = $"/{solicitacaoAssinaturaEletronicaClicksign.ContratoUsina}-{solicitacaoAssinaturaEletronicaClicksign.ContratoAno}-{solicitacaoAssinaturaEletronicaClicksign.ContratoNumero}.pdf";        
            File = "data:application/pdf;base64," + solicitacaoAssinaturaEletronicaClicksign.ContratoPdf.ConvertStreamToBase64();
            Deadline = DateTime.Now.AddDays(clicksignParametro.PrazoLimiteAssinatura);
        }

        public string Path { get; set; }
        public string File { get; set; }
        public DateTime Deadline { get; set; }
        public Guid Id { get; set; }
    }
}
