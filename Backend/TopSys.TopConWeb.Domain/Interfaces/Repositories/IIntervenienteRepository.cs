using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IIntervenienteRepository : IRepositoryBase<Interveniente>{

        Interveniente ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual);

        Interveniente ObterPorCpfCnpjTracking(string cpfCnpj, string inscricaoEstadual);

        Interveniente ObterPorNome(string nome);

        IntervenienteLocal ObterLocalPorIntervenienteEDadosPessoais(int intervenienteCodigo, IDadosPessoais dados, Expression<Func<IntervenienteLocal, bool>> filter = null);

        PagedList<IntervenienteHistorico> ListarHistoricoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<IntervenienteHistorico, bool>> filter);

        int ObterCodigoMaximoCadastrado(int faixaInicial, int FaixaFinal);

        void AdicionarAnexo(string usuario, int intervenienteCodigo, int anoChamada, int numeroChamada, string anexo, string nome);
        void AdicionarAnexoPorOportunidade(string usuario, int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade, string anexo, string nome);

        ICollection<IntervenienteAnexo> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada);
        ICollection<IntervenienteAnexo> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade);

        byte[] ObterAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada);

        void AtualizarDescricaoAnexo(IntervenienteAnexo anexo);

        void RemoverAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada);

        PagedList<Interveniente> ListarComPaginacao(int page, int limit);
        Interveniente ObterPorIdExterno(string keyValue);
        Interveniente ObterPorCnpjCpf(string keyValue);
        PagedList<Interveniente> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        bool VerificaSeExiste(string fieldValue, string fieldName, string tableName);
        Interveniente ObterPorCodigo(int codigo);
    }
}
