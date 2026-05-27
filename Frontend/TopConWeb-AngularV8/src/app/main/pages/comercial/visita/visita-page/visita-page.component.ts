import { Component, OnInit, ViewChild, ViewContainerRef, ChangeDetectionStrategy,
  OnChanges, SimpleChanges, ChangeDetectorRef, TemplateRef, 
  Input} from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { Observable } from 'rxjs';

// **** CLASSES ****
import { Tasks } from 'app/classes/_tasks/tasks';
import { Vendedor } from 'app/classes/vendedor/vendedor';
// ******************************************************************

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { VendedorService } from 'app/services/vendedor.service';
import { PecaAConcretarService } from 'app/services/peca-a-concretar.service';
import { ParametroService } from 'app/services/parametro.service';
// ******************************************************************
import { Endereco } from 'app/classes/endereco/endereco';
import { ICustomValidator } from '../../../../components/interfaces/custom-validator';
import { CadastroGeral } from 'app/classes/cadastro-geral/cadastro-geral';
import { CadastroGeralService } from 'app/services/cadastro-geral.service';
import { Visita } from 'app/classes/visita/visita';
import { VisitaTipoService } from 'app/services/visita-tipo.service';
import { VisitaTipo } from 'app/classes/visita/visita-tipo';
import { VisitaService } from 'app/services/visita-service';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { time } from 'console';
import { VisitaContato } from 'app/classes/visita/visita-contato';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { Usina } from 'app/classes/usina/usina';

@Component({
  selector: 'app-visita-page',
  templateUrl: './visita-page.component.html',
  styleUrls: ['./visita-page.component.scss']
})
export class VisitaPageComponent implements OnInit {

  public static self: VisitaPageComponent;
  
    geralForm: FormGroup;
    contatoForm: FormGroup;
    
    vendedoresPermitidos: Vendedor[] = [];
    
    visita: Visita = new Visita();
    visitaTipos: VisitaTipo[] = [];
    funcoes: CadastroGeral[] = [];

    private _dialogRef: MatDialogRef<any>;
    temDireitoAlteracao: boolean = true;
    podeAlterarDataProgramacao: boolean = true;

    isInsertMode: boolean = false
  
    constructor(
        private _formBuilder: FormBuilder,
        public dialog: MatDialog,
        private _cdr:ChangeDetectorRef,
        private _userService: UserService,
        private _vendedorService: VendedorService,
        private _pecaAConcretarService: PecaAConcretarService,
        private _route: ActivatedRoute,
        private _router: Router,
        private _parametroService: ParametroService,
        private _cadastroGeralService: CadastroGeralService,
        private _visitaTipoService: VisitaTipoService,
        private _visitaService: VisitaService,
        private location: Location
      ) {
        VisitaPageComponent.self = this;
  
      this.isInsertMode = this._route.routeConfig.path.endsWith('/nova');

      if (this.isInsertMode) {
        var temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6104','I', 200);
        if (!temDireitoInclusao) return;
      } else {
        this.temDireitoAlteracao = this._userService.temDireitoAplicativo('WEB6104','A');
      }
      
      _vendedorService.listarPermitidos().then(
        vendedores => { this.vendedoresPermitidos = vendedores },
        error => { this.vendedoresPermitidos = [] }
      );

      _cadastroGeralService.listarFuncoes().then(
        funcoes => { this.funcoes = funcoes },
        error => { this.funcoes = [] }
      )

      _visitaTipoService.ListarAtivos().then(
        visitas => {  this.visitaTipos = visitas },
        error => { this.visitaTipos = [] }
      )

    }
  
