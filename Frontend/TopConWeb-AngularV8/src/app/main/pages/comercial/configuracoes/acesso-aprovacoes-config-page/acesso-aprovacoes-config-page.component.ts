import { Component, OnInit, AfterViewInit, ViewChild, 
    ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef, Self, SkipSelf } from '@angular/core';
  import { MatDialogRef, MatDialog} from '@angular/material';
  import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
  import { trigger, state, transition, style, animate } from '@angular/animations';
  
  import { Tasks } from 'app/classes/_tasks/tasks';
  import { PagedList } from 'app/classes/pagination/paged-list';
  
  import { FilterComponent } from 'app/main/components/list/filter/filter.component';
  
  import { UserService } from 'app/services/user.service';
  import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
  import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
  import { ConfirmDialogButtonComponent } from 'app/main/components/dialog/confirm-dialog-button/confirm-dialog-button.component';
  import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
  import { GrupoAcesso } from 'app/classes/acesso-aprovacoes/grupo-acesso';
  import { LiberacaoAcesso, Usuario } from 'app/classes/acesso-aprovacoes/liberacao-acesso';
  import { CadastroDiverso } from 'app/classes/cadastro-geral/cadastro-diverso';
  import { AcessoAprovacoesConfigService } from 'app/services/acesso-aprovacoes-config.service';
  
  import * as _ from 'lodash';
