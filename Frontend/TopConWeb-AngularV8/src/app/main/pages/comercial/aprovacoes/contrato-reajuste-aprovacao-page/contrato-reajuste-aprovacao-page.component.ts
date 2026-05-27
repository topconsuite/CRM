import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef, Input } from '@angular/core';
import { MatDialogRef, MatDialog, throwToolbarMixedModesError } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';

import { UserService } from 'app/services/user.service';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';

import { IntervenienteService } from 'app/services/interveniente.service';
import { ContratoReajusteService } from 'app/services/contrato-reajuste.service';
import { ContratoReajuste, EStatusReajusteContrato, statusReajusteContrato } from 'app/classes/contrato-reajuste/contrato-reajuste';
import { ContratoReajusteVigencia } from 'app/classes/contrato-reajuste/contrato-reajuste-vigencia';
import { Interveniente } from 'app/classes/interveniente/interveniente';
import { Usina } from 'app/classes/usina/usina';
import { UsinaService } from 'app/services/usina.service';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { VendedorService } from 'app/services/vendedor.service';
import { ContratoReajusteLog } from 'app/classes/contrato-reajuste/contrato-reajuste-log';
import { element } from 'protractor';

export enum ETipoReajuste {
  Concreto = 0,
  Bomba = 1
}

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

