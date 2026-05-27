import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MatGridList, MatGridListModule } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { AprovacaoComercialHierarquia, AprovacaoComercialHierarquiaCondicao, AprovacaoComercialHierarquiaCondicaoPagamentoItem, AprovacaoComercialHierarquiaUsuario, EAprovacaoComercialHierarquiaValor, Usuario, aprovacaoComercialHierarquiaValores } from 'app/classes/aprovacao-comercial/aprovacao-comercial-hierarquia.classes';
import { AprovacaoComercialTipoPessoa } from 'app/classes/aprovacao-comercial/aprovacao-comercial-tipo-pessoa.classes';
import { AprovacaoComercialUsina, EAprovacaoComercialUsinaFluxoAprovacao } from 'app/classes/aprovacao-comercial/aprovacao-comercial-usina.classes';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Usina } from 'app/classes/usina/usina';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { AprovacaoComercialService } from 'app/services/aprovacao-comercial.service';
import { UsinaService } from 'app/services/usina.service';
import { UserService } from 'app/services/user.service';
import { result, values } from 'lodash';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { CondicaoPagamento } from 'app/classes/pagamento/condicao-pagamento';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { SegmentacaoService } from 'app/services/segmentacao.service';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

export interface EdicaoUsina {
  usina?: Usina;
  situacao: boolean;
  situacaoModel: Situacao;
  fluxoAprovacao: number;
  fluxoAprovacaoModel: FluxoAprovacao;
}

export interface EdicaoHierarquia {
  titulo: string;
  quantidadeAprovacoesNecessarias: number;
  nivelAutoridade: number;
  aprovacaoObrigatoria: boolean;
}

export interface Situacao {
  codigo: number;
  nome: string;
}

export interface FluxoAprovacao {
  codigo: number;
  nome: string;
  descricao: string;
}


