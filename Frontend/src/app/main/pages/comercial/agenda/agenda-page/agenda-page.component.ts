import { animate, state, style, transition, trigger } from '@angular/animations';
import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { Compromisso } from 'app/classes/agenda/compromisso';
import { Tarefa } from 'app/classes/agenda/tarefa';
import { PagedList } from 'app/classes/pagination/paged-list';
import { UsuarioWebFiltro } from 'app/classes/usuario/usuario-web-filtro';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ICustomValidator } from 'app/main/components/interfaces/custom-validator';
import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { CompromissoService } from 'app/services/compromisso.service';
import { TarefaService } from 'app/services/tarefa.service';
import { UserService } from 'app/services/user.service';
import { Dictionary, forEach } from 'lodash';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

@Component({
  selector: 'app-agenda-page',
  templateUrl: './agenda-page.component.html',
  styleUrls: ['./agenda-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class AgendaPageComponent implements OnInit, AfterViewInit {
  public static self: AgendaPageComponent;

  tarefaForm: FormGroup;
  
  itensTarefas: PagedList<Tarefa> = new PagedList<Tarefa>();
  itemTarefa: Tarefa;
  tarefas: Tarefa[] = [];
  filtroTarefaString: string = '';
  filtroTarefaAtrasada: boolean = false;
  filtroHorarioDe: string = '';
  filtroHorarioAte: string = '';

  usuarios: { [key: string]: string } = {};
  gruposSelecionados: string[] = [];
  usuariosSelecionados: string[] = [];

  temDireitoGrupoAcesso = false;
  temDireitoGrupoInclui = false;
  temDireitoGrupoAlteracao = false;
  temDireitoGrupoDeletar = false;
  
  temDireitoGeralAcesso = false;
  temDireitoGeralInclui = false;
  temDireitoGeralAlteracao = false;
  temDireitoGeralDeletar = false;

  formUsuario: FormGroup;

  filtroTarefa: {
    descricao: string,
    local: string,
    anoVisita: number,
    numeroVisita: number,
    anoLead: number,
    numeroLead: number,
    anoOportunidade: number,
    numeroOportunidade: number,
    dataDe: Date,
    dataAte: Date,
    finalizado: boolean
  } = {
    descricao: '',
    local: '',
    anoVisita:  0,
    numeroVisita: 0,
    anoLead: 0,
    numeroLead: 0,
    anoOportunidade: 0,
    numeroOportunidade: 0,
    dataDe: null,
    dataAte: null,
    finalizado: false
  };

  paginaAtualTarefa: number = 1;
  registrosPorPaginaTarefa: number = 5;

  itensCompromissos: PagedList<Compromisso> = new PagedList<Compromisso>();
  itemCompromisso: Compromisso;
  compromissos: Compromisso[] = [];
  filtroCompromissoString: string = '';
  filtroCompromissoHoraInicioDe: string = '';
  filtroCompromissoHoraInicioAte: string = '';
  filtroCompromissoHoraFinalDe: string = '';
  filtroCompromissoHoraFinalAte: string = '';

  filtroCompromisso: {
    descricao: string,
    local: string,
    anoVisita: number,
    numeroVisita: number,
    anoLead: number,
    numeroLead: number,
    anoOportunidade: number,
    numeroOportunidade: number,
    dataDe: Date,
    dataAte: Date,
  } = {
    descricao: '',
    local: '',
    anoVisita:  0,
    numeroVisita: 0,
    anoLead: 0,
    numeroLead: 0,
    anoOportunidade: 0,
    numeroOportunidade: 0,
    dataDe: null,
    dataAte: null,
  };

  paginaAtualCompromisso: number = 1;
  registrosPorPaginaCompromisso: number = 5;

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataErrosApi = Tasks.formataErrosApi;
  maskHora = [/\d/, /\d/, ':', /\d/, /\d/];

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  redefinirFiltroGrupo() {
    this.usuariosSelecionados = [];
    this.gruposSelecionados = [];
    this.getTarefaPage();
    this.getCompromissoPage();
  }

  filtroPorGrupo() {
    this.getTarefaPage();
    this.getCompromissoPage();
  }

  //#region TAREFAS

_allColumnsTarefas: TableColumn[] = [
    { name: 'descricao', placeholder: 'Descrição', order: 1, priority: 1},
    { name: 'usuario', placeholder: 'Usuário', order: 2, priority: 2, getValue: this.getTarefaUsuario },
    { name: 'data', placeholder: 'Data', order: 3, priority: 3, getValue: this.getTarefaData},
    { name: 'origem', placeholder: 'Origem', order: 4, priority: 4, getValue: this.getTarefaOrigem},
    { name: 'contato', placeholder: 'Contato', order: 5, priority: 5},
    { name: 'telefone', placeholder: 'Telefone', order: 6, priority: 6, getValue: this.getTarefaTelefone},
    { name: 'celular', placeholder: 'Celular', order: 7, priority: 7, getValue: this.getTarefaCelular},
    { name: 'email', placeholder: 'Email', order: 8, priority: 8},
    { name: 'observacao', placeholder: 'Observação', order: 9, priority: 9 },
    { name: 'providencia', placeholder: 'Providência', order: 10, priority: 10 },
    { name: 'conclusao', placeholder: 'Conclusão', order: 11, priority: 11 }
  ];

  get allColumnsTarefas(): TableColumn[] {
      return this._allColumnsTarefas;
  }

  tarefaExpandedElement: Tarefa | null;

  tarefaHiddenColumns: string[] = [];
  isHiddenColumnTarefa(columnName: string): boolean {
    return this.tarefaHiddenColumns.includes(columnName);
  }
  setHiddenColumnTarefa(columnName: string, hidden: boolean) {
    if (hidden)
      this.tarefaHiddenColumns.push(columnName);
    else
      this.tarefaHiddenColumns = this.tarefaHiddenColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }

  get columnsTarefa(): TableColumn[] {
    return this.allColumnsTarefas.filter(t => !this.tarefaHiddenColumns.includes(t.name));
  }

  get currentViewValueTarefa() {
    return { filter: this.filtroTarefa, hiddenColumns: this.tarefaHiddenColumns, customColumnOrder: this._customColumnOrderTarefa }
  }

  get fixedColumnsLeftTarefa(): string[] {
    return ['done'];
  }
  get fixedColumnsRightTarefa(): string[] {
    return ['edit', 'delete'];
  }
  get fixedColumnsTarefa(): string[] {
    return this.fixedColumnsLeftTarefa.concat(this.fixedColumnsRightTarefa);
  }

  get displayedColumnsTarefa(): TableColumn[] {
    var self = AgendaPageComponent.self;
    
    return this.columnsTarefa.sort((a, b) => {
      return self.getOrderTarefa(a) - self.getOrderTarefa(b);
    }).filter(t => {
      var fixedColsTotalWidth = 235;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 180);
      var hiddenColumnsHighPriority = this.allColumnsTarefas.filter(c => this.tarefaHiddenColumns.includes(c.name) && this.getPriorityTarefa(c) < this.getPriorityTarefa(t)).length;

      return (this.getPriorityTarefa(t) - hiddenColumnsHighPriority) <= colsAllowed;
    });
  }

  get columnNamesTarefa(): string[] {
    return this.fixedColumnsLeftTarefa.concat(this.displayedColumnsTarefa.map(t => t.name)).concat(this.fixedColumnsRightTarefa);
  }

  get foldedColumnsTarefa(): TableColumn[] {
    var self = AgendaPageComponent.self;
    
    return this.columnsTarefa.sort((a, b) => {
      return self.getOrderTarefa(a) - self.getOrderTarefa(b);
    }).filter(t => !this.columnNamesTarefa.includes(t.name));
  }

  private _customColumnOrderTarefa: string[] = [];
  getOrderTarefa(column: TableColumn): number {
    var customOrder = this._customColumnOrderTarefa.indexOf(column.name) + 1;
    if (customOrder) {
      return customOrder;
    } else {
      return column.order + this._customColumnOrderTarefa.length;
    }
  }
  getPriorityTarefa(column: TableColumn): number {
    var orderDiff = this.getOrderTarefa(column) - column.order;
    var priorityDiff = column.order - column.priority;
    return column.priority + orderDiff + priorityDiff;
  }

  get allColumnsOrderedTarefa(): TableColumn[] {
    var self = AgendaPageComponent.self;

    return this.allColumnsTarefas.sort((a, b) => {
      return self.getOrderTarefa(a) - self.getOrderTarefa(b);
    });
  }

  changeColumnOrderTarefa(columnName: string, increment: number): void {
    if (this._customColumnOrderTarefa.length === 0) {
      this._customColumnOrderTarefa = this.allColumnsTarefas.sort((a, b) => {
        return a.order - b.order;
      }).map(t => t.name);
    } else if (this._customColumnOrderTarefa.length < this.allColumnsTarefas.length) {
      this._customColumnOrderTarefa = this.allColumnsOrderedTarefa.map(t => t.name);
    }
    
    var index = this._customColumnOrderTarefa.indexOf(columnName);

    if (index < 0 || index >= this._customColumnOrderTarefa.length) return;
    if ((index+increment) < 0 || (index+increment) >= this._customColumnOrderTarefa.length) return;

    this._customColumnOrderTarefa.splice(index+increment, 0, this._customColumnOrderTarefa.splice(index, 1)[0]);
  }


  get dataSourceTarefas(): Tarefa[] {
    return this.itensTarefas.records;
  };

  @ViewChild('tModalVCR', { read: ViewContainerRef, static: false }) tarefaModalVCR: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalTarefaVCR', { read: ViewContainerRef, static: false }) tarefaColunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild(FilterComponent, { static: false }) tarefaFilter: FilterComponent;
  
  //#endregion

  //#region COMPROMISSOS

  _allColumnsCompromissos: TableColumn[] = [
    { name: 'descricao', placeholder: 'Descrição', order: 1, priority: 1},
    { name: 'usuario', placeholder: 'Usuário', order: 2, priority: 2, getValue: this.getCompromissoUsuario },
    { name: 'dataInicio', placeholder: 'Data Inicio', order: 3, priority: 3, getValue: this.getCompromissoDataInicio},
    { name: 'dataFim', placeholder: 'Data Fim', order: 4, priority: 4, getValue: this.getCompromissoDataFim},
    { name: 'local', placeholder: 'Local', order: 5, priority: 5},
    { name: 'origem', placeholder: 'Origem', order: 6, priority: 6, getValue: this.getCompromissoOrigem},
    { name: 'contato', placeholder: 'Contato', order: 7, priority: 7},
    { name: 'telefone', placeholder: 'Telefone', order: 8, priority: 8, getValue: this.getCompromissoTelefone},
    { name: 'celular', placeholder: 'Celular', order: 9, priority: 9, getValue: this.getCompromissoCelular},
    { name: 'email', placeholder: 'Email', order: 10, priority: 10},
    { name: 'observacao', placeholder: 'Observação', order: 11, priority: 11 },
    { name: 'providencia', placeholder: 'Providência', order: 12, priority: 12 },
    { name: 'conclusao', placeholder: 'Conclusão', order: 13, priority: 13 }
  ];

  get allColumnsCompromissos(): TableColumn[] {
      return this._allColumnsCompromissos;
  }

  compromissoExpandedElement: Compromisso | null;

  compromissoHiddenColumns: string[] = [];
  isHiddenColumnCompromisso(columnName: string): boolean {
    return this.compromissoHiddenColumns.includes(columnName);
  }
  setHiddenColumnCompromisso(columnName: string, hidden: boolean) {
    if (hidden)
      this.compromissoHiddenColumns.push(columnName);
    else
      this.compromissoHiddenColumns = this.compromissoHiddenColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }

  get columnsCompromisso(): TableColumn[] {
    return this.allColumnsCompromissos.filter(t => !this.compromissoHiddenColumns.includes(t.name));
  }

  get currentViewValueCompromisso() {
    return { filter: this.filtroCompromisso, hiddenColumns: this.compromissoHiddenColumns, customColumnOrder: this._customColumnOrderCompromisso }
  }

  get fixedColumnsLeftCompromisso(): string[] {    
    return [];
  }
  get fixedColumnsRightCompromisso(): string[] {
    return ['edit', 'delete'];
  }
  get fixedColumnsCompromisso(): string[] {
    return this.fixedColumnsLeftCompromisso.concat(this.fixedColumnsRightCompromisso);
  }

  get displayedColumnsCompromisso(): TableColumn[] {
    var self = AgendaPageComponent.self;
    
    return this.columnsCompromisso.sort((a, b) => {
      return self.getOrderCompromisso(a) - self.getOrderCompromisso(b);
    }).filter(t => {
      var fixedColsTotalWidth = 235;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 180);
      var hiddenColumnsHighPriority = this.allColumnsCompromissos.filter(c => this.compromissoHiddenColumns.includes(c.name) && this.getPriorityCompromisso(c) < this.getPriorityCompromisso(t)).length;

      return (this.getPriorityCompromisso(t) - hiddenColumnsHighPriority) <= colsAllowed;
    });
  }

  get columnNamesCompromisso(): string[] {
    return this.fixedColumnsLeftCompromisso.concat(this.displayedColumnsCompromisso.map(t => t.name)).concat(this.fixedColumnsRightCompromisso);
  }

  get foldedColumnsCompromisso(): TableColumn[] {
    var self = AgendaPageComponent.self;
    
    return this.columnsCompromisso.sort((a, b) => {
      return self.getOrderCompromisso(a) - self.getOrderCompromisso(b);
    }).filter(t => !this.columnNamesCompromisso.includes(t.name));
  }

  private _customColumnOrderCompromisso: string[] = [];
  getOrderCompromisso(column: TableColumn): number {
    var customOrder = this._customColumnOrderCompromisso.indexOf(column.name) + 1;
    if (customOrder) {
      return customOrder;
    } else {
      return column.order + this._customColumnOrderCompromisso.length;
    }
  }
  getPriorityCompromisso(column: TableColumn): number {
    var orderDiff = this.getOrderCompromisso(column) - column.order;
    var priorityDiff = column.order - column.priority;
    return column.priority + orderDiff + priorityDiff;
  }

  get allColumnsOrderedCompromisso(): TableColumn[] {
    var self = AgendaPageComponent.self;

    return this.allColumnsCompromissos.sort((a, b) => {
      return self.getOrderCompromisso(a) - self.getOrderCompromisso(b);
    });
  }

  changeColumnOrderCompromisso(columnName: string, increment: number): void {
    if (this._customColumnOrderCompromisso.length === 0) {
      this._customColumnOrderCompromisso = this.allColumnsCompromissos.sort((a, b) => {
        return a.order - b.order;
      }).map(t => t.name);
    } else if (this._customColumnOrderCompromisso.length < this.allColumnsCompromissos.length) {
      this._customColumnOrderCompromisso = this.allColumnsOrderedCompromisso.map(t => t.name);
    }
    
    var index = this._customColumnOrderCompromisso.indexOf(columnName);

    if (index < 0 || index >= this._customColumnOrderCompromisso.length) return;
    if ((index+increment) < 0 || (index+increment) >= this._customColumnOrderCompromisso.length) return;

    this._customColumnOrderCompromisso.splice(index+increment, 0, this._customColumnOrderCompromisso.splice(index, 1)[0]);
  }

  get dataSourceCompromissos(): Compromisso[] {
    return this.itensCompromissos.records;
  };

  convertKeysToUpper(data: {}): {} {
    const newData = {};
  
    // Itera sobre as chaves do objeto original
    Object.keys(data).forEach((key) => {
      // Cria a nova chave em UpperCase e atribui o valor original
      newData[key.toUpperCase()] = data[key];
  });

  return newData;
  }

  removerDuplicatas(arrayComDuplicatas: string[]): string[] {
    // Retorna apenas os elementos cuja primeira aparição (indexOf)
    // é igual à sua posição atual (index)
    return arrayComDuplicatas.filter((item, index) => {
        return arrayComDuplicatas.indexOf(item) === index;
    });
  }

  get gruposUsuarios(): string[] {
    
    if (this.temDireitoGeralAcesso) {
      // 1. Pega todos os valores (com repetidos)
      const todosGrupos = Object.values(this.usuarios);
      
      // 2. O "new Set" remove duplicatas e o "[... ]" transforma de volta em array
      return this.removerDuplicatas(todosGrupos);
    }
  
    if (this.temDireitoGrupoAcesso) {
      const userName = this._userService.getUserName();
      const grupoDoUsuarioLogado = this.usuarios[userName]; // Ex: "VENDEDOR"
  
      if (!grupoDoUsuarioLogado) return [];

      return [grupoDoUsuarioLogado];
    }
  
    return [];
  }
  
  get usuariosDoGrupo(): string[] {

    if (this.gruposSelecionados.length === 0) {
      return [];
    }

    const keys = Object.keys(this.usuarios)
      .filter(key => this.gruposSelecionados.filter(x => x === this.usuarios[key]).length > 0)
      .map(key => key.toUpperCase());

    const result = this.removerDuplicatas(keys);
  
    return result;
  }

  @ViewChild('cModalVCR', { read: ViewContainerRef, static: false }) compromissoModalVCR: ViewContainerRef;
  @ViewChild('colunasVisualizacaoModalCompromissoVCR', { read: ViewContainerRef, static: false }) CompromissoColunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild(FilterComponent, { static: false }) compromissoFilter: FilterComponent;

  //#endregion

  constructor(
    private _dialog: MatDialog,
      private _cdr: ChangeDetectorRef,
      private _userService: UserService,
      private _tarefaService: TarefaService,
      private _compromissoService: CompromissoService,
      private _formBuilder: FormBuilder,
  ) { 

    AgendaPageComponent.self = this;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7007','', 50);
      if (!temDireito) return;

      this.formUsuario = this._formBuilder.group({});

      this.temDireitoGrupoAcesso = this._userService.temDireitoAplicativo('WEB7009', '');
      this.temDireitoGrupoInclui = this._userService.temDireitoAplicativo('WEB7009', 'I');
      this.temDireitoGrupoAlteracao = this._userService.temDireitoAplicativo('WEB7009', 'A');
      this.temDireitoGrupoDeletar = this._userService.temDireitoAplicativo('WEB7009', 'E');

      this.temDireitoGeralAcesso = this._userService.temDireitoAplicativo('WEB7010', '');
      this.temDireitoGeralInclui = this._userService.temDireitoAplicativo('WEB7010', 'I');
      this.temDireitoGeralAlteracao = this._userService.temDireitoAplicativo('WEB7010', 'A');
      this.temDireitoGeralDeletar = this._userService.temDireitoAplicativo('WEB7010', 'E');
  
      this._userService.gravarAcessoAplicacao("Agenda", 7007);
      this._compromissoService.ListarGruposDeUsuarioRespeitandoDireito()
      .then((usuarios) => {
        this.usuarios = this.convertKeysToUpper(usuarios);
      });
  }

  ngOnInit() {    
  }

  ngAfterViewInit(): void {
  }

  tarefaFilterChange(novoFiltro: string) {
    this.filtroTarefaString = novoFiltro;
    if (this.filtroTarefa.dataDe) this.filtroTarefa.dataDe = new Date(this.filtroTarefa.dataDe);
    if (this.filtroTarefa.dataAte) this.filtroTarefa.dataAte = new Date(this.filtroTarefa.dataAte);
    this.getTarefaPage();
  }

  compromissoFiltroChange(novoFiltro: string) {
    this.filtroCompromissoString = novoFiltro;
    if (this.filtroCompromisso.dataDe) this.filtroCompromisso.dataDe = new Date(this.filtroCompromisso.dataDe);
    if (this.filtroCompromisso.dataAte) this.filtroCompromisso.dataAte = new Date(this.filtroCompromisso.dataAte);
    this.getCompromissoPage();
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  viewTarefaChanged(view: ICustomView) {      
    if (!this.tarefaFilter) return;
    this.setFilterTarefa(view.value ? view.value.filter : this.tarefaFilter.defaultModel);
    this.isHiddenColumnTarefa = view.value && view.value.hiddenColumns ? view.value.hiddenColumns : [];
    this._customColumnOrderTarefa = view.value && view.value.customColumnOrder ? view.value.customColumnOrder : [];
    this._cdr.detectChanges();
    this.tarefaFilter.aplyFilter();
  }

  setFilterTarefa(newFilter) {
    Object.keys(newFilter).forEach(t => this.filtroTarefa[t] = newFilter[t]);
  }

  getTarefaPage(pageInfo?) {
    let currentPage = this.paginaAtualTarefa;
    let pageSize = this.registrosPorPaginaTarefa;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };
    
    var filtroUsuarios: string = "";

    this.usuariosSelecionados.forEach(usuario => {
      filtroUsuarios = filtroUsuarios + "|" +  usuario
    });

    this._tarefaService.ListarEmOrdemDecrescentePorHorario(currentPage, pageSize, this.filtroTarefaString, this.filtroTarefaAtrasada, this.filtroHorarioDe,this.filtroHorarioAte, filtroUsuarios, false)
    .then(
      Tarefa => {
        this.itensTarefas = Tarefa;
        this.paginaAtualTarefa = Tarefa.currentPage;
        this.registrosPorPaginaTarefa = Tarefa.pageSize;
      },
      error => { this.itensTarefas = new PagedList<Tarefa>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  updateTarefaFinalizado(tarefa: Tarefa): void {
    
    tarefa.finalizado = true;
    this._tarefaService.Atualizar(tarefa)
    .then(success => {
     this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Tarefa concluída com sucesso`
        }
      });
      this.getTarefaPage();
    }, err => {
     this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao concluir a Tarefa.\n${this.formataErrosApi(err)}`
        }
      });
    });
 }

   usuariosItemTarefaAgrupamento:  { [key: string]: string } = {};
   async deleteTarefa(tarefa: Tarefa): Promise<void> {
    let self = AgendaPageComponent.self;

    var possuiDireitoGrupoeGeral = true;

    if(tarefa.idAgrupamento !== '') {
      await this._tarefaService.ListarUsuarioAgrupamento(tarefa.idAgrupamento)
      .then((usuariosAgrupamento) => {
        this.usuariosItemTarefaAgrupamento = usuariosAgrupamento;
      });
      const userName = this._userService.getUserName();
      const grupoDoUsuarioLogado = this.usuarios[userName];
      const existeUsuarioGrupoDiferenteUsuario = Object.values(this.usuariosItemTarefaAgrupamento).filter(x => x !== grupoDoUsuarioLogado).length > 0;

      possuiDireitoGrupoeGeral = (existeUsuarioGrupoDiferenteUsuario && this.temDireitoGeralDeletar)
      possuiDireitoGrupoeGeral = possuiDireitoGrupoeGeral || (!existeUsuarioGrupoDiferenteUsuario && (this.temDireitoGrupoDeletar || this.temDireitoGeralDeletar))
    }

    var temDireito = this._userService.temDireitoAplicativo('WEB7007','E');
    if (!temDireito || !possuiDireitoGrupoeGeral) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de exclusão!`
        }
        
    })
    return;}

    self._tarefaService.Deletar(tarefa.codigo)
      .then(success => {
        self.getTarefaPage();
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

  getFormattedValueTarefa(element: Tarefa, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  getTarefaTelefone(tarefa: Tarefa): string {
    return Tasks.formataTelefone(tarefa.dddTelefone, tarefa.telefone);
  }

  getTarefaData(tarefa: Tarefa): string {
    return Tasks.formataData(tarefa.data) + "  " + tarefa.horario.substring(0, 5)
  }

  getTarefaUsuario(Tarefa: Tarefa): string {
    return Tarefa.idAgrupamento !== "" ? Tarefa.usuario + " [GRUPO]" : Tarefa.usuario;
   }

  getTarefaOrigem(tarefa: Tarefa): string {
    if(tarefa.anoVisita !== 0 && tarefa.numeroVisita !== 0) return 'VISITA : ' + tarefa.numeroVisita.toString().padStart(6, '0') + '-' + tarefa.anoVisita;

    if(tarefa.anoLead !== 0 && tarefa.numeroLead !== 0) return 'LEAD : ' + tarefa.numeroLead.toString().padStart(6, '0') + '-' + tarefa.anoLead;

    if(tarefa.anoOportunidade !== 0 && tarefa.numeroOportunidade !== 0) return 'OPORTUNIDADE : ' + tarefa.numeroOportunidade.toString().padStart(6, '0') + '-' + tarefa.anoOportunidade;

    return '';
  }

  getTarefaCelular(tarefa: Tarefa): string {
    return Tasks.formataTelefone(tarefa.dddCelular, tarefa.celular);
  }
  
  getMensagemConfirmacaoExclusaoTarefa(tarefa: Tarefa) {

    if(tarefa.idAgrupamento !== '') {
      return "Esta tarefa está vinculada a outras. Deseja realmente excluir a tarefa e tarefas vinculadas ?"
    }

    return 'Confirma exclusão deste item ?';

  }

  getMensagemConfirmacaoExclusaoCompromisso(compromisso: Compromisso) {

    if(compromisso.idAgrupamento !== '') {
      return "Este compromisso está vinculado a outros. Deseja realmente excluir o compromisso e compromissos vinculados ?"
    }

    return 'Confirma exclusão deste item ?';

  }

  getMensagemConfirmacaoFinalizacaoTarefa(tarefa: Tarefa) {

    if(tarefa.idAgrupamento !== '') {
      return "Esta tarefa está vinculada a outras. Confirma a conclusão da tarefa e tarefas vinculadas ?"
    }

    return 'Confirma conclusão deste item ?';

  }
  

  showSelecaoColunasModalTarefa(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.tarefaColunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  closeModal() {
    let self = AgendaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;
  }
  
  confirmCallback() {
    let self = AgendaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;

    this.getTarefaPage();
    this.getCompromissoPage();
  }

  confirmModal: Function;
  cancelModal: Function;
  showTarefaModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, tarefa?: Tarefa) {
    
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if (tarefa) {
      var temDireito = this._userService.temDireitoAplicativo('WEB7007','A');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de alteração!`
          }
        });
        return;
      }
      this.itemTarefa = JSON.parse(JSON.stringify(tarefa));
    } else {
      var temDireito = this._userService.temDireitoAplicativo('WEB7007','I');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão para inserir uma nova Tarefa!`
          }
        });
        return;
      }
      this.itemTarefa = new Tarefa();
    }

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  horaValidator(hora: string): ICustomValidator {
    var _self = AgendaPageComponent.self;
    var _tasks = Tasks;

    if(hora === "") return;

    var message = 'Informe uma hora válida';

    return {
      key: 'horaInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        return !(_tasks.horarioValido(hora));
      },
      params: [hora]
    }
  }

  /* COMPROMISSO */

  showCompromissoModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, compromisso?: Compromisso) {    
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if (compromisso) {
      var temDireito = this._userService.temDireitoAplicativo('WEB7007','A');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de alteração!`
          }
        });
        return;
      }
      this.itemCompromisso = JSON.parse(JSON.stringify(compromisso));
    } else {
      var temDireito = this._userService.temDireitoAplicativo('WEB7007','I');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão para inserir um novo Compromisso!`
          }
        });
        return;
      }
      this.itemCompromisso = new Compromisso();
    }

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  getCompromissoPage(pageInfo?) {
    let currentPage = this.paginaAtualCompromisso;
    let pageSize = this.registrosPorPaginaCompromisso;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };

    var filtroUsuarios: string = "";

    this.usuariosSelecionados.forEach(usuario => {
      filtroUsuarios = filtroUsuarios + "|" +  usuario
    });
    
    this._compromissoService.ListarEmOrdemDecrescentePorHorario(currentPage, pageSize, this.filtroCompromissoString,  this.filtroCompromissoHoraInicioDe, this.filtroCompromissoHoraInicioAte, this.filtroCompromissoHoraFinalDe, this.filtroCompromissoHoraFinalAte, filtroUsuarios, false)
    .then(
      Compromisso => {
        this.itensCompromissos = Compromisso;
        this.paginaAtualCompromisso = Compromisso.currentPage;
        this.registrosPorPaginaCompromisso = Compromisso.pageSize;
      },
      error => { this.itensCompromissos = new PagedList<Compromisso>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  usuariosItemCompromissoAgrupamento:  { [key: string]: string } = {};
  async deleteCompromisso(compromisso: Compromisso): Promise<void> {
    let self = AgendaPageComponent.self;

    var possuiDireitoGrupoeGeral = true;

    if(compromisso.idAgrupamento !== '') {
      await this._compromissoService.ListarUsuarioAgrupamento(compromisso.idAgrupamento)
      .then((usuariosAgrupamento) => {
        this.usuariosItemCompromissoAgrupamento = usuariosAgrupamento;
      });
      const userName = this._userService.getUserName();
      const grupoDoUsuarioLogado = this.usuarios[userName];
      const existeUsuarioGrupoDiferenteUsuario = Object.values(this.usuariosItemCompromissoAgrupamento).filter(x => x !== grupoDoUsuarioLogado).length > 0;

      possuiDireitoGrupoeGeral = (existeUsuarioGrupoDiferenteUsuario && this.temDireitoGeralDeletar)
      possuiDireitoGrupoeGeral = possuiDireitoGrupoeGeral || (!existeUsuarioGrupoDiferenteUsuario && (this.temDireitoGrupoDeletar || this.temDireitoGeralDeletar))
    }

    var temDireito = this._userService.temDireitoAplicativo('WEB7007','E');
    if (!temDireito || !possuiDireitoGrupoeGeral) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de exclusão!`
        }
        
    })
    return;}

    self._compromissoService.Deletar(compromisso.codigo)
      .then(success => {
        self.getCompromissoPage();
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

  viewCompromissoChanged(view: ICustomView) {      
    if (!this.compromissoFilter) return;
    this.setFilterCompromisso(view.value ? view.value.filter : this.compromissoFilter.defaultModel);
    this.isHiddenColumnCompromisso = view.value && view.value.hiddenColumns ? view.value.hiddenColumns : [];
    this._customColumnOrderCompromisso = view.value && view.value.customColumnOrder ? view.value.customColumnOrder : [];
    this._cdr.detectChanges();
    this.compromissoFilter.aplyFilter();
  }

  setFilterCompromisso(newFilter) {
    Object.keys(newFilter).forEach(t => this.filtroCompromisso[t] = newFilter[t]);
  }

  getFormattedValueCompromisso(element: Compromisso, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  getCompromissoTelefone(Compromisso: Compromisso): string {
    return Tasks.formataTelefone(Compromisso.dddTelefone, Compromisso.telefone);
  }

  getCompromissoDataInicio(Compromisso: Compromisso): string {
     return Tasks.formataData(Compromisso.dataInicio) + "  " + (Compromisso.horaInicio ? Compromisso.horaInicio.substring(0, 5) : "");
   }

   getCompromissoUsuario(Compromisso: Compromisso): string {
    return Compromisso.idAgrupamento !== "" ? Compromisso.usuario + " [GRUPO]" : Compromisso.usuario;
   }

   getCompromissoDataFim(Compromisso: Compromisso): string {
    return Tasks.formataData(Compromisso.dataFim) + "  " + (Compromisso.horaFim ? Compromisso.horaFim.substring(0, 5) : "");
  }

  getCompromissoOrigem(Compromisso: Compromisso): string {
    if(Compromisso.anoVisita !== 0 && Compromisso.numeroVisita !== 0) return 'VISITA : ' + Compromisso.numeroVisita.toString().padStart(6, '0') + '-' + Compromisso.anoVisita;

    if(Compromisso.anoLead !== 0 && Compromisso.numeroLead !== 0) return 'LEAD : ' + Compromisso.numeroLead.toString().padStart(6, '0') + '-' + Compromisso.anoLead;

    if(Compromisso.anoOportunidade !== 0 && Compromisso.numeroOportunidade !== 0) return 'OPORTUNIDADE : ' + Compromisso.numeroOportunidade.toString().padStart(6, '0') + '-' + Compromisso.anoOportunidade;

    return '';
  }

  getCompromissoCelular(Compromisso: Compromisso): string {
    return Tasks.formataTelefone(Compromisso.dddCelular, Compromisso.celular);
  }
    

  showSelecaoColunasModalCompromisso(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.CompromissoColunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

}