@Component({
  selector: 'app-contrato-reajuste-aprovacao-page',
  templateUrl: './contrato-reajuste-aprovacao-page.component.html',
  styleUrls: ['./contrato-reajuste-aprovacao-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})

export class ContratoReajusteAprovacaoPageComponent implements OnInit {

  public static self: ContratoReajusteAprovacaoPageComponent;
  reajusteAprovacaoForm: FormGroup;

  itens: PagedList<ContratoReajuste> = new PagedList<ContratoReajuste>();
  item: ContratoReajuste = new ContratoReajuste();

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    tipoReajuste: number,
    usina: number,
    vendedor: Vendedor,
    inicioValidade: ContratoReajusteVigencia,
    interveniente: Interveniente,
    intervenienteRazao: string,
    contratoAno: number,
	  contratoNumero: number,
    status: EStatusReajusteContrato,
    pessoaFisica: boolean,
    pessoaJuridica: boolean,
    construtora: boolean
  } = {
    tipoReajuste: ETipoReajuste.Concreto,
    usina: 0,
    vendedor: null,
    inicioValidade: null,
    interveniente: null,
    intervenienteRazao: '',
    contratoAno: 0,
	  contratoNumero: 0,
    status: EStatusReajusteContrato.Pendente,
    pessoaFisica: false,
    pessoaJuridica: false,
    construtora: false
  };

  vigencias: ContratoReajusteVigencia[] = [];
  intervenientes: Interveniente[] = [];
  usinas: Usina[] = [];
  vendedoresPermitidos: Vendedor[] = [];
  logs: ContratoReajusteLog[] = [];

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;
  formataErrosApi = Tasks.formataErrosApi;
  formataDataHora = Tasks.formataDataHora;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  disableConfirmButton : boolean = false;

  inicioValidadeFormatter = (model: ContratoReajusteVigencia): string => model ? this.formataData(model.vigencia) : '';
  
  dataCartaFormatter = (model: Date) => model === null ? '' : this.formataData(model);

  intervenienteFormatter = (model: Interveniente) => model ? model.codigo + ' - ' + model.razao.toUpperCase() : '';
  vendedorFormatter = (model: Vendedor) => model ? model.codigo+' - '+model.nome.toUpperCase() : '';

  usinaEntregaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.usinasCodigos.includes(model) ? model + ' - ' +  this.usinas.filter(e => e.codigo===model)[0].nome.toUpperCase() : '';
  };

  statusFormatter = (model: number): string => {
    if (model === null || model === undefined || isNaN(model)) return '';
    return this.statusReajuste.includes(model) ? statusReajusteContrato.filter(e => e.codigo === model)[0].descricao.toUpperCase() : '';
  };

  getContratoNumero(contratoReajuste: ContratoReajuste): string {
    if (!contratoReajuste.contratoNumero) return 'NÃO GERADO';
    return contratoReajuste.contratoNumero.toString().padStart(6,'0')+'-'+contratoReajuste.contratoAno;
  }

  getUsina(contratoReajuste: ContratoReajuste): string {
    return contratoReajuste.usinaEntrega.codigo + ' - ' + contratoReajuste.usinaEntrega.nome;
  }

  getNomeObra(contratoReajuste: ContratoReajuste): string {
    return contratoReajuste.obraNome;
  }

  getRazaoCliente(contratoReajuste: ContratoReajuste): string {
    return contratoReajuste.contrato.interveniente.razao.toUpperCase().substring(0, 25);
  }

  percentualFormatter = (item: number) => {
    let self = ContratoReajusteAprovacaoPageComponent.self;
    return self.formataValor(item, 1, false) + "%";
  }

  booleanFormatter(item: any): boolean {
    return item === 'S';
  }

  SimNaoFormatter(item: string): string {
    return item.toLowerCase() === 's' ? 'Sim' : 'Não';
  }

  _allColumns: TableColumn[] = [
    { name: 'dataVigencia', placeholder: 'Início Val.', align: 'center', order: 1, priority: 1, formatter: this.formataData },
    { name: 'contrato', placeholder: 'Contrato', align: 'left', order: 2, priority: 2, getValue: this.getContratoNumero, getIcon: this.getStatusIcon },
    { name: 'intervenienteRazao', placeholder: 'Cliente', align: 'left', order: 3, priority: 3, getValue: this.getRazaoCliente },
    { name: 'nomeObra', placeholder: 'Nome da obra', align:'left', order: 4, priority: 4, getValue: this.getNomeObra },
    { name: 'usina', placeholder: 'Central Entrega', align: 'center', order: 5, priority: 5, getValue: this.getUsina },
    { name: 'dataCarta', placeholder: 'Data carta', align: 'center', order: 6, priority: 6, formatter: this.dataCartaFormatter }
  ];

  iconeStatusAprovado: string = 'check_circle';
  iconeStatusReprovado: string = 'cancel';
  iconeStatusPendente: string = 'help_outline';

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: ContratoReajuste | null;

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
    var self = ContratoReajusteAprovacaoPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => {
      var fixedColsTotalWidth = 180;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 235);
      var hiddenColumnsHighPriority = this.allColumns.filter(c => this.hiddenColumns.includes(c.name) && this.getPriority(c) < this.getPriority(t)).length;

      return (this.getPriority(t) - hiddenColumnsHighPriority) <= colsAllowed;
    });
  }

  get usinasCodigos(): number[] {
    let codigos: number[] = [];
    this.usinas.forEach(t => {
      codigos.push(t.codigo);
    });
    
    return codigos;
  }

  get statusReajuste(): number[] {
      let codigos: number[] = [];
      statusReajusteContrato.forEach(status => {
        codigos.push(status.codigo);
      })
      return codigos;
    };

  get columnNames(): string[] {
    return this.fixedColumnsLeft.concat(this.displayedColumns.map(t => t.name)).concat(this.fixedColumnsRight);
  }

  get foldedColumns(): TableColumn[] {
    var self = ContratoReajusteAprovacaoPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => !this.columnNames.includes(t.name));
  }

  get foldedColumnsForCreate(): TableColumn[] {
    var self = ContratoReajusteAprovacaoPageComponent.self;
    
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
    var self = ContratoReajusteAprovacaoPageComponent.self;

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

  get dataSource(): ContratoReajuste[] {
    return this.itens.records;
  };

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('dsModalVCR', { read: ViewContainerRef, static: false }) reajusteAprovacaoModalVCR: ViewContainerRef;  
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('reajusteLogModalVCR', { read: ViewContainerRef, static: false }) reajusteLogModalRef: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _contratoReajusteService: ContratoReajusteService,
    private _intervenienteService: IntervenienteService,
    private _usinaService: UsinaService,
    private _vendedorService: VendedorService
  ) {
    ContratoReajusteAprovacaoPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6008','', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Cadastro", 6008);

    _usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );

    this._vendedorService.listarPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores; },
      error => { this.vendedoresPermitidos = []; }
    );

  }
  
  ngOnInit() {
    this.reajusteAprovacaoForm = this._formBuilder.group({});
  }

  ngAfterViewInit(): void {
    this.carregaVigenciasTraco();

    this._cdr.detectChanges();
    this.filter.aplyFilter();
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  get temDireitoAprovacao(): boolean {
    return this._userService.temDireitoAplicativo("WEB6009", "");
  }

  get temDireitoReprovacao(): boolean {
    return this._userService.temDireitoAplicativo("WEB6010", "");
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

    if (!this.filtroString) this.filtroString = `filter=$(status==${EStatusReajusteContrato.Pendente})`;

    if (this.filtro.tipoReajuste == ETipoReajuste.Concreto){
      this._contratoReajusteService.ListaTodosReajustesTraco(currentPage, pageSize, this.filtroString)
      .then(
        contratoReajusteTracos => {
          this.itens = contratoReajusteTracos;
          this.paginaAtual = contratoReajusteTracos.currentPage;
          this.registrosPorPagina = contratoReajusteTracos.pageSize;
        },
        error => { this.itens = new PagedList<ContratoReajuste>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
    } else if (this.filtro.tipoReajuste == ETipoReajuste.Bomba) {
      this._contratoReajusteService.ListaTodosReajustesBomba(currentPage, pageSize, this.filtroString)
      .then(
        contratoReajusteBomba => {
          this.itens = contratoReajusteBomba;
          this.paginaAtual = contratoReajusteBomba.currentPage;
          this.registrosPorPagina = contratoReajusteBomba.pageSize;
        },
        error => { this.itens = new PagedList<ContratoReajuste>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
    }
  }

  getFormattedValue(element: ContratoReajuste, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name]);
  }

  getStatusIcon(element: ContratoReajuste, column: TableColumn) {
    let self = ContratoReajusteAprovacaoPageComponent.self;

    if(element.dataVigencia == undefined || column.name !== 'contrato')
      return '';
    
    if(element.dataConfirmacao !== null)
      return this.iconeStatusAprovado;
    else if(element.idReprovacao !== '')
      return this.iconeStatusReprovado;
    else
      return this.iconeStatusPendente;
  }

  getTooltipIcon(element: ContratoReajuste, column: TableColumn){
    let self = ContratoReajusteAprovacaoPageComponent.self;

    if(element.dataVigencia == undefined || column.name !== 'contrato')
      return '';

    if(element.dataConfirmacao !== null)
      return 'Aprovado';
    else if(element.idReprovacao !== '')
      return 'Reprovado';
    else
      return 'Pendente';
  }

  getClassIcon(element: ContratoReajuste, column: TableColumn) {
    let self = ContratoReajusteAprovacaoPageComponent.self;

    if(element.dataVigencia == undefined || column.name !== 'contrato')
      return '';

    if(element.dataConfirmacao !== null)
      return 'reajuste-aprovado';
    else if(element.idReprovacao !== '')
      return 'reajuste-reprovado';
    else
      return 'reajuste-pendente';
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

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
  }

  carregaVigenciasTraco() {
    this._contratoReajusteService.ObterVigenciasTraco().then(
      vigencias => { this.vigencias = vigencias },
      error => { this.vigencias = null }
    );
  }

  carregaVigenciasBomba() {
    this._contratoReajusteService.ObterVigenciasBomba().then(
      vigencias => { this.vigencias = vigencias },
      error => { this.vigencias = null }
    );
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
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, contratoReajuste?: ContratoReajuste) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;
    this.item = contratoReajuste;
    
    if (container == this.reajusteLogModalRef) {
      var tipo = this.filtro.tipoReajuste == ETipoReajuste.Concreto ? "traco" : "bomba";

      this._contratoReajusteService.ListarReajusteLogs(contratoReajuste.usinaCodigo, contratoReajuste.contratoAno, contratoReajuste.contratoNumero, contratoReajuste.dataVigencia, tipo)
        .then(
          logs => {
            this.logs = logs;
          },
          error => { this.logs = []; }
        )
        .then(() => {
          this._cdr.detectChanges();
        });
    }

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  aprovarReajuste(content, contratoReajuste?: ContratoReajuste) {
    let self = ContratoReajusteAprovacaoPageComponent.self;
    contratoReajuste = contratoReajuste ? contratoReajuste : this.item;
    if (this.filtro.tipoReajuste === ETipoReajuste.Concreto) {
      self._contratoReajusteService.AprovarReajusteTraco(this.item)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Aprovação realizada com sucesso!')
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    } else if (this.filtro.tipoReajuste === ETipoReajuste.Bomba) {
      self._contratoReajusteService.AprovarReajusteBomba(this.item)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Aprovação realizada com sucesso!')
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    }
      
  }

  aprovarTodos(){
    let self = ContratoReajusteAprovacaoPageComponent.self;
    if (this.filtro.tipoReajuste === ETipoReajuste.Concreto) {
      self._contratoReajusteService.AprovarTodosTraco(this.itens.records)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Aprovações realizadas com sucesso!');
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    } else if (this.filtro.tipoReajuste === ETipoReajuste.Bomba) {
      self._contratoReajusteService.AprovarTodosBomba(this.itens.records)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Aprovações realizadas com sucesso!');
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    }
  }

  reprovarReajuste(content, contratoReajuste?: ContratoReajuste) {
    let self = ContratoReajusteAprovacaoPageComponent.self;
    contratoReajuste = contratoReajuste ? contratoReajuste : this.item;
    if (this.filtro.tipoReajuste === ETipoReajuste.Concreto) {
      self._contratoReajusteService.ReprovarReajusteTraco(this.item)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Reprovação realizada com sucesso!')
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    } else if (this.filtro.tipoReajuste === ETipoReajuste.Bomba) {
      self._contratoReajusteService.ReprovarReajusteBomba(this.item)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Reprovação realizada com sucesso!')
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    }
      
  }

  reprovarTodos(){
    let self = ContratoReajusteAprovacaoPageComponent.self;
    if (this.filtro.tipoReajuste === ETipoReajuste.Concreto) {
      self._contratoReajusteService.ReprovarTodosTraco(this.itens.records)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Reprovações realizadas com sucesso!');
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    } else if (this.filtro.tipoReajuste === ETipoReajuste.Bomba) {
      self._contratoReajusteService.ReprovarTodosBomba(this.itens.records)
      .then(() => {
        self.closeModal();
        self.getPage();
        this.exibirAlerta('Reprovações realizadas com sucesso!');
      }, err => {
        this.exibirAlerta(`${this.formataErrosApi(err)}`);
      });
    }
  }

  exibirAlerta(mensagem: string, titulo: string = 'TopConWeb') {
    let self = ContratoReajusteAprovacaoPageComponent.self;
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
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.reajusteAprovacaoModalVCR });
    this.modalIsOpen = true;
  }

  closeModal() {
    let self = ContratoReajusteAprovacaoPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.reajusteAprovacaoForm.markAsPristine();
    self.reajusteAprovacaoForm.markAsUntouched();

    self.modalIsOpen = false;
  }
}
