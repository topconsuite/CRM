import { Component, OnInit, ViewChild, ViewContainerRef, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

// **** CLASSES ***
import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Proposta, Status, statusProposta, statusComercial ,statusContrato,
  EStatusComercial } from 'app/classes/proposta/proposta.classes';
import { Obra, ObraTraco } from 'app/classes/obra/obra.classes';
import { Contrato, EContratoFinalidade, EStatusContrato, exibicaoContratos } from 'app/classes/contrato/contrato';
import { Interveniente} from 'app/classes/interveniente/interveniente';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { ParametroProposta } from 'app/classes/parametro/parametro';
import { ContratoVersao } from 'app/classes/contrato/contrato-versao';
import { BoletoExterno } from 'app/classes/contrato/contrato-boleto-externo';
import { Usina } from 'app/classes/usina/usina';
import { IntervenienteAnexo } from 'app/classes/interveniente/interveniente-anexo';
import { ObraProjecao } from 'app/classes/obra/obra-projecao';
import { ICustomValidator } from '../../../../components/interfaces/custom-validator';
// ******************************************************************

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { PropostaService } from 'app/services/proposta.service';
import { ProgramacaoService } from 'app/services/programacao.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { VendedorService } from 'app/services/vendedor.service';
import { ObraService } from 'app/services/obra.service';
import { ContratoService } from 'app/services/contrato.service';
import { BoletoExternoService } from 'app/services/boleto-externo.service';
import { ParametroService } from 'app/services/parametro.service';
import { CustoServicoService } from 'app/services/custo-servico.service';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { ObraProjecaoService } from 'app/services/obra-projecao.service';
// ******************************************************************

// **** COMPONENTS ***
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { ObraLogDialogComponent } from 'app/main/components/dialog/obra-log-dialog/obra-log-dialog.component';
import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { MovimentosBancoAVincularDialogComponent } from 'app/main/components/dialog/movimentos-banco-avincular-dialog/movimentos-banco-avincular-dialog.component';
import { SolicitacaoAssinaturaClicksign } from 'app/classes/assinatura-eletronica/solicitacao-assinatura-clicksing';
import { SolicitacaoAssinaturaEletronicaDialogComponent } from 'app/main/components/dialog/solicitacao-assinatura-eletronica-dialog/solicitacao-assinatura-eletronica-dialog/solicitacao-assinatura-eletronica-dialog.component';
import { AssinaturaEletronicaService } from 'app/services/assinatura-eletronica.service';
import { EStatusClicksignDocumento, statusClicksignDocumento } from 'app/classes/assinatura-eletronica/contrato-clicksing-envio';
import { EStatusCadastro } from 'app/classes/obra/obra';
import { statuProjecao } from 'app/classes/obra/obra';
import { now, over } from 'lodash';
import { Endereco, Municipio } from 'app/classes/endereco/endereco';
import { UsinaService } from 'app/services/usina.service';
import { GrupoEconomico } from 'app/classes/grupo-economico/grupo-economico';
import { GrupoEconomicoService } from 'app/services/grupo-economico.service';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { PropostaReportPDF } from 'app/classes/proposta/proposta-report';
import { CDK_CONNECTED_OVERLAY_SCROLL_STRATEGY_PROVIDER } from '@angular/cdk/overlay/typings/overlay-directives';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';
// ******************************************************************

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

export interface EdicaoObraProjecao {  
  periodo: Date;
  volume: number;
}

const LOADING_MESSAGE: string = '[CARREGANDO...]';

@Component({
  selector: 'app-contrato-lista-page',
  templateUrl: './contrato-lista-page.component.html',
  styleUrls: ['./contrato-lista-page.component.scss']
})
export class ContratoListaPageComponent implements OnInit, AfterViewInit {
  public static self: ContratoListaPageComponent;

  cadastroForm: FormGroup;
  contratoVersaoForm: FormGroup;
  impressaoBoletosForm: FormGroup;
  obraProjecaoForm: FormGroup;

  cadObraProjecao: EdicaoObraProjecao;
  cadObraProjecaoNovo: boolean;
  cadObraProjecaoSelecionado: ObraProjecao;
  cadObraProjecaoVolumeAnterior: number;
  cadObraProjecaoPeriodoAnterior: Date = new Date();

  saldoProjecao : number;

  propostaSelecionada : Proposta;
  propostas: PagedList<Proposta> = new PagedList<Proposta>();
  proposta: Proposta;

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  intervenientes: Interveniente[] = [];
  gruposEconomicos: GrupoEconomico[] = [];
  vendedoresPermitidos: Vendedor[] = [];
  segmentacao: Segmentacao[] = [];
  finalidades: ContratoFinalidade[] = [];

  contratoVersoes: ContratoVersao[] = [];
  versaoSelecionada: ContratoVersao;

  boletosExternos: BoletoExterno[] = [];
  boletoSelecionado: BoletoExterno;

  propostaReportSequencia: PropostaReportPDF[] = [];
  propostaReportSelecionada: PropostaReportPDF;

  versaoAtual: number = 0;

  obra: Obra;
  tracosExibicaoEbitda: ObraTraco[] = [];
  custoServico: CustoServico;
  tracoExibEbitda: ObraTraco = new ObraTraco();

  usinas: Usina[] = [];

  parametroProposta: ParametroProposta = new ParametroProposta();  

  possuiIntegracaoAssinaturaEletronica: boolean = false;

  statusPropostaFiltro: number = 0;

  exibicaoContratosFiltro: number = 0;

  divergenciaCarteiraFiltro: boolean = false;

  statusClicksignDocumento: number = null;

  geraAditivoContratoSemAprovCadastro: boolean = false;

  emiteBoletoExterno: boolean = false;

  anexoForm: FormGroup;
  anexoDescricaoForm: FormGroup;
  anexos: IntervenienteAnexo[] = [];
  anexo: IntervenienteAnexo;
  descricaoAnteriorAnexo: string = '';
  obraProjecoes: ObraProjecao[] = [];
  obraProjecaoConsumo: number;
  obraProjecaoVolume: number;
  obraProjecaoSaldo: number;
  obraProjecaoPrevisaoSaldo: number;
  obraProjecaoProximoPeriodo: Date;

