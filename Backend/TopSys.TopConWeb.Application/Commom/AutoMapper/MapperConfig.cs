using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TopSys.TopConWeb.Application.DTOS.Generic.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraBomba;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendentesResponse;
using TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Request.Equipamento;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Application.DTOS.Request.Vendedor;
using TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento;
using System.Linq;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Request.Funcionario;
using TopSys.TopConWeb.Application.DTOS.Response.Funcionario;
using TopSys.TopConWeb.Application.DTOS.Response.Vendedor;
using TopSys.TopConWeb.Application.DTOS.Response.Equipamento;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoIntegracao;
using TopSys.TopConWeb.Application.DTOS.Response.Remessa;
using TopSys.TopConWeb.Application.DTOS.Response.Fatura;
using TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoTracoReajuste.ContratoTracoReajusteVigenciasResponse;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Entities.ReguaDeCobranca;
using TopSys.TopConWeb.Application.DTOS.Request.Visita;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Application.DTOS.Response.Visita;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Lead;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste;

namespace TopSys.TopConWeb.Application.Commom.AutoMapper
{
    public static class MapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
            {
            cfg.CreateMap<bool?, bool>().ConvertUsing(new NullableConverter<bool, bool>());
            cfg.CreateMap<Guid?, Guid>().ConvertUsing(new NullableConverter<Guid, Guid>());
            cfg.CreateMap<decimal?, decimal>().ConvertUsing(new NullableConverter<decimal, decimal>());
            cfg.CreateMap<DateTime?, DateTime>().ConvertUsing(new NullableConverter<DateTime, DateTime>());
            cfg.CreateMap<int?, int>().ConvertUsing(new NullableConverter<int, int>());
            cfg.CreateMap<long?, long>().ConvertUsing(new NullableConverter<long, long>());
            cfg.CreateMap<double?, double>().ConvertUsing(new NullableConverter<double, double>());
            cfg.CreateMap<float?, float>().ConvertUsing(new NullableConverter<float, float>());
            cfg.CreateMap<double?, float>().ConvertUsing(new NullableConverter<double, float>());
            cfg.CreateMap<int?, EBaseCalculoIss>().ConvertUsing(new NullableConverter<int, EBaseCalculoIss>());

                cfg.CreateMap<DTOS.Request.Proposta.PropostaPropagandaAdicionarRequest, PropostaPropaganda>()
                    .IgnoreMember(x => x.Id)
                    .IgnoreMember(x => x.Usuario)
                    .IgnoreMember(x => x.Data)
                    .IgnoreMember(x => x.DataHora);

                cfg.CreateMap<DTOS.Request.Proposta.PropostaPropagandaAtualizarRequest, PropostaPropaganda>()
                    .IgnoreMember(x => x.Nome)
                    .IgnoreMember(x => x.Usuario)
                    .IgnoreMember(x => x.Data)
                    .IgnoreMember(x => x.DataHora);

            // Generic Map
            cfg.CreateMap<DTOS.Generic.EnderecoDTO, Endereco>().ReverseMap();

