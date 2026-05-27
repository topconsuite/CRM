import { Component, OnInit, AfterViewInit, Inject, ChangeDetectorRef, ViewContainerRef, ViewChild } from '@angular/core';
import { PagedList } from 'app/classes/pagination/paged-list';
import { Programacao } from 'app/classes/programacao/programacao';
import { IntervenienteHistorico } from 'app/classes/interveniente/interveniente-historico';
import { IntervenienteService } from 'app/services/interveniente.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Interveniente } from 'app/classes/bomba/bomba.classes';
import { AlertDialogComponent } from '../alert-dialog/alert-dialog.component';
import { ICustomValidator } from '../../interfaces/custom-validator';
import { EVisitaHistoricoTipo, VisitaHistorico, VisitaHistoricoTipos } from 'app/classes/visita/visita-historico';
import { VisitaService } from 'app/services/visita-service';
import { UserService } from 'app/services/user.service';

@Component({
  selector: 'app-historico-visita-dialog',
  templateUrl: './historico-visita-dialog.component.html',
  styleUrls: ['./historico-visita-dialog.component.scss']
})
export class HistoricoVisitaDialogComponent implements OnInit, AfterViewInit {
  public static self: HistoricoVisitaDialogComponent;

  historicos: PagedList<VisitaHistorico> = new PagedList<VisitaHistorico>();

  historicosTipos = VisitaHistoricoTipos;
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

  historico: VisitaHistorico = new VisitaHistorico();
  interveniente: Interveniente = new Interveniente();

  temDireitoInsercao: boolean = true;

  @ViewChild('historicoModalVCR', { read: ViewContainerRef, static: false }) historicoModalRef: ViewContainerRef;

  constructor(public dialogRef: MatDialogRef<HistoricoVisitaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      usina: number,
      numeroVisita: number,
      anoVisita: number
    },
    private _visitaService: VisitaService,
    private _userService: UserService,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    ) {
      this.temDireitoInsercao = this._userService.temDireitoAplicativo('WEB6108', 'I');
    }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this._cdr.detectChanges();
      this.filtroString = `filter=$(usina==${this.data.usina};anoVisita==${this.data.anoVisita};numeroVisita==${this.data.numeroVisita})`
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
  
  get IntervenienteString(): string {
    return (this.interveniente.codigo+' - '+this.interveniente.nome).toUpperCase();
  }

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
  
    this._visitaService.ListarHistorico(currentPage, pageSize, this.filtroString)
    .then(
      historicos => {
        this.historicos = historicos;
        this.paginaAtual = historicos.currentPage;
        this.registrosPorPagina = historicos.pageSize;
      },
      error => { this.historicos = new PagedList<VisitaHistorico>(); }
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
    if(this.historicosTipoSelect.codigo == EVisitaHistoricoTipo.Todos)
      filtroTipo = "";

    var novoFiltro = `filter=$(usina==${this.data.usina};anoVisita==${this.data.anoVisita};numeroVisita==${this.data.numeroVisita}${filtroTipo})`
    this.filtroChange(novoFiltro);

  }

  getTextoTipo(tipoHistorico: EVisitaHistoricoTipo) {

    return this.historicosTipos.filter(t => t.codigo == tipoHistorico)[0].descricao.toUpperCase();

  }

  getTextoDescricao(tipoHistorico: EVisitaHistoricoTipo) {

    if(tipoHistorico == EVisitaHistoricoTipo.AtividadeRealizada)
      return "Atividade realizada";

    if(tipoHistorico == EVisitaHistoricoTipo.InteracaoCliente)
      return "Interação com cliente";

    if(tipoHistorico == EVisitaHistoricoTipo.Chamada)
      return "Assunto";

    return "Descrição";

  }

  exibirRetorno(tipoHistorico: EVisitaHistoricoTipo) {
    return tipoHistorico == EVisitaHistoricoTipo.Chamada;
  }

  insercaoNaoPermitida() {
    return (!this.temDireitoInsercao);
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function) {

    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    if(this.historicosTipoSelect.codigo != EVisitaHistoricoTipo.Todos)
      this.historicoTipoCadastro = this.historicosTipoSelect;
    
    if(this.historicosTipoSelect.codigo == EVisitaHistoricoTipo.Todos)
      this.historicoTipoCadastro = this.historicosTipos[1];

    this.historico = new VisitaHistorico();

    this.historico.usina = this.data.usina;
    this.historico.anoVisita = this.data.anoVisita;
    this.historico.numeroVisita = this.data.numeroVisita;
    this.historico.tipoHistorico = this.historicoTipoCadastro.codigo;

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
    // let self = HistoricoVisitaDialogComponent.self;
    
    if (this._dialogRef) this._dialogRef.close();

    this.novoHistoricoForm.markAsPristine();
    this.novoHistoricoForm.markAsUntouched();

    this.historico = new VisitaHistorico();

    this.modalIsOpen = false;
  }

  addHistorico(): void {

    this.historico.usina = this.data.usina;
    this.historico.anoVisita = this.data.anoVisita;
    this.historico.numeroVisita = this.data.numeroVisita;
    this.historico.tipoHistorico = this.historicoTipoCadastro.codigo;

    this._visitaService.adicionarHistorico(this.historico)
    .then(success => {
      this.closeModal();
      this.getPage();
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Histórico adicionado com sucesso!`
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
