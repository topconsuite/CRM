using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IIntervenienteService : IServiceBase<Interveniente>
    {
        void AtualizaInformacoesBloqueio(Interveniente interterveniente, int bloqueioMotivoCodigo, string bloqueioObservacao);

        void AtualizarLimite(Interveniente interterveniente, DateTime? limiteData, float valorLimite);

        void AtualizarLimite(Interveniente interterveniente, DateTime? limiteData, float valorLimite, int bloqueioMotivoCodigo);
        
        Interveniente ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual);

        Interveniente ObterPorNome(string nome);

        IntervenienteLocal ObterLocalPorIntervenienteEDadosPessoais(int intervenienteCodigo, IDadosPessoais dados, Expression<Func<IntervenienteLocal, bool>> filter = null);

        bool InscricaoEstadualEhValida(string inscricaoEstadual, string uf = "");

        PagedList<IntervenienteHistorico> ListarHistoricoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<IntervenienteHistorico, bool>> filter);

        void AdicionarHistorico(IntervenienteHistorico intertervenienteHistorico, string Usuario);

        void AdicionarAnexo(string usuario, IntervenienteAnexo anexo);

        ICollection<IntervenienteAnexo> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada);
        ICollection<IntervenienteAnexo> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade);

        byte[] ObterAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada);

        void AtualizarDescricaoAnexo(IntervenienteAnexo anexo);

        void RemoverAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada);

        PagedList<Interveniente> Listar(int page, int limit);
        Interveniente ObterPorIdExterno(string IdExterno);
        Interveniente ObterPorCnpjCpf(string cnpjCpf);
        PagedList<Interveniente> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        List<string[]> ValidaCamposRequestAdicionarInterveniente(string cnpjCpf, string externalId, int? codMunic, int? portadorCobranca, string vendedorCodigo, int? enderecoNumero, string enderecoComplemento);
        List<string[]> ValidaCamposRequestAtualizarInterveniente(string cnpjCpf, string externalId, int? codMunic, int? portadorCobranca, string vendedorCodigo, Interveniente interveniente, int? enderecoNumero, string enderecoComplemento);
    }
}
