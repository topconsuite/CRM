import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { PropostaPropaganda } from 'app/classes/proposta/proposta-propaganda';
import { Usina } from 'app/classes/usina/usina';
import { PropostaService } from 'app/services/proposta.service';
import { UsinaService } from 'app/services/usina.service';
import { AlertDialogComponent } from '../../dialog/alert-dialog/alert-dialog.component';

@Component({
  selector: 'app-proposta-propaganda',
  templateUrl: './proposta-propaganda.component.html',
  styleUrls: ['./proposta-propaganda.component.scss']
})

export class PropostaPropagandaComponent implements OnInit {
  public static self: PropostaPropagandaComponent;

  propagandaForm: FormGroup;
  anexoForm: FormGroup;

  @Output() cancel = new EventEmitter<boolean>();
  @Output() confirm = new EventEmitter<boolean>();

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataDataHora = Tasks.formataDataHora;
  formataErrosApi = Tasks.formataErrosApi;
  maskHora = [/\d/, /\d/, ':', /\d/, /\d/];

  records: PropostaPropaganda[] = [];
  unchecked: boolean = false;

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _propostaService: PropostaService
  ) { }
  
  ngOnInit() {
    this.getRecords();

    this.anexoForm = this._formBuilder.group({});

  }

  getRecords() {
    this._propostaService.ListarPropagandaTodos().then(
      result => { 
        this.records = result; 
        this.initializeForm();
      },
      error => { this.records = [] }
    ).then(() => {
      this._cdr.detectChanges();
    });
  }

  initializeForm() {
    this.records.forEach(element => {
      this.anexoForm.addControl(`ativaCheck_${element.id}`, new FormControl(element.ativa)); // Adiciona um FormControl para cada elemento
    });
  }

  abrirSeletorDeArquivos(inputFile: HTMLInputElement) {
    inputFile.click();
  }
  
  arquivoSelecionado(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onloadend = async () => {
        const base64String = reader.result as string;
        try {
          await this._propostaService.AdicionarPropaganda(base64String, file.name);
          this.getRecords();
          this._dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Anexo inserido com sucesso!'
            }
          });
        } catch (err) {
          this._dialog.open(AlertDialogComponent, {
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

  removeFileExtension(fileName: string): string {
    const lastDotIndex = fileName.lastIndexOf(".");
    
    if (lastDotIndex === -1) {
      return fileName;
    }
    
    return fileName.substring(0, lastDotIndex);
  }
  
  abrirAnexo(anexo: PropostaPropaganda) {
    this._propostaService.ObterPropagandaAnexo(anexo, false)
    .then(url => {
      var type = url.split(';')[0];
      type = type.replace("data:", "");
      var arquivo = url.split(',')[1]
      Tasks.openBase64File(arquivo, anexo.nome, type)
    }).catch(error => {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Erro ao obter o Anexo: ${JSON.stringify(error.exceptionMessage)}`
        }
      });
      return;
  });
  }
  
  removerAnexo(anexo: PropostaPropaganda) {

    this._propostaService.RemoverPropaganda(anexo)
      .then(success => {
        if (success) this.getRecords();
      }, err => {
        this._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `${JSON.stringify(err.exceptionMessage)}`
          }
        });
      });

  }

  cancelModal() {
    this.cancel.emit(true);
  }

  confirmModalCallback(): void {
    this.confirm.emit(true);
  }

  checkBoxChange(evento: any, propaganda: PropostaPropaganda) {
    const checkboxControl = this.anexoForm.get(`ativaCheck_${propaganda.id}`);
    var valorOriginal = propaganda.ativa;

    this.anexoForm.get(`ativaCheck_${propaganda.id}`).setValue(evento.checked);
    propaganda.ativa = evento.checked ? true : false;

    this._propostaService.AtualizarPropaganda(propaganda)
      .then(success => {
        this._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: propaganda.ativa ? 'Propaganda ativa!' : 'Propaganda desativada!'
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
        checkboxControl.setValue(valorOriginal);
        propaganda.ativa = valorOriginal;
      })
      .finally(() => {
        this._cdr.detectChanges();
      });
  }
}
