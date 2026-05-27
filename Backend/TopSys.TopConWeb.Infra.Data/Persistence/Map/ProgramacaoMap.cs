using System.Data.Entity.ModelConfiguration;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Map
{
    public class ProgramacaoMap : EntityTypeConfiguration<Programacao>
    {
        public ProgramacaoMap()
        {
            ToTable("topsys.con_programacao");

            HasKey(t => new { t.UsinaCodigo, t.ObraNumero, t.Sequencia});

            Ignore(t => t.TemNotaFicalEmitida);
            Ignore(t => t.ValorTotalRemessasEmitidas);

            HasOptional(t => t.Contrato)
               .WithMany()
               .HasForeignKey(t => new { t.UsinaCodigo, t.ContratoAno, t.ContratoNumero })
               .WillCascadeOnDelete(false);

            HasOptional(t => t.Proposta)
               .WithMany()
               .HasForeignKey(t => new { t.UsinaCodigo, t.PropostaAno, t.PropostaNumero })
               .WillCascadeOnDelete(false);

            HasRequired(t => t.Usina)
                .WithMany()
                .HasForeignKey(t => t.UsinaCodigo);

            HasMany(t => t.DemaisServicos)
                .WithRequired()
                .HasForeignKey(t => new { t.UsinaCodigo, t.ObraNumero, t.ProgramacaoSequencia });

            Property(t => t.UsinaCodigo)
                .HasColumnOrder(0)
                .HasColumnName("usina");

            Property(t => t.ContratoAno)
                .HasColumnOrder(1)
                .HasColumnName("ano_contrato");

            Property(t => t.ContratoNumero)
                .HasColumnOrder(2)
                .HasColumnName("no_contrato");

            Property(t => t.Sequencia)
                .HasColumnOrder(3)
                .HasColumnName("seq_prog");

            Property(t => t.PropostaAno)
                .HasColumnOrder(4)
                .HasColumnName("ano_chamada");

            Property(t => t.PropostaNumero)
                .HasColumnOrder(5)
                .HasColumnName("num_chamada");

            Property(t => t.ObraNumero)
                .HasColumnOrder(6)
                .HasColumnName("no_obra");

            Property(t => t.ObraNome)
                .HasColumnName("obra_nome");
            
            Property(t => t.UsinaEntregaCodigo)
                .HasColumnName("usina_entrega");

            HasRequired(t => t.UsinaEntrega)
                .WithMany()
                .HasForeignKey(t => t.UsinaEntregaCodigo)
                .WillCascadeOnDelete(false);
            
            Property(t => t.EnderecoCep)
                .HasColumnName("obra_cep");
            
            Property(t => t.EnderecoLogradouro)
                .HasColumnName("obra_end");
            
            Property(t => t.EnderecoNumero)
                .HasColumnName("obra_no");
            
            Property(t => t.EnderecoComplemento)
                .HasColumnName("obra_compl");
            
            Property(t => t.EnderecoBairro)
                .HasColumnName("obra_bairro");
            
            Property(t => t.EnderecoMunicipioCodigo)
                .HasColumnName("obra_mun");
            HasOptional(t => t.EnderecoMunicipio)
                .WithMany()
                .HasForeignKey(t => t.EnderecoMunicipioCodigo)
                .WillCascadeOnDelete(false);
            
            Property(t => t.ResistenciaTipoCodigo)
                .HasColumnName("tp_resist");
            HasOptional(t => t.ResistenciaTipo)
                .WithMany()
                .HasForeignKey(t => t.ResistenciaTipoCodigo)
                .WillCascadeOnDelete(false);
            
            Property(t => t.Mpa)
                .HasColumnName("fck");
            
            Property(t => t.Consumo)
                .HasColumnName("consumo");
            
            Property(t => t.PedraCodigo)
                .HasColumnName("pedra");
            HasOptional(t => t.Pedra)
                .WithMany()
                .HasForeignKey(t => t.PedraCodigo)
                .WillCascadeOnDelete(false);
            
            Property(t => t.SlumpCodigo)
                .HasColumnName("slump");
            HasOptional(t => t.Slump)
                .WithMany()
                .HasForeignKey(t => t.SlumpCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.SlumpNotaFiscal)
                .HasColumnName("slump_nf");

            Property(t => t.UsoCodigo)
                .HasColumnName("uso");

            HasOptional(t => t.Uso)
                .WithMany()
                .HasForeignKey(t => t.UsoCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.PecaConcretar)
                .HasColumnName("peca_concretar");

            Property(t => t.Andar)
                .HasColumnName("andar");

            Property(t => t.ObraTracoSequencia)
                .HasColumnName("seq_item_cont");
            
            Property(t => t.VolumeTotal)
                .HasColumnName("qtde_m3");
           
            Property(t => t.VolumeEntregue)
                .HasColumnName("qtde_entregue");
            
            Property(t => t.VolumePorCarga)
                .HasColumnName("qtde_bt");
            
            Property(t => t.DataConcretagem)
                .HasColumnName("dt_concretagem");
            
            Property(t => t.Horario)
                .HasColumnName("horario");

            Property(t => t.NecessitaConfirmacao)
                .HasColumnName("dt_hr_confirmar");

            Property(t => t.VolumeLiberado)
                .HasColumnName("qtde_liberada");

            Property(t => t.IntervaloEmMinutosEntreCargas)
                .HasColumnName("intervalo");
            
            Property(t => t.ObraBombaSequencia)
                .HasColumnName("tipo_bomba");

            Property(t => t.HorarioBomba)
                .HasColumnName("horario_bomba");

            Property(t => t.DistanciaTubulacao)
                .HasColumnName("dist_tub_bomba");

            Property(t => t.ValorAdicionalTubulacao)
                .HasColumnName("vlr_adic_tub");

            Property(t => t.EquipamentoBombaCodigo)
                .HasColumnName("cod_equip_bomba");

            Property(t => t.TracoPesadoResistenciaTipoCodigo)
                .HasColumnName("pTp_resist");
            HasOptional(t => t.TracoPesadoResistenciaTipo)
                .WithMany()
                .HasForeignKey(t => t.TracoPesadoResistenciaTipoCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.TracoPesadoMpa)
                .HasColumnName("pfck");

            Property(t => t.TracoPesadoConsumo)
                .HasColumnName("pconsumo");

            Property(t => t.TracoPesadoPedraCodigo)
                .HasColumnName("ppedra");
            HasOptional(t => t.TracoPesadoPedra)
                .WithMany()
                .HasForeignKey(t => t.TracoPesadoPedraCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.TracoPesadoSlumpCodigo)
                .HasColumnName("pslump");
            HasOptional(t => t.TracoPesadoSlump)
                .WithMany()
                .HasForeignKey(t => t.TracoPesadoSlumpCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.TracoPesadoUsoCodigo)
                .HasColumnName("puso");
            HasOptional(t => t.TracoPesadoUso)
                .WithMany()
                .HasForeignKey(t => t.TracoPesadoUsoCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.VibradorQuantidade)
                .HasColumnName("vibr_qtde");

            Property(t => t.VibradorValorUnitario)
                .HasColumnName("vibr_vlr_unit");

            Property(t => t.VibradorValorTotal)
                .HasColumnName("vlr_total_vib");

            Property(t => t.VibradorVendedorCodigo)
                .HasColumnName("vibr_vendedor");
            HasOptional(t => t.VibradorVendedor)
                .WithMany()
                .HasForeignKey(t => t.VibradorVendedorCodigo)
                .WillCascadeOnDelete(false);

            Property(t => t.Status)
                .HasColumnName("status");

            Property(t => t.Solicitante)
                .HasColumnName("solicitante");

            Property(t => t.Observacao)
                .HasColumnName("obs");

            Property(t => t.FaxSimNao)
                .HasColumnName("fax");

            Property(t => t.ContinuidadeDaSequencia)
                .HasColumnName("continua_seq");

            Property(t => t.ObservacaoInterna)
                .HasColumnName("obs_interna");

            Property(t => t.DataHoraSolicitacao)
                .HasColumnName("dt_hr_solic");

            Property(t => t.IdCadastro)
                .HasColumnName("id_cadast");

            Property(t => t.IdAtual)
                .HasColumnName("id_atual");

            Property(t => t.CorpoDeProvaTipo)
                .HasColumnName("cp_prv_tipo");

            Property(t => t.CorpoDeProvaIntervalo)
                .HasColumnName("cp_prv_intvl_m3");

            Property(t => t.CorpoDeProvaMoldador)
                .HasColumnName("cp_moldador");

            Property(t => t.CorpoDeProvaQuantidade)
                .HasColumnName("cp_prv_qtde");

            Property(t => t.CorpoDeProvaMoldagemRemota)
                .HasColumnName("cp_moldagem_remota");

            Property(t => t.ValorTotal)
                .HasColumnName("vlr_total_prog");

            Property(t => t.ValorConcreto)
                .HasColumnName("vlr_concreto");

            Property(t => t.ValorExtras)
                .HasColumnName("vlr_extras");

            Property(t => t.ValorBomba)
                .HasColumnName("vlr_bomba");

            Property(t => t.ValorDemaisServicos)
                .HasColumnName("vlr_demais_servicos");

            Property(t => t.ExternalId)
                .HasColumnName("external_id");

            Property(t => t.ObraFrenteSequencia)
                .HasColumnName("obra_frente_sequencia");

            Property(t => t.CanceladoPor)
                .HasColumnName("cancelado_por");

            Property(t => t.TempoAteAObra)
                .HasColumnName("temp_ate_a_obra");

            Property(t => t.TempoBtNaObra)
                .HasColumnName("temp_bt_na_obra");

            Property(t => t.TempoDescarga)
                .HasColumnName("tempo_descarga");
        }
    }
}
