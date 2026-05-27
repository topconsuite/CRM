import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef, Input } from '@angular/core';
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

import { Uso } from 'app/classes/traco/uso';
import { Pedra } from 'app/classes/traco/pedra';
import { UsinaService } from 'app/services/usina.service';
import { Slump } from 'app/classes/traco/slump';
import { ETipoVinculoMpaConsumo, ResistenciaTipo } from 'app/classes/traco/resistencia-tipo';
import { TracoPreco } from 'app/classes/traco/traco-preco';
import { PreTracoPrecoService } from 'app/services/pre-traco-preco.service';
import { PreTracoPreco } from 'app/classes/pre-traco-preco/pre-traco-preco';
import { ReturnStatement } from '@angular/compiler';
import { ETipoAlteracaoLoteTabelaVenda } from 'app/classes/tabela-venda/tabela-venda';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { TracoPrecoNumeracaoProduto } from 'app/classes/traco/traco-preco-numeracao-produto';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
  htmlValue?: boolean;
  getIcon?: any;
  getIconClass?: any;
  align: string;
}

export interface EdicaoTabelaVenda {
  valorServicoAtual: number;
  valorCustoAtual: number;
  valorMarkupAtual: number;
  valorPrecoM3Atual: number;

  valorCusto: number;
  valorServico: number;
  valorMarkup: number;
  valorPrecoM3: number;

  novoTraco: boolean;
}

