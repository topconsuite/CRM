import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef, MatDialog, throwToolbarMixedModesError } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Usina } from 'app/classes/usina/usina';
import { Obra } from 'app/classes/obra/obra';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';

import { UserService } from 'app/services/user.service';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';

import { TracoPrecoService } from 'app/services/traco-preco.service';
import { SlumpService } from 'app/services/slump.service';
import { PedraService } from 'app/services/pedra.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';

import { Uso } from 'app/classes/traco/uso';
import { Pedra } from 'app/classes/traco/pedra';
import { UsinaService } from 'app/services/usina.service';
import { Slump } from 'app/classes/traco/slump';
import { ETipoVinculoMpaConsumo, ResistenciaTipo } from 'app/classes/traco/resistencia-tipo';
import { TracoPreco } from 'app/classes/traco/traco-preco';
import { ETipoAlteracaoLoteTabelaVenda } from 'app/classes/tabela-venda/tabela-venda';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { TracoPrecoNumeracaoProduto } from 'app/classes/traco/traco-preco-numeracao-produto';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

export interface EdicaoTabelaVenda {
  usinaBase: string;
  valorCusto: number;
  valorServico: number;
  valorMarkup: number;
  valorPrecoM3: number
}

@Component({
  selector: 'app-tabela-venda-page',
  templateUrl: './tabela-venda-page.component.html',
  styleUrls: ['./tabela-venda-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})

export class TabelaVendaPageComponent implements OnInit, AfterViewInit {
  public static self: TabelaVendaPageComponent;
  tabelaVendaForm: FormGroup;
  alteracaoLoteForm: FormGroup;
  segmentacaoForm: FormGroup;

  itens: PagedList<TracoPreco> = new PagedList<TracoPreco>();
  item: TracoPreco = new TracoPreco();
  itemValores: EdicaoTabelaVenda;
  itemTracoString: string = "";

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    usinaBase: Usina,
    numeracaoProduto: TracoPrecoNumeracaoProduto,
    uso: Uso,
    pedra: Pedra,
    slump: Slump,
    resistenciaTipo: ResistenciaTipo,
    mpa: number,
    consumo: number
  } = {
    usinaBase: null,
    numeracaoProduto: null,
    uso: null,
    pedra: null,
    slump: null,
    resistenciaTipo: null,
    mpa: 0,
    consumo: 0
  };

  valorLote:number = 0; 

  edicaoLote: {
    valorServicoPorcentagem: number,
    valorServico: number,
    valorServicoFixo: number,
    valorMarkup: number,
    valorMarkupFixo: number,
    valorAlteracaoLote: ETipoAlteracaoLoteTabelaVenda,
  } = {
    valorServicoPorcentagem: 0,
    valorServico: 0,
    valorServicoFixo: 0,
    valorMarkup: 0,
    valorMarkupFixo: 0,
    valorAlteracaoLote: ETipoAlteracaoLoteTabelaVenda.ValorServicoPorcentagem,
  };

  segmentacaoFiltro: {
    segmentacao: Segmentacao
  } = {
    segmentacao: null
  };

  dataBase: Date = new Date();
  markupAntigo: number = 0;

  usinas: Usina[] = [];
  usos: Uso[] = [];
  pedras: Pedra[] = [];
  slumps: Slump[] = [];
  resistencias: ResistenciaTipo[] = [];
  mpas: Number[] = [];  
  segmentacao: Segmentacao[] = [];
  numeracoesProduto: TracoPrecoNumeracaoProduto[] = [];
  numeracoesProdutoFiltered: TracoPrecoNumeracaoProduto[] = [];
  numeracoesProdutoGroupedBy: TracoPrecoNumeracaoProduto[] = [];

  desabilitaNumeracaoProduto: boolean = false;
  desabilitaCamposTraco: boolean = false;

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;
  formataErrosApi = Tasks.formataErrosApi;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;


  disableConfirmButton : boolean = false;

  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';
  usoFormatter = (model: Uso): string => model ? (model.codigo + ' - ' + model.descricao).toUpperCase() : '';
  pedraFormatter = (model: Pedra): string => model ? (model.codigo + ' - ' + model.descricao).toUpperCase() : '';
  slumpFormatter = (model: Slump): string => model ? (model.codigo + ' - ' + model.descricao).toUpperCase() : '';
  resistenciaFormatter = (model: ResistenciaTipo): string => model ? (model.codigo + ' - ' + model.descricao).toUpperCase() : '';
  mpaFormatter = (model: Number): string => model.toString();
  segmentacaoFormatter = (model: Segmentacao) => model ? model.id+' - '+model.nome.toUpperCase() : '';

  numeracaoProdutoFormatter = (model: TracoPrecoNumeracaoProduto): string => {
    if (model===null || model===undefined || model.numeracao === 0) return '';
    return model ? (model.numeracao+' - '+model.usoDescricao).toUpperCase() : '';
  };

  moedaFormatter = (item: number) => {
    let self = TabelaVendaPageComponent.self;
    return self.formataValor(item, 2, true);
  }

  percentualFormatter = (item: number) => {
    let self = TabelaVendaPageComponent.self;
    return self.formataValor(item, 1, false) + "%";
  }

  booleanFormatter(item: any): boolean {
    return item === 'S';
  }

  SimNaoFormatter(item: string): string {
    return item.toLowerCase() === 's' ? 'Sim' : 'Não';
  }

  getNumeracaoProdutoValue(element: TracoPreco): string {
    return element.numeracaoProduto.toString();
  }

  _allColumns: TableColumn[] = [
    { name: 'usinaBase', placeholder: 'Central', order: 1, priority: 3, formatter: this.usinaFormatter},
    { name: 'numeracaoProduto', placeholder: 'Numeração Produto', order: 2, priority: 4, getValue: this.getNumeracaoProdutoValue},
    { name: 'traco', placeholder: 'Descrição Produto', order: 3, priority: 1, getValue: this.tracoString},
    { name: 'custoMaterial', placeholder: 'Custo (R$)', order: 4, priority: 5, formatter: this.formataMoeda},
    { name: 'servico', placeholder: 'Serviço (R$)', order: 5, priority: 6, getValue: this.calculoServicoFormatado},
    { name: 'markup', placeholder: 'Markup (%)', order: 6, priority: 7, formatter: this.percentualFormatter},
    { name: 'm3Preco', placeholder: 'Preço M3 (R$)', order: 7, priority: 2, formatter: this.formataMoeda},
  ];

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: TracoPreco | null;

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

  get columns(): TableColumn[] {
    if(!this.temDireitoEdicao)
      this.hiddenColumns = ['custoMaterial', 'servico', 'markup'];

    return this.allColumns.filter(t => !this.hiddenColumns.includes(t.name));
  }

  get currentViewValue() {
    return { filter: this.filtro, hiddenColumns: this.hiddenColumns, customColumnOrder: this._customColumnOrder }
  }

  get fixedColumnsLeft(): string[] {
    return [];
  }
  get fixedColumnsRight(): string[] {
    return ['expand','edit'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = TabelaVendaPageComponent.self;
    
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
    var self = TabelaVendaPageComponent.self;
    
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
    var self = TabelaVendaPageComponent.self;

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

  get dataSource(): TracoPreco[] {
    return this.itens.records;
  };

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('dsModalVCR', { read: ViewContainerRef, static: false }) tabelaVendaModalVCR: ViewContainerRef;
  @ViewChild('alteracaoLoteModalVCR', { read: ViewContainerRef, static: false }) alteracaoLoteModalRef: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _tracoPrecoService: TracoPrecoService,
    private _usinaService: UsinaService,
    private _slumpService: SlumpService,
    private _pedraService: PedraService,    
    private _segmentacaoService: SegmentacaoService
  ) {
    TabelaVendaPageComponent.self = this;

    this.carregaUsina();

    this._userService.gravarAcessoAplicacao("Cadastro", 12);

  }

  ngOnInit() {
    this.tabelaVendaForm = this._formBuilder.group({
      numeroTabela: 0,
      tracoString: "",
      usinaBaseString: "",
      custoServico: 0.0,
      markup: 0.0,
      m3Preco: 0.0,
      dataInicioVigencia: new Date()
    });

    this.alteracaoLoteForm = this._formBuilder.group({
      percentualServico: 0.0,
      valorServico: 0.0,
      valorServicoFixo: 0.0,
      valorMarkup: 0.0,
      valorMarkupFixo: 0.0,
      valorAlteracaoLote: ETipoAlteracaoLoteTabelaVenda.ValorServicoPorcentagem,
    });

    this.segmentacaoForm = this._formBuilder.group({
      segmentacao: 0
    });
  }

  ngAfterViewInit(): void {

    this.carregaUsina();
    this.carregaSegmentacao();
    this.carregaUso();
    this.carregaPedra();
    this.carregaSlump();
    this.carregaResistencia();
    this.carregaMpa();
    this.carregaTracoNumeracoesProduto();

    this._cdr.detectChanges();
    this.filter.aplyFilter();
  }

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
    this.filtro = newFilter;
    Object.keys(newFilter).forEach(t => this.filtro[t] = newFilter[t]);
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };

    var usina = new Usina();
    if(this.filtro.usinaBase != null) usina = this.filtro.usinaBase

    this._tracoPrecoService.ListarPorDataUsinaPagina(this.dataBase, usina, currentPage, pageSize, (this.segmentacaoFiltro.segmentacao && this.segmentacaoFiltro.segmentacao.id != null) ? this.segmentacaoFiltro.segmentacao.id : -1, this.filtroString)
    .then(
      TracosPrecos => {
        this.itens = TracosPrecos;
        this.paginaAtual = TracosPrecos.currentPage;
        this.registrosPorPagina = TracosPrecos.pageSize;
      },
      error => { this.itens = new PagedList<TracoPreco>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  getFormattedValue(element: TracoPreco, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  filtroChange(novoFiltro: string){
    if(!this.filtro.numeracaoProduto)
      this.desabilitaCamposTraco = false;

    if(!this.filtro.uso && !this.filtro.pedra && !this.filtro.slump && !this.filtro.resistenciaTipo)
      this.desabilitaNumeracaoProduto = false;

    this.filtroString = novoFiltro;
    this.getPage();
  }
  
  tracoString(item: TracoPreco): string {
    let self = TabelaVendaPageComponent.self;
    if (!item.resistenciaTipo || !item.pedra || !item.slump || !item.uso) return '';

    let mpaConsumo = self.isVinculoMpa(item.resistenciaTipo) ? item.mpa : (self.isVinculoConsumo(item.resistenciaTipo) ? item.consumo : '');
    let tracoDescr = '' + item.uso.codigo + " " + item.uso.descricao
    + ' - ' + item.pedra.descricao
    + ' - ' + item.slump.descricao
    + ' - ' + item.resistenciaTipo.abreviatura + ' ' + mpaConsumo;

    return tracoDescr
  }

  calculoServicoFormatado(item: TracoPreco): string { 
    let self = TabelaVendaPageComponent.self;
    return self.formataMoeda(self.calculoServico(item)); 
  }

  calculoServico(item: TracoPreco): number { return (item.m3Preco - ((item.m3Preco / 100) * item.markup)) - item.custoMaterial; }

  isVinculoMpa(resistenciaTipo: ResistenciaTipo): boolean {
    if (!resistenciaTipo) return false;
    return resistenciaTipo.vinculo === ETipoVinculoMpaConsumo.MPA;
  }
  isVinculoConsumo(resistenciaTipo: ResistenciaTipo): boolean {
    if (!resistenciaTipo) return false;
    return resistenciaTipo.vinculo === ETipoVinculoMpaConsumo.CONSUMO;
  }

  carregaSegmentacao() {
  this._segmentacaoService.listarTodos().then(
    segmentacoes => { this.segmentacao = segmentacoes; },
    error => { this.segmentacao = []; }
  );
  }

  carregaUso() {
    this._tracoPrecoService.ListarUsos().then(
      usos => { this.usos = usos },
      error => { this.usos = [] }
    );
    this.detectChanges();
  }

  private filtrarNumeracoesProduto() {
    if(this.filtro.usinaBase && this.segmentacaoFiltro.segmentacao)
      this.numeracoesProdutoFiltered = this.numeracoesProduto.filter(t => t.usinaBase == this.filtro.usinaBase.codigo && t.idSegmentacao == this.segmentacaoFiltro.segmentacao.id);
    else if(this.filtro.usinaBase)
      this.numeracoesProdutoFiltered = this.numeracoesProduto.filter(t => t.usinaBase == this.filtro.usinaBase.codigo);
    else if(this.segmentacaoFiltro.segmentacao)
      this.numeracoesProdutoFiltered = this.numeracoesProdutoGroupedBy.filter(t => t.idSegmentacao == this.segmentacaoFiltro.segmentacao.id);
    else
      this.numeracoesProdutoFiltered = this.numeracoesProdutoGroupedBy;
  }

  carregaUsoPorSegmentacao() {
    this._tracoPrecoService.ListarUsosPorSegmentacao(this.segmentacaoFiltro.segmentacao.id, true).then(
      usos => { this.usos = usos },
      error => { this.usos = [] }
    );
    this.filtrarNumeracoesProduto();
    this.detectChanges();
  }

  segmentacaoChange(uso: Uso) {
    this.carregaUsoPorSegmentacao();
  }

  carregaNumeracaoProdutoPorCentral() {
    this.filtrarNumeracoesProduto();
    this.detectChanges();
  }

  carregaTracoNumeracoesProduto() {
    this._tracoPrecoService.ListarNumeracoesProduto().then(
      numeracoesProduto => { 
        this.numeracoesProduto = numeracoesProduto; 

        const numeracoesUnicas = Array.from(
          new Map(this.numeracoesProduto.map(item => [item.numeracao, item])).values()
        );

        this.numeracoesProdutoGroupedBy = numeracoesUnicas;
        this.numeracoesProdutoFiltered = this.numeracoesProdutoGroupedBy;
      },
      error => { 
        this.numeracoesProduto = [];
        this.numeracoesProdutoFiltered = [];
        this.numeracoesProdutoGroupedBy = [];
       }
    );
    this.detectChanges();
  }

  carregaPedra() {
    this._pedraService.listarTodos().then(
      pedras => { this.pedras = pedras },
      error => { this.pedras = [] }
    );
    this.detectChanges();
  }

  carregaSlump() {
    this._slumpService.listarTodos().then(
      slumps => { this.slumps = slumps },
      error => { this.slumps = [] }
    );
    this.detectChanges();
  }

  carregaResistencia() {
    this._tracoPrecoService.ListarResistenciaTipos().then(
      resistencias => { this.resistencias = resistencias },
      error => { this.resistencias = [] }
    );
    this.detectChanges();
  }

  carregaMpa() {
    if(this.filtro.resistenciaTipo == null) return;
    this._tracoPrecoService.ListarMpasPorDataUsinaUsoPedraSlumpResistenciaTipo(this.filtro.usinaBase, this.filtro.uso, this.filtro.pedra, this.filtro.slump, this.filtro.resistenciaTipo).then(
      mpas => { this.mpas = mpas },
      error => { this.mpas = [] }
    );
    this.detectChanges();


  }

  carregaUsina() {
    this._usinaService.listarTodos().then(
      usinas => { this.usinas = usinas; },
      error => { this.usinas = [] }
    );
  }

  carregaUsinaAtualiza() {
    this._usinaService.listarTodos().then(
      usinas => { 
        this.usinas = usinas;
        if(this.filtro.usinaBase == null) this.filtro.usinaBase = this.usinas[0];
        this.getPage();
      },
      error => { this.usinas = [] }
    );
  }

  tracoChange() {
    if(!this.filtro.uso && !this.filtro.pedra && !this.filtro.slump && !this.filtro.resistenciaTipo)
      this.desabilitaNumeracaoProduto = false;
    else
      this.desabilitaNumeracaoProduto = true;
  }

  tracoNumeracaoProdutoChange(tracoPrecoNumeracaoProduto: TracoPrecoNumeracaoProduto) {
    if (!tracoPrecoNumeracaoProduto){
      this.desabilitaCamposTraco = false;

      this.filtro.uso = null;
      this.filtro.pedra = null;
      this.filtro.slump = null;
      this.filtro.resistenciaTipo = null;
      this.filtro.mpa = 0.0;
    }

    if (tracoPrecoNumeracaoProduto){
      this.desabilitaCamposTraco = true;

      if(!this.filtro.numeracaoProduto){
        this.filtro.numeracaoProduto = new TracoPrecoNumeracaoProduto();
      }
      this.filtro.numeracaoProduto.numeracao = tracoPrecoNumeracaoProduto.numeracao;
      this.filtro.numeracaoProduto.usoDescricao = tracoPrecoNumeracaoProduto.usoDescricao;
      this.carregaPreenchimentoTraco();
    } 
  }

  carregaPreenchimentoTraco(dispararChange: boolean = true) {
    let self = TabelaVendaPageComponent.self;

    if (self.filtro.numeracaoProduto && self.filtro.numeracaoProduto.usinaBase != 0 && self.filtro.numeracaoProduto.numeracao > 0) {
      var usinaNumeracaoProduto = new Usina();
      usinaNumeracaoProduto.codigo = self.filtro.numeracaoProduto.usinaBase;

      self._tracoPrecoService.ObterPorNumeracaoProduto(usinaNumeracaoProduto, self.filtro.numeracaoProduto.numeracao, new Obra(), false)
      .then(tracoPreco => {
          self.filtro.uso = tracoPreco.uso;
          self.filtro.pedra = tracoPreco.pedra;
          self.filtro.slump = tracoPreco.slump;
          self.filtro.resistenciaTipo = tracoPreco.resistenciaTipo;
          self.filtro.mpa = tracoPreco.mpa;
          self.filtro.numeracaoProduto.numeracao = tracoPreco.numeracaoProduto;
          self.filtro.numeracaoProduto.usoDescricao = tracoPreco.uso.descricao;
      });
    }
  }

  detectChanges(dalay: number = 0) {
    if (dalay)
      setTimeout(() => { this._cdr.detectChanges(); }, dalay);
    else
      this._cdr.detectChanges();
  }

  showSelecaoColunasModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, tracoPreco?: TracoPreco) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;
    this.item = tracoPreco;
    
    this.itemValores = {
      usinaBase: this.usinaFormatter(this.item.usinaBase),
      valorCusto: this.item.custoMaterial,
      valorMarkup: this.item.markup,
      valorPrecoM3: this.item.m3Preco,
      valorServico: this.calculoServico(this.item)
    };

    this.markupAntigo = this.item.markup;

    this.itemTracoString = this.tracoString(tracoPreco);

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;

  }

  showAlteracaoLoteModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  atualizaServico() {
    
    var novoM3 = this.itemValores.valorCusto + this.itemValores.valorServico;

    novoM3 = (novoM3 / (100 - this.itemValores.valorMarkup)) * 100;

    this.itemValores.valorPrecoM3 = novoM3;

  }

  atualizaMarkup() {

    if(this.markupAntigo == this.itemValores.valorMarkup)
      return;

    console.log(this.itemValores.valorCusto)
    console.log(this.itemValores.valorServico)

    var custoServico = this.itemValores.valorCusto + this.itemValores.valorServico;

    var novoM3 = (custoServico / (100 - this.itemValores.valorMarkup)) * 100;

    this.itemValores.valorPrecoM3 = novoM3;
    this.atualizaM3();

    this.markupAntigo = this.itemValores.valorMarkup;

  }

  atualizaM3() {

    var precoM3SemMarkup = ((this.itemValores.valorPrecoM3 / (100 + this.itemValores.valorMarkup)) * 100);

    var novoServico = precoM3SemMarkup - this.itemValores.valorCusto;

    this.itemValores.valorServico = novoServico;

  }

  updateTabelaVenda() {
    let self = TabelaVendaPageComponent.self;

    if(self.itemValores.valorCusto <= 0) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Custo de Serviço não pode ser igual ou inferior a 0.`
        }
      });
      return;
    }

    if(self.itemValores.valorPrecoM3 <= 0 || self.itemValores.valorPrecoM3 < self.itemValores.valorServico) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Preço do m3 não pode ser igual ou inferior ao Custo do serviço.`
        }
      });
      return;
    }

    self.item.markup = self.itemValores.valorMarkup;
    self.item.m3Preco = self.itemValores.valorPrecoM3;

    self._tracoPrecoService.atualizar(this.item)
    .then(success => {
      self.closeModal();
      self.getPage();
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Tabela de venda Alterada com sucesso!`
        }
      });
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `${this.formataErrosApi(err)}`
        }
      });
    });

  }

  closeModal() {
    let self = TabelaVendaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.tabelaVendaForm.markAsPristine();
    self.tabelaVendaForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  get temDireitoEdicao(): boolean{
    return this._userService.temDireitoAplicativo('CON6105','');
  }

  updateTabelaVendaLote() {
    let self = TabelaVendaPageComponent.self;   
    
    switch(self.edicaoLote.valorAlteracaoLote) {
      case ETipoAlteracaoLoteTabelaVenda.ValorServicoPorcentagem:
        self.valorLote = self.edicaoLote.valorServicoPorcentagem;
        break;
      case ETipoAlteracaoLoteTabelaVenda.ValorServicoReais:
        self.valorLote = self.edicaoLote.valorServico;
        break;
      case ETipoAlteracaoLoteTabelaVenda.ValorServicoFixo:
        self.valorLote = self.edicaoLote.valorServicoFixo;
        break;
      case ETipoAlteracaoLoteTabelaVenda.MarkupPorcentagem:
        self.valorLote = self.edicaoLote.valorMarkup;
        break;
      case ETipoAlteracaoLoteTabelaVenda.MarkupFixo:
        self.valorLote = self.edicaoLote.valorMarkupFixo;
        break;
      default:
        break;
    }

    if(self.valorLote === 0 && self.edicaoLote.valorAlteracaoLote != ETipoAlteracaoLoteTabelaVenda.MarkupFixo) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Valor não pode ser igual a 0.`
        }
      });
      return;
    }
    self._tracoPrecoService.AtualizarLote(self.itens.records,self.edicaoLote.valorAlteracaoLote,self.valorLote)
    .then(success => {
      self.closeModal();
      self.getPage();
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Alteração em Lote realizada com sucesso!`
        }
      });
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `${this.formataErrosApi(err)}`
        }
      });
    });
  } 

}
