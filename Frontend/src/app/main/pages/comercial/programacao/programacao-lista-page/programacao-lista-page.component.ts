import { Component, OnInit, ViewChild, ChangeDetectorRef, AfterViewInit, ViewContainerRef, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { animate, state, style, transition, trigger } from '@angular/animations';

// **** CLASSES ***
import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';

// **** CLASSES ***
import { Programacao, Status, statusProgramacao, EProgramacaoStatus, EProgramacaoConfirmacao, ProgramacaoHora } from 'app/classes/programacao/programacao.classes';
import { statusContrato, statusProposta, statusComercial } from 'app/classes/proposta/proposta.classes';
import { Interveniente } from 'app/classes/interveniente/interveniente';
import { Usina } from 'app/classes/usina/usina';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { Endereco } from 'app/classes/endereco/endereco';
import { ETipoVinculoMpaConsumo } from 'app/classes/traco/traco.classes';
import { CondicaoPagamento } from 'app/classes/pagamento/pagamento.classes';

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { ProgramacaoService } from 'app/services/programacao.service';
import { PropostaService } from 'app/services/proposta.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { UsinaService } from 'app/services/usina.service';
import { VendedorService } from 'app/services/vendedor.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';

import { ProgramacaoLogDialogComponent } from '../../../../components/dialog/programacao-log-dialog/programacao-log-dialog.component';
import { AlertDialogComponent } from '../../../../components/dialog/alert-dialog/alert-dialog.component';
import { FilterComponent } from '../../../../components/list/filter/filter.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';

import { element } from 'protractor';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { ObraTraco } from 'app/classes/obra/obra-traco';
import { Router } from '@angular/router';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';
import { ContratoService } from 'app/services/contrato.service';


export interface TableColumn {
  name: string;
  placeholder: string;
  align?: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-programacao-lista-page',
  templateUrl: './programacao-lista-page.component.html',
  styleUrls: ['./programacao-lista-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class ProgramacaoListaPageComponent implements OnInit, AfterViewInit {
  public static self: ProgramacaoListaPageComponent;

  programacao: Programacao;

  itens: PagedList<Programacao> = new PagedList<Programacao>();
  usinas: Usina[] = [];

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  intervenientes: Interveniente[] = [];

  vendedoresPermitidos: Vendedor[] = [];
  
  segmentacao: Segmentacao[] = [];
  finalidades: ContratoFinalidade[] = [];

  usinasEntrega: Usina[];

  get statusProgramacao(): number[] {
    let codigos: number[] = [];
    statusProgramacao.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos;
  };

  filtroString: string = '';
  getDefaultFilterString(): string {
    return `filter=$(dataConcretagem>=${this.formatDateToFilter(Tasks.dataAtual())};dataConcretagem<=${this.formatDateToFilter(Tasks.addDays(Tasks.dataAtual(), 5))})`
  }

  formatDateToFilter(data: Date): string {

    const yyyy = data.getFullYear();
    const mm = String(data.getMonth() + 1).padStart(2, '0'); // getMonth() retorna 0 a 11
    const dd = String(data.getDate()).padStart(2, '0');
    
    const dataFormatada = `${yyyy}-${mm}-${dd}`;
    return dataFormatada
  }

  filtro: {
    tipoDocumento: number,
    cpfCnpj: string,
    interveniente: Interveniente,
    intervenienteRazao: string,
    usinaEntrega: Usina,
    propostaAno: number,
    propostaNumero: number,
    contratoAno: number,
    contratoNumero: number,
    sequencia: number,
    statusProgramacao: number,
    dataDe: Date,
    dataAte: Date,
    vendedor: Vendedor,
    enderecoObra: string,
    segmentacao: number,
    contratoFinalidade: number
  } = {
      tipoDocumento: 0,
      cpfCnpj: '',
      interveniente: null,
      intervenienteRazao: '',
      usinaEntrega: null,
      propostaAno: 0,
      propostaNumero: 0,
      contratoAno: 0,
      contratoNumero: 0,
      sequencia: 0,
      statusProgramacao: 0,
      dataDe: Tasks.dataAtual(),
      dataAte: Tasks.addDays(Tasks.dataAtual(), 5),
      vendedor: null,
      enderecoObra: '',
      segmentacao: 0,
      contratoFinalidade: 0
    };

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;

  cancelaProgramacaoForm: FormGroup;
  rejeitaProgramacaoForm: FormGroup;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  horarioFormatter = (item: string) => {
    if (item === null || item === undefined) return;
    if (item.length === 3) item = `0${item}`;
    return item ? `${item.substr(0, 2)}:${item.substr(2)}` : '';
  }
  moedaFormatter = (item: number) => {
    let self = ProgramacaoListaPageComponent.self;
    return self.formataValor(item, 2, true);
  }
  getVolume(item: ProgramacaoHora) {
    let self = ProgramacaoListaPageComponent.self;
    return self.formataValor(item.volumeEntregue ? item.volumeEntregue : item.volumeProgramado, 1, false);
  }
  getValorConcreto(item: ProgramacaoHora) {
    let self = ProgramacaoListaPageComponent.self;
    return item.nfVolume ? self.moedaFormatter(item.nfTracoValorTotal) : '0,00';
  }
  getValorHoraExtra(item: ProgramacaoHora) {
    let self = ProgramacaoListaPageComponent.self;
    return self.moedaFormatter(item.nfAdicionalFeriadoValorTotal + item.nfAdicionalHoraExtraValorTotal);
  }
  getValorOutros(item: ProgramacaoHora) {
    let self = ProgramacaoListaPageComponent.self;
    return self.moedaFormatter(item.nfComplementoTaxaPermanenciaValor + item.nfComplementoValorAdicionais
      + item.nfComplementoAdicionalKmValorTotal + item.nfComplementoAdicionalRetornoConcretoTotal
      + item.nfVibradorValorTotal);
  }
  getValorDemaisServicos(item: ProgramacaoHora) {
    let self = ProgramacaoListaPageComponent.self;
    return self.moedaFormatter(item.nfComplementoValorDemaisServicos);
  }
  getValorTotal(item: ProgramacaoHora) {
    let self = ProgramacaoListaPageComponent.self;
    /*return self.moedaFormatter((item.nfVolume ? item.nfTracoValorTotal : 0) + item.nfBombaValorTotal
      + item.nfAdicionalFeriadoValorTotal + item.nfAdicionalHoraExtraValorTotal
      + item.nfComplementoTaxaPermanenciaValor + item.nfComplementoValorAdicionais
      + item.nfComplementoAdicionalKmValorTotal + item.nfComplementoAdicionalRetornoConcretoTotal
      + item.nfVibradorValorTotal);//*/
    return self.moedaFormatter(item.nfComplementoValorTotalCobranca);
  }

  getPropostaNumero(programacao: Programacao): string {
    return (programacao.usina.codigo + '/' + programacao.propostaNumero + '-' + programacao.propostaAno).toUpperCase();
  }
  getContratoNumero(programacao: Programacao): string {
    return (programacao.usina.codigo + '/' + programacao.contratoNumero + '-' + programacao.contratoAno).toUpperCase();
  }
  getProgramacaoNumero(programacao: Programacao): string {
    return (programacao.usina.codigo + '-' + programacao.contratoNumero + '/' + programacao.contratoAno + ' - ' + programacao.sequencia).toUpperCase();
  }
  getInterveniente(programacao: Programacao): string {
    return (programacao.proposta.intervenienteCodigo + ' - ' + programacao.proposta.intervenienteRazao).toUpperCase();
  }
  getSegmento(programacao: Programacao): string {
    return (programacao.proposta.segmento ? programacao.proposta.segmento.nomeAbreviado : '').toUpperCase()
  }
  getEndereco(programacao: Programacao): string {
    return Tasks.ederecoToString(programacao.endereco);
  }
  getDataHoraProgramacao(programacao: Programacao): string {
    if (programacao.dataConcretagem && programacao.horario)
      return `${Tasks.formataData(programacao.dataConcretagem)} ${Tasks.formataHora(programacao.horario || '')}`;
    else
      return '';
  }
  getObraNome(programacao: Programacao): string {
    return programacao.obraNome.toUpperCase();
  }
  getTipoBomba(programacao: Programacao): string {
    let self = ProgramacaoListaPageComponent.self;
    self.getVerificaBomba(programacao)
    if(programacao.obraBombaSequencia && programacao["obraBomba"]){
      return self.bombaTipoFormatter(programacao["obraBomba"].bombaTipo); 
    }
    else
      return 'NENHUMA';
  }
  getVolumeProg(programacao: Programacao): string {
    return `${Tasks.formataValor(programacao.volumeTotal, 1, false)} M3`;
  }
  getQtdeCargas(programacao: Programacao): string {
    if(programacao.obraTracoSequencia){
      return Tasks.formataValor(Math.ceil(programacao.volumeTotal/programacao.volumePorCarga), 0, false);
    }
  }
  getObservacao(programacao: Programacao): string {
    if (programacao.observacao)
      return programacao.observacao.toUpperCase();
    else
      return '';
  }
  getUsinaEntrega(programacao: Programacao): string {
    if (programacao.usinaEntrega)
      return (programacao.usinaEntrega.codigo + ' - ' + programacao.usinaEntrega.nome).toUpperCase();
  }
  getData(programacao: Programacao): string {
    let self = ProgramacaoListaPageComponent.self;
    return self.formataData(programacao.dataConcretagem);
  }
  getHorario(programacao: Programacao): string {
    let self = ProgramacaoListaPageComponent.self;
    return self.formataHora(programacao.horario) + self.labelAConfirmar(programacao);
  }
  getVolumeLiberado(programacao: Programacao): string {
    let self = ProgramacaoListaPageComponent.self;
    if(self.liberacaoParcial(programacao))
      return `${Tasks.formataValor(programacao.volumeLiberado, 1, false)} M3`;
  }
  getPecaConcretar(programacao: Programacao): string {
    if(programacao.obraTracoSequencia)
      return programacao.pecaConcretar.toUpperCase();
    else
      return '';
  }
  getTraco(programacao: Programacao): string {
    let self = ProgramacaoListaPageComponent.self;
    if(programacao.obraTracoSequencia)
      return self.tracoString(programacao);
    else
      return 'Programação apenas de bomba';
  }
  getSolicitante(programacao: Programacao): string {
    return programacao.solicitante.toUpperCase();
  }
  getContato(programacao: Programacao): string {
    return programacao.proposta.contato.toUpperCase();
  }
  getCondicaoPagamento(programacao: Programacao): string {
    return programacao.proposta.obra.condicaoPagamento.descricao.toUpperCase();
  }

  columnsProgHora: TableColumn[] = [
    { name: 'status', placeholder: 'Status', align: 'left', order: 1, priority: 1 },
    { name: 'horario', placeholder: 'Hr.Prev.', align: 'right', order: 2, priority: 2, formatter: this.horarioFormatter },
    { name: 'nfHoraSaidaUsina', placeholder: 'Hr.Saída', align: 'right', order: 3, priority: 6, formatter: this.horarioFormatter },
    { name: 'volume', placeholder: 'Volume', align: 'right', order: 4, priority: 3, getValue: this.getVolume },
    { name: 'nfNumero', placeholder: 'Nº.Remessa', align: 'right', order: 5, priority: 4 },
    { name: 'nfBetoneiraCodigo', placeholder: 'BT', align: 'center', order: 6, priority: 5 },
    { name: 'nfTracoValorUnitario', placeholder: 'Vlr.Unitário', align: 'right', order: 7, priority: 13, formatter: this.moedaFormatter },
    { name: 'nfTracoValorTotal', placeholder: 'Valor', align: 'right', order: 8, priority: 7, getValue: this.getValorConcreto },
    { name: 'nfBombaValorTotal', placeholder: 'Vlr.Bomba', align: 'right', order: 9, priority: 8, formatter: this.moedaFormatter },
    { name: 'adicionalHoraExtra', placeholder: 'Vlr.Hora Extra', align: 'right', order: 10, priority: 9, getValue: this.getValorHoraExtra },
    { name: 'nfM3FaltanteValor', placeholder: 'Vlr.M3 Faltante', align: 'right', order: 11, priority: 10, formatter: this.moedaFormatter },
    { name: 'valorOutros', placeholder: 'Vlr.Outros', align: 'right', order: 12, priority: 11, getValue: this.getValorOutros },
    { name: 'valorDemaisServicos', placeholder: 'Demais Serviços', align: 'right', order: 13, priority: 12, getValue: this.getValorDemaisServicos },
    { name: 'valorTotal', placeholder: 'Vlr.Total', align: 'right', order: 14, priority: 13, getValue: this.getValorTotal }
  ];


  columns: TableColumn[] = [
    { name: 'segmento', placeholder: 'Segmento', order: 1, priority: 1, getValue: this.getSegmento, align: 'center' },
    { name: 'interveniente', placeholder: 'Cliente', order: 2, priority: 2, getValue: this.getInterveniente },
    { name: 'endereco', placeholder: 'Endereço', order: 3, priority: 3, getValue: this.getEndereco },
    { name: 'dataHoraProgramacao', placeholder: 'Data/Hora Prog.', order: 4, priority: 4, getValue: this.getDataHoraProgramacao },
    { name: 'obra', placeholder: 'Obra', order: 5, priority: 5, getValue: this.getObraNome },
    { name: 'bomba', placeholder: 'Tipo Bomba', order: 6, priority: 6, getValue: this.getTipoBomba },
    { name: 'volume', placeholder: 'Volume', order: 7, priority: 7, getValue: this.getVolumeProg },
    { name: 'qtdeCargas', placeholder: 'Qtde. de Cargas Previstas', order: 8, priority: 8, getValue: this.getQtdeCargas },
    { name: 'proposta', placeholder: 'Proposta', order: 9, priority: 9, getValue: this.getPropostaNumero },
    { name: 'contrato', placeholder: 'Contrato', order: 10, priority: 10, getValue: this.getContratoNumero },
    { name: 'programacao', placeholder: 'Programação', order: 11, priority: 11, getValue: this.getProgramacaoNumero },
    { name: 'data', placeholder: 'Data', order: 12, priority: 12, getValue: this.getData },
    { name: 'horario', placeholder: 'Harário', order: 13, priority: 13, getValue: this.getHorario },
    { name: 'volumeLiberado', placeholder: 'Volume Liberado', order: 14, priority: 14, getValue: this.getVolumeLiberado },
    { name: 'solicitante', placeholder: 'Solicitante', order: 15, priority: 15, getValue: this.getSolicitante },
    { name: 'contato', placeholder: 'Contato', order: 16, priority: 16, getValue: this.getContato },
    { name: 'traco', placeholder: 'Traço', order: 17, priority: 17, getValue: this.getTraco },
    { name: 'pecaConcretar', placeholder: 'Peça a Concretar', order: 18, priority: 18, getValue: this.getPecaConcretar },
    { name: 'usinaEntrega', placeholder: 'Usina Entrega', order: 19, priority: 19, getValue: this.getUsinaEntrega },
    { name: 'condicaoPagamento', placeholder: 'Condição de Pagamento', order: 20, priority: 20, getValue: this.getCondicaoPagamento },
    { name: 'observacao', placeholder: 'Observação', order: 21, priority: 21, getValue: this.getObservacao }
  ];

  get allColumns(): TableColumn[] {
    return this.columns;
  }

  hiddenColumns: string[] = [];
  isHiddenColumn(columnName: string): boolean {
    return this.hiddenColumns.includes(columnName);
  }
  setHiddenColumn(columnName: string, hidden: boolean) {
    if (hidden)
      this.hiddenColumns.push(columnName);
    else
      this.hiddenColumns = this.hiddenColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }

  get _columns(): TableColumn[] {
    return this.allColumns.filter(t => !this.hiddenColumns.includes(t.name));
  }

  get currentViewValue() {
    return { filter: this.filtro, hiddenColumns: this.hiddenColumns, customColumnOrder: this._customColumnOrder }
  }

  expandedElement: Programacao | null;

  get fixedColumnsLeft(): string[] {
    return ['statusProgramacao'];
  }
  get fixedColumnsRight(): string[] {
    return ['expand'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = ProgramacaoListaPageComponent.self;

    return this._columns.sort((a, b) => {
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
    var self = ProgramacaoListaPageComponent.self;

    return this._columns.sort((a, b) => {
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
    var self = ProgramacaoListaPageComponent.self;

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
    if ((index + increment) < 0 || (index + increment) >= this._customColumnOrder.length) return;

    this._customColumnOrder.splice(index + increment, 0, this._customColumnOrder.splice(index, 1)[0]);
  }

  get dataSource(): Programacao[] {
    return this.itens.records;
  };

  expandedElementProgHora: ProgramacaoHora | null;

  get displayedColumnsProgHora(): TableColumn[] {
    return this.columnsProgHora.sort((a, b) => {
      return a.order - b.order;
    }).filter(t => {
      var colsAllowed = Math.round(window.innerWidth / 100);
      return t.priority <= colsAllowed;
    });
  }
  get columnNamesProgHora(): string[] {
    return this.displayedColumnsProgHora.map(t => t.name);
  }
  get hiddenColumnsProgHora(): TableColumn[] {
    return this.columnsProgHora.sort((a, b) => {
      return a.order - b.order;
    }).filter(t => !this.columnNamesProgHora.includes(t.name));
  }

  dataSourceProgHora: ProgramacaoHora[];

  exibirAcompanhamento: boolean = false;

  temDireitoSolicitacaoAssinaturaEletronica: boolean = false;

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('cancelaProgModalVCR', { read: ViewContainerRef, static: false }) cancelaProgramacaoModalVCR: ViewContainerRef;

  @ViewChild('rejeitaProgModalVCR', { read: ViewContainerRef, static: false }) rejeitaProgramacaoModalVCR: ViewContainerRef;

  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;

  constructor(
    public dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _programacaoService: ProgramacaoService,
    private _propostaService: PropostaService,
    private _intervenienteService: IntervenienteService,
    private _usinaService: UsinaService,
    private _vendedorService: VendedorService,
    private _segmentacaoService: SegmentacaoService,
    private _contratoService: ContratoService,
    private _router: Router,
  ) {
    ProgramacaoListaPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6201', '', 200);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Comercial", 6201);

    this.temDireitoSolicitacaoAssinaturaEletronica = this._userService.temDireitoAplicativo('WEB6004', '');

    this._vendedorService.listarPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores; },
      error => { this.vendedoresPermitidos = []; }
    );

    this._segmentacaoService.listarTodos().then(
      segmentacoes => { this.segmentacao = segmentacoes; },
      error => { this.segmentacao = []; }
    );

    this._contratoService.ListarFinalidades().then(
      finalidades => { this.finalidades = finalidades },
      error => { this.finalidades = [] }
    )

  }

  ngOnInit() {
    this.cancelaProgramacaoForm = this._formBuilder.group({});
    this.rejeitaProgramacaoForm = this._formBuilder.group({});
  }
  ngAfterViewInit(): void {
    
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;

    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };

    this._usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinasEntrega = usinas },
      error => { this.usinasEntrega = [] }
    );

    this._programacaoService.ListarComPropostaContratoEmOrdemCrescente(currentPage, pageSize, this.filtroString)
      .then(
        programacoes => {
          this.itens = programacoes;
          this.paginaAtual = programacoes.currentPage;
          this.registrosPorPagina = programacoes.pageSize;
        },
        error => { this.itens = new PagedList<Programacao>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
  }

  ederecoString(endereco: Endereco): string {
    return Tasks.ederecoToString(endereco);
  }

  intervenienteFormatter = (model: Interveniente) => model ? model.codigo + ' - ' + model.razao.toUpperCase() : '';
  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';
  vendedorFormatter = (model: Vendedor) => model ? model.codigo + ' - ' + model.nome.toUpperCase() : '';
  segmentacaoFormatter = (model: Segmentacao) => model ? model.id+' - '+model.nome.toUpperCase() : '';
  finalidadeFormatter = (model: ContratoFinalidade): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  viewChanged(view: ICustomView) {
    if (!this.filter) return;
    this.setFilter(view.value ? view.value.filter : this.filter.defaultModel);
    this.hiddenColumns = view.value && view.value.hiddenColumns ? view.value.hiddenColumns : [];
    this._customColumnOrder = view.value && view.value.customColumnOrder ? view.value.customColumnOrder : [];
    this._cdr.detectChanges();
    this.filter.aplyFilter();
  }

  setFilter(newFilter) {
    //this.filtro = newFilter;
    Object.keys(newFilter).forEach(t => this.filtro[t] = newFilter[t]);

    if (this.filtro.dataDe) this.filtro.dataDe = new Date(this.filtro.dataDe);
    if (this.filtro.dataAte) this.filtro.dataAte = new Date(this.filtro.dataAte);
  }

  labelPropostaNumero(programacao: Programacao): string {
    if (this.isSmallScreen) {
      return programacao.propostaNumero.toString().padStart(6, '0');
    } else {
      return programacao.propostaNumero.toString().padStart(6, '0') + '-' + programacao.propostaAno;
    }
  }

  labelClienteObra(programacao: Programacao): string {
    if (this.isSmallScreen) {
      return programacao.proposta.intervenienteRazao.substr(0, 15) + '... / ' + Tasks.ederecoToString(programacao.endereco).substr(0, 25) + '...';
    } else {
      return programacao.proposta.intervenienteRazao + ' / ' + Tasks.ederecoToString(programacao.endereco);
    }
  }
  labelAConfirmar(programacao: Programacao): string {
    return (this.necessitaConfirmacao(programacao) ? ' AC' : (this.liberacaoParcial(programacao) ? ' P' : ''));
  }

  openProgramacaoLog(programacao: Programacao) {
    let self = ProgramacaoListaPageComponent.self;
    this._programacaoService.ListarLogs(programacao)
      .then(
        programacaoLogs => {
          self.dialog.open(ProgramacaoLogDialogComponent, {
            data: {
              programacaoLogs: programacaoLogs
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

  private _timeoutIntervenientesPorRazao = null;
  filtrarIntervenientesPorRazao(cliente: string) {
    this.filtro.intervenienteRazao = cliente;

    var tamanhoMinimo = (isNaN(parseInt(cliente)) ? 3 : 0);

    if (cliente && cliente.length > tamanhoMinimo && (!this.filtro.interveniente || this.filtro.interveniente.razao != cliente)) {

      if (this._timeoutIntervenientesPorRazao) clearTimeout(this._timeoutIntervenientesPorRazao);

      var filtro = 'filter=$(' + (isNaN(parseInt(cliente)) ? 'razao%=' + cliente : 'codigo==' + parseInt(cliente)) + ')';

      this._timeoutIntervenientesPorRazao = setTimeout(() => {
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

  filtroChange(novoFiltro: string) {
    this.filtroString = novoFiltro;
    
    if (this.filtro.dataDe) 
      this.filtro.dataDe = new Date(this.filtro.dataDe);
    else
      this.filtro.dataDe = Tasks.dataAtual();

    if (this.filtro.dataAte) 
      this.filtro.dataAte = new Date(this.filtro.dataAte);
    else
      this.filtro.dataAte = Tasks.addDays(Tasks.dataAtual(), 5);
    
    if (this.filtro.intervenienteRazao) this.filtro.intervenienteRazao = "";
    this.getPage();
  }

  currentPage() {
    if (this.itens.currentPage <= 0) return 0;
    return this.itens.currentPage - 1;
  }

  getStatusComercial(statusCode: number): Status {
    return statusComercial.filter(t => t.codigo === statusCode)[0];
  }

  getStatusProposta(statusCode: number): Status {
    return statusProposta.filter(t => t.codigo === statusCode)[0];
  }

  getStatusProgramacao(statusCode: number): Status {
    if (statusCode === null || statusCode === undefined || isNaN(statusCode)) return {codigo: EProgramacaoStatus.AguardandoConfirmacao , descricao: "Aguardando confirmação", color:'#ffc800' };
    if (statusCode === 0 ) return {codigo: EProgramacaoStatus.AguardandoConfirmacao , descricao: "Aguardando confirmação", color:'#ffc800' };

    return statusProgramacao.filter(t => t.codigo === statusCode)[0];
  }

  getStatusContrato(statusCode: number): Status {
    return statusContrato.filter(t => t.codigo === statusCode)[0];
  }

  podeSerCancelada(programacao: Programacao): boolean {
    return programacao.status !== EProgramacaoStatus.Cancelada
      && programacao.status !== EProgramacaoStatus.Revalidacao
      && new Date(programacao.dataConcretagem) >= Tasks.dataAtual();
  }

  observacaoCancelamento: string = '';
  cancelaProgramacao(programacao: Programacao) {
    let self = ProgramacaoListaPageComponent.self;

    self._programacaoService.Cancelar(programacao, self.observacaoCancelamento)
      .then(result => {
        self.closeModal();
        self.observacaoCancelamento = '';
        programacao.status = EProgramacaoStatus.Revalidacao;
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: result
          }
        });
      }, error => {
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'ERRO',
            message: error.erros || 'OCORREU UM ERRO!'
          }
        });
      });
  }

  podeSerAprovadaERejeitada (programacao: Programacao): boolean {
    return (programacao.status === EProgramacaoStatus.AguardandoConfirmacao
    || programacao.status === EProgramacaoStatus.Revalidacao)
    && new Date(programacao.dataConcretagem) >= Tasks.dataAtual();
  }

  podeSerAprovadoFinanceiro (programacao: Programacao): boolean {
    return (programacao.status === EProgramacaoStatus.AguardandoAnaliseLimiteCredito
      || programacao.status === EProgramacaoStatus.LimiteCreditoInsuficiente 
      || programacao.status === EProgramacaoStatus.AprovacaoInadimplente)
      && this._userService.temDireitoAplicativo('WEB6309','');
  }

  observacaoRejeitada: string = '';
  rejeitaProgramacao(programacao: Programacao) {
    let self = ProgramacaoListaPageComponent.self;

    self._programacaoService.RejeitaProgramacao(programacao, self.observacaoRejeitada)
      .then(result => {
        self.closeModal();
        self.observacaoRejeitada = '';
        programacao.status = EProgramacaoStatus.Rejeitado;
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message:'Programação Rejeitada com Sucesso!'
          }
        });
      }, error => {
        let mensagem = '';
        if (error.errors != undefined){
          for(let i=0; i<error.errors.length; i++){
            if (mensagem != "") mensagem += '\n';
            mensagem += error.errors[i].message;
          }
        } else mensagem = error;

        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'NÃO FOI POSSÍVEL REJEITAR A PROGRAMAÇÃO',
            message: mensagem
          }
        });
      });
  }

  async aprovaProgramacao(programacao: Programacao) {
    let self = ProgramacaoListaPageComponent.self;
    let atualizaComplexidadeBombeado = await this.dialogTemComplexidade(programacao);
    let gravaContinuidadeProgramacao = await this.dialogVerificaContinuidade(programacao);

    self._programacaoService.GeraProgramacao(programacao, atualizaComplexidadeBombeado, gravaContinuidadeProgramacao)
      .then(result => {
        programacao.status = EProgramacaoStatus.Programado;
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: 'Programação Confirmada com Sucesso!'
          }
        });
      }, error => {
        let mensagem = '';
        if (error.errors != undefined){
          for(let i=0; i<error.errors.length; i++){
            if (mensagem != "") mensagem += '\n';
            mensagem += error.errors[i].message;
          }
        } else mensagem = error;

        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'NÃO FOI POSSÍVEL CONFIRMAR A PROGRAMAÇÃO',
            message: mensagem
          }
        });
      });
  }

  async dialogVerificaContinuidade(programacao: Programacao): Promise<boolean>{
    let self = ProgramacaoListaPageComponent.self;
    let temApenasUmBombeamento = false;
    
    await self._programacaoService.VerificaContinuidade(programacao.usina, programacao.obraNumero, programacao.sequencia)
    .then(async(result) => {
      if(result) {
        const dialogRef = self.dialog.open(ConfirmDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Existe uma programação anterior para o mesmo contrato com as características semelhantes:\n` 
            + result 
            + `\nA programação atual pode ser uma sequência da programação acima.\nSerá cobrado apenas um Serviço de Bombeamento?`,

            confirmCallback: () => {
              temApenasUmBombeamento = true;
            }
          }
        });
        await dialogRef.afterClosed().toPromise();
      }
    }, async error => {
      const dialogRef = self.dialog.open(AlertDialogComponent, {
        data: {
          title: 'ERRO',
          message: error.erros || 'OCORREU UM ERRO!'
        }
      });
      await dialogRef.afterClosed().toPromise();
    });
    return Promise.resolve(temApenasUmBombeamento);
  }

  async dialogTemComplexidade(programacao: Programacao): Promise<boolean>{
    let self = ProgramacaoListaPageComponent.self;
    let temComplexoBombeado = false;
    
    await self._programacaoService.TemComplexidadeBombeado(programacao.usina, programacao.obraNumero, programacao.sequencia)
    .then(async(result) => {
      if(result) {
        const dialogRef = self.dialog.open(ConfirmDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Haverá complexidade de bombeado?`,
            confirmCallback: () => {
              temComplexoBombeado = true;
            }
          }
        });
        await dialogRef.afterClosed().toPromise();
      }
    }, async error => {
      const dialogRef = self.dialog.open(AlertDialogComponent, {
        data: {
          title: 'ERRO',
          message: error.erros || 'OCORREU UM ERRO!'
        }
      });
      await dialogRef.afterClosed().toPromise();
    });
    return Promise.resolve(temComplexoBombeado);
  }

  aprovaFinanceiro(programacao: Programacao) {
    let self = ProgramacaoListaPageComponent.self;

    const dialogRef = self.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'TopConWeb',
        message: `Confirma a aprovação da pendência financeira para esta programação?`,
        confirmCallback: () => {
          self._programacaoService.AprovaFinanceiro(programacao)
          .then(async(result) => {
            programacao.status = EProgramacaoStatus.Programado;  
            this.dialog.open(AlertDialogComponent, {
                data: {
                  title: 'TopConWeb',
                  message: result
                }
            });
          }, async error => {
            const dialogRef = self.dialog.open(AlertDialogComponent, {
              data: {
                title: 'ERRO',
                message: error.erros || 'OCORREU UM ERRO!'
              }
            });
          });
        }
      }
    })
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function) {
    var temDireito = this._userService.temDireitoAplicativo('WEB6201', 'A');
    if (!temDireito) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para alterar programações!`
        }
      });
      return;
    }

    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  closeModal() {
    let self = ProgramacaoListaPageComponent.self;

    if (self._dialogRef) self._dialogRef.close();

    self.cancelaProgramacaoForm.markAsPristine();
    self.cancelaProgramacaoForm.markAsUntouched();
    
    self.rejeitaProgramacaoForm.markAsPristine();
    self.rejeitaProgramacaoForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  showSelecaoColunasModal(content) {
    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  imprimirProgramacao(programacao: Programacao) {
    var temDireitoImpressao = this._userService.temDireitoAplicativo('WEB6201', 'R');
    if (!temDireitoImpressao) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para imprimir programações!`
        }
      });
      return;
    }

    Tasks.openPdf(this._programacaoService.ObterUrlProgramacaoReport(programacao));
  }

  clonarProgramacao(programacao: Programacao) {
    console.log(programacao);
    this._propostaService.ObterPorUsinaAnoNumero(programacao.usina, programacao.propostaAno, programacao.propostaNumero).then(
      (proposta) => {
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

        if(programacao.obraTracoSequencia > 0) {
          if (proposta.obra.obraTracos.find(t => t.sequencia == programacao.obraTracoSequencia).inativo) {
            this.dialog.open(AlertDialogComponent, {
              data: {
                title: 'TopConWeb',
                message: `Traço selecionado na programação está inativo!`
              }
            });
            return;
          }
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
          console.log(proposta.obra.numero);
          self._propostaService.ObterVolumeTotalProposto(proposta.usina, proposta.ano, proposta.numero)
          .then(volumeProposto => {
            self._programacaoService.ObterVolumeTotalProgramado(proposta.usina, proposta.obra.numero)
            .then(volumeProgramado => {
              if ((volumeProgramado ==0 && volumeProposto ==0) || (volumeProgramado < volumeProposto)) {
                self._router.navigateByUrl('pages/comercial/programacao/usina/' + programacao.usina.codigo + '/proposta-ano/' + programacao.propostaAno + '/proposta-numero/' + programacao.propostaNumero + '/sequencia/' + programacao.sequencia + '/clonar');
              } else {
                var mensagem = `Volume total da proposta já foi programado! (Proposto: ${volumeProposto}m3 | Programado: ${volumeProgramado}m3) Poderá haver bloqueio durante a pesagem. \nDeseja inserir nova programação mesmo assim?`;
                self.dialog.open(ConfirmDialogComponent, {
                  data: {
                    title: 'TopConWeb',
                    message: mensagem,
                    confirmCallback: () => {
                      self._router.navigateByUrl('pages/comercial/programacao/usina/' + programacao.usina.codigo + '/proposta-ano/' + programacao.propostaAno + '/proposta-numero/' + programacao.propostaNumero + '/sequencia/' + programacao.sequencia + '/clonar');
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
            
          });
        }
      },
      (Error) => {
        this.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: Error
          }
        });
      });
  }

  imprimirProposta(programacao: Programacao) {
    var temDireitoImpressao = this._userService.temDireitoAplicativo('WEB6101', 'R');
    if (!temDireitoImpressao) {
      this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para imprimir propostas!`
        }
      });
      return;
    }

    programacao.proposta.usina = programacao.usina;
    Tasks.openPdf(this._propostaService.ObterUrlPropostaProgramacaoReport(programacao.proposta, programacao.sequencia));
  }
  podeImprimirProposta(programacao: Programacao): boolean {
    return programacao.status === EProgramacaoStatus.Programado;
  }

  necessitaConfirmacao(programacao: Programacao): boolean {
    return programacao.necessitaConfirmacao === EProgramacaoConfirmacao.Sim;
  }

  liberacaoParcial(programacao: Programacao): boolean {
    return programacao.necessitaConfirmacao === EProgramacaoConfirmacao.Parcial;
  }

  private _temNfEmitida = {};
  temNfEmitida(programacao: Programacao): boolean {
    return this._temNfEmitida[`${programacao.usina.codigo}-${programacao.obraNumero}-${programacao.sequencia}`] || false;
  }
  carregarNfEmitida(programacao: Programacao) {
    var key = `${programacao.usina.codigo}-${programacao.obraNumero}-${programacao.sequencia}`;
    if (!this._temNfEmitida[key]) {
      this._programacaoService
        .TemNotaFiscalEmitida(programacao.usina, programacao.obraNumero, programacao.sequencia)
        .then(temNf => {
          this._temNfEmitida[key] = temNf;
          this._cdr.detectChanges();
        });
    }
  }

  expandirProgramacao(programacao: Programacao) {
    this.exibirAcompanhamento = false;
    this.carregarNfEmitida(programacao);
  }

  getVerificaBomba(programacao: Programacao) {
    if (programacao.obraBombaSequencia && !programacao["obraBomba"]) {
      this._programacaoService.ObterBomba(programacao)
        .then(bomba => {
          programacao["obraBomba"] = bomba;
        }, err => {
          programacao["obraBomba"] = undefined;
        });
    }
  }

  openAcompanhamento(programacao: Programacao) {
    if (this.exibirAcompanhamento) {
      this.exibirAcompanhamento = false;
      this._cdr.detectChanges();
    } else {
      this._programacaoService.ListarHorarios(programacao)
        .then(horarios => {
          this.dataSourceProgHora = horarios.filter(t => t.status !== 'N');
          this.exibirAcompanhamento = true;
        }, err => {
          this.dataSourceProgHora = [];
        })
        .then(() => {
          this._cdr.detectChanges();
        });
    }
  }

  getFormattedValueProgramacao(element: Programacao, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  getFormattedValue(element: ProgramacaoHora, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  getCellColor(item: ProgramacaoHora) {
    switch (item.status) {
      case 'B':
        return '#ff8000';
      case 'C':
        return '#ff0000';
      case 'E':
        return '#8080ff';
      case 'L':
        return '#80ff80';
      default:
        return '';
    }
  }

  getVolumeTotal(status?: string) {
    return this.formataValor(
      this.dataSourceProgHora.filter(t => !status || t.status === status)
        .map(t => t.volumeEntregue ? t.volumeEntregue : t.volumeProgramado)
        .reduce((a, b) => a + b, 0)
      , 1, false);
  }

  getViagensTotal(status?: string) {
    return this.dataSourceProgHora.filter(t => !status || t.status === status).length;
  }

  condicaoPagamentoFormatter = (model: CondicaoPagamento): string => model ? model.descricao.toUpperCase() : '';
  bombaTipoFormatter = (model): string => model ? model.descricao.toUpperCase() : '';

  tracoString(programacao: Programacao): string {
    if (!programacao.resistenciaTipo || !programacao.pedra || !programacao.slump || !programacao.uso || (programacao.mpa + programacao.consumo) === 0) return '';
    let vinculo = programacao.resistenciaTipo.vinculo;
    let mpaConsumo = vinculo === ETipoVinculoMpaConsumo.MPA ? programacao.mpa : (vinculo === ETipoVinculoMpaConsumo.CONSUMO ? programacao.consumo : '');
    return programacao.resistenciaTipo.abreviatura + ' ' + mpaConsumo + ' / ' + programacao.pedra.descricao + ' / ' + programacao.slump.descricao + ' / ' + programacao.uso.descricao;
  }

  statusProgramacaoFormatter = (model: number): string => {
    if (model === null || model === undefined || isNaN(model)) return '';
    if (model === 0 ) return '';
    return this.statusProgramacao.includes(model) ? statusProgramacao.filter(e => e.codigo === model)[0].descricao.toUpperCase() : '';
  };

}