  contratoVersaoFormatter = (model: ContratoVersao): string => model ? (' Versão ' + model.numeroVersao).toUpperCase() : '';

  impressaoBoletosFormatter = (model: BoletoExterno): string => {
    if (!model)
      return '';

    if (!model.dataHora)
      return model.nomeArquivo;

    const data = new Date(model.dataHora);
    const dataFormatada = data.toLocaleDateString('pt-BR', {
      year: '2-digit', // aa
      month: '2-digit', // mm
      day: '2-digit' // dd
    });
    const horaFormatada = data.toLocaleTimeString('pt-BR', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false // Formato 24h
    });

    return `${model.nomeArquivo} - ${dataFormatada} ${horaFormatada}`;
  };

  formataTelefone = Tasks.formataTelefone;
  formataData = Tasks.formataData;
  formataDataHora = Tasks.formataDataHora;
  formataMoeda = Tasks.formataMoeda;
  formataValor = Tasks.formataValor;

  displayedColumnsObraProjecao: string[] = ['projecao-volume','projecao-periodo','projecao-saldo', 'projecao-edicao'];
 
  get statusContrato(): number[] {
    let codigos: number[] = [];
    statusContrato.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos.filter(t => t > 0 && t !== EStatusContrato.NaoGerado);
  };

  get usinasCodigos(): number[] {
    let codigos: number[] = [];
    this.usinas.forEach(t => {
      codigos.push(t.codigo);
    });
    
    return codigos;
  }

  get obterCustoServico(): CustoServico {
    return this.custoServico;
  }

  get obraProjecoesAtual(): ObraProjecao[] {;
    const dataAtual = new Date();
    
    dataAtual.setDate(1);
    dataAtual.setHours(0, 0, 0, 0);

    return this.obraProjecoes.filter(obra => {
      const dataPeriodo = new Date(obra.periodo);
      return dataPeriodo >= dataAtual;
    });
  }

  get statusComercial(): number[] {
    let codigos: number[] = [];
    statusComercial.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos.filter(t => t > 0);
  };

  get statusClicksignDocumentoLista(): number[] {
    let codigos: number[] = [];
    statusClicksignDocumento.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos.filter(t => t < EStatusClicksignDocumento.Cancelado);
  };

  get statusProposta(): number[] {
    let codigos: number[] = [];
    statusProposta.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos;
  };

  get exibicaoContratos(): number[] {
    let codigos: number[] = [];
    exibicaoContratos.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos;
  };

  filtroString: string = '';
  filtro: {
    tipoDocumento: number,
    cpfCnpj: string,
    ano: number,
    numero: number,
    statusContrato: number,
    usina: number,
    statusComercial: number,
    interveniente: Interveniente,
    intervenienteRazao: string,
    grupoEconomico: GrupoEconomico,
    grupoEconomicoDescricao: string,
    dataDe: Date,
    dataAte: Date,
    vendedor: Vendedor,
    contato: string,
    telefoneDdd: number,
    telefoneNumero: number,
    enderecoObra: string,
    anoContrato: number,
    numeroContrato: number,    
    numeroContratoAnterior: string,
    segmentacao: number,
    contratoFinalidade: number
  } = {
    tipoDocumento: 0,
    cpfCnpj: '',
    ano: 0,
    numero: 0,
    statusContrato: 0,
    usina: 0,
    statusComercial: 0,
    interveniente: null,
    intervenienteRazao: '',
    grupoEconomico: null,
    grupoEconomicoDescricao: '',
    dataDe: null,
    dataAte: null,
    vendedor: null,
    contato: '',
    telefoneDdd: 0,
    telefoneNumero: 0,
    enderecoObra: '',
    anoContrato: 0,
    numeroContrato: 0,
    numeroContratoAnterior: '',
    segmentacao: 0,
    contratoFinalidade: 0
  };

  temDireitoSolicitacaoAssinaturaEletronica: boolean = false;
  temDireitoAlteracaoProjecaoCarteiraMesAtual: boolean = false;

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  @ViewChild('ebitdaModalVCR', { read: ViewContainerRef, static: false }) ebitdaModalRef: ViewContainerRef;
  @ViewChild('contratoVersaoModalVCR', { read: ViewContainerRef, static: false }) contratoVersaoModalRef: ViewContainerRef;
  @ViewChild('impressaoBoletosModalVCR', { read: ViewContainerRef, static: false }) impressaoBoletosModalRef: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('anexosModalVCR', { read: ViewContainerRef, static: false }) anexosModalRef: ViewContainerRef;
  @ViewChild('descricaoAnexoModalVCR', { read: ViewContainerRef, static: false }) descricaoAnexoModalRef: ViewContainerRef;
  @ViewChild('projecaoObraModalVCR', { read: ViewContainerRef, static: false }) projecaoObraModalRef: ViewContainerRef;  
  @ViewChild('obraProjecaoCadastroModalVCR', { read: ViewContainerRef, static: false }) obraProjecaoCadastroModalRef: ViewContainerRef;  
  @ViewChild('modalVCR', { read: ViewContainerRef, static: false }) ModalRef: ViewContainerRef;

  constructor(
    public dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _custoServicoService: CustoServicoService,
    private _userService: UserService,
    private _propostaService: PropostaService,
    private _programacaoService: ProgramacaoService,
    private _intervenienteService: IntervenienteService,
    private _grupoEconomicoService: GrupoEconomicoService,
    private _vendedorService: VendedorService,
    private _obraService: ObraService,
    private _contratoService: ContratoService,
    private _boletoExternoService: BoletoExternoService,
    private _parametroService: ParametroService,
    private _router: Router,
    private _assinaturaEletronicaService: AssinaturaEletronicaService,
    private _usinaService: UsinaService,    
    private _segmentacaoService: SegmentacaoService,
    private _obraProjecaoService: ObraProjecaoService,
    private _formBuilder: FormBuilder
  ) {
    var temDireito = this._userService.temDireitoAplicativo('WEB6101','', 200);
    if (!temDireito) return;

    ContratoListaPageComponent.self = this;

    this._userService.gravarAcessoAplicacao("Comercial", 6101);

    this.temDireitoSolicitacaoAssinaturaEletronica = this._userService.temDireitoAplicativo('WEB6004','');
    this.temDireitoAlteracaoProjecaoCarteiraMesAtual = this._userService.temDireitoAplicativo('WEB6310', 'A')

    this._vendedorService.listarPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores; },
      error => { this.vendedoresPermitidos = []; }
    );

    this._segmentacaoService.listarTodos().then(
      segmentacoes => { this.segmentacao = segmentacoes; },
      error => { this.segmentacao = []; }
    );

    this._parametroService.obterParametroPropostaPorDataBase(new Date()).then(
      parametroProposta => { this.parametroProposta = parametroProposta },
      error => { this.parametroProposta = new ParametroProposta() }
    );

    this._parametroService.obterParametoN("web", "GeraAditivoContratoSemAprovCadastro").then(
      parametro => { this.geraAditivoContratoSemAprovCadastro = parametro==="1" },
      error => { this.geraAditivoContratoSemAprovCadastro = false }
    );

    this._parametroService.obterParametoN("Topcon", "EmiteBoletoExterno").then(
      parametro => { this.emiteBoletoExterno = parametro==="1" },
      error => { this.emiteBoletoExterno = false }
    );

    _assinaturaEletronicaService.VerificarIntegracaoConfigurada().then(
      resultado => { this.possuiIntegracaoAssinaturaEletronica = resultado; },
      error => { this.possuiIntegracaoAssinaturaEletronica = false }
    );

    _usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );

    _contratoService.ListarFinalidades().then(
      finalidades => { this.finalidades = finalidades; },
      error => { this.finalidades = [] }
    )
  }

  ngOnInit() {
    this.contratoVersaoForm = this._formBuilder.group({});
    this.impressaoBoletosForm = this._formBuilder.group({});
    this.anexoForm = this._formBuilder.group({});
    this.anexoDescricaoForm = this._formBuilder.group({});    
    this.obraProjecaoForm = this._formBuilder.group({});
  }
  ngAfterViewInit(): void {
    
  }

  detectChanges(dalay: number = 0) {
    if (dalay)
      setTimeout(() => { this._cdr.detectChanges(); }, dalay);
    else
      this._cdr.detectChanges();
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;

    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    }
    this._contratoService.ListarPropostasComContratoEmOrdemDecrescente(currentPage, pageSize, this.filtroString, this.statusPropostaFiltro, this.exibicaoContratosFiltro, this.divergenciaCarteiraFiltro, this.statusClicksignDocumento)
    .then(
      propostas => {
        this.propostas = propostas;
        this.paginaAtual = propostas.currentPage;
        this.registrosPorPagina = propostas.pageSize;
      },
      error => { this.propostas = new PagedList<Proposta>(); }
    );
  }

  currentPage() {
    if (this.propostas.currentPage <= 0) return 0;
    return this.propostas.currentPage - 1;
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    if (this.filtro.dataDe) this.filtro.dataDe = new Date(this.filtro.dataDe);
    if (this.filtro.dataAte) this.filtro.dataAte = new Date(this.filtro.dataAte);
    this.getPage();
  }

  private _timeoutIntervenientesPorRazao = null;
  filtrarIntervenientesPorRazao(cliente: string) {
    this.filtro.intervenienteRazao = cliente;

    var tamanhoMinimo = (isNaN(parseInt(cliente)) ? 3 : 0);

    if (cliente && cliente.length>tamanhoMinimo && (!this.filtro.interveniente || this.filtro.interveniente.razao!=cliente)) {
      
      if (this._timeoutIntervenientesPorRazao) clearTimeout(this._timeoutIntervenientesPorRazao);
      
      var filtro = 'filter=$(' + (isNaN(parseInt(cliente)) ? 'razao%=' + cliente : 'codigo==' + parseInt(cliente)) + ')';

      this._timeoutIntervenientesPorRazao = setTimeout( () => {
        this._intervenienteService.listarFiltrados(filtro, true)
          .then(
            intervenientes => { this.intervenientes = intervenientes; },
            error => { this.intervenientes = []; }
          )
      }, 500);

    } else {
      this.intervenientes = [];
    }
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

  quantidadeBombeada(): number {
    let volumeBombeavel: number = 0;
    this.obra.obraTracos.filter(t => t.slump.codigo >= 9).forEach(traco => {
      volumeBombeavel += traco.m3QuantidadeBombeada;
    });
    
    return volumeBombeavel;
  }
  
  bombaString(bomba): string {
    if (!bomba.bombaTipo) return this.intervenienteFormatter(bomba.terceiro) || 'BOMBA DE TERCEIRO';
    return bomba.bombaTipo.descricao;
  }

  async showModal(content, container: ViewContainerRef, proposta: Proposta) {
    let minWidthContainer = this.isSmallScreen ? "95%" : "";        
      this.propostaSelecionada = proposta;

    if (container == this.ebitdaModalRef) {       
      let obra = await  this._obraService.ObterPendenteAprovacaoComercialPorIdUsandoAObra(proposta.obra);      
      this.obra = obra;    
      this.obra.endereco = {
        municipio: {
          codigo: obra['enderecoMunicipio'].codigo as number
        } as Municipio
      } as Endereco;    
      
      this.custoServico = await this._custoServicoService.ObterCustoServicoVigentePorUsina(this.obra.usinaEntrega.codigo);    
      this.tracosExibicaoEbitda = await Promise.all(this.obra.obraTracos.map(t => this._obraService.CalcularEbitdaObraTraco(t, this.obra)));      
      this._cdr.detectChanges();      
    }
    
    if (container == this.contratoVersaoModalRef){
      this.versaoSelecionada = null;
      await this.carregaContratoVersoes();
    }

    if (container == this.impressaoBoletosModalRef){
      this.boletoSelecionado = null;
      await this.carregaBoletosExternos();
    }

    if (container == this.anexosModalRef){
      this.anexos = await this._intervenienteService.listarAnexos(proposta.intervenienteCodigo, proposta.ano, proposta.numero);
      this._cdr.detectChanges();  
    }

    if (container == this.projecaoObraModalRef){
      this.obraProjecoes = await this._obraService.ListarObraProjecao(proposta.obra.usinaCodigo, proposta.obra.numero, proposta.obra.anoChamada, proposta.obra.numChamada);            
      this.obraProjecaoConsumo = await this._obraService.obterConsumoPorContrato(proposta.obra); 
      this.obraProjecaoVolume = await this._obraService.obterVolumePorContrato(proposta.obra);
      this.obraProjecaoSaldo = await this._obraProjecaoService.obterSaldoObraProjecao(proposta.obra);
      this.obraProjecaoPrevisaoSaldo = await this._obraProjecaoService.obterPrevisaoSaldoObraProjecao(proposta.obra);      
      this.obraProjecaoProximoPeriodo = await this._obraProjecaoService.getProximoPeriodoPorContrato(proposta.obra);
    
      this._cdr.detectChanges();       
      
    }

    this._dialogRef = this.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer});  
    
    this.modalIsOpen = true;
  }


  showSelecaoColunasModal(content) {
    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  showModalDescricaoAnexo(content, container: ViewContainerRef, anexo: IntervenienteAnexo) {
    let self = ContratoListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self.anexo = anexo;
    self.descricaoAnteriorAnexo = anexo.descricao;

    self._subDialogRef = self.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  closeModal() {
    let self = ContratoListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;

  }

  closeSubModal() {
    let self = ContratoListaPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  cancelSubModal() {
    let self = ContratoListaPageComponent.self;

    self.closeSubModal();
    self.anexo.descricao = self.descricaoAnteriorAnexo;
  }

  alert(message, title?, afterCloseCallback?) {
    return this.dialog.open(AlertDialogComponent, {
      data: {
        title: (title || 'TopConWeb'),
        message: message,
        afterCloseCallback: afterCloseCallback
      }
    });
  }

  intervenienteFormatter = (model: Interveniente) => model ? model.codigo+' - '+model.razao.toUpperCase() : '';
  grupoEconomicoFormatter = (model: GrupoEconomico) => model ? model.codigo+' - '+model.descricao.toUpperCase() : '';
  vendedorFormatter = (model: Vendedor) => model ? model.codigo+' - '+model.nome.toUpperCase() : '';
  segmentacaoFormatter = (model: Segmentacao) => model ? model.id+' - '+model.nome.toUpperCase() : '';
  finalidadeFormatter = (model: ContratoFinalidade): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  statusPropostaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model) || model===0) return '';
    return statusProposta.filter(e => e.codigo===model)[0].descricao.toUpperCase();
  };

  exibicaoContratosFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model) || model===0) return '';
    return exibicaoContratos.filter(e => e.codigo===model)[0].descricao.toUpperCase();
  };

  imprimirContrato(proposta: Proposta) {
    var url = this._contratoService.ObterUrlContratoReport(proposta.usina.codigo, proposta.obra.anoContrato, proposta.obra.numContrato);
    this.imprimirPropostaContrato(proposta, url);
  }

  listarProjecao(proposta: Proposta) {
    var projecao = this._obraService.ListarObraProjecao(proposta.obra.usinaCodigo, proposta.obra.numero, proposta.obra.anoContrato, proposta.obra.numContrato);
    
  }

  imprimirContratoResidual(proposta: Proposta) {
    var url = this._contratoService.ObterUrlContratoResidualReport(proposta.usina.codigo, proposta.obra.anoContrato, proposta.obra.numContrato);
    this.imprimirPropostaContrato(proposta, url);
  }
  
  imprimirContratoVersao() {
    var url = this._contratoService.ObterUrlContratoVersaoReport(this.versaoSelecionada.numeroVersao, this.proposta.usina.codigo, this.proposta.obra.anoContrato, this.proposta.obra.numContrato);
    this.imprimirPropostaContratoVersao(this.proposta, url);
  }

  imprimirAditivoVersao() {
    var url = this._contratoService.ObterUrlAditivoVersaoReport(this.versaoSelecionada.numeroVersao, this.proposta.usina.codigo, this.proposta.ano, this.proposta.numero, this.proposta.obra.anoContrato, this.proposta.obra.numContrato);
    var reportWindow = Tasks.openPdf('');
    reportWindow.location.href = url;
  }

  versao1Selecionada(): boolean {
    if (this.versaoSelecionada == undefined){
      return false;
    }
    return this.versaoSelecionada.numeroVersao === 1;
  }

  possuiAprovacaoCadastroNasVersoes() {

    if (!this.versaoSelecionada) return

    return this.contratoVersoes.filter(x => x.status != EStatusContrato.Aprovado && x.numeroVersao == this.versaoSelecionada.numeroVersao).length > 0

  }

  versaoSelecionadaRevalidaCadastro(): boolean {
    if(this.versaoSelecionada == undefined)
      return false;

    return this.versaoSelecionada.status != EStatusContrato.Aprovado;
  }

  placeholderContratoVersoes: string = "Versões de Contrato";
  async carregaContratoVersoes() {
    this.contratoVersoes = [];
    this.versaoAtual = 0;
    if (this.proposta.usina.codigo) {
      let placeholder = "Versões de Contrato";
      this.placeholderContratoVersoes = LOADING_MESSAGE+placeholder;
      this._contratoService.ListarVersoesContrato(this.proposta.usina.codigo, this.proposta.obra.anoContrato, this.proposta.obra.numContrato,true)
      .then(contratoVersoes => {
        this.contratoVersoes = contratoVersoes.filter(t => t !== null).sort((a, b) => b.numeroVersao - a.numeroVersao);

        if (this.contratoVersoes.length > 0) {
          this.versaoAtual = Math.max(...this.contratoVersoes.map(v => v.numeroVersao));
        }
      }, err => this.contratoVersoes = [])
      .then(() => {
        this.placeholderContratoVersoes = placeholder;
        this.detectChanges();
      });
    } 
  }

  imprimirPropostaContratoVersao(proposta: Proposta, url: string) {
    var temDireitoImpressao = this._userService.temDireitoAplicativo('WEB6101','R');
    if (!temDireitoImpressao) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para impressão de propostas!`
        }
      });
      return;
    }

    var reportWindow = Tasks.openPdf('');
    
    if (!this.parametroProposta.bloqueiaImpressaoPropostaContratoPendenteAprovacao) {
      if(this.versaoSelecionada != undefined){
        if(proposta.statusContrato != EStatusContrato.Aprovado && proposta.statusContrato != EStatusContrato.Reprovado && this.versaoSelecionada.numeroVersao == this.versaoAtual && !this.geraAditivoContratoSemAprovCadastro){
          reportWindow.close();
          this.dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Não é possível imprimir versões de contrato em aberto. Caso queira saber a diferença entre as versões, utilize o aditivo.`
            }
          });
        } else {
          reportWindow.location.href = url;
        }
      } else {
        reportWindow.location.href = url;
      }
    } else {
      this._obraService.TemAprovacaoPendente(proposta.obra)
      .then(pendente => {
        if (!pendente){
          reportWindow.location.href = url;
        } else {
          reportWindow.close();
          this.dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Não é possível imprimir pois há aprovações comerciais pendentes para essa proposta!`
            }
          });
        }
      }, err => {
        reportWindow.close();
        this.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Ocorreu um erro: ${err}`
          }
        });
      });
    }
  }

  imprimirPropostaContrato(proposta: Proposta, url: string) {
    var temDireitoImpressao = this._userService.temDireitoAplicativo('WEB6101','R');
    if (!temDireitoImpressao) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para impressão de propostas!`
        }
      });
      return;
    }

    var reportWindow = Tasks.openPdf('');

    if (!this.parametroProposta.bloqueiaImpressaoPropostaContratoPendenteAprovacao) {
        reportWindow.location.href = url;
    } else {
      this._obraService.TemAprovacaoPendente(proposta.obra)
      .then(pendente => {
        if (!pendente){
          reportWindow.location.href = url;
        } else {
          reportWindow.close();
          this.dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Não é possível imprimir pois há aprovações comerciais pendentes para essa proposta!`
            }
          });
        }
      }, err => {
        reportWindow.close();
        this.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Ocorreu um erro: ${err}`
          }
        });
      });
    }
  }

  utilizaImpressaoBoletos() {
    if(!this.proposta)
      return false;
      
    return this.emiteBoletoExterno && this.proposta.obra.tipoCobranca.forma == "BL";
  }

  boletoExternoSelecionado(): boolean {
    if (!this.boletoSelecionado || this.boletoSelecionado == undefined){
      return true;
    }
    return false;
  }

  selecaoBoletoExterno(valorSelecionado: string | BoletoExterno) {
    if (typeof valorSelecionado === 'string') {
      if (valorSelecionado == "")
        this.boletoSelecionado = null;

      const valorSelecionadoNoCombo = this.impressaoBoletosForm.get('boletosExternos').value;
      if (!valorSelecionadoNoCombo || typeof valorSelecionadoNoCombo !== 'string')
        return;
      
      this.boletoSelecionado = this.boletosExternos.find(boleto => this.impressaoBoletosFormatter(boleto) === valorSelecionadoNoCombo);

    } else {
      this.boletoSelecionado = valorSelecionado;
    }
  }

  placeholderImpressaoBoletos: string = "Arquivos";
  async carregaBoletosExternos() {
    this.boletosExternos = [];
    if (this.proposta.usina.codigo) {
      let placeholder = "Arquivos";
      this.placeholderImpressaoBoletos = LOADING_MESSAGE+placeholder;
      this._boletoExternoService.ListarBoletosExternos(this.proposta.usina.codigo, this.proposta.obra.anoContrato, this.proposta.obra.numContrato,true)
      .then(boletosExternos => {
        this.boletosExternos = boletosExternos.filter(t => t !== null);
      }, err => this.boletosExternos = [])
      .then(() => {
        this.placeholderImpressaoBoletos = placeholder;
        this.detectChanges();
      });
    } 
  }

  abrirBoletoExterno() {
    let self = ContratoListaPageComponent.self;

    if (!this.boletoSelecionado || this.boletoSelecionado == undefined) {
      self.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Nenhum boleto foi selecionado para impressão.`
        }
      });
      return;
    }

    self._boletoExternoService.ObterArquivoBoletoExterno(this.boletoSelecionado)
    .then(url => {
      var type = url.split(';')[0];
      type = type.replace("data:", "");
      var arquivo = url.split(',')[1]
      Tasks.openBase64File(arquivo, "", type)
    }).catch(error => {
      self.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Erro ao obter o Arquivo: ${JSON.stringify(error.exceptionMessage)}`
        }
      });
      return;
    });
  }

  inserirProgramacao(proposta: Proposta) {
    
    var temDireito = this._userService.temDireitoAplicativo('WEB6201','I');
    if (!temDireito) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para inserir programações!`
        }
      });
      return;
    }

    if (proposta.interveniente && proposta.interveniente.bloqueioMotivo) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Interveniente bloqueado! Motivo: ${proposta.interveniente.bloqueioMotivo.descricao}`
        }
      });
    } else if(proposta.isContratoEncerrado) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: 'O contrato está encerrado! Não é possível incluir programações!'
        }
      });
    } else {
      var self = this;
      self._propostaService.ObterVolumeTotalProposto(proposta.usina, proposta.ano, proposta.numero)
      .then(volumeProposto => {
        self._programacaoService.ObterVolumeTotalProgramado(proposta.usina, proposta.obra.numero)
        .then(volumeProgramado => {
          if ((volumeProgramado ==0 && volumeProposto ==0) || (volumeProgramado < volumeProposto)) {
            self._router.navigateByUrl('pages/comercial/programacao/usina/'+proposta.usina.codigo+'/proposta-ano/'+proposta.ano+'/proposta-numero/'+proposta.numero+'/nova');
          } else {
            var mensagem = `Volume total da proposta já foi programado! (Proposto: ${volumeProposto}m3 | Programado: ${volumeProgramado}m3) Poderá haver bloqueio durante a pesagem. \nDeseja inserir nova programação mesmo assim?`;
            self.dialog.open(ConfirmDialogComponent, {
              data: {
                title: 'TopConWeb',
                message: mensagem,
                confirmCallback: () => {
                  self._router.navigateByUrl('pages/comercial/programacao/usina/'+proposta.usina.codigo+'/proposta-ano/'+proposta.ano+'/proposta-numero/'+proposta.numero+'/nova');
                }
              }
            });
          }
        }, error => {
          self.dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: error
            }
          });
        });
      }, error => {
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: error
          }
        });
      });
    }
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }
  
  get captionInserirProgramacao(): string {
    if (this.isSmallScreen) {
      return 'Programar';
    } else {
      return 'Inserir programação';
    }
  }

  get temDireitoAcessoMargemPosTransporte(): boolean{
    return this._userService.temDireitoAplicativo('WEB6103', '');
  }

  labelContratoNumero(proposta: Proposta): string {
    if (this.isSmallScreen) {
      return proposta.obra.numContrato.toString().padStart(6,'0');
    } else {
      return proposta.usina.codigo+'/'+proposta.obra.numContrato.toString().padStart(6,'0')+'-'+proposta.obra.anoContrato;
    }
  }

  labelClienteObra(proposta: Proposta): string {
    if (this.isSmallScreen) {
      return proposta.intervenienteRazao.substr(0, 13)+'... / '+proposta.obra.nome.substr(0, 13)+'...';
    } else {
      return proposta.intervenienteRazao+' / '+proposta.obra.nome;
    }
  }

  getStatusProposta(statusCode: number): Status {
    return statusProposta.filter(t => t.codigo === statusCode)[0];
  }
  getStatusContrato(statusCode: number): Status {
    return statusContrato.filter(t => t.codigo === statusCode)[0];
  }
  getStatusComercial(statusCode: number): Status {
    return statusComercial.filter(t => t.codigo === statusCode)[0];
  }
  getstatusClicksignDocumento(statusCode: number): Status {
    return statusClicksignDocumento.filter(t => t.codigo === statusCode)[0];
  }

  getstatusProjecao(statusCode: number): Status {
    return statuProjecao.filter(t => t.codigo === statusCode)[0];
  }

  getSolicitacaoAssinaturaEletronicaEnabled(statusCode: EStatusClicksignDocumento): boolean {
    return statusCode == EStatusClicksignDocumento.Assinado;
  }

  getContratoFinalidade(finalidade: EContratoFinalidade): string {
    var finalidadeSelecionada = this.finalidades.filter(t => t.codigo === finalidade)[0]
    return (finalidadeSelecionada ? finalidadeSelecionada.descricao.toUpperCase() : '');
  }

  
  ListarLogs
  openObraLog(proposta: Proposta) {
    let self = this;
    let o = proposta.obra;
    //this._obraService.ListarLogs(o.usinaCodigo, o.numero, o.anoChamada, o.numChamada)
    this._obraService.ListarLogs(o.usinaCodigo, o.numero, o.anoChamada, o.numChamada, o.anoContrato, o.numContrato)
    .then(
      obraLogs => {
        self.dialog.open(ObraLogDialogComponent, {
          data: {
            obraLogs: obraLogs
          }
        });
      },
      error => {
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: error
          }
        });
      }
    );
  }

  statusContratoFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.statusContrato.includes(model) ? statusContrato.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  };

  statusComercialFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.statusComercial.includes(model) ? statusComercial.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  };

  statusClicksignDocumentoFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.statusClicksignDocumentoLista.includes(model) ? statusClicksignDocumento.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  };

  usinaEntregaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.usinasCodigos.includes(model) ? model + ' - ' +  this.usinas.filter(e => e.codigo===model)[0].nome.toUpperCase() : '';
  };

  //usinaEntregaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';

  solicitarAssinaturaEletronica(proposta: Proposta) { 
    let nomeCompletoResponsavelSolidario = '';
    let emailResponsavelSolidario = '';
    let cpfResponsavelSolidario = '';
    let telefoneDddResponsavelSolidario = 0
    let telefoneNumeroResponsavelSolidario = 0;

    let nomeCompletoVendedor = '';
    let emailVendedor = '';
    let cpfVendedor = '';
    let telefoneDddVendedor = 0
    let telefoneNumeroVendedor = 0;
  
    if (proposta.responsavelSolidario) {
      nomeCompletoResponsavelSolidario = proposta.responsavelSolidario.nome || '';
      emailResponsavelSolidario = proposta.responsavelSolidario.email || '';
      cpfResponsavelSolidario = proposta.responsavelSolidario.cpfCnpj || '';
      telefoneDddResponsavelSolidario = proposta.responsavelSolidario.telefoneDdd || 0;
      telefoneNumeroResponsavelSolidario = proposta.responsavelSolidario.telefoneNumero || 0;
    }

    if (proposta.vendedor) {
      nomeCompletoVendedor = proposta.vendedor.nome || '';
      emailVendedor = proposta.vendedor.email || '';
      cpfVendedor = proposta.vendedor.cpfCnpj || '';
      telefoneDddVendedor = proposta.vendedor.telefoneDdd || 0;
      telefoneNumeroVendedor = proposta.vendedor.telefoneNumero || 0;
    }

    this.dialog.open(SolicitacaoAssinaturaEletronicaDialogComponent, {
      data: {
        contratoAno: proposta.obra.anoContrato,
        contratoNumero: proposta.obra.numContrato,
        contratoUsina: proposta.obra.usinaCodigo,
        intervenienteCodigo: proposta.intervenienteCodigo,
        utilizaResponsavelSolidario: proposta.responsavelSolidario != null, 
        nomeCompletoResponsavelSolidario,
        emailResponsavelSolidario,
        cpfResponsavelSolidario,
        telefoneDddResponsavelSolidario,
        telefoneNumeroResponsavelSolidario,
        nomeCompletoVendedor,
        emailVendedor,
        cpfVendedor,
        telefoneDddVendedor,
        telefoneNumeroVendedor,
        afterConfirmCallback:  () => 
        { 
          this.dialog.closeAll();
          this.proposta.obra.contrato.statusClicksignDocumento = EStatusClicksignDocumento.Processando
          if (this.proposta.obra.statusCadastro === EStatusCadastro.Aprovado) {
            this.proposta.obra.statusCadastro = EStatusCadastro.Revalidacao
            this._obraService.AlterarStatusCadastroEAnalista(proposta.obra)
          }
        }
      }
    });
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

  get temDireitoAcessoAnexo(): boolean{
    return this._userService.temDireitoAplicativo('CON0036', '');
  }

  abrirSeletorDeArquivos(inputFile: HTMLInputElement) {
    let self = ContratoListaPageComponent.self;
    var temDireito = self._userService.temDireitoAplicativo('CON0036','I');
    if (!temDireito) {
     self.dialog.open(AlertDialogComponent, {
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
    let self = ContratoListaPageComponent.self;
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onloadend = async () => {
        const base64String = reader.result as string;
        try {
          await self._intervenienteService.adicionarAnexo(base64String,self.proposta.intervenienteCodigo, self.proposta.ano, self.proposta.numero, file.name);
          self.anexos = await self._intervenienteService.listarAnexos(self.proposta.intervenienteCodigo, self.proposta.ano, self.proposta.numero);
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Anexo inserido com sucesso!'
            }
          });
        } catch (err) {
          self.dialog.open(AlertDialogComponent, {
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
    let self = ContratoListaPageComponent.self;

    self._intervenienteService.ObterAnexo(anexo)
    .then(url => {
      var type = url.split(';')[0];
      type = type.replace("data:", "");
      var arquivo = url.split(',')[1]
      Tasks.openBase64File(arquivo, anexo.nome, type)
    }).catch(error => {
      self.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Erro ao obter o Anexo: ${JSON.stringify(error.exceptionMessage)}`
        }
      });
      return;
  });
  }

  atualizarDescricaoAnexo(anexo: IntervenienteAnexo): void {
    let self = ContratoListaPageComponent.self;

    var temDireito = self._userService.temDireitoAplicativo('CON0036','A');
    if (!temDireito) {
      self.dialog.open(AlertDialogComponent, {
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
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Descrição alterada com sucesso!`
        }
      });
     }, err => {
      self.anexo.descricao = self.descricaoAnteriorAnexo;
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro alterar a descrição.\n${JSON.stringify(err.exceptionMessage)}`
        }
      });
     });
  }

  removerAnexo(anexo: IntervenienteAnexo) {
    let self = ContratoListaPageComponent.self;

    var temDireito = self._userService.temDireitoAplicativo('CON0036','E');
    if (!temDireito) {
      self.dialog.open(AlertDialogComponent, {
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
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
  }

  getStatusIcon(status: number): string {
    switch (status) {
      case 9145:
        return 'lock';
      default:
        return 'description';
    }
  }


  confirmModal: Function;
  cancelModal: Function;
  showProjecaoModal(content, confirmCallback: Function, cancelCallback?: Function) {

    let self = ContratoListaPageComponent.self;

    self.confirmModal = confirmCallback;
    self.cancelModal = cancelCallback || self.openProjecaoModal;

    self.openProjecaoModal(content);

    self.modalIsOpen = true;
  }

  openProjecaoModal(content) {

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
    
  }


  closeProjecaoModalGeneric() {
    let self = ContratoListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.dialog.closeAll();

    self.modalIsOpen = false;
  }

  showModalProjecaoObra(content, container: ViewContainerRef, obraProjecao: ObraProjecao = undefined) {
    let self = ContratoListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    if(obraProjecao) {

      self.cadObraProjecao = {
        volume: obraProjecao.volume,
        periodo: obraProjecao.periodo,
      };

      self.cadObraProjecaoNovo = false;
      self.cadObraProjecaoSelecionado = obraProjecao;
      self.cadObraProjecaoSelecionado.periodo = obraProjecao.periodo;
      self.cadObraProjecaoSelecionado.volumeAnterior = self.cadObraProjecaoSelecionado.volume;
      self.cadObraProjecaoSelecionado.periodoAnterior = self.cadObraProjecaoSelecionado.periodo;
    } else {
      self.cadObraProjecaoSelecionado = new ObraProjecao();
      self.cadObraProjecaoSelecionado.periodo = self.obraProjecaoProximoPeriodo;
      self.cadObraProjecao = self.cadObraProjecaoSelecionado;
      self.cadObraProjecaoNovo = true;
    }

    self._subDialogRef = self.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;

  }

  obraProjecaoModalSalvar() {
   
      const dataAtual = new Date();
      const dataPeriodo = new Date(this.cadObraProjecaoSelecionado.periodo);
      const anoAtual = dataAtual.getFullYear();
      const mesAtual = dataAtual.getMonth()+1;
      const anoPeriodo = dataPeriodo.getFullYear();
      const mesPeriodo = dataPeriodo.getMonth()+1;

      const jaExistePeriodo = this.obraProjecoes.some(item => {
        const itemPeriodo = item.periodo instanceof Date ? item.periodo : new Date(item.periodo);
      
        const cadPeriodo = this.cadObraProjecaoSelecionado.periodo instanceof Date 
          ? this.cadObraProjecaoSelecionado.periodo 
          : new Date(this.cadObraProjecaoSelecionado.periodo);
      
        return itemPeriodo.getTime() === cadPeriodo.getTime();
      });

      if (jaExistePeriodo && this.cadObraProjecaoNovo) {
        this.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Período já cadastrado!`
          }
        });
        return;
      } 
      
      if (anoPeriodo < anoAtual || (anoAtual === anoPeriodo && mesPeriodo < mesAtual)) {
        this.dialog.open(AlertDialogComponent, {
           data: {
             title: 'TopConWeb',
             message: `Período não pode ser de meses anteriores`
           }
         });
         return;
      }

      this.cadObraProjecaoModalSalvar();
      
    }  

  closeProjecaoModal() {
    if (this.getPrevisaoSaldoProjecao() > 0 && this.getSaldoTotal() > this.getPrevisaoSaldoProjecao()){
    this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      data: {
        title: 'TopConWeb',
        message: `Previsão total da carteira M³ menor que Saldo total.
          Confirma projeção?`,
        confirmCallback: async () => {
          this.closeModal();
        }
      }
    });}
    else{
      this.closeModal();
    }

  }


  cadObraProjecaoModalSalvar() { 
    this.cadObraProjecaoSelecionado.periodo = this.cadObraProjecao.periodo;
    this.cadObraProjecaoSelecionado.volume = this.cadObraProjecao.volume;
    this.cadObraProjecaoSelecionado.usina = this.propostaSelecionada.obra.usinaCodigo;
    this.cadObraProjecaoSelecionado.noObra = this.propostaSelecionada.obra.numero;
    this.cadObraProjecaoSelecionado.noChamada = this.propostaSelecionada.obra.numChamada;
    this.cadObraProjecaoSelecionado.anoChamada = this.propostaSelecionada.obra.anoChamada;
    
    let self = ContratoListaPageComponent.self;
 
    if(this.cadObraProjecaoNovo) {

      self._obraProjecaoService.Adicionar(this.cadObraProjecaoSelecionado)
      .then(async success => {
        self.closeSubModal();
        self.getPage();
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Projeção da Carteira adicionada com sucesso!`
          }
        });
        await this.AtualizaProjecao();
      }, err => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
    }
     else{
      self._obraProjecaoService.Atualizar(this.cadObraProjecaoSelecionado)
     .then(async success => {
       self.closeSubModal();
       self.getPage();
       self.dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Projeção alterada com sucesso!`
         }
       });
       await this.AtualizaProjecao();
     }, err => {
       self.dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Erro ao alterar a Projeção.\n${JSON.stringify(err.exceptionMessage)}`
         }
       });
     });
     }     
   }
 
  showModalProjecao(o: Obra) {
    this._obraService.ListarObraProjecao(o.usinaCodigo, o.numero, o.anoChamada, o.numChamada)
      .then(
        projecaoObra => {
            if (projecaoObra) {
                this.cadObraProjecaoNovo = false;
            } else {
                this.cadObraProjecaoSelecionado = new ObraProjecao();
                this.cadObraProjecao = this.cadObraProjecaoSelecionado;
                this.cadObraProjecaoNovo = true;
            };
            
            this.openModal(projecaoObra); 
            this.modalIsOpen = true;
        },
        error => {
            this.dialog.open(AlertDialogComponent, {
                data: {
                    title: 'Erro',
                    message: error
                }
            });
        }
      );
}

  openModal(content) {
    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
  }


  closeModalGeneric() {
    let self = ContratoListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.dialog.closeAll();

    self.modalIsOpen = false;
  }  

