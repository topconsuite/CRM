using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class EquipamentoMap : EntityTypeConfiguration<Equipamento>
    {
        public EquipamentoMap()
        {

            ToTable("con_equipamento");

            HasKey(t => t.Codigo);

            Property(t => t.Codigo)
                .HasColumnName("cod")
                .IsRequired()
                .HasMaxLength(8);

            Property(t => t.ExternalID)
                .HasColumnName("external_id")
                .IsRequired();

            Property(t => t.Tipo)
                .HasColumnName("tipo")
                .IsRequired();

            Property(t => t.Betoneira)
                .HasColumnName("betoneira")
                .IsRequired()
                .HasMaxLength(1);

            Property(t => t.Bomba)
                .HasColumnName("bomba")
                .IsRequired()
                .HasMaxLength(1);

            Property(t => t.Placa)
                .HasColumnName("placa")
                .IsRequired()
                .HasMaxLength(8);

            Property(t => t.Descricao)
                .HasColumnName("descr")
                .IsRequired()
                .HasMaxLength(60);

            Property(t => t.CapacidadeM3)
                .HasColumnName("capacidade_m3")
                .IsRequired();

            Property(t => t.MotoristaBombaUsina)
                .HasColumnName("mtr_bomb_usina")
                .IsRequired();

            Property(t => t.FuncionarioAlocado)
                .HasColumnName("mtr_bomb_codigo")
                .IsRequired();

            Property(t => t.AjudanteUsina)
                .HasColumnName("ajudande_usina")
                .IsRequired();

            Property(t => t.AjudanteCodigo)
                .HasColumnName("ajudande_codigo")
                .IsRequired();

            Property(t => t.AjudanteCodigo2)
                .HasColumnName("ajudante_codigo2")
                .IsRequired();

            Property(t => t.AjudanteCodigo3)
                .HasColumnName("ajudante_codigo3")
                .IsRequired();

            Property(t => t.UsinaAlocada)
                .HasColumnName("usina_alocada")
                .IsRequired();

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast")
                .IsRequired();

            Property(t => t.IdAtual)
                .HasColumnName("id_atual")
                .IsRequired();

            Property(t => t.Marca)
                .HasColumnName("marca")
                .IsRequired();

            Property(t => t.Modelo)
                .HasColumnName("modelo")
                .IsRequired();

            Property(t => t.Chassis)
                .HasColumnName("chassis")
                .IsRequired();

            Property(t => t.Ano)
                .HasColumnName("ano")
                .IsRequired();

            Property(t => t.Status)
                .HasColumnName("status")
                .IsRequired();

            Property(t => t.DataOcorrencia)
                .HasColumnName("dt_ocorrencia");

            Property(t => t.TipoProcesso)
                .HasColumnName("tp_processo")
                .IsRequired();

            Property(t => t.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            Property(t => t.PossuiGps)
                .HasColumnName("possui_gps")
                .IsRequired();

            Property(t => t.DataInstalacaoGps)
                .HasColumnName("dt_inst_gps");

            Property(t => t.Gps)
                .HasColumnName("gps")
                .IsRequired();

            Property(t => t.NumeroProcesso)
                .HasColumnName("no_processo")
                .IsRequired();

            Property(t => t.Observacao)
                .HasColumnName("obs")
                .IsRequired();

            Property(t => t.Grupo)
                .HasColumnName("grupo")
                .IsRequired();

            Property(t => t.SubGrupo)
                .HasColumnName("sub_grupo")
                .IsRequired();
            
            Property(t => t.Item)
                .HasColumnName("item")
                .IsRequired();

            Property(t => t.Grupo2)
                .HasColumnName("grupo2")
                .IsRequired();

            Property(t => t.SubGrupo2)
                .HasColumnName("sub_grupo2")
                .IsRequired();

            Property(t => t.Item2)
                .HasColumnName("item2")
                .IsRequired();

            Property(t => t.AnoModelo)
                .HasColumnName("ano_modelo")
                .IsRequired();

            Property(t => t.Renavam)
                .HasColumnName("renavam")
                .IsRequired();

            Property(t => t.Combustivel)
                .HasColumnName("combustivel")
                .IsRequired();

            Property(t => t.CapacidadePotCilindros)
                .HasColumnName("capac_pot_cil")
                .IsRequired();

            Property(t => t.FaixaIpva)
                .HasColumnName("faixa_ipva")
                .IsRequired();

            Property(t => t.Proprietario)
                .HasColumnName("proprietario")
                .IsRequired();

            Property(t => t.TacografoQuebrado)
                .HasColumnName("tacografo_quebr")
                .IsRequired();

            Property(t => t.UF)
                .HasColumnName("uf")
                .IsRequired();

            Property(t => t.FinalPlaca)
                .HasColumnName("final_placa")
                .IsRequired();

            Property(t => t.CapacidadeLitrosCombustivel)
                .HasColumnName("cap_lit_comb")
                .IsRequired();

            Property(t => t.PesoEquipamento)
                .HasColumnName("peso_equip")
                .IsRequired();

            Property(t => t.ControlaKm)
                .HasColumnName("controla_km")
                .IsRequired();

            Property(t => t.ControlaHorimetro)
                .HasColumnName("controla_horim")
                .IsRequired();

            Property(t => t.HorimetroQuebrado)
                .HasColumnName("horim_quebr");

            Property(t => t.DataSubstituicaoTacografo)
                .HasColumnName("dt_subst_tac");

            Property(t => t.DataSubstituicaoHorimetro)
                .HasColumnName("dt_subst_hori");

            Property(t => t.CC)
                .HasColumnName("cc")
                .IsRequired();

            Property(t => t.MaxKmAbastecimento)
                .HasColumnName("max_km_abast")
                .IsRequired();

            Property(t => t.MaxLitrosAbastecimento)
                .HasColumnName("max_litros_abast")
                .IsRequired();

            Property(t => t.KmInicial)
                .HasColumnName("km_inicial")
                .IsRequired();

            Property(t => t.HorimetroInicial)
                .HasColumnName("horimetro_inicial")
                .IsRequired();

            Property(t => t.SeguroResponsabilidade)
                .HasColumnName("seguro_resp_seg")
                .IsRequired();

            Property(t => t.SeguroCnpjCpfResponsavel)
                .HasColumnName("seguro_cnpj_cpf_responsavel");

            Property(t => t.SeguroNomeSeguradora)
                .HasColumnName("seguro_nome_seguradora");

            Property(t => t.SeguroCnpjSeguradora)
                .HasColumnName("seguro_cnpj_seguradora");

            Property(t => t.SeguroNumeroApolice)
                .HasColumnName("seguro_num_apolice");

            Property(t => t.SeguroNumeroAverbacao)
                .HasColumnName("seguro_num_averbacao");

            Property(t => t.TipoRodado)
                .HasColumnName("tipo_rodado")
                .IsRequired();

            Property(t => t.TipoCarroceria)
                .HasColumnName("tipo_carroceria");

            Property(t => t.RNTRC)
                .HasColumnName("rntrc");

            Property(t => t.TipoProprietario)
                .HasColumnName("tipo_proprietario");

        }
    }
}
