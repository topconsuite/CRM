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
import { Usina } from 'app/classes/usina/usina';
import { IntervenienteAnexo } from 'app/classes/interveniente/interveniente-anexo';
import { ObraProjecao } from 'app/classes/obra/obra-projecao';
import { ICustomValidator } from '../../../../components/interfaces/custom-validator';
// ******************************************************************

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { PropostaService } from 'app/services/proposta.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { VendedorService } from 'app/services/vendedor.service';
import { ObraService } from 'app/services/obra.service';
import { ContratoService } from 'app/services/contrato.service';
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
import { EStatusCadastro } from 'app/classes/obra/obra';
import { statuProjecao } from 'app/classes/obra/obra';
import { now, over } from 'lodash';
import { Endereco, Municipio } from 'app/classes/endereco/endereco';
import { UsinaService } from 'app/services/usina.service';
import { GrupoEconomico } from 'app/classes/grupo-economico/grupo-economico';
import { GrupoEconomicoService } from 'app/services/grupo-economico.service';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { PropostaVersao } from 'app/classes/proposta/proposta-versao';
import { PropostaReportPDF } from 'app/classes/proposta/proposta-report';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';
import { PropostaPropaganda } from 'app/classes/proposta/proposta-propaganda';
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
  selector: 'app-proposta-lista-page',
  templateUrl: './proposta-lista-page.component.html',
  styleUrls: ['./proposta-lista-page.component.scss']
})
export class PropostaListaPageComponent implements OnInit, AfterViewInit {
  public static self: PropostaListaPageComponent;

  cadastroForm: FormGroup;
  propostaVersaoForm: FormGroup;  
  propagandaPropostaForm: FormGroup;
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
  propagandas: PropostaPropaganda[] = [];
  utilizaPropaganda: boolean = false;

  propostaReportSequencia: PropostaReportPDF[] = [];
  propostaReportSelecionada: PropostaReportPDF;

  versaoAtual: number = 0;

  obra: Obra;
  tracosExibicaoEbitda: ObraTraco[] = [];
  custoServico: CustoServico;
  tracoExibEbitda: ObraTraco = new ObraTraco();

  usinas: Usina[] = [];

  parametroProposta: ParametroProposta = new ParametroProposta();  

  statusPropostaFiltro: number = 0;

  exibicaoContratosFiltro: number = 0;

  divergenciaCarteiraFiltro: boolean = false;

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
  obraProjecaoConsumoAcumulado: number;
  obraProjecaoConsumoMes: number;

  propostaVersaoFormatter = (model: PropostaReportPDF): string => model ? (' Versão ' + model.sequencia + ' - ' + model.data + ' - ' + model.usuario).toUpperCase() : '';
  propagandaFormatter = (model: PropostaPropaganda): string => model ? (this.removeFileExtension(model.nome) + ' - ' + this.formataDataHora(model.dataHora)).toUpperCase() : '';
  finalidadeFormatter = (model: ContratoFinalidade): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';

  formataTelefone = Tasks.formataTelefone;
  formataData = Tasks.formataData;
  formataDataHora = Tasks.formataDataHora;
  formataMoeda = Tasks.formataMoeda;
  formataValor = Tasks.formataValor;

  displayedColumnsObraProjecao: string[] = ['projecao-volume','projecao-periodo', 'projecao-edicao'];
 
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

  get statusComercial(): number[] {
    let codigos: number[] = [];
    statusComercial.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos.filter(t => t > 0);
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

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  @ViewChild('ebitdaModalVCR', { read: ViewContainerRef, static: false }) ebitdaModalRef: ViewContainerRef;
  @ViewChild('propostaVersaoModalVCR', { read: ViewContainerRef, static: false }) propostaVersaoModalRef: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('anexosModalVCR', { read: ViewContainerRef, static: false }) anexosModalRef: ViewContainerRef;
  @ViewChild('descricaoAnexoModalVCR', { read: ViewContainerRef, static: false }) descricaoAnexoModalRef: ViewContainerRef;
  @ViewChild('projecaoObraModalVCR', { read: ViewContainerRef, static: false }) projecaoObraModalRef: ViewContainerRef;  
  @ViewChild('obraProjecaoCadastroModalVCR', { read: ViewContainerRef, static: false }) obraProjecaoCadastroModalRef: ViewContainerRef;  
  @ViewChild('propagandaModalVCR', { read: ViewContainerRef, static: false }) propagandaModalRef: ViewContainerRef;  
  @ViewChild('selecionarPropagandaModalVCR', { read: ViewContainerRef, static: false }) selecionarPropagandaModalRef: ViewContainerRef;  
  @ViewChild('modalVCR', { read: ViewContainerRef, static: false }) ModalRef: ViewContainerRef;

  constructor(
    public dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _custoServicoService: CustoServicoService,
    private _userService: UserService,
    private _propostaService: PropostaService,
    private _intervenienteService: IntervenienteService,
    private _grupoEconomicoService: GrupoEconomicoService,
    private _vendedorService: VendedorService,
    private _obraService: ObraService,
    private _contratoService: ContratoService,
    private _parametroService: ParametroService,
    private _router: Router,
    private _usinaService: UsinaService,    
    private _segmentacaoService: SegmentacaoService,
    private _obraProjecaoService: ObraProjecaoService,
    private _formBuilder: FormBuilder
  ) {
    var temDireito = this._userService.temDireitoAplicativo('WEB6101','', 200);
    if (!temDireito) return;

    PropostaListaPageComponent.self = this;

    this._userService.gravarAcessoAplicacao("Comercial", 6101);

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

    _usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );

    _contratoService.ListarFinalidades().then(
      finalidades => { this.finalidades = finalidades },
      error => { this.finalidades = [] }
    )

    _parametroService.obterParametoN("web", "RelProposta", true).then(
      parametro => { this.utilizaPropaganda = parametro.toLowerCase().includes("maxmohr"); },
      error => { this.utilizaPropaganda = false; }
    )
    
  }

