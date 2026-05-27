import { Component, OnInit, ViewChild, ChangeDetectorRef, AfterViewInit, ViewContainerRef, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms';
import { animate, state, style, transition, trigger } from '@angular/animations';

// **** CLASSES ***
import { Tasks } from 'app/classes/_tasks/tasks';
import { PagedList } from 'app/classes/pagination/paged-list';

// **** CLASSES ***
import { Programacao, Status, statusProgramacao, EProgramacaoStatus, EProgramacaoConfirmacao, ProgramacaoHora } from 'app/classes/programacao/programacao.classes';
import { statusContrato, statusProposta, statusComercial } from 'app/classes/proposta/proposta.classes';
import { Interveniente } from 'app/classes/interveniente/interveniente';
import { Usina } from 'app/classes/usina/usina';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { Endereco } from 'app/classes/endereco/endereco';
import { ETipoVinculoMpaConsumo } from 'app/classes/traco/traco.classes';
import { CondicaoPagamento } from 'app/classes/pagamento/pagamento.classes';

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { ProgramacaoService } from 'app/services/programacao.service';
import { PropostaService } from 'app/services/proposta.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { UsinaService } from 'app/services/usina.service';
import { VendedorService } from 'app/services/vendedor.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';

import { ProgramacaoLogDialogComponent } from '../../../../components/dialog/programacao-log-dialog/programacao-log-dialog.component';
import { AlertDialogComponent } from '../../../../components/dialog/alert-dialog/alert-dialog.component';
import { FilterComponent } from '../../../../components/list/filter/filter.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';

import { element } from 'protractor';
import { ICustomView } from 'app/main/components/list/view-selector/view-selector.component';
import { ObraTraco } from 'app/classes/obra/obra-traco';
import { Router } from '@angular/router';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { LeadService } from 'app/services/lead.service';
import { Classificacao, classificacaoLead, Fase, faseLead, Lead } from 'app/classes/lead/lead';
import { LeadFase } from 'app/classes/lead/lead-fase';
import { EClassificacaoTemperatura } from 'app/classes/oportunidade/oportunidade';
import { LeadFaseService } from 'app/services/lead-fase.service';
import { LeadLog } from 'app/classes/lead/lead-log';
import { LeadAnexo } from 'app/classes/lead/lead-anexo';
import { InteracaoLeadDialogComponent } from 'app/main/components/dialog/interacao-lead-dialog/interacao-lead-dialog.component';


export interface TableColumn {
  name: string;
  placeholder: string;
  align?: string;
  formatter?: any;
  getValue?: any;
  order: number;
  priority: number;
}

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-lead-lista-page',
  templateUrl: './lead-lista-page.component.html',
  styleUrls: ['./lead-lista-page.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class LeadListaPageComponent implements OnInit, AfterViewInit {
  public static self: LeadListaPageComponent;

  anexoForm: FormGroup;
  anexoDescricaoForm: FormGroup;

  item: Lead;

  leads: PagedList<Lead> = new PagedList<Lead>();
  usinas: Usina[] = [];

  fasesLead: LeadFase[] = [];
  logs: LeadLog[] = [];

  get classificacaoLead(): number[] {
    let codigos: number[] = [];
    classificacaoLead.forEach(classificacao => {
      codigos.push(classificacao.codigo);
    })
    return codigos;
  };

  anexos: LeadAnexo[] = [];
  anexo: LeadAnexo;
  descricaoAnteriorAnexo: string = '';

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  temDireitoInteracao: boolean = true;
  temDireitoOportunidade: boolean = true;
  temDireitoAgenda: boolean = true;

  vendedoresPermitidos: Vendedor[] = [];

  usinasEntrega: Usina[];

  filtroString: string = '';

  filtro: {
    leadNumero: number,
    leadAno: number,
    fase: LeadFase,
    classificacao: EClassificacaoTemperatura,
    dataDe: Date,
    dataAte: Date,
    cliente: string,
    vendedor: Vendedor,
    enderecoObra: string
  } = {
    leadNumero: 0,
    leadAno: 0,
    fase: null,
    classificacao: null,
    dataDe: null,
    dataAte: null,
    cliente: '',
    vendedor: null,
    enderecoObra: ''
  };

  formataTelefone = Tasks.formataTelefone;
  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataDataHora = Tasks.formataDataHora;
  formataErrosApi = Tasks.formataErrosApi;

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  get dataSource(): Lead[] {
    return this.leads.records;
  };

  @ViewChild(FilterComponent, { static: false }) filter: FilterComponent;

  @ViewChild('colunasVisualizacaoModalVCR', { read: ViewContainerRef, static: false }) colunasVisualizacaoModalRef: ViewContainerRef;
  @ViewChild('leadLogModalVCR', { read: ViewContainerRef, static: false }) leadLogModalRef: ViewContainerRef;
  @ViewChild('anexosModalVCR', { read: ViewContainerRef, static: false }) anexosModalRef: ViewContainerRef;
  @ViewChild('descricaoAnexoModalVCR', { read: ViewContainerRef, static: false }) descricaoAnexoModalRef: ViewContainerRef;
  @ViewChild('tarefaModalVCR', { read: ViewContainerRef, static: false }) tarefaModalRef: ViewContainerRef;
  @ViewChild('compromissoModalVCR', { read: ViewContainerRef, static: false }) compromissoModalRef: ViewContainerRef;

  constructor(
    public dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _vendedorService: VendedorService,
    private _leadService: LeadService,
    private _faseLeadService: LeadFaseService,
    private _router: Router,
  ) {
    LeadListaPageComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6105', '', 200);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Leads", 6105);

    this.temDireitoInteracao = this._userService.temDireitoAplicativo('WEB6107', '');
    this.temDireitoOportunidade = this._userService.temDireitoAplicativo('WEB6106', 'I');
    this.temDireitoAgenda = this._userService.temDireitoAplicativo('WEB7007','I');

    this._vendedorService.listarPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores; },
      error => { this.vendedoresPermitidos = []; }
    );

    _faseLeadService.listarFases().then(
        fases => { this.fasesLead = fases },
        error => { this.fasesLead = [] }
      );
  }

  ngOnInit() {
    this.anexoForm = this._formBuilder.group({});
    this.anexoDescricaoForm = this._formBuilder.group({});
  }

  ngAfterViewInit(): void {
    let d = JSON.stringify(this.filter.defaultModel);

    let _filtroToken = localStorage.getItem("t.tcw.lead.filtro.token");
    let _token = localStorage.getItem("t.tcw.token");

    let _filtro = localStorage.getItem("t.tcw.lead.filtro");

    if (_filtro && _filtro !== d && _filtroToken === _token) {
      this.filtro = JSON.parse(_filtro) || this.filter.defaultModel;

      this.filtro = { ...this.filter.defaultModel, ...this.filtro };

      if (this.filtro.dataDe) this.filtro.dataDe = new Date(this.filtro.dataDe);
      if (this.filtro.dataAte) this.filtro.dataAte = new Date(this.filtro.dataAte);
    }

    this.filtro.vendedor = JSON.parse(localStorage.getItem("t.tcw.lead.filtro.vendedor"));

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

    this._leadService.ListarEmOrdemDecrescente(currentPage, pageSize, this.filtroString)
      .then(
        leads => {
          this.leads = leads;
          this.paginaAtual = leads.currentPage;
          this.registrosPorPagina = leads.pageSize;
        },
        error => { this.leads = new PagedList<Lead>(); }
      )
      .then(() => {
        this._cdr.detectChanges();
      });
  }

  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';
  vendedorFormatter = (model: Vendedor) => model ? model.codigo + ' - ' + model.nome.toUpperCase() : '';

  faseLeadFormatter = (model: LeadFase): string => model ? (model.descricao).toUpperCase() : '';
  classificacaoLeadFormatter = (model: number): string => {
    if (model === null || model === undefined || isNaN(model)) return '';
    return this.classificacaoLead.includes(model) ? classificacaoLead.filter(e => e.codigo === model)[0].descricao.toUpperCase() : '';
  };
  
  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  labelLeadNumero(lead: Lead): string {
    if (this.isSmallScreen) {
      return lead.numero.toString().padStart(6,'0');
    } else {
      return lead.usina.codigo + '/' + lead.numero.toString().padStart(6,'0') + '-' + lead.ano;
    }
  }

  setFilter(newFilter) {
    //this.filtro = newFilter;
    Object.keys(newFilter).forEach(t => this.filtro[t] = newFilter[t]);

    if (this.filtro.dataDe) this.filtro.dataDe = new Date(this.filtro.dataDe);
    if (this.filtro.dataAte) this.filtro.dataAte = new Date(this.filtro.dataAte);
  }

  labelClienteObra(lead: Lead): string {
    if (this.isSmallScreen) {
      return lead.cliente.substr(0, 13)+'... / ' + lead.obraNome.substr(0, 13)+'...';
    } else {
      return lead.cliente + ' / '+ lead.obraNome;
    }
  }

  getFase(faseCodigo: number): Fase {
    return faseLead.filter(t => t.codigo === faseCodigo)[0];
  }

  getClassificacao(classificacaoCodigo: number): Classificacao {
    if(classificacaoCodigo == 0){
      var classificacao: Classificacao = { codigo: 0, descricao: "", color: "" };
      return classificacao;
    }
    return classificacaoLead.filter(t => t.codigo === classificacaoCodigo)[0];
  }

  filtroChange(novoFiltro: string) {
    this.filtroString = novoFiltro;

    if (this.filtro.dataDe) 
      this.filtro.dataDe = new Date(this.filtro.dataDe);

    if (this.filtro.dataAte) 
      this.filtro.dataAte = new Date(this.filtro.dataAte);

    localStorage.setItem("t.tcw.lead.filtro", JSON.stringify(this.filtro));
    localStorage.setItem("t.tcw.lead.filtro.token", localStorage.getItem("t.tcw.token"))
    this.getPage();
  }

  filtroVendedorChange(vendedor: Vendedor) {
    if (vendedor) {
      localStorage.setItem("t.tcw.lead.filtro.vendedor", JSON.stringify(vendedor));
    }
    else {
      localStorage.removeItem("t.tcw.lead.filtro.vendedor");
    }
  }
  
  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, lead: Lead, confirmCallback: Function, cancelCallback?: Function) {
    let minWidthContainer = this.isSmallScreen ? "95%" : "";

    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    this.item = lead;

    if (container == this.leadLogModalRef) {
      this._leadService.ListarLeadLogsPorId(lead.usina.codigo, lead.ano, lead.numero)
        .then(
          logs => {
            this.logs = logs;
          },
          error => { this.logs = []; }
        )
        .then(() => {
          this._cdr.detectChanges();
        });
    }

    if (container == this.anexosModalRef){
      this._leadService.listarAnexos(this.item.usina.codigo, this.item.ano, this.item.numero)
      .then(
        (anexos) => { this.anexos = anexos; }, 
        (err) => { this.anexos = [] }
      );
      this._cdr.detectChanges();  
    }

    this._dialogRef = this.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer});  
    
    this.modalIsOpen = true;
  }

  closeModal() {
    let self = LeadListaPageComponent.self;

    if (self._dialogRef) self._dialogRef.close();

    self.modalIsOpen = false;
  }

  abrirSeletorDeArquivos(inputFile: HTMLInputElement) {
    let self = LeadListaPageComponent.self;
    var temDireito = self._userService.temDireitoAplicativo('CON0036','I');
    if (!temDireito) {
     self.dialog.open(AlertDialogComponent, {
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
    let self = LeadListaPageComponent.self;
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onloadend = async () => {
        const base64String = reader.result as string;
        try {
          await self._leadService.adicionarAnexo(
            self.item.usina.codigo,
            self.item.ano,
            self.item.numero,
            base64String,
            file.name
          );
          
          self._leadService.listarAnexos(self.item.usina.codigo, self.item.ano, self.item.numero)
          .then(
            (anexos) => { this.anexos = anexos; }, 
            (err) => { this.anexos = [] }
          );
          
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Anexo inserido com sucesso!'
            }
          });

        } catch (err) {
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `${this.formataErrosApi(err)}`
            }
          });
        }
      };
      
      reader.readAsDataURL(file)
    }
  }

  abrirAnexo(anexo: LeadAnexo) {
    let self = LeadListaPageComponent.self;

    self._leadService.ObterAnexo(anexo.id)
    .then(url => {
      var type = url.split(';')[0];
      type = type.replace("data:", "");
      var arquivo = url.split(',')[1]
      Tasks.openBase64File(arquivo, anexo.nome, type)
    }).catch(error => {
      self.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Erro ao obter o Anexo: ${JSON.stringify(error.exceptionMessage)}`
        }
      });
      return;
  });
  }

  atualizarDescricaoAnexo(anexo: LeadAnexo): void {
    let self = LeadListaPageComponent.self;

    self._leadService.atualizarDescricaoAnexo(anexo)
     .then(success => {
      self.closeSubModal();
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Descrição alterada com sucesso!`
        }
      });
     }, err => {
      self.anexo.descricao = self.descricaoAnteriorAnexo;
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro alterar a descrição.\n${JSON.stringify(err.exceptionMessage)}`
        }
      });
     });
  }

  removerAnexo(anexo: LeadAnexo) {
    let self = LeadListaPageComponent.self;

    self._leadService.removerAnexo(anexo.id)
      .then(success => {
        if (success) self.anexos = self.anexos.filter(a => a !== anexo);
      }, err => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });
  }

  showModalDescricaoAnexo(content, container: ViewContainerRef, anexo: LeadAnexo) {
    let self = LeadListaPageComponent.self;
    let minWidthContainer = self.isSmallScreen ? "95%" : "";   

    self.anexo = anexo;
    self.descricaoAnteriorAnexo = anexo.descricao;

    self._subDialogRef = self.dialog.open(content, { viewContainerRef: container, minWidth: minWidthContainer });
    self.subModalIsOpen = true;
  }

  closeSubModal() {
    let self = LeadListaPageComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  cancelSubModal() {
    let self = LeadListaPageComponent.self;

    self.closeSubModal();
    self.anexo.descricao = self.descricaoAnteriorAnexo;
  }

  openInteracaoLead(lead: Lead) {
    var self = LeadListaPageComponent.self;
    self.dialog.open(InteracaoLeadDialogComponent, {
      data: {
        usina: lead.usinaCodigo,
        anoLead: lead.ano,
        numeroLead: lead.numero
      }
    });
  }

  interacaoPermitida(){
    return (this.temDireitoInteracao);
  }

  oportunidadePermitida(){
    return (this.temDireitoOportunidade);
  }

  agendaPermitida(){
    return (this.temDireitoAgenda);
  }
}
