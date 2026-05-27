using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Services
{
    public class PropostaService : ServiceBase<Proposta>, IPropostaService
    {
        private readonly IPropostaRepository _propostaRepository;
        private readonly IContratoService _contratoService;
        private readonly IUsinaDistanciaCepService _usinaDistanciaCepService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly ITracoPrecoService _tracoPrecoService;
        private readonly IVendedorService _vendedorService;
        private readonly ICondicaoPagamentoService _condicaoPagamentoService;
        private readonly IParametroService _parametroService;
        private readonly IUsuarioService _usuarioService;
        private readonly IObraService _obraService;
        private readonly IObraTaxaService _obraTaxaService;
        private readonly IEnderecoService _enderecoService;
        private readonly IMercadoriaService _mercadoriaService;
        private readonly IUsoService _usoService;
        private readonly ICustoServicoService _custoServicoService;
        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IAprovacaoComercialPendenteService _aprovacaoComercialPendenteService;
        private readonly IAprovacaoComercialService _aprovacaoComercialService;
        private readonly IArquivoService _arquivoService;
        private readonly IDemaisAprovacaoService _demaisAprovacaoService;

        private const int NUMERO_APLICACAO_PROPOSTA_LISTA = 6101;

        public PropostaService(
            IPropostaRepository propostaRepository,
            IContratoService contratoService,
            IUsinaDistanciaCepService usinaDistanciaCepService,
            IIntervenienteService intervenienteService,
            ITracoPrecoService tracoPrecoService,
            IVendedorService vendedorService,
            ICondicaoPagamentoService condicaoPagamentoService,
            IParametroService parametroService,
            IUsuarioService usuarioService,
            IObraService obraService,
            IObraTaxaService obraTaxaService,
            IEnderecoService enderecoService,
            IMercadoriaService mercadoriaService,
            ICustoServicoService custoServicoService,
            IUsoService usoService,
            IComercialLegacyService comercialLegacyService,
            IAprovacaoComercialPendenteService aprovacaoComercialPendenteService,
            IAprovacaoComercialService aprovacaoComercialService,
            IArquivoService arquivoService,
            IDemaisAprovacaoService demaisAprovacaoService
        ) : base(propostaRepository)
        {
            _propostaRepository = propostaRepository;
            _contratoService = contratoService;
            _usinaDistanciaCepService = usinaDistanciaCepService;
            _intervenienteService = intervenienteService;
            _tracoPrecoService = tracoPrecoService;
            _vendedorService = vendedorService;
            _condicaoPagamentoService = condicaoPagamentoService;
            _parametroService = parametroService;
            _usuarioService = usuarioService;
            _obraService = obraService;
            _obraTaxaService = obraTaxaService;
            _enderecoService = enderecoService;
            _mercadoriaService = mercadoriaService;
            _usoService = usoService;
            _custoServicoService = custoServicoService;
            _comercialLegacyService = comercialLegacyService;
            _aprovacaoComercialPendenteService = aprovacaoComercialPendenteService;
            _aprovacaoComercialService = aprovacaoComercialService;
            _arquivoService = arquivoService;
            _demaisAprovacaoService = demaisAprovacaoService;
        }

        public void Adicionar(string usuario, Proposta proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            if (ValidarProposta(usuario, proposta, filtroVendedores))
            {
                proposta.IdCadastro = StringHelper.GetIDD(usuario);
                _propostaRepository.Adicionar(proposta);

                if (proposta.UtilizaResponsavelSolidario)
                {
                    proposta.ResponsavelSolidario.UsinaCodigo = proposta.UsinaCodigo;
                    proposta.ResponsavelSolidario.PropostaAno = proposta.Ano;
                    proposta.ResponsavelSolidario.PropostaNumero = proposta.Numero;
                    _propostaRepository.Adicionar(proposta.ResponsavelSolidario);
                }

                if (proposta.UtilizaDadosFaturamento || proposta.UtilizaEnderecoFaturamento)
                {
                    proposta.Faturamento.UsinaCodigo = proposta.UsinaCodigo;
                    proposta.Faturamento.PropostaAno = proposta.Ano;
                    proposta.Faturamento.PropostaNumero = proposta.Numero;
                    _propostaRepository.Adicionar(proposta.Faturamento);
                }

                if (proposta.UtilizaDadosCobranca || proposta.UtilizaEnderecoCobranca)
                {
                    proposta.Cobranca.UsinaCodigo = proposta.UsinaCodigo;
                    proposta.Cobranca.PropostaAno = proposta.Ano;
                    proposta.Cobranca.PropostaNumero = proposta.Numero;
                    _propostaRepository.Adicionar(proposta.Cobranca);
                }

                proposta.Obra.NumChamada = proposta.Numero;
                proposta.Obra.ObraNova = (proposta.OrigemObraCodigo > 0 && proposta.OrigemUsinaCodigo > 0) ? "N" : "S";
                proposta.Obra.IdCadastro = proposta.IdCadastro;
                _obraService.Adicionar(proposta.Obra, proposta.ValorExtras);

                ValidarDemaisAprovacoes(usuario, proposta);

                _aprovacaoComercialPendenteService.ValidarComercialObra(proposta.UsinaCodigo, proposta.Obra.Numero, usuario);

                _obraService.AtualizarStatusComercial(proposta.UsinaCodigo, proposta.Obra.Numero);
            }

        }

        public void Adicionar(string usuario, PropostaVersao proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            if (ValidarProposta(usuario, proposta, filtroVendedores))
            {
                proposta.IdCadastro = StringHelper.GetIDD(usuario);
                _propostaRepository.Adicionar(proposta);

                if (proposta.UtilizaResponsavelSolidario)
                {
                    proposta.ResponsavelSolidario.UsinaCodigo = proposta.UsinaCodigo;
                    proposta.ResponsavelSolidario.PropostaAno = proposta.Ano;
                    proposta.ResponsavelSolidario.PropostaNumero = proposta.Numero;
                    _propostaRepository.Adicionar(proposta.ResponsavelSolidario);
                }

                if (proposta.UtilizaDadosFaturamento || proposta.UtilizaEnderecoFaturamento)
                {
                    proposta.Faturamento.UsinaCodigo = proposta.UsinaCodigo;
                    proposta.Faturamento.PropostaAno = proposta.Ano;
                    proposta.Faturamento.PropostaNumero = proposta.Numero;
                    _propostaRepository.Adicionar(proposta.Faturamento);
                }

                if (proposta.UtilizaDadosCobranca || proposta.UtilizaEnderecoCobranca)
                {
                    proposta.Cobranca.UsinaCodigo = proposta.UsinaCodigo;
                    proposta.Cobranca.PropostaAno = proposta.Ano;
                    proposta.Cobranca.PropostaNumero = proposta.Numero;
                    _propostaRepository.Adicionar(proposta.Cobranca);
                }

                proposta.Obra.NumChamada = proposta.Numero;
                proposta.Obra.ObraNova = (proposta.OrigemObraCodigo > 0 && proposta.OrigemUsinaCodigo > 0) ? "N" : "S";
                proposta.Obra.IdCadastro = proposta.IdCadastro;
                _obraService.Adicionar(proposta.Obra, proposta.ValorExtras);

                ValidarDemaisAprovacoes(usuario, proposta);

                _aprovacaoComercialPendenteService.ValidarComercialObraVersao(proposta.UsinaCodigo, proposta.Obra.Numero, proposta.Obra.NumeroVersao, usuario);

                _obraService.AtualizarStatusComercial(proposta.UsinaCodigo, proposta.Obra.Numero);
            }

        }

        public bool ValidarProposta(string usuario, Proposta proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores,
            string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0)
        {
            const int USINA_DEFAULT = 999;
            const int PRODUT_TIPO_DEFAULT = 8801;

            var utilizaAprovacaoComercialAlcada = _aprovacaoComercialService.UtilizaAprovacaoComercialPorAlcada(proposta.Obra.UsinaEntregaCodigo);

            if (!proposta.AdicionarPropostaScopeIsValid()) return false;

            if (!proposta.PropostaVendedorPermitidoScopeIsValid(filtroVendedores)) return false;

            if (!proposta.PropostaInscricaoEstadualIsValid(_intervenienteService)) return false;

            proposta.UsinaCodigo = USINA_DEFAULT;
            proposta.Ano = int.Parse($"{proposta.Data.Year}".Substring(2));
            proposta.Hora = int.Parse(DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0'));

            proposta.ProdutoTipoCodigo = PRODUT_TIPO_DEFAULT;
            proposta.TemObras = "N";

            _propostaRepository.SaveChanges();

            proposta.Obra.UsinaCodigo = USINA_DEFAULT;
            proposta.Obra.AnoChamada = proposta.Ano;

            var condicaoPagamentoPrincipal = _condicaoPagamentoService.ObterPrincipal(proposta.Obra.ObraPagamentos);

            if (condicaoPagamentoPrincipal != null)
            {
                proposta.Obra.CondicaoPagamentoCodigo = condicaoPagamentoPrincipal?.Codigo;

                proposta.Obra.TipoCobrancaCodigo = proposta.Obra.ObraPagamentos
                    .Where(t => t.CondicaoPagamentoCodigo == condicaoPagamentoPrincipal.Codigo && t.Ativo)
                    .Select(t => t.TipoCobrancaCodigo)
                    .FirstOrDefault();

                if (proposta.Obra.TipoCobrancaCodigo == 0)
                {
                    proposta.Obra.TipoCobrancaCodigo = proposta.Obra.ObraPagamentos
                        .Where(t => t.CondicaoPagamentoCodigo == condicaoPagamentoPrincipal.Codigo)
                        .Select(t => t.TipoCobrancaCodigo)
                        .FirstOrDefault();
                }
            }

            _propostaRepository.SaveChanges();

            var usinaEntrega = _obraService.ObterPorId<Usina>(proposta.Obra.UsinaEntregaCodigo);
            proposta.Obra.CalcularTempoDeCiclo(usinaEntrega);

            _propostaRepository.SaveChanges();

            var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(proposta.Data);

            var percentualDescontoLimite = _usuarioService.ObterPercentualDescontoLimitePorId(usuario)
                ?? parametroProposta?.PercentualDescontoLimite ?? 0f;

            var hasNotifications = false;
            var aprovacaoComercialPendente = false;

            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == usinaEntrega.Codigo).OrderByDescending(t => t.DataInicioVigencia).FirstOrDefault();

            if ((proposta.Obra.ObraTracos?.Count ?? 0 ) != 0)
            {
                proposta.TracoPrecoNumeroTabela = _tracoPrecoService
                    .ObterNumeroTabelaVigentePorDataBaseUsina(proposta.Data, proposta.Obra.UsinaEntregaCodigo);
            }

            _propostaRepository.SaveChanges();

            foreach (var obraTraco in proposta.Obra.ObraTracos ?? new List<ObraTraco>())
            {
                obraTraco.UsinaCodigo = USINA_DEFAULT;

                obraTraco.NumeracaoFamilia = _tracoPrecoService.ObterNumeracaoFamilia(proposta.Obra.UsinaEntregaCodigo, obraTraco.UsoCodigo, obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo);

                _obraService.ValidarObraTraco(usuario, obraTraco, proposta.Obra.UsinaEntregaCodigo, proposta.Data,
                    proposta.Obra.EnderecoCep, proposta.Obra.DistanciaUsina, proposta.Obra.CondicaoPagamentoCodigo ?? 0,
                    percentualDescontoLimite, proposta.Obra, ref aprovacaoComercialPendente, ref hasNotifications);

                if (obraTraco.ObraCodigo > 0)
                    _comercialLegacyService.VerificaRegrasAlteracaoTraco(obraTraco.UsinaCodigo, obraTraco.ObraCodigo, obraTraco.Sequencia, obraTraco.IdAlteracaoTracoPesado, proposta?.IntervenienteCodigo ?? 0, obraTraco.UsoCodigo, obraTraco.PedraCodigo, obraTraco.SlumpCodigo, obraTraco.ResistenciaTipoCodigo, obraTraco.Fck, obraTraco.Consumo, obraTraco.M3Quantidade);

                _obraService.CalcularEbitdaObraTraco(obraTraco, proposta.Obra);

                ValidarNumeracaoProdutoCorretaObraTraco(obraTraco, proposta.Obra.UsinaEntregaCodigo);
                
                _propostaRepository.SaveChanges();
            }

            foreach (var obraBomba in proposta.Obra.ObraBombas ?? new List<ObraBomba>())
            {
                obraBomba.UsinaCodigo = USINA_DEFAULT;

                _obraService.ValidarObraBomba(usuario, obraBomba, proposta.Obra.UsinaEntregaCodigo, DateTime.Today,
                    ref aprovacaoComercialPendente, ref hasNotifications);

                _obraService.CalcularEbitdaObraBomba(obraBomba, proposta.Obra);

                _propostaRepository.SaveChanges();
            }

            var taxas = _obraTaxaService.ListarByIdObra(proposta.Obra.UsinaEntregaCodigo, proposta.Obra.Numero);

            foreach (var obraTaxa in proposta.Obra.ObraTaxas ?? taxas ?? new List<ObraTaxa>())
            {
                var taxa = _obraService.ObterPorId<ObraTaxa>(obraTaxa.UsinaCodigo, obraTaxa.ObraCodigo, obraTaxa.Sequencia) ?? obraTaxa;
                _obraService.ValidarObraTaxa(usuario, taxa, ref aprovacaoComercialPendente);

                _propostaRepository.SaveChanges();
            }

            // Cadastra distancia de entrega caso ainda não tenha
            _usinaDistanciaCepService.AdicionarCasoNaoCadastrado(usuario, proposta.Obra.UsinaEntregaCodigo, proposta.Obra.EnderecoCep, proposta.Obra.DistanciaUsina);
            _propostaRepository.SaveChanges();

            if (proposta.EnderecoMunicipioCodigo == 0)
            {
                var enderecoProposta = _enderecoService.ObterPorCep(proposta.EnderecoCep);

                if (enderecoProposta != null)
                {
                    enderecoProposta.Municipio = _enderecoService.SalvarMunicipio(enderecoProposta.Municipio);
                    proposta.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    if (proposta.Interveniente != null)
                        proposta.Interveniente.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    _propostaRepository.SaveChanges();
                }
            }

            if (proposta.Obra.EnderecoMunicipioCodigo == 0)
            {
                var enderecoObra = _enderecoService.ObterPorCep(proposta.Obra.EnderecoCep);

                if (enderecoObra != null)
                {
                    enderecoObra.Municipio = _enderecoService.SalvarMunicipio(enderecoObra.Municipio);
                    proposta.Obra.EnderecoMunicipioCodigo = enderecoObra.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }
            }

            if (proposta.UtilizaEnderecoFaturamento && proposta.Faturamento.EnderecoMunicipioCodigo == 0)
            {
                var enderecoFaturamento = _enderecoService.ObterPorCep(proposta.Faturamento.EnderecoCep);

                if (enderecoFaturamento != null)
                {
                    enderecoFaturamento.Municipio = _enderecoService.SalvarMunicipio(enderecoFaturamento.Municipio);
                    proposta.Faturamento.EnderecoMunicipioCodigo = enderecoFaturamento.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }

            }

            if (proposta.UtilizaEnderecoCobranca && proposta.Cobranca.EnderecoMunicipioCodigo == 0)
            {
                var enderecoCobranca = _enderecoService.ObterPorCep(proposta.Cobranca.EnderecoCep);

                if (enderecoCobranca != null)
                {
                    enderecoCobranca.Municipio = _enderecoService.SalvarMunicipio(enderecoCobranca.Municipio);
                    proposta.Cobranca.EnderecoMunicipioCodigo = enderecoCobranca.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }
            }

            if (proposta.UtilizaResponsavelSolidario && proposta.ResponsavelSolidario.EnderecoMunicipioCodigo == 0)
            {
                var enderecoResponsavelSolidario = _enderecoService.ObterPorCep(proposta.ResponsavelSolidario.EnderecoCep);

                if (enderecoResponsavelSolidario != null)
                {
                    enderecoResponsavelSolidario.Municipio = _enderecoService.SalvarMunicipio(enderecoResponsavelSolidario.Municipio);
                    proposta.ResponsavelSolidario.EnderecoMunicipioCodigo = enderecoResponsavelSolidario.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }
            }

            // Salva endereços caso ainda não cadastrados
            if (proposta.EnderecoCep.Length == 8 && proposta.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.EnderecoCep, proposta.EnderecoMunicipioCodigo ?? 0, "Endereço cliente");

            if (proposta.Obra.EnderecoCep.Length == 8 && proposta.Obra.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.Obra.EnderecoCep, proposta.Obra.EnderecoMunicipioCodigo ?? 0, "Endereço obra");

            if (proposta.UtilizaEnderecoFaturamento && proposta.Faturamento.EnderecoCep.Length == 8 && proposta.Faturamento.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.Faturamento.EnderecoCep, proposta.Faturamento.EnderecoMunicipioCodigo ?? 0, "Endereço faturamento");
            if (proposta.UtilizaEnderecoCobranca && proposta.Cobranca.EnderecoCep.Length == 8 && proposta.Cobranca.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.Cobranca.EnderecoCep, proposta.Cobranca.EnderecoMunicipioCodigo ?? 0, "Endereço cobrança");
            if (proposta.UtilizaResponsavelSolidario && proposta.ResponsavelSolidario.EnderecoCep.Length == 8 && proposta.ResponsavelSolidario.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.ResponsavelSolidario.EnderecoCep, proposta.ResponsavelSolidario.EnderecoMunicipioCodigo ?? 0, "Endereço responsavel solidário");

            proposta.ValorConcreto = proposta.Obra.CalcularValorConcreto();
            var naoConsideraTodasBombas = _parametroService.ObterParametroN("TopCon", "NaoConsideraTodasBombas") == "1" ? true : false;
            proposta.ValorBomba = proposta.Obra.CalcularValorBomba(naoConsideraTodasBombas);

            var valorM3Faltante = _obraTaxaService.ObterValorM3Faltante(proposta.Obra.PossuiBomba(),
                proposta.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, proposta.Obra.VolumePorCarga, proposta.Obra.ObraTaxas ?? taxas);

            var valorAdicionalPorKmRodado = _obraTaxaService.ObterValorAdicionalPorKmRodado(proposta.Obra.DistanciaUsina,
                proposta.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, proposta.Obra.VolumePorCarga, proposta.Obra.ObraTaxas ?? taxas, proposta.Obra.PossuiBomba());

            //proposta.ValorExtras = valorM3Faltante + (proposta.Obra.VibradorValorUnitario * proposta.Obra.VibradorQuantidade) + valorAdicionalPorKmRodado;
            proposta.ValorExtras = valorM3Faltante + valorAdicionalPorKmRodado;

            _propostaRepository.SaveChanges();
            proposta.Obra.ValorDemaisServicos = proposta.Obra.CalcularValorDemaisServicos();
            _propostaRepository.SaveChanges();

            proposta.ValorTotalContrato = proposta.ValorConcreto + proposta.ValorBomba + (decimal)proposta.ValorExtras + (decimal)proposta.Obra.ValorDemaisServicos;
            proposta.VolumeTotal = proposta.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0;

            if (proposta.StatusProposta == EPropostaStatusCliente.Aprovado)
            {
                naoConsideraTodasBombas = _parametroService.ObterParametroN("TopCon", "NaoConsideraTodasBombas") == "1" ? true : false;
                hasNotifications |= !proposta.Obra.ValorTotalPagamentosScopeIsValid(proposta.ValorExtras, naoConsideraTodasBombas);
            }

            if (!hasNotifications)
            {
                if (proposta.StatusProposta == EPropostaStatusCliente.Aprovado && aprovacaoComercialPendente && proposta.Status != EPropostaStatus.AguardandoAprovacaoComercial)
                {
                    proposta.StatusAnterior = proposta.Status;
                    proposta.Status = EPropostaStatus.AguardandoAprovacaoComercial;
                }
                else if (proposta.Status == EPropostaStatus.AguardandoAprovacaoComercial && !aprovacaoComercialPendente)
                {
                    switch (proposta.StatusProposta)
                    {
                        case EPropostaStatusCliente.EmNegociacao:
                            proposta.StatusAnterior = 0;
                            proposta.Status = EPropostaStatus.Andamento;
                            break;
                        case EPropostaStatusCliente.Aprovado:
                            proposta.StatusAnterior = 0;
                            proposta.Status = proposta.Obra.NumContrato > 0 ? EPropostaStatus.ContratoGerado : EPropostaStatus.AprovadaPeloCliente;
                            break;
                        case EPropostaStatusCliente.Perdido:
                            proposta.StatusAnterior = 0;
                            proposta.Status = EPropostaStatus.Perdida;
                            break;
                    }
                }

                _propostaRepository.SaveChanges();
            }
            return !hasNotifications;
        }

        public bool ValidarProposta(string usuario, PropostaVersao proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores,
            string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0)
        {
            const int USINA_DEFAULT = 999;
            const int PRODUT_TIPO_DEFAULT = 8801;

            if (!proposta.AdicionarPropostaScopeIsValid()) return false;

            if (!proposta.PropostaVendedorPermitidoScopeIsValid(filtroVendedores)) return false;

            if (!proposta.PropostaInscricaoEstadualIsValid(_intervenienteService)) return false;

            proposta.UsinaCodigo = USINA_DEFAULT;
            proposta.Ano = int.Parse($"{proposta.Data.Year}".Substring(2));
            proposta.Hora = int.Parse(DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0'));

            proposta.ProdutoTipoCodigo = PRODUT_TIPO_DEFAULT;
            proposta.TemObras = "N";

			var utilizaAprovacaoComercialAlcada = _aprovacaoComercialService.UtilizaAprovacaoComercialPorAlcada(proposta.Obra.UsinaEntregaCodigo);

            _propostaRepository.SaveChanges();

            proposta.Obra.UsinaCodigo = USINA_DEFAULT;
            proposta.Obra.AnoChamada = proposta.Ano;

            var condicaoPagamentoPrincipal = _condicaoPagamentoService.ObterPrincipal(proposta.Obra.ObraPagamentos);

            if (condicaoPagamentoPrincipal != null)
            {
                proposta.Obra.CondicaoPagamentoCodigo = condicaoPagamentoPrincipal?.Codigo;

                proposta.Obra.TipoCobrancaCodigo = proposta.Obra.ObraPagamentos
                    .Where(t => t.CondicaoPagamentoCodigo == condicaoPagamentoPrincipal.Codigo && t.Ativo)
                    .Select(t => t.TipoCobrancaCodigo)
                    .FirstOrDefault();

                if (proposta.Obra.TipoCobrancaCodigo == 0)
                {
                    proposta.Obra.TipoCobrancaCodigo = proposta.Obra.ObraPagamentos
                        .Where(t => t.CondicaoPagamentoCodigo == condicaoPagamentoPrincipal.Codigo)
                        .Select(t => t.TipoCobrancaCodigo)
                        .FirstOrDefault();
                }
            }

            _propostaRepository.SaveChanges();

            var usinaEntrega = _obraService.ObterPorId<Usina>(proposta.Obra.UsinaEntregaCodigo);
            proposta.Obra.CalcularTempoDeCiclo(usinaEntrega);

            _propostaRepository.SaveChanges();

            var parametroProposta = _parametroService.ObterPorDataBase<ParametroProposta>(proposta.Data);

            var percentualDescontoLimite = _usuarioService.ObterPercentualDescontoLimitePorId(usuario)
                ?? parametroProposta?.PercentualDescontoLimite ?? 0f;

            var hasNotifications = false;
            var aprovacaoComercialPendente = false;

            var custoServicoVigente = _custoServicoService.ListarFiltrados(t => t.UsinaCodigo == usinaEntrega.Codigo).OrderByDescending(t => t.DataInicioVigencia).FirstOrDefault();

            if ((proposta.Obra.ObraTracos?.Count ?? 0) != 0)
            {
                proposta.TracoPrecoNumeroTabela = _tracoPrecoService
                    .ObterNumeroTabelaVigentePorDataBaseUsina(proposta.Data, proposta.Obra.UsinaEntregaCodigo);
            }

            _propostaRepository.SaveChanges();

            foreach (var obraTracoVersao in proposta.Obra.ObraTracos ?? new List<ObraTracoVersao>())
            {
                obraTracoVersao.UsinaCodigo = USINA_DEFAULT;

                obraTracoVersao.NumeracaoFamilia = _tracoPrecoService.ObterNumeracaoFamilia(proposta.Obra.UsinaEntregaCodigo, obraTracoVersao.UsoCodigo, obraTracoVersao.PedraCodigo, obraTracoVersao.SlumpCodigo, obraTracoVersao.ResistenciaTipoCodigo, obraTracoVersao.Fck, obraTracoVersao.Consumo);

                _obraService.ValidarObraTraco(usuario, obraTracoVersao, proposta.Obra.UsinaEntregaCodigo, proposta.Data,
                    proposta.Obra.EnderecoCep, proposta.Obra.DistanciaUsina, proposta.Obra.CondicaoPagamentoCodigo ?? 0,
                    percentualDescontoLimite, proposta.Obra, ref aprovacaoComercialPendente, ref hasNotifications);

                if (obraTracoVersao.ObraCodigo > 0)
                    _comercialLegacyService.VerificaRegrasAlteracaoTraco(obraTracoVersao.NumeroVersao, obraTracoVersao.UsinaCodigo, obraTracoVersao.ObraCodigo, obraTracoVersao.Sequencia, obraTracoVersao.IdAlteracaoTracoPesado, proposta?.IntervenienteCodigo ?? 0, obraTracoVersao.UsoCodigo, obraTracoVersao.PedraCodigo, obraTracoVersao.SlumpCodigo, obraTracoVersao.ResistenciaTipoCodigo, obraTracoVersao.Fck, obraTracoVersao.Consumo, obraTracoVersao.M3Quantidade);

                _obraService.CalcularEbitdaObraTraco(obraTracoVersao, proposta.Obra);

                ValidarNumeracaoProdutoCorretaObraTracoVersao(obraTracoVersao, proposta.Obra.UsinaEntregaCodigo);

                _propostaRepository.SaveChanges();
            }

            foreach (var obraBombaVersao in proposta.Obra.ObraBombas ?? new List<ObraBombaVersao>())
            {
                obraBombaVersao.UsinaCodigo = USINA_DEFAULT;

                _obraService.ValidarObraBomba(usuario, obraBombaVersao, proposta.Obra.UsinaEntregaCodigo, DateTime.Today,
                    ref aprovacaoComercialPendente, ref hasNotifications);

                _obraService.CalcularEbitdaObraBomba(obraBombaVersao, proposta.Obra);

                _propostaRepository.SaveChanges();
            }

            var taxas = _obraTaxaService.ListarByIdObra(proposta.Obra.UsinaEntregaCodigo, proposta.Obra.Numero, proposta.NumeroVersao);

            foreach (var obraTaxaVersao in proposta.Obra.ObraTaxas ?? taxas ?? new List<ObraTaxaVersao>())
            {
                var taxa = _obraService.ObterPorId<ObraTaxaVersao>(obraTaxaVersao.NumeroVersao, obraTaxaVersao.UsinaCodigo, obraTaxaVersao.ObraCodigo, obraTaxaVersao.Sequencia) ?? obraTaxaVersao;
                _obraService.ValidarObraTaxa(usuario, taxa, ref aprovacaoComercialPendente);
                

                _propostaRepository.SaveChanges();
            }

            // Cadastra distancia de entrega caso ainda não tenha
            _usinaDistanciaCepService.AdicionarCasoNaoCadastrado(usuario, proposta.Obra.UsinaEntregaCodigo, proposta.Obra.EnderecoCep, proposta.Obra.DistanciaUsina);
            _propostaRepository.SaveChanges();

            if (proposta.EnderecoMunicipioCodigo == 0)
            {
                var enderecoProposta = _enderecoService.ObterPorCep(proposta.EnderecoCep);

                if (enderecoProposta != null)
                {
                    enderecoProposta.Municipio = _enderecoService.SalvarMunicipio(enderecoProposta.Municipio);
                    proposta.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    if (proposta.Interveniente != null)
                        proposta.Interveniente.EnderecoMunicipioCodigo = enderecoProposta.Municipio.Codigo;

                    _propostaRepository.SaveChanges();
                }
            }

            if (proposta.Obra.EnderecoMunicipioCodigo == 0)
            {
                var enderecoObra = _enderecoService.ObterPorCep(proposta.Obra.EnderecoCep);

                if (enderecoObra != null)
                {
                    enderecoObra.Municipio = _enderecoService.SalvarMunicipio(enderecoObra.Municipio);
                    proposta.Obra.EnderecoMunicipioCodigo = enderecoObra.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }
            }

            if (proposta.UtilizaEnderecoFaturamento && proposta.Faturamento.EnderecoMunicipioCodigo == 0)
            {
                var enderecoFaturamento = _enderecoService.ObterPorCep(proposta.Faturamento.EnderecoCep);

                if (enderecoFaturamento != null)
                {
                    enderecoFaturamento.Municipio = _enderecoService.SalvarMunicipio(enderecoFaturamento.Municipio);
                    proposta.Faturamento.EnderecoMunicipioCodigo = enderecoFaturamento.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }

            }

            if (proposta.UtilizaEnderecoCobranca && proposta.Cobranca.EnderecoMunicipioCodigo == 0)
            {
                var enderecoCobranca = _enderecoService.ObterPorCep(proposta.Cobranca.EnderecoCep);

                if (enderecoCobranca != null)
                {
                    enderecoCobranca.Municipio = _enderecoService.SalvarMunicipio(enderecoCobranca.Municipio);
                    proposta.Cobranca.EnderecoMunicipioCodigo = enderecoCobranca.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }
            }

            if (proposta.UtilizaResponsavelSolidario && proposta.ResponsavelSolidario.EnderecoMunicipioCodigo == 0)
            {
                var enderecoResponsavelSolidario = _enderecoService.ObterPorCep(proposta.ResponsavelSolidario.EnderecoCep);

                if (enderecoResponsavelSolidario != null)
                {
                    enderecoResponsavelSolidario.Municipio = _enderecoService.SalvarMunicipio(enderecoResponsavelSolidario.Municipio);
                    proposta.ResponsavelSolidario.EnderecoMunicipioCodigo = enderecoResponsavelSolidario.Municipio.Codigo;
                    _propostaRepository.SaveChanges();
                }
            }

            // Salva endereços caso ainda não cadastrados
            if (proposta.EnderecoCep.Length == 8 && proposta.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.EnderecoCep, proposta.EnderecoMunicipioCodigo ?? 0, "Endereço cliente");

            if (proposta.Obra.EnderecoCep.Length == 8 && proposta.Obra.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.Obra.EnderecoCep, proposta.Obra.EnderecoMunicipioCodigo ?? 0, "Endereço obra");

            if (proposta.UtilizaEnderecoFaturamento && proposta.Faturamento.EnderecoCep.Length == 8 && proposta.Faturamento.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.Faturamento.EnderecoCep, proposta.Faturamento.EnderecoMunicipioCodigo ?? 0, "Endereço faturamento");
            if (proposta.UtilizaEnderecoCobranca && proposta.Cobranca.EnderecoCep.Length == 8 && proposta.Cobranca.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.Cobranca.EnderecoCep, proposta.Cobranca.EnderecoMunicipioCodigo ?? 0, "Endereço cobrança");
            if (proposta.UtilizaResponsavelSolidario && proposta.ResponsavelSolidario.EnderecoCep.Length == 8 && proposta.ResponsavelSolidario.EnderecoMunicipioCodigo > 0)
                _enderecoService.Salvar(proposta.ResponsavelSolidario.EnderecoCep, proposta.ResponsavelSolidario.EnderecoMunicipioCodigo ?? 0, "Endereço responsavel solidário");

            proposta.ValorConcreto = proposta.Obra.CalcularValorConcreto();
            var naoConsideraTodasBombas = _parametroService.ObterParametroN("TopCon", "NaoConsideraTodasBombas") == "1" ? true : false;
            proposta.ValorBomba = proposta.Obra.CalcularValorBomba(naoConsideraTodasBombas);

            var valorM3Faltante = _obraTaxaService.ObterValorM3Faltante(proposta.Obra.PossuiBomba(),
                proposta.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, proposta.Obra.VolumePorCarga, proposta.Obra.ObraTaxas ?? taxas);

            var valorAdicionalPorKmRodado = _obraTaxaService.ObterValorAdicionalPorKmRodado(proposta.Obra.DistanciaUsina,
                proposta.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0, proposta.Obra.VolumePorCarga, proposta.Obra.ObraTaxas ?? taxas, proposta.Obra.PossuiBomba());

            //proposta.ValorExtras = valorM3Faltante + (proposta.Obra.VibradorValorUnitario * proposta.Obra.VibradorQuantidade) + valorAdicionalPorKmRodado;
            proposta.ValorExtras = valorM3Faltante + valorAdicionalPorKmRodado;

            _propostaRepository.SaveChanges();
            proposta.Obra.ValorDemaisServicos = proposta.Obra.CalcularValorDemaisServicos();
            _propostaRepository.SaveChanges();

            proposta.ValorTotalContrato = proposta.ValorConcreto + proposta.ValorBomba + (decimal)proposta.ValorExtras + (decimal)proposta.Obra.ValorDemaisServicos;
            proposta.VolumeTotal = proposta.Obra.ObraTracos?.Sum(t => t.M3Quantidade) ?? 0;

            if (proposta.StatusProposta == EPropostaStatusCliente.Aprovado)
            {
                naoConsideraTodasBombas = _parametroService.ObterParametroN("TopCon", "NaoConsideraTodasBombas") == "1" ? true : false;
                hasNotifications |= !proposta.Obra.ValorTotalPagamentosScopeIsValid(_obraService, proposta.ValorExtras, naoConsideraTodasBombas);
            }

            if (!hasNotifications)
            {
                if (proposta.StatusProposta == EPropostaStatusCliente.Aprovado && aprovacaoComercialPendente && proposta.Status != EPropostaStatus.AguardandoAprovacaoComercial)
                {
                    proposta.StatusAnterior = proposta.Status;
                    proposta.Status = EPropostaStatus.AguardandoAprovacaoComercial;
                }
                else if (proposta.Status == EPropostaStatus.AguardandoAprovacaoComercial && !aprovacaoComercialPendente)
                {
                    switch (proposta.StatusProposta)
                    {
                        case EPropostaStatusCliente.EmNegociacao:
                            proposta.StatusAnterior = 0;
                            proposta.Status = EPropostaStatus.Andamento;
                            break;
                        case EPropostaStatusCliente.Aprovado:
                            proposta.StatusAnterior = 0;
                            proposta.Status = proposta.Obra.NumContrato > 0 ? EPropostaStatus.ContratoGerado : EPropostaStatus.AprovadaPeloCliente;
                            break;
                        case EPropostaStatusCliente.Perdido:
                            proposta.StatusAnterior = 0;
                            proposta.Status = EPropostaStatus.Perdida;
                            break;
                    }
                }

                _propostaRepository.SaveChanges();
            }
            return !hasNotifications;
        }

        public void ValidarDemaisAprovacoes(string usuario, Proposta proposta, string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0)
        {
            var validaVendedorExclusivo = _parametroService.ObterParametroN("web", "ValidaVendedorExclusivo").Trim() == "1";

            if (!validaVendedorExclusivo)
                return;

            if (proposta.EmissaoPropostaAprovada != "S" && proposta.CpfCnpj != "")
            {                
                var cliente = _intervenienteService.ObterPorCpfCnpj(proposta.CpfCnpj, proposta.InscricaoEstadual);

                var necessitaAprovacaoVendedorExclusivo = !proposta.PropostaVendedorExclusivoScopeIsValid(cliente);

                var temPropostaEmAberto = _propostaRepository
                    .ListarFiltrados(t => t.CpfCnpj == proposta.CpfCnpj
                        && t.Status != EPropostaStatus.Perdida
                        && t.Status != EPropostaStatus.ContratoGerado
                        && t.Status != EPropostaStatus.ReprovadaComercialmente
                        && t.Status != EPropostaStatus.Reprovada
                        && !(t.UsinaCodigo == proposta.UsinaCodigo && t.Ano == proposta.Ano && t.Numero == proposta.Numero))
                    .Count() > 0;

                if (cliente != null && !temPropostaEmAberto)
                {
                    temPropostaEmAberto = _contratoService
                        .ListarFiltrados(t => t.IntervenienteCodigo == cliente.Codigo
                            && t.DataEncerramento == null)
                        .Count() > 0;
                }

                var demaisAprov = _demaisAprovacaoService.BuscarDemaisAprovacaoByIdObra(proposta.Obra.UsinaCodigo, proposta.Obra.Numero, usuario);
                foreach (var aprov in demaisAprov)
                {
                    if (aprov.AprovacaoTipoCodigo == 1 && aprov.StatusAprovacao == EStatusAprovacao.Pendente)
                    {
                        _demaisAprovacaoService.RemoverAprovacoes(aprov.Chave);
                    }
                }

                if ((necessitaAprovacaoVendedorExclusivo && temPropostaEmAberto) || necessitaAprovacaoVendedorExclusivo)
                {
                    var demaisAprovacao = new DemaisAprovacao();

                    demaisAprovacao.Chave = Guid.NewGuid().ToString();

                    demaisAprovacao.Complemento = $"{proposta.UsinaCodigo}/{proposta.Numero}/{proposta.Ano} - {cliente?.Codigo ?? 0}";

                    demaisAprovacao.AprovacaoTipoCodigo = 1;

                    demaisAprovacao.DataHoraSolicitacao = DateTime.Now;

                    demaisAprovacao.UsuarioRequisitante = usuario;

                    var scriptAprovacao = "UPDATE con_chtel"
                        + " SET status=status_anterior, status_anterior=0, Prop_Emi_Aprov = 'S'"
                        + $" WHERE usina={proposta.UsinaCodigo} AND ano_chamada={proposta.Ano}"
                        + $" AND num_chamada={proposta.Numero} AND no_obra={proposta.Obra.Numero} AND status_anterior<>0;";

                    if (necessitaAprovacaoVendedorExclusivo)
                    {
                        demaisAprovacao.Complemento += " - Vend.Exclusivo";
                        var vendedorAnteriorNome = _vendedorService.ObterPorId(cliente.VendedorCodigo)?.Nome ?? "";
                        demaisAprovacao.Observacao += $"Atual: {vendedorAnteriorNome} Novo: {_vendedorService.ObterPorId(proposta.VendedorCodigo)?.Nome ?? ""}";
                        scriptAprovacao += $" UPDATE ger_interv SET Vend={proposta.VendedorCodigo} WHERE CNPJ_CPF='{proposta.CpfCnpj}';";
                    }

                    if (temPropostaEmAberto)
                        demaisAprovacao.Complemento += " - Prop. em Aberto";

                    var scriptReprovacao = "UPDATE con_chtel"
                        + $" SET status={(int)EPropostaStatus.ReprovadaComercialmente}, Prop_Emi_Aprov='N'"
                        + $" WHERE usina={proposta.UsinaCodigo} AND ano_chamada={proposta.Ano}"
                        + $" AND num_chamada={proposta.Numero} AND no_obra={proposta.Obra.Numero};";

                    _obraService.Adicionar(demaisAprovacao);

                    _obraService.Adicionar(new AprovacaoScript()
                    {
                        Chave = demaisAprovacao.Chave,
                        OperacaoTipo = "A",
                        Script = scriptAprovacao,
                        Status = ""
                    });

                    _obraService.Adicionar(new AprovacaoScript()
                    {
                        Chave = demaisAprovacao.Chave,
                        OperacaoTipo = "R",
                        Script = scriptReprovacao,
                        Status = ""
                    });
                }
            }
        }

        public void ValidarDemaisAprovacoes(string usuario, PropostaVersao proposta, string cpfCnpjAnterior = "", EPropostaStatus statusAnterior = 0)
        {
            var validaVendedorExclusivo = _parametroService.ObterParametroN("web", "ValidaVendedorExclusivo").Trim() == "1";

            if (!validaVendedorExclusivo)
                return;

            if (proposta.EmissaoPropostaAprovada != "S" && proposta.CpfCnpj != "")
            {
                var cliente = _intervenienteService.ObterPorCpfCnpj(proposta.CpfCnpj, proposta.InscricaoEstadual);

                var necessitaAprovacaoVendedorExclusivo = !proposta.PropostaVendedorExclusivoScopeIsValid(cliente);

                var temPropostaEmAberto = _propostaRepository
                    .ListarFiltrados<PropostaVersao>(t => t.CpfCnpj == proposta.CpfCnpj
                        && t.Status != EPropostaStatus.Perdida
                        && t.Status != EPropostaStatus.ContratoGerado
                        && t.Status != EPropostaStatus.ReprovadaComercialmente
                        && t.Status != EPropostaStatus.Reprovada
                        && !(t.NumeroVersao == proposta.NumeroVersao && t.UsinaCodigo == proposta.UsinaCodigo && t.Ano == proposta.Ano && t.Numero == proposta.Numero))
                    .Count() > 0;

                if (cliente != null && !temPropostaEmAberto)
                {
                    temPropostaEmAberto = _contratoService
                        .ListarFiltrados(t => t.IntervenienteCodigo == cliente.Codigo
                            && t.DataEncerramento == null)
                        .Count() > 0;
                }

                if ((necessitaAprovacaoVendedorExclusivo && temPropostaEmAberto) || necessitaAprovacaoVendedorExclusivo)
                {
                    var demaisAprovacao = new DemaisAprovacao();

                    demaisAprovacao.Chave = Guid.NewGuid().ToString();

                    demaisAprovacao.Complemento = $"{proposta.UsinaCodigo}/{proposta.Numero}/{proposta.Ano} - {cliente?.Codigo ?? 0}";

                    demaisAprovacao.AprovacaoTipoCodigo = 1;

                    demaisAprovacao.DataHoraSolicitacao = DateTime.Now;

                    demaisAprovacao.UsuarioRequisitante = usuario;

                    var scriptAprovacao = "UPDATE con_chtel_versao"
                        + " SET status=status_anterior, status_anterior=0, Prop_Emi_Aprov = 'S'"
                        + $" WHERE usina={proposta.UsinaCodigo} AND ano_chamada={proposta.Ano} and num_versao={proposta.NumeroVersao}"
                        + $" AND num_chamada={proposta.Numero} AND no_obra={proposta.Obra.Numero} AND status_anterior<>0;";

                    if (necessitaAprovacaoVendedorExclusivo)
                    {
                        demaisAprovacao.Complemento += " - Vend.Exclusivo";
                        var vendedorAnteriorNome = _vendedorService.ObterPorId(cliente.VendedorCodigo)?.Nome ?? "";
                        demaisAprovacao.Observacao += $"Atual: {vendedorAnteriorNome} Novo: {_vendedorService.ObterPorId(proposta.VendedorCodigo)?.Nome ?? ""}";
                        scriptAprovacao += $" UPDATE ger_interv SET Vend={proposta.VendedorCodigo} WHERE CNPJ_CPF='{proposta.CpfCnpj}';";
                    }

                    if (temPropostaEmAberto)
                        demaisAprovacao.Complemento += " - Prop. em Aberto";

                    var scriptReprovacao = "UPDATE con_chtel_versao"
                        + $" SET status={(int)EPropostaStatus.ReprovadaComercialmente}, Prop_Emi_Aprov='N'"
                        + $" WHERE usina={proposta.UsinaCodigo} AND ano_chamada={proposta.Ano} and num_versao={proposta.NumeroVersao}"
                        + $" AND num_chamada={proposta.Numero} AND no_obra={proposta.Obra.Numero};";

                    _obraService.Adicionar(demaisAprovacao);

                    _obraService.Adicionar(new AprovacaoScript()
                    {
                        Chave = demaisAprovacao.Chave,
                        OperacaoTipo = "A",
                        Script = scriptAprovacao,
                        Status = ""
                    });

                    _obraService.Adicionar(new AprovacaoScript()
                    {
                        Chave = demaisAprovacao.Chave,
                        OperacaoTipo = "R",
                        Script = scriptReprovacao,
                        Status = ""
                    });
                }
            }
        }

        public PagedList<Proposta> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Proposta, bool>> filter, bool divergenciaCarteira, EStatusClicksignDocumento? statusClicksignDocumento, bool propostaComContrato = false)
        {
            var pagedList = _propostaRepository.ListarEmOrdemDecrescente(pagina, porPagina, filter, divergenciaCarteira, statusClicksignDocumento, propostaComContrato);

            return pagedList;
        }

        public Proposta ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false)
        {
            var proposta = _propostaRepository.ObterPorUsinaAnoNumero(idUsina, ano, numero, tracking);

            if (proposta != null)
            {
                foreach (var traco in proposta.Obra.ObraTracos)
                {
                    if (_usoService.PossuiDescricaoPersonalizada(traco.UsoCodigo) || _mercadoriaService.TracoCriadoPeloTech(traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo))
                    {
                        var mercadoria = _mercadoriaService.ObterTracoMercadoria(traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo);
                        if (mercadoria != null)
                            traco.DescricaoPersonalizada = mercadoria.Descricao;
                    }
                }
            }

            return proposta;
        }

        public PropostaVersao ObterPorUsinaAnoNumero(int numVersao, int idUsina, int ano, int numero, bool tracking = false)
        {
            var proposta = _propostaRepository.ObterPorUsinaAnoNumero(numVersao, idUsina, ano, numero, tracking);

            if (proposta != null)
            {
                foreach (var traco in proposta.Obra.ObraTracos)
                {
                    if (_usoService.PossuiDescricaoPersonalizada(traco.UsoCodigo) || _mercadoriaService.TracoCriadoPeloTech(traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo))
                    {
                        var mercadoria = _mercadoriaService.ObterTracoMercadoria(traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo);
                        if (mercadoria != null)
                            traco.DescricaoPersonalizada = mercadoria.Descricao;
                    }
                }
            }

            return proposta;
        }

        public PagedList<Proposta> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina)
        {
            var pagedList = _propostaRepository.ListarPorCpfCnpj(cpfCnpj, pagina, porPagina);

            return pagedList;
        }

        public float ObterVolumeTotalProposto(int idUsina, int ano, int numero)
        {
            return _propostaRepository.ObterVolumeTotalProposto(idUsina, ano, numero);
        }

        public void AtualizarStatusProposta(Proposta proposta, string usuario)
        {
            var obra = proposta.Obra;
            obra.AtualizarStatusAprovacao(usuario);
            var temReprovacao = (obra.ObraTracos?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraBombas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraTaxas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.DemaisAprovacoes?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            if (proposta.Status == EPropostaStatus.ReprovadaComercialmente && !temReprovacao)
                proposta.Status = EPropostaStatus.Andamento;


            _propostaRepository.SaveChanges();
        }

        public void AtualizarStatusProposta(PropostaVersao proposta, string usuario)
        {
            var obra = proposta.Obra;
            obra.AtualizarStatusAprovacao(usuario);
            var temReprovacao = (obra.ObraTracos?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraBombas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.ObraTaxas?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            temReprovacao |= (obra.DemaisAprovacoes?.Count(t => t.StatusAprovacao == EStatusAprovacao.Reprovado) ?? 0) > 0;
            if (proposta.Status == EPropostaStatus.ReprovadaComercialmente && !temReprovacao)
                proposta.Status = EPropostaStatus.Andamento;


            _propostaRepository.SaveChanges();
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            _propostaRepository.AdicionarVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            _propostaRepository.ExcluirVersaoContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            _propostaRepository.AdicionarContrato(codUsina, anoContrato, numeroContrato, numVersao);
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            _propostaRepository.ExcluirContrato(codUsina, anoContrato, numeroContrato);
        }

        public PropostaVersao ObterPorId(int codUsina, int anoProposta, int numeroProposta, int numVersao)
        {
            return _propostaRepository.ObterPorId(codUsina, anoProposta, numeroProposta, numVersao);
        }

        public PropostaVersao ObterVersaoPorIdForList(int codUsina, int anoProposta, int numeroProposta, int numVersao)
        {
            return _propostaRepository.ObterVersaoPorIdForList(codUsina, anoProposta, numeroProposta, numVersao);
        }

        public int GetUltimaVersaoProposta(int codUsina, int anoProposta, int numeroProposta)
        {
            return _propostaRepository.GetUltimaVersaoProposta(codUsina, anoProposta, numeroProposta);
        }

        public void SalvarPDFPropostaVersao(int versao, int codUsina, int anoChamada, int numeroChamada, Stream contrato)
        {
            var chave = ObterChavePropostaVersao(versao, codUsina, anoChamada, numeroChamada);
            _arquivoService.SalvarArquivo(NUMERO_APLICACAO_PROPOSTA_LISTA, chave, contrato);
        }

        private string ObterChavePropostaVersao(int versao, int codUsina, int anoChamada, int numeroChamada)
        {
            return $"{codUsina};{anoChamada};{numeroChamada};{versao}";
        }

        public Stream ObterPDFPropostaVersao(int versao, int codUsina, int anoChamada, int numeroChamada)
        {
            var chave = ObterChavePropostaVersao(versao, codUsina, anoChamada, numeroChamada);
            return _arquivoService.ObterArquivo(NUMERO_APLICACAO_PROPOSTA_LISTA, chave);
        }

        public void ValidarNumeracaoProdutoCorretaObraTraco(ObraTraco obraTraco, int obraUsina)
        {

            var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                obraUsina,
                obraTraco.UsoCodigo,
                obraTraco.PedraCodigo,
                obraTraco.SlumpCodigo,
                obraTraco.ResistenciaTipoCodigo,
                obraTraco.Fck,
                obraTraco.Consumo, false);

            if (tracoPreco is null)
                return;

            if (obraTraco.NumeracaoProduto != tracoPreco.NumeracaoProduto)
            {
                AssertionConcern.Notify("ObraTracos", $"Uso ({obraTraco.UsoCodigo}) selecionado não pertence à numeração do produto ({obraTraco.NumeracaoProduto}).");
            }
        }

        public void ValidarNumeracaoProdutoCorretaObraTracoVersao(ObraTracoVersao obraTraco, int obraUsina)
        {

            var tracoPreco = _tracoPrecoService.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                obraUsina,
                obraTraco.UsoCodigo,
                obraTraco.PedraCodigo,
                obraTraco.SlumpCodigo,
                obraTraco.ResistenciaTipoCodigo,
                obraTraco.Fck,
                obraTraco.Consumo, false);

            if (tracoPreco is null)
                return;

            if (obraTraco.NumeracaoProduto != tracoPreco.NumeracaoProduto)
            {
                AssertionConcern.Notify("ObraTracos", $"Uso ({obraTraco.UsoCodigo}) selecionado não pertence à numeração do produto ({obraTraco.NumeracaoProduto}).");
            }
        }

        public void VerificaTracoJaIncluso(ObraTraco obraTraco)
        {
            var traco = _obraService.ListarFiltradosTracking<ObraTraco>(t => t.UsinaCodigo == obraTraco.UsinaCodigo && t.ObraCodigo == obraTraco.ObraCodigo
                    && t.UsoCodigo == obraTraco.UsoCodigo && t.PedraCodigo == obraTraco.PedraCodigo && t.SlumpNominalCodigo == obraTraco.SlumpNominalCodigo && t.ResistenciaTipoCodigo == obraTraco.ResistenciaTipoCodigo && t.Consumo == obraTraco.Consumo && t.Fck == obraTraco.Fck,
                    t => t.ResistenciaTipo, t => t.Pedra, t => t.SlumpNominal, t => t.Uso)
                    .FirstOrDefault();

            if (traco is null)
                return;
            else
                AssertionConcern.Notify("ObraTracos", $"A inclusão do traço não é permitida, pois o mesmo já se encontra na proposta atual. (Numeração do produto: {obraTraco.NumeracaoProduto} - Uso: {obraTraco.UsoCodigo})");
        }

        public void VerificaTracoJaInclusoVersao(ObraTracoVersao obraTraco, int numVersao)
        {
            var tracoVersao = _obraService.ListarFiltradosTracking<ObraTracoVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == obraTraco.UsinaCodigo && t.ObraCodigo == obraTraco.ObraCodigo
                    && t.UsoCodigo == obraTraco.UsoCodigo && t.PedraCodigo == obraTraco.PedraCodigo && t.SlumpNominalCodigo == obraTraco.SlumpNominalCodigo && t.ResistenciaTipoCodigo == obraTraco.ResistenciaTipoCodigo && t.Consumo == obraTraco.Consumo && t.Fck == obraTraco.Fck,   
                    t => t.ResistenciaTipo, t => t.Pedra, t => t.SlumpNominal, t => t.Uso)
                    .FirstOrDefault();

            if (tracoVersao is null)
                return;
            else
                AssertionConcern.Notify("ObraTracos", $"A inclusão do traço não é permitida, pois o mesmo já se encontra na proposta atual. (Numeração do produto: {obraTraco.NumeracaoProduto} - Uso: {obraTraco.UsoCodigo})");
        }
    }
}