@Component({
  selector: 'app-tabela-venda-aprovacao-page',
  templateUrl: './tabela-venda-aprovacao-page.component.html',
  styleUrls: ['./tabela-venda-aprovacao-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})

export class TabelaVendaAprovacaoPageComponent implements OnInit {

  public static self: TabelaVendaAprovacaoPageComponent;
  tabelaVendaAprovacaoForm: FormGroup;
  alteracaoLoteForm: FormGroup;  
  segmentacaoForm: FormGroup;

  itens: PagedList<PreTracoPreco> = new PagedList<PreTracoPreco>();
  item: PreTracoPreco = new PreTracoPreco();
  itemValores: EdicaoTabelaVenda;
  itemTracoString: string = "";

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    usina: Usina,
    numeracaoProduto: TracoPrecoNumeracaoProduto,
    uso: Uso,
    pedra: Pedra,
    slump: Slump,
    resistenciaTipo: ResistenciaTipo,
    mpa: number,
    consumo: number
  } = {
    usina: null,
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
    manterPrecoVenda: number,
    valorMarkup: number,
    valorMarkupFixo: number,
    valorAlteracaoLote: ETipoAlteracaoLoteTabelaVenda,
  } = {
    valorServicoPorcentagem: 0,
    valorServico: 0,
    valorServicoFixo: 0,
    manterPrecoVenda: 0,
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
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataValor(item, 2, true);
  }

  percentualFormatter = (item: number) => {
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataValor(item, 1, false) + "%";
  }

  booleanFormatter(item: any): boolean {
    return item === 'S';
  }

  SimNaoFormatter(item: string): string {
    return item.toLowerCase() === 's' ? 'Sim' : 'Não';
  }

  getNumeracaoProdutoValue(element: PreTracoPreco): string {
    return element.numeracaoProduto.toString();
  }

  _allColumns: TableColumn[] = [
    { name: 'usina', placeholder: 'Central', align: 'left', order: 1, priority: 3, formatter: this.usinaFormatter},
    { name: 'numeracaoProduto', placeholder: 'Numeração Produto', align: 'left', order: 2, priority: 4, getValue: this.getNumeracaoProdutoValue},
    { name: 'traco', placeholder: 'Descrição Produto', align: 'left', order: 3, priority: 1, getValue: this.tracoString, htmlValue: true},
    { name: 'custoMaterial', placeholder: 'Custo (R$)', align: 'left', order: 4, priority: 5, getValue: this.getCustoMaterialValue, htmlValue: true, getIcon: this.getCustoMaterialIcon, getIconClass: this.getCustoMaterialClass },
    { name: 'valorServico', placeholder: 'Serviço (R$)', align: 'left', order: 5, priority: 6, getValue: this.getValorServicoValue, htmlValue: true, getIcon: this.getValorServicoIcon, getIconClass: this.getValorServicoClass },
    { name: 'markup', placeholder: 'Markup (%)', align: 'left', order: 6, priority: 7, getValue: this.getMarkupValue, htmlValue: true },
    { name: 'precoM3', placeholder: 'Valor M3 (R$)', align: 'left', order: 7, priority: 2, getValue: this.getValorM3, htmlValue: true, getIcon: this.getValorM3Icon, getIconClass: this.getValorM3Class }
  ];

  iconeDiferencaPositivo: string = 'arrow_upward';
  iconeDiferencaNegativo: string = 'arrow_downward';

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: PreTracoPreco | null;

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
    return this.allColumns.filter(t => !this.hiddenColumns.includes(t.name));
  }

  get currentViewValue() {
    return { filter: this.filtro, hiddenColumns: this.hiddenColumns, customColumnOrder: this._customColumnOrder }
  }

  get fixedColumnsLeft(): string[] {
    return [];
  }

  get fixedColumnsRight(): string[] {
    return ['expand','confirm', 'disapprove'];
  }

  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = TabelaVendaAprovacaoPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => {
      var fixedColsTotalWidth = 260;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 210);
      var hiddenColumnsHighPriority = this.allColumns.filter(c => this.hiddenColumns.includes(c.name) && this.getPriority(c) < this.getPriority(t)).length;

      return (this.getPriority(t) - hiddenColumnsHighPriority) <= colsAllowed;
    });
  }

  get columnNames(): string[] {
    return this.fixedColumnsLeft.concat(this.displayedColumns.map(t => t.name)).concat(this.fixedColumnsRight);
  }

  get foldedColumns(): TableColumn[] {
    var self = TabelaVendaAprovacaoPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => !this.columnNames.includes(t.name));
  }

  get foldedColumnsForCreate(): TableColumn[] {
    var self = TabelaVendaAprovacaoPageComponent.self;
    
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
    var self = TabelaVendaAprovacaoPageComponent.self;

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

  get dataSource(): PreTracoPreco[] {
    return this.itens.records;
  };

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('dsModalVCR', { read: ViewContainerRef, static: false }) tabelaVendaAprovacaoModalVCR: ViewContainerRef;  
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
    private _preTracoPrecoService: PreTracoPrecoService,    
    private _segmentacaoService: SegmentacaoService
  ) {
    TabelaVendaAprovacaoPageComponent.self = this;

    this.carregaUsina();
    var temDireito = this._userService.temDireitoAplicativo('CON6105','', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Cadastro", 12);

  }

  ngOnInit() {
    this.tabelaVendaAprovacaoForm = this._formBuilder.group({
      numeroTabela: 0,
      tracoString: "",
      custoServico: 0.0,
      markup: 0.0,
      m3Preco: 0.0,
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
    if(this.filtro.usina != null) 
      usina = this.filtro.usina;

    this._preTracoPrecoService.ListarAguardandoCienciaPorPagina(currentPage, pageSize,(this.segmentacaoFiltro.segmentacao && this.segmentacaoFiltro.segmentacao.id != null) ? this.segmentacaoFiltro.segmentacao.id : -1, this.filtroString)
    .then(
      TracosPrecos => {
        this.itens = TracosPrecos;
        this.paginaAtual = TracosPrecos.currentPage;
        this.registrosPorPagina = TracosPrecos.pageSize;
      },
      error => { this.itens = new PagedList<PreTracoPreco>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  getFormattedValue(element: PreTracoPreco, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name]);
  }

  getCellMatIcon(element: PreTracoPreco, column: TableColumn) : string {
    return column.getIcon ? column.getIcon(element) : '';
  }

  getCellIconClass(element: PreTracoPreco, column: TableColumn) {
    return column.getIconClass ? column.getIconClass(element) : '';
  }

  filtroChange(novoFiltro: string){
    if(!this.filtro.numeracaoProduto)
      this.desabilitaCamposTraco = false;

    if(!this.filtro.uso && !this.filtro.pedra && !this.filtro.slump && !this.filtro.resistenciaTipo)
      this.desabilitaNumeracaoProduto = false;

    this.filtroString = novoFiltro;
    this.getPage();
  }

  tracoString(item: PreTracoPreco, geraTagNovo: boolean = true): string {
    let self = TabelaVendaAprovacaoPageComponent.self;
    if (!item.resistenciaTipo || !item.pedra || !item.slump || !item.uso) return '';

    let mpaConsumo = self.isVinculoMpa(item.resistenciaTipo) ? item.mpa : (self.isVinculoConsumo(item.resistenciaTipo) ? item.consumo : '');
    let tracoDescr = item.uso.codigo + " " + item.uso.descricao
    + ' - ' + item.pedra.descricao
    + ' - ' + item.slump.descricao
    + ' - ' + item.resistenciaTipo.abreviatura + ' ' + mpaConsumo;

    if(item.tracoPrecoVigente == undefined && geraTagNovo)
      tracoDescr = `<span class="mat-label-traco-new">[NOVO] </span> ${tracoDescr}`

    return tracoDescr
  }

  getCustoMaterialValue(element: PreTracoPreco) {

    let self = TabelaVendaAprovacaoPageComponent.self;
    
    if(element.tracoPrecoVigente == undefined)
      return `<span class="mat-label-new">${ self.formataValor(element.custoMaterial, 2, true) }</span>`

    var percentualDiferencia = self.calculoPercentualDiferencaMaterial(element);

    var result = `
            ${ self.formataValor(element.tracoPrecoVigente.custoMaterial, 2, true) }
            <span class="mat-label-new">/ ${ self.formataValor(element.custoMaterial, 2, true) }</span>
            <span class="${self.getCustoMaterialClass(element)}">${ self.getPercentualDiferencaMaterial(element) }</span>`;

    return result;

  }

  getCustoMaterialIcon(element: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;

    if(element.tracoPrecoVigente == undefined)
      return '';

    var percentualDiferencia = self.calculoPercentualDiferencaMaterial(element);
    
    return self.getIconeDiferenca(percentualDiferencia);
  }

  getCustoMaterialClass(element: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;
    var percentualDiferencia = self.calculoPercentualDiferencaMaterial(element);
    
    return self.getCorPercentual(percentualDiferencia);
  }

  getValorServicoValue(element: PreTracoPreco) {

    let self = TabelaVendaAprovacaoPageComponent.self;

    if(element.tracoPrecoVigente == undefined)
      return `<label class="mat-label-new">${ self.formataValor(self.calculoServico(element), 2, true) }</label>`

    var percentualDiferencia = self.calculoPercentualDiferencaServico(element);

    var result = `
            ${ self.formataValor(self.calculoServico(element.tracoPrecoVigente), 2, true) }
            <label class="mat-label-new">/ ${ self.formataValor(self.calculoServico(element), 2, true) }</label>
            <label class="${self.getCorPercentual(percentualDiferencia)}">${ self.getPercentualDiferencaServico(element) }</label>`;
    
    return result;

  }

  getValorServicoIcon(element: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;

    if(element.tracoPrecoVigente == undefined)
      return '';

    var percentualDiferencia = self.calculoPercentualDiferencaServico(element);
    
    return self.getIconeDiferenca(percentualDiferencia);
  }

  getValorServicoClass(element: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;
    var percentualDiferencia = self.calculoPercentualDiferencaServico(element);
    
    return self.getCorPercentual(percentualDiferencia);
  }

  getMarkupValue(element: PreTracoPreco) {

    let self = TabelaVendaAprovacaoPageComponent.self;

    if(element.tracoPrecoVigente == undefined)
      return `<label class="mat-label-new">${ self.formataValor(element.markup, 1, false) }</label>`

    var percentualDiferencia = self.calculoPercentualDiferencaMarkup(element);

    var result = `
            ${ self.formataValor(element.tracoPrecoVigente.markup, 1, false) }
            <label class="mat-label-new">/ ${ self.formataValor(element.markup, 1, false) }</label>`;
    
    return result;

  }

  getValorM3(element: PreTracoPreco) {

    let self = TabelaVendaAprovacaoPageComponent.self;

    if(element.tracoPrecoVigente == undefined)
      return `<label class="mat-label-new">${ self.formataValor(element.m3Preco, 2, true) }</label>`

    var percentualDiferencia = self.calculoPercentualDiferencaM3(element);

    var result = `
            ${ self.formataValor(element.tracoPrecoVigente.m3Preco, 2, true) }
            <label class="mat-label-new">/ ${ self.formataValor(element.m3Preco, 2, true) }</label>
            <label class="${self.getCorPercentual(percentualDiferencia)}">${ self.getPercentualDiferencaM3(element) }</label>`;

    return result;

  }

  getValorM3Icon(element: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;
    var percentualDiferencia = self.calculoPercentualDiferencaM3(element);
    
    return self.getIconeDiferenca(percentualDiferencia);
  }

  getValorM3Class(element: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;
    var percentualDiferencia = self.calculoPercentualDiferencaM3(element);
    
    return self.getCorPercentual(percentualDiferencia);
  }

  calculoPercentualDiferencaMaterial(item: PreTracoPreco): number { return (((item.custoMaterial * 100) / item.tracoPrecoVigente.custoMaterial) - 100); }

  getPercentualDiferencaMaterial(item: PreTracoPreco): string {
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataValor(self.calculoPercentualDiferencaMaterial(item), 3, true) + '%'
  }

  getCorPercentual(valor: number): string {
  
    if(valor > 0)
      return 'mat-label-positive' // Verde Positivo
    
    if(valor < 0)  
      return 'mat-label-negative' // Vermelho Negativo

    return 'mat-label-zero' // Preto Valor Zerado
    

  }

  getIconeDiferenca(valor: number): string {
    let self = TabelaVendaAprovacaoPageComponent.self;
    if(valor > 0)
      return self.iconeDiferencaPositivo;

    if(valor < 0)
      return self.iconeDiferencaNegativo;

    return '';
  }

  getCustoMaterialAtual(item: PreTracoPreco): string { 
    let self = TabelaVendaAprovacaoPageComponent.self;
    return '<b>' + self.formataMoeda(item.tracoPrecoVigente.custoMaterial) + '</b>'; 
  }

  getValorServicoAtual(item: PreTracoPreco): string { 
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataMoeda(self.calculoServico(item.tracoPrecoVigente)); 
  }

  getValorServicoNovo(item: PreTracoPreco): string { 
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataMoeda(self.calculoServico(item)); 
  }

  calculoServico(item: PreTracoPreco | TracoPreco): number {  return ((item.m3Preco) * ((100 - item.markup) /100)) - item.custoMaterial }

  calculoPercentualDiferencaServico(item: PreTracoPreco): number { 
    let self = TabelaVendaAprovacaoPageComponent.self;
    return (((self.calculoServico(item) * 100) / self.calculoServico(item.tracoPrecoVigente)) - 100); 
  }

  getPercentualDiferencaServico(item: PreTracoPreco): string {
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataValor(self.calculoPercentualDiferencaServico(item), 3, true) + '%'
  }

  calculoPercentualDiferencaM3(item: PreTracoPreco): number { 
    let self = TabelaVendaAprovacaoPageComponent.self;
    if(item.tracoPrecoVigente == undefined) 
      return 0;
    return (((item.m3Preco * 100) / item.tracoPrecoVigente.m3Preco) - 100); 
  }

  calculoPercentualDiferenca(valorAntigo: number, valorNovo: number): number {

    if(valorAntigo == 0)
      return valorNovo;

    if(valorNovo == 0)
      return valorAntigo * -1;

    return (((valorNovo * 100) / valorAntigo) - 100); 
  }

  getPercentualDiferencaM3(item: PreTracoPreco): string {
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataValor(self.calculoPercentualDiferencaM3(item), 3, true) + '%'
  }

  calculoPercentualDiferencaMarkup(item: PreTracoPreco): number { 
    let self = TabelaVendaAprovacaoPageComponent.self;
    return (((item.markup * 100) / item.tracoPrecoVigente.markup) - 100); 
  }

  getPercentualDiferencaMarkup(item: PreTracoPreco): string {
    let self = TabelaVendaAprovacaoPageComponent.self;
    return self.formataValor(self.calculoPercentualDiferencaMarkup(item), 3, true) + '%'
  }

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
    if(this.filtro.usina && this.segmentacaoFiltro.segmentacao)
      this.numeracoesProdutoFiltered = this.numeracoesProduto.filter(t => t.usinaBase == this.filtro.usina.codigo && t.idSegmentacao == this.segmentacaoFiltro.segmentacao.id);
    else if(this.filtro.usina)
      this.numeracoesProdutoFiltered = this.numeracoesProduto.filter(t => t.usinaBase == this.filtro.usina.codigo);
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
    this._tracoPrecoService.ListarMpasPorDataUsinaUsoPedraSlumpResistenciaTipo(this.filtro.usina, this.filtro.uso, this.filtro.pedra, this.filtro.slump, this.filtro.resistenciaTipo).then(
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
        if(this.filtro.usina == null) this.filtro.usina = this.usinas[0];
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
    let self = TabelaVendaAprovacaoPageComponent.self;

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
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, preTracoPreco?: PreTracoPreco) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;
    this.item = preTracoPreco;
    
    this.itemValores = {
      valorCusto: this.item.custoMaterial,
      valorMarkup: this.item.markup,
      valorPrecoM3: this.item.m3Preco,
      valorServico: this.calculoServico(this.item),

      valorCustoAtual: this.item.tracoPrecoVigente != undefined ? this.item.tracoPrecoVigente.custoMaterial : 0,
      valorMarkupAtual: this.item.tracoPrecoVigente != undefined ? this.item.tracoPrecoVigente.markup : 0,
      valorPrecoM3Atual: this.item.tracoPrecoVigente != undefined ? this.item.tracoPrecoVigente.m3Preco : 0,
      valorServicoAtual: this.item.tracoPrecoVigente != undefined ? this.calculoServico(this.item.tracoPrecoVigente) : 0,
      
      novoTraco : this.item.tracoPrecoVigente == undefined
    };

    this.markupAntigo = this.item.markup;

    this.itemTracoString = this.tracoString(preTracoPreco, false);

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
    let self = TabelaVendaAprovacaoPageComponent.self;

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

    self.item.valorServico = self.itemValores.valorServico;
    self.item.markup = self.itemValores.valorMarkup;
    self.item.m3Preco = self.itemValores.valorPrecoM3;
    
    self._preTracoPrecoService.AtualizarConfirmarCiencia(this.item)
    .then(() => {
      self.closeModal();
      self.getPage();
      this.exibirAlerta('Alteração aprovada com sucesso!')
    }, err => {
      this.exibirAlerta(`${this.formataErrosApi(err)}`);
    });

  }

  aprovarTodos(){
    let self = TabelaVendaAprovacaoPageComponent.self;

    self._preTracoPrecoService.AprovarTodos(this.itens.records)
    .then(() => {
      self.closeModal();
      self.getPage();
      this.exibirAlerta('Alterações Aprovadas com sucesso!');
    }, err => {
      this.exibirAlerta(`${this.formataErrosApi(err)}`);
    });
  }

  reprovarTodos(){
    let self = TabelaVendaAprovacaoPageComponent.self;

    self._preTracoPrecoService.ReprovarTodos(this.itens.records)
    .then(() => {
      self.closeModal();
      self.getPage();
      this.exibirAlerta('Alterações Reprovadas com sucesso!');
    }, err => {
      this.exibirAlerta(`${this.formataErrosApi(err)}`);
    });
  }

  reprovarValorVenda() {
    let self = TabelaVendaAprovacaoPageComponent.self;

    self.item.valorServico = self.itemValores.valorServico;
    self.item.markup = self.itemValores.valorMarkup;
    self.item.m3Preco = self.itemValores.valorPrecoM3;

    self._preTracoPrecoService.ReprovarAlteracao(this.item)
    .then(() => {
      self.closeModal();
      self.getPage();
      this.exibirAlerta('Alteração Reprovada com sucesso!');
    }, err => {
      this.exibirAlerta(`${this.formataErrosApi(err)}`);
    });
  }

  validarCustoEValorServico(content, preTracoPreco?: PreTracoPreco) {
    let self = TabelaVendaAprovacaoPageComponent.self;
    preTracoPreco = preTracoPreco ? preTracoPreco : this.item;

    if(preTracoPreco.custoMaterial > preTracoPreco.tracoPrecoVigente.m3Preco) {
      this.closeModal();
      self.openModal(content);
      return;
    }

    this.reprovarValorVenda();
  }

  validarCustoEValorServicos(content) {
    let self = TabelaVendaAprovacaoPageComponent.self;
    let possuiInconsistencia = false

    for(let preTracoPreco of this.itens.records){
      if(preTracoPreco.custoMaterial > preTracoPreco.tracoPrecoVigente.m3Preco) {
        possuiInconsistencia = true;
        break;
      }
    }

    if (possuiInconsistencia) {
      this.closeModal();
      self.openModal(content);
      return;
    }

    this.reprovarTodos();
  }

  exibirAlerta(mensagem: string, titulo: string = 'TopConWeb') {
    let self = TabelaVendaAprovacaoPageComponent.self;
    self._dialog.open(AlertDialogComponent, {
      disableClose: true,
      data: {
        title: titulo,
        message: mensagem
      }
    });
  }

  abrirConfirmacaoModal(content) {
    this.openModal(content);
  }

  openModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.tabelaVendaAprovacaoModalVCR });
    this.modalIsOpen = true;
  }

  closeModal() {
    let self = TabelaVendaAprovacaoPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.tabelaVendaAprovacaoForm.markAsPristine();
    self.tabelaVendaAprovacaoForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  updateTabelaVendaLote() {
    let self = TabelaVendaAprovacaoPageComponent.self;   
    
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
      case ETipoAlteracaoLoteTabelaVenda.ManterPrecoVenda:
        self.valorLote = self.edicaoLote.manterPrecoVenda;
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

    if(self.valorLote === 0 && self.edicaoLote.valorAlteracaoLote != ETipoAlteracaoLoteTabelaVenda.MarkupFixo && self.edicaoLote.valorAlteracaoLote != ETipoAlteracaoLoteTabelaVenda.ManterPrecoVenda) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Valor não pode ser igual a 0.`
        }
      });
      return;
    }
    
    self._preTracoPrecoService.AtualizarLote(this.itens.records,self.edicaoLote.valorAlteracaoLote,self.valorLote)
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
