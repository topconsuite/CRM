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

@Component({
  selector: 'app-historico-interveniente-dialog',
  templateUrl: './historico-interveniente-dialog.component.html',
  styleUrls: ['./historico-interveniente-dialog.component.scss']
})
export class HistoricoIntervenienteDialogComponent implements OnInit, AfterViewInit {
  public static self: HistoricoIntervenienteDialogComponent;

  historicos: PagedList<IntervenienteHistorico> = new PagedList<IntervenienteHistorico>();

  historicosTipos = [ {codigo:1,descricao:"Todos", chave:"TODOS"},{codigo:2,descricao:"CAR", chave:"CAR"},{codigo:3,descricao:"CAP", chave:"CAP"},{codigo:4,descricao:"Interveniente", chave:"INTERV"}];
  historicosTipoSelect = this.historicosTipos[0];

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;
  filtroString: string;

  filtroForm: FormGroup;
  novoHistoricoForm: FormGroup;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  historico: IntervenienteHistorico = new IntervenienteHistorico();
  interveniente: Interveniente = new Interveniente();

  @ViewChild('historicoModalVCR', { read: ViewContainerRef, static: false }) historicoModalRef: ViewContainerRef;

  constructor(public dialogRef: MatDialogRef<HistoricoIntervenienteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      intervenienteCodigo: number
    },
    private _intervenienteService: IntervenienteService,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    ) { }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this._cdr.detectChanges();
      this.filtroString = `filter=$(CodigoInterveniente==${this.data.intervenienteCodigo})`
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
  historicoTipoFormatter = (model: any): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';

  
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

  
    this._intervenienteService.listarHistoricoEmOrdemDescrecente(currentPage, pageSize, this.filtroString)
    .then(
      historicos => {
        this.historicos = historicos;
        this.paginaAtual = historicos.currentPage;
        this.registrosPorPagina = historicos.pageSize;
      },
      error => { this.historicos = new PagedList<IntervenienteHistorico>(); }
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
    var filtroComplemento;
   switch (this.historicosTipoSelect.chave) {
     case "TODOS":
      filtroComplemento = "";
       break;
     case "CAR":
     case "CAP":
      filtroComplemento = `;Vinculo==${this.historicosTipoSelect.descricao}`;
       break;
    case "INTERV":
      filtroComplemento = `;Vinculo==`;
       break;
   }
   var novoFiltro = `filter=$(CodigoInterveniente==${this.data.intervenienteCodigo} ${filtroComplemento})`;
   this.filtroChange(novoFiltro);
  }

  confirmModal: Function;
  cancelModal: Function;
  showModal(content, container: ViewContainerRef, confirmCallback: Function, cancelCallback?: Function) {
    this.confirmModal = confirmCallback;
    this.cancelModal = cancelCallback || this.closeModal;

    this.historico = new IntervenienteHistorico();
    this.historico.codigoInterveniente = this.data.intervenienteCodigo;

    this._intervenienteService.obterPorCodigoInterveniente(this.data.intervenienteCodigo)
    .then(
      interveniente => {
        this.interveniente = interveniente;
      },
      error => { this.interveniente = new Interveniente(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });

    this._dialogRef = this._dialog.open(content, { viewContainerRef: container });

    this.modalIsOpen = true;

  }
  
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
    // let self = HistoricoIntervenienteDialogComponent.self;
    
    if (this._dialogRef) this._dialogRef.close();

    this.novoHistoricoForm.markAsPristine();
    this.novoHistoricoForm.markAsUntouched();

    this.historico = new IntervenienteHistorico();

    this.modalIsOpen = false;
  }

  addHistorico(): void {
    //let self = HistoricoIntervenienteDialogComponent.self;
    this.historico.codigoInterveniente = this.interveniente.codigo;

    this._intervenienteService.adicionarHistoricoInterveniente(this.historico)
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
