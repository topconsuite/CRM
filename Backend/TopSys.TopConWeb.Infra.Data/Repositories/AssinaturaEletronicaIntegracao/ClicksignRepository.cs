using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Infra.Data.Repositories.AssinaturaEletronicaIntegracao
{
    public class ClicksignRepository : IClicksignRepository
    {
        private readonly AppDataContext _context;
        private readonly IdentityHelperService _identityHelperService;

        public ClicksignRepository(AppDataContext context, IdentityHelperService identityHelperService)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public void AtualizarParametros(ClicksignParametros clicksignParametro)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT id FROM con_integracao_clicksign LIMIT 1");
            var id  = _context.Database.Connection.QueryFirstOrDefault<Guid>(sql.ToString());
            clicksignParametro.Id = id == Guid.Empty ? Guid.NewGuid() : id;

            sql = new StringBuilder();
            sql.Append($"REPLACE INTO con_integracao_clicksign SET ");
            sql.Append($"id =@{nameof(ClicksignParametros.Id)}, ");
            sql.Append($"corpo_email=@{nameof(ClicksignParametros.CorpoEmail)}, ");
            sql.Append($"obriga_documento_oficial=@{nameof(ClicksignParametros.ObrigaDocumentoOficial)}, ");
            sql.Append($"obriga_selfie=@{nameof(ClicksignParametros.ObrigaSelfie)}, ");
            sql.Append($"obriga_assinatura_manuscrita=@{nameof(ClicksignParametros.ObrigaAssinaturaManuscrita)}, ");
            sql.Append($"obriga_reconhecimento_facial=@{nameof(ClicksignParametros.ObrigaReconhecimentoFacial)}, ");
            sql.Append($"notifica_cliente_confirmacao_assinatura=@{nameof(ClicksignParametros.NotificaClienteNaConfirmacaoDeAssinatura)},");
            if (clicksignParametro.PrazoLimiteAssinatura != 0) {
                sql.Append($"prazo_limite_assinatura=@{nameof(ClicksignParametros.PrazoLimiteAssinatura)},");
            }
            sql.Append($"id_atual='{StringHelper.GetIDD(_identityHelperService.GetUserName())}', ");
            sql.Append($"envia_assinatura_contratada=@{nameof(ClicksignParametros.EnviaAssinaturaContratada)}, ");
            sql.Append($"envia_assinatura_responsavel_solidario=@{nameof(ClicksignParametros.EnviaAssinaturaResponsavelSolidario)}, ");
            sql.Append($"email_contratada=@{nameof(ClicksignParametros.EmailContratada)}, ");
            sql.Append($"ddd_wpp_contratada=@{nameof(ClicksignParametros.DDDContratada)}, ");
            sql.Append($"telefone_wpp_contratada=@{nameof(ClicksignParametros.TelefoneContratada)}, ");
            sql.Append($"metodo_envio_assinatura_contratada=@{nameof(ClicksignParametros.MetodoEnvioAssinaturaContratada)}, ");
            sql.Append($"metodo_autenticacao_contratada=@{nameof(ClicksignParametros.MetodoAutenticacaoContratada)}, ");
            sql.Append($"permite_assinatura_whatsapp=@{nameof(ClicksignParametros.PermiteAssinaturaWhatsApp)}, ");
            sql.Append($"primeira_testemunha=@{nameof(ClicksignParametros.PrimeiraTestemunha)}, ");
            sql.Append($"segunda_testemunha=@{nameof(ClicksignParametros.SegundaTestemunha)}");

            _context.Database.Connection.Execute(sql.ToString(), clicksignParametro);

        }

        public ClicksignParametros ObterParametros()
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT");
            sql.Append($" id {nameof(ClicksignParametros.Id)}");
            sql.Append($", corpo_email {nameof(ClicksignParametros.CorpoEmail)}");
            sql.Append($", obriga_documento_oficial {nameof(ClicksignParametros.ObrigaDocumentoOficial)}");
            sql.Append($", obriga_selfie {nameof(ClicksignParametros.ObrigaSelfie)}");
            sql.Append($", obriga_assinatura_manuscrita {nameof(ClicksignParametros.ObrigaAssinaturaManuscrita)}");
            sql.Append($", obriga_reconhecimento_facial {nameof(ClicksignParametros.ObrigaReconhecimentoFacial)}");
            sql.Append($", notifica_cliente_confirmacao_assinatura {nameof(ClicksignParametros.NotificaClienteNaConfirmacaoDeAssinatura)}");
            sql.Append($", prazo_limite_assinatura {nameof(ClicksignParametros.PrazoLimiteAssinatura)}");
            sql.Append($", envia_assinatura_contratada {nameof(ClicksignParametros.EnviaAssinaturaContratada)}");
            sql.Append($", envia_assinatura_responsavel_solidario {nameof(ClicksignParametros.EnviaAssinaturaResponsavelSolidario)}");
            sql.Append($", email_contratada {nameof(ClicksignParametros.EmailContratada)}");
            sql.Append($", ddd_wpp_contratada {nameof(ClicksignParametros.DDDContratada)}");
            sql.Append($", telefone_wpp_contratada {nameof(ClicksignParametros.TelefoneContratada)}");
            sql.Append($", metodo_envio_assinatura_contratada {nameof(ClicksignParametros.MetodoEnvioAssinaturaContratada)}");
            sql.Append($", metodo_autenticacao_contratada {nameof(ClicksignParametros.MetodoAutenticacaoContratada)}");
            sql.Append($", permite_assinatura_whatsapp {nameof(ClicksignParametros.PermiteAssinaturaWhatsApp)}");
            sql.Append($", primeira_testemunha {nameof(ClicksignParametros.PrimeiraTestemunha)}");
            sql.Append($", segunda_testemunha {nameof(ClicksignParametros.SegundaTestemunha)}");
            sql.Append($" FROM topsys.con_integracao_clicksign LIMIT 1");

            return _context.Database.Connection.QueryFirstOrDefault<ClicksignParametros>(sql.ToString());
        }

        public IEnumerable<ContratoClicksignEnvio> ListarContratoClicksignEnvios(int usinaContrato, int anoContrato, int numContrato)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT");
            sql.Append($" id {nameof(ContratoClicksignEnvio.Id)}");
            sql.Append($", usina {nameof(ContratoClicksignEnvio.ContratonUsina)}");
            sql.Append($", ano_contrato {nameof(ContratoClicksignEnvio.ContratoAno)}");
            sql.Append($", num_contrato {nameof(ContratoClicksignEnvio.ContratoNumero)}");
            sql.Append($", id_clicksign {nameof(ContratoClicksignEnvio.IdClicksign)}");
            sql.Append($", data_envio {nameof(ContratoClicksignEnvio.DataEnvio)}");
            sql.Append($", data_assinatura {nameof(ContratoClicksignEnvio.DataAssinatura)}");
            sql.Append($", data_cancelamento {nameof(ContratoClicksignEnvio.DataCancelamento)}");
            sql.Append($", id_envio {nameof(ContratoClicksignEnvio.IdEnvio)}");
            sql.Append($", id_cancelamento {nameof(ContratoClicksignEnvio.IdCancelamento)}");
            sql.Append($" FROM topsys.con_clicksign_envios");
            sql.Append($" WHERE usina=@{nameof(usinaContrato)}");
            sql.Append($" AND ano_contrato=@{nameof(anoContrato)}");
            sql.Append($" AND num_contrato=@{nameof(numContrato)}");

            var filtro = new
            {
                usinaContrato,
                anoContrato,
                numContrato
            };

            return _context.Database.Connection.Query<ContratoClicksignEnvio>(sql.ToString(), filtro);
        }

        public ContratoClicksignEnvio ObterUltimoContratoAssinadoClicksignEnvio(int usinaContrato, int anoContrato, int numContrato)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT");
            sql.Append($" id {nameof(ContratoClicksignEnvio.Id)}");
            sql.Append($", usina {nameof(ContratoClicksignEnvio.ContratonUsina)}");
            sql.Append($", ano_contrato {nameof(ContratoClicksignEnvio.ContratoAno)}");
            sql.Append($", num_contrato {nameof(ContratoClicksignEnvio.ContratoNumero)}");
            sql.Append($", id_clicksign {nameof(ContratoClicksignEnvio.IdClicksign)}");
            sql.Append($", data_envio {nameof(ContratoClicksignEnvio.DataEnvio)}");
            sql.Append($", data_assinatura {nameof(ContratoClicksignEnvio.DataAssinatura)}");
            sql.Append($", data_cancelamento {nameof(ContratoClicksignEnvio.DataCancelamento)}");
            sql.Append($", id_envio {nameof(ContratoClicksignEnvio.IdEnvio)}");
            sql.Append($", id_cancelamento {nameof(ContratoClicksignEnvio.IdCancelamento)}");
            sql.Append($" FROM topsys.con_clicksign_envios");
            sql.Append($" WHERE usina=@{nameof(usinaContrato)}");
            sql.Append($" AND ano_contrato=@{nameof(anoContrato)}");
            sql.Append($" AND num_contrato=@{nameof(numContrato)}");
            sql.Append($" AND NOT ISNULL(data_assinatura)");
            sql.Append($" ORDER BY data_envio DESC LIMIT 1");

            var filtro = new
            {
                usinaContrato,
                anoContrato,
                numContrato
            };

            return _context.Database.Connection.QueryFirstOrDefault<ContratoClicksignEnvio>(sql.ToString(), filtro);
        }

        public void SalvarIdClicksignDocument(Guid documentClicksignId, SolicitacaoAssinaturaEletronicaClicksign solicitacaoAssinaturaEletronicaClicksign, int qtdEnvioAssinaturaWhatsApp)
        {
            var sql = new StringBuilder();
            sql.Append($"INSERT INTO con_clicksign_envios SET ");
            sql.Append($"id =(SELECT uuid()), ");
            sql.Append($"usina=@{nameof(SolicitacaoAssinaturaEletronicaClicksign.ContratoUsina)}, ");
            sql.Append($"ano_contrato=@{nameof(SolicitacaoAssinaturaEletronicaClicksign.ContratoAno)}, ");
            sql.Append($"num_contrato=@{nameof(SolicitacaoAssinaturaEletronicaClicksign.ContratoNumero)}, ");
            sql.Append($"id_clicksign='{documentClicksignId}', ");
            sql.Append($"id_envio='{StringHelper.GetIDD(_identityHelperService.GetUserName())}',");
            sql.Append($"data_envio=NOW(),");
            sql.Append($"qtd_assinatura_whatsapp={qtdEnvioAssinaturaWhatsApp}");
            _context.Database.Connection.Execute(sql.ToString(), solicitacaoAssinaturaEletronicaClicksign);
        }

        public void AtualizarDataCancelamento(Guid id, DateTime dataCancelamento)
        {
            var sql = new StringBuilder();
            sql.Append($"UPDATE con_clicksign_envios SET ");
            sql.Append($"id_cancelamento='{StringHelper.GetIDD(_identityHelperService.GetUserName())}',");
            sql.Append($"data_cancelamento=@{nameof(dataCancelamento)}");
            sql.Append($" WHERE id=@{nameof(id)}");

            var filtro = new
            {
                dataCancelamento,
                id
            };

            _context.Database.Connection.Execute(sql.ToString(), filtro);
        }

        public void AtualizarDataAssinatura(Guid idClicksign, DateTime dataAssinatura)
        {
            var sql = new StringBuilder();
            sql.Append($"UPDATE con_clicksign_envios SET ");
            sql.Append($"data_assinatura=@{nameof(dataAssinatura)}");
            sql.Append($" WHERE id_clicksign=@{nameof(idClicksign)}");

            var filtro = new
            {
                dataAssinatura,
                idClicksign
            };
            _context.Database.Connection.Execute(sql.ToString(), filtro);
        }

        public ContratoClicksignEnvio ObterContratoClicksignEnvio(Guid idDocumento)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT");
            sql.Append($" id {nameof(ContratoClicksignEnvio.Id)}");
            sql.Append($", usina {nameof(ContratoClicksignEnvio.ContratonUsina)}");
            sql.Append($", ano_contrato {nameof(ContratoClicksignEnvio.ContratoAno)}");
            sql.Append($", num_contrato {nameof(ContratoClicksignEnvio.ContratoNumero)}");
            sql.Append($", id_clicksign {nameof(ContratoClicksignEnvio.IdClicksign)}");
            sql.Append($", data_envio {nameof(ContratoClicksignEnvio.DataEnvio)}");
            sql.Append($", data_assinatura {nameof(ContratoClicksignEnvio.DataAssinatura)}");
            sql.Append($", data_cancelamento {nameof(ContratoClicksignEnvio.DataCancelamento)}");
            sql.Append($", id_envio {nameof(ContratoClicksignEnvio.IdEnvio)}");
            sql.Append($", id_cancelamento {nameof(ContratoClicksignEnvio.IdCancelamento)}");
            sql.Append($" FROM topsys.con_clicksign_envios");
            sql.Append($" WHERE id=@{nameof(idDocumento)}");

            var filtro = new
            {
                idDocumento
            };

            return _context.Database.Connection.QueryFirstOrDefault<ContratoClicksignEnvio>(sql.ToString(), filtro);
        }

        public ContratoClicksignEnvio ObterUltimoContratoClicksignEnvio(int usinaContrato, int anoContrato, int numContrato)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT");
            sql.Append($" id {nameof(ContratoClicksignEnvio.Id)}");
            sql.Append($", usina {nameof(ContratoClicksignEnvio.ContratonUsina)}");
            sql.Append($", ano_contrato {nameof(ContratoClicksignEnvio.ContratoAno)}");
            sql.Append($", num_contrato {nameof(ContratoClicksignEnvio.ContratoNumero)}");
            sql.Append($", id_clicksign {nameof(ContratoClicksignEnvio.IdClicksign)}");
            sql.Append($", data_envio {nameof(ContratoClicksignEnvio.DataEnvio)}");
            sql.Append($", data_assinatura {nameof(ContratoClicksignEnvio.DataAssinatura)}");
            sql.Append($", data_cancelamento {nameof(ContratoClicksignEnvio.DataCancelamento)}");
            sql.Append($", id_envio {nameof(ContratoClicksignEnvio.IdEnvio)}");
            sql.Append($", id_cancelamento {nameof(ContratoClicksignEnvio.IdCancelamento)}");
            sql.Append($" FROM topsys.con_clicksign_envios");
            sql.Append($" WHERE usina=@{nameof(usinaContrato)}");
            sql.Append($" AND ano_contrato=@{nameof(anoContrato)}");
            sql.Append($" AND num_contrato=@{nameof(numContrato)}");
            sql.Append($" ORDER BY data_envio DESC LIMIT 1");

            var filtro = new
            {
                usinaContrato,
                anoContrato,
                numContrato
            };

            return _context.Database.Connection.QueryFirstOrDefault<ContratoClicksignEnvio>(sql.ToString(), filtro);
        }
    }
}