    ngOnInit() {
      this.geralForm = this._formBuilder.group({});
      this.contatoForm = this._formBuilder.group({});

      this.visita = new Visita();

      if(!this.isInsertMode) {

        let idUsina = parseFloat(this._route.snapshot.paramMap.get('idUsina'));
        let ano = parseFloat(this._route.snapshot.paramMap.get('ano'));
        let numero = parseFloat(this._route.snapshot.paramMap.get('numero'));

        if (idUsina && ano && numero) {
          
          this._visitaService.ObterPorId(idUsina, ano, numero, false).then(
            (visita) => {

              this.visita = visita;
              this.visita.horaVisitaString = this.visita.horaVisita.length >= 5 ? this.visita.horaVisita.substring(0, 5).replace(':', '') : this.visita.horaVisita;

              if(!this.visita.contatoPrincipal) this.visita.contatoPrincipal = new VisitaContato();
              if(!this.visita.contatoSecundario) this.visita.contatoSecundario = new VisitaContato();

            },
            (error) => {
              var self = VisitaPageComponent.self;
              self.dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `Erro ao buscar a vista informada.`
                }
              });
            }
          )

        }
        
      } else {

        let savedProgress = this.getSavedProgress();
        if (savedProgress) {
          let cdr = this._cdr;
          setTimeout(() => {
            this.dialog.open(ConfirmDialogComponent, {
              data: {
                title: 'TopConWeb',
                message: 'Existe uma visita já iniciada. Deseja continuar?',
                confirmCallback: () => {
                  this.visita = savedProgress;
                  cdr.detectChanges();
                },
                cancelCallback: () => {
                  this.clearSavedProgress();
                  this.visita = new Visita();
                  this.detectChanges();
                }
              }
            });
          }, 500);
        }
      }

    }
  
    ngOnChanges(changes: SimpleChanges) {
      this._cdr.detectChanges();
    }

    detectChanges(dalay: number = 0) {
      if (dalay)
        setTimeout(() => { this._cdr.detectChanges(); }, dalay);
      else
        this._cdr.detectChanges();
    }

    getSavedProgress(): Visita {
      let pStr: string = localStorage.getItem('t.tcw.novaVisita');
      if (!pStr) return null;
      let p: Visita = JSON.parse(pStr);
      return p;
    }

    saveProgress() {
      if (!this.isInsertMode) return;
      localStorage.setItem('t.tcw.novaVisita', JSON.stringify(this.visita));
    }

    clearSavedProgress() {
      localStorage.removeItem('t.tcw.novaVisita');
    }
  
    vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
    cadastroGeralFormatter = (model: CadastroGeral): string => model ? model.descricao.toUpperCase() : '';
    tipoVisitaFormatter = (model: VisitaTipo): string => model ? (model.codigo + ' - ' + model.descricao).toUpperCase() : '';

    numeroVisitaString(): string {
      return (this.visita.usina ? this.visita.usina.codigo : '0')+'/'+this.visita.numero+'-'+this.visita.ano;
    }

    get horaValidator(): ICustomValidator {
      var _self = VisitaPageComponent.self;
      var _tasks = Tasks;
  
      var message = 'Informe um horário válido';
  
      return {
        key: 'horarioInvalido',
        message: message,
        validatorFunction: (hora: string): boolean => {
          return !(_tasks.horarioValido(hora));
        },
        params: [ _self.visita.horaVisitaString ]
      }
    }

    stringHHMMParaTimeSpan(timeString: string): string {
      const horas = timeString.slice(0, 2); // Extrai as duas primeiras posições como horas
      const minutos = timeString.slice(2);  // Extrai as últimas duas posições como minutos
  
      // Formata para o formato "HH:mm" ou "HH:mm:ss" (com segundos 00)
      return `${horas}:${minutos}:00`;
    }

    formatarHora(date: Date): string {
      const horas = date.getHours().toString().padStart(2, '0'); // Garante que terá dois dígitos
      const minutos = date.getMinutes().toString().padStart(2, '0'); // Garante que terá dois dígitos
  
      return `${horas}${minutos}`;
    }
  

    changeHoraVisita(hora: string) { if(hora.length === 4) this.visita.horaVisita = this.stringHHMMParaTimeSpan(hora); }
    changeVendedor(vendedor: Vendedor) { if(vendedor) this.visita.vendedorCodigo = vendedor.codigo; }
    changeTipoVisita(tipoVisita: VisitaTipo) { if(tipoVisita) this.visita.visitaTipoCodigo = tipoVisita.codigo; }
    changeFuncao(funcao: CadastroGeral, contato: VisitaContato) { if(contato) contato.funcaoCodigo = funcao.codigo; }
    changeNomeCliente(cliente: string) { if(cliente) this.visita.contatoPrincipal.nome = cliente; }
    changeDddTelefone(ddd: number) { if(ddd) this.visita.contatoPrincipal.dddTelefone = ddd; }
    changeDddCelular(ddd: number) { if(ddd) this.visita.contatoPrincipal.dddCelular = ddd; }
    changeTelefone(numero: number) { if(numero) this.visita.contatoPrincipal.telefone = numero; }
    changeCelular(numero: number) { if(numero) this.visita.contatoPrincipal.celular = numero; }
    changeEmail(email: string) { if(email) this.visita.contatoPrincipal.email = email; }
  
    formataValor = Tasks.formataValor;
    formataMoeda = Tasks.formataMoeda;
  
    onGeralNext() { this.saveProgress();  }
    onContatoNext() { this.saveProgress();  }
    
    onGeralPrevious() { this.saveProgress(); }
    onContatoPrevious() { this.saveProgress(); }
  
    async onComplete() {

      let self = VisitaPageComponent.self;

      let request = self.isInsertMode ?
        self._visitaService.Adicionar(self.visita) :
        self._visitaService.Atualizar(self.visita);

      await request.then(
        resultado => {

          var message = self.isInsertMode ? `Visita inserida com sucesso!` : resultado;
    
          self.clearSavedProgress();

          if (self.isInsertMode) {
            self.visita.usina = new Usina();
            self.visita.usinaCodigo = resultado.usinaCodigo;
            self.visita.usina.codigo = self.visita.usinaCodigo;
            self.visita.ano = resultado.ano;
            self.visita.numero = resultado.numero;
          }

          let router = self._router;
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: message,
              afterCloseCallback: async () => {
                router.navigateByUrl("pages/comercial/visita/lista");
              }
            }
          });
        }, error => {
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'OCORREU UM ERRO!'
            }
          });
        });
      
    }
  
    goBack() {
      this.visita = new Visita();
      this.location.back();
    }

    alteracaoNaoPermitida() {

      if(this.isInsertMode)
        return false;

      return (!this.temDireitoAlteracao);

    }

    maskHora = [/\d/, /\d/, ':', /\d/, /\d/];
    maskCEP = [/\d/, /\d/, /\d/, /\d/, /\d/,'-', /\d/, /\d/, /\d/];

    private _cepErrorMessagesDefault = {key: 'minLength', message: 'cep deve conter 8 dígitos!'};
    private _cepErrorMessages: {key: string, message: string}[] = [this._cepErrorMessagesDefault];
    @Input() set cepErrorMessages(value: {key: string, message: string}[]) {
              if (!value) value = [];
              value.push(this._cepErrorMessagesDefault);
              this._cepErrorMessages = value;
           };
           get cepErrorMessages() { return this._cepErrorMessages; };

}
