import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { trigger, state, transition, style, animate } from '@angular/animations';
import { PagedList } from 'app/classes/pagination/paged-list';
import { ObraProjecao } from 'app/classes/obra/obra-projecao';
import { Tasks } from 'app/classes/_tasks/tasks';
import { MatDialog, MatDialogRef } from '@angular/material';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { ObraProjecaoService } from 'app/services/obra-projecao.service';
import { Obra, statuProjecao } from 'app/classes/obra/obra';
import { Usina } from 'app/classes/usina/usina';
import { Contrato } from 'app/classes/contrato/contrato';
import { Placeholder } from '@angular/compiler/src/i18n/i18n_ast';
import { Status } from 'app/classes/proposta/proposta';
import { ObraService } from 'app/services/obra.service';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { UserService } from 'app/services/user.service';
import { ICustomValidator } from 'app/main/components/interfaces/custom-validator';
import { UsinaService } from 'app/services/usina.service';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
  htmlValue?: boolean;
  align?: string;
}

export interface EdicaoObraProjecao {  
  periodo: Date;
  volume: number;
}

@Component({
  selector: 'app-carteira-page',
  templateUrl: './carteira-page.component.html',
  styleUrls: ['./carteira-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class CarteiraPageComponent implements OnInit {
  public static self: CarteiraPageComponent;

  //#region Data/Paginação

  itens: PagedList<Obra> = new PagedList<Obra>();
  item: Obra = new Obra();

  itemTracoString: string = "";

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  get dataSource(): Obra[] {
    return this.itens.records;
  };

  getPage(pageInfo?) {

    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };

    this._obraProjecaoService.ListarPorPagina(currentPage, pageSize, this.filtroString)
    .then(pagedResponse => {
      this.itens = pagedResponse;
      this.paginaAtual = pagedResponse.currentPage;
      this.registrosPorPagina = pagedResponse.pageSize;
    });

  }

  //#endregion
  //#region Modal Projeção Carteira

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  temDireitoAlteracaoProjecaoCarteiraMesAtual: boolean = false;
  
  obraProjecoes: ObraProjecao[] = [];
  obraProjecaoConsumo: number;
  obraProjecaoVolume: number;
  obraProjecaoSaldo: number;
  obraProjecaoPrevisaoSaldo: number;
  obraProjecaoProximoPeriodo: Date;

  displayedColumnsObraProjecao: string[] = ['projecao-volume','projecao-periodo','projecao-saldo', 'projecao-edicao'];

  saldoProjecao : number;

  obraProjecaoForm: FormGroup;
  
  cadObraProjecao: EdicaoObraProjecao;
  cadObraProjecaoNovo: boolean;
  cadObraProjecaoSelecionado: ObraProjecao;
  cadObraProjecaoVolumeAnterior: number;
  cadObraProjecaoPeriodoAnterior: Date = new Date();

  get volumeProjecaoValidator(): ICustomValidator {
    var message = 'Volume projetado maior que o saldo disponível!';
      return {
        
        key: 'volumeInvalido',
        message: message,
        validatorFunction: (volumeContratado: number, volumeConsumido: number, volumeDigitado: number, volumePrevisao: number): boolean => {
          if (this.cadObraProjecaoNovo) {
            return (volumeContratado - (volumeConsumido + volumeDigitado + volumePrevisao)) < 0;
          }
          else{
            return (volumeContratado - (volumeConsumido  + (volumePrevisao - volumeDigitado) + this.cadObraProjecao.volume)) < 0;
          }
  
        },
        params: [this.getVolumeTotal(), this.getVolumeConsumido(), this.cadObraProjecaoSelecionado.volume, this.getPrevisaoSaldoProjecao()]
      }
  }

  getSegmento(): string {
    return '';
  }
  
  getVolumeTotal(): number {
    return this.obraProjecaoVolume;
  }
  
  getSaldoTotal(): number {
    this.saldoProjecao = this.getVolumeTotal() - this.getVolumeConsumido()
    return this.getVolumeTotal() - this.getVolumeConsumido();
  }
  
  getSaldoProjecaoLista(volume : number): number {
    return this.saldoProjecao = this.saldoProjecao - volume;
  }
  
  getVolumeConsumido(): number {   
    return this.obraProjecaoConsumo;
  }
  
  getSaldoProjecao(): number {   
    return this.obraProjecaoSaldo;
  }
  
  getPrevisaoSaldoProjecao(): number {   
    return this.obraProjecaoPrevisaoSaldo;
  }

  isDataDesabilitada(periodo: Date): boolean {
    const dataAtual = new Date();
    const dataPeriodo = new Date(periodo);
  
    return dataPeriodo <= dataAtual && this.temDireitoAlteracaoProjecaoCarteiraMesAtual === false;
  };

  get obraProjecoesAtual(): ObraProjecao[] {;
    const dataAtual = new Date();
    
    dataAtual.setDate(1);
    dataAtual.setHours(0, 0, 0, 0);

    return this.obraProjecoes.filter(obra => {
      const dataPeriodo = new Date(obra.periodo);
      return dataPeriodo >= dataAtual;
    });
  }

  async showModal(content, container: ViewContainerRef, obra: Obra) {
    let minWidthContainer = this.isSmallScreen ? "95%" : "";        
    this.item = obra;
  
  
    if (container == this.projecaoObraModalRef){
      this.obraProjecoes = obra.obraProjecao;            
      this.obraProjecaoConsumo = await this._obraService.obterConsumoPorContrato(obra);      
      this.obraProjecaoVolume = await this._obraService.obterVolumePorContrato(obra);
      this.obraProjecaoSaldo = await this._obraProjecaoService.obterSaldoObraProjecao(obra);
      this.obraProjecaoPrevisaoSaldo = await this._obraProjecaoService.obterPrevisaoSaldoObraProjecao(obra);      
      this.obraProjecaoProximoPeriodo = await this._obraProjecaoService.getProximoPeriodoPorContrato(obra);
    
      this._cdr.detectChanges();       
      
    }
  
    this._dialogRef = this._dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer});  
    
    this.modalIsOpen = true;
  }
  
  openModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
  }

  closeModal() {
    let self = CarteiraPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;

  }

  closeSubModal() {
    let self = CarteiraPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  cancelSubModal() {
    let self = CarteiraPageComponent.self;

    self.closeSubModal();
  }

  confirmModal: Function;
  cancelModal: Function;
  showProjecaoModal(content, confirmCallback: Function, cancelCallback?: Function) {

    let self = CarteiraPageComponent.self;

    self.confirmModal = confirmCallback;
    self.cancelModal = cancelCallback || self.openProjecaoModal;

    self.openProjecaoModal(content);

    self.modalIsOpen = true;
  }
  
  openProjecaoModal(content) {

    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
    
  }
  

  closeProjecaoModalGeneric() {
    let self = CarteiraPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self._dialog.closeAll();

    self.modalIsOpen = false;
  }
  
  showModalProjecaoObra(content, container: ViewContainerRef, obraProjecao: ObraProjecao = undefined) {
    let self = CarteiraPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    if(obraProjecao) {

      self.cadObraProjecao = {
        volume: obraProjecao.volume,
        periodo: obraProjecao.periodo,
      };

      self.cadObraProjecaoNovo = false;
      self.cadObraProjecaoSelecionado = obraProjecao;
      self.cadObraProjecaoSelecionado.periodo = obraProjecao.periodo;
      self.cadObraProjecaoSelecionado.volumeAnterior = self.cadObraProjecaoSelecionado.volume;
      self.cadObraProjecaoSelecionado.periodoAnterior = self.cadObraProjecaoSelecionado.periodo;
    } else {
      self.cadObraProjecaoSelecionado = new ObraProjecao();
      self.cadObraProjecaoSelecionado.periodo = self.obraProjecaoProximoPeriodo;
      self.cadObraProjecao = self.cadObraProjecaoSelecionado;
      self.cadObraProjecaoNovo = true;
    }

    self._subDialogRef = self._dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  
  }
  
  obraProjecaoModalSalvar() {
    
      const dataAtual = new Date();
      const dataPeriodo = new Date(this.cadObraProjecaoSelecionado.periodo);
      const anoAtual = dataAtual.getFullYear();
      const mesAtual = dataAtual.getMonth()+1;
      const anoPeriodo = dataPeriodo.getFullYear();
      const mesPeriodo = dataPeriodo.getMonth()+1;

      const jaExistePeriodo = this.obraProjecoes.some(item => {
        const itemPeriodo = item.periodo instanceof Date ? item.periodo : new Date(item.periodo);
      
        const cadPeriodo = this.cadObraProjecaoSelecionado.periodo instanceof Date 
          ? this.cadObraProjecaoSelecionado.periodo 
          : new Date(this.cadObraProjecaoSelecionado.periodo);
      
        return itemPeriodo.getTime() === cadPeriodo.getTime();
      });

      if (jaExistePeriodo && this.cadObraProjecaoNovo) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Período já cadastrado!`
          }
        });
        return;
      } 
      
      if (anoPeriodo < anoAtual || (anoAtual === anoPeriodo && mesPeriodo < mesAtual)) {
        this._dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: `Período não pode ser de meses anteriores`
            }
          });
          return;
      }

      this.cadObraProjecaoModalSalvar();
      
    }  

  closeProjecaoModal() {
    if (this.getPrevisaoSaldoProjecao() > 0 && this.getSaldoTotal() > this.getPrevisaoSaldoProjecao()){
    this._dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      data: {
        title: 'TopConWeb',
        message: `Previsão total da carteira M³ menor que Saldo total.
          Confirma projeção?`,
        confirmCallback: async () => {
          this.closeModal();
        }
      }
    });}
    else{
      this.closeModal();
    }

  }


  cadObraProjecaoModalSalvar() { 
    this.cadObraProjecaoSelecionado.periodo = this.cadObraProjecao.periodo;
    this.cadObraProjecaoSelecionado.volume = this.cadObraProjecao.volume;
    this.cadObraProjecaoSelecionado.usina = this.item.usinaCodigo;
    this.cadObraProjecaoSelecionado.noObra = this.item.numero;
    this.cadObraProjecaoSelecionado.noChamada = this.item.numChamada;
    this.cadObraProjecaoSelecionado.anoChamada = this.item.anoChamada;
    
    let self = CarteiraPageComponent.self;
  
    if(this.cadObraProjecaoNovo) {

      self._obraProjecaoService.Adicionar(this.cadObraProjecaoSelecionado)
      .then(async success => {
        self.closeSubModal();
        self.getPage();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Projeção da Carteira adicionada com sucesso!`
          }
        });
        await this.AtualizaProjecao();
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
      else{
      self._obraProjecaoService.Atualizar(this.cadObraProjecaoSelecionado)
      .then(async success => {
        self.closeSubModal();
        self.getPage();
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Projeção alterada com sucesso!`
          }
        });
        await this.AtualizaProjecao();
      }, err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Erro ao alterar a Projeção.\n${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
      }     
    }
  
  showModalProjecao(o: Obra) {
    this._obraService.ListarObraProjecao(o.usinaCodigo, o.numero, o.anoChamada, o.numChamada)
      .then(
        projecaoObra => {
            if (projecaoObra) {
                this.cadObraProjecaoNovo = false;
            } else {
                this.cadObraProjecaoSelecionado = new ObraProjecao();
                this.cadObraProjecao = this.cadObraProjecaoSelecionado;
                this.cadObraProjecaoNovo = true;
            };
            
            this.openModal(projecaoObra); 
            this.modalIsOpen = true;
        },
        error => {
            this._dialog.open(AlertDialogComponent, {
                data: {
                    title: 'Erro',
                    message: error
                }
            });
        }
      );
  }

  async AtualizaProjecao() {  
    this.obraProjecoes = await this._obraService.ListarObraProjecao(this.item.usinaCodigo, this.item.numero, this.item.anoChamada, this.item.numChamada);                
    this.obraProjecaoSaldo = await this._obraProjecaoService.obterSaldoObraProjecao(this.item);
    this.obraProjecaoPrevisaoSaldo = await this._obraProjecaoService.obterPrevisaoSaldoObraProjecao(this.item);      
    this.obraProjecaoProximoPeriodo = await this._obraProjecaoService.getProximoPeriodoPorContrato(this.item);
    
    this._cdr.detectChanges();  
  
  }

  //#endregion
  //#region  Filtros

  filtroString: string = '';

  filtro: {
    usina: number,
    anoContrato: number,
    numeroContrato: number,
  } = {
    usina: 0,
    anoContrato: 0,
    numeroContrato: 0,
  };

  setFilter(newFilter) {
    this.filtro = newFilter;
    Object.keys(newFilter).forEach(t => this.filtro[t] = newFilter[t]);
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
  }

  //#endregion
  //#region Formatter

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;
  formataErrosApi = Tasks.formataErrosApi;

  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';
  contratoFormatter = (model: Contrato): string => model ? (model.numero + '-' + model.ano).toUpperCase() : '';

  usinaEntregaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.usinasCodigos.includes(model) ? model + ' - ' +  this.usinas.filter(e => e.codigo===model)[0].nome.toUpperCase() : '';
  };

  getMesProjecaoValue(model: Obra, mesesAdicionais: number = 0): string {
    let self = CarteiraPageComponent.self;

    if (model.obraProjecao.length === 0)
        return 'N/A';

    const alvo = new Date();
    alvo.setMonth(alvo.getMonth() + mesesAdicionais);

    const mesAlvo = alvo.getMonth();
    const anoAlvo = alvo.getFullYear();

    var obraProjecao = model.obraProjecao.find(proj => {
        const d = new Date(proj.periodo);
        return d.getMonth() === mesAlvo && d.getFullYear() === anoAlvo;
    });

    if (!obraProjecao)
        return 'N/A';

    return `${self.formataValor(obraProjecao.volume, 1, false)} M3`;
  }

  getMesAtualValue(model: Obra): string {
    let self = CarteiraPageComponent.self;
    return self.getMesProjecaoValue(model, 0);
  }

  getSegundoMesValue(model: Obra): string {
    let self = CarteiraPageComponent.self;
    return self.getMesProjecaoValue(model, 1);
  }

  getTerceiroMesValue(model: Obra): string {
    let self = CarteiraPageComponent.self;
    return self.getMesProjecaoValue(model, 2);
  }

  getMesPlaceholder(model: Date): string {
    const nome = model.toLocaleString('pt-BR', { month: 'long' });
    return nome.charAt(0).toUpperCase() + nome.slice(1);
  }

  getMesAtualPlaceholder(): string {
    const alvo = new Date();
    alvo.setMonth(alvo.getMonth() + 0);
    return this.getMesPlaceholder(alvo); 
  }

  getSegundoMesPlaceholder(): string {
    const alvo = new Date();
    alvo.setMonth(alvo.getMonth() + 1);
    return this.getMesPlaceholder(alvo); 
  }
  
  getTerceiroMesPlaceholder(): string {
    const alvo = new Date();
    alvo.setMonth(alvo.getMonth() + 2);
    return this.getMesPlaceholder(alvo); 
  } 

  getstatusProjecao(statusCode: number): Status {
    return statuProjecao.filter(t => t.codigo === statusCode)[0];
  }

  //#endregion

  disableConfirmButton : boolean = false;

  //#region Columns

  _allColumns: TableColumn[] = [
    { name: 'usinaEntrega', placeholder: 'Central', order: 1, priority: 1, formatter: this.usinaFormatter},
    { name: 'contrato', placeholder: 'Contrato', order: 2, priority: 2, formatter: this.contratoFormatter},
    { name: 'nome', placeholder: 'Nome', order: 3, priority: 3 },
    { name: 'mesAtual', placeholder: `Vol. ${this.getMesAtualPlaceholder()}`, order: 4, priority: 4, getValue: this.getMesAtualValue },
    { name: 'mesSegundo', placeholder: `Vol. ${this.getSegundoMesPlaceholder()}`, order: 5, priority: 5, getValue: this.getSegundoMesValue },
    { name: 'mesTerceiro', placeholder: `Vol. ${this.getTerceiroMesPlaceholder()}`, order: 6, priority: 6, getValue: this.getTerceiroMesValue }
  ];

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  getFormattedValue(element: Obra, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  expandedElement: Obra | null;

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
    return ['icon'];
  }
  get fixedColumnsRight(): string[] {
    return ['edit', 'expand'];
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = CarteiraPageComponent.self;
    
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

  alwaysFoldedColumns: string[] = ['edit']
  get foldedColumns(): TableColumn[] {
    var self = CarteiraPageComponent.self;
    
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
    var self = CarteiraPageComponent.self;

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

  //#endregion
  //#region View Child
  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('projecaoObraModalVCR', { read: ViewContainerRef, static: false }) projecaoObraModalRef: ViewContainerRef;  
  @ViewChild('obraProjecaoCadastroModalVCR', { read: ViewContainerRef, static: false }) obraProjecaoCadastroModalRef: ViewContainerRef;  
  @ViewChild('modalVCR', { read: ViewContainerRef, static: false }) ModalRef: ViewContainerRef;
  //#endregion
  //#region Constructor
  constructor(
    private _obraService: ObraService,
    private _userService: UserService,
    private _usinaService: UsinaService,
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _obraProjecaoService: ObraProjecaoService
  ) { 
    CarteiraPageComponent.self = this;
    this.temDireitoAlteracaoProjecaoCarteiraMesAtual = this._userService.temDireitoAplicativo('WEB6310', 'A')

    _usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );
  }
  //#endregion
  //#region ngInit
  ngOnInit() {
    this.obraProjecaoForm = this._formBuilder.group({});
  }

  ngAfterViewInit(): void {

    this._cdr.detectChanges();
    this.filter.aplyFilter();

  }
  //#endregion
  //#region ViewChanged

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

  detectChanges(delay: number = 0) {
    if (delay)
      setTimeout(() => { this._cdr.detectChanges(); }, delay);
    else
      this._cdr.detectChanges();
  }

  //#endregion
  //#region Modal

  showSelecaoColunasModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  //#endregion
  //#region Filtros
  usinas: Usina[] = [];

  get usinasCodigos(): number[] {
    let codigos: number[] = [];
    this.usinas.forEach(t => {
      codigos.push(t.codigo);
    });
    
    return codigos;
  }
  //#endregion

  
}
