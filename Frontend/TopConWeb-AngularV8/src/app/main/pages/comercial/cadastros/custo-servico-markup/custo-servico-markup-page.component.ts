import {
  Component, OnInit, AfterViewInit, ViewChild,
  ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef
} from '@angular/core';
import { MatDialogRef, MatDialog } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { trigger, state, transition, style, animate } from '@angular/animations';

import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Usina } from 'app/classes/usina/usina';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';

import { FilterComponent } from 'app/main/components/list/filter/filter.component';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';

import { UsinaService } from 'app/services/usina.service';
import { UserService } from 'app/services/user.service';
import { CustoServicoService } from 'app/services/custo-servico.service';

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
  templateUrl: './custo-servico-markup-page.component.html',
  styleUrls: ['./custo-servico-markup-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class CustoServicoMarkupPageComponent implements OnInit, AfterViewInit {
  public static self: CustoServicoMarkupPageComponent;

  usinas: Usina[] = [];

  custoServicoMarkupForm: FormGroup;

  itens: PagedList<CustoServico> = new PagedList<CustoServico>();
  item: CustoServico;

  isEdicao: boolean = false;

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  filtroString: string = '';
  filtro: {
    usina: Usina
  } = {
      usina: null,
    };

  formataData = Tasks.formataData;
  formataValor = Tasks.formataValor;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';

  decimalFormatter = (item: number) => {
    let self = CustoServicoMarkupPageComponent.self;
    return self.formataValor(item, 2, true);
  }

  columns: TableColumn[] = [
    { name: 'usina', placeholder: 'Usina', order: 1, priority: 1, formatter: this.usinaFormatter },
    { name: 'dataInicioVigencia', placeholder: 'Data início validade', order: 2, priority: 2, formatter: this.formataData },
    { name: 'custoMedioServico', placeholder: 'Custo Serviço', order: 3, priority: 3, formatter: this.decimalFormatter },
    { name: 'custoMedioProducao', placeholder: 'Custo Produção', order: 4, priority: 5, formatter: this.decimalFormatter },
    { name: 'custoMedioHoraTransporte', placeholder: 'Custo Transporte', order: 5, priority: 6, formatter: this.decimalFormatter },
    { name: 'markup', placeholder: 'Markup (%)', order: 6, priority: 4, formatter: this.decimalFormatter }
  ];

  expandedElement: CustoServico | null;

  get fixedColumns(): string[] {
    return ['edit', 'delete'];
  }

  get displayedColumns(): TableColumn[] {
    return this.columns.sort((a, b) => {
      return a.order - b.order;
    }).filter(t => {
      const colsAllowed = Math.round((window.innerWidth - 130) / 100);
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

  get dataSource(): CustoServico[] {
    return this.itens.records;
  };

  get valorMedioServico() {
    const { item } = this;
    const valoresCalculoCustoMedioServico = [
      item.custoMedioCombustivel, item.custoMedioMaoDeObra, item.custoMedioImpostos,
      item.custoMedioManutencao, item.custoMedioLubrificantes, item.lucro,
      item.custoMedioProducao, item.custoMedioLaboratorio, item.custoMedioComercial,
      item.custoMedioAdministrativo
    ];

    item.custoMedioServico = valoresCalculoCustoMedioServico
      .reduce((valoresAnteriores, valorAtual) => valoresAnteriores + valorAtual);
    return item.custoMedioServico;
  }

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('csmModalVCR', { read: ViewContainerRef, static: false }) custoServicoMarkupModalVCR: ViewContainerRef;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _usinaService: UsinaService,
    private _custoServicoService: CustoServicoService
  ) {
    CustoServicoMarkupPageComponent.self = this;

    const temDireito = this._userService.temDireitoAplicativo('WEB6006', '', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao('Cadastro', 6002);
  }

  ngOnInit() {
    this.custoServicoMarkupForm = this._formBuilder.group({
      atualizaEstoque: ['']
    });
  }

  ngAfterViewInit(): void {
    const d = JSON.stringify(this.filter.defaultModel);

    this._usinaService.listarTodos().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );

    const _filtroToken = localStorage.getItem('t.tcw.custo-servico-markup.filtro.token');
    const _token = localStorage.getItem('t.tcw.token');
    const _filtro = localStorage.getItem('t.tcw.custo-servico-markup.filtro');

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

    this._custoServicoService.listarFiltrados(currentPage, pageSize, this.filtroString)
      .then(
        custoServicos => {
          this.itens = custoServicos;
          this.paginaAtual = custoServicos.currentPage;
          this.registrosPorPagina = custoServicos.pageSize;
        },
        error => { this.itens = new PagedList<CustoServico>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
  }

  getFormattedValue(element: CustoServico, column: TableColumn) {
    return column.getValue ? column.getValue(element) : (column.formatter ? column.formatter(element[column.name]) : element[column.name])
  }

  filtroChange(novoFiltro: string) {
    this.filtroString = novoFiltro;
    localStorage.setItem('t.tcw.custo-servico-markup.filtro', JSON.stringify(this.filtro));
    localStorage.setItem('t.tcw.custo-servico-markup.filtro.token', localStorage.getItem('t.tcw.token'));
    this.getPage();
  }

  exibirAlerta(mensagem: string, titulo: string = 'TopConWeb') {
    this._dialog.open(AlertDialogComponent, {
      data: {
        title: titulo,
        message: mensagem
      }
    });
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function, custoServico?: CustoServico) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if (custoServico) {
      this.isEdicao = true;
      const temDireito = this._userService.temDireitoAplicativo('WEB6006', 'A');
      if (!temDireito) {
        this.exibirAlerta('Você não tem permissão de alteração!');
        return;
      }
      this.item = JSON.parse(JSON.stringify(custoServico));
    } else {
      this.isEdicao = false;
      const temDireito = this._userService.temDireitoAplicativo('WEB6006', 'I');
      if (!temDireito) {
        this.exibirAlerta('Você não tem permissão para inserir custo de serviço!');
        return;
      }

      this.item = new CustoServico();
    }

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;
  }

  closeModal() {
    const self = CustoServicoMarkupPageComponent.self;

    if (self._dialogRef) self._dialogRef.close();

    self.custoServicoMarkupForm.markAsPristine();
    self.custoServicoMarkupForm.markAsUntouched();

    self.modalIsOpen = false;
  }

  addCustoServico(custoServico: CustoServico) {
    const self = CustoServicoMarkupPageComponent.self;

    const codigoUsina = custoServico.usina.codigo;
    custoServico.usinaCodigo = codigoUsina;
    custoServico.usinaTabelapreco = codigoUsina;
    custoServico.formaMedidaAditivo = 1; // PESO

    self._custoServicoService.Adicionar(custoServico)
      .then(success => {
        self.closeModal();
        self.getPage();
      }, err => {
        this.exibirAlerta(`Erro ao tentar adicionar item.\n${JSON.stringify(err).substr(0, 50)}`);
      });
  }

  updateCustoServico(custoServico: CustoServico): void {
    const self = CustoServicoMarkupPageComponent.self;

    self._custoServicoService.Atualizar(custoServico)
      .then(success => {
        self.closeModal();
        self.getPage();
      }, err => {
        this.exibirAlerta(`Erro ao tentar atualizar item.\n${JSON.stringify(err).substr(0, 50)}`);
      });
  }

  removecustoServico(custoServico: CustoServico) {
    const self = CustoServicoMarkupPageComponent.self;

    const temDireito = this._userService.temDireitoAplicativo('WEB6006', 'E');
    if (!temDireito) {
      this.exibirAlerta('Você não tem permissão de exclusão!');
      return;
    }

    const dataFormatada = this.formataData(custoServico.dataInicioVigencia).split('/').join('-');
    self._custoServicoService.Deletar(custoServico.usinaCodigo, dataFormatada)
      .then(success => {
        self.getPage();
      }, err => {
        this.exibirAlerta(`Erro ao tentar excluir item.\n${JSON.stringify(err).substr(0, 50)}`);
      });
  }
  
}