import { Usina } from 'app/classes/usina/usina';
import { UsinaService } from 'app/services/usina.service';
import { SelectorFlags } from '@angular/compiler/src/core';
import { Console } from 'console';
import { WriteVarExpr } from '@angular/compiler';
import { PeriodoAusenciaUsuario } from 'app/classes/acesso-aprovacoes/periodo-ausencia-usuario';
  
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
    selector: 'app-acesso-aprovacoes-config-page',
    templateUrl: './acesso-aprovacoes-config-page.component.html',
    styleUrls: ['./acesso-aprovacoes-config-page.component.scss'],
    animations: [
      trigger('detailExpand', [
        state('collapsed', style({height: '0px', minHeight: '0'})),
        state('expanded', style({height: '*'})),
        transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      ]),
    ]
  })
  export class AcessoAprovacoesConfigPageComponent implements OnInit, AfterViewInit {
    public static self: AcessoAprovacoesConfigPageComponent;
  
    grupoForm: FormGroup;
    configUsuariosForm: FormGroup;
    liberacaoAcessoUsuarioForm: FormGroup;
  
    itens: PagedList<GrupoAcesso> = new PagedList<GrupoAcesso>();
    item: GrupoAcesso;
  
    gruposAcesso: GrupoAcesso[] = [];
    usinas: Usina[] = [];
  
    paginaAtual: number = 1;
    registrosPorPagina: number = 30;
  
    filtroString: string = '';
  
    filtroPesquisaUsuarios: CadastroDiverso[] = [];

    diasArray: string[] = ['SEGUNDA', 'TERCA', 'QUARTA', 'QUINTA', 'SEXTA', 'SABADO', 'DOMINGO'];
    turnosArray: number[] = [1,2];
  
    filtro: {
      grupoAcesso: GrupoAcesso,
      codigo: number,
      descricao: string,
      usuario: CadastroDiverso,
      usuarioRazao: string
    } = {
      grupoAcesso: null,
      codigo: 0,
      descricao: '',
      usuario: null,
      usuarioRazao: ''
    };
  
    formataData = Tasks.formataData;
    formataHora = Tasks.formataHora;
    formataValor = Tasks.formataValor;
    formataErrosApi = Tasks.formataErrosApi;
  
    modalIsOpen: boolean = false;
    subModalIsOpen: boolean = false;
    private _dialogRef: MatDialogRef<any>;
    private _subDialogRef: MatDialogRef<any>;
  
    usuarios: CadastroDiverso[] = [];
  
    usuarioFormatter = (model: Usuario): string => model ? model.nome.toUpperCase() : '';
    usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';
    grupoAcessoFormatter = (model: GrupoAcesso) => model ? model.descricao.toUpperCase() : '';
    
    _allColumns: TableColumn[] = [
      { name: 'codigo', placeholder: 'Código', order: 1, priority: 1 },
      { name: 'usinaInf', placeholder: 'Central', order: 2, priority: 2, formatter: this.usinaFormatter},
      { name: 'descricao', placeholder: 'Descrição', order: 3, priority: 3}
    ];
  
    get allColumns(): TableColumn[] {
        return this._allColumns;
    }
  
    expandedElement: GrupoAcesso | null;
  
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
      var self = AcessoAprovacoesConfigPageComponent.self;
      
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
      var self = AcessoAprovacoesConfigPageComponent.self;
      
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
      var self = AcessoAprovacoesConfigPageComponent.self;
  
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
  
  
    get dataSource(): GrupoAcesso[] {
      return this.itens.records;
    };
  
    exibirAcompanhamento: boolean = false;
  
  
    @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  
    @ViewChild('gModalVCR', { read: ViewContainerRef, static: false }) grupoAcessoModalVCR: ViewContainerRef;
    @ViewChild('configUsuarioModalVCR', { read: ViewContainerRef, static: false }) configUsuarioModalRef: ViewContainerRef;
    @ViewChild('liberacaoAcessoUsuarioModalVCR', { read: ViewContainerRef, static: false }) liberacaoAcessoUsuarioModalRef: ViewContainerRef;
    @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
    @ViewChild('subModalVCR', { read: ViewContainerRef, static: false }) SubModalRef: ViewContainerRef;
  
    constructor(
      private _dialog: MatDialog,
      private _cdr: ChangeDetectorRef,
      private _formBuilder: FormBuilder,
      private _userService: UserService,
      private _usinaService: UsinaService,
      private _acessoAprovacoesConfigService: AcessoAprovacoesConfigService
    ) {
      AcessoAprovacoesConfigPageComponent.self = this;
      this.carregaUsina();
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7002','', 50);
      if (!temDireito) return;
  
      this._userService.gravarAcessoAplicacao("Cadastro", 6007);
    }
  
    ngOnInit() {
      this.grupoForm = this._formBuilder.group({
        liberacaoAcesso: this._formBuilder.array([])
      });;
      this.configUsuariosForm = this._formBuilder.group({});
      this.liberacaoAcessoUsuarioForm = this._formBuilder.group({
        liberacaoAcesso: this._formBuilder.array([]),
        tipoAusencia: ['']
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
      
      this._acessoAprovacoesConfigService.Listar(currentPage, pageSize, this.filtroString)
      .then(
        GrupoAcesso => {
          this.itens = GrupoAcesso;

          this.itens.records.forEach(grupo => {
            grupo.usinaInf = this.usinas.find(usina => usina.codigo === grupo.usina) || new Usina();
          });
          
          this.paginaAtual = GrupoAcesso.currentPage;
          this.registrosPorPagina = GrupoAcesso.pageSize;
        },
       error => { this.itens = new PagedList<GrupoAcesso>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
    }

    carregaUsina() {
        this._usinaService.listarTodos().then(
          usinas => { this.usinas = usinas; },
          error => { this.usinas = [] }
        );
    }
  
    getFormattedValue(element: GrupoAcesso, column: TableColumn) {
      return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
    }
  
    filtroChange(novoFiltro: string){
      this.filtroString = novoFiltro;
      this.getPage();
    }

  
    private _timeoutGrupoAcessoPorDescricao = null;
    filtrarGrupoAcessoPorDescricao(grupoAcesso: string) {
      this.filtro.descricao = grupoAcesso;
  
      var tamanhoMinimo = (isNaN(parseInt(grupoAcesso)) ? 2 : 0);
  
      if (grupoAcesso && grupoAcesso.length>tamanhoMinimo && (!this.filtro.grupoAcesso || this.filtro.grupoAcesso.descricao!=grupoAcesso)) {
        
        if (this._timeoutGrupoAcessoPorDescricao) clearTimeout(this._timeoutGrupoAcessoPorDescricao);
        
        var filtro = 'filter=$(' + (isNaN(parseInt(grupoAcesso)) ? 'descricao==' + grupoAcesso : 'codigo==' + parseInt(grupoAcesso)) + ')';
  
        this._timeoutGrupoAcessoPorDescricao = setTimeout( () => {
          this._acessoAprovacoesConfigService.Listar(null, null, filtro)
            .then(
              gruposAcesso => { this.gruposAcesso = gruposAcesso.records; },
              error => { this.gruposAcesso = []; }
            )
        }, 500);
  
      } else {
        this.gruposAcesso = [];
      }
    }
  
    showSelecaoColunasModal(content) {
      this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
      this.modalIsOpen = true;
    }
  
    confirmModal: Function;
    cancelModal: Function;
    showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, grupoAcesso?: GrupoAcesso) {
      this.confirmModal = confirmCallback;
      this.cancelModal = cancelCallback || this.closeModal;
  
      if (grupoAcesso) {
        var temDireito = this._userService.temDireitoAplicativo('WEB7002','A');
        if (!temDireito) {
          this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Você não tem permissão de alteração!`
            }
          });
          return;
        }
         this.item = JSON.parse(JSON.stringify(grupoAcesso));

         this.item.usinaInf = this.usinas.find(usina => usina.codigo === this.item.usina) || new Usina();

         const liberacaoArray = this.grupoForm.get('liberacaoAcesso') as FormArray;
         liberacaoArray.clear();
 
        this.diasArray.forEach((dia, diaIndex) => {
          this.turnosArray.forEach((turno, turnoIndex) => {
              const liberacao = this.item.liberacoesAcessos.find(l => l.diaSemana === dia && l.turno === turno && !l.usuario);

              liberacaoArray.push(this._formBuilder.group({
                  codigo: [liberacao ? liberacao.codigo : ''],
                  usuario: [liberacao ? liberacao.usuario : ''],
                  grupo: [liberacao ? liberacao.grupo : ''],
                  tipoLiberacao: [liberacao ? liberacao.tipoLiberacao : ''],
                  diaSemana: [dia],
                  turno: [turno],
                  horaEntrada: [liberacao ? liberacao.horaEntrada : ''],
                  horaSaida: [liberacao ? liberacao.horaSaida : ''],
                  bloquear: [liberacao ? liberacao.bloquear : false],
                  criadoEm: [liberacao ? liberacao.criadoEm : ''],
                  atualizadoEm: [liberacao ? liberacao.atualizadoEm : '']
              }));
          });
      });
      } else {
        var temDireito = this._userService.temDireitoAplicativo('WEB7002','I');
        if (!temDireito) {
          this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Você não tem permissão para inserir um novo Grupo!`
            }
          });
          return;
        }

        this.item = new GrupoAcesso();
        this.item.liberacoesAcessos = [];

        this.diasArray.forEach((_, diaIndex) => {
            this.turnosArray.forEach(() => {
                this.item.liberacoesAcessos.push(new LiberacaoAcesso());
            });
        });
        
        const liberacaoArray = this.grupoForm.get('liberacaoAcesso') as FormArray;
        liberacaoArray.clear();

        this.diasArray.forEach((dia, diaIndex) => {
            this.turnosArray.forEach((turno, turnoIndex) => {
                liberacaoArray.push(this._formBuilder.group({
                    diaSemana: [dia], 
                    turno: [turno], 
                    horaEntrada: [''],
                    horaSaida: [''],
                    bloquear: [false] 
                }));
            });
        });
      }
      this._cdr.detectChanges();
  
      this._dialogRef = this._dialog.open(content, { viewContainerRef: container });
  
      this.modalIsOpen = true;
  
    }
  
    closeModal() {
      let self = AcessoAprovacoesConfigPageComponent.self;
      
      if (self._dialogRef) self._dialogRef.close();
  
      self.grupoForm.markAsPristine();
      self.grupoForm.markAsUntouched();
  
      self.configUsuariosForm.markAsPristine();
      self.configUsuariosForm.markAsUntouched();
  
      self.modalIsOpen = false;
    }

    get liberacaoAcesso() {
        return this.grupoForm.get('liberacaoAcesso') as FormArray;
    }
  
    deleteGrupoAcesso(grupoAcesso: GrupoAcesso): void {
      let self = AcessoAprovacoesConfigPageComponent.self;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7002','E');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de exclusão!`
          }
          
      })
      return;}

      self._acessoAprovacoesConfigService.Deletar(grupoAcesso.codigo)
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
      });;
    }
    
    updateGrupoAcesso(grupoAcesso: GrupoAcesso): void {
      let self = AcessoAprovacoesConfigPageComponent.self;

      const liberacaoArray = this.grupoForm.get('liberacaoAcesso') as FormArray;
      grupoAcesso.liberacoesAcessos = liberacaoArray.value;

      self._dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Deseja realizar a alteração em todos os usuários do grupo?`,
          confirmCallback: async () => {
            await self._acessoAprovacoesConfigService.Atualizar(grupoAcesso, true)
            .then(success => {
              self.closeModal();
              self.getPage();
              self._dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `Grupo alterado com sucesso!`
                }
              });
            }, err => {
              self._dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `Erro alterar o Grupo.\n${self.formataErrosApi(err)}`
                }
              });
            });
          },
          cancelCallback: async () => {
            await self._acessoAprovacoesConfigService.Atualizar(grupoAcesso, false)
            .then(success => {
              self.closeModal();
              self.getPage();
              self._dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `Grupo alterado com sucesso!`
                }
              });
            }, err => {
              self._dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `Erro alterar o Grupo.\n${self.formataErrosApi(err)}`
                }
              });
            });
          }
        }
      });
    }
  
    addGrupoAcesso(grupoAcesso: GrupoAcesso): void {
      let self = AcessoAprovacoesConfigPageComponent.self;

      const liberacaoArray = this.grupoForm.get('liberacaoAcesso') as FormArray;
      grupoAcesso.liberacoesAcessos = liberacaoArray.value;

      self._acessoAprovacoesConfigService.Adicionar(grupoAcesso)
      .then(success => {
        self.closeModal();
        self.getPage();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Grupo adicionado com sucesso!`
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

    bloquearDia(form: FormGroup, diaIndex: number) {
      const primeiroTurno = diaIndex * 2;
      const segundoTurno = diaIndex * 2 + 1;

      const liberacaoArray = form.get('liberacaoAcesso') as FormArray;
      const bloqueado = liberacaoArray.at(primeiroTurno).get('bloquear').value;

      liberacaoArray.at(primeiroTurno).get('bloquear').setValue(!bloqueado);
      liberacaoArray.at(segundoTurno).get('bloquear').setValue(!bloqueado);
    }

    isHoraEntradaRequired(form: FormGroup, diaIndex: number, turnoIndex: number): boolean{
      const liberacaoAcessoArray = form.get('liberacaoAcesso') as FormArray;
      const horaEntrada = liberacaoAcessoArray.at(diaIndex * 2 + turnoIndex).get('horaEntrada').value;
      const horaSaida = liberacaoAcessoArray.at(diaIndex * 2 + turnoIndex).get('horaSaida').value;
      return (horaSaida == "" || horaEntrada == "") && !(horaSaida== "" && horaEntrada =="");
    }
  
    isHoraSaidaRequired(form: FormGroup, diaIndex: number, turnoIndex: number): boolean {
      const liberacaoAcessoArray = form.get('liberacaoAcesso') as FormArray;
      const horaEntrada = liberacaoAcessoArray.at(diaIndex * 2 + turnoIndex).get('horaEntrada').value;
      const horaSaida = liberacaoAcessoArray.at(diaIndex * 2 + turnoIndex).get('horaSaida').value;
      return (horaSaida == "" || horaEntrada == "") && !(horaSaida== "" && horaEntrada =="");
    }
  
   // -------------- Modal - Cadastro Usuarios -------------------------------------

    usuariosDisponiveis?: Usuario[];
    usuariosAgrupados = {};
    usuarioAtual = '';
  
    usuarioSelecionado: Usuario = new Usuario();
  
    tipoAusenciaSelecionado = "";
    feriasSelecionado = false;
    periodosAusenciaUsuario: PeriodoAusenciaUsuario[];
    periodoAtual: PeriodoAusenciaUsuario = new PeriodoAusenciaUsuario();
    periodoFeriasAtual: PeriodoAusenciaUsuario = new PeriodoAusenciaUsuario();
  
    showConfiguracaoModal(content, container: ViewContainerRef, grupoAcesso: GrupoAcesso, cancelCallback?: Function) {
      let self = AcessoAprovacoesConfigPageComponent.self;
      
      self.cancelModal = cancelCallback || self.closeModal;
  
      self.usuarioSelecionado = new Usuario();
      self.usuariosAgrupados = {};
  
      var temDireito = self._userService.temDireitoAplicativo('WEB7002','A');
      if (!temDireito) {
        self._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de alteração!`
          }
        });
        return;
      }
  
      self._acessoAprovacoesConfigService.ListarUsuariosDisponiveis()
      .then(
        usuarios => { self.usuariosDisponiveis = usuarios },
        err => { self. usuariosDisponiveis = [] }
      )
  
      self.item = JSON.parse(JSON.stringify(grupoAcesso));
  
      self._acessoAprovacoesConfigService.ListarUsuariosPorGrupo(grupoAcesso.codigo)
      .then(
        usuarios => {
          if (usuarios != undefined){      
            usuarios.forEach(liberacao => {
              if (!self.usuariosAgrupados[liberacao.usuario]) {
                self.usuariosAgrupados[liberacao.usuario] = {
                  usuarioId: liberacao.usuario, // ID do usuário
                  liberacoesAcessos: []
                };
              }
              self.usuariosAgrupados[liberacao.usuario].liberacoesAcessos.push(liberacao);
            });
          }
        }
      )
  
      this._cdr.detectChanges();
  
      this._dialogRef = this._dialog.open(content, { viewContainerRef: container });
  
      this.modalIsOpen = true;
    }
    
    confirmSubModal: Function;
    cancelSubModal: Function;
  
    showSubModal(content, container: ViewContainerRef, confirmCallback: Function, liberacoesAcessos: LiberacaoAcesso[], cancelCallback?: Function) {
  
      let self = AcessoAprovacoesConfigPageComponent.self;
  
      self.confirmSubModal = confirmCallback;
      self.cancelSubModal = cancelCallback || self.closeModal;
  
      self.liberacaoAcessoUsuarioForm.get('tipoAusencia').reset();
      self.usuarioAtual = liberacoesAcessos.length > 0 ? liberacoesAcessos[0].usuario : null;
      self.periodoAtual= new PeriodoAusenciaUsuario();
      self.tipoAusenciaSelecionado = "";
  
      self._acessoAprovacoesConfigService.ListarPeriodosAusenciaPorUsuario(self.usuarioAtual)
      .then( periodos => {
        self.periodosAusenciaUsuario = periodos;

        self.periodoAtual = this.periodosAusenciaUsuario.find(periodo => periodo.tipoAusencia === "AUSENCIA" || periodo.tipoAusencia === "AFASTAMENTO");
        if(self.periodoAtual){
          self.tipoAusenciaSelecionado = self.periodoAtual.tipoAusencia;
          self.periodoAtual.checked = true;
        }
        
        self.periodoFeriasAtual = this.periodosAusenciaUsuario.find(periodo => periodo.tipoAusencia === "FERIAS");
        if(self.periodoFeriasAtual){
          self.feriasSelecionado = true;
          self.periodoFeriasAtual.checked = true;
        }
      })
  
      const liberacaoArray = self.liberacaoAcessoUsuarioForm.get('liberacaoAcesso') as FormArray;
      liberacaoArray.clear();
  
      self.diasArray.forEach((dia, diaIndex) => {
        self.turnosArray.forEach((turno, turnoIndex) => {
          const liberacao = liberacoesAcessos.find(l => l.diaSemana === dia && l.turno === turno);
  
          liberacaoArray.push(self._formBuilder.group({
              codigo: [liberacao ? liberacao.codigo : ''],
              usuario: [liberacao ? liberacao.usuario : ''],
              grupo: [liberacao ? liberacao.grupo : ''],
              tipoLiberacao: [liberacao ? liberacao.tipoLiberacao : ''],
              diaSemana: [dia],
              turno: [turno],
              horaEntrada: [liberacao ? liberacao.horaEntrada : ''],
              horaSaida: [liberacao ? liberacao.horaSaida : ''],
              bloquear: [liberacao ? liberacao.bloquear : false],
              criadoEm: [liberacao ? liberacao.criadoEm : ''],
              atualizadoEm: [liberacao ? liberacao.atualizadoEm : '']
          }));
        });
      });
  
      this._cdr.detectChanges();
  
      self.openSubModal(content);
    }
  
    openSubModal(content) {
      let self = AcessoAprovacoesConfigPageComponent.self
  
      self._subDialogRef = self._dialog.open(content, { viewContainerRef: self.SubModalRef });
      self.subModalIsOpen = true;
    }
  
    closeSubModal() {
      let self = AcessoAprovacoesConfigPageComponent.self
  
      if (self._subDialogRef) self._subDialogRef.close();
  
      self.subModalIsOpen = false;
    }
  
    onTipoAusenciaChange(tipo: string, isChecked: boolean) {
      let self = AcessoAprovacoesConfigPageComponent.self
      setTimeout(() => {
        if (isChecked) {
          self.tipoAusenciaSelecionado = tipo;
    
          self.periodoAtual = self.periodosAusenciaUsuario.find(periodo => periodo.tipoAusencia === tipo);
        
          if (!self.periodoAtual) {
            self.periodoAtual = { codigo: 0, usuario: self.usuarioAtual, tipoLiberacao: 'APROVACOES', tipoAusencia: tipo, inicioPeriodo: null, fimPeriodo: null, criadoEm: null, atualizadoEm: null, checked: true};
            self.periodosAusenciaUsuario.push(self.periodoAtual);
          }
        } else {
          if (self.tipoAusenciaSelecionado === tipo) {
            self.tipoAusenciaSelecionado = null;
            self.periodoAtual.checked = false;
          }
        }
      });
    }

    onFeriasChange(isChecked: boolean) {
      let self = AcessoAprovacoesConfigPageComponent.self

      setTimeout(() => {
        self.feriasSelecionado = isChecked;
  
        self.periodoFeriasAtual = this.periodosAusenciaUsuario.find(periodo => periodo.tipoAusencia === "FERIAS");
      
        if (!self.periodoFeriasAtual) {
          self.periodoFeriasAtual = { codigo: 0, usuario: self.usuarioAtual, tipoLiberacao: 'APROVACOES', tipoAusencia: "FERIAS", inicioPeriodo: null, fimPeriodo: null, criadoEm: null, atualizadoEm: null, checked: true};
          self.periodosAusenciaUsuario.push(self.periodoFeriasAtual);
        }

        if(!isChecked){
          self.periodoFeriasAtual.checked = false;
        }
      });
    }
  
    adicionarUsuario() {
      let self = AcessoAprovacoesConfigPageComponent.self;

      if(!self.usuarioSelecionado || self.usuarioSelecionado.id == "") {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${"Favor selecionar um usuário."}`
          }
        });
        return;
      }
  
      var usuario: LiberacaoAcesso[] = [];
  
      this.diasArray.forEach((dia, diaIndex) => {
        this.turnosArray.forEach((turno, turnoIndex) => {
          usuario.push({
            codigo: 0,
            usuario: self.usuarioSelecionado.id,
            grupo: self.item.codigo,
            tipoLiberacao: '',
            diaSemana: dia,
            turno: turno,
            horaEntrada: new Date(),
            horaSaida: new Date(),
            bloquear: false,
            criadoEm: new Date(),
            atualizadoEm: new Date()
          });
        });
      });
  
      self._acessoAprovacoesConfigService.AdicionarUsuario(usuario)
        .then(success => {
          success.forEach( (horariosUsuario,horariosUsuarioIndex)=> {
            
            if (!self.usuariosAgrupados[horariosUsuario.usuario]) {
              self.usuariosAgrupados[horariosUsuario.usuario] = {
                usuarioId: horariosUsuario.usuario,
                liberacoesAcessos: []
              };
            }
            self.usuariosAgrupados[horariosUsuario.usuario].liberacoesAcessos.push(horariosUsuario);
          })
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Adicionado com sucesso!`
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
  
    updateUsuario(): void {
      let self = AcessoAprovacoesConfigPageComponent.self;

      if((self.periodoAtual && new Date(self.periodoAtual.inicioPeriodo) > new Date(self.periodoAtual.fimPeriodo)) 
        || (self.periodoFeriasAtual && new Date(self.periodoFeriasAtual.inicioPeriodo) > new Date(self.periodoFeriasAtual.fimPeriodo))) {
        self._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: 'A data de término não pode ser anterior à data de início!'
          }
        });
        return;
      }
  
      const liberacaoArray = this.liberacaoAcessoUsuarioForm.get('liberacaoAcesso') as FormArray;
      var liberacaoAcessoUsuario = liberacaoArray.value;
  
      self._acessoAprovacoesConfigService.AtualizarUsuario(liberacaoAcessoUsuario)
      .then(success => {
        self.closeModal();
        self.closeSubModal();
        self.getPage();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Usuário alterado com sucesso!`
          }
        });
      }, err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Erro ao alterar o Usuário.\n${self.formataErrosApi(err)}`
          }
        });
      });      
  
      if((self.periodoAtual && self.periodoAtual.tipoAusencia != "") ||(self.periodoFeriasAtual && self.periodoFeriasAtual.tipoAusencia != "")){
        var periodosAusencias: PeriodoAusenciaUsuario[] = [];

        if(self.periodoAtual && self.periodoAtual.tipoAusencia != "" && self.periodoAtual.inicioPeriodo != null)
          periodosAusencias.push(self.periodoAtual);

        if(self.periodoFeriasAtual && self.periodoFeriasAtual.tipoAusencia != "" && self.periodoFeriasAtual.inicioPeriodo != null)
          periodosAusencias.push(self.periodoFeriasAtual);

        self._acessoAprovacoesConfigService.AtualizarPeriodoAusenciaUsuario(periodosAusencias)
        .then(success => {
          self.closeModal();
          self.closeSubModal();
          self.getPage();
        }, err => {
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Erro ao alterar o periodo de ${self.periodoAtual.tipoAusencia}.\n${self.formataErrosApi(err)}`
            }
          });
        });
      }
    }
  
    removerUsuario(usuarioCodigo: string) {
      let self = AcessoAprovacoesConfigPageComponent.self;
  
      var temDireito = this._userService.temDireitoAplicativo('WEB7002','E');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de exclusão!`
          }
          
      })
      return;}
  
      self._dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Ao confirmar a exclusão, o usuário não terá mais restrições de acesso às Aprovações` ,
          confirmCallback: async () => {
            await self._acessoAprovacoesConfigService.RemoverUsuario(usuarioCodigo)
            .then(success => {
              if (self.usuariosAgrupados[usuarioCodigo]) {
                delete self.usuariosAgrupados[usuarioCodigo];
              }
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
      });
    }
}
