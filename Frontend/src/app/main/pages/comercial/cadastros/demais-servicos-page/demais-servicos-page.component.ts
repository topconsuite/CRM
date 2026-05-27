import { Component, OnInit, AfterViewInit, ViewChild, 
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';
import { DemaisServicos, EFormaDeCobrancaDemaisServicos, EFrequenciaDeCobranca } from 'app/classes/demais-servicos/demais-servicos';
import { Usina } from 'app/classes/usina/usina';
import { Mercadoria, Unidade } from 'app/classes/mercadoria/mercadoria';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';

import { UsinaService } from 'app/services/usina.service';
import { DemaisServicosService } from 'app/services/demais-servicos.service';
import { MercadoriaService } from 'app/services/mercadoria.service';
import { UnidadeService } from 'app/services/unidade.service';
import { UserService } from 'app/services/user.service';

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
  selector: 'app-demais-servicos-page',
  templateUrl: './demais-servicos-page.component.html',
  styleUrls: ['./demais-servicos-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class DemaisServicosPageComponent implements OnInit, AfterViewInit {
  public static self: DemaisServicosPageComponent;

  usinas: Usina[] = [];
  unidades: Unidade[] = [];
  mercadorias: Mercadoria[] = [];

  demaisServicosForm: FormGroup;

  itens: PagedList<DemaisServicos> = new PagedList<DemaisServicos>();
  item: DemaisServicos;

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';

  filtro: {
    codigo: number,
    usina: Usina,
    mercadoria: Mercadoria,
    unidade: Unidade
  } = {
    codigo: 0,
    usina: null,
    mercadoria: null,
    unidade: null
  };

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  unidadeFormatter = (model: Unidade): string => model ? ('('+model.sigla+') '+model.descricao).toUpperCase() : '';
  mercadoriaFormatter = (model: Mercadoria): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  
  moedaFormatter = (item: number) => {
    let self = DemaisServicosPageComponent.self;
    return self.formataValor(item, 2, true);
  }

  booleanFormatter = (item: boolean) => {
    if (item) return 'Sim';
    return 'Não'
  }

  get frequenciaCobrancaLista(): EFrequenciaDeCobranca[] {
    return [
      EFrequenciaDeCobranca.Bombeamento,
      EFrequenciaDeCobranca.Contrato,
      EFrequenciaDeCobranca.M3,
      EFrequenciaDeCobranca.M3Bombeado,
      EFrequenciaDeCobranca.Programacao,
      EFrequenciaDeCobranca.Remessa
    ];
  }

  frequenciaCobrancaFormatter = (item: EFrequenciaDeCobranca) => {
    //console.log(`${EFrequenciaDeCobranca[item]}`);
    switch (item) {
      case EFrequenciaDeCobranca.Bombeamento:
        return 'Bombeamento'.toUpperCase();
      case EFrequenciaDeCobranca.Contrato:
        return 'Contrato'.toUpperCase();
      case EFrequenciaDeCobranca.M3:
        return 'M3'.toUpperCase();
      case EFrequenciaDeCobranca.M3Bombeado:
        return 'M3 Bombeado'.toUpperCase();
      case EFrequenciaDeCobranca.Programacao:
        return 'Programacao'.toUpperCase();
      case EFrequenciaDeCobranca.Remessa:
        return 'Remessa'.toUpperCase();
      default:
        return '';
    }
  }

  get formaDeCobrancaLista(): EFormaDeCobrancaDemaisServicos[] {
    return [
      //EFormaDeCobrancaDemaisServicos.FinalConcretagem,
      EFormaDeCobrancaDemaisServicos.NaRemessa
    ];
  }

  formaDeCobrancaFormatter = (item: EFormaDeCobrancaDemaisServicos) => {
    //console.log(`${EFormaDeCobrancaDemaisServicos[item]}`);
    switch (item) {
      case EFormaDeCobrancaDemaisServicos.FinalConcretagem:
        return 'Final Concretagem'.toUpperCase();
      case EFormaDeCobrancaDemaisServicos.NaRemessa:
        return 'Na Remessa'.toUpperCase();
      default:
        return '';
    }
  }

  columns: TableColumn[] = [
    { name: 'codigo', placeholder: 'Código', order: 1, priority: 1 },
    { name: 'usina', placeholder: 'Usina', order: 2, priority: 2, formatter: this.usinaFormatter  },
    { name: 'mercadoria', placeholder: 'Produto/Serviço', order: 3, priority: 3, formatter: this.mercadoriaFormatter },
    { name: 'unidade', placeholder: 'Unidade', order: 4, priority: 4, formatter: this.unidadeFormatter },
    { name: 'numeroDeCasasDecimais', placeholder: 'Casas Decimais', order: 5, priority: 5 },
    { name: 'precoSugerido', placeholder: 'Preço Sugerido', order: 6, priority: 6, formatter: this.moedaFormatter },
    { name: 'precoMinimo', placeholder: 'Preço Mínimo', order: 7, priority: 7, formatter: this.moedaFormatter },
    { name: 'frequenciaDeCobranca', placeholder: 'Frequência Cobrança', order: 8, priority: 8, formatter: this.frequenciaCobrancaFormatter },
    { name: 'formaDeCobranca', placeholder: 'Forma Cobrança', order: 9, priority: 9, formatter: this.formaDeCobrancaFormatter },
    { name: 'atualizaEstoque', placeholder: 'Atualiza Estoque', order: 10, priority: 10, formatter: this.booleanFormatter }
  ];

  expandedElement: DemaisServicos | null;

  get fixedColumns(): string[] {
    return ['edit','delete'];
  }

  get displayedColumns(): TableColumn[] {
    return this.columns.sort((a, b) => {
      return a.order - b.order;
    }).filter(t => {
      var colsAllowed = Math.round((window.innerWidth - 130) / 100);
      return t.priority <= colsAllowed;
    });
  }
  get columnNames(): string[] {
    return this.displayedColumns.map(t => t.name).concat(this.fixedColumns);
  }
  get hiddenColumns(): TableColumn[] {
    return this.columns.sort((a, b) => {
      return a.order - b.order;
    }).filter(t => !this.columnNames.includes(t.name));
  }

  get dataSource(): DemaisServicos[] {
    return this.itens.records;
  };

  exibirAcompanhamento: boolean = false;


  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('dsModalVCR', { read: ViewContainerRef, static: false }) demaisServicosModalVCR: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _usinaService: UsinaService,
    private _demaisServicosService: DemaisServicosService,
    private _mercadoriaService: MercadoriaService,
    private _unidadeService: UnidadeService
  ) {
    DemaisServicosPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6002','', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Cadastro", 6002);
  }

  ngOnInit() {
    this.demaisServicosForm = this._formBuilder.group({
      atualizaEstoque: ['']
    });
  }
  ngAfterViewInit(): void {
    let d = JSON.stringify(this.filter.defaultModel);
    
    this._usinaService.listarTodos().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );
    
    this._unidadeService.listarTodos().then(
      unidades => { this.unidades = unidades },
      error => { this.unidades = [] }
    );

    let _filtroToken = localStorage.getItem("t.tcw.demais-servicos.filtro.token");
    let _token = localStorage.getItem("t.tcw.token");

    let _filtro = localStorage.getItem("t.tcw.demais-servicos.filtro");

    if (_filtro && _filtro !== d && _filtroToken === _token) {
      this.filtro = JSON.parse(_filtro) || this.filter.defaultModel;

      this.filtro = { ...this.filter.defaultModel, ...this.filtro };
    }

    this._cdr.detectChanges();
    this.filter.aplyFilter();
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;
    
    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    };
    
    this._demaisServicosService.listarFiltrados(currentPage, pageSize, this.filtroString)
    .then(
      demaisServicos => {
        this.itens = demaisServicos;
        this.paginaAtual = demaisServicos.currentPage;
        this.registrosPorPagina = demaisServicos.pageSize;
      },
      error => { this.itens = new PagedList<DemaisServicos>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  getFormattedValue(element: DemaisServicos, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    localStorage.setItem("t.tcw.demais-servicos.filtro", JSON.stringify(this.filtro));
    localStorage.setItem("t.tcw.demais-servicos.filtro.token", localStorage.getItem("t.tcw.token"))
    this.getPage();
  }

  private _timeoutFiltrarMercadoria = null;
  filtrarMercadorias(text: string) {
    //this.filtro.intervenienteRazao = text;

    var tamanhoMinimo = (isNaN(parseInt(text)) ? 3 : 0);

    if (text && text.length>tamanhoMinimo && (!this.filtro.mercadoria || this.filtro.mercadoria.descricao!=text)) {
      
      if (this._timeoutFiltrarMercadoria) clearTimeout(this._timeoutFiltrarMercadoria);
      
      var filtro = 'filter=$(' + (isNaN(parseInt(text)) ? 'descricao%=' + text : 'codigo==' + parseInt(text)) + ')';
      
      this._timeoutFiltrarMercadoria = setTimeout( () => {
        this._mercadoriaService.listarProdutosEServicosFiltrados(filtro, true)
          .then(
            mercadorias => { this.mercadorias = mercadorias; },
            error => { this.mercadorias = []; }
          )
      }, 500);

    } else {
      this.mercadorias = [];
    }
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, demaisServicos?: DemaisServicos) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if (demaisServicos) {
      var temDireito = this._userService.temDireitoAplicativo('WEB6002','A');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão de alteração!`
          }
        });
        return;
      }

      this.item = JSON.parse(JSON.stringify(demaisServicos));
    } else {
      var temDireito = this._userService.temDireitoAplicativo('WEB6002','I');
      if (!temDireito) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão para inserir demais serviços!`
          }
        });
        return;
      }

      this.item = new DemaisServicos();
    }
    
    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  closeModal() {
    let self = DemaisServicosPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.demaisServicosForm.markAsPristine();
    self.demaisServicosForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  addDemaisServicos(demaisServicos: DemaisServicos): void {
    let self = DemaisServicosPageComponent.self;

    self._demaisServicosService.adicionar(demaisServicos)
    .then(success => {
      self.closeModal();
      self.getPage();
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao tentar adicionar item.\n${JSON.stringify(err).substr(0, 50)}`
        }
      });
    });
    
  }
  
  updateDemaisServicos(demaisServicos: DemaisServicos): void {
    let self = DemaisServicosPageComponent.self;
    
    self._demaisServicosService.atualizar(demaisServicos)
    .then(success => {
      self.closeModal();
      self.getPage();
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao tentar atualizar item.\n${JSON.stringify(err).substr(0, 50)}`
        }
      });
    });
  }

  removeDemaisServicos(demaisServicos: DemaisServicos): void {
    let self = DemaisServicosPageComponent.self;

    var temDireito = this._userService.temDireitoAplicativo('WEB6002','E');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de exclusão!`
        }
      });
      return;
    }
    
    self._demaisServicosService.delete(demaisServicos.codigo)
    .then(success => {
      self.getPage();
    }, err => {
      self._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao tentar excluir item.\n${JSON.stringify(err).substr(0, 50)}`
        }
      });
    });
  }

}