  ngOnInit() {
    this.propostaVersaoForm = this._formBuilder.group({});
    this.propagandaPropostaForm = this._formBuilder.group({});
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
    
    this._propostaService.ListarEmOrdemDecrescente(currentPage, pageSize, this.filtroString, this.statusPropostaFiltro, this.exibicaoContratosFiltro, this.divergenciaCarteiraFiltro)
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

    if(container == this.propostaVersaoModalRef) {
      this.propostaReportSelecionada = null;
      await this.carregaPropostaReport();
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
      this.obraProjecaoConsumoAcumulado = await this._obraService.obterConsumoAcumuladoPorContrato(proposta.obra);  
      this.obraProjecaoConsumoMes = await this._obraService.obterConsumoMesAtualPorContrato(proposta.obra);  
    
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
    let self = PropostaListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self.anexo = anexo;
    self.descricaoAnteriorAnexo = anexo.descricao;

    self._subDialogRef = self.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  showSubModal(content, container: ViewContainerRef) {
    let self = PropostaListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self._subDialogRef = self.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  resultadoSelecionaPropaganda: boolean
  propagandaModal: any;
  propagandaContainer: ViewContainerRef;

  closeModal() {
    let self = PropostaListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;

  }

  closeSubModal() {
    let self = PropostaListaPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  cancelSubModal() {
    let self = PropostaListaPageComponent.self;

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
  statusPropostaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model) || model===0) return '';
    return statusProposta.filter(e => e.codigo===model)[0].descricao.toUpperCase();
  };

  exibicaoContratosFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model) || model===0) return '';
    return exibicaoContratos.filter(e => e.codigo===model)[0].descricao.toUpperCase();
  };

  listarProjecao(proposta: Proposta) {
    var projecao = this._obraService.ListarObraProjecao(proposta.obra.usinaCodigo, proposta.obra.numero, proposta.obra.anoContrato, proposta.obra.numContrato);
    
  }

  placeholderPropostaVersoes: string = "Versões de Relatório de Proposta";
  async carregaPropostaReport() {
    this.propostaReportSequencia = [];
    this.versaoAtual = 0;
    if (this.proposta.usina.codigo) {
      let placeholder = "Versões de Relatório de Proposta";
      this._propostaService.ListarPropostaReport(this.proposta.usina.codigo, this.proposta.ano, this.proposta.numero, true)
      .then(propostaReportSequencia => {
        this.propostaReportSequencia = propostaReportSequencia.filter(t => t !== null);
      }, err => this.propostaReportSequencia = [])
      .then(() => {
        this.placeholderPropostaVersoes = placeholder;
        
        if(this.propostaReportSequencia.length > 0)
          this.propostaReportSelecionada = this.propostaReportSequencia[0];

        if(this.propostaReportSequencia.length == 0) {
          var dialogRef = this.dialog.open(ConfirmDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: 'Não há versões salvas para o relatório da proposta, deseja criar uma nova versão ?',
              confirmCallback: () => { this.salvarPropostaReport(); }
            },
            disableClose: false
          });
        }

        this.detectChanges();
      });
    } 
  }

   async salvarPropostaReport() {
    await this._propostaService.ListarPropagandaTodos().then(
      result => { this.propagandas = result; },
      error => { this.propagandas = [] }
    ).then(() => {
      this._cdr.detectChanges();
    });

    if (this.utilizaPropaganda)
      var propagandaId = this.propagandas.filter(p => p.ativa == true).length > 0 ? this.propagandas.filter(p => p.ativa == true)[0].id : '';
    
    this._propostaService.CriarNovaPropostaReport(this.proposta.usina.codigo, this.proposta.ano, this.proposta.numero, propagandaId, false)
    .then(sequenciaInserida => {
 
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Criado nova versão de PDF da proposta!`
        }
      });

      this.carregaPropostaReport();
      this.closeSubModal();

    });
  }

  imprimirPropostaVersao() {
    var url = this._propostaService.ObterUrlPropostaReportSequencia(this.proposta.usina.codigo, this.proposta.ano, this.proposta.numero, this.propostaReportSelecionada.sequencia);
    this.imprimirPropostaContrato(this.proposta, url);
  }

  imprimirProposta(proposta: Proposta) {
    var url = this._propostaService.ObterUrlPropostaReport(proposta);
    this.imprimirPropostaContrato(proposta, url);
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

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }
  
  get temDireitoAcessoMargemPosTransporte(): boolean{
    return this._userService.temDireitoAplicativo('WEB6103', '');
  }
  labelPropostaNumero(proposta: Proposta): string {
    if (this.isSmallScreen) {
      return proposta.numero.toString().padStart(6,'0');
    } else {
      return proposta.usina.codigo+'/'+proposta.numero.toString().padStart(6,'0')+'-'+proposta.ano;
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

  getstatusProjecao(statusCode: number): Status {
    return statuProjecao.filter(t => t.codigo === statusCode)[0];
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

  usinaEntregaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.usinasCodigos.includes(model) ? model + ' - ' +  this.usinas.filter(e => e.codigo===model)[0].nome.toUpperCase() : '';
  };

  //usinaEntregaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';

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
    let self = PropostaListaPageComponent.self;
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
    let self = PropostaListaPageComponent.self;
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
    let self = PropostaListaPageComponent.self;

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
    let self = PropostaListaPageComponent.self;

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
    let self = PropostaListaPageComponent.self;

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

    let self = PropostaListaPageComponent.self;

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
    let self = PropostaListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.dialog.closeAll();

    self.modalIsOpen = false;
  }

  showModalProjecaoObra(content, container: ViewContainerRef, obraProjecao: ObraProjecao = undefined) {
    let self = PropostaListaPageComponent.self;
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
    this.cadObraProjecaoSelecionado.saldo = this.getSaldoTotal();
    
    let self = PropostaListaPageComponent.self;
 
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
    let self = PropostaListaPageComponent.self;
    
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

getVolumeConsumidoAcumulado(): number {   
  return this.obraProjecaoConsumoAcumulado;
}

getVolumeConsumidoMesAtual(): number {   
  return this.obraProjecaoConsumoMes;
}

getSaldoProjecao(): number {   
  return this.obraProjecaoSaldo;
}

getPrevisaoSaldoProjecao(): number {   
  return this.obraProjecaoPrevisaoSaldo;
}

getContratoFinalidade(finalidade: EContratoFinalidade): string {
  var finalidadeSelecionada = this.finalidades.filter(t => t.codigo === finalidade)[0]
  return (finalidadeSelecionada ? finalidadeSelecionada.descricao.toUpperCase() : '');
}

isDataDesabilitada(periodo: Date): boolean {
  const dataAtual = new Date();
  const dataPeriodo = new Date(periodo);

  const mesAtual = dataAtual.getMonth();
  const anoAtual = dataAtual.getFullYear();

  const mesPeriodo = dataPeriodo.getMonth();
  const anoPeriodo = dataPeriodo.getFullYear();
  
  if (anoPeriodo < anoAtual) {
    return true;
  } else if (anoPeriodo === anoAtual && mesPeriodo < mesAtual) {
    return true;
  }

  return false;
};

get volumeProjecaoValidator(): ICustomValidator {
    var message = 'Volume projetado maior que o saldo disponível!';
      return {
        
        key: 'volumeInvalido',
        message: message,
        validatorFunction: (volumeContratado: number, volumeConsumido: number, volumeDigitado: number, volumePrevisao: number): boolean => {
          if (this.cadObraProjecaoNovo) {
            return (volumeContratado - (volumeConsumido + volumeDigitado + volumePrevisao)) < 0;
          }
          else{
            return (volumeContratado - (volumeConsumido  + (volumePrevisao - volumeDigitado) + this.cadObraProjecao.volume)) < 0;
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
  
  removeFileExtension(fileName: string): string {
    const lastDotIndex = fileName.lastIndexOf(".");
    
    if (lastDotIndex === -1) {
      return fileName;
    }
    
    return fileName.substring(0, lastDotIndex);
  }

}