@Component({
  selector: 'app-aprovacao-comercial-config-page',
  templateUrl: './aprovacao-comercial-config-page.component.html',
  styleUrls: ['./aprovacao-comercial-config-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class AprovacaoComercialConfigPageComponent implements OnInit {
  public static self: AprovacaoComercialConfigPageComponent;
  itens: PagedList<AprovacaoComercialUsina> = new PagedList<AprovacaoComercialUsina>();

  aprovacaoComercialUsinaForm: FormGroup;
  itemAprovacaoComercial: AprovacaoComercialUsina = new AprovacaoComercialUsina();

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    usina: Usina,
  } = {
    usina: null,
  };

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;
  formataErrosApi = Tasks.formataErrosApi;

  usinas: Usina[] = [];
  situacoes: Situacao[] = [];
  fluxoAprovacoes: FluxoAprovacao[] = []

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  @ViewChild('modalVCR', { read: ViewContainerRef, static: false }) ModalRef: ViewContainerRef;
  @ViewChild('subModalVCR', { read: ViewContainerRef, static: false }) SubModalRef: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _aprovacaoComercialService: AprovacaoComercialService,
    private _usinaService: UsinaService,
    private _userService: UserService,
    private _segmentacaoService: SegmentacaoService
  ) { 
    var temDireito = this._userService.temDireitoAplicativo('WEB7001','', 200);
    if (!temDireito) return;

    AprovacaoComercialConfigPageComponent.self = this;
    this.carregaUsina();
    this.carregaSituacao();
    this.carregaFluxoAprovacao();
  }

  carregaUsina() {
    this._usinaService.listarTodos().then(
      usinas => { this.usinas = usinas; },
      error => { this.usinas = [] }
    );
  }

  carregaSituacao() {
    this.situacoes = []
    this.situacoes.push({codigo: 0, nome: "Desativado"})
    this.situacoes.push({codigo: 1, nome: "Ativo"})
  }

  carregaFluxoAprovacao() {
    this.fluxoAprovacoes = [];
    this.fluxoAprovacoes.push({codigo: 0, nome: "Workflow", descricao: "Workflow no qual deve passar por todos os níveis inferiores até chegar no último nível. Cada nível só pode aprovar após o nível anterior finalizar sua aprovação, indo sequencialmente até o último nível aprovar para que o contrato seja aprovado."});
    this.fluxoAprovacoes.push({codigo: 1, nome: "Último Nível", descricao: "Necessário apenas a aprovação do nível com a faixa de desconto, caso haja mais de um nível que abraanja para aprovação, a aprovação pode ser feita simultaneamente entre os níveis."});
  }

  ngOnInit() {
    this.aprovacaoComercialUsinaForm = this._formBuilder.group({
      usinaString: "",
      ativo: false,
      fluxoAprovacoes: EAprovacaoComercialUsinaFluxoAprovacao.TodosNiveis
    });
    this.aprovacaoComercialHierarquiaForm = this._formBuilder.group({
      descricao: ""
    });
    this.aprovacaoComercialCadHierarquiaForm = this._formBuilder.group({
      titulo: "",
      quantidadeAprovacoesNecessarias: 1,
      nivelAutoridade: 1,
      aprovacaoObrigatoria: false,
    })
    this.aprovacaoComercialCadHierarquiaUsuarioForm = this._formBuilder.group({
      nivelAutoridade: 0
    })
    this.aprovacaoComercialCadHierarquiaCondicaoForm = this._formBuilder.group({
      tipoPessoa: new AprovacaoComercialTipoPessoa(),
      tipoPessoaCopia: new AprovacaoComercialTipoPessoa(),
      field0: 0,
      field1: 0,
      field2: 0,
      field3: 0,
      field4: 0,
      field5: 0,
      field6: 0,
      field7: 0,
      field8: 0,
      field9: 0,

      field10: 0,
      field11: 0,
      field12: 0,
      field13: 0,
      field14: 0,
      field15: 0,
      field16: 0,
      field17: 0,
      field18: 0,
      field19: 0,

      field20: 0,
      field21: 0,
      field22: 0,
      field23: 0,
      field24: 0,
      field25: 0,
      field26: 0,
      field27: 0,
      field28: 0,
      field29: 0,

      field30: 0,
      field31: 0,
      field32: 0,
      field33: 0,
      field34: 0,
      field35: 0,
      field36: 0,
      field37: 0,
      field38: 0,
      field39: 0
    });
    this.aprovacaoComercialCadHierarquiaCondicaoPagamentoForm = this._formBuilder.group({
      tipoPessoa: new AprovacaoComercialTipoPessoa(),
      tipoPessoaCopia: new AprovacaoComercialTipoPessoa(),
    });
  }

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;

  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
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

  get dataSource(): AprovacaoComercialUsina[] {
    return this.itens.records;
  };

  getPage(pageInfo?) {

    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };

    this._aprovacaoComercialService.ListarAguardandoCienciaPorPagina(currentPage, pageSize, this.filtroString)
    .then(
      AprovacaoComercialUsinas => {
        this.itens = AprovacaoComercialUsinas;
        this.paginaAtual = AprovacaoComercialUsinas.currentPage;
        this.registrosPorPagina = AprovacaoComercialUsinas.pageSize;
      },
      error => { this.itens = new PagedList<AprovacaoComercialUsina>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  // -------------------- Formatters ------------------------------
  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';
  ativoFormatter = (model: boolean): string => model ? "Ativo" : "Desativado";
  hierarquiasFormatter = (model: AprovacaoComercialHierarquia[]): string => model ? model.length.toString() : "0";
  situacaoFormatter = (model: Situacao): string => model ? (model.nome).toUpperCase() : '';
  fluxoAprovacaoFormatter = (model: FluxoAprovacao): string => model ? (model.nome).toUpperCase() : '';
  fluxoAprovacaoFormatterNumber = (model: number): string => this.fluxoAprovacoes[model].nome;
  segmentacaoFormatter = (model: Segmentacao): string => model ? (model.nome).toUpperCase() : '';
  // --------------------------------------------------------------

  _allColumns: TableColumn[] = [
    { name: 'usina', placeholder: 'Central', order: 1, priority: 1, formatter: this.usinaFormatter},
    { name: 'ativo', placeholder: 'Situação', order: 2, priority: 2, formatter: this.ativoFormatter},
    { name: 'hierarquias', placeholder: 'Níveis Aprovação', order: 3, priority: 3, formatter: this.hierarquiasFormatter},
    { name: 'fluxoAprovacao', placeholder: 'Fluxo Aprovação', order: 4, priority : 4, formatter: this.fluxoAprovacaoFormatterNumber }
  ];

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: AprovacaoComercialUsina | null;

  get fixedColumnsLeft(): string[] {
    return [];
  }
  get fixedColumnsRight(): string[] {
    return ['expand','edit', 'hierarquia'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get columns(): TableColumn[] {
    return this.allColumns.filter(t => !this.hiddenColumns.includes(t.name));
  }

  get columnNames(): string[] {
    return this.fixedColumnsLeft.concat(this.displayedColumns.map(t => t.name)).concat(this.fixedColumnsRight);
  }

  get foldedColumns(): TableColumn[] {
    var self = AprovacaoComercialConfigPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => !this.columnNames.includes(t.name));
  }

  hiddenColumns: string[] = [];
  isHiddenColumn(columnName: string): boolean {
    return this.hiddenColumns.includes(columnName);
  }

  get currentViewValue() {
    return { filter: this.filtro, hiddenColumns: this.hiddenColumns, customColumnOrder: this._customColumnOrder }
  }

  getFormattedValue(element: AprovacaoComercialUsina, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }
  
  setHiddenColumn(columnName: string, hidden: boolean) {
    if (hidden)
      this.hiddenColumns.push(columnName);
    else
      this.hiddenColumns = this.hiddenColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }

  get displayedColumns(): TableColumn[] {
    var self = AprovacaoComercialConfigPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => {
      var fixedColsTotalWidth = 235;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 180);
      var hiddenColumnsHighPriority = this.allColumns.filter(c => this.hiddenColumns.includes(c.name) && this.getPriority(c) < this.getPriority(t)).length;

      return (this.getPriority(t) - hiddenColumnsHighPriority) <= colsAllowed;
    });
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

  ngAfterViewInit(): void {

  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  // --------------- Modal ---------------------------------------------------------------------------------------

  confirmModal: Function;
  cancelModal: Function;

  confirmSubModal: Function;
  cancelSubModal: Function;

  showModal(content, confirmCallback: Function, cancelCallback?: Function) {

    let self = AprovacaoComercialConfigPageComponent.self;

    self.confirmModal = confirmCallback;
    self.cancelModal = cancelCallback || self.closeModal;

    self.openModal(content);

    self.modalIsOpen = true;

  }

  showSubModal(content, confirmCallback: Function, cancelCallback?: Function) {

    let self = AprovacaoComercialConfigPageComponent.self;

    self.confirmSubModal = confirmCallback;
    self.cancelSubModal = cancelCallback || self.closeModal;

    self.openSubModal(content);

  }

  openModal(content) {
    this._subDialogRef = this._dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
  }

  openSubModal(content) {
    this._subDialogRef = this._dialog.open(content, { viewContainerRef: this.SubModalRef });
    this.subModalIsOpen = true;
  }

  closeModal() {
    let self = AprovacaoComercialConfigPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.closeSubModal();

    self._dialog.closeAll();

    self.modalIsOpen = false;
  }

  closeSubModal() {
    let self = AprovacaoComercialConfigPageComponent.self

    if (self._subDialogRef) self._subDialogRef.close();

    self.subModalIsOpen = false;
  }

  // -------------- Modal - Cadastro de Usina -----------------------------------

  edicaoUsina: AprovacaoComercialUsina = new AprovacaoComercialUsina();
  edicaoUsinaValores: EdicaoUsina
  edicaoUsinaNovo: boolean = false;

  editaUsinaModal(content, usina?: AprovacaoComercialUsina) {

    this.edicaoUsina = usina ? usina : new AprovacaoComercialUsina();
    this.edicaoUsinaNovo = usina ? false : true;

    this.edicaoUsinaValores = {
      usina: usina ? this.edicaoUsina.usina : null,
      situacao: this.edicaoUsina.ativo,
      situacaoModel: this.edicaoUsina.ativo ? {codigo: 1, nome:"Ativo"} : {codigo: 0, nome:"Desativado"},
      fluxoAprovacao: this.edicaoUsina.fluxoAprovacao,
      fluxoAprovacaoModel: this.edicaoUsina.fluxoAprovacao ? this.fluxoAprovacoes[this.edicaoUsina.fluxoAprovacao] : this.fluxoAprovacoes[0]
    }

    this.showModal(content, this.salvarUsina, this.cancelaUsina);

  }

  salvarUsina() {
    let self = AprovacaoComercialConfigPageComponent.self;
    
    self.edicaoUsina.ativo = this.edicaoUsinaValores.situacaoModel.codigo === 1 ? true : false;
    self.edicaoUsina.fluxoAprovacao = this.edicaoUsinaValores.fluxoAprovacaoModel.codigo;
    self.edicaoUsina.usinaId = this.edicaoUsinaValores.usina.codigo;

    if(self.edicaoUsinaNovo) {
      self._aprovacaoComercialService.AdicionarUsina(self.edicaoUsina)
      .then(success => {
        self.closeModal();
        self.getPage();
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
    } else {
      self._aprovacaoComercialService.AtualizarUsina(self.edicaoUsina)
      .then(success => {
        self.closeModal();
        self.getPage();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Atualizada com sucesso!`
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

  cancelaUsina() {
    this.closeModal();
  }


  // -------------- Modal - Hierarquia -------------------------------------

  aprovacaoComercialHierarquiaForm: FormGroup;
  hierarquiaUsinaSelecionada: AprovacaoComercialUsina;
  hierarquias: AprovacaoComercialHierarquia[] = [];
  estaComErro: boolean = false;

  editHierarquiaModal(content, usina: AprovacaoComercialUsina) {
    let self = AprovacaoComercialConfigPageComponent.self;

    self.estaComErro = false;
    self.hierarquiaUsinaSelecionada = usina;

    this._aprovacaoComercialService.ListarHierarquiaPorUsina(usina)
    .then(
      hierarquias => { this.hierarquias = hierarquias; },
      error => { 
        self.estaComErro = true;
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${self.formataErrosApi(error)}`
          }
        });
       }
    )

    this.openModal(content);

  }

  carregaHierarquiaPorUsina(usina: AprovacaoComercialUsina) {
    this._aprovacaoComercialService.ListarHierarquiaPorUsina(usina)
    .then(
      hierarquias => { this.hierarquias = hierarquias; },
      error => { this.usinas = [] }
    )
  }
  
  // -------------- Modal - Cadastro Hierarquia -------------------------------------

  aprovacaoComercialCadHierarquiaForm: FormGroup;

  cadHierarquia?: AprovacaoComercialHierarquia;
  cadHierarquiaNovo: boolean = false;
  cadHierarquiaValores: EdicaoHierarquia;
  cadHierarquiaErro: boolean = false;

  cadHierarquiaAprovacaoNecessarias: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

  openCadastroHierarquia(content, hierarquia?: AprovacaoComercialHierarquia) {
    
    let self = AprovacaoComercialConfigPageComponent.self;

    self.cadHierarquia = hierarquia ? hierarquia : new AprovacaoComercialHierarquia();
    self.cadHierarquiaNovo = hierarquia ? false : true;

    self.cadHierarquiaErro = false;

    if(self.cadHierarquiaNovo) {
      self.cadHierarquia.aprovacaoComercialUsinaId = self.hierarquiaUsinaSelecionada.id;
      self.cadHierarquia.quantidadeAprovacoesNecessarias = 1;
      
      self._aprovacaoComercialService.ObterProximoNivelAutoridadePorAprovacaoComercial(self.hierarquiaUsinaSelecionada, false)
      .then(
        nivelAutoridade => { 
          self.cadHierarquia.nivelAutoridade = nivelAutoridade;
          if(self.cadHierarquiaValores)
            self.cadHierarquiaValores.nivelAutoridade = nivelAutoridade;
        },
        err => { self.cadHierarquiaErro = true; }
      );

    }

    self.cadHierarquiaValores = {
      titulo: self.cadHierarquia.titulo,
      quantidadeAprovacoesNecessarias: self.cadHierarquia.quantidadeAprovacoesNecessarias,
      nivelAutoridade: self.cadHierarquia.nivelAutoridade,
      aprovacaoObrigatoria: self.cadHierarquia.aprovacaoObrigatoria
    }

    self.openSubModal(content);

  }

  salvarCadastroHierarquia() {

    let self = AprovacaoComercialConfigPageComponent.self;

    self.cadHierarquia.titulo = self.cadHierarquiaValores.titulo;
    self.cadHierarquia.quantidadeAprovacoesNecessarias = self.cadHierarquiaValores.quantidadeAprovacoesNecessarias;
    self.cadHierarquia.aprovacaoObrigatoria = self.cadHierarquiaValores.aprovacaoObrigatoria;

    if(self.cadHierarquiaNovo) {
      self._aprovacaoComercialService.AdicionarHierarquia(self.cadHierarquia)
      .then(success => {
        self.closeSubModal();
        self.hierarquias.unshift(success);
        self.hierarquiaUsinaSelecionada.hierarquias.unshift(success);
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
    } else {
      self._aprovacaoComercialService.AtualizarHierarquia(self.cadHierarquia)
      .then(success => {
        self.closeSubModal();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Atualizada com sucesso!`
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

  // -------------- Modal - Cadastro Usuarios -------------------------------------

  aprovacaoComercialCadHierarquiaUsuarioForm: FormGroup;

  cadHierarquiaSelecionado: AprovacaoComercialHierarquia;
  cadHierarquiaUsuarios: AprovacaoComercialHierarquiaUsuario[];
  cadHierarquiaUsuariosDisponiveis?: Usuario[];

  cadHierarquiaUsuarioSelecionado: Usuario = new Usuario();

  usuarioFormatter = (model: Usuario): string => model ? model.nome.toUpperCase() : '';
  condicaoPagamentoFormatter = (model: CondicaoPagamento): string => model ? `${model.codigo} - ${model.descricao}` : '';

  openModalUsuario(content, hierarquia: AprovacaoComercialHierarquia) {

    let self = AprovacaoComercialConfigPageComponent.self;

    self.cadHierarquiaSelecionado = hierarquia;
    
    self._aprovacaoComercialService.ListarUsuariosPorHierarquia(self.cadHierarquiaSelecionado)
    .then(
      usuarios => { self.cadHierarquiaUsuarios = usuarios },
      err => { self.cadHierarquiaUsuarios = [] }
    )

    self._aprovacaoComercialService.ListarUsuariosDisponiveis()
    .then(
      usuarios => { self.cadHierarquiaUsuariosDisponiveis = usuarios },
      err => { self.cadHierarquiaUsuariosDisponiveis = [] }
    )

    this.openSubModal(content);


  }

  adicionarUsuario() {

    let self = AprovacaoComercialConfigPageComponent.self;

    if(self.cadHierarquiaUsuarioSelecionado == null) {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `${self.formataErrosApi("Favor selecionar um usuário.")}`
        }
      });
    }

    var usuario = new AprovacaoComercialHierarquiaUsuario();

    usuario.usuarioId = self.cadHierarquiaUsuarioSelecionado.id;
    usuario.aprovacaoComercialHierarquiaId = self.cadHierarquiaSelecionado.id;

    self._aprovacaoComercialService.AdicionarUsuario(usuario)
      .then(success => {
        self.cadHierarquiaUsuarios.unshift(success);
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

  desfazerRemoverUsuario(usuarioOld: AprovacaoComercialHierarquiaUsuario) {
    
    let self = AprovacaoComercialConfigPageComponent.self;

    var usuario = new AprovacaoComercialHierarquiaUsuario();
    usuario.usuarioId = usuarioOld.usuarioId.replace(' - REMOVIDO', '');
    usuario.aprovacaoComercialHierarquiaId = usuarioOld.aprovacaoComercialHierarquiaId;

    self._aprovacaoComercialService.AdicionarUsuario(usuario)
      .then(success => {
        usuarioOld.id = success.id;
        usuarioOld.usuarioId = success.usuarioId;
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

  removerUsuario(usuario: AprovacaoComercialHierarquiaUsuario) {

    let self = AprovacaoComercialConfigPageComponent.self;
    self._aprovacaoComercialService.RemoverUsuario(usuario)
      .then(success => {
        usuario.id = '';
        usuario.usuarioId = usuario.usuarioId + ' - REMOVIDO';
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

  // -------------- Modal - Cadastro Condições -------------------------------------

  aprovacaoComercialCadHierarquiaCondicaoForm: FormGroup;

  cadTipoPessoa: AprovacaoComercialTipoPessoa[] = [];
  cadTipoValor = aprovacaoComercialHierarquiaValores;

  cadCondicaoHierarquia: AprovacaoComercialHierarquia = new AprovacaoComercialHierarquia();

  modalError: boolean = false;

  cadTipoPessoaSelecionado: AprovacaoComercialTipoPessoa = new AprovacaoComercialTipoPessoa();
  cadTipoPessoaCopiaSelecionado?: AprovacaoComercialTipoPessoa;

  cadTipoValorSelecionado: any = this.cadTipoValor[0];

  cadCondicoes: AprovacaoComercialHierarquiaCondicao[] = [];

  cadCopiaIsVisible: boolean = false;

  tipoPessoaFormatter = (model: AprovacaoComercialTipoPessoa): string => model ? model.descricao.toUpperCase() : '';
  tipoValorFormatter = (model: any): string => model ? model.desc.toUpperCase() : '';

  async openModalCondicao(content, hierarquia: AprovacaoComercialHierarquia) {

    let self = AprovacaoComercialConfigPageComponent.self;

    self.modalError = false;
    self.cadTipoPessoaCopiaSelecionado = undefined;

    self.cadCondicaoHierarquia = hierarquia;

    await self._aprovacaoComercialService.ListarTipoPessoa()
    .then(
      success => { self.cadTipoPessoa = success; },
      err => { 
        self.cadTipoPessoa = []; self.modalError = true; }
    )
    
    if(self.cadTipoPessoa.length > 0)
      self.cadTipoPessoaSelecionado = this.cadTipoPessoa[0];

    await this.getCondicao();

    this.openSubModal(content);

  }

  async getCondicao() {

    let self = AprovacaoComercialConfigPageComponent.self;

    await self._aprovacaoComercialService.ListarCondicaoPorHierarquaTipoPessoa(self.cadCondicaoHierarquia, self.cadTipoPessoaSelecionado)
    .then(
      success => { self.cadCondicoes = success },
      err => { 
        self.cadCondicoes = []; 
        self.cadTipoPessoa = []; 
        self.closeSubModal();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${self.formataErrosApi(err)}`
          }
        });
       }
      );
    
    aprovacaoComercialHierarquiaValores.forEach((tipoValor) => {
      var founded = self.cadCondicoes.find((cond) => {return cond.tipoValor === tipoValor.codigo});

      if(!founded) {

        var newCond = new AprovacaoComercialHierarquiaCondicao();

        newCond.tipoPessoaId = self.cadTipoPessoaSelecionado.id;
        newCond.tipoPessoa = self.cadTipoPessoaSelecionado;;
        newCond.aprovacaoComercialHierarquiaId = self.cadCondicaoHierarquia.id;
        newCond.tipoValor = tipoValor.codigo;
        newCond.valor = tipoValor.valor;

        self.cadCondicoes.push(newCond);

      }
    });

    self.cadCondicoes.sort((a, b) => (a.tipoValor < b.tipoValor ? -1 : 1))
  }

  cadTipoPessoaAnterior?: AprovacaoComercialTipoPessoa;

  async changeTipoPessoa(out: string) {
    
    let self = AprovacaoComercialConfigPageComponent.self;

    if(self.cadTipoPessoaAnterior) {
      if(self.cadTipoPessoaAnterior.id == self.cadTipoPessoaSelecionado.id)
        return;
    }

    self.cadTipoPessoaAnterior = self.cadTipoPessoaSelecionado;

    self.getCondicao();

  }


  getTipoValorString(condicao: AprovacaoComercialHierarquiaCondicao) {

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.ValorVendaTracos)
      return 'Desconto Sobre Valor de Venda Traços';

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.ValorVendaBomba)
      return 'Desconto Sobre Valor de Venda Bomba';
    
    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.MargemMCC)
      return 'Margem Pós MCC e impostos';

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.MargemTransporte)
      return 'Margem Pós Transporte';

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.Ebtida)
      return 'Valor Ebtida';

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.Volume)
      return 'Volume M3 do Contrato';

    return condicao.valor;

  }

  getTipoUnidadeCondicao(condicao: AprovacaoComercialHierarquiaCondicao) {

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.Volume) {
      return 'M3';
    } else {
      return 'R$';
    }

  }

  exibirCampoPercentual(condicao: AprovacaoComercialHierarquiaCondicao) {

    if(condicao.tipoValor === EAprovacaoComercialHierarquiaValor.ValorVendaTracos || 
      condicao.tipoValor === EAprovacaoComercialHierarquiaValor.ValorVendaBomba)
      return true;

    return false;

  }

  async salvarCondicao() {

    let self = AprovacaoComercialConfigPageComponent.self;

    await self._aprovacaoComercialService.SalvarCondicoes(self.cadCondicoes)
    .then(
      success => {
        self.getCondicao();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Atualizado/Adicionado(s) com sucesso!`
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

  // -------------- Modal - Cadastro Condições Pagamento -------------------------------------

  aprovacaoComercialCadHierarquiaCondicaoPagamentoForm: FormGroup;

  cadDemaisTipoPessoa: AprovacaoComercialTipoPessoa[] = [];
  cadDemaisTipoPessoaSelecionado: AprovacaoComercialTipoPessoa = new AprovacaoComercialTipoPessoa();

  cadDemaisSegmentacao: Segmentacao[] = [];
  cadDemaisSegmentacaoSelecionado: Segmentacao;

  cadCondicoesPagamentoSelecionada: AprovacaoComercialHierarquiaCondicaoPagamentoItem = new AprovacaoComercialHierarquiaCondicaoPagamentoItem();


  async openModalCondicaoPagamento(content, hierarquia: AprovacaoComercialHierarquia) {

    let self = AprovacaoComercialConfigPageComponent.self;

    self.modalError = false;
    self.cadDemaisTipoPessoaSelecionado = undefined;

    self.cadCondicaoHierarquia = hierarquia;

    await self._aprovacaoComercialService.ListarTipoPessoa()
    .then(
      success => { self.cadDemaisTipoPessoa = success; },
      err => { self.cadDemaisTipoPessoa = []; self.modalError = true; }
    )

    await self._segmentacaoService.listarTodos()
    .then(
      success => { self.cadDemaisSegmentacao = success; },
      err => {  self.cadDemaisSegmentacao = []; self.modalError = true; }
    )

    self.cadDemaisTipoPessoa = self.cadDemaisTipoPessoa.filter(x => x.descricao.toUpperCase().includes('PESSOA'));
    
    if(self.cadDemaisTipoPessoa.length > 0)
      self.cadDemaisTipoPessoaSelecionado = this.cadDemaisTipoPessoa[0];

    if(self.cadDemaisSegmentacao.length > 0)
      self.cadDemaisSegmentacaoSelecionado = this.cadDemaisSegmentacao[0];

    await self.getCondicaoPagamento();

    this.openSubModal(content);

  }

  async changeDemaisTipoPessoa(out: string) {
    
    let self = AprovacaoComercialConfigPageComponent.self;

    await self.getCondicaoPagamento();

  }

  async changeDemaisSegmentacao(out: string) {
    
    let self = AprovacaoComercialConfigPageComponent.self;

    await self.getCondicaoPagamento();

  }

  async getCondicaoPagamento() {

    let self = AprovacaoComercialConfigPageComponent.self;

    self._aprovacaoComercialService.ObterCondicaoPagamento(self.cadCondicaoHierarquia, self.cadDemaisTipoPessoaSelecionado, self.cadDemaisSegmentacaoSelecionado)
    .then(
      success => { 
        if(success) {
          self.cadCondicoesPagamentoSelecionada = success; 
        } else {
          self.cadCondicoesPagamentoSelecionada = new AprovacaoComercialHierarquiaCondicaoPagamentoItem();
          self.cadCondicoesPagamentoSelecionada.aprovacaoComercialHierarquiaId = self.cadCondicaoHierarquia.id;
          self.cadCondicoesPagamentoSelecionada.segmentacaoId = self.cadDemaisSegmentacaoSelecionado.id;
          self.cadCondicoesPagamentoSelecionada.tipoPessoaId = self.cadDemaisTipoPessoaSelecionado.id;
        }
      },
      err => {  
        self.cadCondicoesPagamentoSelecionada = new AprovacaoComercialHierarquiaCondicaoPagamentoItem();
        self.cadCondicoesPagamentoSelecionada.aprovacaoComercialHierarquiaId = self.cadCondicaoHierarquia.id;
        self.cadCondicoesPagamentoSelecionada.segmentacaoId = self.cadDemaisSegmentacaoSelecionado.id;
        self.cadCondicoesPagamentoSelecionada.tipoPessoaId = self.cadDemaisTipoPessoaSelecionado.id;
      }
    )

  }

  async salvarCondicaoPagamento() {

    let self = AprovacaoComercialConfigPageComponent.self;
    
    self._aprovacaoComercialService.AtualizarCondicaoPagamento([ self.cadCondicoesPagamentoSelecionada ], false)
    .then(
      success => {
        self.getCondicaoPagamento();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Atualizado com sucesso!`
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
