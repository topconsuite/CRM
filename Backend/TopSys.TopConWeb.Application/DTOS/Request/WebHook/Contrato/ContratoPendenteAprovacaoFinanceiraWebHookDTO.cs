using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook
{
    public class ContratoPendenteAprovacaoFinanceiraWebHookDTO
    {
        public ContratoPendenteAprovacaoFinanceiraWebHookDTO(
            int intervenienteCodigo,
            string intervenienteExternalId,
            string intervenienteCPFCNPJ,
            long codigoObra,
            string nomeObra,
            IEnumerable<ObraTributacaoMunicipalDTO> obraTributacoesMunicipais,
            int usina,
            int usinaEntrega,
            int ano,
            int numero,
            DateTime? dataContrato,
            string numeroContratoAnterior,
            string enderecoCep,
            string enderecoLogradouro,
            int enderecoNumero,
            string enderecoComplemento,
            string enderecoBairro,
            int? enderecoMunicipioCodigo,
            string enderecoUf,
            string enderecoMunicipioNome,
            string contratoFinalidade,
            string emailObra,
            string cei,
            string codigoObraPrefeitura,
            string codigoBeneficioFiscal,
            string status,
            string observacaoNf,
            ContratoDadosFaturamentoDTO dadosFaturamento,
            ContratoEnderecoFaturamentoDTO enderecoFaturamento,
            ContratoDadosCobrancaDTO dadosCobranca,
            ContratoEnderecoCobrancaDTO enderecoCobranca,
            IEnumerable<ContratoPagamentoDTO> pagamentos,
            VendedorDTO vendedor,
            CondicaoPagamentoWebHookDTO condicaoPagamento,
            TipoCobrancaWebhookDTO tipoCobranca)
        {
            IntervenienteCodigo = intervenienteCodigo;
            IntervenienteExternalId = intervenienteExternalId;
            IntervenienteCPFCNPJ = intervenienteCPFCNPJ;
            CodigoObra = codigoObra;
            NomeObra = nomeObra;
            ObraTributacoesMunicipais = obraTributacoesMunicipais;
            Usina = usina;
            UsinaEntrega = usinaEntrega;
            Ano = ano;
            Numero = numero;
            DataContrato = dataContrato;
            NumeroContratoAnterior = numeroContratoAnterior;
            EnderecoCep = enderecoCep;
            EnderecoLogradouro = enderecoLogradouro;
            EnderecoNumero = enderecoNumero;
            EnderecoComplemento = enderecoComplemento;
            EnderecoBairro = enderecoBairro;
            EnderecoMunicipioCodigo = enderecoMunicipioCodigo;
            EnderecoUf = enderecoUf;
            EnderecoMunicipio = enderecoMunicipioNome;
            ContratoFinalidade = contratoFinalidade;
            EmailObra = emailObra;
            Cei = cei;
            CodigoObraPrefeitura = codigoObraPrefeitura;
            CodigoBeneficioFiscal = codigoBeneficioFiscal;
            Pagamentos = pagamentos;
            DadosFaturamento = dadosFaturamento;
            EnderecoFaturamento = enderecoFaturamento;
            DadosCobranca = dadosCobranca;
            EnderecoCobranca = enderecoCobranca;
            Status = status;
            ObservacaoNf = observacaoNf;
            Vendedor = vendedor;
            CondicaoPagamento = condicaoPagamento;
            TipoCobranca = tipoCobranca;
        }

        [JsonProperty(PropertyName = "intervener_id")]
        public int IntervenienteCodigo { get; set; }

        [JsonProperty(PropertyName = "intervener_document")]
        public string IntervenienteCPFCNPJ { get; set; }

        [JsonProperty(PropertyName = "intervener_external_id")]
        public string IntervenienteExternalId { get; set; }

        [JsonProperty(PropertyName = "construction_number")]
        public long CodigoObra { get; set; }

        [JsonProperty(PropertyName = "construction_name")]
        public string NomeObra { get; set; }

        [JsonProperty(PropertyName = "construction_municipal_taxation")]
        public IEnumerable<ObraTributacaoMunicipalDTO> ObraTributacoesMunicipais { get; set; }

        [JsonProperty(PropertyName = "contract_concrete_batching_plant")]
        public int Usina { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int UsinaEntrega { get; set; }

        [JsonProperty(PropertyName = "contract_year")]
        public int Ano { get; set; }

        [JsonProperty(PropertyName = "contract_number")]
        public int Numero { get; set; }

        [JsonProperty(PropertyName = "contract_date")]
        public DateTime? DataContrato { get; set; }

        [JsonProperty(PropertyName = "previous_contract_number")]
        public string NumeroContratoAnterior { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_zip_code")]
        public string EnderecoCep { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_street")]
        public string EnderecoLogradouro { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_number")]
        public int EnderecoNumero { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_complement")]
        public string EnderecoComplemento { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_district")]
        public string EnderecoBairro { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_city_code")]
        public int? EnderecoMunicipioCodigo { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_state")]
        public string EnderecoUf { get; set; }

        [JsonProperty(PropertyName = "address_construction_site_city")]
        public string EnderecoMunicipio { get; set; }

        [JsonProperty(PropertyName = "construction_email")]
        public string EmailObra { get; set; }

        [JsonProperty(PropertyName = "cei")]
        public string Cei { get; set; }

        [JsonProperty(PropertyName = "construction_permit_number")]
        public string CodigoObraPrefeitura { get; set; }

        [JsonProperty(PropertyName = "tax_benefit_code")]
        public string CodigoBeneficioFiscal { get; set; }

        [JsonProperty(PropertyName = "contract_purpose")]
        public string ContratoFinalidade { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "invoice_observation")]
        public string ObservacaoNf { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public VendedorDTO Vendedor { get; set; }

        [JsonProperty(PropertyName = "billing_data")]
        public ContratoDadosFaturamentoDTO DadosFaturamento { get; set; }

        [JsonProperty(PropertyName = "billing_address")]
        public ContratoEnderecoFaturamentoDTO EnderecoFaturamento { get; set; }

        [JsonProperty(PropertyName = "charging_data")]
        public ContratoDadosCobrancaDTO DadosCobranca { get; set; }

        [JsonProperty(PropertyName = "charging_address")]
        public ContratoEnderecoCobrancaDTO EnderecoCobranca { get; set; }

        [JsonProperty(PropertyName = "contract_payment_condition")]
        public CondicaoPagamentoWebHookDTO CondicaoPagamento { get; set; }

        [JsonProperty(PropertyName = "contract_biling_type")]
        public TipoCobrancaWebhookDTO TipoCobranca { get; set; }

        [JsonProperty(PropertyName = "payments")]
        public IEnumerable<ContratoPagamentoDTO> Pagamentos { get; set; }

        public static ContratoPendenteAprovacaoFinanceiraWebHookDTO FromContrato(Domain.Entities.Contrato contrato, Domain.Entities.Obra obra)
        {
            var pagamentosDTO = new List<ContratoPagamentoDTO>();
            foreach (var pagamento in obra.ContratoPagamentos)
            {
                var condicaoPagamentoDTO = new CondicaoPagamentoDTO(pagamento.CondicaoPagamentoCodigo, pagamento.CondicaoPagamento.Descricao, pagamento.IdCadastro);

                var pagamentoDTO = new ContratoPagamentoDTO(pagamento.Sequencia, pagamento.Forma, pagamento.TipoCobrancaCodigo, pagamento.Valor, condicaoPagamentoDTO);

                var detalhesDTO = new List<ContratoPagamentoDetalheDTO>();
                if(pagamento != null && pagamento.Detalhes != null)
                {
                    foreach (var detalhe in pagamento.Detalhes)
                    {
                        var portador = new Portador();
                        if (typeof(ContratoPagamentoDetalheDeposito).Equals(detalhe.GetType()))
                            portador = (((ContratoPagamentoDetalheDeposito)detalhe).Portador ?? pagamento.TipoCobranca.Portador);
                        else
                            portador = pagamento.TipoCobranca.Portador;

                        var portadorDTO = new PortadorDTO(0, "");
                        if (portador != null)
                        {
                            portadorDTO.Codigo = portador.Codigo;
                            portadorDTO.Descricao = portador.Descricao;
                        }

                        detalhesDTO.Add(new ContratoPagamentoDetalheDTO(detalhe.DetalheSequencia, detalhe.Valor, detalhe.DataTitulo(), portadorDTO));
                    }
                    pagamentoDTO.Detalhes = detalhesDTO;

                    pagamentosDTO.Add(pagamentoDTO);
                }
            }

            var obraTributacaoDto = new List<ObraTributacaoMunicipalDTO>();
            foreach(var obraTributacao in obra.ObraTributacoesMunicipais)
            {

                var dto = new ObraTributacaoMunicipalDTO()
                {
                    CodigoObraPrefeitura = obraTributacao.CodigoObraPrefeitura,
                    ObraCCM = obraTributacao.ObraCCM,
                    UsinaEntregaCodigo = obraTributacao.UsinaEntregaCodigo,
                    TributacaoISS = obraTributacao.TributacaoISS == 0 ? "NORMAL" : "ISENTO",
                    RetencaoISS = obraTributacao.RetencaoISS == "S" ? "SIM" : obraTributacao.RetencaoISS == "N" ? "NÃO" : "REGRA MUNICÍPIO"
                };

                obraTributacaoDto.Add(dto);

            }

            ContratoDadosFaturamentoDTO dadosFaturamento = null;
            ContratoEnderecoFaturamentoDTO enderecoFaturamento = null;
            if (obra.Proposta.Faturamento != null)
            {
                if (obra.Proposta.UtilizaDadosFaturamento)
                {
                    dadosFaturamento = new ContratoDadosFaturamentoDTO();

                    dadosFaturamento.IntervenienteTipo = obra.Proposta.Faturamento.IntervenienteTipo;
                    dadosFaturamento.CpfCnpj = obra.Proposta.Faturamento.CpfCnpj;
                    dadosFaturamento.InscricaoEstadual = obra.Proposta.Faturamento.InscricaoEstadual;
                    dadosFaturamento.InscricaoMunicipal = obra.Proposta.Faturamento.InscricaoMunicipal;
                    dadosFaturamento.Rg = obra.Proposta.Faturamento.Rg;
                    dadosFaturamento.Razao = obra.Proposta.Faturamento.Razao;
                    dadosFaturamento.Nome = obra.Proposta.Faturamento.Nome;
                }

                if (obra.Proposta.UtilizaEnderecoFaturamento)
                {
                    enderecoFaturamento = new ContratoEnderecoFaturamentoDTO();

                    enderecoFaturamento.Logradouro = obra.Proposta.Faturamento.EnderecoLogradouro;
                    enderecoFaturamento.Numero = obra.Proposta.Faturamento.EnderecoNumero.ToString();
                    enderecoFaturamento.Complemento = obra.Proposta.Faturamento.EnderecoComplemento;
                    enderecoFaturamento.Bairro = obra.Proposta.Faturamento.EnderecoBairro;
                    enderecoFaturamento.MunicipioCodigo = obra.Proposta.Faturamento.EnderecoMunicipioCodigo;
                    enderecoFaturamento.Municipio = obra.Proposta.Faturamento.EnderecoMunicipio.Nome;
                    enderecoFaturamento.Uf = obra.Proposta.Faturamento.EnderecoMunicipio.Uf;
                    enderecoFaturamento.Cep = obra.Proposta.Faturamento.EnderecoCep;
                }
            }

            ContratoDadosCobrancaDTO dadosCobranca = null;
            ContratoEnderecoCobrancaDTO enderecoCobranca = null;
            if (obra.Proposta.Cobranca != null)
            {
                if (obra.Proposta.UtilizaDadosCobranca)
                {
                    dadosCobranca = new ContratoDadosCobrancaDTO();

                    dadosCobranca.IntervenienteTipo = obra.Proposta.Cobranca.IntervenienteTipo;
                    dadosCobranca.CpfCnpj = obra.Proposta.Cobranca.CpfCnpj;
                    dadosCobranca.InscricaoEstadual = obra.Proposta.Cobranca.InscricaoEstadual;
                    dadosCobranca.InscricaoMunicipal = obra.Proposta.Cobranca.InscricaoMunicipal;
                    dadosCobranca.Rg = obra.Proposta.Cobranca.Rg;
                    dadosCobranca.Razao = obra.Proposta.Cobranca.Razao;
                    dadosCobranca.Nome = obra.Proposta.Cobranca.Nome;
                    dadosCobranca.Email = obra.Proposta.Cobranca.Email;
                }

                if (obra.Proposta.UtilizaEnderecoCobranca)
                {
                    enderecoCobranca = new ContratoEnderecoCobrancaDTO();

                    enderecoCobranca.Logradouro = obra.Proposta.Cobranca.EnderecoLogradouro;
                    enderecoCobranca.Numero = obra.Proposta.Cobranca.EnderecoNumero.ToString();
                    enderecoCobranca.Complemento = obra.Proposta.Cobranca.EnderecoComplemento;
                    enderecoCobranca.Bairro = obra.Proposta.Cobranca.EnderecoBairro;
                    enderecoCobranca.MunicipioCodigo = obra.Proposta.Cobranca.EnderecoMunicipioCodigo;
                    enderecoCobranca.Municipio = obra.Proposta.Cobranca.EnderecoMunicipio.Nome;
                    enderecoCobranca.Uf = obra.Proposta.Cobranca.EnderecoMunicipio.Uf;
                    enderecoCobranca.Cep = obra.Proposta.Cobranca.EnderecoCep;
                }
            }

            var contratoAprovadoWebHookDTO = new ContratoPendenteAprovacaoFinanceiraWebHookDTO(
                contrato.IntervenienteCodigo ?? 0,
                contrato.Interveniente?.IdExterno ?? "",
                contrato.Interveniente?.CpfCnpj ?? "",
                obra.Numero,
                obra.Nome,
                obraTributacaoDto,
                contrato.Usina,
                obra.UsinaEntregaCodigo,
                contrato.Ano,
                contrato.Numero,
                contrato.DataContrato,
                contrato.NumeroContratoAnterior,
                obra.EnderecoCep,
                obra.EnderecoLogradouro,
                obra.EnderecoNumero,
                obra.EnderecoComplemento,
                obra.EnderecoBairro,
                obra.EnderecoMunicipioCodigo,
                obra.EnderecoMunicipio?.Uf ?? "",
                obra.EnderecoMunicipio?.Nome ?? "",
                contrato.ContratoFinalidade.ToString(),
                obra.Email,
                obra.Cei,
                obra.Proposta.CodigoObraPrefeitura,
                obra.CodigoBeneficioFiscal,
                contrato.Status.ToString(),
                obra.ObservacaoNf,
                dadosFaturamento,
                enderecoFaturamento,
                dadosCobranca,
                enderecoCobranca,
                pagamentosDTO,
                new VendedorDTO(contrato.Vendedor.Codigo, contrato.Vendedor.CpfCnpj, contrato.Vendedor.ExternalId),
                new CondicaoPagamentoWebHookDTO(obra.CondicaoPagamento.Codigo, obra.CondicaoPagamento.Descricao, obra.CondicaoPagamento.IdExterno),
                new TipoCobrancaWebhookDTO(obra.TipoCobranca.Codigo, obra.TipoCobranca.Descricao));

            return contratoAprovadoWebHookDTO;
        }

        public static ContratoPendenteAprovacaoFinanceiraWebHookDTO FromContratoVersao(Domain.Entities.ContratoVersao contrato, Domain.Entities.ObraVersao obra)
        {
            var pagamentosDTO = new List<ContratoPagamentoDTO>();
            foreach (var pagamento in obra.ContratoPagamentos)
            {

                var condicaoPagamentoDTO = new CondicaoPagamentoDTO(pagamento.CondicaoPagamentoCodigo, pagamento.CondicaoPagamento.Descricao, pagamento.IdCadastro);

                var pagamentoDTO = new ContratoPagamentoDTO(pagamento.Sequencia, pagamento.Forma, pagamento.TipoCobrancaCodigo, pagamento.Valor, condicaoPagamentoDTO);

                var detalhesDTO = new List<ContratoPagamentoDetalheDTO>();
                if (pagamento != null && pagamento.Detalhes != null)
                {
                    foreach (var detalhe in pagamento.Detalhes)
                    {
                        var portador = new Portador();
                        if (typeof(ContratoPagamentoDetalheDepositoVersao).Equals(detalhe.GetType()))
                            portador = (((ContratoPagamentoDetalheDepositoVersao)detalhe).Portador ?? pagamento.TipoCobranca.Portador);
                        else
                            portador = pagamento.TipoCobranca.Portador;

                        var portadorDTO = new PortadorDTO(0, "");
                        if (portador != null)
                        {
                            portadorDTO.Codigo = portador.Codigo;
                            portadorDTO.Descricao = portador.Descricao;
                        }

                        detalhesDTO.Add(new ContratoPagamentoDetalheDTO(detalhe.DetalheSequencia, detalhe.Valor, detalhe.DataTitulo(), portadorDTO));
                    }
                    pagamentoDTO.Detalhes = detalhesDTO;

                    pagamentosDTO.Add(pagamentoDTO);
                }
            }

            var obraTributacaoDto = new List<ObraTributacaoMunicipalDTO>();
            foreach (var obraTributacao in obra.ObraTributacoesMunicipais)
            {

                var dto = new ObraTributacaoMunicipalDTO()
                {
                    CodigoObraPrefeitura = obraTributacao.CodigoObraPrefeitura,
                    ObraCCM = obraTributacao.ObraCCM,
                    UsinaEntregaCodigo = obraTributacao.UsinaEntregaCodigo,
                    TributacaoISS = obraTributacao.TributacaoISS == 0 ? "NORMAL" : "ISENTO",
                    RetencaoISS = obraTributacao.RetencaoISS == "S" ? "SIM" : obraTributacao.RetencaoISS == "N" ? "NÃO" : "REGRA MUNICÍPIO"
                };

                obraTributacaoDto.Add(dto);

            }

            ContratoDadosFaturamentoDTO dadosFaturamento = null;
            ContratoEnderecoFaturamentoDTO enderecoFaturamento = null;
            if (obra.Proposta.Faturamento != null)
            {
                if (obra.Proposta.UtilizaDadosFaturamento)
                {
                    dadosFaturamento = new ContratoDadosFaturamentoDTO();

                    dadosFaturamento.IntervenienteTipo = obra.Proposta.Faturamento.IntervenienteTipo;
                    dadosFaturamento.CpfCnpj = obra.Proposta.Faturamento.CpfCnpj;
                    dadosFaturamento.InscricaoEstadual = obra.Proposta.Faturamento.InscricaoEstadual;
                    dadosFaturamento.InscricaoMunicipal = obra.Proposta.Faturamento.InscricaoMunicipal;
                    dadosFaturamento.Rg = obra.Proposta.Faturamento.Rg;
                    dadosFaturamento.Razao = obra.Proposta.Faturamento.Razao;
                    dadosFaturamento.Nome = obra.Proposta.Faturamento.Nome;
                }

                if (obra.Proposta.UtilizaEnderecoFaturamento)
                {
                    enderecoFaturamento = new ContratoEnderecoFaturamentoDTO();

                    enderecoFaturamento.Logradouro = obra.Proposta.Faturamento.EnderecoLogradouro;
                    enderecoFaturamento.Numero = obra.Proposta.Faturamento.EnderecoNumero.ToString();
                    enderecoFaturamento.Complemento = obra.Proposta.Faturamento.EnderecoComplemento;
                    enderecoFaturamento.Bairro = obra.Proposta.Faturamento.EnderecoBairro;
                    enderecoFaturamento.MunicipioCodigo = obra.Proposta.Faturamento.EnderecoMunicipioCodigo;
                    enderecoFaturamento.Municipio = obra.Proposta.Faturamento.EnderecoMunicipio.Nome;
                    enderecoFaturamento.Uf = obra.Proposta.Faturamento.EnderecoMunicipio.Uf;
                    enderecoFaturamento.Cep = obra.Proposta.Faturamento.EnderecoCep;
                }
            }

            ContratoDadosCobrancaDTO dadosCobranca = null;
            ContratoEnderecoCobrancaDTO enderecoCobranca = null;
            if (obra.Proposta.Cobranca != null)
            {
                if (obra.Proposta.UtilizaDadosCobranca)
                {
                    dadosCobranca = new ContratoDadosCobrancaDTO();

                    dadosCobranca.IntervenienteTipo = obra.Proposta.Cobranca.IntervenienteTipo;
                    dadosCobranca.CpfCnpj = obra.Proposta.Cobranca.CpfCnpj;
                    dadosCobranca.InscricaoEstadual = obra.Proposta.Cobranca.InscricaoEstadual;
                    dadosCobranca.InscricaoMunicipal = obra.Proposta.Cobranca.InscricaoMunicipal;
                    dadosCobranca.Rg = obra.Proposta.Cobranca.Rg;
                    dadosCobranca.Razao = obra.Proposta.Cobranca.Razao;
                    dadosCobranca.Nome = obra.Proposta.Cobranca.Nome;
                    dadosCobranca.Email = obra.Proposta.Cobranca.Email;
                }

                if (obra.Proposta.UtilizaEnderecoCobranca)
                {
                    enderecoCobranca = new ContratoEnderecoCobrancaDTO();

                    enderecoCobranca.Logradouro = obra.Proposta.Cobranca.EnderecoLogradouro;
                    enderecoCobranca.Numero = obra.Proposta.Cobranca.EnderecoNumero.ToString();
                    enderecoCobranca.Complemento = obra.Proposta.Cobranca.EnderecoComplemento;
                    enderecoCobranca.Bairro = obra.Proposta.Cobranca.EnderecoBairro;
                    enderecoCobranca.MunicipioCodigo = obra.Proposta.Cobranca.EnderecoMunicipioCodigo;
                    enderecoCobranca.Municipio = obra.Proposta.Cobranca.EnderecoMunicipio.Nome;
                    enderecoCobranca.Uf = obra.Proposta.Cobranca.EnderecoMunicipio.Uf;
                    enderecoCobranca.Cep = obra.Proposta.Cobranca.EnderecoCep;
                }
            }

            var contratoAprovadoWebHookDTO = new ContratoPendenteAprovacaoFinanceiraWebHookDTO(
                contrato.IntervenienteCodigo ?? 0,
                contrato.Interveniente?.IdExterno ?? "",
                contrato.Interveniente?.CpfCnpj ?? "",
                obra.Numero,
                obra.Nome,
                obraTributacaoDto,
                contrato.Usina,
                obra.UsinaEntregaCodigo,
                contrato.Ano,
                contrato.Numero,
                contrato.DataContrato,
                contrato.NumeroContratoAnterior,
                obra.EnderecoCep,
                obra.EnderecoLogradouro,
                obra.EnderecoNumero,
                obra.EnderecoComplemento,
                obra.EnderecoBairro,
                obra.EnderecoMunicipioCodigo,
                obra.EnderecoMunicipio?.Uf ?? "",
                obra.EnderecoMunicipio?.Nome ?? "",
                contrato.ContratoFinalidade.ToString(),
                obra.Email,
                obra.Cei,
                obra.Proposta.CodigoObraPrefeitura,
                obra.CodigoBeneficioFiscal,
                contrato.Status.ToString(),
                obra.ObservacaoNf,
                dadosFaturamento,
                enderecoFaturamento,
                dadosCobranca,
                enderecoCobranca,
                pagamentosDTO,
                new VendedorDTO(contrato.Vendedor.Codigo, contrato.Vendedor.CpfCnpj, contrato.Vendedor.ExternalId),
                new CondicaoPagamentoWebHookDTO(obra.CondicaoPagamento.Codigo, obra.CondicaoPagamento.Descricao, obra.CondicaoPagamento.IdExterno),
                new TipoCobrancaWebhookDTO(obra.TipoCobranca.Codigo, obra.TipoCobranca.Descricao));

            return contratoAprovadoWebHookDTO;
        }
    }
}
