import { Component, OnInit, AfterViewInit, Inject, ChangeDetectorRef, ViewContainerRef, ViewChild } from '@angular/core';
import { PagedList } from 'app/classes/pagination/paged-list';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AlertDialogComponent } from '../alert-dialog/alert-dialog.component';
import { ICustomValidator } from '../../interfaces/custom-validator';
import { ELeadInteracaoTipo, LeadInteracao, LeadInteracaoTipos } from 'app/classes/lead/lead-interacao';
import { LeadService } from 'app/services/lead.service';
import { UserService } from 'app/services/user.service';

@Component({
  selector: 'app-interacao-lead-dialog',
  templateUrl: './interacao-lead-dialog.component.html',
  styleUrls: ['./interacao-lead-dialog.component.scss']
})
export class InteracaoLeadDialogComponent implements OnInit, AfterViewInit {
  public static self: InteracaoLeadDialogComponent;

  historicos: PagedList<LeadInteracao> = new PagedList<LeadInteracao>();

  historicosTipos = LeadInteracaoTipos;
  historicosTipoSelect = this.historicosTipos[0];
  historicosTipoParaCadastro = this.historicosTipos.filter(t => t.codigo < 99);

  historicoTipoCadastro = this.historicosTipos[1];

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;
  filtroString: string;

  filtroForm: FormGroup;
  novoHistoricoForm: FormGroup;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  historico: LeadInteracao = new LeadInteracao();

  temDireitoInsercao: boolean = true;
  
  @ViewChild('historicoModalVCR', { read: ViewContainerRef, static: false }) historicoModalRef: ViewContainerRef;

  constructor(public dialogRef: MatDialogRef<InteracaoLeadDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      usina: number,
      numeroLead: number,
      anoLead: number
    },
    private _leadService: LeadService,
    private _userService: UserService,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    ) {

      this.temDireitoInsercao = this._userService.temDireitoAplicativo('WEB6107', 'I');
    }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this._cdr.detectChanges();
      this.filtroString = `filter=$(usina==${this.data.usina};anoLead==${this.data.anoLead};numeroLead==${this.data.numeroLead})`
      this.getPage();
     });
  }
  ngOnInit(
  ) {
    this.filtroForm = this._formBuilder.group({});
    this.novoHistoricoForm = this._formBuilder.group({});
  }

  formataData = Tasks.formataData;
  formataDataNullavel = Tasks.formataDataNullavel;
  formataErrosApi = Tasks.formataErrosApi;
  historicoTipoFormatter = (model: any): string => model ? (model.codigo == 99 ? model.descricao : model.codigo + ' - ' + model.descricao).toUpperCase() : '';

  maskHora = [/\d/, /\d/, ':', /\d/, /\d/];

  closeDialog() {
    this.dialogRef.close();
  }

  getPage(pageInfo?) {

    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;

    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    }
    
    this._leadService.listarInteracoes(currentPage, pageSize, this.filtroString)
    .then(
      historicos => {
        this.historicos = historicos;
        this.paginaAtual = historicos.currentPage;
        this.registrosPorPagina = historicos.pageSize;
      },
      error => { this.historicos = new PagedList<LeadInteracao>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });

  }

  currentPage() {
    if (this.historicos.currentPage <= 0) return 0;
    return this.historicos.currentPage - 1;
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  setFiltroTipoHistorico() { 

    var filtroTipo = `;tipo==${this.historicosTipoSelect.sigla}`;
    if(this.historicosTipoSelect.codigo == ELeadInteracaoTipo.Todos)
      filtroTipo = "";

    var novoFiltro = `filter=$(usina==${this.data.usina};anoLead==${this.data.anoLead};numeroLead==${this.data.numeroLead}${filtroTipo})`
    this.filtroChange(novoFiltro);

  }

  getTextoTipo(tipoHistorico: string) {

    return this.historicosTipos.filter(t => t.sigla == tipoHistorico)[0].descricao.toUpperCase();

  }

  getTextoDescricao(tipoHistorico: string) {

    if(tipoHistorico == "ATIVIDADE")
      return "Atividade realizada";

    if(tipoHistorico == "INTERACAO")
      return "Interação com cliente";

    if(tipoHistorico == "CHAMADA")
      return "Assunto";

    return "Descrição";

  }

  exibirRetorno(tipo: string) {
    return tipo == "CHAMADA";
  }

  insercaoNaoPermitida() {
    return (!this.temDireitoInsercao);
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function) {

    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if(this.historicosTipoSelect.codigo != ELeadInteracaoTipo.Todos)
      this.historicoTipoCadastro = this.historicosTipoSelect;
    
    if(this.historicosTipoSelect.codigo == ELeadInteracaoTipo.Todos)
      this.historicoTipoCadastro = this.historicosTipos[1];

    this.historico = new LeadInteracao();

    this.historico.usina = this.data.usina;
    this.historico.anoLead = this.data.anoLead;
    this.historico.numeroLead = this.data.numeroLead;
    this.historico.tipo = this.historicoTipoCadastro.sigla;

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;

  }

  stringHHMMParaTimeSpan(timeString: string): string {
    const horas = timeString.slice(0, 2); // Extrai as duas primeiras posições como horas
    const minutos = timeString.slice(2);  // Extrai as últimas duas posições como minutos

    // Formata para o formato "HH:mm" ou "HH:mm:ss" (com segundos 00)
    return `${horas}:${minutos}:00`;
  }

  changeHora(hora: string) { if(hora.length === 4) this.historico.hora = this.stringHHMMParaTimeSpan(hora); }
  changeHoraRetorno(hora: string) { if(hora.length === 4) this.historico.horaRetorno = this.stringHHMMParaTimeSpan(hora); }
  
  horaValidator(hora: string): ICustomValidator {

    var _tasks = Tasks;

    var message = 'Informe um horário válido';

    if (hora === '') {
      return;
    }

    return {
      key: 'horarioInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        return hora !== '' ? !_tasks.horarioValido(hora) : true;
      },
      params: [hora]
    }
  }

  lenghtValidator(text: string) {
    var message = 'Limite de 500 caracteres ultrapassado';

    if (text.length <= 500) {
      return;
    }

    return {
      key: 'limiteUltrapassado',
      message: message,
      validatorFunction: (text: string): boolean => {
        return text.length <= 500 ? false : true;
      },
      params: [text]
    }
  }

  remainingCharacters(text: string) {
    return (text.length <= 500) ? (500 - text.length) + " caracteres restantes" : "";
  }
  
  closeModal() {
    
    if (this._dialogRef) this._dialogRef.close();

    this.novoHistoricoForm.markAsPristine();
    this.novoHistoricoForm.markAsUntouched();

    this.historico = new LeadInteracao();

    this.modalIsOpen = false;
  }

  addHistorico(): void {

    this.historico.usina = this.data.usina;
    this.historico.anoLead = this.data.anoLead;
    this.historico.numeroLead = this.data.numeroLead;
    this.historico.tipo = this.historicoTipoCadastro.sigla;

    this._leadService.adicionarInteracao(this.historico)
    .then(success => {
      this.closeModal();
      this.getPage();
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Interação adicionada com sucesso!`
        }
      });
    }, err => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `${this.formataErrosApi(err)}`
        }
      });
    });

  }

}
