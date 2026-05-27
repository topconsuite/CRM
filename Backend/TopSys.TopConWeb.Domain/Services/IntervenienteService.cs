using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel;

namespace TopSys.TopConWeb.Domain.Services
{
    public class IntervenienteService : ServiceBase<Interveniente>, IIntervenienteService
    {
        private readonly IIntervenienteRepository _intervenienteRepository;
        private readonly IHeaderProvider _headerProvider;

        public IntervenienteService(IIntervenienteRepository intervenienteRepository, IHeaderProvider headerProvider) : base(intervenienteRepository)
        {
            _intervenienteRepository = intervenienteRepository;
            _headerProvider = headerProvider;
        }

        public void AtualizarLimite(Interveniente interterveniente, DateTime? limiteData, float valorLimite, int bloqueioMotivoCodigo)
        {
            var interv = _intervenienteRepository.ObterPorId(interterveniente.Codigo);

            interv.AtualizaLimiteCredito(limiteData, valorLimite, bloqueioMotivoCodigo);

        }
        public Interveniente ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual)
        {
            var interv = _intervenienteRepository.ObterPorCpfCnpj(cpfCnpj, inscricaoEstadual);

            return interv;
        }

        public Interveniente ObterPorNome(string nome)
        {
            return _intervenienteRepository.ObterPorNome(nome);
        }

        public bool InscricaoEstadualEhValida(string inscricaoEstadual, string uf = "")
        {
            var ufs = new[] { "AC", "AL", "AM", "AP", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RO", "RS", "RR", "SC", "SE", "SP", "TO" };

            if (ufs.Contains(uf.ToUpper()))
            {
                try
                {
                    DocsBr.IE ie = new DocsBr.IE(inscricaoEstadual, uf);
                    return ie.IsValid();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                foreach (var sigla in ufs)
                {
                    try
                    {
                        DocsBr.IE ie = new DocsBr.IE(inscricaoEstadual, sigla);
                        if (ie.IsValid()) return true;
                    }
                    catch (Exception) { /* DO NOTHING */ };
                }
                return false;
            }
            
        }

        public IntervenienteLocal ObterLocalPorIntervenienteEDadosPessoais(int intervenienteCodigo, IDadosPessoais dados, Expression<Func<IntervenienteLocal, bool>> filter = null)
        {
            return _intervenienteRepository.ObterLocalPorIntervenienteEDadosPessoais(intervenienteCodigo, dados, filter);
        }

        public void AtualizaInformacoesBloqueio(Interveniente interterveniente, int bloqueioMotivoCodigo, string bloqueioObservacao)
        {
            var interv = _intervenienteRepository.ObterPorId(interterveniente.Codigo);

            interv.AtualizaInformacoesBloqueio(bloqueioMotivoCodigo, bloqueioObservacao);
        }

        public void AtualizarLimite(Interveniente interterveniente, DateTime? limiteData, float valorLimite)
        {
            var interv = _intervenienteRepository.ObterPorId(interterveniente.Codigo);

            interv.AtualizaApenasLimiteCredito(limiteData, valorLimite);
        }

        public PagedList<IntervenienteHistorico> ListarHistoricoEmOrdemDescrescente(int pagina, int porPagina, Expression<Func<IntervenienteHistorico, bool>> filter)
        {
            return _intervenienteRepository.ListarHistoricoEmOrdemDescrescente(pagina, porPagina, filter);
        }

        public void AdicionarHistorico(IntervenienteHistorico intertervenienteHistorico, string usuario)
        {
            var historicos = _intervenienteRepository.ListarFiltrados<IntervenienteHistorico>
                (t => t.CodigoInterveniente == intertervenienteHistorico.CodigoInterveniente);

            var quantidadeHistoricos = historicos.ToArray().Length;

            var sequencia = 1;

            if (quantidadeHistoricos != 0)
                sequencia = historicos.Max(t => t.SequenciaHistorico) + 1;

            intertervenienteHistorico.SetIdCadastro(usuario);
            intertervenienteHistorico.SetSequencia(sequencia);
            intertervenienteHistorico.SetHorarioNow();

            if (!intertervenienteHistorico.IntervenienteHistoricoScopeIsValid())
                return;

            _intervenienteRepository.Adicionar<IntervenienteHistorico>(intertervenienteHistorico);
        }

        public void AdicionarAnexo(string usuario, IntervenienteAnexo anexo)
        {
            if (anexo.OportunidadeAnexo != null)
            {
                _intervenienteRepository.AdicionarAnexoPorOportunidade(
                    usuario,
                    anexo.IntervenienteCodigo,
                    anexo.OportunidadeAnexo.Usina,
                    anexo.OportunidadeAnexo.AnoOportunidade,
                    anexo.OportunidadeAnexo.NumeroOportunidade,
                    anexo.Arquivo,
                    anexo.Nome);
            }
            else if (anexo.IntervenienteCodigo != 0)
            {
                _intervenienteRepository.AdicionarAnexo(usuario, anexo.IntervenienteCodigo, 0, 0, anexo.Arquivo, anexo.Nome);
            } 
            else
            {
                _intervenienteRepository.AdicionarAnexo(usuario, anexo.IntervenienteCodigo, anexo.AnoChamada, anexo.NumeroChamada, anexo.Arquivo, anexo.Nome);
            }
        }

        public ICollection<IntervenienteAnexo> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada)
        {
            return _intervenienteRepository.ListarAnexos(intervenienteCodigo, anoChamada, numeroChamada);
        }

        public ICollection<IntervenienteAnexo> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade)
        {
            return _intervenienteRepository.ListarAnexosPorOportunidade(intervenienteCodigo, usina, anoOportunidade, numeroOportunidade);
        }

