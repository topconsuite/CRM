using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IObraTaxaService
    {
        void AprovarTaxas(string usuario, ICollection<ObraTaxa> obraTaxas);

        void AprovarTaxas(string usuario, ICollection<ObraTaxaVersao> obraTaxas, int numVersao);

        ICollection<ObraTaxa> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo);

        ICollection<ObraTaxaVersao> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo, int numVersao);

        ICollection<ObraTaxaVersao> ListarByIdObraVersao(int versao, int usinaEntregaCodigo, int obraCodigo);

        ICollection<ObraTaxa> ListarTaxaPadraoByIdUsina(int usinaEntregaCodigo);

        ICollection<ObraTaxa> ListarTaxaPadraoByIdUsinaSegmento(int usinaEntregaCodigo, int idSegmentacao);

        float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina);

        float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero);

        float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, ICollection<ObraTaxa> obraTaxas);

        float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, ICollection<ObraTaxaVersao> obraTaxas);

        float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, bool possuiBomba);

        float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero, bool possuiBomba);

        float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, ICollection<ObraTaxa> obraTaxas, bool possuiBomba);

        float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, ICollection<ObraTaxaVersao> obraTaxas, bool possuiBomba);

        float ObterValorAdicionalDomingosEFeriados(float valorConcretoTotal, int idUsina);

        float ObterValorAdicionalDomingosEFeriados(float valorConcretoTotal, int idUsina, int obraNumero);

        float ObterValorAdicionalDomingosEFeriados(float valorConcretoTotal, ICollection<ObraTaxa> obraTaxas);

        float ObterValorAdicionalNoturno(string horario, float volume, float valorConcretoUnitario, string[] tiposPessoa, int idUsina);

        float ObterValorAdicionalNoturno(string horario, float volume, float valorConcretoUnitario, string[] tiposPessoa, int idUsina, int obraNumero);

        float ObterValorAdicionalNoturno(string horario, DateTime dataConcretagem, float volume, float valorConcretoUnitario, string[] tiposPessoa, ICollection<ObraTaxa> obraTaxas);

        void SalvarPersonalizada(ObraTaxa obraTaxa);

        void SalvarPersonalizada(ObraTaxaVersao obraTaxa);

        void DeletarPersonalizada(ObraTaxa obraTaxa);

        void DeletarPersonalizada(ObraTaxaVersao obraTaxa);

        void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra);

        void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra);

        void AdicionarContrato(int codUsina, int numVersao, int numObra);

        void ExcluirContrato(int codUsina, int numObra);
        void MarcarTaxa(int numVersao, int usinaCodigo, int numeroObra, int seq);

        void MarcarTaxa(int usinaCodigo, int numeroObra, int seq);
    }
}
