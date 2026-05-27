import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef, MatDialog, MatExpansionPanel } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Usina } from 'app/classes/usina/usina';
import { ObraConsulta, EStatusCadastro, EStatusFinanceiro, EStatusEngenharia, statusCadastro, statusEngenharia, statusFinanceiro, Obra, ObraTributacaoMunicipal, ObraTraco, ObraTaxa } from 'app/classes/obra/obra.classes';
import { Proposta, Status, statusProposta, statusComercial, statusContrato, statusGeracaoContrato,
  EStatusComercial } from 'app/classes/proposta/proposta.classes';
import { EStatusContrato, Contrato } from 'app/classes/contrato/contrato';
import { Interveniente, intervenienteTipos } from 'app/classes/interveniente/interveniente';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { TipoCobranca } from 'app/classes/pagamento/tipo-cobranca';
import { Pagamento, PagamentoDetalheDeposito, CartaoBandeira, Portador } from 'app/classes/pagamento/pagamento.classes';
import { CadastroGeral } from 'app/classes/bomba/bomba.classes';
import { Funcionario } from 'app/classes/funcionario/funcionario';
import { MovimentoBanco } from 'app/classes/movimento-banco/movimento-banco';
import { IntervenienteAnexo } from 'app/classes/interveniente/interveniente-anexo';
import { EStatusClicksignDocumento, statusClicksignDocumento } from 'app/classes/assinatura-eletronica/contrato-clicksing-envio';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ObraLogDialogComponent } from 'app/main/components/dialog/obra-log-dialog/obra-log-dialog.component';
import { PropostaProgramacoesDialogComponent } from 'app/main/components/dialog/proposta-programacoes-dialog/proposta-programacoes-dialog.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { MovimentosBancoAVincularDialogComponent } from 'app/main/components/dialog/movimentos-banco-avincular-dialog/movimentos-banco-avincular-dialog.component';

