import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { CadastroGeral } from 'app/classes/cadastro-geral/cadastro-geral';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { Visita } from 'app/classes/visita/visita';
import { VisitaTipo } from 'app/classes/visita/visita-tipo';
import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { UserService } from 'app/services/user.service';
import { VisitaService } from 'app/services/visita-service';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { modelosDanfeRomaneio, Proposta } from 'app/classes/proposta/proposta';
import { debug } from 'console';
import { VisitaTipoService } from 'app/services/visita-tipo.service';
import { VendedorService } from 'app/services/vendedor.service';
import { IntervenienteAnexo } from 'app/classes/interveniente/interveniente-anexo';
import { IntervenienteService } from 'app/services/interveniente.service';
import { VisitaAnexo } from 'app/classes/visita/visita-anexo';
import { HistoricoVisitaDialogComponent } from 'app/main/components/dialog/historico-visita-dialog/historico-visita-dialog.component';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { OportunidadeTipo } from 'app/classes/oportunidade/oportunidade-tipo';
import { Interveniente, Usina } from 'app/classes/bomba/bomba.classes';
import { MotivoPerda } from 'app/classes/motivo-perda/motivo-perda';
import { Concorrente } from 'app/classes/oportunidade/concorrente';
import { ClassificacaoTemperaturas, EClassificacaoTemperatura, Oportunidade } from 'app/classes/oportunidade/oportunidade';
import { OportunidadeService } from 'app/services/oportunidade.service';
import { InteracaoOportunidadeDialogComponent } from 'app/main/components/dialog/interacao-oportunidade-dialog/interacao-oportunidade-dialog.component';
import { EClassificacaoLead } from 'app/classes/lead/lead';
import { OportunidadeFase } from 'app/classes/oportunidade/oportunidade-fase';
import { OportunidadeTipoService } from 'app/services/oportunidade-tipo.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { Router } from '@angular/router';

export interface TableColumn {
  name: string;
  placeholder: string;
  formatter?: any;
  getValue?: any;
  htmlValue?: boolean;
  button?: boolean;
  buttonOptions?: {
    text: string;
    icon: string;
    click: any;
  }
  order: number;
  priority: number;
  hidden?: any;
}