getSegmento(): string {
  return this.propostaSelecionada.segmento.nome;
}

getVolumeTotal(): number {
  return this.obraProjecaoVolume;
}

getSaldoTotal(): number {
    this.saldoProjecao = this.getVolumeTotal() - this.getVolumeConsumido()
    return this.getVolumeTotal() - this.getVolumeConsumido();
  }

getSaldoProjecaoLista(volume : number): number {
  return this.saldoProjecao = this.saldoProjecao - volume;
}

getVolumeConsumido(): number {   
  return this.obraProjecaoConsumo;
}

getSaldoProjecao(): number {   
  return this.obraProjecaoSaldo;
}

getPrevisaoSaldoProjecao(): number {   
  return this.obraProjecaoPrevisaoSaldo;
}

isDataDesabilitada(periodo: Date): boolean {
  const dataAtual = new Date();
  const dataPeriodo = new Date(periodo);

  return dataPeriodo <= dataAtual && this.temDireitoAlteracaoProjecaoCarteiraMesAtual === false;
};

get volumeProjecaoValidator(): ICustomValidator {
    var message = 'Volume projetado maior que o saldo disponível!';
      return {
        
        key: 'volumeInvalido',
        message: message,
        validatorFunction: (volumeContratado: number, volumeConsumido: number, volumeDigitado: number, volumePrevisao: number): boolean => {
          if (this.cadObraProjecaoNovo) {
            return (volumeContratado - (volumeDigitado + volumePrevisao)) < 0;
          }
          else{
            return (volumeContratado - ((volumePrevisao - volumeDigitado) + this.cadObraProjecao.volume)) < 0;
          }

        },
        params: [this.getVolumeTotal(), this.getVolumeConsumido(), this.cadObraProjecaoSelecionado.volume, this.getPrevisaoSaldoProjecao()]
      }
}

async AtualizaProjecao() {  
    this.obraProjecoes = await this._obraService.ListarObraProjecao(this.proposta.obra.usinaCodigo, this.proposta.obra.numero, this.proposta.obra.anoChamada, this.proposta.obra.numChamada);                
    this.obraProjecaoSaldo = await this._obraProjecaoService.obterSaldoObraProjecao(this.proposta.obra);
    this.obraProjecaoPrevisaoSaldo = await this._obraProjecaoService.obterPrevisaoSaldoObraProjecao(this.proposta.obra);      
    this.obraProjecaoProximoPeriodo = await this._obraProjecaoService.getProximoPeriodoPorContrato(this.proposta.obra);
    
    this._cdr.detectChanges();  
  
}
}