import { UsinaService } from 'app/services/usina.service';
import { UserService } from 'app/services/user.service';
import { PropostaService } from 'app/services/proposta.service';
import { ProgramacaoService } from 'app/services/programacao.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { VendedorService } from 'app/services/vendedor.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { ObraService } from 'app/services/obra.service';
import { ContratoService } from 'app/services/contrato.service';
import { PagamentoService } from 'app/services/pagamento.service';
import { ICustomValidator } from 'app/main/components/interfaces/custom-validator';
import { CadastroGeralService } from 'app/services/cadastro-geral.service';
import { FuncionarioService } from 'app/services/funcionario.service';
import { ObraTributacoesMunicipaisDialogComponent } from 'app/main/components/dialog/obra-tributacoes-municipais-dialog/obra-tributacoes-municipais-dialog.component';
import { IntervenienteHistorico } from 'app/classes/interveniente/interveniente-historico';
import { HistoricoIntervenienteDialogComponent } from 'app/main/components/dialog/historico-interveniente-dialog/historico-interveniente-dialog.component';
import { ParametroService } from 'app/services/parametro.service';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';
import { CustoServicoService } from 'app/services/custo-servico.service';
import { TracoPreco } from 'app/classes/traco/traco-preco';
import { TracoPrecoService } from 'app/services/traco-preco.service';
import { Filial, ModeloDocumentoRemessaConcreto } from 'app/classes/filial/Filial';
import { FilialService } from 'app/services/filial.service';
import { CadastroDiversoService } from 'app/services/cadastro-diverso.service';
import { EbitaObraTracoComponent } from 'app/main/components/business components/ebitda-obra-traco/ebitda-obra-traco.component';
import { EbitaObraBombaComponent } from 'app/main/components/business components/ebitda-obra-bomba/ebitda-obra-bomba.component';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Endereco, Municipio } from 'app/classes/endereco/endereco';
import { userInfo } from 'os';
import { AprovacaoComercialService } from 'app/services/aprovacao-comercial.service';
import { AprovacaoComercialHierarquiaDireito } from 'app/classes/aprovacao-comercial/aprovacao-comercial-hierarquia-direito.classes';
import { AprovacaoComercialDados, AprovacaoComercialDadosItem, EAprovacaoComercialPendenteStatus } from 'app/classes/aprovacao-comercial/aprovacao-comercial-dados.classes';
import { ModeloDocumentoDanfeRomaneio, modelosDanfeRomaneio, statusComercialAlcada } from 'app/classes/proposta/proposta';
import { ESituacaoAprovacaoComercialAlcadaUsuario } from 'app/classes/obra/obra-consulta';
import { EObraDemaisStatusComercial } from 'app/classes/obra/obra';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { AcessoAprovacoesConfigService } from 'app/services/acesso-aprovacoes-config.service';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';
import { GrupoEconomico } from 'app/classes/grupo-economico/grupo-economico';
import { GrupoEconomicoService } from 'app/services/grupo-economico.service';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
  getDynamicPlaceholder?: any;
}

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-proposta-aprovacoes-page',
  templateUrl: './proposta-aprovacoes-page.component.html',
  styleUrls: ['./proposta-aprovacoes-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})

export class PropostaAprovacoesPageComponent implements OnInit, AfterViewInit {
  public static self: PropostaAprovacoesPageComponent;

  cadastroForm: FormGroup;
  engenhariaForm: FormGroup;
  financeiroForm: FormGroup;
  comercialForm: FormGroup;

  maskHora = [/\d/, /\d/, ':', /\d/, /\d/];
  maskCEI = [/\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, /\d/, /\d/,'/',  /\d/, /\d/];

  usinas: Usina[] = [];
  motivosBloqueioInterveniente: CadastroGeral[] = [];

  analistas: Funcionario[] = [];

  itens: PagedList<ObraConsulta> = new PagedList<ObraConsulta>();
  obra: Obra;
  tracosExibicaoEbitda: ObraTraco[] = [];
  custoServico: CustoServico;
  openedElement: ObraConsulta;

  aprovacaoComercialAprovacoesRestantes: AprovacaoComercialDados = new AprovacaoComercialDados();
  aprovacaoComercialHierarquiaDireitos: AprovacaoComercialHierarquiaDireito;

  intervenientes: Interveniente[] = [];
  vendedoresPermitidos: Vendedor[] = [];
  tiposCobranca: TipoCobranca[] = [];
  cartaoBandeiras: CartaoBandeira[] = [];
  portadores: Portador[] = [];
  
  segmentacao: Segmentacao[] = [];
  finalidades: ContratoFinalidade[] = [];

  gruposEconomicos: GrupoEconomico[] = [];

  temDireitoAprovarCadastro: boolean = false;

  tracosPreco: TracoPreco[] = [];

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filiais: Filial[] = [];
  modelosDocumentosRemessa: ModeloDocumentoRemessaConcreto[] = [];
  modelosItensDanfeERomaneio: ModeloDocumentoDanfeRomaneio[] = [];

  geraFaturaNaPesagem: boolean = false;

  anexoForm: FormGroup;
  anexoDescricaoForm: FormGroup;
  anexos: IntervenienteAnexo[] = [];
  anexo: IntervenienteAnexo;
  descricaoAnteriorAnexo: string = '';

  statusAprovacao = {
    naoNecessita: 0,
    pendente: 1,
    aprovado: 2,
    reprovado: 3,
    alterado: 4
  };

  semOrdenacao: boolean[] = [];
  ordenacaoCrescente: boolean[] = [];
  ordenacaoDecrescente: boolean[] = [];
  ordenacao: string = "";

  statusComercial = statusComercial;
  statusGeracaoContrato = statusGeracaoContrato;
  statusCadastro = statusCadastro;
  statusEngenharia = statusEngenharia;
  statusFinanceiro = statusFinanceiro;
  statusClicksignDocumento = statusClicksignDocumento.filter(t => t.codigo < EStatusClicksignDocumento.Cancelado);
  statusComercialCodigos = statusComercial.map(t => t.codigo);
  statusGeracaoContratoCodigos = statusGeracaoContrato.map(t => t.codigo);
  statusCadastroCodigos = statusCadastro.map(t => t.codigo);
  statusEngenhariaCodigos = statusEngenharia.map(t => t.codigo);
  statusFinanceiroCodigos = statusFinanceiro.map(t => t.codigo);
  statusClicksignDocumentoCodigos = statusClicksignDocumento.map(t => t.codigo);

  statusAlcadaAguardandoAprovacaoOutroNivel = false;

  temAnaliseCreditoPorTerceiros = false;

  inativarAprovacoesFinanceiras = false;

  limiteCreditoPorGrupoEconomico: boolean = false;

  get statusComercialIn(): string {
    if (!this.filtro.statusComercialIn)
      return '';
    return this.filtro.statusComercialIn.map(t => t.codigo).join(',');
  }
  get statusGeracaoContratoIn(): string {
    if (!this.filtro.statusGeracaoContratoIn)
      return '';
    return this.filtro.statusGeracaoContratoIn.map(t => t.codigo).join(',');
  }
  get statusCadastroIn(): string {
    if (!this.filtro.statusCadastroIn)
      return '';
    return this.filtro.statusCadastroIn.map(t => t.codigo).join(',');
  }
  get statusEngenhariaIn(): string {
    if (!this.filtro.statusEngenhariaIn)
      return '';
    return this.filtro.statusEngenhariaIn.map(t => t.codigo).join(',');
  }
  get statusFinanceiroIn(): string {
    if (!this.filtro.statusFinanceiroIn)
      return '';
    return this.filtro.statusFinanceiroIn.map(t => t.codigo).join(',');
  }
  get statusClicksignDocumentoIn(): string {
    if (!this.filtro.statusClicksignDocumentoIn)
      return '';
    return this.filtro.statusClicksignDocumentoIn.map(t => t.codigo).join(',');
  }

  dadosStatusComercial(statusCode: number, statusAlcada: ESituacaoAprovacaoComercialAlcadaUsuario = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza): Status {
    if(statusAlcada == ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza)
      return statusComercial.filter(t => t.codigo === statusCode)[0];
    
    return statusComercialAlcada.filter(t => t.codigo === statusAlcada)[0];
  }
  dadosStatusGeracaoContratoIn(statusCode: number): Status {
    return statusGeracaoContrato.filter(t => t.codigo === statusCode)[0];
  }
  dadosStatusCadastro(statusCode: number): Status {
    return statusCadastro.filter(t => t.codigo === statusCode)[0];
  }
  dadosStatusEngenharia(statusCode: number): Status {
    return statusEngenharia.filter(t => t.codigo === statusCode)[0];
  }
  dadosStatusFinanceiro(statusCode: number): Status {
    return statusFinanceiro.filter(t => t.codigo === statusCode)[0];
  }
  dadosStatusClicksignDocumento(statusCode: number): Status {
    return statusClicksignDocumento.filter(t => t.codigo === statusCode)[0];
  }
  statusFormatter = (model: Status): string => {
    return model ? model.descricao.toUpperCase() : '';
  };
  statusCadastroFormatter = (codigo: number): string => {
    let status = statusCadastro.filter(t => t.codigo === codigo)[0];
    return status ? status.descricao.toUpperCase() : '';
  };
  statusFinanceiroFormatter = (codigo: number): string => {
    let status = statusFinanceiro.filter(t => t.codigo === codigo)[0];
    return status ? status.descricao.toUpperCase() : '';
  };
  motivoBloqueioFormatter = (model: CadastroGeral): string => {
    return model ? model.descricao.toUpperCase() : '';
  }

  corAnaliseLimiteCredito(mensagemAnaliseLimite: string) {
    switch ((mensagemAnaliseLimite || '').toUpperCase()) {
      case 'CRÉDITO OK':
        return '#00a500';
      case '':
        return '#CCCCCC';
      default:
        return '#FF8072';
    }
  }

  get usinaEntregaIn(): string {
    if (!this.filtro.usinaEntregaIn)
      return '';
    return this.filtro.usinaEntregaIn.map(t => t.codigo).join(',');
  }
  get analistaIn(): string {
    if (!this.filtro.analistaIn)
      return '';
    return this.filtro.analistaIn.map(t => t.codigo).join(',');
  }
  get tipoCobrancaIn(): string {
    if (!this.filtro.tipoCobrancaIn)
      return '';
    return this.filtro.tipoCobrancaIn.map(t => t.codigo).join(',');
  }
  get bandeiraIn(): string {
    if (!this.filtro.bandeiraIn)
      return '';
    return this.filtro.bandeiraIn.map(t => t.codigo).join(',');
  }
  get portadorIn(): string {
    if (!this.filtro.portadorIn)
      return '';
    return this.filtro.portadorIn.map(t => t.codigo).join(',');
  }

  addDaysFromCurdate(days: number): Date {
    var date = new Date();
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    date.setDate(date.getDate() + days);
    return date;
  }

  setProgramacaoDiasAteDataAtual(days: number) {
    if (this.filtro.programacaoDataRelativaDePosterior)
      this.filtro.programacaoDataDe = this.addDaysFromCurdate(days);
    else
      this.filtro.programacaoDataDe = this.addDaysFromCurdate(-days);
  }
  setProgramacaoDiasAposDataAtual(days: number) {
    if (this.filtro.programacaoDataRelativaAteAnterior)
      this.filtro.programacaoDataAte = this.addDaysFromCurdate(-days);
    else
      this.filtro.programacaoDataAte = this.addDaysFromCurdate(days);
  }

  setPeriodoDiasAteDataAtual(days: number) {
    this.filtro.periodoDe = this.addDaysFromCurdate(-days);
  }
  setPeriodoDiasAposDataAtual(days: number) {
    this.filtro.periodoAte = this.addDaysFromCurdate(days);
  }

  quantidadeBombeada(): number {
    let volumeBombeavel: number = 0;
    this.obra.obraTracos.filter(t => t.slump.codigo >= 9).forEach(traco => {
      volumeBombeavel += traco.m3QuantidadeBombeada;
    });
    return volumeBombeavel;
  }

  get periodoAte(): string {
    this.filtro.periodoAte = new Date(this.filtro.periodoAte)
    if (this.filtro.periodoRelativo)
      return this.addDaysFromCurdate(this.filtro.periodoDiasAposDataAtual).toISOString().substr(0, 10);
    return this.filtro.periodoAte.toISOString().substr(0, 10);
  }
  get periodoDe(): string {
    this.filtro.periodoDe = new Date(this.filtro.periodoDe)
    if (this.filtro.periodoRelativo)
      return this.addDaysFromCurdate(-this.filtro.periodoDiasAteDataAtual).toISOString().substr(0, 10);
    return this.filtro.periodoDe.toISOString().substr(0, 10);
  }

  get programacaoDataAte(): Date {
    if (this.filtro.programacaoDataRelativa) {
      if (this.filtro.programacaoDataRelativaAteAnterior)
        return this.addDaysFromCurdate(-this.filtro.programacaoDiasAposDataAtual);
      else
        return this.addDaysFromCurdate(this.filtro.programacaoDiasAposDataAtual);
    }
    return this.filtro.programacaoDataAte;
  }
  get programacaoDataDe(): Date {
    if (this.filtro.programacaoDataRelativa) {
      if (this.filtro.programacaoDataRelativaDePosterior)
        return this.addDaysFromCurdate(this.filtro.programacaoDiasAteDataAtual);
      else
        return this.addDaysFromCurdate(-this.filtro.programacaoDiasAteDataAtual);
    }
    return this.filtro.programacaoDataDe;
  }

  get programacaoDataHoraAte(): string {
    let horas = 23;
    let minutos = 59;
    let segundos = 59;

    if (this.filtro.programacaoHoraAte.length === 4) {
      horas = parseInt(this.filtro.programacaoHoraAte.substr(0, 2));
      minutos = parseInt(this.filtro.programacaoHoraAte.substr(2, 2));
    }

    let resultado = new Date(this.programacaoDataAte);
    resultado.setHours(horas);
    resultado.setMinutes(minutos);
    resultado.setSeconds(segundos);

    return resultado.toISOString();
  }

  get programacaoDataHoraDe(): string {
    let horas = 0;
    let minutos = 0;
    let segundos = 0;

    if (this.filtro.programacaoHoraDe.length === 4) {
      horas = parseInt(this.filtro.programacaoHoraDe.substr(0, 2));
      minutos = parseInt(this.filtro.programacaoHoraDe.substr(2, 2));
    }

    let resultado = new Date(this.programacaoDataDe);
    resultado.setHours(horas);
    resultado.setMinutes(minutos);
    resultado.setSeconds(segundos);

    return resultado.toISOString();
  }

  get percentualLocacaoValorMinimoValidator(): ICustomValidator {
    var _self = PropostaAprovacoesPageComponent.self;
    
    var message = `Valor abaixo do permitido! (mínimo: 0,01)`;

    return {
      key: 'valorAbaixoDoMinimo',
      message: message,
      validatorFunction: (obra): boolean => {
        return obra.contrato.percentualLocacao < 0.01;
      },
      params: [_self.obra]
    }
  }

  get temBomba(): boolean{
    if(this.obra.obraBombas == undefined) {
      return false;
    } else {
      return this.obra.obraBombas.length > 0;
    }
  }

  get maoObraPropriaOpcoes(): string[] {
    return ['', 'S', 'N', 'P'];
  };

  maoObraPropriaFormatter = (value: string): string => {
    switch (value) {
      case '':
          return '';
      case 'S':
        return 'Sim';
      case 'N':
        return 'Não';
      case 'P':
        return 'Parcial';
      default:
        return '';
    }
  };

  filtro: {
    usinaEntregaIn: Usina[],
    analistaIn: Funcionario[],
    tipoCobrancaIn: TipoCobranca[],
    bandeiraIn: CartaoBandeira[],
    portadorIn: Portador[],
    tipoDocumento: number,
    cpfCnpj: string,
    interveniente: Interveniente,
    intervenienteRazao: string,
    programacaoDataRelativa: boolean,
    programacaoDataRelativaDePosterior: boolean,
    programacaoDataRelativaAteAnterior: boolean,
    programacaoDiasAteDataAtual: number,
    programacaoDiasAposDataAtual: number,
    programacaoDataDe: Date,
    programacaoHoraDe: string,
    programacaoDataAte: Date,
    programacaoHoraAte: string,
    periodoRelativo: boolean,
    periodoDiasAteDataAtual: number,
    periodoDiasAposDataAtual: number,
    periodoDe: Date,
    periodoAte: Date,
    vendedor: Vendedor,
    considerarEncerrados: boolean,
    analisarLimiteCredito: boolean,
    analiseLimiteConsiderarPrevisao: boolean,
    statusGeracaoContratoIn: Status[],
    statusCadastroIn: Status[],
    statusComercialIn: Status[],
    statusEngenhariaIn: Status[],
    statusFinanceiroIn: Status[],
    statusClicksignDocumentoIn: Status[],
    anoContrato: number,
    numeroContrato: number,
    anoProposta: number,
    numeroProposta: number,
    segmentacao: Segmentacao,
    contratoFinalidade: ContratoFinalidade,
    aguardandoOutroNivel: boolean,
    grupoEconomico: GrupoEconomico,
    grupoEconomicoDescricao: string
  } = {
    usinaEntregaIn: [],
    analistaIn: [],
    tipoCobrancaIn: [],
    bandeiraIn: [],
    portadorIn: [],
    tipoDocumento: 0,
    cpfCnpj: '',
    interveniente: null,
    intervenienteRazao: '',
    programacaoDataRelativa: false,
    programacaoDataRelativaDePosterior: false,
    programacaoDataRelativaAteAnterior: false,
    programacaoDiasAteDataAtual: 0,
    programacaoDiasAposDataAtual: 0,
    programacaoDataDe: null,
    programacaoHoraDe: '0000',
    programacaoDataAte: null,
    programacaoHoraAte: '2359',
    periodoRelativo: false,
    periodoDiasAteDataAtual: 0,
    periodoDiasAposDataAtual: 0,
    periodoDe: null,
    periodoAte: null,
    vendedor: null,
    considerarEncerrados: false,
    analisarLimiteCredito: false,
    analiseLimiteConsiderarPrevisao: false,
    statusGeracaoContratoIn: [],
    statusCadastroIn: [],
    statusComercialIn: [],
    statusEngenhariaIn: [],
    statusFinanceiroIn: [],
    statusClicksignDocumentoIn: [],
    anoContrato: 0,
    numeroContrato: 0,
    anoProposta: 0,
    numeroProposta: 0,
    segmentacao: null,
    contratoFinalidade: null,
    aguardandoOutroNivel: false,
    grupoEconomico: null,
    grupoEconomicoDescricao: ''
  };

  formataData = Tasks.formataData;
  formataDataHora = Tasks.formataDataHora;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;
  ederecoToString = Tasks.ederecoToString;
  convertMinutosParaHora = Tasks.convertMinutosParaHora;

  subModalIsOpen: boolean = false;
  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  analistaFormatter = (model: Funcionario): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  tipoCobrancaFormatter = (model: TipoCobranca): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  bandeiraFormatter = (model: CartaoBandeira): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  portadorFormatter = (model: Portador): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  intervenienteFormatter = (model: Interveniente) => model ? model.codigo+' - '+model.razao.toUpperCase() : '';
  vendedorFormatter = (model: Vendedor) => model ? model.codigo+' - '+model.nome.toUpperCase() : '';
  segmentacaoFormatter = (model: Segmentacao) => model ? model.id+' - '+model.nome.toUpperCase() : '';
  finalidadeFormatter = (model: ContratoFinalidade): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  cepFormatter = (model: string) => model ?  `${model.substring(0,5)}-${model.substring(5)}` : '';
  intervenienteTipoFormatter = (model: Interveniente) => model ? intervenienteTipos.filter(t => t.codigo === model.intervenienteTipo).map(t => t.descricao) : '';
  dataContratoFormatter = (model: Date) => Tasks.formataData(model) === '01/01/1'  ? 'NÃO GERADO' : Tasks.formataData(model);

  grupoEconomicoFormatter = (model: GrupoEconomico) => model ? model.codigo+' - '+model.descricao.toUpperCase() : '';

  moedaFormatter = (item: number) => {
    let self = PropostaAprovacoesPageComponent.self;
    return self.formataValor(item, 2, true);
  }

  getAnalista(obraConsulta: ObraConsulta): string {
    if(obraConsulta.analistaCodigo === 0) return '';
    return (obraConsulta.analistaCodigo+' - '+obraConsulta.analistaNomeReduzido).toUpperCase();
  }

  getGrupoEconomico(obraConsulta: ObraConsulta): string {
    if(obraConsulta.grupoEconomicoCodigo === 0) return '';
    return (obraConsulta.grupoEconomicoCodigo+' - '+obraConsulta.grupoEconomicoDescricao).toUpperCase();
  }

  getPropostaNumero(obraConsulta: ObraConsulta): string {
    return obraConsulta.propostaNumero.toString().padStart(6,'0')+'-'+obraConsulta.propostaAno;
  }
  
  getContratoNumero(obraConsulta: ObraConsulta): string {
    if (!obraConsulta.contratoNumero) return 'NÃO GERADO';
    return obraConsulta.contratoNumero.toString().padStart(6,'0')+'-'+obraConsulta.contratoAno;
  }
  getInterveniente(obraConsulta: ObraConsulta): string {
    return (obraConsulta.clienteCodigo+' - '+obraConsulta.clienteRazao).toUpperCase();
  }
  getCpfCnpj(obraConsulta: ObraConsulta): string {
    if(obraConsulta.clienteCpfCnpj === null)
      return "";

    return Tasks.formataCpfCnpj(obraConsulta.clienteCpfCnpj);
  }
  getObraNome(obraConsulta: ObraConsulta): string {
    return obraConsulta.obraNome.toUpperCase();
  }
  getDataHoraProgramacao(obraConsulta: ObraConsulta): string {
    if (obraConsulta.dataConcretagem && obraConsulta.horario)
      return `${Tasks.formataData(obraConsulta.dataConcretagem)} ${Tasks.formataHora(obraConsulta.horario || '')}`;
    else
      return '';
  }
  getUsinaEntrega(obraConsulta: ObraConsulta): string {
    var self = PropostaAprovacoesPageComponent.self;
    return self.usinaFormatter(self.usinas.filter(t => t.codigo === obraConsulta.usinaEntrega)[0]);
  }
  getTipoCobranca(obraConsulta: ObraConsulta): string {
    return (obraConsulta.tipoCobrancaCodigo+' - '+obraConsulta.tipoCobrancaDescricao).toUpperCase();
  }
  getTelefone(obraConsulta: ObraConsulta): string {
    if(obraConsulta.clienteTelefoneDdd == 0 && obraConsulta.clienteTelefoneNumero == 0) return 'NÃO INFORMADO';
    return Tasks.formataTelefone(obraConsulta.clienteTelefoneDdd, obraConsulta.clienteTelefoneNumero);
  }
  getCelular(obraConsulta: ObraConsulta): string {
    if(obraConsulta.clienteCelularDdd == 0 && obraConsulta.clienteCelularNumero == 0) return 'NÃO INFORMADO';
    return Tasks.formataTelefone(obraConsulta.clienteCelularDdd, obraConsulta.clienteCelularNumero);
  }
  getTelefoneComercial(obraConsulta: ObraConsulta): string {
    if(obraConsulta.clienteTelefoneComercialDdd == 0 && obraConsulta.clienteTelefoneComercialNumero == 0) return 'NÃO INFORMADO';
    return Tasks.formataTelefone(obraConsulta.clienteTelefoneComercialDdd, obraConsulta.clienteTelefoneComercialNumero);
  }
  getVendedor(obraConsulta: ObraConsulta): string {
    return (obraConsulta.vendedorCodigo+' - '+obraConsulta.vendedorNome).toUpperCase();
  }
  getVolumeTotal(obraConsulta: ObraConsulta): string {
    return `${Tasks.formataValor(obraConsulta.volumeTotal, 1, false)} M3`;
  }
  getSalcoContasAReceber(obraConsulta: ObraConsulta): string {
    return `${Tasks.formataMoeda(obraConsulta.clienteSaldoContasAReceber)}${(obraConsulta.clienteSaldoContasAReceber < 0 ? ' (Crédito)' : '')}`;
  }
  getDataLimiteCredito(obraConsulta: ObraConsulta): string {
    if (obraConsulta.clienteLimiteData)
      return `${Tasks.formataData(obraConsulta.clienteLimiteData)}`;
    else
      return '';
  }

  getPlaceHolderValorProgramado(obraConsulta: ObraConsulta): string {
    var self = PropostaAprovacoesPageComponent.self;

    if (self.limiteCreditoPorGrupoEconomico && obraConsulta.grupoEconomicoCodigo != 0)
      return 'Valor Programado Previsto Grupo';

    return  'Valor Programado Previsto Cliente';
  }

  getCondicaoPagamento(obraConsulta: ObraConsulta): string {
    return (obraConsulta.condicaoPagamentoCodigo+' - '+obraConsulta.condicaoPagamentoDescricao).toUpperCase();
  }
  volumeTotalTracosContratado(): number {
    let total: number = 0;
    this.obra.obraTracos.forEach(item => {
      total += item.m3Quantidade;
    });

    return total;
  }
  volumeTotalTracosConsumido(): number {
    return this.volumeTotalConsumido;
  }

  obraEstaReprovado() {
    return this.aprovacaoComercialHierarquiaDireitos.obraEstaReprovado;
  }

  getAprovacaoRestanteString(item: AprovacaoComercialDadosItem ) {
    if(item.status == 0) {
      if(!this.obraEstaReprovado()) {
        return (item.quantidadeNotificacoes - item.quantidadeNotificacoesAprovadas) + " Restante(s)" ;
      }
    }
      return "";
  }

  getPropostaOuContratoString(): string {
    if (this.obra.numContrato == 0) return 'Proposta';
    return 'Contrato'
  }

  formataStatusAprovacao(status: EAprovacaoComercialPendenteStatus , smallScreen: boolean) {
    switch (status) {
      case EAprovacaoComercialPendenteStatus.AguardandoAprovacao:
        return "AG. APROVAÇÃO";
      case EAprovacaoComercialPendenteStatus.Aprovado:
        return "APROVADO";
      case EAprovacaoComercialPendenteStatus.Reprovado:
        return "REPROVADO";
      case EAprovacaoComercialPendenteStatus.Descartado:
        return "DESCARTADO";
      case EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior:
        return "AG. HIERARQUIA INFERIOR";
    }
  }

  _allColumns: TableColumn[] = [
    { name: 'interveniente', placeholder: 'Cliente', order: 1, priority: 1, getValue: this.getInterveniente },
    { name: 'contrato', placeholder: 'Contrato', order: 2, priority: 2, getValue: this.getContratoNumero },
    { name: 'clienteCpfCnpj', placeholder: 'CPF/CNPJ', order: 3, priority: 3, getValue: this.getCpfCnpj },
    { name: 'obra', placeholder: 'Obra', order: 4, priority: 4, getValue: this.getObraNome },
    { name: 'vendedor', placeholder: 'Vendedor', order: 5, priority: 5, getValue: this.getVendedor },
    { name: 'dataHoraProgramacao', placeholder: 'Data/Hora Prog.', order: 6, priority: 6, getValue: this.getDataHoraProgramacao },
    { name: 'usinaEntrega', placeholder: 'Usina Entrega', order: 7, priority: 7, getValue: this.getUsinaEntrega },
    { name: 'tipoCobranca', placeholder: 'Tipo Cobrança', order: 8, priority: 8, getValue: this.getTipoCobranca },
    { name: 'obraMunicipio', placeholder: 'Município da obra', order: 9, priority: 9 },
    { name: 'proposta', placeholder: 'Proposta', order: 10, priority: 10, getValue: this.getPropostaNumero },
    { name: 'obraContato', placeholder: 'Contato', order: 11, priority: 11 },
    { name: 'telefone', placeholder: 'Telefone', order: 12, priority: 12, getValue: this.getTelefone },
    { name: 'celular', placeholder: 'Celular', order: 13, priority: 13, getValue: this.getCelular },
    { name: 'telefoneComercial', placeholder: 'Tel.Comercial', order: 14, priority: 14, getValue: this.getTelefoneComercial },
    { name: 'contratoData', placeholder: 'Data Contrato', order: 15, priority: 15, formatter: this.dataContratoFormatter },
    { name: 'analista', placeholder: 'Analista', order: 16, priority: 16, getValue: this.getAnalista },
    { name: 'grupoEconomico', placeholder: 'Grupo Econômico', order: 17, priority: 17, getValue: this.getGrupoEconomico },
    { name: 'volumeTotal', placeholder: 'Volume Total', order: 18, priority: 18, getValue: this.getVolumeTotal },
    { name: 'contratoValorTotal', placeholder: 'Valor Total Contrato', order: 19, priority: 19, formatter: this.formataMoeda },
    { name: 'valorProgramado', placeholder: 'Valor Programado Previsto Contrato', order: 20, priority: 20, formatter: this.formataMoeda },
    { name: 'valorProgramadoCliente', placeholder: 'Valor Programado Previsto Cliente', order: 21, priority: 21, formatter: this.formataMoeda, getDynamicPlaceholder: this.getPlaceHolderValorProgramado },
    { name: 'clienteLimiteValor', placeholder: 'Limite Cadastrado', order: 22, priority: 22, formatter: this.formataMoeda },
    { name: 'clienteLimiteData', placeholder: 'Data Vencimento Limite', order: 23, priority: 23, getValue: this.getDataLimiteCredito },
    { name: 'clienteSaldoContasAReceber', placeholder: 'Saldo Contas a Receber', order: 24, priority: 24, getValue: this.getSalcoContasAReceber },
    { name: 'clienteValorTotalNfsNaoFaturadas', placeholder: 'Total NFs Não Faturadas', order: 25, priority: 25, formatter: this.formataMoeda },
    { name: 'limiteCreditoDisponivel', placeholder: 'Limite Disponível', order: 26, priority: 26, formatter: this.formataMoeda },
    { name: 'limiteCreditoSaldo', placeholder: 'Saldo Limite', order: 27, priority: 27, formatter: this.formataMoeda },
    { name: 'limiteCreditoAnalise', placeholder: 'Análise Limite de Crédito', order: 28, priority: 28 },
    { name: 'condicaoPagamento', placeholder: 'Condição de Pagamento', order: 29, priority: 29, getValue: this.getCondicaoPagamento }
  ];

  get allColumns(): TableColumn[] {
    if (this.filtro.analisarLimiteCredito) {
      var colunasLimitePrevisao = [
        'valorProgramado',
        'valorProgramadoCliente',
        'limiteCreditoSaldo'
      ];

      if (!this.filtro.analiseLimiteConsiderarPrevisao)
        return this._allColumns.filter(t => !colunasLimitePrevisao.includes(t.name));

      return this._allColumns;
    } else {
      var colunasLimiteCredito = [
        'valorProgramado',
        'valorProgramadoCliente',
        'clienteLimiteValor',
        'clienteLimiteData',
        'clienteSaldoContasAReceber',
        'clienteValorTotalNfsNaoFaturadas',
        'limiteCreditoDisponivel',
        'limiteCreditoSaldo',
        'limiteCreditoAnalise'
      ];
      return this._allColumns.filter(t => !colunasLimiteCredito.includes(t.name));
    }
  }

  hiddenColumns: string[] = [];
  isHiddenColumn(columnName: string): boolean {
    return this.hiddenColumns.includes(columnName);
  }
  setHiddenColumn(columnName: string, hidden: boolean) {
    this.setOrdenacaoInicial();

    if (hidden)
      this.hiddenColumns.push(columnName);
    else
      this.hiddenColumns = this.hiddenColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }

  get columns(): TableColumn[] {
    return this.allColumns.filter(t => !this.hiddenColumns.includes(t.name));
  }

  get currentViewValue() {
    return { filter: this.filtro, hiddenColumns: this.hiddenColumns, customColumnOrder: this._customColumnOrder }
  }

  expandedElement: Proposta | null;

  get fixedColumnsLeft(): string[] {
    return ['status'];
  }
  get fixedColumnsRight(): string[] {
    return ['expand'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = PropostaAprovacoesPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => {
      var fixedColsTotalWidth = 235;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 180);
      var hiddenColumnsHighPriority = this.allColumns.filter(c => this.hiddenColumns.includes(c.name) && this.getPriority(c) < this.getPriority(t)).length;

      return (this.getPriority(t) - hiddenColumnsHighPriority) <= colsAllowed;
    });
  }
  get columnNames(): string[] {
    return this.fixedColumnsLeft.concat(this.displayedColumns.map(t => t.name)).concat(this.fixedColumnsRight);
  }
  get foldedColumns(): TableColumn[] {
    var self = PropostaAprovacoesPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => !this.columnNames.includes(t.name));
  }

  private _customColumnOrder: string[] = [];
  getOrder(column: TableColumn): number {
    var customOrder = this._customColumnOrder.indexOf(column.name) + 1;
    if (customOrder) {
      return customOrder;
    } else {
      return column.order + this._customColumnOrder.length;
    }
  }
  getPriority(column: TableColumn): number {
    var orderDiff = this.getOrder(column) - column.order;
    var priorityDiff = column.order - column.priority;
    return column.priority + orderDiff + priorityDiff;
  }

  get allColumnsOrdered(): TableColumn[] {
    var self = PropostaAprovacoesPageComponent.self;

    return this.allColumns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    });
  }

  changeColumnOrder(columnName: string, increment: number): void {
    if (this._customColumnOrder.length === 0) {
      this._customColumnOrder = this.allColumns.sort((a, b) => {
        return a.order - b.order;
      }).map(t => t.name);
    } else if (this._customColumnOrder.length < this.allColumns.length) {
      this._customColumnOrder = this.allColumnsOrdered.map(t => t.name);
    }
    
    var index = this._customColumnOrder.indexOf(columnName);

    if (index < 0 || index >= this._customColumnOrder.length) return;
    if ((index+increment) < 0 || (index+increment) >= this._customColumnOrder.length) return;

    this._customColumnOrder.splice(index+increment, 0, this._customColumnOrder.splice(index, 1)[0]);
  }

  get dataSource(): ObraConsulta[] {
    return this.itens.records;
  };

  alterarOrdenacao(coluna: string){
    if (this.semOrdenacao[coluna]) {
      this.ordenacaoCrescente[coluna] = true;
      this.semOrdenacao[coluna] = false;
    } else if (this.ordenacaoCrescente[coluna]) {
      this.ordenacaoDecrescente[coluna] = true;
      this.ordenacaoCrescente[coluna] = false;
    } else if (this.ordenacaoDecrescente[coluna]) {
      this.semOrdenacao[coluna] = true;
      this.ordenacaoDecrescente[coluna] = false;
    }

    this.geraFiltroOrdenacao(coluna);

    this.getPage();    
  }

  geraFiltroOrdenacao(coluna: string) {
    if (this.semOrdenacao[coluna]) {
      this.ordenacao = this.ordenacao.replace(`${coluna} ASC,`, '').replace(`${coluna} DESC,`, '');
    }

    if (this.ordenacaoCrescente[coluna]) {
      this.ordenacao += `${coluna} ASC,`;
    }

    if (this.ordenacaoDecrescente[coluna]) {
      this.ordenacao = this.ordenacao.replace(`${coluna} ASC,`, '');

      this.ordenacao += `${coluna} DESC,`;
    }
  }

  setOrdenacaoInicial() {
    this.ordenacao = "";
    this.semOrdenacao = [];
    this._allColumns.forEach(column => {
      this.semOrdenacao[column.name] = true;
    });
    
    this.ordenacaoCrescente = [];
    this._allColumns.forEach(column => {
      this.ordenacaoCrescente[column.name] = false;
    });

    this.ordenacaoDecrescente = [];
    this._allColumns.forEach(column => {
      this.ordenacaoDecrescente[column.name] = false;
    });
  }

  horaValidatorDe(hora: string): ICustomValidator {
    var _tasks = Tasks;

    var message = 'Informe um horário válido';

    return {
      key: 'horarioInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        if (this.analiseLimitePrevisao()) {
          let hour = hora.substring(0, 2);
          let minutes = hora.substring(2);
        
          if (hour === '23' && minutes === '59')
            return true;
        }

        return !_tasks.horarioValido(hora);
      },
      params: [hora]
    }
  }

  horaValidatorAte(hora: string): ICustomValidator {
    var _tasks = Tasks;

    var message = 'Informe um horário válido';

    return {
      key: 'horarioInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        if (this.analiseLimiteProducao()) {
          let hour = hora.substring(0, 2);
          let minutes = hora.substring(2);
        
          if (hour === '00' && minutes === '00')
            return true;
        }

        return !_tasks.horarioValido(hora);
      },
      params: [hora]
    }
  }

  dataMinima(): Date {
    if (this.analiseLimiteProducao() || !this.filtro.analisarLimiteCredito) return null;

    var _tasks = Tasks;
    return _tasks.dataAtual();
  }

  dataMaxima(): Date {
    if (this.analiseLimitePrevisao() || !this.filtro.analisarLimiteCredito) return null;
    
    var _tasks = Tasks;
    return _tasks.dataAtual();
  }

  analiseLimiteProducao(): boolean {
    if (!this.filtro.analisarLimiteCredito) return false;

    if (!this.filtro.analiseLimiteConsiderarPrevisao) this.filtro.programacaoDiasAposDataAtual = 0;
    return !this.filtro.analiseLimiteConsiderarPrevisao;
  }

  analiseLimitePrevisao(): boolean {
    if (!this.filtro.analisarLimiteCredito) return false;

    if (this.filtro.analiseLimiteConsiderarPrevisao) this.filtro.programacaoDiasAteDataAtual = 0;
    return this.filtro.analiseLimiteConsiderarPrevisao;
  }

  horarioPropriedades(horario: string) {
    var _horario = (horario || '').replace(':', '');
    if (_horario.length < 4) return null;
    var h = parseInt(_horario.substring(0, _horario.length - 2));
    var m = parseInt(_horario.substring(_horario.length - 2, _horario.length));
    var mTotal = (h * 60) + m;

    return {
        horas: h,
        minutos: m,
        totalMinutos: mTotal
    }
  }

  horaMaxima(hora: string): ICustomValidator {
    var _tasks = Tasks;

    var message = 'Informe um horário menor que 00:00';

    return {
      key: 'horarioMaximo',
      message: message,
      validatorFunction: (hora: string): boolean => {
        var h = this.horarioPropriedades(hora); 
        return (h && h.horas == 0)
      },
      params: [hora]
    }
  }

  exibirAcompanhamento: boolean = false;

  temDireitoAcessoRetencaoContratual: boolean = false;
  temDireitoInsercaoRetencaoContratual: boolean = false;

  volumeTotalConsumido: number = 0;

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('cadastroModalVCR', { read: ViewContainerRef, static: false }) cadastroModalRef: ViewContainerRef;
  @ViewChild('comercialModalVCR', { read: ViewContainerRef, static: false }) comercialModalRef: ViewContainerRef;
  @ViewChild('engenhariaModalVCR', { read: ViewContainerRef, static: false }) engenhariaModalRef: ViewContainerRef;
  @ViewChild('financeiroModalVCR', { read: ViewContainerRef, static: false }) financeiroModalRef: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('ebitdaModalVCR', { read: ViewContainerRef, static: false }) ebitdaModalRef: ViewContainerRef;
  @ViewChild('anexosModalVCR', { read: ViewContainerRef, static: false }) anexosModalRef: ViewContainerRef;
  @ViewChild('descricaoAnexoModalVCR', { read: ViewContainerRef, static: false }) descricaoAnexoModalRef: ViewContainerRef;
  
  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _propostaService: PropostaService,
    private _programacaoService: ProgramacaoService,
    private _funcionarioService: FuncionarioService,
    private _intervenienteService: IntervenienteService,
    private _vendedorService: VendedorService,
    private _segmentacaoService: SegmentacaoService,
    private _obraService: ObraService,
    private _contratoService: ContratoService,
    private _pagamentoService: PagamentoService,
    private _usinaService: UsinaService,
    private _cadastroGeralService: CadastroGeralService,
    private _parametroService: ParametroService,
    private _custoServicoService: CustoServicoService,
    private _tracoPrecoService: TracoPrecoService,
    private _filialService: FilialService,
    private _cadastroDiversoService: CadastroDiversoService,
    private _aprovacaoComercialService: AprovacaoComercialService,
    private _acessoAprovacoesCondifService: AcessoAprovacoesConfigService,
    private _grupoEconomicoService: GrupoEconomicoService
  ) {
    PropostaAprovacoesPageComponent.self = this;

    var temLiberacaoAcesso = this._acessoAprovacoesCondifService.TemLiberacaoAcesso(50);
    if (!temLiberacaoAcesso) return;


    var temDireito = this._userService.temDireitoAplicativo('WEB6156','', 200);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Comercial", 6156);

    this._vendedorService.listarPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores; },
      error => { this.vendedoresPermitidos = []; }
    );

    this._segmentacaoService.listarTodos().then(
      segmentacoes => { this.segmentacao = segmentacoes; },
      error => { this.segmentacao = []; }
    );

    this._parametroService.obterParametoN('web','AnaliseLimiteCreditoPorTerceiros').then(
      response => {
        this.temAnaliseCreditoPorTerceiros = response === "1"
      }
    )

    this._parametroService.obterParametoN('web','InativarAprovacoesFinanceiras').then(
      response => {
        this.inativarAprovacoesFinanceiras = response === "1"
      }
    )

    this._parametroService.obterParametoN('web', 'LimiteCreditoPorGrupoEconomico').then(
      response => {
        this.limiteCreditoPorGrupoEconomico = response === "1"
      }
    )

    this._pagamentoService.ListarTiposCobranca().then(
      tiposCobranca => {
        this.tiposCobranca = tiposCobranca.sort((a, b) => {
          return a.descricao.localeCompare(b.descricao);
        });
      },
      error => { this.tiposCobranca = []; }
    );

    this._pagamentoService.ListarCartaoBandeiras().then(
      cartaoBandeiras => {
        this.cartaoBandeiras = cartaoBandeiras.sort((a, b) => {
          return a.descricao.localeCompare(b.descricao);
        });
      },
      error => { this.cartaoBandeiras = []; }
    );

    this._pagamentoService.ListarPortadoresVinculadosComContas().then(
      portadores => {
        this.portadores = portadores.sort((a, b) => {
          return a.descricao.localeCompare(b.descricao);
        });
      },
      error => { this.portadores = []; }
    );

    this._usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinas = usinas; },
      error => { this.usinas = []; }
    );

    this._cadastroGeralService.listarmotivosBloqueioInterveniente().then(
      motivos => {this.motivosBloqueioInterveniente = motivos;},
      error => {this.motivosBloqueioInterveniente = [];}
    )

    this._funcionarioService.ListarAnalistas().then(
      analistas => {this.analistas = analistas;},
      error => {this.analistas = [];}
    )

    this._parametroService.obterParametoN("TopCon","GeraFaturaNaPesagem").then(
      parametro => { this.geraFaturaNaPesagem = parametro==="1"},
      error => {this.geraFaturaNaPesagem = false}
    );

    _filialService.listar().then(
      lista => { this.filiais = lista; },
      error => { this.filiais = []; }
    );

    _contratoService.ListarFinalidades().then(
      lista => { this.finalidades = lista},
      error => { this.finalidades = [] }
    );

    _cadastroDiversoService.listarModeloDocumentoRemessaConcreto().then(
      modelos => { modelos.forEach((modelo) => { 
        var newModelo = new ModeloDocumentoRemessaConcreto();
        newModelo.codigo = parseInt(modelo.codigo);
        newModelo.descricao = modelo.descricao;
        this.modelosDocumentosRemessa.push(newModelo); 
      }) },
      error => { this.modelosDocumentosRemessa = []; }
    );

    modelosDanfeRomaneio.forEach(modelo => {      
      this.modelosItensDanfeERomaneio.push(modelo);
    });

    this.temDireitoAprovarCadastro = this._userService.temDireitoAplicativo('WEB6118','');

    this.temDireitoAcessoRetencaoContratual = this._userService.temDireitoAplicativo('WEB6005','');
    this.temDireitoInsercaoRetencaoContratual = this._userService.temDireitoAplicativo('WEB6005','I');
  }

  ngOnInit() {
    this.cadastroForm = this._formBuilder.group({
      cadastroFaturamentoPendenteForm: ['']
    });
    this.engenhariaForm = this._formBuilder.group({});
    this.financeiroForm = this._formBuilder.group({});
    this.comercialForm = this._formBuilder.group({});
    this.anexoForm = this._formBuilder.group({});
    this.anexoDescricaoForm = this._formBuilder.group({})

    this.setOrdenacaoInicial();
  }
  ngAfterViewInit(): void {
    
  }

  setFilter(newFilter) {
    //this.filtro = newFilter;
    Object.keys(newFilter).forEach(t => this.filtro[t] = newFilter[t]);

    if (this.filtro.programacaoDataDe) this.filtro.programacaoDataDe = new Date(this.filtro.programacaoDataDe);
    if (this.filtro.programacaoDataAte) this.filtro.programacaoDataAte = new Date(this.filtro.programacaoDataAte);
    if (this.filtro.periodoDe) this.filtro.periodoDe = new Date(this.filtro.periodoDe);
    if (this.filtro.periodoAte) this.filtro.periodoAte = new Date(this.filtro.periodoAte);
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };

    this._obraService.ConsultarObras(currentPage, pageSize, this.filtroString, this.ordenacao)
    .then(
      obras => {
        this.itens = obras;
        this.paginaAtual = obras.currentPage;
        this.registrosPorPagina = obras.pageSize;
      },
      error => { this.itens = new PagedList<ObraConsulta>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  private _timeoutGrupoEconomicoPorDescricao = null;
  filtrarGrupoEconomicoPorDescricao(grupoEconomico: string) {
    this.filtro.grupoEconomicoDescricao = grupoEconomico;

    var tamanhoMinimo = (isNaN(parseInt(grupoEconomico)) ? 3 : 0);

    if (grupoEconomico && grupoEconomico.length>tamanhoMinimo && (!this.filtro.grupoEconomico || this.filtro.grupoEconomico.descricao!=grupoEconomico)) {
      
      if (this._timeoutGrupoEconomicoPorDescricao) clearTimeout(this._timeoutGrupoEconomicoPorDescricao);
      
      var filtro = 'filter=$(' + (isNaN(parseInt(grupoEconomico)) ? 'descricao==' + grupoEconomico : 'codigo==' + parseInt(grupoEconomico)) + ')';

      this._timeoutGrupoEconomicoPorDescricao = setTimeout( () => {
        this._grupoEconomicoService.Listar(null, null, filtro)
          .then(
            gruposEconomicos => { this.gruposEconomicos = gruposEconomicos.records; },
            error => { this.gruposEconomicos = []; }
          )
      }, 500);

    } else {
      this.gruposEconomicos = [];
    }
  }

  statusAprov = {};
  getBtnAprovacaoClass(obraConsulta: ObraConsulta, tipoAprovacao: string) {
    var possibleStatus = ['aprovado', 'reprovado', 'aguardando-aprovacao', 'nao-necessita-aprovacao'];
    
    var key = `${this.getPropostaNumero(obraConsulta)}-${tipoAprovacao}`;
    this.statusAprov[key] = this.statusAprov[key] || possibleStatus[Math.floor(Math.random() * 1000) % 4];

    return [
      'btn-aprovacao',
      this.statusAprov[key]
    ]
  }

  getFormattedValue(element: Proposta, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  getFormattedPlaceholder(element: ObraConsulta, column: TableColumn) {
    return column.getDynamicPlaceholder ? column.getDynamicPlaceholder(element) : column.placeholder;
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
  }

  private _timeoutIntervenientesPorRazao = null;
  filtrarIntervenientesPorRazao(cliente: string) {
    this.filtro.intervenienteRazao = cliente;

    var tamanhoMinimo = (isNaN(parseInt(cliente)) ? 3 : 0);

    if (cliente && cliente.length>tamanhoMinimo && (!this.filtro.interveniente || this.filtro.interveniente.razao!=cliente)) {
      
      if (this._timeoutIntervenientesPorRazao) clearTimeout(this._timeoutIntervenientesPorRazao);
      
      var filtro = 'filter=$(' + (isNaN(parseInt(cliente)) ? 'razao%=' + cliente : 'codigo==' + parseInt(cliente));

      if (this.filtro.grupoEconomico)
        filtro += ';grupoEconomicoCodigo==' + this.filtro.grupoEconomico.codigo;

      filtro += ')';

      this._timeoutIntervenientesPorRazao = setTimeout( () => {
        this._intervenienteService.listarFiltrados(filtro, true)
          .then(
            intervenientes => {
              this.intervenientes = intervenientes.sort((a, b) => a.razao.localeCompare(b.razao));
            },
            error => { this.intervenientes = []; }
          )
      }, 500);

    } else {
      this.intervenientes = [];
    }
  }

  openObraLog(obraConsulta: ObraConsulta) {
    let self = this;
    let o = obraConsulta;
    this._obraService.ListarLogs(o.usina, o.obraNumero, o.propostaAno, o.propostaNumero, o.contratoAno, o.contratoNumero)
    .then(
      obraLogs => {
        self._dialog.open(ObraLogDialogComponent, {
          data: {
            obraLogs: obraLogs
          }
        });
      },
      err => {
        self.alert(Tasks.formataErrosApi(err));
      }
    );
  }

  async showModal(content, container: ViewContainerRef, obraConsulta: ObraConsulta) {
    let minWidthContainer = this.isSmallScreen ? "95%" : "";
    this.obra = null;
    this.openedElement = obraConsulta;
    this._dialogRef = this._dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer});
    this.modalIsOpen = true;
    this.tracosPreco = [];

    let response = null;

    if (container == this.comercialModalRef) {
      this.statusAlcadaAguardandoAprovacaoOutroNivel = obraConsulta.statusAprovComAlcada === ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel || false;
      response = this._obraService.ObterPendenteAprovacaoComercialPorId(obraConsulta);
    } else if (container == this.cadastroModalRef) {
      response = this._obraService.ObterObraParaValidacaoCadastro(obraConsulta);
    } else if (container == this.engenhariaModalRef) {
      response = this._obraService.ListarObraTracos(obraConsulta); 
    } else if (container == this.financeiroModalRef) {
      response = this._obraService.ListarObraPagamentos(obraConsulta);
    }

    if (response) {
      response.then(obra => {
        this.obra = obra;

        if (container == this.engenhariaModalRef) {
        this.volumeTotalConsumido = 0;

        this.obra.obraTracos.forEach(traco => {
          traco.mpa = traco.fck;
          this._obraService.ObterConsumoPorTraco(this.obra, traco, this.obra.contrato.interveniente.codigo).then(volumeConsumido => {
            traco.m3Consumido = volumeConsumido;
            this.volumeTotalConsumido += volumeConsumido;
          });
        });}
      }).then(() => {
        this._cdr.detectChanges();
      });
    }

    if (container == this.comercialModalRef) {

      this._aprovacaoComercialService.ListarAprovacoes(obraConsulta).then(aprovacaoComercialAprovacoesRestantes =>{ 
        this.aprovacaoComercialAprovacoesRestantes = aprovacaoComercialAprovacoesRestantes
      }).then(() => {
        this._cdr.detectChanges();
      });
      
      this._aprovacaoComercialService.ListarDireitosPorUsuario(obraConsulta.usina, obraConsulta.obraNumero).then(aprovComercialHierarquiaDireito => {
        this.aprovacaoComercialHierarquiaDireitos = aprovComercialHierarquiaDireito
      }).then(() => {
        this._cdr.detectChanges();
      })

      let obra = await this._obraService.ObterPendenteAprovacaoComercialPorId(obraConsulta);      
      this.obra = obra; 

      this.obra.endereco = {
        municipio: {
          codigo: obra['enderecoMunicipio'].codigo as number
        } as Municipio
      } as Endereco;
      this.tracosExibicaoEbitda = await Promise.all(this.obra.obraTracos.map(t => this._obraService.CalcularEbitdaObraTraco(t, this.obra)));      
           
      response = this._tracoPrecoService.ListarPrecosAtuaisPorObra(obraConsulta.usina, obraConsulta.obraNumero, obraConsulta.contratoNumero, obraConsulta.contratoAno)
      .then(tracos => {
        this.tracosPreco = tracos;
      }).then(() => {
        this._cdr.detectChanges();
      });
    }

    if (container == this.ebitdaModalRef) {
      let obra = await this._obraService.ObterPendenteAprovacaoComercialPorId(obraConsulta);      
      this.obra = obra;    
      this.obra.endereco = {
        municipio: {
          codigo: obra['enderecoMunicipio'].codigo as number
        } as Municipio
      } as Endereco;    
      
      this.custoServico = await this._custoServicoService.ObterCustoServicoVigentePorUsina(this.obra.usinaEntrega.codigo);    
      this.tracosExibicaoEbitda = await Promise.all(this.obra.obraTracos.map(t => this._obraService.CalcularEbitdaObraTraco(t, this.obra)));  

      this._cdr.detectChanges(); 
      //this._obraService.ObterPendenteAprovacaoComercialPorId(obraConsulta).then(obra => {
      //  this.obra = obra;
      //}).then(() => {
      //  this._custoServicoService.ObterCustoServicoVigentePorUsina(this.obra.usinaEntrega.codigo).then( custoServico => {
      //    this.custoServico = custoServico;
      //  }).then(() => {
      //    this._cdr.detectChanges();
      //  });
      //});
    }

    if (container == this.anexosModalRef){
      this.anexos = await this._intervenienteService.listarAnexos(obraConsulta.clienteCodigo, obraConsulta.propostaAno, obraConsulta.propostaNumero);
      this._cdr.detectChanges();  
    }

  }

  closeModal() {
    let self = PropostaAprovacoesPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.cadastroForm.markAsPristine();
    self.cadastroForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  showSelecaoColunasModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  showModalDescricaoAnexo(content, container: ViewContainerRef, anexo: IntervenienteAnexo) {
    let self = PropostaAprovacoesPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self.anexo = anexo;
    self.descricaoAnteriorAnexo = anexo.descricao;

    self._subDialogRef = self._dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  closeSubModal() {
    let self = PropostaAprovacoesPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }


  cancelSubModal() {
    let self = PropostaAprovacoesPageComponent.self;

    self.closeSubModal();
    self.anexo.descricao = self.descricaoAnteriorAnexo;
  }

  alert(message, title?, afterCloseCallback?) {
    return this._dialog.open(AlertDialogComponent, {
      data: {
        title: (title || 'TopConWeb'),
        message: message,
        afterCloseCallback: afterCloseCallback
      }
    });
  }

  pagamentoAprovado(item: Pagamento): boolean {
    return (item.idAprovacao !== "");
  }
  isPagamentoInativo(pagamento: Pagamento): boolean {
    return (pagamento.ativoSimNao === 'N');
  }
  naoNecessitaAprovacao(item: any): boolean {
    return (item.statusAprovacao === this.statusAprovacao.naoNecessita);
  }
  pendenteDeAprovacao(item: any, tipo: string = ''): boolean {
    if(this.aprovacaoComercialHierarquiaDireitos && (tipo === "Bomba" || tipo === "Traco")) {
      if(this.aprovacaoComercialHierarquiaDireitos.utilizaAprovacaoComercialAlcada) {
        if(tipo === "Bomba") {
    
          var possuiPermissao = this.possuiPermissaoParaAprovacaoBomba(item.sequencia);
          return (item.statusAprovacao === this.statusAprovacao.pendente) && possuiPermissao;

        } else if(tipo === "Traco") {
          
          var possuiPermissao = this.possuiPermissaoParaAprovacaoTraco(item.sequencia);
          return (item.statusAprovacao === this.statusAprovacao.pendente) && possuiPermissao;

        } else if(tipo === "Volume") {

          var direitoVolume = this.aprovacaoComercialHierarquiaDireitos.direitoVolume;
          return direitoVolume;

        }
      }
    }

    return (item.statusAprovacao === this.statusAprovacao.pendente);
  }
  pendenteAprovacaoCoincidenciasCadastro(contrato : Contrato): boolean {
    return contrato.aguardandoAprovacao === 'S' && contrato.status !== EStatusContrato.Aprovado && contrato.status !== EStatusContrato.Reprovado && contrato.status !== EStatusContrato.Cancelado
  }
  pendenteAprovacaoEngenharia(obra: Obra): boolean {
    return (obra.contrato.aprovaEngenharia === 'S' && obra.contrato.idAprovacaoEngenharia === '');
  }
  get temDireitoAprovacaoEngenharia(): boolean{
    return this._userService.temDireitoAplicativo('WEB6149',''); 
  }
  get temDireitoAcessoMargemPosTransporte(): boolean{
    return this._userService.temDireitoAplicativo('WEB6103', '');
  }
  get getCoincidencias(): string[] {
    if (!this.obra.contrato.descricaoCoincidencia) return [];
    return Tasks.replaceAll(this.obra.contrato.descricaoCoincidencia,"#","").split(/\r?\n/);
  }
  get getIconAtencaoCadastro(): string {
      return "warning";
  }
  get getColorAtencaoCadastro(): string {
    return "#ffc800";
  }
  limiteCreditoIcon(obra: Obra): string {
    if(this.possuiLimiteDeCreditoVencido(obra)) 
      return `cancel`;
    else if(this.possuiLimiteDeCreditoVazioEPagamentoFuturo(obra))
      return `warning`;
    else
      return ``;
  }

  limiteCreditoColor(obra: Obra): string {
  if(this.possuiLimiteDeCreditoVencido(obra)) 
    return `#FF8072`;
  else if(this.possuiLimiteDeCreditoVazioEPagamentoFuturo(obra))
    return `#ffc800`;
  else
    return ``;
  }
  get temDireitoAprovacaoFinanceira(): boolean {
    return this._userService.temDireitoAplicativo('WEB6309','');  
  }
  temDireitoAprovacaoPagamento(pagamento: Pagamento): boolean {
    switch (pagamento.tipoCobranca.forma) {
      case 'BE':
        return this._userService.temDireitoAplicativo('CON6980','');
      case 'DN':
        return this._userService.temDireitoAplicativo('CON6981','');
      case 'DP':
        return this._userService.temDireitoAplicativo('CON6982','');
      case 'CC':
        return this._userService.temDireitoAplicativo('CON6983','');
      case 'CD':
        return this._userService.temDireitoAplicativo('CON6984','');
      case 'CH':
        return this._userService.temDireitoAplicativo('CON6985','');
      case 'CP':
        return this._userService.temDireitoAplicativo('CON6986','');
      case 'CT':
        return this._userService.temDireitoAplicativo('CON6987','');
      default:
        return false;
    }
  }
  get temDireitoAprovacaoComercial(): boolean {
    return this._userService.temDireitoAplicativo('WEB6998','');  
  }
  get temDireitoAlteracaoDistancia(): boolean {
    return this._userService.temDireitoAplicativo('CON6157','');  
  }
  get temDireitoAprovacaoReprovacaoCoincidencias(): boolean {
    return this._userService.temDireitoAplicativo('WEB6268','');  
  }
  get temDireitoAprovacaoIss(): boolean {
    return this._userService.temDireitoAplicativo('WEB6272','');  
  }
  get temDireitoAprovacaoLimiteCredito(): boolean {
    return this._userService.temDireitoAplicativo('CON0028','') && this._userService.temDireitoAplicativo('CON0002','');  
  }
  get temDireitoAlteracaoStatusCadastro(): boolean {
    return this._userService.temDireitoAplicativo('WEB6127','') || this._userService.temDireitoAplicativo('WEB6118','');  
  }
  get temDireitoAlteracaoNfPendentes(): boolean {
    return this._userService.temDireitoAplicativo('CON6295','');  
  }
  pendenteAprovacaoISSCadastro(interveniente: Interveniente) : boolean {
    return interveniente.retemIss === 'N' && interveniente.idAprovacaoRetencaoIss === '' && interveniente.intervenienteTipo !== intervenienteTipos[0].codigo
  }
  pendenteAprovacaoZMRCCadastro(obra: Obra) : boolean {  
    return obra.necessitaAprovacaoZMRC === 'S'
  }
  pendeteAprovacaoLimiteCreditoCadastro(obra: Obra) : boolean {
    return this.possuiLimiteDeCreditoVencido(obra) || this.possuiLimiteDeCreditoVazioEPagamentoFuturo(obra); 
  }
  aprovarCoincidenciaCadastro(obra: Obra, panel: MatExpansionPanel) {
    this.fecharPainel(panel);

    this._contratoService.AprovaCoincidenciasCadastrais(obra.contrato)
    .then( rs => {
      var obraConsulta = new ObraConsulta();
      obraConsulta.obraNumero = obra.numero;
      obraConsulta.usina = obra.usinaCodigo;
      this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
      .then(obra => {
        this.obra = obra;
      }).then(() => {
        this.alert('Aprovação efetuada com sucesso!');
        this._cdr.detectChanges();
      })}
     );
  }

  fecharPainel(panel: MatExpansionPanel) {
    panel.close();
  }
  
  reprovarCoincidenciaCadastro(obra: Obra, panel: MatExpansionPanel) {
    this.fecharPainel(panel);

    this._contratoService.ReprovaCoincidenciasCadastrais(obra.contrato)
    .then( rs => {
      var obraConsulta = new ObraConsulta();
      obraConsulta.obraNumero = obra.numero;
      obraConsulta.usina = obra.usinaCodigo;
      this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
      .then(obra => {
        this.obra = obra;
      }).then(() => {
        this.alert('Reprovação efetuada com sucesso!');
        this._cdr.detectChanges();
      })}
     );
  }
  aprovarIssCadastro(obra: Obra, panel: MatExpansionPanel) {
    this.fecharPainel(panel);

    this._intervenienteService.aprovarIss(obra.contrato.interveniente)
    .then( rs => {
      var obraConsulta = new ObraConsulta();
      obraConsulta.obraNumero = obra.numero;
      obraConsulta.usina = obra.usinaCodigo;
      this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
      .then(obra => {
        this.obra = obra;
      }).then(() => {
        this.alert('Aprovação efetuada com sucesso!');
        this._cdr.detectChanges();
      })}
     );
  }
  aprovarZMRC(obra: Obra, panel: MatExpansionPanel) {
    this.fecharPainel(panel);
    
    this._obraService.aprovarZMRC(obra)
    .then( rs => {
      var obraConsulta = new ObraConsulta();
      obraConsulta.obraNumero = obra.numero;
      obraConsulta.usina = obra.usinaCodigo;
      this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
      .then(obra => {
        this.obra = obra;
      }).then(() => {
        this.alert('Aprovação efetuada com sucesso!');
        this._cdr.detectChanges();
      })}
     );
  }
  reprovarZMRC(obra: Obra, panel: MatExpansionPanel) {
    this.fecharPainel(panel);

    this._obraService.reprovarZMRC(obra)
    .then( rs => {
      var obraConsulta = new ObraConsulta();
      obraConsulta.obraNumero = obra.numero;
      obraConsulta.usina = obra.usinaCodigo;
      this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
      .then(obra => {
        this.obra = obra;
      }).then(() => {
        this.alert('Reprovação efetuada com sucesso!');
        this._cdr.detectChanges();
      })}
     );
  }
  aprovarDistanciaUsinaCepCadastro(obra: Obra, panel: MatExpansionPanel) {
    this.fecharPainel(panel);

    this._obraService.aprovarDistanciaUsinaCep(obra)
    .then( rs => {
      var obraConsulta = new ObraConsulta();
      obraConsulta.obraNumero = obra.numero;
      obraConsulta.usina = obra.usinaCodigo;
      this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
      .then(obra => {
        this.obra = obra;
      }).then(() => {
        this.alert('Aprovação efetuada com sucesso!');
        this._cdr.detectChanges();
      })}
     );
  }
  confirmarAprovacaoEngenharia(){
      this._obraService.AprovarEngenharia(this.obra)
      .then(res => {
        this.alert('Aprovação efetuada com sucesso!');
        this.closeModal();
        this.filter.aplyFilter();
      }, err => {
        this.closeModal();
        if(err != "Unauthorized"){
          this.alert(Tasks.formataErrosApi(err));
        }
      });
    }
  possuiLimiteDeCreditoVencido(obra: Obra): boolean {
    if (this.limiteCreditoPorGrupoEconomico && obra.contrato.interveniente.grupoEconomicoCodigo != 0)
      return (new Date(obra.contrato.interveniente.grupoEconomico.limiteData) < Tasks.dataAtual() && obra.contrato.interveniente.grupoEconomico.limiteData !== null)
      || (!obra.contrato.interveniente.grupoEconomico.limiteValor && obra.contrato.interveniente.grupoEconomico.limiteData !== null);

    return (new Date(obra.contrato.interveniente.limiteData) < Tasks.dataAtual() && obra.contrato.interveniente.limiteData !== null)
    || (!obra.contrato.interveniente.limiteValor && obra.contrato.interveniente.limiteData !== null);
  }
  possuiLimiteDeCreditoVazioEPagamentoFuturo(obra: Obra): boolean {
    //Pagamento Futuro: quando não é antecipado, tipocobranca.aprovacao === N

    if (this.limiteCreditoPorGrupoEconomico && obra.contrato.interveniente.grupoEconomicoCodigo != 0)
      return (obra.tipoCobranca.aprovacao === 'N'
        && obra.contrato.interveniente.grupoEconomico.limiteValor === 0) || (!obra.contrato.interveniente.grupoEconomico.limiteValor && obra.contrato.interveniente.grupoEconomico.limiteData === null);
    
    return (obra.tipoCobranca.aprovacao === 'N'
    && obra.contrato.interveniente.limiteValor === 0) || (!obra.contrato.interveniente.limiteValor && obra.contrato.interveniente.limiteData === null);
  }
  confirmarAlteracaoLimiteCredito(obra: Obra, panel: MatExpansionPanel){
    this.fecharPainel(panel);

    if (this.limiteCreditoPorGrupoEconomico && obra.contrato.interveniente.grupoEconomicoCodigo != 0) {
      if(new Date(obra.contrato.interveniente.grupoEconomico.limiteData) < Tasks.dataAtual() && obra.contrato.interveniente.grupoEconomico.limiteData !== null){
        this.alert('Alteração não Permitida! Data do Limite Credito Vencida');
        return;
      }

      this._grupoEconomicoService.Atualizar(obra.contrato.interveniente.grupoEconomico)
      .then(rs => {
        var obraConsulta = new ObraConsulta();
        obraConsulta.obraNumero = obra.numero;
        obraConsulta.usina = obra.usinaCodigo;
        this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
        .then(obra => {
          this.obra = obra;
        }).then(() => {
          this.alert('Alteração efetuada com sucesso!');
          this._cdr.detectChanges();
        })
      });
    } else {
      if(new Date(obra.contrato.interveniente.limiteData) < Tasks.dataAtual() && obra.contrato.interveniente.limiteData !== null){
        this.alert('Alteração não Permitida! Data do Limite Credito Vencida');
        return;
      }

      this._intervenienteService.alterarLimiteCredito(obra.contrato.interveniente)
      .then( rs => {
        var obraConsulta = new ObraConsulta();
        obraConsulta.obraNumero = obra.numero;
        obraConsulta.usina = obra.usinaCodigo;
        this._obraService.ObterObraParaValidacaoCadastro(obraConsulta)
        .then(obra => {
          this.obra = obra;
        }).then(() => {
          this.alert('Alteração efetuada com sucesso!');
          this._cdr.detectChanges();
        })}
      );
    }
  }
  confirmarAlteracaoStatusCadastroEAnalista(obra: Obra){
    if (obra.necessitaAprovacaoZMRC =="S") {
      this.alert('Ainda há itens pendentes de aprovação! Resolva todos antes de confirmar.');
      return;
    }

    this._obraService.AlterarStatusCadastroEAnalista(obra)
    .then(res => {
      this.alert('Alteração efetuada com sucesso!');
      this.closeModal();
      this.filter.aplyFilter();
    }, err => {
      this.closeModal();
      if(err != "Unauthorized"){
        this.alert(Tasks.formataErrosApi(err));
      }
    });
  }
  confirmarAlteracaoDadosFiscais(obra: Obra, panel: MatExpansionPanel){
    this.fecharPainel(panel);

    this._obraService.AlterarDadosFiscais(obra)
    .then(res => {
      this.alert('Dados fiscais salvos com sucesso!');
    }, err => {
      this.alert(Tasks.formataErrosApi(err));
    });
  }

  popupObservacao(item: any) {
    this.itemNovosValores = this.itemNovosValores || {};
    this.itemNovosValores.logObservacao = item.logObservacao || '';
    item.logObservacao = prompt("Informe uma observação para o vendedor:", item.logObservacao);
  }

  itemComercialEmAlteracao: any = null;
  itemNovosValores: any = {};

  isItemComercialEmAlteracao(item: any): boolean {
    return this.itemComercialEmAlteracao === item;
  }

  alterarItemComercial(item: any) {
    if (this.isItemComercialEmAlteracao(item)) {
      this.itemComercialEmAlteracao = null;
    } else {
      item.proposto = item.proposto || {};
      this.itemComercialEmAlteracao = item;
      this.itemNovosValores = {
        m3PropostoAte: item.m3PropostoAte || null,
        taxaMinimaPrecoProposto: item.taxaMinimaPrecoProposto || null,
        m3PrecoProposto: item.m3PrecoProposto || null
      };
    }
  }
  
  confirmarAlteracaoItemComercial(item: any, novosValores: any, panel: MatExpansionPanel) {
    item.proposto = item.proposto || {};
    if (
      (item.m3PropostoAte != (novosValores.m3PropostoAte || null)) ||
      (item.taxaMinimaPrecoProposto != (novosValores.taxaMinimaPrecoProposto || null)) ||
      (item.m3PrecoProposto != (novosValores.m3PrecoProposto || null))
    ) {
      item.statusAprovacao = this.statusAprovacao.alterado;
      item.m3PropostoAte = novosValores.m3PropostoAte || null;
      item.taxaMinimaPrecoProposto = novosValores.taxaMinimaPrecoProposto || null;
      item.m3PrecoProposto = novosValores.m3PrecoProposto || null;
      this.popupObservacao(item);
    }
    this.alterarItemComercial(item);

    this.fecharPainel(panel);
  }

  aprovarItemComercial(item: any, panel: MatExpansionPanel) {
    
    if (item.statusAprovacao == this.statusAprovacao.aprovado) {
      item.statusAprovacao = this.statusAprovacao.pendente;
    } else if (item.statusAprovacao == this.statusAprovacao.alterado) {
      this.alert('Não Permitido! Item já foi alterado!');
    } else {
      item.statusAprovacao = this.statusAprovacao.aprovado;

      if(!this.utilizaAprovacaoComercialPorAlcada()) {
        this.popupObservacao(item);
      }
      
    }

    this.fecharPainel(panel);
  }

  aprovarVolumeComercial() {
    
    this.obra.volumeStatusComercial = EObraDemaisStatusComercial.Aprovado;

  }

  aprovarCondicaoPagamentoComercial() {
    
    this.obra.condicaoPagamentoStatusComercial = EObraDemaisStatusComercial.Aprovado;

  }

  reprovarVolumeComercial() {

    this.obra.volumeStatusComercial = EObraDemaisStatusComercial.Reprovado;

  }

  reprovarCondicaoPagamentoComercial() {

    this.obra.condicaoPagamentoStatusComercial = EObraDemaisStatusComercial.Reprovado;

  }

  reprovarItemComercial(item: any, panel: MatExpansionPanel) {
    if (item.statusAprovacao == this.statusAprovacao.reprovado) {
      item.statusAprovacao = this.statusAprovacao.pendente;
    } else if (item.statusAprovacao == this.statusAprovacao.alterado) {
      this.alert('Não Permitido! Item já foi alterado!');
    } else {
      item.statusAprovacao = this.statusAprovacao.reprovado;
      this.popupObservacao(item);
    }

    this.fecharPainel(panel);
  }

  temPendenciaComercial(): boolean {
    return (
      this.obra.obraTracos.filter(t => this.pendenteDeAprovacao(t, "Traco")).length > 0 ||
      this.obra.obraBombas.filter(t => this.pendenteDeAprovacao(t, "Bomba")).length > 0 ||
      this.obra.obraTaxas.filter(t => this.pendenteDeAprovacao(t)).length > 0 ||
      ((this.obra.volumeStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao ||
      this.obra.volumeStatusComercial == EObraDemaisStatusComercial.Reprovado) && this.aprovacaoComercialHierarquiaDireitos.direitoVolume) ||
      ((this.obra.condicaoPagamentoStatusComercial == EObraDemaisStatusComercial.AguardandoAprovacao ||
        this.obra.condicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Reprovado) && this.aprovacaoComercialHierarquiaDireitos.direitoCondicaoPagamento) ||
      this.obra['demaisAprovacoes'].filter(t => this.pendenteDeAprovacao(t)).length > 0
    );
  }

  confirmarAprovacaoComercial() {
    if (this.temPendenciaComercial()) {
      this.alert('Ainda há itens pendentes de aprovação! Resolva todos antes de confirmar.');
      return;
    }

    this._obraService.AprovarComercialmente(this.obra)
    .then(res => {
      this.closeModal();
      this.filter.aplyFilter();
      
      if((this.openedElement.contratoNumero == 0) && this.openedElement.statusCadastro == EStatusCadastro.Aprovado) {
        this._contratoService.GerarContratoPelaObraAposAprovacaoComercial(this.obra)
        .then(res => {
          this.filter.aplyFilter();
        }, err => { })
      }
    }, err => {
      this.closeModal();
      if(err != "Unauthorized"){
        this.alert(Tasks.formataErrosApi(err));
      }
    })
  }

  aprovarItemFinanceiro(item: any, panel: MatExpansionPanel) {
    if (item.statusAprovacao === this.statusAprovacao.aprovado) {
      item.statusAprovacao = this.statusAprovacao.pendente;
    } else {
      item.statusAprovacao = this.statusAprovacao.aprovado;
    }

    this.fecharPainel(panel);
  }
  reprovarItemFinanceiro(item: any, panel: MatExpansionPanel) {
    if (item.statusAprovacao == this.statusAprovacao.reprovado) {
      item.statusAprovacao = this.statusAprovacao.pendente;
    } else {
      item.statusAprovacao = this.statusAprovacao.reprovado;
      this.popupObservacao(item);
    }

    this.fecharPainel(panel);
  }
  desaprovarItemFinanceiro(obra: Obra, item: any) {
    var self = PropostaAprovacoesPageComponent.self;

    this._pagamentoService.DesaprovarPagamento(obra, item, true)
      .then(res => {
        this.closeModal();
        this.alert('Pagamento desaprovado com sucesso!', '', () => self.filter.aplyFilter());
      }, err => {
        if (err.errors.map(t => t.key).includes('MovimentoBancarioConciliado')) {
         this.confirmacaoDesaprovarItemFinanceiro(Tasks.formataErrosApi(err), obra, item);
        } else {
          this.alert(Tasks.formataErrosApi(err));
        }
      });
  }
  confirmacaoDesaprovarItemFinanceiro(message: string, obra: Obra, item: any) {
    this._dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      data: {
        title: 'TopConWeb',
        message: message,
        confirmCallback: () => {
          this._pagamentoService.DesaprovarPagamento(obra, item, false)
          .then(res => {
            this.closeModal();
            this.filter.aplyFilter();
          }, err => {
            this.alert(Tasks.formataErrosApi(err));
          });
        }
      }
    });
  }

  temPendenciaFinanceiro(): boolean {
    return (
      this.obra.obraPagamentos.filter(t => this.pendenteDeAprovacao(t)).length > 0
    );
  }

  confirmarAprovacaoFinanceiro(movimentoBancoAVincular?: MovimentoBanco[]) {
    var self = PropostaAprovacoesPageComponent.self;
    
    self._obraService.AprovarObraPagamentos(self.obra, movimentoBancoAVincular)
    .then(res => {
      self.closeModal();
      self.alert('Aprovação realizada com sucesso!', '', () => self.filter.aplyFilter());
    }, err => {
      if (err.errors && err.errors.length == 1 && err.errors[0].key.endsWith('-DataContaConciliada')) {
        self._dialog.open(ConfirmDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: Tasks.formataErrosApi(err),
            confirmCallback: () => {
              var pagamento = self.obra.obraPagamentos.find(t => t['statusAprovacao'] === EStatusFinanceiro.Aprovado);
              var detalhe = <PagamentoDetalheDeposito>pagamento.detalhes[0];

              self._dialog.open(MovimentosBancoAVincularDialogComponent, {
                data: {
                  conta: detalhe.portador.conta,
                  valorAVincular: detalhe.valor,
                  dataOperacao: detalhe.dataDeposito,
                  pagamentoService: self._pagamentoService,
                  confirmCallback: (movimentosAvincular: MovimentoBanco[]) => {
                    self.confirmarAprovacaoFinanceiro(movimentosAvincular);
                  }
                }
              });
            }
          }
        });
      } else {
        this.closeModal();
        if(err != "Unauthorized"){
          this.alert(Tasks.formataErrosApi(err));
        }
      }
    });
  }

  calculaDesconto(valorProposto, valorTabela) {
    return (1-(valorProposto / valorTabela))*100;
  };

  retornaDescontoAcrescimoAbs(descontoAcrescimo: number){
    return Math.abs(descontoAcrescimo)
  }
  
  precoUltimoContratoReajuste(traco: ObraTraco): number {
    var contratoReajuste = this.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste)[0];
    if (!contratoReajuste) return 0;
    return contratoReajuste.precoRecalculado
  }

  precoTabelaAtualTraco(traco: ObraTraco): number {
    var tracoPreco = this.tracosPreco.filter(t => t.uso.codigo === traco.uso.codigo && t.slump.codigo === traco.slump.codigo
         && t.pedra.codigo === traco.pedra.codigo && t.resistenciaTipo.codigo === traco.resistenciaTipo.codigo && t.mpa === traco.mpa && t.consumo === traco.consumo )[0];
    if (!tracoPreco) return 0;
    return tracoPreco.m3Preco;
  }

  custoServicoTabelaAtualTraco(traco: ObraTraco): number {
    var tracoPreco = this.tracosPreco.filter(t => t.uso.codigo === traco.uso.codigo && t.slump.codigo === traco.slump.codigo
         && t.pedra.codigo === traco.pedra.codigo && t.resistenciaTipo.codigo === traco.resistenciaTipo.codigo && t.mpa === traco.mpa && t.consumo === traco.consumo )[0];
    if (!tracoPreco) return 0;
    return tracoPreco.m3Preco- tracoPreco.custoMaterial;
  }

  custoMaterialTabelaAtualTraco(traco: ObraTraco): number {
    var tracoPreco = this.tracosPreco.filter(t => t.uso.codigo === traco.uso.codigo && t.slump.codigo === traco.slump.codigo
         && t.pedra.codigo === traco.pedra.codigo && t.resistenciaTipo.codigo === traco.resistenciaTipo.codigo && t.mpa === traco.mpa && t.consumo === traco.consumo )[0];
    if (!tracoPreco) return 0;
    return tracoPreco.custoMaterial;
  }

  possuiContratoReajuste(traco: ObraTraco): boolean {
    return this.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste).length > 0;
  }

  valorServicoUltimoContratoReajuste(traco: ObraTraco): number {
    var contratoReajuste = this.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste)[0];
    if (!contratoReajuste) return 0;
    return contratoReajuste.valorServicoRecalculado
  }

  EbitdaTotalObra(): number {
    let volumeXEbitda: number = 0;
    this.tracosExibicaoEbitda.forEach(item => {
      volumeXEbitda += item.ebitda * item.m3Quantidade;
    });

    var mediaBomba = this.obra.obraBombas.map(t => t.ebitda).reduce((a, b) => a+b, 0) / this.obra.obraBombas.map(t => t.ebitda).reduce((a) => a+ 1, 0);
    var volume = this.tracosExibicaoEbitda.map(t => t.m3Quantidade).reduce((a, b) => a+b, 0)

    if (mediaBomba > 0) {
      return ((volumeXEbitda + mediaBomba) / volume);
    }

    return (volumeXEbitda / volume);
  }

  itemAprovacaoFinanceiraColor(item) {
    switch (item.statusAprovacao) {
      case this.statusAprovacao.pendente:
        return this.dadosStatusFinanceiro(EStatusFinanceiro.AguardandoConfirmacao).color;
      case this.statusAprovacao.aprovado:
        return this.dadosStatusFinanceiro(EStatusFinanceiro.Aprovado).color;
      // case this.statusAprovacao.reprovado:
      //   return this.dadosStatusFinanceiro(EStatusFinanceiro.Reprovado).color;
      default:
        return '';
    }
  }

  itemAprovacaoComercialColor(item) {
    switch (item.statusAprovacao) {
      case this.statusAprovacao.pendente:
        return this.dadosStatusComercial(EStatusComercial.Aguardando).color;
      case this.statusAprovacao.aprovado:
        return this.dadosStatusComercial(EStatusComercial.Aprovado).color;
      case this.statusAprovacao.reprovado:
        return this.dadosStatusComercial(EStatusComercial.Reprovado).color;
      case this.statusAprovacao.alterado:
        return this.dadosStatusComercial(EStatusComercial.Aguardando).color;
      default:
        return '';
    }
  }

  itemAprovacaoIcon(item) {
    switch (item.statusAprovacao) {
      case this.statusAprovacao.pendente:
        return `warning`;
      case this.statusAprovacao.aprovado:
        return `check_circle`;
      case this.statusAprovacao.reprovado:
        return `cancel`;
      case this.statusAprovacao.alterado:
        return `edit`;
      default:
        return '';
    }
  }

  volumeAprovacaoIcon() {
    switch (this.obra.volumeStatusComercial) {
      case EObraDemaisStatusComercial.AguardandoAprovacao:
        return `warning`;
      case EObraDemaisStatusComercial.Aprovado:
        return `check_circle`;
      case EObraDemaisStatusComercial.Reprovado:
        return `cancel`;
      default:
        return '';
    }
  }

  condicaoPagamentoAprovacaoIcon() {
    switch (this.obra.condicaoPagamentoStatusComercial) {
      case EObraDemaisStatusComercial.AguardandoAprovacao:
        return `warning`;
      case EObraDemaisStatusComercial.Aprovado:
        return `check_circle`;
      case EObraDemaisStatusComercial.Reprovado:
        return `cancel`;
      default:
        return '';
    }
  }

  volumeAprovacaoColor() {
    switch (this.obra.volumeStatusComercial) {
      case EObraDemaisStatusComercial.AguardandoAprovacao:
        return this.dadosStatusComercial(EStatusComercial.Aguardando).color;
      case EObraDemaisStatusComercial.Aprovado:
        return this.dadosStatusComercial(EStatusComercial.Aprovado).color;
      case EObraDemaisStatusComercial.Reprovado:
        return this.dadosStatusComercial(EStatusComercial.Reprovado).color;
      default:
        return '';
    }
  }

  condicaoPagamentoAprovacaoColor() {
    switch (this.obra.condicaoPagamentoStatusComercial) {
      case EObraDemaisStatusComercial.AguardandoAprovacao:
        return this.dadosStatusComercial(EStatusComercial.Aguardando).color;
      case EObraDemaisStatusComercial.Aprovado:
        return this.dadosStatusComercial(EStatusComercial.Aprovado).color;
      case EObraDemaisStatusComercial.Reprovado:
        return this.dadosStatusComercial(EStatusComercial.Reprovado).color;
      default:
        return '';
    }
  }

  bombaString(bomba): string {
    if (!bomba.bombaTipo) return this.intervenienteFormatter(bomba.terceiro) || 'BOMBA DE TERCEIRO';
    return bomba.bombaTipo.descricao;
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  volumeSemAprovacao() : boolean { 
    return !(this.obra.volumeStatusComercial == EObraDemaisStatusComercial.Aprovado);
  }

  condicaoPagamentoSemAprovacao() : boolean { 
    return !(this.obra.condicaoPagamentoStatusComercial == EObraDemaisStatusComercial.Aprovado);
  }

  viewChanged(view: ICustomView) {
    if (!this.filter) return;
    this.setFilter(view.value ? view.value.filter : this.filter.defaultModel);
    this.hiddenColumns = view.value && view.value.hiddenColumns ? view.value.hiddenColumns : [];
    this._customColumnOrder = view.value && view.value.customColumnOrder ? view.value.customColumnOrder : [];
    this._cdr.detectChanges();
    this.filter.aplyFilter();
  }
  openObraProgramacoes(_obra: ObraConsulta) {
    var self = PropostaAprovacoesPageComponent.self;
    self._dialog.open(PropostaProgramacoesDialogComponent, {
      data: {
        propostaAno: _obra.propostaAno,
        propostaNumero: _obra.propostaNumero
      }
    });
  }
  openHistoricoInterveniente(_obra: ObraConsulta) {
    var self = PropostaAprovacoesPageComponent.self;
    self._dialog.open(HistoricoIntervenienteDialogComponent, {
      data: {
        intervenienteCodigo: _obra.clienteCodigo
      }
    });
  }

  changeCodigoObraPrefeitura(newValue: string) {
    var valorAnterior = this.obra.contrato.codigoObraPrefeitura;
    this.obra.contrato.codigoObraPrefeitura = newValue;
    if (valorAnterior !== newValue) {
      var usinaDefault = this.obra.obraTributacoesMunicipais.filter(t => t.usinaEntregaCodigo === 0);
      
      if (usinaDefault.length > 0) {
        usinaDefault.forEach(t => {
          t.codigoObraPrefeitura = newValue;
        });
      } else {
        var newUsinaDefault = new ObraTributacaoMunicipal();
        newUsinaDefault.codigoObraPrefeitura = newValue;
        this.obra.obraTributacoesMunicipais.push(newUsinaDefault);
      }
    }
  }

  openTributacoesMunicipaisModal() {
    this._dialog.open(ObraTributacoesMunicipaisDialogComponent, {
      data: {
        disabled: !this.temDireitoAlteracaoStatusCadastro,
        tributacoesMunicipais: JSON.parse(JSON.stringify(this.obra.obraTributacoesMunicipais)),
        usinas: this.usinas,
        confirmTributacoesMunicipaisCallback: this.updateTributacoesMunicipais
      }
    });
  }

  updateTributacoesMunicipais(tributacoesMunicipais: ObraTributacaoMunicipal[]): boolean {
    let self = PropostaAprovacoesPageComponent.self;

    var usinaDuplicada = false;

    tributacoesMunicipais.forEach(t => {
      var repeticoes = tributacoesMunicipais.filter(u => u.usinaEntregaCodigo === t.usinaEntregaCodigo).length;
      if (repeticoes > 1) usinaDuplicada = true

      if (!usinaDuplicada && t.usinaEntregaCodigo === 0) {
        self.obra.contrato.codigoObraPrefeitura = t.codigoObraPrefeitura;
      }
    });

    if (usinaDuplicada) {
      self.alert('Usina duplicada!');
      return false;
    }
    
    self.obra.obraTributacoesMunicipais = tributacoesMunicipais;
    return true;
  }

  allowDocumentoDiferenteRemessa(): boolean {
    var saida = false;
    this.filiais.forEach((filial) => {
      if(filial.permiteDocumentoDiferentePadraoRemessa === 'S') { saida = true; }
    });
    return saida;
  }

  allowDocumentoDiferenteBomba(): boolean {
    var saida = false;
    this.filiais.forEach(filial => {
      if(filial.permiteDocumentoDiferentePadraoBomba === 'S') { saida =  true; }
    });
    return saida;
  }

  modeloDocumentoRemessaFormatter = (model: number) : string => {
    if(this.modelosDocumentos.length === 0) return '';
    return this.modelosDocumentos.includes(model) ? this.modelosDocumentosRemessa.filter(e => e.codigo === model)[0].descricao : '';
  };

  get modelosDocumentos(): number[] {
    let codigos: number[] = [];
    this.modelosDocumentosRemessa.forEach(modelo => {
      codigos.push(modelo.codigo);
    })
    return codigos;
  };

  descricaoItensdanfeERomaneioFormatter = (model: number) : string => {
    if (this.modelosDescricaoItensDanfeERomaneio.includes(model)) {
      return this.modelosItensDanfeERomaneio.filter(e => e.codigo === model)[0].descricao;
    }
    return '';
  };

  get modelosDescricaoItensDanfeERomaneio() : number[] {
    return this.modelosItensDanfeERomaneio.map(modelo => modelo.codigo);
  }

  getTipoAprovacaoObraTaxa(obraTaxa: ObraTaxa): string {
    return obraTaxa.selecionada == 'S' ? 'PERSONALIZADA' : 'EXCLUÍDA';
  }

  possuiPermissaoParaAprovacaoTraco(sequenciaTraco: number): boolean {

    if(!this.aprovacaoComercialHierarquiaDireitos)
      return false;
    
    if(this.aprovacaoComercialHierarquiaDireitos.utilizaAprovacaoComercialAlcada === false)
      return true;

    var direitoTraco = this.aprovacaoComercialHierarquiaDireitos.direitoTracos.filter(e => e === sequenciaTraco).length > 0;

    return direitoTraco;

  }

  utilizaAprovacaoComercialPorAlcada(): boolean {

    if(!this.aprovacaoComercialHierarquiaDireitos)
      return false;
  
    if(this.aprovacaoComercialHierarquiaDireitos.utilizaAprovacaoComercialAlcada === false)
      return false;

    return true;

  }

  possuiPermissaoParaAprovacaoBomba(sequenciaBomba: number): boolean {
    
    if(!this.aprovacaoComercialHierarquiaDireitos)
      return false;
    
    if(this.aprovacaoComercialHierarquiaDireitos.utilizaAprovacaoComercialAlcada === false)
      return true;

    var direitoBomba = this.aprovacaoComercialHierarquiaDireitos.direitoBombas.filter(e => e === sequenciaBomba).length > 0;

    return direitoBomba;

  }

  filtrarTracosAprovacaoComercialPendente(aprovacoes: AprovacaoComercialDadosItem[], sequenciaTraco: number) {
    return aprovacoes.filter(x => x.sequencia == sequenciaTraco);
  }

  filtrarBombasAprovacaoComercialPendente(aprovacoes: AprovacaoComercialDadosItem[], sequenciaBomba: number) {
    return aprovacoes.filter(x => x.sequencia == sequenciaBomba);
  }

  tracoPossuiAprovacaoComercialPendente(sequenciaTraco: number) {
    return this.aprovacaoComercialAprovacoesRestantes.tracos.filter(x => x.sequencia == sequenciaTraco).length > 0;
  }

  bombaPossuiAprovacaoComercialPendente(sequenciaBomba: number) {
    return this.aprovacaoComercialAprovacoesRestantes.bombas.filter(x => x.sequencia == sequenciaBomba).length > 0;
  }

  volumePossuiAprovacaoComercialPendente() {
    return this.aprovacaoComercialAprovacoesRestantes.volumes.length > 0;
  }

  condicaoPagamentoPossuiAprovacaoComercialPendente() {
    return this.aprovacaoComercialAprovacoesRestantes.condicoesPagamento.length > 0;
  }

  getVolumeTotalContrato() {

    return this.obra.obraTracos.reduce((sum, current) => sum + current.m3Quantidade, 0);

  }

  getCondicaoPagamentoContrato() {

    return `${this.obra.condicaoPagamento.codigo} - ${this.obra.condicaoPagamento.descricao}`.toUpperCase();

  }
  
  temContratoGerado(obraConsulta: ObraConsulta): boolean{
    if (!obraConsulta.contratoNumero) return false;
    return true;
  }

  calculaMargemPosMCC(obraTraco: ObraTraco): number {
    if (obraTraco === null || obraTraco === undefined) return 0;
    var valorServicoAtual = obraTraco.custoServicoReajustado === 0 ? obraTraco.valorServico : obraTraco.custoServicoReajustado;
    return valorServicoAtual - obraTraco.totalImpostos;
  }

  get temDireitoAcessoAnexo(): boolean{
    return this._userService.temDireitoAplicativo('CON0036', '');
  }

  abrirSeletorDeArquivos(inputFile: HTMLInputElement) {
    let self = PropostaAprovacoesPageComponent.self;
    var temDireito = self._userService.temDireitoAplicativo('CON0036','I');
    if (!temDireito) {
     self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para inserir Anexos!`
        }
      });
      return;
    }

    inputFile.click();
  }

  arquivoSelecionado(event: Event) {
    let self = PropostaAprovacoesPageComponent.self;
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onloadend = async () => {
        const base64String = reader.result as string;
        try {
          await self._intervenienteService.adicionarAnexo(base64String,self.openedElement.clienteCodigo, self.openedElement.propostaAno, self.openedElement.propostaNumero, file.name);
          self.anexos = await self._intervenienteService.listarAnexos(self.openedElement.clienteCodigo, self.openedElement.propostaAno, self.openedElement.propostaNumero);
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Anexo inserido com sucesso!'
            }
          });
        } catch (err) {
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Erro ao inserir o Anexo.\n${err}`
            }
          });
        }
      };
      
      reader.readAsDataURL(file)
    }
  }

  abrirAnexo(anexo: IntervenienteAnexo) {
    let self = PropostaAprovacoesPageComponent.self;

    self._intervenienteService.ObterAnexo(anexo)
    .then(url => {
      var type = url.split(';')[0];
      type = type.replace("data:", "");
      var arquivo = url.split(',')[1]
      Tasks.openBase64File(arquivo, anexo.nome, type)
    }).catch(error => {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Erro ao obter o Anexo: ${JSON.stringify(error.exceptionMessage)}`
        }
      });
      return;
  });
  }

  atualizarDescricaoAnexo(anexo: IntervenienteAnexo): void {
    let self = PropostaAprovacoesPageComponent.self;

    var temDireito = self._userService.temDireitoAplicativo('CON0036','A');
    if (!temDireito) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para alterar Anexos!`
        }
      });
      return;
    }

    self._intervenienteService.atualizarDescricaoAnexo(anexo)
     .then(success => {
      self.closeSubModal();
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Descrição alterada com sucesso!`
        }
      });
     }, err => {
      self.anexo.descricao = self.descricaoAnteriorAnexo;
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro alterar a descrição.\n${JSON.stringify(err.exceptionMessage)}`
        }
      });
     });
  }

  removerAnexo(anexo: IntervenienteAnexo) {
    let self = PropostaAprovacoesPageComponent.self;

    var temDireito = self._userService.temDireitoAplicativo('CON0036','E');
    if (!temDireito) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para excluir Anexos!`
        }
      });
      return;
    }

    self._intervenienteService.removerAnexo(anexo)
      .then(success => {
        if (success) self.anexos = self.anexos.filter(a => a !== anexo);
      }, err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
  }

  aguardandoOutroNivelChange(ativo: any){
    let self = PropostaAprovacoesPageComponent.self;

    if(ativo){
      self.filtro.statusComercialIn = [];
    }
  }



}
