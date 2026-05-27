using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;

namespace TopSys.TopConWeb.Domain.Interfaces.Integrations
{
    public interface IAssinaturaEletronicaIntegrationService
    {
        /// <summary>
        /// Define o contexto da Usina para resolução das credenciais ClickSign.
        /// Deve ser chamado antes das operações de integração para ativar o fallback por Usina.
        /// </summary>
        void ConfigurarContextoUsina(int? usinaId);

        ClicksignDocument CreateDocument(ClicksignDocument clicksignDocument);
        ClicksignSigner CreateSigner(ClicksignSigner clicksignSigner);
        ClicksignSigner CreateSignerHired(ClicksignSigner clicksignSignerHired);
        ClicksignSigner CreateSignerCoResponsible(ClicksignSigner clicksignSignerCoResponsible);
        ClicksignSigner AddSignerToDocument(ClicksignDocument clicksignDocument, ClicksignSigner clicksignSigner, bool assinaturaContratada, bool assinaturaVendedor, bool assinaturaTestemunha);
        ClicksignSigner AddSignerCoResponsibleToDocument(ClicksignDocument clicksignDocument, ClicksignSigner clicksignSigner); 
        void RequestSignaturesEmail(ClicksignSigner clicksignSigner);
        void RequestSignaturesWhatsApp(ClicksignSigner clicksignSigner);
        void RequestDocumentCancelClicksign(Guid documentId);
        string GetClicksignUrlDocumentSigned(Guid documentId);
        byte[] DownloadFile(string FileUrl);

    }
}
