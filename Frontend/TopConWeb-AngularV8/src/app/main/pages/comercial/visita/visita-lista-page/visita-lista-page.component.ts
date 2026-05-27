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
import { TableColumn } from '../../cadastros/demais-servicos-page/demais-servicos-page.component';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { modelosDanfeRomaneio } from 'app/classes/proposta/proposta';
import { debug } from 'console';
import { VisitaTipoService } from 'app/services/visita-tipo.service';
import { VendedorService } from 'app/services/vendedor.service';
import { IntervenienteAnexo } from 'app/classes/interveniente/interveniente-anexo';
import { IntervenienteService } from 'app/services/interveniente.service';
import { VisitaAnexo } from 'app/classes/visita/visita-anexo';
import { HistoricoVisitaDialogComponent } from 'app/main/components/dialog/historico-visita-dialog/historico-visita-dialog.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-visita-lista-page',
  templateUrl: './visita-lista-page.component.html',
  styleUrls: ['./visita-lista-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class VisitaListaPageComponent implements OnInit, AfterViewInit {

  public static self: VisitaListaPageComponent;

  tipoVisitaForm: FormGroup;

  anexoForm: FormGroup;
  anexoDescricaoForm: FormGroup;
  anexos: VisitaAnexo[] = [];
  anexo: VisitaAnexo;
  descricaoAnteriorAnexo: string = '';
  
  itens: PagedList<Visita> = new PagedList<Visita>();
  item: Visita;

  tiposVisita: VisitaTipo[] = [];
  vendedores: Vendedor[] = [];

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  temDireitoInteracao: boolean = true;
  temDireitoAgenda: boolean = true;

  filtroString: string = '';

  filtro: {
    ano: number,
    numero: number,
    tipoVisita: VisitaTipo,
    visitaTipoCodigo: number,
    dataDe: Date,
    dataAte: Date,
    cliente: string,
    vendedor: Vendedor,
    vendedorCodigo: number,
    enderecoLogradouro: string
  } = {
    ano: 0,
    numero: 0,
    tipoVisita: null,
    visitaTipoCodigo: 0,
    dataDe: null,
    dataAte: null,
    cliente: '',
    vendedor: null,
    vendedorCodigo: 0,
    enderecoLogradouro: ''
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

  vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  genericStringFormatter = (model: string): string => model ? model.toUpperCase() : 'NÃO INFORMADO';
  cadastroGeralFormatter = (model: CadastroGeral): string => model ? model.descricao.toUpperCase() : '';
  tipoVisitaFormatter = (model: VisitaTipo): string => model ? (model.codigo + ' - ' + model.descricao).toUpperCase() : '';
  dataVisitaFormatter = (model: Date): string => model ? Tasks.formataData(model) : '';
  horaVisitaFormatter = (model: string): string => model ? model.substring(0, 5) : '';
  numeroVisitaGetValue = (model: Visita): string => model ? (model.numero.toString().padStart(6, '0') + '-' + model.ano) : '';
  leadGetValue = (model: Visita): string => model ? (model.leadNumero == 0 ? 'NÃO GERADO' : (model.leadNumero.toString().padStart(6, '0') + '-' + model.leadAno) ) : '';
  
  telefoneGetValue = (model: Visita): string => model ? this.telefoneFormatter(model.dddTelefone, model.telefone) : 'NÃO INFORMADO';
  celularGetValue = (model: Visita): string => model ? this.telefoneFormatter(model.dddCelular, model.celular) : 'NÃO INFORMADO';

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

  
  _allColumns: TableColumn[] = [
    { name: 'numeroVisita', placeholder: 'Visita', order: 1, priority: 1, getValue: this.numeroVisitaGetValue },
    { name: 'cliente', placeholder: 'Cliente', order: 2, priority: 2 },
    { name: 'tipoVisita', placeholder: 'Tipo Visita', order: 3, priority: 3, formatter: this.tipoVisitaFormatter },
    { name: 'data', placeholder: 'Data Visita', order: 4, priority: 4, formatter: this.dataVisitaFormatter },
    { name: 'horaVisita', placeholder: 'Hora Visita', order: 4, priority: 4, formatter: this.horaVisitaFormatter },
    { name: 'lead', placeholder: 'Lead', order: 5, priority: 5, getValue: this.leadGetValue },
    { name: 'obraNome', placeholder: 'Obra', order: 6, priority: 6, formatter: this.genericStringFormatter },
    { name: 'vendedor', placeholder: 'Vendedor', order: 7, priority: 7, formatter: this.vendedorFormatter },
    { name: 'telefone', placeholder: 'Telefone', order: 8, priority: 8, getValue: this.telefoneGetValue },
    { name: 'celular', placeholder: 'Celular', order: 9, priority: 9, getValue: this.celularGetValue }
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
  
  get fixedFoldedColumns(): string[] {
    return ['lead', 'obraNome', 'vendedor', 'telefone', 'celular'];
  }
  get fixedColumnsLeft(): string[] {
    return [];
  }
  get fixedColumnsRight(): string[] {
    return ['expand']
  }
  get fixedColumns(): string[] {
    return this.fixedColumnsLeft.concat(this.fixedColumnsRight);
  }

  get displayedColumns(): TableColumn[] {
    var self = VisitaListaPageComponent.self;
    
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
    var self = VisitaListaPageComponent.self;
    
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
    var self = VisitaListaPageComponent.self;

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


  get dataSource(): Visita[] {
    return this.itens.records;
  };

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;
  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('tarefaModalVCR', { read: ViewContainerRef, static: false }) tarefaModalRef: ViewContainerRef;
  @ViewChild('compromissoModalVCR', { read: ViewContainerRef, static: false }) compromissoModalRef: ViewContainerRef;
  @ViewChild('anexosModalVCR', { read: ViewContainerRef, static: false }) anexosModalRef: ViewContainerRef;
  @ViewChild('descricaoAnexoModalVCR', { read: ViewContainerRef, static: false }) descricaoAnexoModalRef: ViewContainerRef;
  @ViewChild('visitaLogModalVCR', { read: ViewContainerRef, static: false }) visitaLogModalRef: ViewContainerRef;


  temDireitoInclusao: boolean = false;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _visitaService: VisitaService,
    private _visitaTipoService: VisitaTipoService,
    private _vendedorService: VendedorService,
    private _intervenienteService: IntervenienteService,
  ) { 
    
    VisitaListaPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6104', '', 200);
    if (!temDireito) return;

    this.temDireitoInteracao = this._userService.temDireitoAplicativo('WEB6108', '');

    this.temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6104','I');
    this._userService.gravarAcessoAplicacao("Comercial", 6104);

    this.temDireitoAgenda = this._userService.temDireitoAplicativo('WEB7007','I');

  }

  ngOnInit() {

    this._visitaTipoService.ListarAtivos().then(
      (tiposVisita) => { this.tiposVisita = tiposVisita },
      (error) => { this.tiposVisita = [] });

    this._vendedorService.listarAtivos().then(
      (vendedores) => { this.vendedores = vendedores },
      (error) => { this.vendedores = [] }
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
    
    this._visitaService.Listar(currentPage, pageSize, this.filtroString)
    .then(
      Visita => {
        this.itens = Visita;
        this.paginaAtual = Visita.currentPage;
        this.registrosPorPagina = Visita.pageSize;
      },
     error => { this.itens = new PagedList<Visita>(); }
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
  showModal(content, container: ViewContainerRef, visita: Visita, confirmCallback: Function, cancelCallback?: Function) {

    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    this.item = visita;
    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    if (container == this.anexosModalRef){
      this._visitaService.listarAnexos(this.item.usinaCodigo, this.item.ano, this.item.numero)
      .then(
        (anexos) => { this.anexos = anexos; }, 
        (err) => { this.anexos = [] }
      );
      this._cdr.detectChanges();  
    }

    this.modalIsOpen = true;

  }

  closeModal() {
    let self = VisitaListaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.tipoVisitaForm.markAsPristine();
    self.tipoVisitaForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  openHistoricoVisita(visita: Visita) {
    var self = VisitaListaPageComponent.self;
    self._dialog.open(HistoricoVisitaDialogComponent, {
      data: {
        usina: visita.usinaCodigo,
        anoVisita: visita.ano,
        numeroVisita: visita.numero
      }
    });
  }

  abrirSeletorDeArquivos(inputFile: HTMLInputElement) {
    let self = VisitaListaPageComponent.self;
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
    let self = VisitaListaPageComponent.self;
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onloadend = async () => {
        const base64String = reader.result as string;
        try {
          await self._visitaService.adicionarAnexo(
            self.item.usinaCodigo,
            self.item.ano,
            self.item.numero,
            base64String,
            file.name
          );
          
          self._visitaService.listarAnexos(self.item.usinaCodigo, self.item.ano, self.item.numero)
          .then(
            (anexos) => { this.anexos = anexos; }, 
            (err) => { this.anexos = [] }
          );
          
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

  abrirAnexo(anexo: VisitaAnexo) {
    let self = VisitaListaPageComponent.self;

    self._visitaService.ObterAnexo(anexo.id)
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

  atualizarDescricaoAnexo(anexo: VisitaAnexo): void {
    let self = VisitaListaPageComponent.self;

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

    self._visitaService.atualizarDescricaoAnexo(anexo)
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

  removerAnexo(anexo: VisitaAnexo) {
    let self = VisitaListaPageComponent.self;

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

    self._visitaService.removerAnexo(anexo.id)
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

  showModalDescricaoAnexo(content, container: ViewContainerRef, anexo: VisitaAnexo) {
    let self = VisitaListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self.anexo = anexo;
    self.descricaoAnteriorAnexo = anexo.descricao;

    self._subDialogRef = self._dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  closeSubModal() {
    let self = VisitaListaPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  cancelSubModal() {
    let self = VisitaListaPageComponent.self;

    self.closeSubModal();
    self.anexo.descricao = self.descricaoAnteriorAnexo;
  }

  interacaoPermitida(){
    return (this.temDireitoInteracao);
  }

  agendaPermitida(){
    return (this.temDireitoAgenda);
  }
}