            // ClicksignConfiguracao
            cfg.CreateMap<DTOS.Request.ClicksignConfiguracao.ClicksignConfiguracaoRequest, ClicksignConfiguracao>().ReverseMap();
            cfg.CreateMap<ClicksignConfiguracao, DTOS.Response.ClicksignConfiguracao.ClicksignConfiguracaoResponse>();
            cfg.CreateMap<DTOS.Generic.MunicipioDTO, Municipio>().ReverseMap();
            cfg.CreateMap<DTOS.Generic.PropostaCobrancaDTO, PropostaCobranca>()
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Proposta)
                .ReverseMap()
                .MapEnderecoToDto();
            cfg.CreateMap<DTOS.Generic.PropostaCobrancaDTO, PropostaCobrancaVersao>()
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Proposta)
                .IgnoreMember(t => t.NumeroVersao)
                .ReverseMap()
                .MapEnderecoToDto();
            cfg.CreateMap<DTOS.Generic.PropostaFaturamentoDTO, PropostaFaturamento>()
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Proposta)
                .ReverseMap()
                .MapEnderecoToDto();
            cfg.CreateMap<DTOS.Generic.PropostaFaturamentoDTO, PropostaFaturamentoVersao>()
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Proposta)
                .IgnoreMember(t => t.NumeroVersao)
                .ReverseMap()
                .MapEnderecoToDto();
            cfg.CreateMap<DTOS.Generic.PropostaResponsavelSolidarioDTO, PropostaResponsavelSolidario>()
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Proposta)
                .ReverseMap()
                .MapEnderecoToDto();

                cfg.CreateMap<ContratoPagamento, DTOS.Request.WebHook.ContratoPagamentoWebHookDTO>();
                cfg.CreateMap<ContratoPagamentoVersao, DTOS.Request.WebHook.ContratoPagamentoWebHookDTO>();
                cfg.CreateMap<CondicaoPagamento, DTOS.Request.WebHook.CondicaoPagamentoWebHookDTO>();

                cfg.CreateMap<Interveniente, DTOS.Request.WebHook.IntervenienteAddressWebHookDTO>()
                .ForMember(dest => dest.EnderecoLogradouro, opt => opt.MapFrom(src => src.EnderecoLogradouro))
                .ForMember(dest => dest.EnderecoNumero, opt => opt.MapFrom(src => src.EnderecoNumero.ToString())) // Converte int para string
                .ForMember(dest => dest.EnderecoComplemento, opt => opt.MapFrom(src => src.EnderecoComplemento))
                .ForMember(dest => dest.EnderecoBairro, opt => opt.MapFrom(src => src.EnderecoBairro))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.EnderecoMunicipio.Nome)) // Mapeia o nome da cidade
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.EnderecoMunicipio.Uf))   // Mapeia o UF do estado
                .ForMember(dest => dest.EnderecoCep, opt => opt.MapFrom(src => src.EnderecoCep))
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => "BR")); // Valor fixo

                // Mapeamento para o DTO principal
            cfg.CreateMap<Interveniente, DTOS.Request.WebHook.IntervenienteWebHookDTO>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo.ToString())) // Mapeia Codigo para string
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Razao, opt => opt.MapFrom(src => src.Razao))
                .ForMember(dest => dest.IdExterno, opt => opt.MapFrom(src => src.IdExterno))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => $"{src.TelefoneDdd}{src.TelefoneNumero}")) // Concatena e formata
                .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.CpfCnpj))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src)); // Mapeia o Interveniente para o IntervenienteAddressDTO

	          //  DTOs Request
	          // Obra
                cfg.CreateMap<CadastroGeralIntegracaoAdicionarRequest, CadastroGeral>()
                .IgnoreMember(x => x.IdCadastro)
                .IgnoreMember(x => x.IdAtualizacao);

            cfg.CreateMap<CadastroGeralIntegracaoAtualizarRequest, CadastroGeral>()
                .IgnoreMember(x => x.IdCadastro)
                .IgnoreMember(x => x.IdAtualizacao)
                .IgnoreMember(x => x.Codigo)
                .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

            cfg.CreateMap<VendedorIntegracaoAdicionarRequest, Vendedor>()
                .ForMember(e => e.Ativo, opt => opt.MapFrom(src => src.Ativo ? "S" : "N"))
                .IgnoreMember(x => x.IdCadastro)
                .IgnoreMember(x => x.IdAtualizacao);

            cfg.CreateMap<VendedorIntegracaoAtualizarRequest, Vendedor>()
                .ForMember(e => e.Ativo, opt => opt.MapFrom(src => src.Ativo == true ? "S" : "N"))
                .IgnoreMember(x => x.Codigo)
                .IgnoreMember(x => x.IdCadastro)
                .IgnoreMember(x => x.IdAtualizacao)
                .IgnoreMember(x => x.ExternalId)
                .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

            cfg.CreateMap<Vendedor, VendedorIntegracaoResponse>()
                .ForMember(e => e.Ativo, opt => opt.MapFrom(src => src.Ativo == "S"));

            cfg.CreateMap<EquipamentoAdicionarRequest, Equipamento>()
                .ForMember(e => e.Bomba, opt => opt.MapFrom(src => src.Bomba ? "S" : ""))
                .ForMember(e => e.HorimetroQuebrado, opt => opt.MapFrom(src => src.HorimetroQuebrado ? "S" : ""))
                .ForMember(e => e.TacografoQuebrado, opt => opt.MapFrom(src => src.TacografoQuebrado ? "S" : ""))
                .ForMember(e => e.TacografoQuebrado, opt => opt.MapFrom(e => e.TacografoQuebrado ? "S" : ""))
                .IgnoreMember(e => e.Betoneira)
                .IgnoreMember(e => e.MotoristaBombaUsina)
                .IgnoreMember(e => e.AjudanteUsina)
                .IgnoreMember(e => e.AjudanteCodigo)
                .IgnoreMember(e => e.AjudanteCodigo2)
                .IgnoreMember(e => e.AjudanteCodigo3)
                .IgnoreMember(e => e.IdCadastro)
                .IgnoreMember(e => e.IdAtual)
                .IgnoreMember(e => e.TipoProcesso)
                .IgnoreMember(e => e.Ativo)
                .IgnoreMember(e => e.Grupo)
                .IgnoreMember(e => e.SubGrupo)
                .IgnoreMember(e => e.Item)
                .IgnoreMember(e => e.Grupo2)
                .IgnoreMember(e => e.SubGrupo2)
                .IgnoreMember(e => e.Item2)
                .IgnoreMember(e => e.Combustivel)
                .IgnoreMember(e => e.Proprietario)
                .IgnoreMember(e => e.FinalPlaca)
                .IgnoreMember(e => e.CC)
                .IgnoreMember(e => e.SeguroResponsabilidade)
                .IgnoreMember(e => e.SeguroCnpjCpfResponsavel)
                .IgnoreMember(e => e.SeguroNomeSeguradora)
                .IgnoreMember(e => e.SeguroCnpjSeguradora)
                .IgnoreMember(e => e.SeguroNumeroApolice)
                .IgnoreMember(e => e.SeguroNumeroAverbacao)
                .IgnoreMember(e => e.TipoRodado)
                .IgnoreMember(e => e.TipoCarroceria)
                .IgnoreMember(e => e.TipoProprietario)
                .IgnoreMember(e => e.FuncionarioAlocado);

            cfg.CreateMap<EquipamentoAtualizarRequest, Equipamento>()
                .ForMember(e => e.Bomba, opt => opt.MapFrom(src => src.Bomba == true ? "S" : "N"))
                .ForMember(e => e.HorimetroQuebrado, opt => opt.MapFrom(src => src.HorimetroQuebrado == true ? "S" : ""))
                .ForMember(e => e.TacografoQuebrado, opt => opt.MapFrom(src => src.TacografoQuebrado == true ? "S" : ""))
                .ForMember(e => e.TacografoQuebrado, opt => opt.MapFrom(src => src.TacografoQuebrado == true ? "S" : ""))
                .IgnoreMember(e => e.Codigo)
                .IgnoreMember(e => e.ExternalID)
                .IgnoreMember(e => e.Betoneira)
                .IgnoreMember(e => e.MotoristaBombaUsina)
                .IgnoreMember(e => e.AjudanteUsina)
                .IgnoreMember(e => e.AjudanteCodigo)
                .IgnoreMember(e => e.AjudanteCodigo2)
                .IgnoreMember(e => e.AjudanteCodigo3)
                .IgnoreMember(e => e.IdCadastro)
                .IgnoreMember(e => e.IdAtual)
                .IgnoreMember(e => e.TipoProcesso)
                .IgnoreMember(e => e.Ativo)
                .IgnoreMember(e => e.Grupo2)
                .IgnoreMember(e => e.SubGrupo2)
                .IgnoreMember(e => e.Item2)
                .IgnoreMember(e => e.Combustivel)
                .IgnoreMember(e => e.FinalPlaca)
                .IgnoreMember(e => e.SeguroResponsabilidade)
                .IgnoreMember(e => e.SeguroCnpjCpfResponsavel)
                .IgnoreMember(e => e.SeguroNomeSeguradora)
                .IgnoreMember(e => e.SeguroCnpjSeguradora)
                .IgnoreMember(e => e.SeguroNumeroApolice)
                .IgnoreMember(e => e.SeguroNumeroAverbacao)
                .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

            cfg.CreateMap<Equipamento, EquipamentoResponse>()
                .ForMember(e => e.Bomba, opt => opt.MapFrom(src => src.Bomba == "S"))
                .ForMember(e => e.HorimetroQuebrado, opt => opt.MapFrom(src => src.HorimetroQuebrado == "S"))
                .ForMember(e => e.TacografoQuebrado, opt => opt.MapFrom(src => src.TacografoQuebrado == "S"))
                .ForMember(e => e.TacografoQuebrado, opt => opt.MapFrom(e => e.TacografoQuebrado == "S"));

            cfg.CreateMap<Obra, ObraPendenteResponse>()
                .ReverseMap();
            cfg.CreateMap<Usuario, RegistrarUsuarioResponse>()
                .ReverseMap();
            cfg.CreateMap<Obra, ObraPendenteAprovacaoRequest>()
                .ReverseMap();
            cfg.CreateMap<ObraVersao, ObraPendenteAprovacaoRequest>()
                .ReverseMap();
            cfg.CreateMap<Obra, ObraPendenteAprovacaoResponse>()
                .ReverseMap();
            cfg.CreateMap<ObraVersao, ObraPendenteAprovacaoResponse>()
                .ReverseMap();//***
            cfg.CreateMap<CondicaoPagamento, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.CondicaoPagamentoDTO>()
                .ReverseMap();
            cfg.CreateMap<CadastroGeral, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.CadastroGeralDTO>()
                .ReverseMap();
            cfg.CreateMap<TipoCobranca, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.CadastroGeralDTO>()
                .ReverseMap();
            cfg.CreateMap<Usina, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.UsinaDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraTraco, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraTracoDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraTracoVersao, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraTracoDTO>()
                .ReverseMap();
            cfg.CreateMap<Pedra, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.PedraDTO>()
                .ReverseMap();
            cfg.CreateMap<Slump, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.SlumpDTO>()
                .ReverseMap();
            cfg.CreateMap<Uso, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.UsoDTO>()
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraFrenteDTO.ObraFrenteDTO, Domain.Entities.ObraFrentes.ObraFrente>()
                .IgnoreMember(x => x.ID);
            cfg.CreateMap<ObraBomba, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraBombaDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraBombaVersao, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraBombaDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraTaxa, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraTaxaDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraTaxaVersao, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraTaxaDTO>()
                .ReverseMap();
            cfg.CreateMap<AprovacaoTipo, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.AprovacaoTipoDTO>()
                .ReverseMap();
            cfg.CreateMap<DemaisAprovacao, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.DemaisAprovacaoDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraLog, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraLogDTO>()
                .ReverseMap();
            cfg.CreateMap<ObraLogVersao, DTOS.Request.Obra.ObraPendenteAprovacaoRequest.ObraLogDTO>()
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest.ObraAlterarStatusCadastroEAnalistaRequest, Obra>()
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest.ObraAlterarStatusCadastroEAnalistaRequest, ObraVersao>()
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest.FuncionarioDTO, Funcionario>()
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest.ContratoDTO, Contrato>()
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest.ContratoDTO, ContratoVersao>()
                 .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraTraco.CalcularEbitdaObraTraco, ObraTraco>()
                .MapMemberFrom(dest => dest.Fck, src => src.Mpa);
            cfg.CreateMap<DTOS.Request.Obra.ObraTraco.CalcularEbitdaObraTraco, Obra>()
                .MapMemberFrom(dest => dest.VolumePorCarga, src => src.ObraVolumePorCarga)
                .MapMemberFrom(dest => dest.EnderecoMunicipioCodigo, src => src.MunicipioCodigo)
                .MapMemberFrom(dest => dest.TempoAteAObra, src => src.obraTempoAteAObra)
                .MapMemberFrom(dest => dest.TempoBtNaObra, src => src.ObraTempoBtNaObra)
                .ReverseMap();
            cfg.CreateMap<DTOS.Request.Obra.ObraTraco.CalcularEbitdaObraTraco, ObraTracoVersao>()//*****
                .MapMemberFrom(dest => dest.Fck, src => src.Mpa);
            cfg.CreateMap<DTOS.Request.Obra.ObraTraco.CalcularEbitdaObraTraco, ObraVersao>()
                .MapMemberFrom(dest => dest.VolumePorCarga, src => src.ObraVolumePorCarga)
                .MapMemberFrom(dest => dest.EnderecoMunicipioCodigo, src => src.MunicipioCodigo)
                .MapMemberFrom(dest => dest.TempoAteAObra, src => src.obraTempoAteAObra)
                .MapMemberFrom(dest => dest.TempoBtNaObra, src => src.ObraTempoBtNaObra)
                .ReverseMap();

            // Contrato
            cfg.CreateMap<Contrato, DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest.ContratoRevalidacaoCadastroRequest>()
                .ReverseMap();
            cfg.CreateMap<Interveniente, DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest.IntervenienteDTO>()
                .ReverseMap();
            cfg.CreateMap<CadastroGeral, DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest.CadastroGeralDTO>()
                .ReverseMap();

            // Programacao Inclusao
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.PedraDTO, Pedra>();
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.SlumpDTO, Slump>();
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.UsinaDTO, Usina>();
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.UsoDTO, Uso>();
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.VendedorDTO, Vendedor>();
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.ProgramacaoDemaisServicosDTO, ProgramacaoDemaisServicos>();
            cfg.CreateMap<DTOS.Request.Programacao.Inclusao.ProgramacaoInclusaoRequest, Programacao>()
                .IgnoreMember(t => t.TemNotaFicalEmitida)
                .IgnoreMember(t => t.Contrato)
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Pedra).MapMemberFrom(t => t.PedraCodigo, t => (t.Pedra != null ? t.Pedra.Codigo : 0))
                .IgnoreMember(t => t.Proposta)
                .IgnoreMember(t => t.ResistenciaTipo).MapMemberFrom(t => t.ResistenciaTipoCodigo, t => (t.ResistenciaTipo != null ? t.ResistenciaTipo.Codigo : 0))
                .IgnoreMember(t => t.Slump).MapMemberFrom(t => t.SlumpCodigo, t => (t.Slump != null ? t.Slump.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoPedra).MapMemberFrom(t => t.TracoPesadoPedraCodigo, t => (t.TracoPesadoPedra != null ? t.TracoPesadoPedra.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoResistenciaTipo).MapMemberFrom(t => t.TracoPesadoResistenciaTipoCodigo, t => (t.TracoPesadoResistenciaTipo != null ? t.TracoPesadoResistenciaTipo.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoSlump).MapMemberFrom(t => t.TracoPesadoSlumpCodigo, t => (t.TracoPesadoSlump != null ? t.TracoPesadoSlump.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoUso).MapMemberFrom(t => t.TracoPesadoUsoCodigo, t => (t.TracoPesadoUso != null ? t.TracoPesadoUso.Codigo : 0))
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.UsinaEntrega)
                .IgnoreMember(t => t.Uso).MapMemberFrom(t => t.UsoCodigo, t => (t.Uso != null ? t.Uso.Codigo : 0))
                .IgnoreMember(t => t.VibradorVendedor).MapMemberFrom(t => t.VibradorVendedorCodigo, t => (t.VibradorVendedor != null ? t.VibradorVendedor.Codigo : 0))
                .IgnoreMember(t => t.VolumeEntregue);

            // Programacao Alteracao
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.PedraDTO, Pedra>();
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.SlumpDTO, Slump>();
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.UsinaDTO, Usina>();
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.UsoDTO, Uso>();
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.VendedorDTO, Vendedor>();
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.ProgramacaoDemaisServicosDTO, ProgramacaoDemaisServicos>();
            cfg.CreateMap<DTOS.Request.Programacao.Alteracao.ProgramacaoAlteracaoRequest, Programacao>()
                .IgnoreMember(t => t.DemaisServicos)
                .IgnoreMember(t => t.TemNotaFicalEmitida)
                .IgnoreMember(t => t.Contrato)
                .IgnoreMember(t => t.ContratoAno)
                .IgnoreMember(t => t.ContratoNumero)
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Pedra).MapMemberFrom(t => t.PedraCodigo, t => (t.Pedra != null ? t.Pedra.Codigo : 0))
                .IgnoreMember(t => t.Proposta)
                .IgnoreMember(t => t.ResistenciaTipo).MapMemberFrom(t => t.ResistenciaTipoCodigo, t => (t.ResistenciaTipo != null ? t.ResistenciaTipo.Codigo : 0))
                .IgnoreMember(t => t.Slump).MapMemberFrom(t => t.SlumpCodigo, t => (t.Slump != null ? t.Slump.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoPedra).MapMemberFrom(t => t.TracoPesadoPedraCodigo, t => (t.TracoPesadoPedra != null ? t.TracoPesadoPedra.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoResistenciaTipo).MapMemberFrom(t => t.TracoPesadoResistenciaTipoCodigo, t => (t.TracoPesadoResistenciaTipo != null ? t.TracoPesadoResistenciaTipo.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoSlump).MapMemberFrom(t => t.TracoPesadoSlumpCodigo, t => (t.TracoPesadoSlump != null ? t.TracoPesadoSlump.Codigo : 0))
                .IgnoreMember(t => t.TracoPesadoUso).MapMemberFrom(t => t.TracoPesadoUsoCodigo, t => (t.TracoPesadoUso != null ? t.TracoPesadoUso.Codigo : 0))
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.UsinaEntrega)
                .IgnoreMember(t => t.Uso).MapMemberFrom(t => t.UsoCodigo, t => (t.Uso != null ? t.Uso.Codigo : 0))
                .IgnoreMember(t => t.VibradorVendedor).MapMemberFrom(t => t.VibradorVendedorCodigo, t => (t.VibradorVendedor != null ? t.VibradorVendedor.Codigo : 0))
                .IgnoreMember(t => t.VolumeEntregue);

            // Proposta Inclusao
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraIndicadorDTO, ObraIndicadorVersao>()
                .IgnoreMember(t => t.Interveniente)
                .IgnoreMember(t => t.Vendedor);
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraIndicadorDTO, ObraIndicador>()
                .IgnoreMember(t => t.Interveniente)
                .IgnoreMember(t => t.Vendedor);

            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.CadastroGeralDTO, CadastroGeral>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.CartaoBandeiraDTO, CartaoBandeira>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.TipoCobrancaDTO, TipoCobranca>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.CondicaoPagamentoDTO, CondicaoPagamento>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ContaDTO, Conta>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.IntervenienteDTO, Interveniente>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraDTO, Obra>()
                .MapMemberFrom(
                    dest => dest.PropostaPagamentos,
                    src => src.ObraPagamentos
                );
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraBombaDTO, ObraBomba>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraTracoDTO, ObraTraco>()
                .MapMemberFrom(dest => dest.Fck, src => src.Mpa);
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraTaxaDTO, ObraTaxa>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraLogDTO, ObraLog>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraTributacaoMunicipalDTO, ObraTributacaoMunicipal>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraDemaisServicosDTO, ObraDemaisServicos>()
                .IgnoreMember(t => t.UsinaEntrega).MapMemberFrom(t => t.UsinaEntregaCodigo, t => (t.UsinaEntrega != null ? t.UsinaEntrega.Codigo : 0))
                .IgnoreMember(t => t.Mercadoria).MapMemberFrom(t => t.MercadoriaCodigo, t => (t.Mercadoria != null ? t.Mercadoria.Codigo : ""))
                .IgnoreMember(t => t.Unidade).MapMemberFrom(t => t.UnidadeSigla, t => (t.Unidade != null ? t.Unidade.Sigla : ""));
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraReajusteDTO, ObraReajuste>()
                .IgnoreMember(t => t.Obra);
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDTO, PropostaPagamento>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalhe>()
                .ConvertUsing(dto => {
                    if (dto.NumeroCartao > 0) return Mapper.Map<PropostaPagamentoDetalheCartao>(dto);
                    if (dto.Portador != null) return Mapper.Map<PropostaPagamentoDetalheDeposito>(dto);
                    if (dto.NumeroCheque > 0) return Mapper.Map<PropostaPagamentoDetalheCheque>(dto);
                    if (dto.DataPagamento != null) return Mapper.Map<PropostaPagamentoDetalheDinheiro>(dto);
                    return Mapper.Map<PropostaPagamentoDetalheBoleto>(dto);
                });
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheCartao>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheDeposito>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheBoleto>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheCheque>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheDinheiro>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.PedraDTO, Pedra>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.PortadorDTO, Portador>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.SlumpDTO, Slump>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.SlumpDTO, SlumpReal>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.UsinaDTO, Usina>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.UsoDTO, Uso>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.VendedorDTO, Vendedor>();
            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.PropostaInclusaoRequest, Proposta>()
                .IgnoreMember(t => t.Cobranca)
                .IgnoreMember(t => t.Faturamento)
                .IgnoreMember(t => t.ResponsavelSolidario);
            cfg.CreateMap<CalcularEbitdaObraBomba, Obra>();
            cfg.CreateMap<CalcularEbitdaObraBomba, ObraBomba>();
            /*cfg.CreateMap<DTOS.Request.Obra.ObraBomba.CalcularEbitdaObraBomba, ObraTracoVersao>()//*****
                .MapMemberFrom(dest => dest.M3QuantidadeBombeada, src => src.ObraTracos.Sum(t => t.M3QuantidadeBombeada));
            cfg.CreateMap<DTOS.Request.Obra.ObraBomba.CalcularEbitdaObraBomba, ObraVersao>()
                .MapMemberFrom(dest => dest.VolumePorCarga, src => src.ObraVolumePorCarga)
                .MapMemberFrom(dest => dest.EnderecoMunicipioCodigo, src => src.MunicipioCodigo)
                .MapMemberFrom(dest => dest.TempoAteAObra, src => src.obraTempoAteAObra)
                .MapMemberFrom(dest => dest.TempoBtNaObra, src => src.ObraTempoBtNaObra)
                .ReverseMap();*/

            // Proposta Alteração
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraIndicadorDTO, ObraIndicador>()
                .IgnoreMember(t => t.Interveniente)
                .IgnoreMember(t => t.Vendedor);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraIndicadorDTO, ObraIndicadorVersao>()
                .IgnoreMember(t => t.Interveniente)
                .IgnoreMember(t => t.Vendedor);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.CadastroGeralDTO, CadastroGeral>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.CartaoBandeiraDTO, CartaoBandeira>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.TipoCobrancaDTO, TipoCobranca>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.CondicaoPagamentoDTO, CondicaoPagamento>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ContaDTO, Conta>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.IntervenienteDTO, Interveniente>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.PedraDTO, Pedra>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.PortadorDTO, Portador>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.SlumpDTO, Slump>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.SlumpDTO, SlumpReal>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.UsinaDTO, Usina>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.UsoDTO, Uso>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.VendedorDTO, Vendedor>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraBombaDTO, ObraBomba>()
                .IgnoreMember(t => t.BombaTipo).ForMember(t => t.BombaTipoCodigo, opt => opt.MapFrom(t => t.BombaTipo != null ? (int?)t.BombaTipo.Codigo : 0))
                .IgnoreMember(t => t.Obra)
                .IgnoreMember(t => t.Terceiro).ForMember(t => t.TerceiroCodigo, opt => opt.MapFrom(t => t.Terceiro != null ? (int?)t.Terceiro.Codigo : 0));
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraBombaDTO, ObraBombaVersao>()
                .IgnoreMember(t => t.BombaTipo).ForMember(t => t.BombaTipoCodigo, opt => opt.MapFrom(t => t.BombaTipo != null ? (int?)t.BombaTipo.Codigo : 0))
                .IgnoreMember(t => t.Obra)
                .IgnoreMember(t => t.Terceiro).ForMember(t => t.TerceiroCodigo, opt => opt.MapFrom(t => t.Terceiro != null ? (int?)t.Terceiro.Codigo : 0));
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraTracoDTO, ObraTraco>()
                .MapMemberFrom(dest => dest.Fck, src => src.Mpa)
                .IgnoreMember(t => t.Obra)
                .IgnoreMember(t => t.Pedra)
                .IgnoreMember(t => t.ResistenciaTipo)
                .IgnoreMember(t => t.Slump)
                .IgnoreMember(t => t.SlumpNominal)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Uso);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraTracoDTO, ObraTracoVersao>()
                .MapMemberFrom(dest => dest.Fck, src => src.Mpa)
                .IgnoreMember(t => t.Obra)
                .IgnoreMember(t => t.Pedra)
                .IgnoreMember(t => t.ResistenciaTipo)
                .IgnoreMember(t => t.Slump)
                .IgnoreMember(t => t.SlumpNominal)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Uso);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraTaxaDTO, ObraTaxa>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraTaxaDTO, ObraTaxaVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraLogDTO, ObraLog>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraLogDTO, ObraLogVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraReajusteDTO, ObraReajuste>()
                .IgnoreMember(t => t.Obra);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraReajusteDTO, ObraReajusteVersao>()
                .IgnoreMember(t => t.Obra);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraTributacaoMunicipalDTO, ObraTributacaoMunicipal>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraTributacaoMunicipalDTO, ObraTributacaoMunicipalVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraDemaisServicosDTO, ObraDemaisServicos>()
                .IgnoreMember(t => t.UsinaEntrega).MapMemberFrom(t => t.UsinaEntregaCodigo, t => (t.UsinaEntrega != null ? t.UsinaEntrega.Codigo : 0))
                .IgnoreMember(t => t.Mercadoria).MapMemberFrom(t => t.MercadoriaCodigo, t => (t.Mercadoria != null ? t.Mercadoria.Codigo : ""))
                .IgnoreMember(t => t.Unidade).MapMemberFrom(t => t.UnidadeSigla, t => (t.Unidade != null ? t.Unidade.Sigla : ""));
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraDemaisServicosDTO, ObraDemaisServicosVersao>()
                .IgnoreMember(t => t.UsinaEntrega).MapMemberFrom(t => t.UsinaEntregaCodigo, t => (t.UsinaEntrega != null ? t.UsinaEntrega.Codigo : 0))
                .IgnoreMember(t => t.Mercadoria).MapMemberFrom(t => t.MercadoriaCodigo, t => (t.Mercadoria != null ? t.Mercadoria.Codigo : ""))
                .IgnoreMember(t => t.Unidade).MapMemberFrom(t => t.UnidadeSigla, t => (t.Unidade != null ? t.Unidade.Sigla : ""));
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDTO, PropostaPagamento>()
                .IgnoreMember(t => t.CondicaoPagamento)
                .IgnoreMember(t => t.TipoCobranca)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Detalhes);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDTO, PropostaPagamentoVersao>()
                .IgnoreMember(t => t.CondicaoPagamento)
                .IgnoreMember(t => t.TipoCobranca)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Detalhes);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDTO, ContratoPagamento>()
                .IgnoreMember(t => t.CondicaoPagamento)
                .IgnoreMember(t => t.TipoCobranca)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Detalhes);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDTO, ContratoPagamentoVersao>()
                .IgnoreMember(t => t.CondicaoPagamento)
                .IgnoreMember(t => t.TipoCobranca)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Detalhes);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDTO, ContratoPagamentoForSaving>()
                .IgnoreMember(t => t.CondicaoPagamento)
                .IgnoreMember(t => t.TipoCobranca)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Detalhes);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDTO, ContratoPagamentoForSavingVersao>()
                .IgnoreMember(t => t.CondicaoPagamento)
                .IgnoreMember(t => t.TipoCobranca)
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Detalhes);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalhe>()
                .ConvertUsing(dto => {
                    if (dto.NumeroCartao > 0) return Mapper.Map<PropostaPagamentoDetalheCartao>(dto);
                    if (dto.Portador != null) return Mapper.Map<PropostaPagamentoDetalheDeposito>(dto);
                    if (dto.NumeroCheque > 0) return Mapper.Map<PropostaPagamentoDetalheCheque>(dto);
                    if (dto.DataPagamento != null) return Mapper.Map<PropostaPagamentoDetalheDinheiro>(dto);
                    return Mapper.Map<PropostaPagamentoDetalheBoleto>(dto);
                });
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheVersao>()
                .ConvertUsing(dto => {
                    if (dto.NumeroCartao > 0) return Mapper.Map<PropostaPagamentoDetalheCartaoVersao>(dto);
                    if (dto.Portador != null) return Mapper.Map<PropostaPagamentoDetalheDepositoVersao>(dto);
                    if (dto.NumeroCheque > 0) return Mapper.Map<PropostaPagamentoDetalheChequeVersao>(dto);
                    if (dto.DataPagamento != null) return Mapper.Map<PropostaPagamentoDetalheDinheiroVersao>(dto);
                    return Mapper.Map<PropostaPagamentoDetalheBoletoVersao>(dto);
                });
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheCartao>()
                .IgnoreMember(t => t.Bandeira);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheCartaoVersao>()
                .IgnoreMember(t => t.Bandeira);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheDeposito>()
                .IgnoreMember(t => t.Portador);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheDepositoVersao>()
                .IgnoreMember(t => t.Portador);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheBoleto>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheBoletoVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheCheque>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheChequeVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheDinheiro>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, PropostaPagamentoDetalheDinheiroVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalhe>()
                .ConvertUsing(dto => {
                    if (dto.NumeroCartao > 0) return Mapper.Map<ContratoPagamentoDetalheCartao>(dto);
                    if (dto.Portador != null) return Mapper.Map<ContratoPagamentoDetalheDeposito>(dto);
                    if (dto.NumeroCheque > 0) return Mapper.Map<ContratoPagamentoDetalheCheque>(dto);
                    if (dto.DataPagamento != null) return Mapper.Map<ContratoPagamentoDetalheDinheiro>(dto);
                    return Mapper.Map<ContratoPagamentoDetalheBoleto>(dto);
                });
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheVersao>()
                .ConvertUsing(dto => {
                    if (dto.NumeroCartao > 0) return Mapper.Map<ContratoPagamentoDetalheCartaoVersao>(dto);
                    if (dto.Portador != null) return Mapper.Map<ContratoPagamentoDetalheDepositoVersao>(dto);
                    if (dto.NumeroCheque > 0) return Mapper.Map<ContratoPagamentoDetalheChequeVersao>(dto);
                    if (dto.DataPagamento != null) return Mapper.Map<ContratoPagamentoDetalheDinheiroVersao>(dto);
                    return Mapper.Map<ContratoPagamentoDetalheBoletoVersao>(dto);
                });
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheCartao>()
                .IgnoreMember(t => t.Bandeira);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheCartaoVersao>()
                .IgnoreMember(t => t.Bandeira);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDeposito>()
                .IgnoreMember(t => t.Portador);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDepositoVersao>()
                .IgnoreMember(t => t.Portador);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheBoleto>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheBoletoVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheCheque>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheChequeVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDinheiro>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDinheiroVersao>();
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraDTO, Obra>()
                .MapMemberFrom(
                    dest => dest.PropostaPagamentos,
                    src => src.ObraPagamentos
                )
                .IgnoreMember(t => t.CondicaoPagamento).MapMemberFrom(t => t.CondicaoPagamentoCodigo, t => (t.CondicaoPagamento != null ? t.CondicaoPagamento.Codigo : 0))
                .IgnoreMember(t => t.ContatoPrincipalFuncao).MapMemberFrom(t => t.ContatoPrincipalFuncaoCodigo, t => (t.ContatoPrincipalFuncao != null ? t.ContatoPrincipalFuncao.Codigo : 0))
                .IgnoreMember(t => t.ContatoSecundarioFuncao).MapMemberFrom(t => t.ContatoSecundarioFuncaoCodigo, t => (t.ContatoSecundarioFuncao != null ? t.ContatoSecundarioFuncao.Codigo : 0))
                .IgnoreMember(t => t.Contrato)
                .IgnoreMember(t => t.AnoContrato)
                .IgnoreMember(t => t.NumContrato)
                .IgnoreMember(t => t.ContratoPagamentos)
                .IgnoreMember(t => t.DemaisAprovacoes)
                .MapDtoToEndereco()
                .IgnoreMember(t => t.ObraBombas)
                .IgnoreMember(t => t.ObraLogs)
                .IgnoreMember(t => t.ObraPagamentos)
                .IgnoreMember(t => t.ObraTaxas)
                .IgnoreMember(t => t.ObraTracos)
                .IgnoreMember(t => t.ObraTributacoesMunicipais)
                .IgnoreMember(t => t.ObraDemaisServicos)
                .IgnoreMember(t => t.Proposta)
                .IgnoreMember(t => t.PropostaPagamentos)
                .IgnoreMember(t => t.ObraReajuste)
                .IgnoreMember(t => t.TipoCobranca).MapMemberFrom(t => t.TipoCobrancaCodigo, t => (t.TipoCobranca != null ? t.TipoCobranca.Codigo : 0))
                .IgnoreMember(t => t.UsinaEntrega)
                .IgnoreMember(t => t.ViaCaptacao)
                .IgnoreMember(t => t.StatusProjecao)
                .IgnoreMember(t => t.TipoObra).MapMemberFrom(t => t.TipoObraCodigo, t => (t.TipoObra != null ? t.TipoObra.Codigo : 0))
                .IgnoreMember(t => t.PorteObra).MapMemberFrom(t => t.PorteObraCodigo, t => (t.PorteObra != null ? t.PorteObra.Codigo : 0))
                .IgnoreMember(t => t.TributacaoPisCofins).MapMemberFrom(t => t.TributacaoPisCofinsCodigo, t => (t.TributacaoPisCofins != null ? t.TributacaoPisCofins.Codigo : ""))
                .IgnoreMember(t => t.TributacaoIS)
                .IgnoreMember(t => t.TributacaoIBS)
                .IgnoreMember(t => t.TributacaoCBS);

            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.ObraDTO, ObraVersao>()
                .MapMemberFrom(
                    dest => dest.PropostaPagamentos,
                    src => src.ObraPagamentos
                )
                .IgnoreMember(t => t.CondicaoPagamento).MapMemberFrom(t => t.CondicaoPagamentoCodigo, t => (t.CondicaoPagamento != null ? t.CondicaoPagamento.Codigo : 0))
                .IgnoreMember(t => t.ContatoPrincipalFuncao).MapMemberFrom(t => t.ContatoPrincipalFuncaoCodigo, t => (t.ContatoPrincipalFuncao != null ? t.ContatoPrincipalFuncao.Codigo : 0))
                .IgnoreMember(t => t.ContatoSecundarioFuncao).MapMemberFrom(t => t.ContatoSecundarioFuncaoCodigo, t => (t.ContatoSecundarioFuncao != null ? t.ContatoSecundarioFuncao.Codigo : 0))
                .IgnoreMember(t => t.Contrato)
                .IgnoreMember(t => t.AnoContrato)
                .IgnoreMember(t => t.NumContrato)
                .IgnoreMember(t => t.ContratoPagamentos)
                .IgnoreMember(t => t.DemaisAprovacoes)
                .MapDtoToEndereco()
                .IgnoreMember(t => t.ObraBombas)
                .IgnoreMember(t => t.ObraLogs)
                .IgnoreMember(t => t.ObraPagamentos)
                .IgnoreMember(t => t.ObraReajuste)
                .IgnoreMember(t => t.ObraTaxas)
                .IgnoreMember(t => t.ObraTracos)
                .IgnoreMember(t => t.ObraTributacoesMunicipais)
                .IgnoreMember(t => t.ObraDemaisServicos)
                .IgnoreMember(t => t.Proposta)
                .IgnoreMember(t => t.PropostaPagamentos)
                .IgnoreMember(t => t.TipoCobranca).MapMemberFrom(t => t.TipoCobrancaCodigo, t => (t.TipoCobranca != null ? t.TipoCobranca.Codigo : 0))
                .IgnoreMember(t => t.UsinaEntrega)
                .IgnoreMember(t => t.ViaCaptacao)
                .IgnoreMember(t => t.StatusProjecao)
                .IgnoreMember(t => t.TipoObra).MapMemberFrom(t => t.TipoObraCodigo, t => (t.TipoObra != null ? t.TipoObra.Codigo : 0))
                .IgnoreMember(t => t.PorteObra).MapMemberFrom(t => t.PorteObraCodigo, t => (t.PorteObra != null ? t.PorteObra.Codigo : 0))
                .IgnoreMember(t => t.TributacaoPisCofins).MapMemberFrom(t => t.TributacaoPisCofinsCodigo, t => (t.TributacaoPisCofins != null ? t.TributacaoPisCofins.Codigo : ""))
                .IgnoreMember(t => t.TributacaoIS)
                .IgnoreMember(t => t.TributacaoIBS)
                .IgnoreMember(t => t.TributacaoCBS);

            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.PropostaAlteracaoRequest, Proposta>()
                .IgnoreMember(t => t.IntervenienteCodigo) // correção para acesso concorrente onde o cliente era cadastrado no desktop enquanto a proposta estava aberta no web
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Vendedor)
                .IgnoreMember(t => t.VendedorPadrinho).MapMemberFrom(t => t.VendedorPadrinhoCodigo, t => (t.VendedorPadrinho != null ? t.VendedorPadrinho.Codigo : 0))
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Interveniente)
                .IgnoreMember(t => t.Obra).MapMemberFrom(t => t.ObraCodigo, t => (t.Obra != null ? t.Obra.Numero : 0))
                .IgnoreMember(t => t.Cobranca)
                .IgnoreMember(t => t.Faturamento)
                .IgnoreMember(t => t.ResponsavelSolidario);
            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.PropostaAlteracaoRequest, PropostaVersao>()
                .IgnoreMember(t => t.IntervenienteCodigo) // correção para acesso concorrente onde o cliente era cadastrado no desktop enquanto a proposta estava aberta no web
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Vendedor)
                .IgnoreMember(t => t.VendedorPadrinho).MapMemberFrom(t => t.VendedorPadrinhoCodigo, t => (t.VendedorPadrinho != null ? t.VendedorPadrinho.Codigo : 0))
                .MapDtoToEndereco()
                .IgnoreMember(t => t.Interveniente)
                .IgnoreMember(t => t.Obra).MapMemberFrom(t => t.ObraCodigo, t => (t.Obra != null ? t.Obra.Numero : 0))
                .IgnoreMember(t => t.Cobranca)
                .IgnoreMember(t => t.Faturamento)
                .IgnoreMember(t => t.ResponsavelSolidario);

            cfg.CreateMap<DTOS.Request.Proposta.Inclusao.PropostaInclusaoRequest, Interveniente>()
                .MapMemberFrom(dest => dest.Razao, src => src.IntervenienteRazao)
                .MapMemberFrom(dest => dest.Nome, src => src.IntervenienteNome)
                .IgnoreMember(t => t.Vendedor)
                .IgnoreMember(t => t.VendedorCodigo)
                .MapDtoToEndereco();

            cfg.CreateMap<DTOS.Request.Proposta.Alteracao.PropostaAlteracaoRequest, Interveniente>()
                .MapMemberFrom(dest => dest.Razao, src => src.IntervenienteRazao)
                .MapMemberFrom(dest => dest.Nome, src => src.IntervenienteNome)
                .MapMemberFrom(dest => dest.IdExterno, src => src.Interveniente.IdExterno)
                .IgnoreMember(t => t.Vendedor)
                .IgnoreMember(t => t.VendedorCodigo)
                .MapDtoToEndereco();

            cfg.CreateMap<PropostaCobranca, IntervenienteLocal>()
                .IgnoreMember(t => t.IdAtualizacao).IgnoreMember(t => t.IdCadastro)
                .IgnoreMember(t => t.LocalCobrancaSimNao).IgnoreMember(t => t.LocalEntregaSimNao).IgnoreMember(t => t.LocalFaturamentoSimNao)
                .IgnoreMember(t => t.IntervenienteCodigo)
                .IgnoreMember(t => t.Sequencia);
            cfg.CreateMap<PropostaFaturamento, IntervenienteLocal>()
                .IgnoreMember(t => t.IdAtualizacao).IgnoreMember(t => t.IdCadastro)
                .IgnoreMember(t => t.LocalCobrancaSimNao).IgnoreMember(t => t.LocalEntregaSimNao).IgnoreMember(t => t.LocalFaturamentoSimNao)
                .IgnoreMember(t => t.IntervenienteCodigo)
                .IgnoreMember(t => t.Sequencia);
            cfg.CreateMap<PropostaResponsavelSolidario, IntervenienteLocal>()
                .IgnoreMember(t => t.IdAtualizacao).IgnoreMember(t => t.IdCadastro)
                .IgnoreMember(t => t.LocalCobrancaSimNao).IgnoreMember(t => t.LocalEntregaSimNao).IgnoreMember(t => t.LocalFaturamentoSimNao)
                .IgnoreMember(t => t.IntervenienteCodigo)
                .IgnoreMember(t => t.Sequencia);
            cfg.CreateMap<PropostaResponsavelSolidarioVersao, IntervenienteLocal>()
                .IgnoreMember(t => t.IdAtualizacao).IgnoreMember(t => t.IdCadastro)
                .IgnoreMember(t => t.LocalCobrancaSimNao).IgnoreMember(t => t.LocalEntregaSimNao).IgnoreMember(t => t.LocalFaturamentoSimNao)
                .IgnoreMember(t => t.IntervenienteCodigo)
                .IgnoreMember(t => t.Sequencia);
            cfg.CreateMap<PropostaFaturamentoVersao, IntervenienteLocal>()
                .IgnoreMember(t => t.IdAtualizacao).IgnoreMember(t => t.IdCadastro)
                .IgnoreMember(t => t.LocalCobrancaSimNao).IgnoreMember(t => t.LocalEntregaSimNao).IgnoreMember(t => t.LocalFaturamentoSimNao)
                .IgnoreMember(t => t.IntervenienteCodigo)
                .IgnoreMember(t => t.Sequencia);
            cfg.CreateMap<PropostaCobrancaVersao, IntervenienteLocal>()
                .IgnoreMember(t => t.IdAtualizacao).IgnoreMember(t => t.IdCadastro)
                .IgnoreMember(t => t.LocalCobrancaSimNao).IgnoreMember(t => t.LocalEntregaSimNao).IgnoreMember(t => t.LocalFaturamentoSimNao)
                .IgnoreMember(t => t.IntervenienteCodigo)
                .IgnoreMember(t => t.Sequencia);

            //Lead
            cfg.CreateMap<LeadAlteracaoRequest, Lead>()
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Vendedor)
                .IgnoreMember(t => t.ViaCaptacao)
                .IgnoreMember(t => t.Fase)
                .IgnoreMember(t => t.MotivoPerda)
                .IgnoreMember(t => t.Ano)
                .IgnoreMember(t => t.Numero)
                .MapDtoToEndereco();

            //LeadContato
            cfg.CreateMap<LeadContatoAlteracaoDTO, LeadContato>()
                .IgnoreMember(t => t.Usina)
                .IgnoreMember(t => t.Funcao);

            cfg.CreateMap<LeadAnexoAdicionarRequest, LeadAnexo>()
                .IgnoreMember(t => t.Id)
                .IgnoreMember(t => t.Descricao)
                .IgnoreMember(t => t.Usuario)
                .IgnoreMember(t => t.DataHora);

            cfg.CreateMap<LeadAnexoAtualizarRequest, LeadAnexo>()
                .IgnoreMember(t => t.Arquivo)
                .IgnoreMember(t => t.Usuario);
            
            cfg.CreateMap<LeadInteracao, LeadInteracaoResponse>();
            
            cfg.CreateMap<LeadInteracaoAdicionarRequest, LeadInteracao>()
                    .IgnoreMember(x => x.Id)
                    .IgnoreMember(x => x.IdCadastro);

            cfg.CreateMap<Visita, Lead>()
                    .MapMemberFrom(dest => dest.VisitaAno, src => src.Ano)
                    .MapMemberFrom(dest => dest.VisitaNumero, src => src.Numero)
                    .MapMemberFrom(dest => dest.VisitaNumero, src => src.Numero)
                    .IgnoreMember(t => t.Numero)
                    .IgnoreMember(t => t.Ano)
                    .IgnoreMember(t => t.OportunidadeNumero)
                    .IgnoreMember(t => t.OportunidadeAno)
                    .IgnoreMember(t => t.ViaCaptacao)
                    .IgnoreMember(t => t.ViaCaptacaoCodigo)
                    .IgnoreMember(t => t.Fase)
                    .IgnoreMember(t => t.FaseCodigo)
                    .IgnoreMember(t => t.Classificacao)
                    .IgnoreMember(t => t.ProximaEtapa)
                    .IgnoreMember(t => t.MotivoPerda)
                    .IgnoreMember(t => t.MotivoPerdaCodigo)
                    .IgnoreMember(t => t.IdCadastro)
                    .IgnoreMember(t => t.IdAtualizacao)
                    .IgnoreMember(t => t.Logs);

                cfg.CreateMap<VisitaContato, LeadContato>()
                    .MapMemberFrom(dest => dest.Ddd, src => src.DddTelefone);

                cfg.CreateMap<Lead, Oportunidade>()
                    .MapMemberFrom(dest => dest.AnoLead, src => src.Ano)
                    .MapMemberFrom(dest => dest.NumeroLead, src => src.Numero)
                    .MapMemberFrom(dest => dest.NumeroVisita, src => src.VisitaNumero)
                    .MapMemberFrom(dest => dest.AnoVisita, src => src.VisitaAno)
                    .IgnoreMember(t => t.Numero)
                    .IgnoreMember(t => t.Ano)
                    .IgnoreMember(t => t.Fase)
                    .IgnoreMember(t => t.FaseCodigo)
                    .IgnoreMember(t => t.Classificacao)
                    .IgnoreMember(t => t.ProximaEtapa)
                    .IgnoreMember(t => t.MotivoPerda)
                    .IgnoreMember(t => t.MotivoPerdaCodigo)
                    .IgnoreMember(t => t.Logs);

                cfg.CreateMap<LeadContato, OportunidadeContato>()
                    .MapMemberFrom(dest => dest.DddTelefone, src => src.Ddd);

                cfg.CreateMap<OportunidadeInteracao, OportunidadeInteracaoResponse>();

                cfg.CreateMap<OportunidadeInteracaoAdicionarRequest, OportunidadeInteracao>()
                        .IgnoreMember(x => x.Id)
                        .IgnoreMember(x => x.IdCadastro);

                // Demais Servicos
            cfg.CreateMap<DTOS.Request.DemaisServicos.Inclusao.DemaisServicosInclusaoRequest, DemaisServicos>();
                cfg.CreateMap<DTOS.Request.DemaisServicos.Alteracao.DemaisServicosAlteracaoRequest, DemaisServicos>();

                // Solicitação Pagamento
                cfg.CreateMap<DTOS.Request.SolicitacaoPagamento.CartaoBandeiraDTO, CartaoBandeira>();
                cfg.CreateMap<DTOS.Request.SolicitacaoPagamento.ObraDTO, Obra>();
                cfg.CreateMap<DTOS.Request.SolicitacaoPagamento.SolicitacaoPagamentoRequest, SolicitacaoPagamento>();

                // Assinatura Eletronica
                cfg.CreateMap<DTOS.Request.AssinaturaEletronica.Clicksign.ClicksignParametrosRequest, ClicksignParametros>();

                //Obra Pagamentos
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.CartaoBandeiraDTO, CartaoBandeira>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.CondicaoPagamentoDTO, CondicaoPagamento>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ContaDTO, Conta>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.IntervenienteDTO, Interveniente>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDTO, ContratoPagamento>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDTO, ContratoPagamentoVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDTO, ContratoPagamentoForSaving>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDTO, ContratoPagamentoForSavingVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheBoleto>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheBoletoVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheCartao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheCartaoVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheCheque>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheChequeVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDeposito>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDepositoVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDinheiro>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheDinheiroVersao>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalhe>()
                    .ConvertUsing(dto => {
                        if (dto.Bandeira != null) return Mapper.Map<ContratoPagamentoDetalheCartao>(dto);
                        if (dto.Portador != null) return Mapper.Map<ContratoPagamentoDetalheDeposito>(dto);
                        if (dto.NumeroRecibo != null) return Mapper.Map<ContratoPagamentoDetalheDinheiro>(dto);
                        if (dto.NumeroCheque > 0) return Mapper.Map<ContratoPagamentoDetalheCheque>(dto);
                        return Mapper.Map<ContratoPagamentoDetalheBoleto>(dto);
                    });
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentoDetalheDTO, ContratoPagamentoDetalheVersao>()
                    .ConvertUsing(dto => {
                        if (dto.Bandeira != null) return Mapper.Map<ContratoPagamentoDetalheCartaoVersao>(dto);
                        if (dto.Portador != null) return Mapper.Map<ContratoPagamentoDetalheDepositoVersao>(dto);
                        if (dto.NumeroRecibo != null) return Mapper.Map<ContratoPagamentoDetalheDinheiroVersao>(dto);
                        if (dto.NumeroCheque > 0) return Mapper.Map<ContratoPagamentoDetalheChequeVersao>(dto);
                        return Mapper.Map<ContratoPagamentoDetalheBoletoVersao>(dto);
                    });
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.PortadorDTO, Portador>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.TipoCobrancaDTO, TipoCobranca>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.MovimentoBancoDTO, MovimentoBanco>();
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentosAprovacaoRequest, Obra>()
                    .MapMemberFrom(dest => dest.ContratoPagamentos, src => src.ObraPagamentos);
                cfg.CreateMap<DTOS.Request.Obra.ObraPagamentosAprovacaoRequest.ObraPagamentosAprovacaoRequest, ObraVersao>()
                    .MapMemberFrom(dest => dest.ContratoPagamentos, src => src.ObraPagamentos);

                // Interveniente
                cfg.CreateMap<DTOS.Request.Interveniente.AlterarLimiteCreditoRequest.AlterarLimiteCreditoRequest, Interveniente>()
                    .IgnoreMember(t => t.BloqueioMotivo);

                // Interveniente Historico
                cfg.CreateMap<DTOS.Request.IntervenienteHistorico.Inclusao.IntervenienteHistoricoRequest, IntervenienteHistorico>()
                .ForMember(t => t.HoraPrevistaDeRetorno, opt => opt.MapFrom(t =>
                (t.HoraPrevistaDeRetornoString != "" ? TimeSpan.Parse(t.HoraPrevistaDeRetornoString.Insert(2, ":")) : (TimeSpan?)null)
                ));

                // Condição de pagamento
                cfg.CreateMap<CondicaoPagamento, DTOS.Request.CondicaoPagamento.Alteracao.CondicaoPagamentoAlteracaoRequest>()
                    .ReverseMap()
                    .IgnoreMember(t => t.CondicaoDaCobranca);
                cfg.CreateMap<CondicaoPagamento, DTOS.Request.CondicaoPagamento.Inclusao.CondicaoPagamentoInclusaoRequest>()
                    .ReverseMap()
                    .IgnoreMember(t => t.CondicaoDaCobranca);
                cfg.CreateMap<CondicaoPagamentoParcela, DTOS.Request.CondicaoPagamento.Inclusao.CondicaoPagamentoParcelaDTO>()
                    .ReverseMap()
                    .IgnoreMember(t => t.CondicaoPagamento);
                cfg.CreateMap<CondicaoPagamentoParcela, DTOS.Request.CondicaoPagamento.Alteracao.CondicaoPagamentoParcelaDTO>()
                    .ReverseMap()
                    .IgnoreMember(t => t.CondicaoPagamento);

                cfg.CreateMap<DTOS.Request.AssinaturaEletronica.Clicksign.SolicitacaoAssinaturaClicksignRequest, SolicitacaoAssinaturaEletronicaClicksign>()
                    .MapMemberFrom(src => src.TelefoneResponsavelSolidario, dest => $"{dest.TelefoneDddResponsavelSolidario}{dest.TelefoneNumeroResponsavelSolidario}")
                    .MapMemberFrom(src => src.TelefonePrimeiraTestemunha, dest => $"{dest.TelefoneDddPrimeiraTestemunha}{dest.TelefoneNumeroPrimeiraTestemunha}")
                    .MapMemberFrom(src => src.TelefoneSegundaTestemunha, dest => $"{dest.TelefoneDddSegundaTestemunha}{dest.TelefoneNumeroSegundaTestemunha}")
                    .ForMember(dest => dest.DadosPessoaisAssinatura, opt => opt.MapFrom(src => src.DadosPessoaisAssinatura));

                cfg.CreateMap<DTOS.Request.AssinaturaEletronica.Clicksign.DadosPessoaisAssinaturaRequest, DadosPessoaisAssinatura>()
                    .MapMemberFrom(dest => dest.Telefone, src => $"{src.TelefoneDdd}{src.TelefoneNumero}");

                // DTOs Response
                // Obra
                cfg.CreateMap<CondicaoPagamento, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.CondicaoPagamentoDTO>()
                    .ReverseMap();
                cfg.CreateMap<CadastroGeral, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.CadastroGeralDTO>()
                    .ReverseMap();
                cfg.CreateMap<TipoCobranca, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.CadastroGeralDTO>()
                    .ReverseMap();
                cfg.CreateMap<Usina, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.UsinaDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraTraco, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraTracoDTO>()
                    .MapMemberFrom(src => src.Mpa, dest => dest.Fck)
                    .ReverseMap()
                    .MapMemberFrom(dest => dest.Fck, src => src.Mpa);
                cfg.CreateMap<ObraTracoVersao, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraTracoDTO>()
                    .MapMemberFrom(src => src.Mpa, dest => dest.Fck)
                    .ReverseMap()
                    .MapMemberFrom(dest => dest.Fck, src => src.Mpa);//****
                cfg.CreateMap<Pedra, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.PedraDTO>()
                    .ReverseMap();
                cfg.CreateMap<Slump, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.SlumpDTO>()
                    .ReverseMap();
                cfg.CreateMap<Uso, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.UsoDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraBomba, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraBombaDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraBombaVersao, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraBombaDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraTaxa, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraTaxaDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraTaxaVersao, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraTaxaDTO>()
                    .ReverseMap();
                cfg.CreateMap<AprovacaoTipo, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.AprovacaoTipoDTO>()
                    .ReverseMap();
                cfg.CreateMap<DemaisAprovacao, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.DemaisAprovacaoDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraLog, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraLogDTO>()
                    .ReverseMap();
                cfg.CreateMap<ObraLogVersao, DTOS.Response.Obra.ObraPendenteAprovacaoResponse.ObraLogDTO>()
                    .ReverseMap();
                cfg.CreateMap<Obra, DTOS.Response.Obra.ObraTracosResponse.ObraTracosResponse>()
                    .ReverseMap();
                cfg.CreateMap<ObraTraco, DTOS.Response.Obra.ObraTracosResponse.ObraTracoDTO>()
                    .ReverseMap();
                cfg.CreateMap<Pedra, DTOS.Response.Obra.ObraTracosResponse.PedraDTO>()
                    .ReverseMap();
                cfg.CreateMap<Slump, DTOS.Response.Obra.ObraTracosResponse.SlumpDTO>()
                    .ReverseMap();
                cfg.CreateMap<Uso, DTOS.Response.Obra.ObraTracosResponse.UsoDTO>()
                    .ReverseMap();
                cfg.CreateMap<Interveniente, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.IntervenienteDTO>()
                    .ReverseMap();
                cfg.CreateMap<Contrato, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.ContratoDTO>()
                    .ReverseMap();
                cfg.CreateMap<ContratoVersao, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.ContratoDTO>()
                    .ReverseMap();
                cfg.CreateMap<CadastroGeral, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.CadastroGeralDTO>()
                    .ReverseMap();
                cfg.CreateMap<Usina, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.UsinaDTO>()
                    .ReverseMap();
                cfg.CreateMap<TipoCobranca, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.TipoCobrancaDTO>()
                    .ReverseMap();
                cfg.CreateMap<Funcionario, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.FuncionarioDTO>()
                   .ReverseMap();
                cfg.CreateMap<Obra, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.ObraParaAnaliseCadastroResponse>()
                    .MapEnderecoToDto()
                    .ReverseMap();
                cfg.CreateMap<ObraVersao, DTOS.Response.Obra.ObraParaAnaliseCadastroResponse.ObraParaAnaliseCadastroResponse>()
                    .MapEnderecoToDto()
                    .ReverseMap();
                cfg.CreateMap<ObraTraco, DTOS.Response.Obra.ObraTracoResponse.ObraTracoResponse>()
                    .MapMemberFrom(dest => dest.Mpa, src => src.Fck);
                cfg.CreateMap<ObraTracoVersao, DTOS.Response.Obra.ObraTracoResponse.ObraTracoResponse>() //**
                    .MapMemberFrom(dest => dest.Mpa, src => src.Fck);

                // ObraLog
                cfg.CreateMap<ObraLog, DTOS.Response.Obra.ObraLogResponse.ObraLogResponse>();

                // ObraProjecao
                cfg.CreateMap<ObraProjecao, DTOS.Response.Obra.ObraProjecao.ObraProjecaoResponse>();

                // Contrato
                cfg.CreateMap<Contrato, DTOS.Response.Contrato.ContratoRevalidacaoCadastroResponse.ContratoRevalidacaoCadastroResponse>();
                cfg.CreateMap<Interveniente, DTOS.Response.Contrato.ContratoRevalidacaoCadastroResponse.IntervenienteDTO>();
                cfg.CreateMap<CadastroGeral, DTOS.Response.Contrato.ContratoRevalidacaoCadastroResponse.CadastroGeralDTO>();
                cfg.CreateMap<Contrato, DTOS.Response.Contrato.ContratoIntegracao.ContratoSimplificadoResponse>()
                    .IgnoreMember(x => x.Pagamentos);

                // Contrato Gerado
                cfg.CreateMap<Contrato, DTOS.Response.Contrato.ContratoGeradoResponse.ContratoGeradoResponse>();

                // CadastroGeral
                cfg.CreateMap<CadastroGeral, DTOS.Response.CadastroGeral.CadastroGeralResponse>();

                // CadastroDiverso
                cfg.CreateMap<CadastroDiverso, DTOS.Response.CadastroDiverso.CadastroDiversoResponse>();

                // Interveniente
                cfg.CreateMap<CadastroGeral, DTOS.Response.Interveniente.CadastroGeralDTO>();
                cfg.CreateMap<GrupoEconomico, DTOS.Response.Interveniente.GrupoEconomicoDTO>();
                cfg.CreateMap<Interveniente, DTOS.Response.Interveniente.IntervenienteResponse>()
                .MapEnderecoToDto();
                cfg.CreateMap<DTOS.Request.Interveniente.Inclusao.IntervenienteInclusaoRequest, Interveniente>()
                    .IgnoreMember(t => t.Vendedor)
                    .IgnoreMember(t => t.LimiteValor)
                    .IgnoreMember(t => t.LimiteData)
                    .IgnoreMember(t => t.BloqueioMotivoCodigo)
                    .IgnoreMember(t => t.BloqueioObservacao)
                    .IgnoreMember(t => t.bombista)
                    .IgnoreMember(t => t.BloqueioMotivo)
                    .IgnoreMember(t => t.EnderecoMunicipio)
                    .IgnoreMember(t => t.Contratos)
                    .IgnoreMember(t => t.IdAtualizacao)
                    .IgnoreMember(t => t.RetemIss)
                    .IgnoreMember(t => t.IdAprovacaoRetencaoIss)
                    .IgnoreMember(t => t.Codigo);

                cfg.CreateMap<DTOS.Request.Interveniente.Alteracao.IntervenienteAlteracaoRequest, Interveniente>()
                    .IgnoreMember(t => t.Vendedor)
                    .IgnoreMember(t => t.LimiteValor)
                    .IgnoreMember(t => t.LimiteData)
                    .IgnoreMember(t => t.BloqueioMotivoCodigo)
                    .IgnoreMember(t => t.BloqueioObservacao)
                    .IgnoreMember(t => t.bombista)
                    .IgnoreMember(t => t.BloqueioMotivo)
                    .IgnoreMember(t => t.EnderecoMunicipio)
                    .IgnoreMember(t => t.Contratos)
                    .IgnoreMember(t => t.IdAtualizacao)
                    .IgnoreMember(t => t.RetemIss)
                    .IgnoreMember(t => t.IdAprovacaoRetencaoIss)
                    .IgnoreMember(t => t.Codigo)
                    .IgnoreMember(t => t.IntervenienteTipo);

                //Interveniente Request Integração Publica 

                cfg.CreateMap<DTOS.Request.Interveniente.IntervenienteAdicionarRequest, Interveniente>()
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente ? "S" : "N"))
                    .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor ? "S" : "N"))
                    .ForMember(dest => dest.Transportador, opt => opt.MapFrom(src => src.Transportador ? "S" : "N"))
                    .ForMember(dest => dest.PrestadorServico, opt => opt.MapFrom(src => src.PrestadorServico ? "S" : "N"))
                    .ForMember(dest => dest.OrgaoPublico, opt => opt.MapFrom(src => src.OrgaoPublico ? "S" : "N"))
                    .ForMember(dest => dest.Outros, opt => opt.MapFrom(src => src.Outros ? "S" : "N"))
                    .ForMember(dest => dest.ContaContabil, opt => opt.MapFrom(src => src.ContaContabil.HasValue ? src.ContaContabil.ToString() : ""))
                    .ForMember(dest => dest.In86, opt => opt.MapFrom(src => src.In86.HasValue ? (src.In86.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.Inativo, opt => opt.MapFrom(src => src.Inativo.HasValue ? (src.Inativo.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.RetemIrrf, opt => opt.MapFrom(src => src.RetemIrrf.HasValue ? (src.RetemIrrf.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.RetemCofins, opt => opt.MapFrom(src => src.RetemCofins.HasValue ? (src.RetemCofins.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.RetemPis, opt => opt.MapFrom(src => src.RetemPis.HasValue ? (src.RetemPis.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.RetemCsll, opt => opt.MapFrom(src => src.RetemCsll.HasValue ? (src.RetemCsll.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional.HasValue ? (src.SimplesNacional.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.RetemInss, opt => opt.MapFrom(src => src.RetemInss.HasValue ? (src.RetemInss.Value ? "S" : "N") : ""))
                    .ForMember(dest => dest.Funcionario, opt => opt.MapFrom(src => src.Funcionario.HasValue ? (src.Funcionario.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.LocalEntrega, opt => opt.MapFrom(src => src.LocalEntrega.HasValue ? (src.LocalEntrega.Value ? "S" : "N") : ""))
                    .ForMember(dest => dest.FornecedorMp, opt => opt.MapFrom(src => src.FornecedorMp.HasValue ? (src.FornecedorMp.Value ? "S" : "N") : ""))
                    .ForMember(dest => dest.RetemIss, opt => opt.MapFrom(src => src.RetemIss == 0 ? "N" : src.RetemIss == 1 ? "S" : "X"))
                    .ForMember(dest => dest.VendedorCodigo, opt => opt.MapFrom(src => src.VendedorCodigo == "" ? 0 : int.Parse(src.VendedorCodigo)))
                    .ForMember(dest => dest.ContribuiIcms, opt => opt.MapFrom(src => src.ContribuiIcms == 3 ? 9 : src.ContribuiIcms))
                    .ForMember(dest => dest.bombista, opt => opt.MapFrom(src => src.bombista.HasValue ? (src.bombista.Value ? "S" : "N") : ""))
                    .ForMember(dest => dest.AprovacaoEngenharia, opt => opt.MapFrom(src => src.AprovacaoEngenharia.HasValue ? (src.AprovacaoEngenharia.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional.HasValue ? (src.SimplesNacional.Value ? "S" : "N") : "N"))
                    .AfterMap((src, dest) => dest.OrgaoExpedidor = "")
                    .AfterMap((src, dest) => dest.IdAtualizacao = "")
                    .AfterMap((src, dest) => dest.Profissao = "")
                    .AfterMap((src, dest) => dest.EmpresaTrabalho = "");

                cfg.CreateMap<DTOS.Request.Interveniente.IntervenienteAtualizarRequest, Interveniente>()
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente == null ? null : src.Cliente == true ? "S" : "N"))
                    .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor == null ? null : src.Fornecedor == true ? "S" : "N"))
                    .ForMember(dest => dest.Transportador, opt => opt.MapFrom(src => src.Transportador == null ? null : src.Transportador == true ? "S" : "N"))
                    .ForMember(dest => dest.PrestadorServico, opt => opt.MapFrom(src => src.PrestadorServico == null ? null : src.PrestadorServico == true ? "S" : "N"))
                    .ForMember(dest => dest.OrgaoPublico, opt => opt.MapFrom(src => src.OrgaoPublico == null ? null : src.OrgaoPublico == true ? "S" : "N"))
                    .ForMember(dest => dest.Outros, opt => opt.MapFrom(src => src.Outros == null ? null : src.Outros == true ? "S" : "N"))
                    .ForMember(dest => dest.ContaContabil, opt => opt.MapFrom(src => src.ContaContabil.HasValue ? src.ContaContabil.ToString() : null))
                    .ForMember(dest => dest.In86, opt => opt.MapFrom(src => src.In86 == null ? null : src.In86 == true ? "S" : "N"))
                    .ForMember(dest => dest.Inativo, opt => opt.MapFrom(src => src.Inativo == null ? null : src.Inativo == true ? "S" : "N"))
                    .ForMember(dest => dest.RetemIrrf, opt => opt.MapFrom(src => src.RetemIrrf == null ? null : src.RetemIrrf == true ? "S" : "N"))
                    .ForMember(dest => dest.RetemCofins, opt => opt.MapFrom(src => src.RetemCofins == null ? null : src.RetemCofins == true ? "S" : "N"))
                    .ForMember(dest => dest.RetemPis, opt => opt.MapFrom(src => src.RetemPis == null ? null : src.RetemPis == true ? "S" : "N"))
                    .ForMember(dest => dest.RetemCsll, opt => opt.MapFrom(src => src.RetemCsll == null ? null : src.RetemCsll == true ? "S" : "N"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional == null ? null : src.SimplesNacional == true ? "S" : "N"))
                    .ForMember(dest => dest.RetemInss, opt => opt.MapFrom(src => src.RetemInss == null ? null : src.RetemInss == true ? "S" : "N"))
                    .ForMember(dest => dest.Funcionario, opt => opt.MapFrom(src => src.Funcionario == null ? null : src.Funcionario == true ? "S" : "N"))
                    .ForMember(dest => dest.LocalEntrega, opt => opt.MapFrom(src => src.LocalEntrega == null ? null : src.LocalEntrega == true ? "S" : "N"))
                    .ForMember(dest => dest.FornecedorMp, opt => opt.MapFrom(src => src.FornecedorMp == null ? null : src.FornecedorMp == true ? "S" : "N"))
                    .ForMember(dest => dest.RetemIss, opt => opt.MapFrom(src => src.RetemIss == 0 ? "N" : src.RetemIss == 1 ? "S" : "X"))
                    .ForMember(dest => dest.VendedorCodigo, opt => opt.MapFrom(src => src.VendedorCodigo == "" ? 0 : int.Parse(src.VendedorCodigo)))
                    .ForMember(dest => dest.ContribuiIcms, opt => opt.MapFrom(src => src.ContribuiIcms == 3 ? 9 : src.ContribuiIcms))
                    .ForMember(dest => dest.bombista, opt => opt.MapFrom(src => src.bombista == null ? null : src.bombista == true ? "S" : "N"))
                    .ForMember(dest => dest.AprovacaoEngenharia, opt => opt.MapFrom(src => src.AprovacaoEngenharia == null ? null : src.AprovacaoEngenharia == true ? "S" : "N"))
                    .AfterMap((src, dest) => dest.OrgaoExpedidor = "")
                    .AfterMap((src, dest) => dest.IdAtualizacao = "")
                    .AfterMap((src, dest) => dest.Profissao = "")
                    .AfterMap((src, dest) => dest.EmpresaTrabalho = "")
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<Interveniente, DTOS.Request.Interveniente.IntervenienteAtualizarRequest>()
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente == "S"))
                    .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor == "S"))
                    .ForMember(dest => dest.Transportador, opt => opt.MapFrom(src => src.Transportador == "S"))
                    .ForMember(dest => dest.PrestadorServico, opt => opt.MapFrom(src => src.PrestadorServico == "S"))
                    .ForMember(dest => dest.OrgaoPublico, opt => opt.MapFrom(src => src.OrgaoPublico == "S"))
                    .ForMember(dest => dest.Outros, opt => opt.MapFrom(src => src.Outros == "S"))
                    .ForMember(dest => dest.EnderecoCep, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.EnderecoCep) ? default(int) : int.Parse(src.EnderecoCep)))
                    .ForMember(dest => dest.ContaContabil, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.ContaContabil) ? default(int) : long.Parse(src.ContaContabil)))
                    .ForMember(dest => dest.In86, opt => opt.MapFrom(src => src.In86 == "S"))
                    .ForMember(dest => dest.Inativo, opt => opt.MapFrom(src => src.Inativo == "S"))
                    .ForMember(dest => dest.RetemIrrf, opt => opt.MapFrom(src => src.RetemIrrf == "S"))
                    .ForMember(dest => dest.RetemCofins, opt => opt.MapFrom(src => src.RetemCofins == "S"))
                    .ForMember(dest => dest.RetemPis, opt => opt.MapFrom(src => src.RetemPis == "S"))
                    .ForMember(dest => dest.RetemCsll, opt => opt.MapFrom(src => src.RetemCsll == "S"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional == "S"))
                    .ForMember(dest => dest.RetemInss, opt => opt.MapFrom(src => src.RetemInss == "S"))
                    .ForMember(dest => dest.Funcionario, opt => opt.MapFrom(src => src.Funcionario == "S"))
                    .ForMember(dest => dest.LocalEntrega, opt => opt.MapFrom(src => src.LocalEntrega == "S"))
                    .ForMember(dest => dest.FornecedorMp, opt => opt.MapFrom(src => src.FornecedorMp == "S"))
                    .ForMember(dest => dest.RetemIss, opt => opt.MapFrom(src => src.RetemIss == "N" ? 0 : src.RetemIss == "S" ? 1 : 2))
                    .ForMember(dest => dest.VendedorCodigo, opt => opt.MapFrom(src => src.VendedorCodigo.HasValue ? src.VendedorCodigo.ToString() : ""))
                    .ForMember(dest => dest.ContribuiIcms, opt => opt.MapFrom(src => src.ContribuiIcms == 9 ? 3 : src.ContribuiIcms))
                    .ForMember(dest => dest.bombista, opt => opt.MapFrom(src => src.bombista == "S"))
                    .ForMember(dest => dest.AprovacaoEngenharia, opt => opt.MapFrom(src => src.AprovacaoEngenharia == "S"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional == "S"));

                // Interveniente Response Integração Publica 
                cfg.CreateMap<Interveniente, DTOS.Response.Interveniente.PublicoIntervenienteResponse>()
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente == "S"))
                    .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor == "S"))
                    .ForMember(dest => dest.Transportador, opt => opt.MapFrom(src => src.Transportador == "S"))
                    .ForMember(dest => dest.PrestadorServico, opt => opt.MapFrom(src => src.PrestadorServico == "S"))
                    .ForMember(dest => dest.OrgaoPublico, opt => opt.MapFrom(src => src.OrgaoPublico == "S"))
                    .ForMember(dest => dest.Outros, opt => opt.MapFrom(src => src.Outros == "S"))
                    .ForMember(dest => dest.ContaContabil, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.ContaContabil) ? default(int) : long.Parse(src.ContaContabil)))
                    .ForMember(dest => dest.In86, opt => opt.MapFrom(src => src.In86 == "S"))
                    .ForMember(dest => dest.Inativo, opt => opt.MapFrom(src => src.Inativo == "S"))
                    .ForMember(dest => dest.RetemIrrf, opt => opt.MapFrom(src => src.RetemIrrf == "S"))
                    .ForMember(dest => dest.RetemCofins, opt => opt.MapFrom(src => src.RetemCofins == "S"))
                    .ForMember(dest => dest.RetemPis, opt => opt.MapFrom(src => src.RetemPis == "S"))
                    .ForMember(dest => dest.RetemCsll, opt => opt.MapFrom(src => src.RetemCsll == "S"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional == "S"))
                    .ForMember(dest => dest.RetemInss, opt => opt.MapFrom(src => src.RetemInss == "S"))
                    .ForMember(dest => dest.Funcionario, opt => opt.MapFrom(src => src.Funcionario == "S"))
                    .ForMember(dest => dest.LocalEntrega, opt => opt.MapFrom(src => src.LocalEntrega == "S"))
                    .ForMember(dest => dest.FornecedorMp, opt => opt.MapFrom(src => src.FornecedorMp == "S"))
                    .ForMember(dest => dest.RetemIss, opt => opt.MapFrom(src => src.RetemIss == "N" ? 0 : src.RetemIss == "S" ? 1 : 2))
                    .ForMember(dest => dest.VendedorCodigo, opt => opt.MapFrom(src => src.VendedorCodigo.HasValue ? src.VendedorCodigo.ToString() : ""))
                    .ForMember(dest => dest.ContribuiIcms, opt => opt.MapFrom(src => src.ContribuiIcms == 9 ? 3 : src.ContribuiIcms))
                    .ForMember(dest => dest.bombista, opt => opt.MapFrom(src => src.bombista == "S"))
                    .ForMember(dest => dest.AprovacaoEngenharia, opt => opt.MapFrom(src => src.AprovacaoEngenharia == "S"))
                    .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional == "S"));


                // Interveniente Simples
                cfg.CreateMap<Interveniente, DTOS.Response.Interveniente.IntervenienteSimplesResponse.IntervenienteSimplesResponse>();

                //Interveniente Historico
                cfg.CreateMap<IntervenienteHistorico, DTOS.Response.IntervenienteHistorico.IntervenienteHistoricoResponse>();

                //Interveniente Anexo
                cfg.CreateMap<DTOS.Request.IntervenienteAnexo.IntervenienteAnexoAdicionarRequest, IntervenienteAnexo >();
                cfg.CreateMap<DTOS.Request.IntervenienteAnexo.IntervenienteAnexoAtualizarRequest, IntervenienteAnexo>();
                cfg.CreateMap<IntervenienteAnexo, DTOS.Response.IntervenienteAnexo.IntervenienteAnexoResponse>();

                // Usina
                cfg.CreateMap<Usina, DTOS.Response.Usina.UsinaResponse>();
                cfg.CreateMap<Usina, DTOS.Response.Usina.ParametroProgramacaoResponse>();

                // Pedra
                cfg.CreateMap<Pedra, DTOS.Response.Pedra.PedraResponse>();

                // ProgramacaoSimples
                cfg.CreateMap<CadastroGeral, DTOS.Response.Programacao.ProgramacaoSimplesResponse.CadastroGeralDTO>();
                cfg.CreateMap<ObraBomba, DTOS.Response.Programacao.ProgramacaoSimplesResponse.ObraBombaDTO>();
                cfg.CreateMap<Obra, DTOS.Response.Programacao.ProgramacaoSimplesResponse.ObraDTO>();
                cfg.CreateMap<Usina, DTOS.Response.Programacao.ProgramacaoSimplesResponse.UsinaDTO>();
                cfg.CreateMap<CondicaoPagamento, DTOS.Response.Programacao.ProgramacaoSimplesResponse.CondicaoPagamentoDTO>();
                cfg.CreateMap<Proposta, DTOS.Response.Programacao.ProgramacaoSimplesResponse.PropostaDTO>();
                cfg.CreateMap<Pedra, DTOS.Response.Programacao.ProgramacaoSimplesResponse.PedraDTO>();
                cfg.CreateMap<Slump, DTOS.Response.Programacao.ProgramacaoSimplesResponse.SlumpDTO>();
                cfg.CreateMap<Uso, DTOS.Response.Programacao.ProgramacaoSimplesResponse.UsoDTO>();
                cfg.CreateMap<Programacao, DTOS.Response.Programacao.ProgramacaoSimplesResponse.ProgramacaoSimplesResponse>()
                    .MapEnderecoToDto();

                // ProgramacaoDetalhada
                cfg.CreateMap<Pedra, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.PedraDTO>();
                cfg.CreateMap<Slump, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.SlumpDTO>();
                cfg.CreateMap<Usina, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.UsinaDTO>();
                cfg.CreateMap<Uso, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.UsoDTO>();
                cfg.CreateMap<Vendedor, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.VendedorDTO>();
                cfg.CreateMap<ProgramacaoDemaisServicos, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.ProgramacaoDemaisServicosDTO>();
                cfg.CreateMap<Programacao, DTOS.Response.Programacao.ProgramacaoDetalhadaResponse.ProgramacaoDetalhadaResponse>()
                    .MapEnderecoToDto();

                // ProgramacaoLog
                cfg.CreateMap<ProgramacaoLog, DTOS.Response.Programacao.ProgramacaoLogResponse.ProgramacaoLogResponse>();

                // Slump
                cfg.CreateMap<Slump, DTOS.Response.Slump.SlumpResponse>();

                // Uso
                cfg.CreateMap<Uso, DTOS.Response.Uso.UsoResponse>();

                // TracoPreco
                cfg.CreateMap<Usina, DTOS.Response.TracoPreco.UsinaDTO>();
                cfg.CreateMap<Uso, DTOS.Response.TracoPreco.UsoDTO>();
                cfg.CreateMap<Pedra, DTOS.Response.TracoPreco.PedraDTO>();
                cfg.CreateMap<Slump, DTOS.Response.TracoPreco.SlumpDTO>();
                cfg.CreateMap<SlumpReal, DTOS.Response.TracoPreco.SlumpDTO>();
                cfg.CreateMap<Vendedor, DTOS.Response.TracoPreco.VendedorDTO>();
                cfg.CreateMap<TracoPreco, DTOS.Response.TracoPreco.TracoPrecoResponse>();

                //TracoParticuliaridades
                cfg.CreateMap<TracoParticularidades, DTOS.Response.TracoParticuliaridades.TracoParticuliaridadesResponse>();

                // BombaPreco
                cfg.CreateMap<Usina, DTOS.Response.BombaPreco.UsinaDTO>();
                cfg.CreateMap<Interveniente, DTOS.Response.BombaPreco.IntervenienteDTO>();
                cfg.CreateMap<CadastroGeral, DTOS.Response.BombaPreco.CadastroGeralDTO>();
                cfg.CreateMap<BombaPreco, DTOS.Response.BombaPreco.BombaPrecoResponse>();
                cfg.CreateMap<BombaPrecoTerceiro, DTOS.Response.BombaPreco.BombaPrecoTerceiroResponse>();

                // Peça a concretar
                cfg.CreateMap<PecaAConcretar, DTOS.Response.PecaAConcretar.PecaAConcretarResponse>();

                // Endereço
                cfg.CreateMap<Endereco, DTOS.Response.Endereco.EnderecoResponse>();

                // Condição de pagamento
                cfg.CreateMap<CondicaoPagamento, DTOS.Response.CondicaoPagamento.CondicaoPagamentoResponse>();
                cfg.CreateMap<CondicaoPagamentoParcela, DTOS.Response.CondicaoPagamento.CondicaoPagamentoParcelaDTO>();
                cfg.CreateMap<CondicaoPagamentoParcela, DTOS.Response.CondicaoPagamento.CondicaoPagamentoParcelaDTO>();
                cfg.CreateMap<CondicaoPagamentoParcela, DTOS.Response.CondicaoPagamento.CondicaoPagamentoParcelaDTO>();

                // Condição de pagamento Integrations
                cfg.CreateMap<CondicoesPagamentoAdicionarRequest, CondicaoPagamento>()
                    .ForMember(dest => dest.VencimentoFixoSemana, opt => opt.MapFrom(src => src.VencimentoFixoSemana ? "S" : "N"))
                    .ForMember(dest => dest.TiposDeCobrancaCodigos, opt => opt.ResolveUsing(src => src.TiposDeCobrancaCodigos != null ? string.Join(",", src.TiposDeCobrancaCodigos) : null))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
                cfg.CreateMap<CondicoesPagamentoAtualizarRequest, CondicaoPagamento>()
                    .IgnoreMember(t => t.Codigo)
                    .ForMember(dest => dest.VencimentoFixoSemana, opt => opt.MapFrom(src => src.VencimentoFixoSemana == null ? null : src.VencimentoFixoSemana.Value ? "S" : "N"))
                    .ForMember(dest => dest.TiposDeCobrancaCodigos, opt => opt.ResolveUsing(src => src.TiposDeCobrancaCodigos != null ? string.Join(",", src.TiposDeCobrancaCodigos) : null))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));


                cfg.CreateMap<CondicaoPagamento, CondicaoDePagamentoResponse>()
                    .ForMember(dest => dest.VencimentoFixoSemana, opt => opt.MapFrom(src => src.VencimentoFixoSemana == "S"))
                    .ForMember(dest => dest.TiposDeCobrancaCodigos, opt => opt.ResolveUsing(src =>
                    {
                        return src.TiposDeCobrancaCodigos?.Split(',')
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .Select(s => int.TryParse(s, out var parsedValue) ? parsedValue : 0)
                            .ToList();
                    }))
                    .ForMember(dest => dest.PercentualParcelas, opt => opt.ResolveUsing(src => src.Parcelas.Select(p => p.Percentual)));

                //Municipio de Tributação
                cfg.CreateMap<MunicipalTaxesInclusion, MunicipalTaxes>()
                    .ForMember(dest => dest.Code, src => src.Ignore())
                    .ForMember(dest => dest.RegisterId, src => src.Ignore());
                cfg.CreateMap<MunicipalTaxesAlterationRequest, MunicipalTaxes>()
                    .ForMember(dest => dest.Code, src => src.Ignore())
                    .ForMember(dest => dest.RegisterId, src => src.Ignore());

                // Tipo de cobrança
                cfg.CreateMap<Conta, DTOS.Response.TipoCobranca.ContaDTO>();
                cfg.CreateMap<Portador, DTOS.Response.TipoCobranca.PortadorDTO>();
                cfg.CreateMap<TipoCobranca, DTOS.Response.TipoCobranca.TipoCobrancaResponse>();

                cfg.CreateMap<DTOS.Request.TipoDeCobranca.TipoDeCobrancaAdicionarRequest, TipoDeCobranca>()
                    .ForMember(dest => dest.Obrigatorio, opt => opt.MapFrom(src => src.Obrigatorio.HasValue ? (src.Obrigatorio.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.Aprovacao, opt => opt.MapFrom(src => src.Aprovacao.HasValue ? (src.Aprovacao.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo.HasValue ? (src.Fixo.Value ? "S" : "N") : "N"))
                    .ForMember(dest => dest.UtilCap, opt => opt.MapFrom(src => src.UtilCap.HasValue ? (src.UtilCap.Value ? "S" : "N") : "N"));
                cfg.CreateMap<DTOS.Request.TipoDeCobranca.TipoDeCobrancaAtualizarRequest, TipoDeCobranca>()
                    .ForMember(dest => dest.Obrigatorio, opt => opt.MapFrom(src => src.Obrigatorio == null ? null : src.Obrigatorio.Value ? "S" : "N"))
                    .ForMember(dest => dest.Aprovacao, opt => opt.MapFrom(src => src.Aprovacao == null ? null : src.Aprovacao.Value ? "S" : "N"))
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == null ? null : src.Fixo.Value ? "S" : "N"))
                    .ForMember(dest => dest.UtilCap, opt => opt.MapFrom(src => src.UtilCap == null ? null : src.UtilCap.Value ? "S" : "N"))
                    .IgnoreMember(t => t.Codigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
                cfg.CreateMap<TipoDeCobranca, DTOS.Response.TipoDeCobranca.TipoDeCobrancaResponse>()
                    .ForMember(dest => dest.Obrigatorio, opt => opt.MapFrom(src => src.Obrigatorio == "S"))
                    .ForMember(dest => dest.Aprovacao, opt => opt.MapFrom(src => src.Aprovacao == "S"))
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == "S"))
                    .ForMember(dest => dest.UtilCap, opt => opt.MapFrom(src => src.UtilCap == "S"));

                // Bandeira de cartão
                cfg.CreateMap<Interveniente, DTOS.Response.CartaoBandeira.IntervenienteDTO>();
                cfg.CreateMap<Conta, DTOS.Response.CartaoBandeira.ContaDTO>();
                cfg.CreateMap<Portador, DTOS.Response.CartaoBandeira.PortadorDTO>();
                cfg.CreateMap<CartaoBandeira, DTOS.Response.CartaoBandeira.CartaoBandeiraResponse>();

                // Portador
                cfg.CreateMap<Conta, DTOS.Response.Portador.ContaDTO>();
                cfg.CreateMap<Portador, DTOS.Response.Portador.PortadorResponse>();

                cfg.CreateMap<DTOS.Request.PortadorCobranca.PortadorCobrancaAdicionarRequest, Portador>()
                    .ForMember(dest => dest.EmiteCobranca, opt => opt.MapFrom(src => src.EmiteCobranca.Value ? "S" : "N"))
                    .IgnoreMember(t => t.Conta);
                cfg.CreateMap<DTOS.Request.PortadorCobranca.PortadorCobrancaAtualizarRequest, Portador>()
                    .ForMember(dest => dest.EmiteCobranca, opt => opt.MapFrom(src =>src.EmiteCobranca == null ? null : src.EmiteCobranca.Value ? "S" : "N"))
                    .IgnoreMember(t=> t.Conta)
                    .IgnoreMember(t => t.Codigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
                
                cfg.CreateMap<Portador, DTOS.Response.PortadorCobranca.PortadorCobrancaResponse>()
                    .ForMember(dest => dest.EmiteCobranca, opt => opt.MapFrom(src => src.EmiteCobranca == "S"));

                // Proposta Importação Simples
                cfg.CreateMap<CondicaoPagamento, DTOS.Response.Proposta.PropostaImportacaoSimplesResponse.CondicaoPagamentoDTO>();
                cfg.CreateMap<Obra, DTOS.Response.Proposta.PropostaImportacaoSimplesResponse.ObraDTO>()
                    .MapEnderecoToDto();
                cfg.CreateMap<CadastroGeral, DTOS.Response.Proposta.PropostaImportacaoSimplesResponse.TipoCobrancaDTO>();
                cfg.CreateMap<Usina, DTOS.Response.Proposta.PropostaImportacaoSimplesResponse.UsinaDTO>();
                cfg.CreateMap<ObraTributacaoMunicipal, DTOS.Response.Proposta.PropostaImportacaoSimplesResponse.ObraTributacaoMunicipalDTO>();
                cfg.CreateMap<Proposta, DTOS.Response.Proposta.PropostaImportacaoSimplesResponse.PropostaImportacaoSimplesResponse>();

                //  Proposta Inserida
                cfg.CreateMap<Interveniente, DTOS.Response.Proposta.PropostaInseridaResponse.PropostaInseridaResponse>();

                // Proposta Simples
                cfg.CreateMap<Interveniente, DTOS.Response.Proposta.PropostaSimplesResponse.IntervenienteDTO>();
                cfg.CreateMap<Obra, DTOS.Response.Proposta.PropostaSimplesResponse.ObraDTO>();
                cfg.CreateMap<TipoCobranca, DTOS.Response.Proposta.PropostaSimplesResponse.TipoCobrancaDTO>();
                cfg.CreateMap<Usina, DTOS.Response.Proposta.PropostaSimplesResponse.UsinaDTO>();
                cfg.CreateMap<Vendedor, DTOS.Response.Proposta.PropostaSimplesResponse.VendedorDTO>();
                cfg.CreateMap<Contrato, DTOS.Response.Proposta.PropostaSimplesResponse.ContratoDTO>();
                cfg.CreateMap<Proposta, DTOS.Response.Proposta.PropostaSimplesResponse.PropostaSimplesResponse>();

                // Proposta Detalhada
                cfg.CreateMap<CadastroGeral, DTOS.Response.Proposta.PropostaDetalhadaResponse.CadastroGeralDTO>();
                cfg.CreateMap<CartaoBandeira, DTOS.Response.Proposta.PropostaDetalhadaResponse.CartaoBandeiraDTO>();
                cfg.CreateMap<TipoCobranca, DTOS.Response.Proposta.PropostaDetalhadaResponse.TipoCobrancaDTO>();
                cfg.CreateMap<CondicaoPagamento, DTOS.Response.Proposta.PropostaDetalhadaResponse.CondicaoPagamentoDTO>();
                cfg.CreateMap<Conta, DTOS.Response.Proposta.PropostaDetalhadaResponse.ContaDTO>();
                cfg.CreateMap<Interveniente, DTOS.Response.Proposta.PropostaDetalhadaResponse.IntervenienteDTO>()
                    .MapEnderecoToDto();
                cfg.CreateMap<Obra, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraDTO>()
                    .MapEnderecoToDto();
                cfg.CreateMap<ObraIndicador, DTOS.Response.Obra.ObraIndicadorResponse.ObraIndicadorDTO>();
                cfg.CreateMap<CadastroGeralViaCaptacao, DTOS.Response.Proposta.PropostaDetalhadaResponse.CadastroGeralViaCaptacaoDTO>();
                cfg.CreateMap<ObraBomba, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraBombaDTO>();
                cfg.CreateMap<ObraTraco, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraTracoDTO>()
                    .MapMemberFrom(dest => dest.Mpa, src => src.Fck);
                cfg.CreateMap<ObraTaxa, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraTaxaDTO>();
                cfg.CreateMap<ObraLog, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraLogDTO>();
                cfg.CreateMap<ObraTributacaoMunicipal, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraTributacaoMunicipalDTO>();
                cfg.CreateMap<ObraDemaisServicos, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraDemaisServicosDTO>();
                cfg.CreateMap<ObraPagamento, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDTO>();
                cfg.CreateMap<ContratoPagamento, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDTO>()
                    .IncludeBase<ObraPagamento, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDTO>();
                cfg.CreateMap<PropostaPagamento, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDTO>()
                    .IncludeBase<ObraPagamento, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDTO>();
                cfg.CreateMap<ObraPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>()
                    .IncludeBase<ObraPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<PropostaPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>()
                    .IncludeBase<ObraPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheCartao, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheCartaoDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheDeposito, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDepositoDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheBoleto, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheBoletoDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheCheque, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheChequeDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheDinheiro, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDinheiroDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<PropostaPagamentoDetalheCartao, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheCartaoDTO>()
                    .IncludeBase<PropostaPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<PropostaPagamentoDetalheDeposito, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDepositoDTO>()
                    .IncludeBase<PropostaPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<PropostaPagamentoDetalheBoleto, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheBoletoDTO>()
                    .IncludeBase<PropostaPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<PropostaPagamentoDetalheCheque, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheChequeDTO>()
                    .IncludeBase<PropostaPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<PropostaPagamentoDetalheDinheiro, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDinheiroDTO>()
                    .IncludeBase<PropostaPagamentoDetalhe, DTOS.Response.Proposta.PropostaDetalhadaResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<Pedra, DTOS.Response.Proposta.PropostaDetalhadaResponse.PedraDTO>();
                cfg.CreateMap<Portador, DTOS.Response.Proposta.PropostaDetalhadaResponse.PortadorDTO>();
                cfg.CreateMap<Slump, DTOS.Response.Proposta.PropostaDetalhadaResponse.SlumpDTO>();
                cfg.CreateMap<SlumpReal, DTOS.Response.Proposta.PropostaDetalhadaResponse.SlumpDTO>();
                cfg.CreateMap<Usina, DTOS.Response.Proposta.PropostaDetalhadaResponse.UsinaDTO>();
                cfg.CreateMap<Uso, DTOS.Response.Proposta.PropostaDetalhadaResponse.UsoDTO>();
                cfg.CreateMap<Vendedor, DTOS.Response.Proposta.PropostaDetalhadaResponse.VendedorDTO>();
                cfg.CreateMap<Proposta, DTOS.Response.Proposta.PropostaDetalhadaResponse.PropostaDetalhadaResponse>()
                    .MapEnderecoToDto();

                cfg.CreateMap<Lead, LeadResponse>().MapEnderecoToDto();

                cfg.CreateMap<LeadInclusaoRequest, Lead>()
                    .IgnoreMember(t => t.Logs);

                // ObraSimplesResponse
                cfg.CreateMap<Obra, DTOS.Response.Obra.ObraSimplesResponse.ObraSimplesResponse>()
                    .ReverseMap();

                // Titulos Contas a Receber
                cfg.CreateMap<TituloContasAReceber, DTOS.Response.TituloContasAReceber.TituloContasAReceberResponse>()
                    .ReverseMap();
                cfg.CreateMap<TituloContasAReceber, TituloContasAReceber>();

                // Titulos Contas a Receber Response Integração Publica 

                cfg.CreateMap<TituloContasAReceber, DTOS.Response.TituloContasAReceber.PublicoTituloContasAReceberResponse>()
                    .ForMember(dest => dest.DocumentoSequencia, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DocumentoSequencia) ? default(int) : long.Parse(src.DocumentoSequencia)))
                    .ForPath(dest => dest.TipoDeCobranca.Codigo, opt => opt.MapFrom(src => src.MeioPagamento))
                    .ForMember(dest => dest.Segmentacao, opt => opt.MapFrom(src => new SegmentacaoDTO()))
                    .ForMember(dest => dest.TotalizaCbsIbsIs, opt => opt.MapFrom(src => src.TotalizaCbsIbsIs == "S" ));
                cfg.CreateMap<ReguaDeCobrancaTituloContasAReceber, DTOS.Response.TituloContasAReceber.PublicoReguaDeCobrancaTituloContasAReceberResponse>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.ContaNumero, opt => opt.MapFrom(src => src.Organizacao.Codigo))
                    .ForMember(dest => dest.ContaNome, opt => opt.MapFrom(src => src.Organizacao.Nome))
                    .ForMember(dest => dest.ChaveNfe, opt => opt.MapFrom(src => src.TituloContasAReceber.NumeroNFSE.ToString()))
                    .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.Interveniente.CpfCnpj))
                    .ForMember(dest => dest.OrganizacaoCodigo, opt => opt.MapFrom(src => src.TituloContasAReceber.IntervenienteCodigo.ToString()))
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Interveniente.Nome))
                    .ForMember(dest => dest.ClienteTipo, opt => opt.MapFrom(src => src.ClienteTipo))
                    .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.TituloContasAReceber.Observacao))
                    .ForMember(dest => dest.Vencimento, opt => opt.MapFrom(src => src.TituloContasAReceber.DataVencimento))
                    .ForMember(dest => dest.Emissao, opt => opt.MapFrom(src => src.TituloContasAReceber.DataEmissao))
                    .ForMember(dest => dest.Pagamento, opt => opt.MapFrom(src => src.TituloContasAReceber.DataLiquidacao ?? null))
                    .ForMember(dest => dest.PagamentoMetodo, opt => opt.MapFrom(src => src.PagamentoMetodo))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.PagamentoValor, opt => opt.MapFrom(src => src.TituloContasAReceber.SomaRecebimentosToDouble))
                    .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.TituloContasAReceber.SaldoToDouble))
                    .ForMember(dest => dest.ClienteEmail, opt => opt.MapFrom(src => src.Interveniente.EmailCobranca))
                    .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Interveniente.Nome))
                    .ForMember(dest => dest.ClienteNomeFantasia, opt => opt.MapFrom(src => src.Interveniente.Nome))
                    .ForMember(dest => dest.ClienteRazaoSocial, opt => opt.MapFrom(src => src.Interveniente.Razao))
                    .ForMember(dest => dest.ClienteTelefone, opt => opt.MapFrom(src => src.ClienteTelefone))
                    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.TituloContasAReceber.ValorToDouble))
                    .ForMember(dest => dest.NumeroBoleto, opt => opt.MapFrom(src => src.TituloContasAReceber.LinhaDigitavelBoleto))
                    .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.TituloContasAReceber.LinhaDigitavelBoleto))
                    .ForMember(dest => dest.LinkSegundaViaBoleto, opt => opt.MapFrom(src => src.LinkSegundaViaBoleto));

                // Titulos Contas a Receber Request Integração Publica 
                cfg.CreateMap<DTOS.Request.TituloContasAReceber.TituloContasAReceberAdicionarRequest, TituloContasAReceber>()
                    .ForMember(dest => dest.DocumentoSequencia, opt => opt.MapFrom(src => src.DocumentoSequencia.HasValue ? src.DocumentoSequencia.ToString() : ""));

                cfg.CreateMap<DTOS.Request.TituloContasAReceber.TituloContasAReceberAtualizarRequest, TituloContasAReceber>()
                    .ForMember(dest => dest.DocumentoSequencia, opt => opt.MapFrom(src => src.DocumentoSequencia.HasValue ? src.DocumentoSequencia.ToString() : ""))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
                cfg.CreateMap<TituloContasAReceber, DTOS.Request.TituloContasAReceber.TituloContasAReceberAtualizarRequest>()
                    .ForMember(dest => dest.DocumentoSequencia, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DocumentoSequencia) ? default(int) : long.Parse(src.DocumentoSequencia)));

                // Programação Hora
                cfg.CreateMap<ProgramacaoHora, DTOS.Response.Programacao.ProgramacaoHoraResponse.ProgramacaoHoraResponse>()
                    .ReverseMap();

                //Demais Serviços
                cfg.CreateMap<DTOS.Response.DemaisServicos.DemaisServicosResponse, DemaisServicos>()
                    .IgnoreMember(t => t.FormaDeCobrancaString)
                    .IgnoreMember(t => t.FrequenciaDeCobrancaString)
                    .IgnoreMember(t => t.MercadoriaCodigo)
                    .IgnoreMember(t => t.UsinaCodigo)
                    .IgnoreMember(t => t.UnidadeSigla)
                    .ReverseMap();

                //Unidades
                cfg.CreateMap<DTOS.Response.Unidade.UnidadeResponse, Unidade>()
                    .ReverseMap();

                //Mercadoria
                cfg.CreateMap<DTOS.Response.Mercadoria.MercadoriaResponse, Mercadoria>()
                    .ReverseMap();

                //Obra Pagamentos
                cfg.CreateMap<CartaoBandeira, DTOS.Response.Obra.ObraPagamentosResponse.CartaoBandeiraDTO>();
                cfg.CreateMap<CondicaoPagamento, DTOS.Response.Obra.ObraPagamentosResponse.CondicaoPagamentoDTO>();
                cfg.CreateMap<Conta, DTOS.Response.Obra.ObraPagamentosResponse.ContaDTO>();
                cfg.CreateMap<Interveniente, DTOS.Response.Obra.ObraPagamentosResponse.IntervenienteDTO>();
                cfg.CreateMap<ObraPagamento, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDTO>();
                cfg.CreateMap<ContratoPagamento, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDTO>()
                    .IncludeBase<ObraPagamento, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDTO>();
                cfg.CreateMap<ObraPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>()
                    .IncludeBase<ObraPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheCartao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheCartaoDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheDeposito, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDepositoDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheBoleto, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheBoletoDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheCheque, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheChequeDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheDinheiro, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDinheiroDTO>()
                    .IncludeBase<ContratoPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<Portador, DTOS.Response.Obra.ObraPagamentosResponse.PortadorDTO>();
                cfg.CreateMap<TipoCobranca, DTOS.Response.Obra.ObraPagamentosResponse.TipoCobrancaDTO>();
                cfg.CreateMap<Obra, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentosResponse>();

                cfg.CreateMap<ObraVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentosResponse>();

                cfg.CreateMap<ContratoPagamentoVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDTO>()
                    .IncludeBase<ObraPagamento, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDTO>();

                cfg.CreateMap<ContratoPagamentoDetalheVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>()
                    .IncludeBase<ObraPagamentoDetalhe, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheCartaoVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheCartaoDTO>()
                    .IncludeBase<ContratoPagamentoDetalheVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheDepositoVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDepositoDTO>()
                    .IncludeBase<ContratoPagamentoDetalheVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheBoletoVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheBoletoDTO>()
                    .IncludeBase<ContratoPagamentoDetalheVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheChequeVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheChequeDTO>()
                    .IncludeBase<ContratoPagamentoDetalheVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();
                cfg.CreateMap<ContratoPagamentoDetalheDinheiroVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDinheiroDTO>()
                    .IncludeBase<ContratoPagamentoDetalheVersao, DTOS.Response.Obra.ObraPagamentosResponse.ObraPagamentoDetalheDTO>();

                // Movimentos de Banco não vinculados com CAR
                cfg.CreateMap<MovimentoBanco, DTOS.Response.MovimentoBanco.MovimentoBancoNaoVinvuladoComContasAReceberResponse>();

                // Custo Servico
                cfg.CreateMap<CustoServico, DTOS.Response.CustoServico.CustoServicoResponse>();
                cfg.CreateMap<DTOS.Request.CustoServico.Inclusao.CustoServicoInclusaoRequest, CustoServico>();
                cfg.CreateMap<DTOS.Request.CustoServico.Alteracao.CustoServicoAlteracaoRequest, CustoServico>();

                // Assinatura Eletronica
                cfg.CreateMap<DTOS.Response.AssinaturaEletronica.Clicksign.ClicksignParametrosResponse, ClicksignParametros>();
                cfg.CreateMap<ContratoClicksignEnvio, DTOS.Response.AssinaturaEletronica.Clicksign.ContratoClicksignEnvioDTO>();

                // Opportunities
                cfg.CreateMap<DTOS.Request.Opportunity.Adicionar.OpportunityTypeAdicionarRequest, OpportunityType>()
                    .IgnoreMember(o => o.Id);

                cfg.CreateMap<DTOS.Request.Opportunity.Adicionar.OpportunityOriginAdicionarRequest, OpportunityOrigin>()
                    .IgnoreMember(o => o.Id);

                cfg.CreateMap<DTOS.Request.Opportunity.Adicionar.OpportunityFailureReasonAdicionarRequest, OpportunityFailureReason>()
                    .IgnoreMember(o => o.Id);

                cfg.CreateMap<DTOS.Request.Opportunity.Adicionar.OpportunityAdicionarRequest, Opportunity>()
                    .IgnoreMember(o => o.Id)
                    .IgnoreMember(o => o.OpportunityType)
                    .IgnoreMember(o => o.OpportunityFailureReason)
                    .IgnoreMember(o => o.OpportunityOrigin)
                    .IgnoreMember(o => o.CreatedAt)
                    .IgnoreMember(o => o.UpdatedAt)
                    .IgnoreMember(o => o.DeletedAt)
                    .IgnoreMember(o => o.Deleted)
                    .IgnoreMember(o => o.ConcreteBatchingPlant)
                    .IgnoreMember(o => o.AddressCity);

                cfg.CreateMap<DTOS.Request.Opportunity.Alteracao.OpportunityAlteracaoRequest, Opportunity>()
                    .IgnoreMember(o => o.OpportunityType)
                    .IgnoreMember(o => o.OpportunityFailureReason)
                    .IgnoreMember(o => o.OpportunityOrigin)
                    .IgnoreMember(o => o.CreatedAt)
                    .IgnoreMember(o => o.UpdatedAt)
                    .IgnoreMember(o => o.DeletedAt)
                    .IgnoreMember(o => o.Deleted)
                    .IgnoreMember(o => o.ConcreteBatchingPlant)
                    .IgnoreMember(o => o.AddressCity);

                cfg.CreateMap<DTOS.Request.AprovacaoComercial.AprovacaoComercialUsinaAlteracaoRequest, AprovacaoComercialUsina>()
                    .IgnoreMember(o => o.Usina)
                    .IgnoreMember(o => o.CreatedAt)
                    .IgnoreMember(o => o.UpdatedAt)
                    .IgnoreMember(o => o.Hierarquias);

                cfg.CreateMap<DTOS.Request.AprovacaoComercial.AprovacaoComercialUsinaInsercaoRequest, AprovacaoComercialUsina>()
                    .IgnoreMember(o => o.Id)
                    .IgnoreMember(o => o.Usina)
                    .IgnoreMember(o => o.CreatedAt)
                    .IgnoreMember(o => o.UpdatedAt)
                    .IgnoreMember(o => o.Hierarquias);

                cfg.CreateMap<DTOS.Request.AprovacaoComercial.AprovacaoComercialHierarquiaInsercaoRequest, AprovacaoComercialHierarquia>()
                    .IgnoreMember(x => x.Id)
                    .IgnoreMember(x => x.CreatedAt)
                    .IgnoreMember(x => x.UpdatedAt)
                    .IgnoreMember(x => x.Condicoes);

                cfg.CreateMap<DTOS.Request.AprovacaoComercial.AprovacaoComercialHierarquiaAlteracaoRequest, AprovacaoComercialHierarquia>()
                    .IgnoreMember(x => x.CreatedAt)
                    .IgnoreMember(x => x.UpdatedAt)
                    .IgnoreMember(x => x.Condicoes);

                cfg.CreateMap<DTOS.Request.AprovacaoComercial.AprovacaoComercialHierarquiaUsuarioInsercaoRequest, AprovacaoComercialHierarquiaUsuario>()
                    .IgnoreMember(x => x.Id);

                cfg.CreateMap<DTOS.Request.AprovacaoComercial.AprovacaoComercialHierarquiaCondicaoInsercaoRequest, AprovacaoComercialHierarquiaCondicao>()
                    .IgnoreMember(x => x.Id)
                    .IgnoreMember(x => x.TipoPessoa)
                    .IgnoreMember(x => x.AprovacaoComercialHierarquia);

                //Proposta Versão
                cfg.CreateMap<PropostaVersao, Proposta>();
                cfg.CreateMap<ContratoVersao, Contrato>();
                cfg.CreateMap<PropostaFaturamentoVersao, PropostaFaturamento>();
                cfg.CreateMap<ObraVersao, Obra>();
                cfg.CreateMap<PropostaCobrancaVersao, PropostaCobranca>();
                cfg.CreateMap<PropostaResponsavelSolidarioVersao, PropostaResponsavelSolidario>();
                cfg.CreateMap<DTOS.Generic.PropostaResponsavelSolidarioDTO, PropostaResponsavelSolidarioVersao>()
                    .IgnoreMember(x => x.NumeroVersao)
                    .IgnoreMember(x => x.Proposta)
                    .IgnoreMember(x => x.EnderecoMunicipio);
                cfg.CreateMap<PropostaPagamentoVersao, PropostaPagamento>()
                    .IgnoreMember(x => x.Detalhes);
                cfg.CreateMap<ContratoPagamentoForSavingVersao, ContratoPagamentoForSaving>();
                cfg.CreateMap<ObraTracoVersao, ObraTraco>();
                cfg.CreateMap<ContratoTracoReajusteVersao, ContratoTracoReajuste>();
                cfg.CreateMap<ObraBombaVersao, ObraBomba>();
                cfg.CreateMap<ObraDemaisServicosVersao, ObraDemaisServicos>();
                cfg.CreateMap<ObraReajusteVersao, ObraReajuste>();
                cfg.CreateMap<ProgramacaoDemaisServicosVersao, ProgramacaoDemaisServicos>();
                cfg.CreateMap<ObraTaxaVersao, ObraTaxa>();
                cfg.CreateMap<ContratoPagamentoDetalheCartaoVersao, ContratoPagamentoDetalheCartao>();
                cfg.CreateMap<ContratoPagamentoDetalheDepositoVersao, ContratoPagamentoDetalheDeposito>();
                cfg.CreateMap<ContratoPagamentoDetalheDinheiroVersao, ContratoPagamentoDetalheDinheiro>();
                cfg.CreateMap<ContratoPagamentoDetalheBoletoVersao, ContratoPagamentoDetalheBoleto>();
                cfg.CreateMap<ContratoPagamentoDetalheChequeVersao, ContratoPagamentoDetalheCheque>();
                cfg.CreateMap<ContratoPagamentoVersao, ContratoPagamento>()
                    .IgnoreMember(x => x.Detalhes);
                cfg.CreateMap<ContratoBombaReajusteVersao, ContratoBombaReajuste>();
                cfg.CreateMap<PropostaPagamentoDetalheDepositoVersao, PropostaPagamentoDetalheDeposito>();
                cfg.CreateMap<PropostaPagamentoDetalheCartaoVersao, PropostaPagamentoDetalheCartao>();
                cfg.CreateMap<PropostaPagamentoDetalheBoletoVersao, PropostaPagamentoDetalheBoleto>();
                cfg.CreateMap<PropostaPagamentoDetalheChequeVersao, PropostaPagamentoDetalheCheque>();
                cfg.CreateMap<PropostaPagamentoDetalheDinheiroVersao, PropostaPagamentoDetalheDinheiro>();
                cfg.CreateMap<ObraLogVersao, ObraLog>();
                cfg.CreateMap<ObraMensagemPadraoVersao, ObraMensagemPadrao>();
                cfg.CreateMap<ObraTributacaoMunicipalVersao, ObraTributacaoMunicipal>();
                cfg.CreateMap<ContratoPagamentoDetalheVersao, ContratoPagamentoDetalhe>();
                cfg.CreateMap<PropostaPagamentoDetalheVersao, PropostaPagamentoDetalhe>();
                cfg.CreateMap<TaxaExtraVersao, TaxaExtra>();
                cfg.CreateMap<ContratoVersao, DTOS.Response.Contrato.ContratoVersaoResponse>();

                cfg.CreateMap<ContratoTracoReajuste, ObraTracoReajusteResponse>()
                    .IgnoreMember(t => t.DescricaoPersonalizada);

                cfg.CreateMap<ContratoBombaReajuste, ObraBombaReajusteResponse>();

                //Grupo Economico
                cfg.CreateMap<GrupoEconomico, DTOS.Response.GrupoEconomico.GrupoEconomicoResponse>();
                cfg.CreateMap<Interveniente, DTOS.Response.GrupoEconomico.ClienteDTO>();
                cfg.CreateMap<DTOS.Request.GrupoEconomico.Inclusao.GrupoEconomicoInclusaoRequest, GrupoEconomico>()
                    .IgnoreMember(x => x.Clientes)
                    .IgnoreMember(t => t.BloqueioMotivoCodigo);
                cfg.CreateMap<DTOS.Request.GrupoEconomico.Alteracao.GrupoEconomicoAlteracaoRequest, GrupoEconomico>()
                    .IgnoreMember(x => x.Clientes)
                    .IgnoreMember(t => t.BloqueioMotivoCodigo);

                //Liberacao Acesso
                cfg.CreateMap<GrupoAcesso, DTOS.Response.LiberacaoAcesso.GrupoAcessoResponse>();
                cfg.CreateMap<LiberacaoAcesso, DTOS.Response.LiberacaoAcesso.LiberacaoAcessoResponse>();
                cfg.CreateMap<PeriodoAusenciaUsuario, DTOS.Response.LiberacaoAcesso.PeriodoAusenciaUsuarioResponse>();
                cfg.CreateMap<LiberacaoAcesso, DTOS.Request.LiberacaoAcesso.LiberacaoAcessoDTO>();
                cfg.CreateMap<DTOS.Request.LiberacaoAcesso.LiberacaoAcessoInclusaoRequest, LiberacaoAcesso>();
                cfg.CreateMap<DTOS.Request.LiberacaoAcesso.LiberacaoAcessoDTO, LiberacaoAcesso>();
                cfg.CreateMap<DTOS.Request.LiberacaoAcesso.PeriodoAusenciaUsuarioAlteracaoRequest, PeriodoAusenciaUsuario>();
                cfg.CreateMap<DTOS.Request.LiberacaoAcesso.GrupoAcessoInclusaoRequest, GrupoAcesso>();
                cfg.CreateMap<DTOS.Request.LiberacaoAcesso.GrupoAcessoAlteracaoRequest, GrupoAcesso>()
                    .IgnoreMember(x => x.LiberacoesAcessos);

                //Visita
                cfg.CreateMap<VisitaTipo, DTOS.Response.Visita.VisitaTipoResponse>();
                cfg.CreateMap<DTOS.Request.Visita.VisitaTipoInclusaoRequest, VisitaTipo>();
                cfg.CreateMap<DTOS.Request.Visita.VisitaTipoAlteracaoRequest, VisitaTipo>();
                cfg.CreateMap<DTOS.Request.Visita.VisitaHistoricoAdicionarRequest, VisitaHistorico>()
                    .IgnoreMember(x => x.Id)
                    .IgnoreMember(x => x.Tipo)
                    .IgnoreMember(x => x.IdCadastro);

                cfg.CreateMap<VisitaHistorico, DTOS.Response.Visita.VisitaHistoricoResponse>();

                //Oportunidade
                cfg.CreateMap<OportunidadeTipo, DTOS.Response.Oportunidade.OportunidadeTipoResponse>();
                cfg.CreateMap<DTOS.Request.Oportunidade.OportunidadeTipoInclusaoRequest, OportunidadeTipo>();
                cfg.CreateMap<DTOS.Request.Oportunidade.OportunidadeTipoAlteracaoRequest, OportunidadeTipo>();
                cfg.CreateMap<Concorrente, DTOS.Response.Oportunidade.ConcorrenteResponse>();
                cfg.CreateMap<DTOS.Request.Oportunidade.ConcorrenteInclusaoRequest, Concorrente>();
                cfg.CreateMap<DTOS.Request.Oportunidade.ConcorrenteAlteracaoRequest, Concorrente>();
                cfg.CreateMap<DTOS.Request.Oportunidade.OportunidadeAdicionarRequest, Oportunidade>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<DTOS.Request.Oportunidade.OportunidadeAtualizarRequest, Oportunidade>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<DTOS.Request.Oportunidade.OportunidadeContatoAdicionarRequest, OportunidadeContato>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<DTOS.Request.Oportunidade.OportunidadeContatoAtualizarRequest, OportunidadeContato>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<Oportunidade, DTOS.Response.Oportunidade.OportunidadeResponse>()
                    .MapEnderecoToDto();
                cfg.CreateMap<OportunidadeContato, DTOS.Response.Oportunidade.OportunidadeContatoDTO>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<OportunidadeLog, DTOS.Response.Oportunidade.OportunidadeLogDTO>()
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<Oportunidade, Proposta>()
                    .MapMemberFrom(dest => dest.AnoOportunidade, src => src.Ano)
                    .MapMemberFrom(dest => dest.NumeroOportunidade, src => src.Numero)
                    .MapMemberFrom(dest => dest.Segmentacao, src => src.SegmentacaoCodigo)
                    .MapMemberFrom(dest => dest.Segmento, src => src.Segmentacao)
                    .IgnoreMember(t => t.Numero)
                    .IgnoreMember(t => t.Ano)
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<Oportunidade, Obra>()
                    .MapMemberFrom(dest => dest.Nome, src => src.ObraNome)
                    .MapMemberFrom(dest => dest.VolumeEstimado, src => src.VolumeEstimadoObra)
                    .IgnoreMember(t => t.Numero)
                    .IgnoreMember(t => t.Concorrente)
                    .IgnoreMember(t => t.ConcorrenteCodigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

                //MotivoPerda
                cfg.CreateMap<MotivoPerda, DTOS.Response.MotivoPerda.MotivoPerdaResponse>();
                cfg.CreateMap<DTOS.Request.MotivoPerda.MotivoPerdaInclusaoRequest, MotivoPerda>();
                cfg.CreateMap<DTOS.Request.MotivoPerda.MotivoPerdaAlteracaoRequest, MotivoPerda>();

                //Tarefa
                cfg.CreateMap<Tarefa, DTOS.Response.Tarefa.TarefaResponse>();
                cfg.CreateMap<DTOS.Request.Tarefa.TarefaInclusaoRequest, Tarefa>();
                cfg.CreateMap<DTOS.Request.Tarefa.TarefaAlteracaoRequest, Tarefa>();


                //Compromisso
                cfg.CreateMap<Compromisso, DTOS.Response.Compromisso.CompromissoResponse>();
                cfg.CreateMap<DTOS.Request.Compromisso.CompromissoInclusaoRequest, Compromisso>();
                cfg.CreateMap<DTOS.Request.Compromisso.CompromissoAlteracaoRequest, Compromisso>();

                //Projecao Carteira
                cfg.CreateMap<DTOS.Request.Obra.ObraProjecao.ObraProjecaoRequest, ObraProjecao>();

                // Centro Custo
                cfg.CreateMap<DTOS.Request.CentroCusto.CentroCustoAdicionarRequest, CentroCusto>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == null ? null : src.Fixo == true ? "S" : "N"))
                    .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo == null ? null : src.Ativo == true ? "S" : "N"));

                cfg.CreateMap<DTOS.Request.CentroCusto.CentroCustoAtualizarRequest, CentroCusto>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == null ? null : src.Fixo == true ? "S" : "N"))
                    .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo == null ? null : src.Ativo == true ? "S" : "N"))
                    .IgnoreMember(x => x.Codigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<CentroCusto, DTOS.Response.CentroCusto.CentroCustoResponse>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == "S"))
                    .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo == "S"));

                // Conta - Banco
                cfg.CreateMap<DTOS.Request.Banco.BancoAdicionarRequest, Conta>()
                    .ForMember(dest => dest.DvAgencia, opt => opt.MapFrom(src => src.DvAgencia.HasValue ? src.DvAgencia.ToString() : ""))
                    .ForMember(dest => dest.DvConta, opt => opt.MapFrom(src => src.DvConta.HasValue ? src.DvConta.ToString() : ""))
                    .ForMember(dest => dest.DataUltimaConciliacao, opt => opt.Ignore());

                cfg.CreateMap<DTOS.Request.Banco.BancoAtualizarRequest, Conta>()
                    .ForMember(dest => dest.DvAgencia, opt => opt.MapFrom(src => src.DvAgencia.HasValue ? src.DvAgencia.ToString() : null))
                    .ForMember(dest => dest.DvConta, opt => opt.MapFrom(src => src.DvConta.HasValue ? src.DvConta.ToString() : null))
                    .IgnoreMember(x => x.DataUltimaConciliacao)
                    .IgnoreMember(x => x.Codigo)
                    .IgnoreMember(x => x.EmpresaCodigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<Conta, DTOS.Response.Banco.BancoResponse>()
                    .ForMember(dest => dest.DvAgencia, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DvAgencia) ? default(int) : int.Parse(src.DvAgencia)))
                    .ForMember(dest => dest.DvConta, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.DvConta) ? default(int) : int.Parse(src.DvConta)));

                // Situação Financeira
                cfg.CreateMap<DTOS.Request.SituacaoFinanceira.SituacaoFinanceiraAdicionarRequest, SituacaoFinanceira>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo ? "S" : "N"));

                cfg.CreateMap<DTOS.Request.SituacaoFinanceira.SituacaoFinanceiraAtualizarRequest, SituacaoFinanceira>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == null ? null : src.Fixo == true ? "S" : "N"))
                    .IgnoreMember(x => x.Codigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<SituacaoFinanceira, DTOS.Response.SituacaoFinanceira.SituacaoFinanceiraResponse>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == "S"));

                //Municipio Tributação
                cfg.CreateMap<DTOS.Request.MunicipioTributacao.MunicipioTributacaoAdicionarRequest, Municipio>()
                    .ForMember(dest => dest.IssRetido, opt => opt.MapFrom(src => src.IssRetido ? "S" : "N"))
                    .ForMember(dest => dest.TaxasTributadasIntegralmente, opt => opt.MapFrom(src => src.TaxasTributadasIntegralmente ? "S" : "N"))
                    .ForMember(dest => dest.TributacaoIntegralBomba, opt => opt.MapFrom(src => src.TributacaoIntegralBomba ? "S" : "N"))
                    .ForMember(dest => dest.TributacaoIntegralDemaisServicos, opt => opt.MapFrom(src => src.TributacaoIntegralDemaisServicos ? "S" : "N"));

                cfg.CreateMap<DTOS.Request.MunicipioTributacao.MunicipioTributacaoAtualizarRequest, Municipio>()
                    .ForMember(dest => dest.IssRetido, opt => opt.MapFrom(src => src.IssRetido == null ? null : src.IssRetido == true ? "S" : "N"))
                    .ForMember(dest => dest.TaxasTributadasIntegralmente, opt => opt.MapFrom(src => src.TaxasTributadasIntegralmente == null ? null : src.TaxasTributadasIntegralmente == true ? "S" : "N"))
                    .ForMember(dest => dest.TributacaoIntegralBomba, opt => opt.MapFrom(src => src.TributacaoIntegralBomba == null ? null : src.TributacaoIntegralBomba == true ? "S" : "N"))
                    .ForMember(dest => dest.TributacaoIntegralDemaisServicos, opt => opt.MapFrom(src => src.TributacaoIntegralDemaisServicos == null ? null : src.TributacaoIntegralDemaisServicos == true ? "S" : "N"))
                    .IgnoreMember(x => x.Codigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<Municipio, DTOS.Response.MunicipioTributacao.MunicipioTributacaoResponse>()
                    .ForMember(dest => dest.IssRetido, opt => opt.MapFrom(src => src.IssRetido == "S"))
                    .ForMember(dest => dest.TaxasTributadasIntegralmente, opt => opt.MapFrom(src => src.TaxasTributadasIntegralmente == "S"))
                    .ForMember(dest => dest.TributacaoIntegralBomba, opt => opt.MapFrom(src => src.TributacaoIntegralBomba == "S"))
                    .ForMember(dest => dest.TributacaoIntegralDemaisServicos, opt => opt.MapFrom(src => src.TributacaoIntegralDemaisServicos == "S"));

                // Funcionário
                cfg.CreateMap<FuncionarioAdicionarRequest, Funcionario>()
                    .ForMember(e => e.Ativo, opt => opt.MapFrom(src => src.Ativo ? "S" : "N"))
                    .ForMember(e => e.Comprador, opt => opt.MapFrom(src => src.Comprador ? "S" : "N"));

                cfg.CreateMap<FuncionarioAtualizarRequest, Funcionario>()
                    .ForMember(e => e.Ativo, opt => opt.MapFrom(src => src.Ativo == true ? "S" : "N"))
                    .ForMember(e => e.Comprador, opt => opt.MapFrom(src => src.Comprador == true ? "S" : "N"))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<Funcionario, FuncionarioResponse>()
                    .ForMember(e => e.Ativo, opt => opt.MapFrom(src => src.Ativo == "S"))
                    .ForMember(e => e.Comprador, opt => opt.MapFrom(src => src.Comprador == "S"))
                    .ForMember(e => e.CPF, opt => opt.MapFrom(src => src.Interveniente == null ? "" : src.Interveniente.CpfCnpj));

                // Programacao
                cfg.CreateMap<ProgramacaoAdicionarRequest, Programacao>()
                    .ForMember(e => e.Horario, opt => opt.MapFrom(src => src.Horario.Replace(":", "")))
                    .ForMember(e => e.HorarioBomba, opt => opt.MapFrom(src => src.HorarioBomba.Replace(":", "")));

                cfg.CreateMap<ProgramacaoAtualizarRequest, Programacao>()
                    .ForMember(e => e.Horario, opt => opt.MapFrom(src => src.Horario.Replace(":", "")))
                    .ForMember(e => e.HorarioBomba, opt => opt.MapFrom(src => src.HorarioBomba.Replace(":", "")))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<Programacao, ProgramacaoResponse>()
                    .IgnoreMember(e => e.NumeroContratoAnterior)
                    .ForMember(e => e.Horario, opt => opt.MapFrom(src => (src.Horario.Length != 4) ? src.Horario : src.Horario.Substring(0, 2) + ":" + src.Horario.Substring(2, 2)))
                    .ForMember(e => e.HorarioBomba, opt => opt.MapFrom(src => (src.HorarioBomba.Length != 4) ? src.HorarioBomba : src.HorarioBomba.Substring(0, 2) + ":" + src.HorarioBomba.Substring(2, 2)));

                // Visita
                cfg.CreateMap<VisitaAdicionarRequest, Visita>()
                    .IgnoreMember(t => t.Usina)
                    .IgnoreMember(t => t.UsinaCodigo)
                    .IgnoreMember(t => t.Numero)
                    .IgnoreMember(t => t.Ano)
                    .IgnoreMember(t => t.TipoVisita)
                    .IgnoreMember(t => t.Vendedor);

                cfg.CreateMap<VisitaContatoAdicionarRequest, VisitaContato>()
                    .IgnoreMember(t => t.Usina)
                    .IgnoreMember(t => t.NumeroVisita)
                    .IgnoreMember(t => t.AnoVisita)
                    .IgnoreMember(t => t.Funcao)
                    .IgnoreMember(t => t.Sequencia);

                cfg.CreateMap<VisitaAtualizarRequest, Visita>()
                    .IgnoreMember(t => t.Usina)
                    .IgnoreMember(t => t.TipoVisita)
                    .IgnoreMember(t => t.Vendedor);

                cfg.CreateMap<VisitaContatoAtualizarRequest, VisitaContato>()
                    .IgnoreMember(t => t.Funcao);

                cfg.CreateMap<VisitaAnexoAdicionarRequest, VisitaAnexo>()
                    .IgnoreMember(t => t.Id)
                    .IgnoreMember(t => t.Descricao)
                    .IgnoreMember(t => t.Usuario)
                    .IgnoreMember(t => t.DataHora);

                cfg.CreateMap<VisitaAnexoAtualizarRequest, VisitaAnexo>()
                    .IgnoreMember(t => t.Arquivo)
                    .IgnoreMember(t => t.Usuario);

                cfg.CreateMap<VisitaLog, VisitaLogDTO>();
                cfg.CreateMap<Visita, VisitaResponse>()
                .MapEnderecoToDto();


                //Remessa
                cfg.CreateMap<NotaFiscalFisica, RemessaResponse>()
                .ForMember(e => e.ComissaoAjudanteBomba, opt => opt.MapFrom(src => src.ComissaoAjudanteBomba == "S"))
                .ForMember(e => e.ComissaoBombista, opt => opt.MapFrom(src => src.ComissaoBombista == "S"))
                .ForMember(e => e.ComissaoMotorista, opt => opt.MapFrom(src => src.ComissaoMotorista == "S"))
                .ForMember(e => e.ComissaoRepresentante, opt => opt.MapFrom(src => src.ComissaoRepresentante == "S"))
                .ForMember(e => e.ComissaoVendedor, opt => opt.MapFrom(src => src.ComissaoVendedor == "S"))
                .ForMember(e => e.Desvio, opt => opt.MapFrom(src => src.Desvio == "S"))
                .ForMember(e => e.MaoDeObraPropria, opt => opt.MapFrom(src => src.MaoDeObraPropria == "S"))
                .ForMember(e => e.ResponsavelCliente, opt => opt.MapFrom(src => src.ResponsavelCliente == "S"))
                .ForMember(e => e.ValorUnitarioAdicionalFeriado, opt => opt.MapFrom(src => src.Complemento.ValorUnitarioAdicionalFeriado))
                .ForMember(e => e.ValorUnitarioAdicionalHoraExtra, opt => opt.MapFrom(src => src.Complemento.ValorUnitarioAdicionalHoraExtra))
                .ForMember(e => e.ValorAdicionalZmrc, opt => opt.MapFrom(src => src.Complemento.ValorAdicionalZmrc))
                .ForMember(e => e.AdicionalTubulacaoExtra, opt => opt.MapFrom(src => src.Complemento.AdicionalTubulacaoExtra))
                .ForMember(e => e.ConfirmaMoldagemRemota, opt => opt.MapFrom(src => src.Complemento.ConfirmaMoldagemRemota))
                .ForMember(e => e.EquipamentoTransporteMateriaPrimaCodigo, opt => opt.MapFrom(src => src.Complemento.EquipamentoTransporteMateriaPrimaCodigo))
                .ForMember(e => e.HoraBombeamentoFim, opt => opt.MapFrom(src => src.Complemento.HoraBombeamentoFim))
                .ForMember(e => e.HoraBombeamentoInicio, opt => opt.MapFrom(src => src.Complemento.HoraBombeamentoInicio))
                .ForMember(e => e.HoraBombaPronta, opt => opt.MapFrom(src => src.Complemento.HoraBombaPronta))
                .ForMember(e => e.HoraTrabalhadaEfetivamente, opt => opt.MapFrom(src => src.Complemento.HoraTrabalhadaEfetivamente))
                .ForMember(e => e.HoraTrabalhada, opt => opt.MapFrom(src => src.Complemento.HoraTrabalhada))
                .ForMember(e => e.IdUsuarioMoldagemRemota, opt => opt.MapFrom(src => src.Complemento.IdUsuarioMoldagemRemota))
                .ForMember(e => e.JustificativaOrdemBt, opt => opt.MapFrom(src => src.Complemento.JustificativaOrdemBt))
                .ForMember(e => e.LoteEmissao, opt => opt.MapFrom(src => src.Complemento.LoteEmissao))
                .ForMember(e => e.MotivoMudancaTaxaPermanencia, opt => opt.MapFrom(src => src.Complemento.MotivoMudancaTaxaPermanencia))
                .ForMember(e => e.ObservacaoMoldagemRemota, opt => opt.MapFrom(src => src.Complemento.ObservacaoMoldagemRemota))
                .ForMember(e => e.OrdemBt, opt => opt.MapFrom(src => src.Complemento.OrdemBt))
                .ForMember(e => e.PercentualAdicionalZmrc, opt => opt.MapFrom(src => src.Complemento.PercentualAdicionalZmrc))
                .ForMember(e => e.QuantidadeAdicionalKmRodado, opt => opt.MapFrom(src => src.Complemento.QuantidadeAdicionalKmRodado))
                .ForMember(e => e.QuantidadeAdicionalRetornoConcreto, opt => opt.MapFrom(src => src.Complemento.QuantidadeAdicionalRetornoConcreto))
                .ForMember(e => e.QuantidadeTaxaPermanencia, opt => opt.MapFrom(src => src.Complemento.QuantidadeTaxaPermanencia))
                .ForMember(e => e.QuantidadeAdicionalFeriado, opt => opt.MapFrom(src => src.Complemento.QuantidadeAdicionalFeriado))
                .ForMember(e => e.QuantidadeAdicionalHoraExtra, opt => opt.MapFrom(src => src.Complemento.QuantidadeAdicionalHoraExtra))
                .ForMember(e => e.ReaproveitamentoProgramacao, opt => opt.MapFrom(src => src.Complemento.ReaproveitamentoProgramacao))
                .ForMember(e => e.QuantidadeTaxaPermanenciaBomba, opt => opt.MapFrom(src => src.Complemento.QuantidadeTaxaPermanenciaBomba))
                .ForMember(e => e.ValorUnitarioTaxaPermanenciaBomba, opt => opt.MapFrom(src => src.Complemento.ValorUnitarioTaxaPermanenciaBomba))
                .ForMember(e => e.ValorTaxaPermanencia, opt => opt.MapFrom(src => src.Complemento.TaxaPermanenciaValor))
                .ForMember(e => e.ValorTaxaPermanenciaBomba, opt => opt.MapFrom(src => src.Complemento.TaxaPermanenciaBombaValor))
                .ForMember(e => e.VersaoContrato, opt => opt.MapFrom(src => src.Complemento.VersaoContrato))
                .ForMember(e => e.ValorUnitarioAdicionalKmRodado, opt => opt.MapFrom(src => src.Complemento.ValorUnitarioAdicionalKmRodado))
                .ForMember(e => e.ValorUnitarioAdicionalRetornoConcreto, opt => opt.MapFrom(src => src.Complemento.ValorUnitarioAdicionalRetornoConcreto))
                .ForMember(e => e.ValorAdicionais, opt => opt.MapFrom(src => src.Complemento.ValorAdicionais))
                .ForMember(e => e.ValorDemaisServicos, opt => opt.MapFrom(src => src.Complemento.ValorDemaisServicos))
                .ForMember(e => e.ValorHoraTrabalhada, opt => opt.MapFrom(src => src.Complemento.ValorHoraTrabalhada))
                .ForMember(e => e.ValorAdicionalKmRodado, opt => opt.MapFrom(src => src.Complemento.AdicionalKmValorTotal))
                .ForMember(e => e.ValorAdicionalRetornoConcreto, opt => opt.MapFrom(src => src.Complemento.AdicionalRetornoConcretoTotal))
                .ForMember(e => e.ValorTotalCobranca, opt => opt.MapFrom(src => Math.Round(src.Complemento.ValorTotalCobranca, 2)))
                .ForMember(e => e.ValorVendaHoraBomba, opt => opt.MapFrom(src => src.Complemento.ValorVendaHoraBomba))
                .ForMember(e => e.ValorTotalHoraBomba, opt => opt.MapFrom(src => src.Complemento.ValorTotalHoraBomba))
                .ForMember(e => e.NumeracaoProduto, opt => opt.MapFrom(src => src.Complemento.NumeracaoProduto));
                cfg.CreateMap<NotaFiscalFisicaItem, RemessaItemDto>();
                cfg.CreateMap<Reaproveitamento, RemessaReaproveitamentoDto>();
                cfg.CreateMap<NotaFiscalFisicaDemaisServicos, RemessaDemaisServicosDto>();

                //Nota Fiscal Digital
                cfg.CreateMap<NotaFiscalDigital, NotaFiscalDigitalResponse>()
                    .ForMember(dest => dest.Cancelada, opt => opt.MapFrom(src => src.Cancelada == "S"))
                    .ForMember(dest => dest.ClienteContrato, opt => opt.MapFrom(src => src.Cliente))
                    .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src =>
                        src.IndicadorOperacao == EIndicadorOperacao.Saida && src.DetalhesFiscais != null
                            ? src.DetalhesFiscais.Cliente
                            : src.Cliente));

                cfg.CreateMap<NotaFiscalDigitalItem, NotaFiscalDigitalItemResponse>();
                cfg.CreateMap<NotaFiscalDigitalItemComplemento, NotaFiscalDigitalItemComplementoResponse>();

                cfg.CreateMap<NotaFiscalDigitalComplemento, NotaFiscalDigitalComplementoResponse>();
                cfg.CreateMap<NotaFiscalDigitalDetalhesFiscais, NotaFiscalDigitalDetalhesFiscaisResponse>();

                //Fatura
                cfg.CreateMap<Fatura, FaturaResponse>()
                    .ForMember(dest => dest.Cancelada, opt => opt.MapFrom(src => src.Cancelada == "S"));
                cfg.CreateMap<FaturaItem, FaturaItemResponse>();
                cfg.CreateMap<DTOS.Request.Fatura.FaturaAtualizarRequest, Fatura>()
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                //Operação Financeira
                cfg.CreateMap<DTOS.Request.OperacaoFinanceira.OperacaoFinanceiraAdicionarRequest, OperacaoFinanceira>()
                    .ForMember(dest => dest.SemMovFinanceiro, opt => opt.MapFrom(src => src.SemMovFinanceiro == null ? null : src.SemMovFinanceiro.Value ? "S" : "N"))
                    .ForMember(dest => dest.CentrosDeCusto, opt => opt.ResolveUsing(src => src.CentrosDeCusto != null ? string.Concat(src.CentrosDeCusto.Select((x) => x.ToString("D3") + ";")) : null))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
                
                cfg.CreateMap<DTOS.Request.OperacaoFinanceira.OperacaoFinanceiraAtualizarRequest, OperacaoFinanceira>()
                    .IgnoreMember(t => t.Codigo)
                    .ForMember(dest => dest.SemMovFinanceiro, opt => opt.MapFrom(src => src.SemMovFinanceiro == null ? null : src.SemMovFinanceiro.Value ? "S" : "N"))
                    .ForMember(dest => dest.CentrosDeCusto, opt => opt.ResolveUsing(src => src.CentrosDeCusto != null ? string.Concat(src.CentrosDeCusto.Select((x) => x.ToString("D3") + ";")) : null))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<OperacaoFinanceira, DTOS.Response.OperacaoFinanceira.OperacaoFinanceiraResponse>()
                    .ForMember(dest => dest.SemMovFinanceiro, opt => opt.MapFrom(src => src.SemMovFinanceiro == "S"))
                    .ForMember(dest => dest.CentrosDeCusto, opt => opt.ResolveUsing(src =>
                    {
                        return src.CentrosDeCusto?.Split(';')
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .Select(s => int.TryParse(s, out var parsedValue) ? parsedValue : 0)
                            .ToList();
                    }));
                
                // Tipo de Documento
                cfg.CreateMap<DTOS.Request.TipoDocumento.TipoDocumentoAdicionarRequest, TipoDocumento>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == null ? null : src.Fixo == true ? "S" : "N"))
                    .ForMember(dest => dest.Nfse, opt => opt.MapFrom(src => src.Nfse == null ? null : src.Nfse == true ? "S" : "N"))
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<DTOS.Request.TipoDocumento.TipoDocumentoAtualizarRequest, TipoDocumento>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == null ? null : src.Fixo == true ? "S" : "N"))
                    .ForMember(dest => dest.Nfse, opt => opt.MapFrom(src => src.Nfse == null ? null : src.Nfse == true ? "S" : "N"))
                    .IgnoreMember(x => x.Codigo)
                    .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

                cfg.CreateMap<TipoDocumento, DTOS.Response.TipoDocumento.TipoDocumentoResponse>()
                    .ForMember(dest => dest.Fixo, opt => opt.MapFrom(src => src.Fixo == "S"))
                    .ForMember(dest => dest.Nfse, opt => opt.MapFrom(src => src.Nfse == "S"));

                // Boleto Externo
                cfg.CreateMap<DTOS.Request.BoletoExterno.BoletoExternoAdicionarRequest, BoletoExterno>()
                    .IgnoreMember(x => x.Id)
                    .IgnoreMember(x => x.DataHora);

                cfg.CreateMap<BoletoExterno, DTOS.Response.BoletoExterno.BoletoExternoResponse>();

            });
        }

        public static IMappingExpression<TSource, TDestination> IgnoreMember<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            return map.ForMember(selector, config => config.Ignore());
        }

        public static IMappingExpression<TSource, TDestination> MapMemberFrom<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> destination,
            Expression<Func<TSource, object>> source)
        {
            return map.ForMember(destination, opts => opts.MapFrom(source));
        }

        public static IMappingExpression<TSource, TDestination> MapDtoToEndereco<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map)
            where TSource : IHasEnderecoDTO
            where TDestination : IHasEndereco
        {
            return map.IgnoreMember(t => t.EnderecoMunicipio)
                .MapMemberFrom(t => t.EnderecoMunicipioCodigo, t => (t.Endereco.Municipio != null ? t.Endereco.Municipio.Codigo : 0));
        }

        public static IMappingExpression<TSource, TDestination> MapEnderecoToDto<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map)
            where TSource : IHasEndereco
            where TDestination : IHasEnderecoDTO
        {
            return map.MapMemberFrom(
                dest => dest.Endereco,
                src => new Endereco
                {
                    Cep = src.EnderecoCep,
                    Logradouro = src.EnderecoLogradouro,
                    Numero = src.EnderecoNumero,
                    Complemento = src.EnderecoComplemento,
                    Bairro = src.EnderecoBairro,
                    MunicipioCodigo = src.EnderecoMunicipioCodigo ?? 0,
                    Municipio = src.EnderecoMunicipio
                }
            );
        }

        public class NullableConverter<TSource, TDestination> : ITypeConverter<TSource?, TDestination>
            where TSource : struct
        {
            public TDestination Convert(TSource? source, TDestination destination, ResolutionContext context)
            {
                return source.HasValue ? context.Mapper.Map<TSource, TDestination>(source.Value) : destination;
            }
        }
    }
}