        public byte[] ObterAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada)
        {
            return _intervenienteRepository.ObterAnexo(intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada);
        }

        public void AtualizarDescricaoAnexo(IntervenienteAnexo anexo)
        {
            _intervenienteRepository.AtualizarDescricaoAnexo(anexo);
        }

        public void RemoverAnexo(int intervenienteCodigo, string nome, DateTime dataHora, int anoChamada, int numeroChamada)
        {
            _intervenienteRepository.RemoverAnexo(intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada);
        }

        //Public Integration
        public PagedList<Interveniente> Listar(int page, int limit)
        {

            return _intervenienteRepository.ListarComPaginacao(page, limit);
        }

        public Interveniente ObterPorIdExterno(string idExterno)
        {

            return _intervenienteRepository.ObterPorIdExterno(idExterno);
        }

        public Interveniente ObterPorCnpjCpf(string cnpjCpf)
        {

            return _intervenienteRepository.ObterPorCnpjCpf(cnpjCpf);
        }

        public PagedList<Interveniente> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {

            return _intervenienteRepository.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit);
        }

        public List<string[]> ValidaCamposRequestAdicionarInterveniente(string cnpjCpf, string externalId, int? codMunic, int? portadorCobranca, string vendedorCodigo, int? enderecoNumero, string enderecoComplemento)
        {
            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (_intervenienteRepository.VerificaSeExiste(cnpjCpf, "cnpj_cpf", "ger_interv"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "Cpf_Cnpj"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode()
                });
            }

            if (externalId != "")
            {
                if (_intervenienteRepository.VerificaSeExiste(externalId.ToString(), "external_id", "ger_interv"))
                {
                    errors.Add(new string[] {
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "External_id"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode()
                    });
                }
            }

            if (codMunic != 0)
            {
                if (!_intervenienteRepository.VerificaSeExiste(codMunic.ToString(), "cod", "ger_municipio"))
                {
                    errors.Add(new string[] {
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "municipal_code"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                    });
                }
            }
            
            if (portadorCobranca != 0)
            {
                if (!_intervenienteRepository.VerificaSeExiste(portadorCobranca.ToString(), "cod", "fin_portador"))
                {
                    errors.Add(new string[] {
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bearer_billing_code"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                    });
                }
            }
            
            if (vendedorCodigo != "")
            {
                if (!_intervenienteRepository.VerificaSeExiste(vendedorCodigo.ToString(), "cod", "con_vendedor"))
                {
                    errors.Add(new string[] {
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "seller_code"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                    });
                }
            }

            if ((enderecoComplemento??"") == "" && (enderecoNumero??0) == 0)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "Number"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode()
                });
            }

            return errors;

        }

        public List<string[]> ValidaCamposRequestAtualizarInterveniente(string cnpjCpf, string externalId, int? codMunic, int? portadorCobranca, string vendedorCodigo, Interveniente interveniente, int? enderecoNumero, string enderecoComplemento)
        {
            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (cnpjCpf != null && cnpjCpf.ToString() != "" && _intervenienteRepository.VerificaSeExiste(cnpjCpf, "cnpj_cpf", "ger_interv") && cnpjCpf.ToString() != interveniente.CpfCnpj)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "Cpf_Cnpj"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode()
                });
            }
            
            if (cnpjCpf != null && cnpjCpf == "")
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "cnpj_cpf"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode()
                });
            }

            if (externalId != null && _intervenienteRepository.VerificaSeExiste(externalId.ToString(), "external_id", "ger_interv") && externalId.ToString() != interveniente.IdExterno)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "External_id"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode()
                });
            }

            if (codMunic != 0 && !_intervenienteRepository.VerificaSeExiste(codMunic.ToString(), "cod", "ger_municipio") && codMunic != interveniente.EnderecoMunicipioCodigo)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "municipal_code"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (!_intervenienteRepository.VerificaSeExiste(portadorCobranca.ToString(), "cod", "fin_portador") && portadorCobranca != interveniente.PortadorCobranca)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bearer_billing_code"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            int.TryParse(vendedorCodigo, out int result);
            if (vendedorCodigo != null && !_intervenienteRepository.VerificaSeExiste(vendedorCodigo.ToString(), "cod", "con_vendedor") && result != interveniente.VendedorCodigo)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "seller_code"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if ((enderecoComplemento??"") == "" && interveniente.EnderecoComplemento == "" && (enderecoNumero??0) == 0 && interveniente.EnderecoNumero == 0)
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "Number"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode()
                });
            }
            
            return errors;
        }
    }
}
