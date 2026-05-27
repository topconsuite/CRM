import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef, MatDialog} from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';

import { UserService } from 'app/services/user.service';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { GrupoEconomico } from 'app/classes/grupo-economico/grupo-economico';
import { Interveniente } from 'app/classes/interveniente/interveniente';
import { IntervenienteService } from 'app/services/interveniente.service';
import { GrupoEconomicoService } from 'app/services/grupo-economico.service';

import * as _ from 'lodash';
import { CadastroGeral } from 'app/classes/cadastro-geral/cadastro-geral';
import { CadastroGeralService } from 'app/services/cadastro-geral.service';
import { ParametroService } from 'app/services/parametro.service';

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
  selector: 'app-grupo-economico-page',
  templateUrl: './grupo-economico-page.component.html',
  styleUrls: ['./grupo-economico-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class GrupoEconomicoPageComponent implements OnInit, AfterViewInit {
  public static self: GrupoEconomicoPageComponent;

  grupoEconomicoForm: FormGroup;
  grupoEconomicoCadClienteForm: FormGroup;
  grupoEconomicoCadLimiteCreditoForm: FormGroup;

  itens: PagedList<GrupoEconomico> = new PagedList<GrupoEconomico>();
  item: GrupoEconomico;

  gruposEconomicos: GrupoEconomico[] = [];

  limiteCreditoPorGrupoEconomico: boolean = false;

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtroPesquisaClientes: Interveniente[] = [];

  filtro: {
    grupoEconomico: GrupoEconomico,
    codigo: number,
    descricao: string,
    interveniente: Interveniente,
    intervenienteRazao: string
  } = {
    grupoEconomico: null,
    codigo: 0,
    descricao: '',
    interveniente: null,
    intervenienteRazao: ''
  };

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataErrosApi = Tasks.formataErrosApi;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  intervenientes: Interveniente[] = [];
  motivosBloqueioGrupo: CadastroGeral[] = [];

  intervenienteFormatter = (model: Interveniente) => model ? model.codigo+' - '+model.razao.toUpperCase() : '';
  grupoEconomicoFormatter = (model: GrupoEconomico) => model ? model.descricao.toUpperCase() : '';
  
  motivoBloqueioFormatter = (model: CadastroGeral): string => {
    return model ? model.descricao.toUpperCase() : '';
  }

  _allColumns: TableColumn[] = [
    { name: 'codigo', placeholder: 'Código', order: 1, priority: 1 },
    { name: 'descricao', placeholder: 'Descrição', order: 2, priority: 2},
    { name: 'quantidadeClientes', placeholder: 'Quantidade de Clientes', order: 3, priority: 3, getValue: this.getQuantidadeClientes}
  ];

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: GrupoEconomico | null;

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
    return ['edit', 'config', 'delete'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = GrupoEconomicoPageComponent.self;
    
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
    var self = GrupoEconomicoPageComponent.self;
    
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
    var self = GrupoEconomicoPageComponent.self;

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


  get dataSource(): GrupoEconomico[] {
    return this.itens.records;
  };

  exibirAcompanhamento: boolean = false;


  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('dsModalVCR', { read: ViewContainerRef, static: false }) grupoEconomicoModalVCR: ViewContainerRef;
  @ViewChild('configClienteModalVCR', { read: ViewContainerRef, static: false }) configClienteModalRef: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _intervenienteService: IntervenienteService,
    private _grupoEconomicoService: GrupoEconomicoService,
    private _cadastroGeralService: CadastroGeralService,
    private _parametroService: ParametroService
  ) {
    GrupoEconomicoPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6007','', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Cadastro", 6007);

    this._cadastroGeralService.listarmotivosBloqueioInterveniente().then(
      motivos => {this.motivosBloqueioGrupo = motivos;},
      error => {this.motivosBloqueioGrupo = [];}
    )

    this._parametroService.obterParametoN('web', 'LimiteCreditoPorGrupoEconomico').then(
      response => {
        this.limiteCreditoPorGrupoEconomico = response === "1"
      }
    )
  }

  ngOnInit() {
    this.grupoEconomicoForm = this._formBuilder.group({});
    this.grupoEconomicoCadClienteForm = this._formBuilder.group({});
    this.grupoEconomicoCadLimiteCreditoForm = this._formBuilder.group({});
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
    
    this._grupoEconomicoService.Listar(currentPage, pageSize, this.filtroString)
    .then(
      GrupoEconomico => {
        this.itens = GrupoEconomico;
        this.paginaAtual = GrupoEconomico.currentPage;
        this.registrosPorPagina = GrupoEconomico.pageSize;
      },
     error => { this.itens = new PagedList<GrupoEconomico>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  getFormattedValue(element: GrupoEconomico, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  getQuantidadeClientes(item: GrupoEconomico): string {
    if (item.clientes === null) return '0';
    return `${item.clientes.length}`;
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
    this.filtro.descricao = grupoEconomico;

    var tamanhoMinimo = (isNaN(parseInt(grupoEconomico)) ? 2 : 0);

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

  showSelecaoColunasModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, grupoEconomico?: GrupoEconomico) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if (grupoEconomico) {
      var temDireito = this._userService.temDireitoAplicativo('WEB6007','A');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de alteração!`
          }
        });
        return;
      }

      this.item = JSON.parse(JSON.stringify(grupoEconomico));
    } else {
      var temDireito = this._userService.temDireitoAplicativo('WEB6007','I');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão para inserir um novo Grupo Econômico!`
          }
        });
        return;
      }

      this.item = new GrupoEconomico();
    }

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;

  }

  closeModal() {
    let self = GrupoEconomicoPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.grupoEconomicoForm.markAsPristine();
    self.grupoEconomicoForm.markAsUntouched();

    self.grupoEconomicoCadClienteForm.markAsPristine();
    self.grupoEconomicoCadClienteForm.markAsUntouched();

    self.grupoEconomicoCadLimiteCreditoForm.markAsPristine();
    self.grupoEconomicoCadLimiteCreditoForm.markAsUntouched();

    self.modalIsOpen = false;
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

  confirmarAlteracaoLimiteCredito(grupoEconomico: GrupoEconomico){

    if(new Date(grupoEconomico.limiteData) < Tasks.dataAtual() && grupoEconomico.limiteData !== null){
      this.alert('Alteração não Permitida! Data do Limite Credito Vencida');
      return;
    }

    this._grupoEconomicoService.Atualizar(grupoEconomico)
     .then(success => {
      this.closeModal();
      this.getPage();
      this._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Grupo Econômico alterado com sucesso!`
         }
       });
     }, err => {
      this._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Erro alterar o Grupo Econômico.\n${JSON.stringify(err.exceptionMessage)}`
         }
       });
     });
  }

  deleteGrupoEconomico(grupoEconomico: GrupoEconomico): void {
    let self = GrupoEconomicoPageComponent.self;

    var temDireito = this._userService.temDireitoAplicativo('WEB6007','E');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de exclusão!`
        }
        
    })
    return;}
  
    self._grupoEconomicoService.Deletar(grupoEconomico.codigo)
      .then(success => {
        self.getPage();
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
  
  updateGrupoEconomico(grupoEconomico: GrupoEconomico): void {
    let self = GrupoEconomicoPageComponent.self;

     self._grupoEconomicoService.Atualizar(grupoEconomico)
     .then(success => {
       self.closeModal();
       self.getPage();
       self._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Grupo Econômico alterado com sucesso!`
         }
       });
     }, err => {
       self._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Erro alterar o Grupo Econômico.\n${JSON.stringify(err.exceptionMessage)}`
         }
       });
     });
  }

  addGrupoEconomico(grupoEconomico: GrupoEconomico): void {
    let self = GrupoEconomicoPageComponent.self;

     self._grupoEconomicoService.Adicionar(grupoEconomico)
     .then(success => {
       self.closeModal();
       self.getPage();
       self._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Grupo Econômico adicionado com sucesso!`
         }
       });
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

  limiteCreditoColor(grupoEconomico: GrupoEconomico): string {
    if(this.possuiLimiteDeCreditoVencido(grupoEconomico)) 
      return `#FF8072`;
    else if(this.possuiLimiteDeCreditoVazio(grupoEconomico))
      return `#ffc800`;
    else
      return ``;
  }

  limiteCreditoIcon(grupoEconomico: GrupoEconomico): string {
    if(this.possuiLimiteDeCreditoVencido(grupoEconomico)) 
      return `cancel`;
    else if(this.possuiLimiteDeCreditoVazio(grupoEconomico))
      return `warning`;
    else
      return ``;
  }

  possuiLimiteDeCreditoVencido(grupoEconomico: GrupoEconomico): boolean {
    return (new Date(grupoEconomico.limiteData) < Tasks.dataAtual() && grupoEconomico.limiteData !== null)
     || (!grupoEconomico.limiteValor && grupoEconomico.limiteData !== null);
  }
  
  possuiLimiteDeCreditoVazio(grupoEconomico: GrupoEconomico): boolean {
    return (grupoEconomico.limiteValor === 0) || (!grupoEconomico.limiteValor && grupoEconomico.limiteData === null);
  }

  // -------------- Modal - Cadastro Usuarios -------------------------------------

  configClienteSelecionado: Interveniente = new Interveniente();
  configGrupoEconomico: GrupoEconomico;
    
  showConfiguracaoModal(content, container: ViewContainerRef, grupoEconomico: GrupoEconomico, cancelCallback?: Function) {
    this.cancelModal = cancelCallback || this.closeModal;

    var temDireito = this._userService.temDireitoAplicativo('WEB6007','A');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de alteração!`
        }
      });
      return;
    }

    this.configGrupoEconomico = grupoEconomico;

    this.configClienteSelecionado = null;

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  filtrarListaClientes(cliente: string) {
    let self = GrupoEconomicoPageComponent.self;

    self.filtroPesquisaClientes = [...self.configGrupoEconomico.clientes];

    self.filtroPesquisaClientes = self.filtroPesquisaClientes.filter(c =>
      c.nome.toLowerCase().includes(cliente.toLowerCase()));
    
    self._cdr.detectChanges();
  }
  
  adicionarCliente() {
    let self = GrupoEconomicoPageComponent.self;

    if(self.configClienteSelecionado == null) {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `${self.formataErrosApi("Favor selecionar um cliente.")}`
        }
      });
    }

    const clienteExistente = self.configGrupoEconomico.clientes.find(
      cliente => cliente.codigo === self.configClienteSelecionado.codigo
    );
  
    if (clienteExistente) {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `O cliente já está adicionado neste Grupo Econômico.`
        }
      });
      return;
    }

    const GrupoEconomicoSubst = _.cloneDeep(self.configGrupoEconomico);
    self.configClienteSelecionado.grupoEconomicoCodigo = GrupoEconomicoSubst.codigo;
    GrupoEconomicoSubst.clientes.unshift(self.configClienteSelecionado);

    self._grupoEconomicoService.Atualizar(GrupoEconomicoSubst)
      .then(success => {        
        self.configGrupoEconomico.clientes.unshift(self.configClienteSelecionado);
        self.filtroPesquisaClientes = self.configGrupoEconomico.clientes;
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Adicionado com sucesso!`
          }
        });
      }, err => {
        self.filtroPesquisaClientes = self.configGrupoEconomico.clientes;
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
      self._cdr.detectChanges();
  }

  removerCliente(cliente: Interveniente) {
    let self = GrupoEconomicoPageComponent.self;

    const GrupoEconomicoSubst = _.cloneDeep(self.configGrupoEconomico);
    GrupoEconomicoSubst.clientes = self.configGrupoEconomico.clientes.filter(c => c !== cliente);

    self._grupoEconomicoService.Atualizar(GrupoEconomicoSubst)
      .then(success => {
        self.configGrupoEconomico.clientes = self.configGrupoEconomico.clientes.filter(c => c !== cliente);
        self.filtroPesquisaClientes = self.configGrupoEconomico.clientes;
        self._cdr.detectChanges();
      }, err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Erro ao remover um cliente.\n${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
  }
}

