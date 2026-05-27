using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Helpers
{
    public class TituloContasAReceberHelper
    {
        private static (int Início, int Tamanho) EmpresaCodigo = (0, 2);
        private static (int Início, int Tamanho) DocumentoTipo = (2, 2);
        private static (int Início, int Tamanho) DocumentoSerie = (4, 5);
        private static (int Início, int Tamanho) DocumentoNumero = (9, 11);
        private static (int Início, int Tamanho) Sequencia = (20, 3);
        private static (int Início, int Tamanho) BandeiraCodigo = (23, 3);
        private static (int Início, int Tamanho) AgenciaNumero = (26, 4);
        private static (int Início, int Tamanho) ContaNumero = (30, 10);
        private static (int Início, int Tamanho) ContaDigito = (40, 1);
        private static (int Início, int Tamanho) Desdobramento = (41, 2);

        private static readonly Dictionary<string, (int Início, int Tamanho)> CamposMapeamento = new Dictionary<string, (int, int)>
        {
            { "EmpresaCodigo", EmpresaCodigo },
            { "DocumentoTipo", DocumentoTipo },
            { "DocumentoSerie", DocumentoSerie },
            { "DocumentoNumero", DocumentoNumero },
            { "Sequencia", Sequencia },
            { "BandeiraCodigo", BandeiraCodigo },
            { "AgenciaNumero", AgenciaNumero },
            { "ContaNumero", ContaNumero },
            { "ContaDigito", ContaDigito },
            { "Desdobramento", Desdobramento }
        };

        public static (int Empresa, int TipoDocumento, string SerieDocumento, int NumeroDocumento, int SequenciaDocumento, int BandeiraCodigo, int AgenciaNumero, int ContaNumero, int ContaDigito, int Desdobramento) ObtemChaveDoOriginalIdReguaDeCobranca(string id)
        {
            var empresa = int.Parse(id.Substring(EmpresaCodigo.Início, EmpresaCodigo.Tamanho));
            var tipoDocumento = int.Parse(id.Substring(DocumentoTipo.Início, DocumentoTipo.Tamanho));
            var serieDocumento = id.Substring(DocumentoSerie.Início, DocumentoSerie.Tamanho).TrimStart('0');
            var numeroDocumento = int.Parse(id.Substring(DocumentoNumero.Início, DocumentoNumero.Tamanho));
            var sequenciaDocumento = int.Parse(id.Substring(Sequencia.Início, Sequencia.Tamanho));
            var bandeiraCodigo = int.Parse(id.Substring(BandeiraCodigo.Início, BandeiraCodigo.Tamanho));
            var agenciaNumero = int.Parse(id.Substring(AgenciaNumero.Início, AgenciaNumero.Tamanho));
            var contaNumero = int.Parse(id.Substring(ContaNumero.Início, ContaNumero.Tamanho));
            var contaDigito = int.Parse(id.Substring(ContaDigito.Início, ContaDigito.Tamanho));
            var desdobramento = int.Parse(id.Substring(Desdobramento.Início, Desdobramento.Tamanho));

            return (empresa, tipoDocumento, serieDocumento, numeroDocumento, sequenciaDocumento, bandeiraCodigo, agenciaNumero, contaNumero, contaDigito, desdobramento);
        }

    }
}

