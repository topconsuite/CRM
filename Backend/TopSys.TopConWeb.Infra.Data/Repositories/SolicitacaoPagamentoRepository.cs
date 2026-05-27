using Dapper;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class SolicitacaoPagamentoRepository : ISolicitacaoPagamentoRepository
    {
        private readonly AppDataContext _context;

        public SolicitacaoPagamentoRepository(AppDataContext context)
        {
            _context = context;
        }

        public void Adicionar(SolicitacaoPagamento solicitacaoPagamento)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT INTO topsys.con_bandeira_integracoes_envios");
            sql.Append(" SET cod_bandeira=@CartaoBandeiraCodigo");
            sql.Append(", id_integracao=@IntegracaoId");
            sql.Append(", valor_total=@ValorTotal");
            sql.Append(", cpf_cnpj=@CpfCnpj");
            sql.Append(", nome_cliente=@IntervenienteRazao");
            sql.Append(", tipo_cobranca=@TipoCobranca");
            sql.Append(", qtde_parcelas=@QuantidadeParcelas");
            sql.Append(", obra_usina=@ObraUsinaCodigo");
            sql.Append(", obra_numero=@ObraNumero");
            sql.Append(", observacao=@Observacao");
            sql.Append(", solicitante=@Solicitante");

            _context.Database.Connection.Execute(sql.ToString(), solicitacaoPagamento);
        }
    }
}