@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-oportunidade-lista-page',
  templateUrl: './oportunidade-lista-page.component.html',
  styleUrls: ['./oportunidade-lista-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class OportunidadeListaPageComponent implements OnInit, AfterViewInit {

  public static self: OportunidadeListaPageComponent;

  itens: PagedList<Oportunidade> = new PagedList<Oportunidade>();
  item: Oportunidade;

  vendedores: Vendedor[] = [];
  tipoOportunidades: OportunidadeTipo[] = [];
  fases: OportunidadeFase[] = [];
  classificacoes: any[] = ClassificacaoTemperaturas;
  segmentacoes: Segmentacao[] = [];

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    numero: number,
    ano: number,
    oportunidadeNome: string
    oportunidadeTipo: OportunidadeTipo,
    oportunidadeTipoCodigo: number,
    classificacaoSelecionada: any,
    classificacao: number,
    fase: OportunidadeFase,
    faseCodigo: number,
    dataFechamentoDe: Date,
    dataFechamentoAte: Date,
    dataDe: Date,
    dataAte: Date,
    cliente: string,
    vendedor: Vendedor,
    vendedorCodigo: number,
    enderecoLogradouro: string,
    segmentacao: Segmentacao,
    segmentacaoCodigo: number,
  } = {
    numero: 0,
    ano: 0,
    oportunidadeNome: '',

    oportunidadeTipo: null,
    oportunidadeTipoCodigo: 0,

    classificacaoSelecionada: null,
    classificacao: 0,

    fase: null,
    faseCodigo: 0,

    dataFechamentoAte: null,
    dataFechamentoDe: null,

    dataAte: null,
    dataDe: null,

    cliente: '',
    vendedor: null,
    vendedorCodigo: 0,
    enderecoLogradouro: '',
    segmentacao: null,
    segmentacaoCodigo: 0
  };

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataDataHora = Tasks.formataDataHora;
  formataValor = Tasks.formataValor;
  formataErrosApi = Tasks.formataErrosApi;

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  genericStringFormatter = (model: string): string => model ? model.toUpperCase() : 'NÃO INFORMADO';
  
  leadGetValue = (model: Oportunidade): string => model ? (model.anoLead == 0 ? 'N/A' : (model.numeroLead.toString().padStart(6, '0') + '-' + model.anoLead) ) : '';
  visitaGetValue = (model: Oportunidade): string => model ? (model.anoVisita == 0 ? 'N/A' : (model.numeroVisita.toString().padStart(6, '0') + '-' + model.anoVisita) ) : '';
  vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  segmentacaoFormatter = (model: Segmentacao): string => model ? (model.nome).toUpperCase() : '';
  cadastroGeralFormatter = (model: CadastroGeral): string => model ? model.descricao.toUpperCase() : '';
  oportunidadeTipoFormatter = (model: OportunidadeTipo): string => model ? model.descricao.toUpperCase() : '';
  intervenienteFormatter = (model: Interveniente): string => model ? (model.codigo > 0 ? (model.codigo+' - '+(model.razao || model.nome)).toUpperCase() :'') : '';
  genericFormatter = (model: any): string => model ? (model.codigo == 99 ? model.descricao : model.codigo + ' - ' + model.descricao).toUpperCase() : '';
  motivoPerdaFormatter = (model: MotivoPerda) => model ? (model.codigo + ' - ' + model.descricao) : '';
  concorrenteFormatter = (model: Concorrente) => model ? (model.codigo + ' - ' + model.descricao) : '';
  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  faseFormatter = (model: OportunidadeFase): string => model ? (model.descricao).toUpperCase() : '';

  numeroOportunidadeGetValue = (model: Oportunidade): string => model ? (model.numero.toString().padStart(6, '0') + '-' + model.ano + ' ' + model.segmentacao.nomeAbreviado) : '';
  telefoneGetValue = (model: Visita): string => model ? this.telefoneFormatter(model.dddTelefone, model.telefone) : 'NÃO INFORMADO';
  celularGetValue = (model: Visita): string => model ? this.telefoneFormatter(model.dddCelular, model.celular) : 'NÃO INFORMADO';
  obraNomeGetValue = (model: Oportunidade) : string => model ? model.obraNome : '';

  leadHidden = (model: Oportunidade) : boolean => model ? model.numeroLead === 0 : true;
  visitaHidden = (model: Oportunidade) : boolean => model ? model.numeroVisita === 0 : true;
  motivoPerdaHidden = (model: Oportunidade) : boolean => model ? model.motivoPerdaCodigo === 0 : true;
  proximaEtapaHidden = (model: Oportunidade) : boolean => model ? model.proximaEtapa === '' : true;
  telefoneHidden = (model: Oportunidade) : boolean => model ? model.telefone === 0 : true;
  celularHidden = (model: Oportunidade) : boolean => model ? model.celular === 0 : true;
  previsaoFechamentoHidden = (model: Oportunidade) : boolean => model ? !model.previsaoFechamento : true ;

  telefoneFormatter(ddd: number, numero: number): string {

    if(!ddd || !numero)
      return 'NÃO INFORMADO';

    var result = `(${ddd}) `;
    var numeroString = numero.toString();

    if(numeroString.length === 9)
      result = result + `${numeroString.substring(0, 1)} ${numeroString.substring(1, 5)}-${numeroString.substring(5, 10)}`;
    else
      result = result + `${numeroString.substring(1, 5)}-${numeroString.substring(5, 9)}`;

    return result;

  }

  getClassificacao(classificacaoCodigo: EClassificacaoTemperatura): any {
    return ClassificacaoTemperaturas.filter(t => t.codigo === classificacaoCodigo)[0];
  }

  abrirModalPropostas = (model: Oportunidade) => {
    this.item = model;
    document.getElementById("openPropostaModal").click();
  }

  propostasGetValue = (model: Oportunidade) => model ? model.propostas.length : '0';
  propostasHidden = (model: Oportunidade) => model ? model.propostas.length === 0 : true;

  _allColumns: TableColumn[] = [
    { name: 'numeroOportunidade', placeholder: 'Número', order: 1, priority: 1, getValue: this.numeroOportunidadeGetValue },
    { name: 'cliente', placeholder: 'Cliente', order: 2, priority: 2 },
    { name: 'obraNomeSuperior', placeholder: 'Obra', order: 3, priority: 3, getValue: this.obraNomeGetValue },
    { name: 'propostas', placeholder: 'Proposta(s)', order: 4, priority: 4, hidden: this.propostasHidden, getValue: this.propostasGetValue, button: true, buttonOptions: { text: 'Propostas vinculadas', click: this.abrirModalPropostas, icon: 'question_answer' }},
    { name: 'oportunidadeNome', placeholder: 'Oportunidade Nome', order: 4, priority: 4 },
    { name: 'numeroLead', placeholder: 'Lead', order: 5, priority: 5, getValue: this.leadGetValue, hidden: this.leadHidden },
    { name: 'numeroVisita', placeholder: 'Visita', order: 6, priority: 6, getValue: this.visitaGetValue, hidden: this.visitaHidden },
    { name: 'segmentacao', placeholder: 'Segmento', order: 7, priority: 7, formatter: this.segmentacaoFormatter },
    { name: 'data', placeholder: 'Data', order: 8, priority: 8, formatter: this.formataData },
    { name: 'oportunidadeTipo', placeholder: 'Tipo', order: 9, priority: 9, formatter: this.oportunidadeTipoFormatter },
    { name: 'fase', placeholder: 'Fase', order: 10, priority: 10, formatter: this.faseFormatter },
    { name: 'proximaEtapa', placeholder: 'Próxima Etapa', order: 11, priority: 11, hidden: this.proximaEtapaHidden },
    { name: 'previsaoFechamento', placeholder: 'Previsão Fechamento', order: 12, priority: 12, formatter: this.formataData, hidden:  this.previsaoFechamentoHidden },
    { name: 'obraNome', placeholder: 'Obra', order: 13, priority: 13 },
    { name: 'vendedor', placeholder: 'Vendedor', order: 14, priority: 14, formatter: this.vendedorFormatter },
    { name: 'telefone', placeholder: 'Telefone', order: 15, priority: 15, getValue: this.telefoneGetValue, hidden: this.telefoneHidden },
    { name: 'celular', placeholder: 'Celular', order: 16, priority: 16, getValue: this.celularGetValue, hidden: this.celularHidden },
    { name: 'motivoPerda', placeholder: 'Motivo Perda', order: 17, priority: 17, formatter: this.motivoPerdaFormatter, hidden: this.motivoPerdaHidden }
  ];

  get allColumns(): TableColumn[] {
      return this._allColumns;
  }

  expandedElement: VisitaTipo | null;

  hiddenColumns: string[] = [];

  isHiddenColumn(columnName: string): boolean {
    return this.hiddenColumns.includes(columnName);
  }

  setHiddenColumn(columnName: string) {
    var isHidden = this.hiddenColumns.includes(columnName);

    if (!isHidden)
      this.hiddenColumns.push(columnName);
    else
      this.hiddenColumns = this.hiddenColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }

  isFoldedColumn(columnName: string): boolean {
    return this._fixedFoldedColumns.includes(columnName);
  }

  setFoldedColumn(columnName: string) {
    var isFolded = this._fixedFoldedColumns.includes(columnName);

    if (!isFolded)
      this._fixedFoldedColumns.push(columnName);
    else
      this._fixedFoldedColumns = this._fixedFoldedColumns.filter(t => t !== columnName);

    this._cdr.detectChanges();
  }


  get columns(): TableColumn[] {
    return this.allColumns.filter(t => !this.hiddenColumns.includes(t.name));
  }

  get currentViewValue() {
    return { filter: this.filtro, hiddenColumns: this.hiddenColumns, customColumnOrder: this._customColumnOrder }
  }
  
  _fixedFoldedColumns: string[] = ['propostas', 'oportunidadeNome', 'numeroLead', 'numeroVisita', 'segmentacao', 'data', 'oportunidadeTipo', 'fase', 'proximaEtapa', 'previsaoFechamento'
    , 'obraNome', 'vendedor', 'telefone', 'celular', 'motivoPerda'
  ];

  get fixedFoldedColumns(): string[] {
    return this._fixedFoldedColumns
  }
  get fixedColumnsLeft(): string[] {
    return ['status'];
  }
  get fixedColumnsRight(): string[] {
    return ['expand']
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = OportunidadeListaPageComponent.self;
    
    return this.columns.sort((a, b) => {
      return self.getOrder(a) - self.getOrder(b);
    }).filter(t => {
      var fixedColsTotalWidth = 235;
      var colsAllowed = Math.round((window.innerWidth - fixedColsTotalWidth) / 180);
      var hiddenColumnsHighPriority = this.allColumns.filter(c => this.hiddenColumns.includes(c.name) && this.getPriority(c) < this.getPriority(t)).length;

      return (this.getPriority(t) - hiddenColumnsHighPriority) <= colsAllowed;
    })
  }

  get columnNames(): string[] {
    return this.fixedColumnsLeft.concat(this.displayedColumns.map(t => t.name)).concat(this.fixedColumnsRight).filter((c) => !this.fixedFoldedColumns.includes(c));
  }

  get foldedColumns(): TableColumn[] {
    var self = OportunidadeListaPageComponent.self;
    
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
    var self = OportunidadeListaPageComponent.self;

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

  nextFixedFolded(name: string): boolean {
    
    var columns = this._customColumnOrder.length === 0 ? this.allColumns.map(t => t.name) : this._customColumnOrder;

    var indexAtual = columns.indexOf(name);
    var actualIsFolded = this.fixedFoldedColumns.includes(name);

    if(actualIsFolded)
      return false;

    if(columns.length === (indexAtual + 1))
      return false;

    var nextColumn = columns[indexAtual + 1];
    var isFolded = this.fixedFoldedColumns.includes(nextColumn);

    return isFolded; 

  }

  previousIsNotFixedFolded(name: string): boolean {

    var columns = this._customColumnOrder.length === 0 ? this.allColumns.map(t => t.name) : this._customColumnOrder;
    var actualIsFolded = this.fixedFoldedColumns.includes(name);

    var indexAtual = columns.indexOf(name);
    if(indexAtual == 0)
      return false;

    var previousColumn = columns[indexAtual - 1];
    var isFolded = this.fixedFoldedColumns.includes(previousColumn);
    
    return isFolded != actualIsFolded;

  }


  get dataSource(): Oportunidade[] {
    return this.itens.records;
  };

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('oportunidadeLogModalVCR', { read: ViewContainerRef, static: false }) oportunidadeLogModalRef: ViewContainerRef;
  @ViewChild('anexosModalVCR', { read: ViewContainerRef, static: false }) anexosModalRef: ViewContainerRef;
  @ViewChild('tarefaModalVCR', { read: ViewContainerRef, static: false }) tarefaModalRef: ViewContainerRef;
  @ViewChild('compromissoModalVCR', { read: ViewContainerRef, static: false }) compromissoModalRef: ViewContainerRef;
  @ViewChild('propostasModalVCR', { read: ViewContainerRef, static: false }) propostasModalRef: ViewContainerRef;

  temDireitoInclusao: boolean = false;
  temDireitoInteracao: boolean = true;
  temDireitoAgenda: boolean = true;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _oportunidadeService: OportunidadeService,
    private _vendedorService: VendedorService,
    private _intervenienteService: IntervenienteService,
    private _oportunidadeTipoService: OportunidadeTipoService,
    private _segmentacao: SegmentacaoService,
    private _router: Router,
  ) { 
    
    OportunidadeListaPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6106', '', 200);
    if (!temDireito) return;

    this.temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6106','I');
    this._userService.gravarAcessoAplicacao("Comercial", 6106);

    this.temDireitoInteracao = this._userService.temDireitoAplicativo('WEB6109', '');

    this.temDireitoAgenda = this._userService.temDireitoAplicativo('WEB7007','I');

  }

  ngOnInit() {

    this._vendedorService.listarAtivos().then(
      (vendedores) => { this.vendedores = vendedores },
      (error) => { this.vendedores = [] }
    );

    this._oportunidadeTipoService.ListarAtivos().then(
      (tipos) => { this.tipoOportunidades = tipos },
      (error) => { this.tipoOportunidades = [] }
    );

    this._oportunidadeService.ListarFases().then(
      (fases) => { this.fases = fases },
      (error) => { this.fases = [] }
    );

    this._segmentacao.listarTodos().then(
      (segmentacoes) => { this.segmentacoes = segmentacoes },
      (error) => { this.segmentacoes = [] }
    );

    this.anexoForm = this._formBuilder.group({});
    this.anexoDescricaoForm = this._formBuilder.group({});    
      
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
    
    this._oportunidadeService.Listar(currentPage, pageSize, this.filtroString)
    .then(
      Oportunidade => {
        this.itens = Oportunidade;
        this.paginaAtual = Oportunidade.currentPage;
        this.registrosPorPagina = Oportunidade.pageSize;
      },
     error => { this.itens = new PagedList<Oportunidade>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  hidenColumnCondition(element: Oportunidade, column: TableColumn) {
    return column.hidden ? column.hidden(element) : false;
  }

  getFormattedValue(element: Oportunidade, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  performClick(element: Oportunidade, column: TableColumn) {
    column.buttonOptions ? column.buttonOptions.click(element) : '';
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;

    if (this.filtro.dataDe) 
      this.filtro.dataDe = new Date(this.filtro.dataDe);

    if (this.filtro.dataAte) 
      this.filtro.dataAte = new Date(this.filtro.dataAte);

    this.getPage();
  }

  showSelecaoColunasModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.colunasVisualizacaoModalRef });
    this.modalIsOpen = true;
  }

  confirmModal: Function;
  cancelModal: Function;
  async showModal(content, container: ViewContainerRef, oportunidade: Oportunidade, confirmCallback?: Function, cancelCallback?: Function) {
    
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    this.item = oportunidade;
    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    if (container == this.anexosModalRef){
      this.anexos = await this._intervenienteService.listarAnexosPorOportunidade(oportunidade.intervenienteCodigo, oportunidade.usinaCodigo, oportunidade.ano, oportunidade.numero);
      this._cdr.detectChanges();  
    }

    this.modalIsOpen = true;

  }

  closeModal() {

    let self = OportunidadeListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;

  }


  closeSubModal() {
    let self = OportunidadeListaPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  cancelSubModal() {
    let self = OportunidadeListaPageComponent.self;

    self.closeSubModal();
  }

  openInteracaoOportunidade(oportunidade: Oportunidade) {
    var self = OportunidadeListaPageComponent.self;
    self._dialog.open(InteracaoOportunidadeDialogComponent, {
      data: {
        usina: oportunidade.usinaCodigo,
        anoOportunidade: oportunidade.ano,
        numeroOportunidade: oportunidade.numero
      }
    });
  }

  interacaoPermitida(){
    return (this.temDireitoInteracao);
  }
  
  agendaPermitida(){
    return (this.temDireitoAgenda);
  }

  anexoForm: FormGroup;
  anexoDescricaoForm: FormGroup;
  anexos: IntervenienteAnexo[] = [];
  anexo: IntervenienteAnexo;
  descricaoAnteriorAnexo: string = '';

  abrirSeletorDeArquivos(inputFile: HTMLInputElement) {
    let self = OportunidadeListaPageComponent.self;
    var temDireito = self._userService.temDireitoAplicativo('CON0036','I');
    if (!temDireito) {
     self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para inserir Anexos!`
        }
      });
      return;
    }

    inputFile.click();
  }

  arquivoSelecionado(event: Event) {
    let self = OportunidadeListaPageComponent.self;
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onloadend = async () => {
        const base64String = reader.result as string;
        try {
          await self._intervenienteService.adicionarAnexoPorOportunidade(base64String,self.item.intervenienteCodigo, self.item.ano, self.item.numero, file.name);
          self.anexos = await self._intervenienteService.listarAnexosPorOportunidade(self.item.intervenienteCodigo, self.item.usinaCodigo, self.item.ano, self.item.numero);
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Anexo inserido com sucesso!'
            }
          });
        } catch (err) {
          self._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Erro ao inserir o Anexo.\n${err}`
            }
          });
        }
      };
      
      reader.readAsDataURL(file)
    }
  }

  abrirAnexo(anexo: IntervenienteAnexo) {
    let self = OportunidadeListaPageComponent.self;

    self._intervenienteService.ObterAnexo(anexo)
    .then(url => {
      var type = url.split(';')[0];
      type = type.replace("data:", "");
      var arquivo = url.split(',')[1]
      Tasks.openBase64File(arquivo, anexo.nome, type)
    }).catch(error => {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Erro ao obter o Anexo: ${JSON.stringify(error.exceptionMessage)}`
        }
      });
      return;
  });
  }

  atualizarDescricaoAnexo(anexo: IntervenienteAnexo): void {
    let self = OportunidadeListaPageComponent.self;

    var temDireito = self._userService.temDireitoAplicativo('CON0036','A');
    if (!temDireito) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para alterar Anexos!`
        }
      });
      return;
    }

    self._intervenienteService.atualizarDescricaoAnexo(anexo)
     .then(success => {
      self.closeSubModal();
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Descrição alterada com sucesso!`
        }
      });
     }, err => {
      self.anexo.descricao = self.descricaoAnteriorAnexo;
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro alterar a descrição.\n${JSON.stringify(err.exceptionMessage)}`
        }
      });
     });
  }

  removerAnexo(anexo: IntervenienteAnexo) {
    let self = OportunidadeListaPageComponent.self;

    var temDireito = self._userService.temDireitoAplicativo('CON0036','E');
    if (!temDireito) {
      self._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para excluir Anexos!`
        }
      });
      return;
    }

    self._intervenienteService.removerAnexo(anexo)
      .then(success => {
        if (success) self.anexos = self.anexos.filter(a => a !== anexo);
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

  showModalDescricaoAnexo(content, container: ViewContainerRef, anexo: IntervenienteAnexo) {
    let self = OportunidadeListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self.anexo = anexo;
    self.descricaoAnteriorAnexo = anexo.descricao;

    self._subDialogRef = self._dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  

  abrirModalPropostasRef(content, container: ViewContainerRef) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });
    this.modalIsOpen = true;
  }

  direcionarProposta(usinaCodigo: number, model: Proposta) {
    this.closeModal();
    setTimeout(() => {
      this._router.navigateByUrl('pages/comercial/proposta/usina/' + usinaCodigo + '/ano/' + model.ano + '/numero/' + model.numero);
    }, 1000);
  }
}
