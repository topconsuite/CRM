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
  import { VisitaTipoService } from 'app/services/visita-tipo.service';
  
  import * as _ from 'lodash';
import { VisitaTipo } from 'app/classes/visita/visita-tipo';
  
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
    selector: 'appvisita-tipo-page',
    templateUrl: './visita-tipo-page.component.html',
    styleUrls: ['./visita-tipo-page.component.scss'],
    animations: [
      trigger('detailExpand', [
        state('collapsed', style({height: '0px', minHeight: '0'})),
        state('expanded', style({height: '*'})),
        transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      ]),
    ]
  })
  export class VisitaTipoPageComponent implements OnInit, AfterViewInit {
    public static self: VisitaTipoPageComponent;
  
    tipoVisitaForm: FormGroup;
  
    itens: PagedList<VisitaTipo> = new PagedList<VisitaTipo>();
    item: VisitaTipo;
  
    tiposVisita: VisitaTipo[] = [];
  
    paginaAtual: number = 1;
    registrosPorPagina: number = 30;
  
    filtroString: string = '';
  
    filtro: {
      tipoVisita: VisitaTipo,
      codigo: number,
      descricao: string
    } = {
      tipoVisita: null,
      codigo: 0,
      descricao: ''
    };
  
    formataData = Tasks.formataData;
    formataHora = Tasks.formataHora;
    formataValor = Tasks.formataValor;
    formataErrosApi = Tasks.formataErrosApi;
  
    modalIsOpen: boolean = false;
    private _dialogRef: MatDialogRef<any>;

    tipoVisitaFormatter = (model: VisitaTipo) => model ? model.descricao.toUpperCase() : '';

    SimNaoFormatter(item: boolean): string {
      return item ? 'Sim' : 'Não';
    }
    
    _allColumns: TableColumn[] = [
      { name: 'codigo', placeholder: 'Código', order: 1, priority: 1 },
      { name: 'descricao', placeholder: 'Descrição', order: 2, priority: 2},
      { name: 'ativo', placeholder: 'Condição Ativa', order: 3, priority: 3, formatter: this.SimNaoFormatter},
    ];
  
    get allColumns(): TableColumn[] {
        return this._allColumns;
    }
  
    expandedElement: VisitaTipo | null;
  
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
      return ['edit', 'delete'];
    }
    get fixedColumns(): string[] {
      return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
    }
  
    get displayedColumns(): TableColumn[] {
      var self = VisitaTipoPageComponent.self;
      
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
      var self = VisitaTipoPageComponent.self;
      
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
      var self = VisitaTipoPageComponent.self;
  
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
  
  
    get dataSource(): VisitaTipo[] {
      return this.itens.records;
    };
  
    @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  
    @ViewChild('tvModalVCR', { read: ViewContainerRef, static: false }) tipoVisitaModalVCR: ViewContainerRef;
    @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  
    constructor(
      private _dialog: MatDialog,
      private _cdr: ChangeDetectorRef,
      private _formBuilder: FormBuilder,
      private _userService: UserService,
      private _tipoVisitaService: VisitaTipoService
    ) {
      VisitaTipoPageComponent.self = this;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7003','', 50);
      if (!temDireito) return;
  
      this._userService.gravarAcessoAplicacao("Cadastro TipoVisita", 7003);
    }
  
    ngOnInit() {
      this.tipoVisitaForm = this._formBuilder.group({
        ativo: ['']
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
      
      this._tipoVisitaService.Listar(currentPage, pageSize, this.filtroString)
      .then(
        TipoVisita => {
          this.itens = TipoVisita;
          this.paginaAtual = TipoVisita.currentPage;
          this.registrosPorPagina = TipoVisita.pageSize;
        },
       error => { this.itens = new PagedList<VisitaTipo>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
    }
  
    getFormattedValue(element: VisitaTipo, column: TableColumn) {
      return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
    }
  
    filtroChange(novoFiltro: string){
      this.filtroString = novoFiltro;
      this.getPage();
    }
  
    private _timeoutTipoVisitaPorDescricao = null;
    filtrarTipoVisitaPorDescricao(tipoVisita: string) {
      this.filtro.descricao = tipoVisita;
  
      var tamanhoMinimo = (isNaN(parseInt(tipoVisita)) ? 2 : 0);
  
      if (tipoVisita && tipoVisita.length>tamanhoMinimo && (!this.filtro.tipoVisita || this.filtro.tipoVisita.descricao!=tipoVisita)) {
        
        if (this._timeoutTipoVisitaPorDescricao) clearTimeout(this._timeoutTipoVisitaPorDescricao);
        
        var filtro = 'filter=$(' + (isNaN(parseInt(tipoVisita)) ? 'descricao==' + tipoVisita : 'codigo==' + parseInt(tipoVisita)) + ')';
  
        this._timeoutTipoVisitaPorDescricao = setTimeout( () => {
          this._tipoVisitaService.Listar(null, null, filtro)
            .then(
              tiposVisita => { this.tiposVisita = tiposVisita.records; },
              error => { this.tiposVisita = []; }
            )
        }, 500);
  
      } else {
        this.tiposVisita = [];
      }
    }
  
    showSelecaoColunasModal(content) {
      this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
      this.modalIsOpen = true;
    }
  
    confirmModal: Function;
    cancelModal: Function;
    showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, tipoVisita?: VisitaTipo) {
      this.confirmModal = confirmCallback;
      this.cancelModal = cancelCallback || this.closeModal;
  
      if (tipoVisita) {
        var temDireito = this._userService.temDireitoAplicativo('WEB7003','A');
        if (!temDireito) {
          this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Você não tem permissão de alteração!`
            }
          });
          return;
        }
  
        this.item = JSON.parse(JSON.stringify(tipoVisita));
      } else {
        var temDireito = this._userService.temDireitoAplicativo('WEB7003','I');
        if (!temDireito) {
          this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Você não tem permissão para inserir um novo Tipo de Visita!`
            }
          });
          return;
        }
  
        this.item = new VisitaTipo();
      }
  
      this._dialogRef = this._dialog.open(content, { viewContainerRef: container });
  
      this.modalIsOpen = true;
  
    }
  
    closeModal() {
      let self = VisitaTipoPageComponent.self;
      
      if (self._dialogRef) self._dialogRef.close();
  
      self.tipoVisitaForm.markAsPristine();
      self.tipoVisitaForm.markAsUntouched();
  
      self.modalIsOpen = false;
    }

    checkBoxChange(evento: any, propriedade: string) {
      if(propriedade == "ativo")
          this.item.ativo = evento.checked ? true : false;
    }
  
    deleteTipoVisita(tipoVisita: VisitaTipo): void {
      let self = VisitaTipoPageComponent.self;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7003','E');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de exclusão!`
          }
          
      })
      return;}
    
      self._tipoVisitaService.Deletar(tipoVisita.codigo)
        .then(success => {
          self.getPage();
        }, err => {
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `${self.formataErrosApi(err)}`
            }
          });
        });
    }
    
    updateTipoVisita(tipoVisita: VisitaTipo): void {
      let self = VisitaTipoPageComponent.self;
  
       self._tipoVisitaService.Atualizar(tipoVisita)
       .then(success => {
         self.closeModal();
         self.getPage();
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `Tipo de Visita alterado com sucesso!`
           }
         });
       }, err => {
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `Erro ao alterar o Tipo de Visita.\n${self.formataErrosApi(err)}`
           }
         });
       });
    }
  
    addTipoVisita(tipoVisita: VisitaTipo): void {
      let self = VisitaTipoPageComponent.self;
  
       self._tipoVisitaService.Adicionar(tipoVisita)
       .then(success => {
         self.closeModal();
         self.getPage();
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `Tipo de Visita adicionado com sucesso!`
           }
         });
       }, err => {
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `${self.formataErrosApi(err)}`
           }
         });
       });
    }
  }
  
  