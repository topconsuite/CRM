using Dapper;
using MySql.Data.MySqlClient.Memcached;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;


namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class IntervenienteRepository : RepositoryBase<Interveniente>, IIntervenienteRepository
    {
        public IntervenienteRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public Interveniente ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual)
        {
            if (cpfCnpj.Length < 11)
                return null;

            var filtraPorIE = cpfCnpj.Length < 14;

            var interveniente = _context.Intervenientes
                .Include(i => i.EnderecoMunicipio)
                .Include(i => i.BloqueioMotivo)
                .Where(i => i.CpfCnpj == cpfCnpj &&
                    ((!filtraPorIE || i.InscricaoEstadual == inscricaoEstadual) || i.InscricaoEstadual == "ISENTO" || i.InscricaoEstadual == ""))
                .AsNoTracking()
                .FirstOrDefault();

            return interveniente;
        }

        public Interveniente ObterPorCpfCnpjTracking(string cpfCnpj, string inscricaoEstadual)
        {
            if (cpfCnpj.Length < 11)
                return null;

            var interveniente = _context.Intervenientes
                .Include(i => i.EnderecoMunicipio)
                .Include(i => i.BloqueioMotivo)
                .Where(i => i.CpfCnpj == cpfCnpj &&
                (i.InscricaoEstadual == inscricaoEstadual || i.InscricaoEstadual == "ISENTO" || i.InscricaoEstadual == ""))
                .FirstOrDefault();

            return interveniente;
        }

        public Interveniente ObterPorNome(string nome)
        {
            var interveniente = _context.Intervenientes
                .Include(i => i.EnderecoMunicipio)
                .Where(i => i.Nome == nome)
                .AsNoTracking()
                .FirstOrDefault();

            return interveniente;
        }

        public IntervenienteLocal ObterLocalPorIntervenienteEDadosPessoais(int intervenienteCodigo, IDadosPessoais dados, Expression<Func<IntervenienteLocal, bool>> filter = null)
        {
            var query = _context.IntervenienteLocais
                .Where(t => t.IntervenienteCodigo == intervenienteCodigo
                    && t.CpfCnpj == dados.CpfCnpj
                    && t.EnderecoCep == dados.EnderecoCep
                    && t.EnderecoLogradouro == dados.EnderecoLogradouro
                    && t.EnderecoNumero == dados.EnderecoNumero
                );

            if (filter != null)
                query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public PagedList<IntervenienteHistorico> ListarHistoricoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<IntervenienteHistorico, bool>> filter)
        {
            var pagedList = _context.IntervenienteHistoricos
                .OrderByDescending(t => t.SequenciaHistorico)
                .Where(filter)
                .PagedList(pagina, porPagina, filter);

            return pagedList;
        }

        public int ObterCodigoMaximoCadastrado(int faixaInicial, int faixaFinal)
        {
            var result = _context.Intervenientes
                .Where(g => g.Codigo >= faixaInicial && g.Codigo <= faixaFinal)
                .Max(g => (int?)g.Codigo);

            return result ?? 0;
        }

        public void AdicionarAnexo(string usuario, int intervenienteCodigo, int anoChamada, int numeroChamada, string anexo, string nome)
        {
            var sql = new StringBuilder();

            sql.Append("INSERT IGNORE INTO topsys.ger_interv_anex SET ");
            sql.Append($"interv=@{nameof(intervenienteCodigo)}, ");
            sql.Append($"descricao=@{nameof(nome)}, ");
            sql.Append($"nome=@{nameof(nome)}, ");
            sql.Append($"usuario=@{nameof(usuario)}, ");
            sql.Append($"data=CURRENT_DATE(), ");
            sql.Append($"data_hora=NOW(), ");
            sql.Append($"arquivo=@{nameof(anexo)}, ");
            sql.Append($"ano_chamada=@{nameof(anoChamada)}, ");
            sql.Append($"num_chamada=@{nameof(numeroChamada)} ");

            _context.Database.Connection.Execute(sql.ToString(), new { intervenienteCodigo, nome, usuario, anexo, anoChamada, numeroChamada });
        }

        public void AdicionarAnexoPorOportunidade(string usuario, int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade, string anexo, string nome)
        {

            var sql = new StringBuilder();
            sql.Append("SELECT NOW()");
            var dataHora = _context.Database.Connection.QueryFirstOrDefault<DateTime>(sql.ToString());

            sql.Clear();
            sql.Append("INSERT IGNORE INTO topsys.ger_interv_anex SET ");
            sql.Append($"interv=@{nameof(intervenienteCodigo)}, ");
            sql.Append($"descricao=@{nameof(nome)}, ");
            sql.Append($"nome=@{nameof(nome)}, ");
            sql.Append($"usuario=@{nameof(usuario)}, ");
            sql.Append($"data=CURRENT_DATE(), ");
            sql.Append($"data_hora=@{nameof(dataHora)}, ");
            sql.Append($"arquivo=@{nameof(anexo)}, ");
            sql.Append($"ano_chamada=0, ");
            sql.Append($"num_chamada=0 ");

            _context.Database.Connection.Execute(sql.ToString(), new { intervenienteCodigo, nome, usuario, dataHora, anexo });

            sql.Clear();
            sql.AppendLine("INSERT IGNORE INTO topsys.con_oportunidade_anexo SET ");
            sql.AppendLine($"usina = @{nameof(usina)}, ");
            sql.AppendLine($"ano_oportunidade = @{nameof(anoOportunidade)}, ");
            sql.AppendLine($"num_oportunidade = @{nameof(numeroOportunidade)}, ");
            sql.AppendLine($"interv = @{nameof(intervenienteCodigo)}, ");
            sql.AppendLine($"nome = @{nameof(nome)}, ");
            sql.AppendLine($"data_hora = @{nameof(dataHora)} ");

            var parameters = new
            {
                usina,
                anoOportunidade,
                numeroOportunidade,
                intervenienteCodigo,
                nome,
                dataHora
            };

            _context.Database.Connection.Execute(sql.ToString(), parameters);

        }

        public ICollection<IntervenienteAnexo> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada)
        {
            if(intervenienteCodigo == 0)
            {
                return _context.IntervenienteAnexos
                    .Where(t => t.IntervenienteCodigo == 0 && t.AnoChamada == anoChamada && t.NumeroChamada == numeroChamada)
                    .OrderByDescending(t => t.DataHora)
                    .AsNoTracking()
                    .ToList();
            } else {
                return _context.IntervenienteAnexos
                    .Where(t => t.IntervenienteCodigo == intervenienteCodigo && t.AnoChamada == 0 && t.NumeroChamada == 0)
                    .OrderByDescending(t => t.DataHora)
                    .AsNoTracking()
                    .ToList();
            }
        }

        public ICollection<IntervenienteAnexo> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade)
        {


            var results = _context.IntervenienteAnexos
                .Include(t => t.OportunidadeAnexo)
                .Where(t => (t.IntervenienteCodigo == intervenienteCodigo && t.IntervenienteCodigo > 0)
                            || (t.OportunidadeAnexo.Usina == usina && t.OportunidadeAnexo.AnoOportunidade == anoOportunidade && t.OportunidadeAnexo.NumeroOportunidade == numeroOportunidade))
                .ToList();

            return results;

        }

        public byte[] ObterAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT arquivo");
            sql.Append($" FROM topsys.ger_interv_anex");
            sql.Append($" WHERE interv=@{nameof(intervenienteCodigo)}");
            sql.Append($" AND nome=@{nameof(nome)}");
            sql.Append($" AND data_hora=@{nameof(dataHora)}");
            sql.Append($" AND ano_chamada=@{nameof(anoChamada)}");
            sql.Append($" AND num_chamada=@{nameof(numeroChamada)}"); ;

            return _context.Database.Connection.QueryFirstOrDefault<byte[]>(sql.ToString(), new { intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada }); 
        }

        public void AtualizarDescricaoAnexo(IntervenienteAnexo anexo)
        {
            var sql = new StringBuilder();

            sql.Append("UPDATE topsys.ger_interv_anex SET ");
            sql.Append($"descricao=@{nameof(anexo.Descricao)}");
            sql.Append($" WHERE interv=@{nameof(anexo.IntervenienteCodigo)}");
            sql.Append($" AND nome=@{nameof(anexo.Nome)}");
            sql.Append($" AND data_hora=@{nameof(anexo.DataHora)}");
            sql.Append($" AND ano_chamada=@{nameof(anexo.AnoChamada)}");
            sql.Append($" AND num_chamada=@{nameof(anexo.NumeroChamada)}");

            _context.Database.Connection.Execute(sql.ToString(), new { anexo.Descricao, anexo.IntervenienteCodigo, anexo.Nome, anexo.DataHora, anexo.AnoChamada, anexo.NumeroChamada });
        }

        public void RemoverAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada)
        {
            var sql = new StringBuilder();

            sql.Append("DELETE FROM topsys.ger_interv_anex WHERE ");
            sql.Append($"interv=@{nameof(intervenienteCodigo)}");
            sql.Append($" AND nome=@{nameof(nome)}");
            sql.Append($" AND data_hora=@{nameof(dataHora)}");
            sql.Append($" AND ano_chamada=@{nameof(anoChamada)}");
            sql.Append($" AND num_chamada=@{nameof(numeroChamada)}");

            _context.Database.Connection.Execute(sql.ToString(), new { intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada });

            sql.Clear();
            sql.Append("DELETE FROM topsys.con_oportunidade_anexo WHERE ");
            sql.Append($"interv=@{nameof(intervenienteCodigo)}");
            sql.Append($" AND nome=@{nameof(nome)}");
            sql.Append($" AND data_hora=@{nameof(dataHora)}");

            _context.Database.Connection.Execute(sql.ToString(), new { intervenienteCodigo, nome, dataHora });

        }

        //Public Integration
        public PagedList<Interveniente> ListarComPaginacao(int page, int limit)
        {
            return _context.Intervenientes
                .OrderBy(g => g.Codigo)
                .AsNoTracking()
                .PagedList(page, limit, null);
        }

        public Interveniente ObterPorIdExterno(string idExterno)
        {

            Interveniente interveniente = _context.Intervenientes
                    .Where(t => t.IdExterno == idExterno)
                    .AsNoTracking()
                    .FirstOrDefault();

            return interveniente;
        }

        public Interveniente ObterPorCnpjCpf(string cnpjCpf)
        {

            Interveniente interveniente = _context.Intervenientes
                    .Where(t => t.CpfCnpj == cnpjCpf)
                    .AsNoTracking()
                    .FirstOrDefault();

            return interveniente;
        }

        public PagedList<Interveniente> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.AppendLine("SELECT");
            sqlComando.Append($" Cod {nameof(Interveniente.Codigo)}");
            sqlComando.Append($", Nome {nameof(Interveniente.Nome)}");
            sqlComando.Append($", Razao {nameof(Interveniente.Razao)}");
            sqlComando.Append($", Cli {nameof(Interveniente.Cliente)}");
            sqlComando.Append($", Forn {nameof(Interveniente.Fornecedor)}");
            sqlComando.Append($", Transpo {nameof(Interveniente.Transportador)}");
            sqlComando.Append($", Prest_serv {nameof(Interveniente.PrestadorServico)}");
            sqlComando.Append($", Org_Publ {nameof(Interveniente.OrgaoPublico)}");
            sqlComando.Append($", Outro {nameof(Interveniente.Outros)}");
            sqlComando.Append($", Cep {nameof(Interveniente.EnderecoCep)}");
            sqlComando.Append($", End {nameof(Interveniente.EnderecoLogradouro)}");
            sqlComando.Append($", Num {nameof(Interveniente.EnderecoNumero)}");
            sqlComando.Append($", compl {nameof(Interveniente.EnderecoComplemento)}");
            sqlComando.Append($", Bairro {nameof(Interveniente.EnderecoBairro)}");
            sqlComando.Append($", cod_munic {nameof(Interveniente.EnderecoMunicipioCodigo)}");
            sqlComando.Append($", CNPJ_CPF {nameof(Interveniente.CpfCnpj)}");
            sqlComando.Append($", IE {nameof(Interveniente.InscricaoEstadual)}");
            sqlComando.Append($", RG {nameof(Interveniente.Rg)}");
            sqlComando.Append($", Ccm {nameof(Interveniente.InscricaoMunicipal)}");
            sqlComando.Append($", Cei {nameof(Interveniente.Cei)}");
            sqlComando.Append($", DDD {nameof(Interveniente.TelefoneDdd)}");
            sqlComando.Append($", Tel {nameof(Interveniente.TelefoneNumero)}");
            sqlComando.Append($", Ramal {nameof(Interveniente.Ramal)}");
            sqlComando.Append($", ddd_celular {nameof(Interveniente.CelularDdd)}");
            sqlComando.Append($", celular {nameof(Interveniente.CelularNumero)}");
            sqlComando.Append($", email {nameof(Interveniente.Email)}");
            sqlComando.Append($", email_cobranca {nameof(Interveniente.EmailCobranca)}");
            sqlComando.Append($", contato {nameof(Interveniente.Contato)}");
            sqlComando.Append($", Ativ {nameof(Interveniente.Atividade)}");
            sqlComando.Append($", tp_cobranca {nameof(Interveniente.TipoCobranca)}");
            sqlComando.Append($", Vend {nameof(Interveniente.VendedorCodigo)}");
            sqlComando.Append($", Bloq {nameof(Interveniente.BloqueioMotivoCodigo)}");
            sqlComando.Append($", Limite_Cred {nameof(Interveniente.LimiteValor)}");
            sqlComando.Append($", Pct_Desco {nameof(Interveniente.PorcentagemDesconto)}");
            sqlComando.Append($", obs {nameof(Interveniente.Observacao)}");
            sqlComando.Append($", In86 {nameof(Interveniente.In86)}");
            sqlComando.Append($", ctb_cta_contab {nameof(Interveniente.ContaContabil)}");
            sqlComando.Append($", bombista {nameof(Interveniente.bombista)}");
            sqlComando.Append($", forn_mp {nameof(Interveniente.FornecedorMp)}");
            sqlComando.Append($", regiao {nameof(Interveniente.Regiao)}");
            sqlComando.Append($", rot {nameof(Interveniente.Rota)}");
            sqlComando.Append($", seq_rot {nameof(Interveniente.RotaSequencia)}");
            sqlComando.Append($", transp {nameof(Interveniente.Transp)}");
            sqlComando.Append($", tp_cliente {nameof(Interveniente.IntervenienteTipo)}");
            sqlComando.Append($", ret_iss {nameof(Interveniente.RetemIss)}");
            sqlComando.Append($", local_entrega {nameof(Interveniente.LocalEntrega)}");
            sqlComando.Append($", especificacao {nameof(Interveniente.Especificacao)}");
            sqlComando.Append($", nome_mae {nameof(Interveniente.NomeMae)}");
            sqlComando.Append($", conjuge {nameof(Interveniente.NomeConjuge)}");
            sqlComando.Append($", port_cobranca {nameof(Interveniente.PortadorCobranca)}");
            sqlComando.Append($", func {nameof(Interveniente.Funcionario)}");
            sqlComando.Append($", site {nameof(Interveniente.Site)}");
            sqlComando.Append($", aprov_eng {nameof(Interveniente.AprovacaoEngenharia)}");
            sqlComando.Append($", profissao {nameof(Interveniente.Profissao)}");
            sqlComando.Append($", ddd_com {nameof(Interveniente.TelefoneComercialDdd)}");
            sqlComando.Append($", tel_com {nameof(Interveniente.TelefoneComercialNumero)}");
            sqlComando.Append($", lim_cred_val {nameof(Interveniente.LimiteData)}");
            sqlComando.Append($", simpl_nacional {nameof(Interveniente.SimplesNacional)}");
            sqlComando.Append($", ret_inss {nameof(Interveniente.RetemInss)}");
            sqlComando.Append($", contrib_icms {nameof(Interveniente.ContribuiIcms)}");
            sqlComando.Append($", Inativado {nameof(Interveniente.Inativo)}");
            sqlComando.Append($", external_id {nameof(Interveniente.IdExterno)}");
            sqlComando.Append($", ret_irrf {nameof(Interveniente.RetemIrrf)}");
            sqlComando.Append($", ret_cofins {nameof(Interveniente.RetemCofins)}");
            sqlComando.Append($", ret_pis {nameof(Interveniente.RetemPis)}");
            sqlComando.Append($", ret_csll {nameof(Interveniente.RetemCsll)}");
            sqlComando.Append($", id_atual {nameof(Interveniente.IdAtualizacao)}");
            sqlComando.Append($", Org_Uf_Emi {nameof(Interveniente.OrgaoExpedidor)}");
            sqlComando.Append($", emp_trabalho {nameof(Interveniente.EmpresaTrabalho)}");
            sqlComando.Append($", obs_bloq {nameof(Interveniente.BloqueioObservacao)}");
            sqlComando.Append($", id_aprov_iss {nameof(Interveniente.IdAprovacaoRetencaoIss)}");
            sqlComando.Append($", grupo_economico {nameof(Interveniente.GrupoEconomicoCodigo)}");
            sqlComando.Append($", atualizado_em {nameof(Interveniente.DataAtualizacao)}");
            sqlComando.Append(" FROM ger_interv");
            sqlComando.Append($" WHERE atualizado_em >= '{dataInicio.ToString("yyyy-MM-dd HH:mm:ss")}'");

            if (dataFim != null)
                sqlComando.Append($" AND atualizado_em <= '{dataFim?.ToString("yyyy-MM-dd HH:mm:ss")}'");

            sqlComando.Append($" ORDER BY atualizado_em");

            var intervenientes = _context.Connection.QueryPagedList<Interveniente>(sqlComando.ToString(), page, limit);

            var intervenientesLista = new List<Interveniente>();

            var intervenientesResultPagedList = new PagedList<Interveniente>
            {
                CurrentPage = intervenientes.CurrentPage,
                PageCount = intervenientes.PageCount,
                PageSize = intervenientes.PageSize,
                RecordCount = intervenientes.RecordCount
            };

            foreach (var record in intervenientes.Records)
            {
                var interveniente = (Interveniente)record;

                intervenientesLista.Add(interveniente);
            }

            intervenientesResultPagedList.Records = intervenientesLista;

            return intervenientesResultPagedList;
        }

        public bool VerificaSeExiste(string fieldValue, string fieldName, string tableName)
        {
            if (fieldValue == "")
            {
                return true;
            }

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT 1 FROM {tableName} WHERE {fieldName} = '{fieldValue}'");

            bool result = _context.Database.Connection.Query<int>(sqlComando.ToString()).Any();

            return result;
        }

        public Interveniente ObterPorCodigo(int codigo)
        {
            Interveniente interveniente = _context.Intervenientes
                    .Where(t => t.Codigo == codigo)
                    .AsNoTracking()
                    .FirstOrDefault();

            return interveniente;
        }
    }
}
