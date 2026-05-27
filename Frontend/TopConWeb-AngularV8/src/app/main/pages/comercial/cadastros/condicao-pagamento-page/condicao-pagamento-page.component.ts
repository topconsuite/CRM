import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Usina } from 'app/classes/usina/usina';
import { Mercadoria, Unidade } from 'app/classes/mercadoria/mercadoria';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';

import { UserService } from 'app/services/user.service';
import { CondicaoPagamentoService } from 'app/services/condicao-pagamento.service';
import { CondicaoPagamento, TipoCobranca } from 'app/classes/pagamento/pagamento.classes';
import { PagamentoService } from 'app/services/pagamento.service';
import { CadastroDiversoService } from 'app/services/cadastro-diverso.service';
import { CadastroDiverso } from 'app/classes/cadastro-geral/cadastro-diverso';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { CondicaoPagamentoParcelas } from 'app/classes/pagamento/condicao-pagamento';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { debug } from 'util';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-condicao-pagamento-page',
  templateUrl: './condicao-pagamento-page.component.html',
  styleUrls: ['./condicao-pagamento-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class CondicaoPagamentoPageComponent implements OnInit, AfterViewInit {
  public static self: CondicaoPagamentoPageComponent;

  condicaoPagamentoForm: FormGroup;

  itens: PagedList<CondicaoPagamento> = new PagedList<CondicaoPagamento>();
  item: CondicaoPagamento;

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    codigo: number,
    tipoCobrancaIn: TipoCobranca[],
    descricao: string
  } = {
    codigo: 0,
    tipoCobrancaIn: [],
    descricao: ''
  };

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataErrosApi = Tasks.formataErrosApi;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  tiposCobrancas : TipoCobranca[] = [];
  diasSemanaFixo : CadastroDiverso[] =[];
  condicoesPagamento: CadastroDiverso[] = [];
  vencimentoEmDiaNaoUtil: CadastroDiverso[] = [];

  tiposCobrancasSelected: TipoCobranca[] = [];

  disableConfirmButton : boolean = true;

  tiposCobrancaFormatter = (model: TipoCobranca): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  condicoesPagamentoFormatter = (model: CadastroDiverso): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  diasSemanaFixoFormatter = (model: CadastroDiverso): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  vencimentoEmDiaNaoUtilFormatter = (model: CadastroDiverso): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  
  moedaFormatter = (item: number) => {
    let self = CondicaoPagamentoPageComponent.self;
    return self.formataValor(item, 2, true);
  }

  booleanFormatter(item: any): boolean {
    return item === 'S';
  }

  SimNaoFormatter(item: string): string {
    return item.toLowerCase() === 's' ? 'Sim' : 'Não';
  }

  _allColumns: TableColumn[] = [
    { name: 'codigo', placeholder: 'Código', order: 1, priority: 1 },
    { name: 'descricao', placeholder: 'Descrição', order: 2, priority: 2},
    { name: 'quantidadeParcelas', placeholder: 'Quantidade de Parcelas', order: 3, priority: 3},
    { name: 'condicaoDaCobranca', placeholder: 'Condição do Pagamento', order: 4, priority: 4, getValue: this.getCondicaoDaCobranca},
    { name: 'ativo', placeholder: 'Condicão Ativa', order: 5, priority: 5, formatter: this.SimNaoFormatter},
    { name: 'analisaFraude', placeholder: 'Analisa Fraude', order: 6, priority: 6, formatter: this.SimNaoFormatter}
  ];

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: CondicaoPagamento | null;

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
    return ['expand','edit','delete'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = CondicaoPagamentoPageComponent.self;
    
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
    var self = CondicaoPagamentoPageComponent.self;
    
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
    var self = CondicaoPagamentoPageComponent.self;

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


  get dataSource(): CondicaoPagamento[] {
    return this.itens.records;
  };

  get tiposDeCobrancaCodigos(): string {
    if (!this.filtro.tipoCobrancaIn)
      return '';
    return this.filtro.tipoCobrancaIn.map(t => t.codigo).join(',');
  }

  getCondicaoDaCobranca(item: CondicaoPagamento): string {
    if (item.condicaoDaCobranca === null) return '';
    return `${item.condicaoDaCobranca.codigo} - ${item.condicaoDaCobranca.descricao} `;
  }

  getTiposCobrancaItem(item: CondicaoPagamento){
    var codigos = item.tiposDeCobrancaCodigos.split(',');
    return this.tiposCobrancas.filter(t => codigos.includes(t.codigo.toString()));
  }

  parcelaExistente(parcelaNumero: number): boolean {
    return this.item.quantidadeParcelas >= parcelaNumero;
  }

  exibirAcompanhamento: boolean = false;


  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('dsModalVCR', { read: ViewContainerRef, static: false }) CondicaoPagamentoModalVCR: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _condicaoPagamentoService: CondicaoPagamentoService,
    private _pagamentoService: PagamentoService,
    private _cadastroDiversoService: CadastroDiversoService
  ) {
    CondicaoPagamentoPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('CON0012','', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Cadastro", 12);

    this._cadastroDiversoService.listarCondicoesPagamentos().then(
      condicoes => { 
        this.condicoesPagamento = condicoes; },
      error => { this.condicoesPagamento = []; }
    );

    this._cadastroDiversoService.listarDiasDaSemanaFixo().then(
      dias => { 
        this.diasSemanaFixo = dias; },
      error => { this.diasSemanaFixo = []; }
    );

    this._cadastroDiversoService.listarOpcoesDeVencimentoEmDiaNaoUtil().then(
      opcoes => { 
        this.vencimentoEmDiaNaoUtil = opcoes; },
      error => { this.vencimentoEmDiaNaoUtil = []; }
    );
    
    this._pagamentoService.ListarTiposCobranca().then(
      tiposCobrancas => { this.tiposCobrancas = tiposCobrancas; },
      error => { this.tiposCobrancas = []; }
    );

  }

  ngOnInit() {
    this.condicaoPagamentoForm = this._formBuilder.group({
      analisaFraude: [''],
      mesFixo30Dias: [''],
      retencaoPrimeiraParcela: [''],
      ativo: [''],
      vencimentoFixoSemana: ['']
    });
  }
  
  ngAfterViewInit(): void {
    
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
    //this.filtro = newFilter;
    Object.keys(newFilter).forEach(t => this.filtro[t] = newFilter[t]);
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };
    
    this._condicaoPagamentoService.listarFiltrados(currentPage, pageSize, this.filtroString)
    .then(
      CondicaoPagamento => {
        this.itens = CondicaoPagamento;
        this.paginaAtual = CondicaoPagamento.currentPage;
        this.registrosPorPagina = CondicaoPagamento.pageSize;
      },
      error => { this.itens = new PagedList<CondicaoPagamento>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  getFormattedValue(element: CondicaoPagamento, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
  }

  checkBoxChange(evento: any, propriedade: string) {
    switch (propriedade) {
      case "analisaFraude":
        this.item.analisaFraude = evento.checked ? "S" : "N";
        break;
      case "mesFixo30Dias":
        this.item.mesFixo30Dias = evento.checked ? "S" : "N";
        break;
      case "retencaoPrimeiraParcela":
        this.item.retencaoPrimeiraParcela = evento.checked ? "S" : "N";
        break;
      case "ativo":
        this.item.ativo = evento.checked ? "S" : "N";
        break;
      case  "vencimentoFixoSemana":
        if (evento.checked) {
          this.item.vencimentoFixoSemana = "S";
        } else {
          this.item.vencimentoFixoSemana = "N";
          this.item.diaVencimentoFixoSemana = 0;
        }
        break;
      default:
        break;
    }
  }

  showSelecaoColunasModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  getDiaVencimentoFixoSemana(item: CondicaoPagamento): CadastroDiverso {
    return this.diasSemanaFixo.filter(t => t.codigo === item.diaVencimentoFixoSemana.toString())[0];
  }

  setDiaVencimentoFixoSemana(item: CadastroDiverso) {
    this.item.diaVencimentoFixoSemana = Number(item.codigo);
  }

  getVencimentoEmDiaNaoUtil(item: CondicaoPagamento): CadastroDiverso {
    return this.vencimentoEmDiaNaoUtil.filter(t => t.codigo === item.diaUltimoVencimento.toString())[0];
  }

  setVencimentoEmDiaNaoUtil(item: CadastroDiverso) {
    if (item === null) return;
    this.item.diaUltimoVencimento = Number(item.codigo);
  }

  SetCondicaoDaCobranca() {
    if(this.item === null) return;
    this.item.condicaoDaCobrancaCod = this.item.condicaoDaCobranca.codigo;
  }

  quantidadeParcelasChange(quantidadeParcelas: number){
    this.disableConfirmButton = false;
    if (this.item.parcelas.length > 0 && this.item.parcelas.length === quantidadeParcelas) return;
    
    this.item.parcelas = [];
    this.removeDataParcelas();

    if (quantidadeParcelas > 12) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `O máximo de parcelas permitidas é 12!`
        }
      });
      this.item.quantidadeParcelas = 0;
    }

    var totalPercentual = 0.0;
    for (let index = 1; index <= quantidadeParcelas; index++) 
    {
      var newParcela = new CondicaoPagamentoParcelas();
      newParcela.condicaoPagamentoCodigo = this.item.codigo;
      newParcela.dias = 0;

      if (index < quantidadeParcelas ) {
        newParcela.percentual = Number((100/quantidadeParcelas).toFixed(5));
        totalPercentual +=newParcela.percentual
      } else {
        newParcela.percentual = Number((100 - totalPercentual).toFixed(5));
      }

      this.item.parcelas.push(newParcela);
    }
   this._cdr.detectChanges();
   this.disableConfirmButton = true;
  }

  removeDataParcelas() {
    this.item.vencimento1Parcela = 0;
    this.item.vencimento2Parcela = 0;
    this.item.vencimento3Parcela = 0;
    this.item.vencimento4Parcela = 0;
    this.item.vencimento5Parcela = 0;
    this.item.vencimento6Parcela = 0;
    this.item.vencimento7Parcela = 0;
    this.item.vencimento8Parcela = 0;
    this.item.vencimento9Parcela = 0;
    this.item.vencimento10Parcela = 0;
    this.item.vencimento11Parcela = 0;
    this.item.vencimento12Parcela = 0;
   }

  atualizaTipoCobrancaCod(){
    this.item.tiposDeCobrancaCodigos = this.tiposCobrancasSelected.map(t => t.codigo).join(',');
  }

  atualizaCondicaoCobrancaCod(){
    this.item.condicaoDaCobrancaCod = this.item.condicaoDaCobranca.codigo;
  }

  atualizaDiasParcelas() {
    for (let index = 0; index < this.item.parcelas.length; index++) {
      this.item.parcelas[index].dias = this.getDiaDaParcela(index + 1);
    }
  }
  getDiaDaParcela(numeroParcela: number) : number {
    switch (numeroParcela) {
      case 1:
        return this.item.vencimento1Parcela;
      case 2:
        return this.item.vencimento2Parcela;
      case 3:
        return this.item.vencimento3Parcela;
      case 4:
        return this.item.vencimento4Parcela;
      case 5:
        return this.item.vencimento5Parcela;
      case 6:
        return this.item.vencimento6Parcela;
      case 7:
        return this.item.vencimento7Parcela;
      case 8:
        return this.item.vencimento8Parcela;
      case 9:
        return this.item.vencimento9Parcela;
      case 10:
        return this.item.vencimento10Parcela;
      case 11:
        return this.item.vencimento11Parcela;
      case 12:
        return this.item.vencimento12Parcela;
      default:
        break;
    }
  }


  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, condicaoPagamento?: CondicaoPagamento) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;
    this.tiposCobrancasSelected = [];
    this.mediaDiasAntiga = 0;

    if (condicaoPagamento) {
      var temDireito = this._userService.temDireitoAplicativo('CON0012','A');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de alteração!`
          }
        });
        return;
      }

      this.item = JSON.parse(JSON.stringify(condicaoPagamento));
      this.tiposCobrancasSelected = this.getTiposCobrancaItem(this.item);
    } else {
      var temDireito = this._userService.temDireitoAplicativo('CON0012','I');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão para inserir demais serviços!`
          }
        });
        return;
      }

      this.item = new CondicaoPagamento();
    }

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;

  }

  closeModal() {
    let self = CondicaoPagamentoPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.condicaoPagamentoForm.markAsPristine();
    self.condicaoPagamentoForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  removeCondicaoPagamento(condicaoPagamento: CondicaoPagamento): void {
    let self = CondicaoPagamentoPageComponent.self;

    var temDireito = this._userService.temDireitoAplicativo('CON0012','E');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de exclusão!`
        }
      });
      return;
    }

   self._condicaoPagamentoService.possuiObrasUtilizando(condicaoPagamento.codigo)
   .then( result => {
        if(result) {
          self._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Condição de Pagamento não pode ser excluida, pois já está sendo utilizada!`
            }
          });
          return;
        }else {
          self.deleteCondicaoPagamento(condicaoPagamento);
        }
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao tentar excluir item.\n${JSON.stringify(err).substr(0, 50)}`
        }
      });
    });  
}

deleteCondicaoPagamento(condicaoPagamento: CondicaoPagamento): void {
  let self = CondicaoPagamentoPageComponent.self;

  self._condicaoPagamentoService.delete(condicaoPagamento.codigo)
    .then(success => {
      self.getPage();
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao tentar excluir item.\n${JSON.stringify(err).substr(0, 50)}`
        }
      });
    });
  }

  
  updateCondicaoPagamento(condicaoPagamento: CondicaoPagamento): void {
    let self = CondicaoPagamentoPageComponent.self;

    this.atualizaTipoCobrancaCod();
    this.atualizaCondicaoCobrancaCod();
    this.atualizaDiasParcelas();
    self._condicaoPagamentoService.atualizar(condicaoPagamento)
    .then(success => {
      self.closeModal();
      self.getPage();
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Condição de Pagamento Alterada com sucesso!`
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

  addCondicaoPagamento(condicaoPagamento: CondicaoPagamento): void {
    let self = CondicaoPagamentoPageComponent.self;

    this.atualizaTipoCobrancaCod();
    this.atualizaCondicaoCobrancaCod();
    this.atualizaDiasParcelas();
    self._condicaoPagamentoService.adicionar(condicaoPagamento)
    .then(success => {
      self.closeModal();
      self.getPage();
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Condição de Pagamento Adicionada com sucesso!`
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

  calculoMediaDias(condicaoPagamento: CondicaoPagamento): number {
    const mediaParcial = this.somarDias(condicaoPagamento) / condicaoPagamento.quantidadeParcelas;
    switch (condicaoPagamento.condicaoDaCobrancaCod) {
        case "D":
            return mediaParcial + 10 / 2;
        case "M":
            return mediaParcial + 30 / 2;
        case "Q":
            return mediaParcial + 15 / 2;
        case "S":
            return mediaParcial + 7 / 2;
        case "E":
        case "F":
        default:
            return mediaParcial;
    }
  }

  somarDias(condicaoPagamento: CondicaoPagamento): number {
      let soma = 0;
  
      const vencimentos = [
          condicaoPagamento.vencimento1Parcela,
          condicaoPagamento.vencimento2Parcela,
          condicaoPagamento.vencimento3Parcela,
          condicaoPagamento.vencimento4Parcela,
          condicaoPagamento.vencimento5Parcela,
          condicaoPagamento.vencimento6Parcela,
          condicaoPagamento.vencimento7Parcela,
          condicaoPagamento.vencimento8Parcela,
          condicaoPagamento.vencimento9Parcela,
          condicaoPagamento.vencimento10Parcela,
          condicaoPagamento.vencimento11Parcela,
          condicaoPagamento.vencimento12Parcela
      ];
  
      vencimentos.forEach(vencimento => {
          soma += vencimento || 0; // Ignorar valores `undefined` ou `null`
      });
  
      return soma;
  }


  mediaDiasAntiga: number = 0;
  mediaDiasRecalculoConfirmado: boolean = false;
  onFocusMediaDias() {

    if(this.item.mediaDias > 0 && !this.mediaDiasRecalculoConfirmado) {

      this._dialog.open(ConfirmDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Deseja recalcular a média de dias ?`,
          confirmCallback: async () => {
            var novaMedia = this.calculoMediaDias(this.item);
            this.mediaDiasAntiga = this.item.mediaDias;
            this.item.mediaDias = novaMedia;
          }
        }
      });

      this.mediaDiasRecalculoConfirmado = true;

    } else if(this.mediaDiasRecalculoConfirmado) {
      this.mediaDiasRecalculoConfirmado = false;
    } else {
      var novaMedia = this.calculoMediaDias(this.item);
      this.mediaDiasAntiga = this.item.mediaDias;
      this.item.mediaDias = novaMedia;
    }
    
  }


}

