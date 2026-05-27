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
  import { OportunidadeTipoService } from 'app/services/oportunidade-tipo.service';
  import { OportunidadeTipo } from 'app/classes/oportunidade/oportunidade-tipo';
  
  import * as _ from 'lodash';
  
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
    selector: 'app-oportunidade-tipo-page',
    templateUrl: './oportunidade-tipo-page.component.html',
    styleUrls: ['./oportunidade-tipo-page.component.scss'],
    animations: [
      trigger('detailExpand', [
        state('collapsed', style({height: '0px', minHeight: '0'})),
        state('expanded', style({height: '*'})),
        transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      ]),
    ]
  })
  export class OportunidadeTipoPageComponent implements OnInit, AfterViewInit {
    public static self: OportunidadeTipoPageComponent;
  
    oportunidadeTipoForm: FormGroup;
  
    itens: PagedList<OportunidadeTipo> = new PagedList<OportunidadeTipo>();
    item: OportunidadeTipo;
  
    oportunidadeTipos: OportunidadeTipo[] = [];
  
    paginaAtual: number = 1;
    registrosPorPagina: number = 30;
  
    filtroString: string = '';
  
    filtro: {
      oportunidadeTipo: OportunidadeTipo,
      codigo: number,
      descricao: string
    } = {
      oportunidadeTipo: null,
      codigo: 0,
      descricao: ''
    };
  
    formataData = Tasks.formataData;
    formataHora = Tasks.formataHora;
    formataValor = Tasks.formataValor;
    formataErrosApi = Tasks.formataErrosApi;
  
    modalIsOpen: boolean = false;
    private _dialogRef: MatDialogRef<any>;

    oportunidadeTipoFormatter = (model: OportunidadeTipo) => model ? model.descricao.toUpperCase() : '';

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
  
    expandedElement: OportunidadeTipo | null;
  
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
      var self = OportunidadeTipoPageComponent.self;
      
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
      var self = OportunidadeTipoPageComponent.self;
      
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
      var self = OportunidadeTipoPageComponent.self;
  
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
  
  
    get dataSource(): OportunidadeTipo[] {
      return this.itens.records;
    };
  
    @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  
    @ViewChild('otModalVCR', { read: ViewContainerRef, static: false }) oportunidadeTipoModalVCR: ViewContainerRef;
    @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  
    constructor(
      private _dialog: MatDialog,
      private _cdr: ChangeDetectorRef,
      private _formBuilder: FormBuilder,
      private _userService: UserService,
      private _oportunidadeTipoService: OportunidadeTipoService
    ) {
      OportunidadeTipoPageComponent.self = this;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7005','', 50);
      if (!temDireito) return;
  
      this._userService.gravarAcessoAplicacao("Cadastro OportunidadeTipo", 7005);
    }
  
    ngOnInit() {
      this.oportunidadeTipoForm = this._formBuilder.group({
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
      
      this._oportunidadeTipoService.Listar(currentPage, pageSize, this.filtroString)
      .then(
        oportunidadeTipo => {
          this.itens = oportunidadeTipo;
          this.paginaAtual = oportunidadeTipo.currentPage;
          this.registrosPorPagina = oportunidadeTipo.pageSize;
        },
       error => { this.itens = new PagedList<OportunidadeTipo>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
    }
  
    getFormattedValue(element: OportunidadeTipo, column: TableColumn) {
      return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
    }
  
    filtroChange(novoFiltro: string){
      this.filtroString = novoFiltro;
      this.getPage();
    }
  
    private _timeoutOportunidadeTipoPorDescricao = null;
    filtrarOportunidadeTipoPorDescricao(oportunidadeTipo: string) {
      this.filtro.descricao = oportunidadeTipo;
  
      var tamanhoMinimo = (isNaN(parseInt(oportunidadeTipo)) ? 2 : 0);
  
      if (oportunidadeTipo && oportunidadeTipo.length>tamanhoMinimo && (!this.filtro.oportunidadeTipo || this.filtro.oportunidadeTipo.descricao!=oportunidadeTipo)) {
        
        if (this._timeoutOportunidadeTipoPorDescricao) clearTimeout(this._timeoutOportunidadeTipoPorDescricao);
        
        var filtro = 'filter=$(' + (isNaN(parseInt(oportunidadeTipo)) ? 'descricao==' + oportunidadeTipo : 'codigo==' + parseInt(oportunidadeTipo)) + ')';
  
        this._timeoutOportunidadeTipoPorDescricao = setTimeout( () => {
          this._oportunidadeTipoService.Listar(null, null, filtro)
            .then(
              oportunidadeTipos => { this.oportunidadeTipos = oportunidadeTipos.records; },
              error => { this.oportunidadeTipos = []; }
            )
        }, 500);
  
      } else {
        this.oportunidadeTipos = [];
      }
    }
  
    showSelecaoColunasModal(content) {
      this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
      this.modalIsOpen = true;
    }
  
    confirmModal: Function;
    cancelModal: Function;
    showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, oportunidadeTipo?: OportunidadeTipo) {
      this.confirmModal = confirmCallback;
      this.cancelModal = cancelCallback || this.closeModal;
  
      if (oportunidadeTipo) {
        var temDireito = this._userService.temDireitoAplicativo('WEB7005','A');
        if (!temDireito) {
          this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Você não tem permissão de alteração!`
            }
          });
          return;
        }
  
        this.item = JSON.parse(JSON.stringify(oportunidadeTipo));
      } else {
        var temDireito = this._userService.temDireitoAplicativo('WEB7005','I');
        if (!temDireito) {
          this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Você não tem permissão para inserir um novo Tipo de Oportunidade!`
            }
          });
          return;
        }
  
        this.item = new OportunidadeTipo();
      }
  
      this._dialogRef = this._dialog.open(content, { viewContainerRef: container });
  
      this.modalIsOpen = true;
  
    }
  
    closeModal() {
      let self = OportunidadeTipoPageComponent.self;
      
      if (self._dialogRef) self._dialogRef.close();
  
      self.oportunidadeTipoForm.markAsPristine();
      self.oportunidadeTipoForm.markAsUntouched();
  
      self.modalIsOpen = false;
    }

    checkBoxChange(evento: any, propriedade: string) {
      if(propriedade == "ativo")
          this.item.ativo = evento.checked ? true : false;
    }
  
    deleteOportunidadeTipo(oportunidadeTipo: OportunidadeTipo): void {
      let self = OportunidadeTipoPageComponent.self;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7005','E');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de exclusão!`
          }
          
      })
      return;}
    
      self._oportunidadeTipoService.Deletar(oportunidadeTipo.codigo)
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
    
    updateOportunidadeTipo(oportunidadeTipo: OportunidadeTipo): void {
      let self = OportunidadeTipoPageComponent.self;
  
       self._oportunidadeTipoService.Atualizar(oportunidadeTipo)
       .then(success => {
         self.closeModal();
         self.getPage();
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `Tipo de Oportunidade alterado com sucesso!`
           }
         });
       }, err => {
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `Erro ao alterar o Tipo de Oportunidade.\n${self.formataErrosApi(err)}`
           }
         });
       });
    }
  
    addOportunidadeTipo(oportunidadeTipo: OportunidadeTipo): void {
      let self = OportunidadeTipoPageComponent.self;
  
       self._oportunidadeTipoService.Adicionar(oportunidadeTipo)
       .then(success => {
         self.closeModal();
         self.getPage();
         self._dialog.open(AlertDialogComponent, {
           disableClose: true,
           data: {
             title: 'TopConWeb',
             message: `Tipo de Oportunidade adicionado com sucesso!`
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
  
  