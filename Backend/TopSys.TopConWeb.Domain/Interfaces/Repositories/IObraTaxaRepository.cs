using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IObraTaxaRepository : IRepositoryBase<ObraTaxa>
    {
        void AtualizarObraTaxa(ObraTaxa obraTaxa);

        void AtualizarObraTaxa(ObraTaxaVersao obraTaxa);

        bool AdicionalNoturnoVerificarDia(ObraTaxa taxa, DateTime data);

        ICollection<ObraTaxa> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo, int segmentacao);

        ICollection<ObraTaxaVersao> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo, int numVersao, int segmentacao);

        ICollection<ObraTaxa> ListarTaxaPadraoByIdUsina(int usinaEntregaCodigo);

        ICollection<ObraTaxa> ListarTaxaPadraoByIdUsinaAndSegmento(int usinaEntregaCodigo, int idSegmentacao);

        ICollection<ObraTaxaVersao> ListarTaxaPadraoByIdUsinaVersao(int numVersao, int usinaEntregaCodigo);

        ICollection<ObraTaxaVersao> ListarTaxaPadraoByIdUsinaAndSegmentoVersao(int numVersao, int usinaEntregaCodigo, int idSegmentacao);

        void SalvarPersonalizada(ObraTaxa obraTaxa);

        void SalvarPersonalizada(ObraTaxaVersao obraTaxa);

        void DeletarPersonalizada(ObraTaxa obraTaxa);

        void DeletarPersonalizada(ObraTaxaVersao obraTaxa);

        void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra);

        void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra);

        void AdicionarContrato(int codUsina, int numVersao, int numObra);

        void ExcluirContrato(int codUsina, int numObra);

        ObraTaxa ObterTaxaCancelamentoProgramacao(int codUsina, int numObra, string tipoTaxa, string tipoAntecedencia, int valor);

        int ObterCodMercadoriaTaxaCancelamentoProgramacao();
    }
}
