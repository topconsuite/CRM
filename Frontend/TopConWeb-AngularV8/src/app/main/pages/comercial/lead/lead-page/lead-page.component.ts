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
  import { LeadFaseService } from 'app/services/lead-fase.service';
  // ******************************************************************
  import { Endereco } from 'app/classes/endereco/endereco';
  import { LeadFase } from 'app/classes/lead/lead-fase';
  import { CadastroGeral } from 'app/classes/cadastro-geral/cadastro-geral';
  import { CadastroGeralService } from 'app/services/cadastro-geral.service';
  import { Classificacao, classificacaoLead, EFaseLead, faseLead, Lead } from 'app/classes/lead/lead';
  import { LeadContato } from 'app/classes/lead/lead-contato';
  import { MotivoPerda } from 'app/classes/motivo-perda/motivo-perda';
  import { MotivoPerdaService } from 'app/services/motivo-perda.service';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { LeadService } from 'app/services/lead.service';
import { Usina } from 'app/classes/usina/usina';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { error } from 'console';
import { VisitaService } from 'app/services/visita-service';
  
  @Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-lead-page',
    templateUrl: './lead-page.component.html',
    styleUrls: ['./lead-page.component.scss']
  })
  export class LeadPageComponent implements OnInit, OnChanges {
  
    public static self: LeadPageComponent;
  
    geralForm: FormGroup;
    contatoForm: FormGroup;
    
    vendedoresPermitidos: Vendedor[] = [];
    fasesLead: LeadFase[] = [];
    viasCaptacao: CadastroGeral[] = [];
    motivosPerda: MotivoPerda[] = [];
    
    funcoes: CadastroGeral[] = [];

    lead: Lead = new Lead();
    isInsertMode: boolean = true;

    get classificacaoLead(): number[] {
      let codigos: number[] = [];
      classificacaoLead.forEach(classificacao => {
        codigos.push(classificacao.codigo);
      })
      return codigos;
    };

    private _dialogRef: MatDialogRef<any>;
    temDireitoAlteracao: boolean = true;
  
    constructor(
        private _formBuilder: FormBuilder,
        public dialog: MatDialog,
        private _cdr:ChangeDetectorRef,
        private _userService: UserService,
        private _vendedorService: VendedorService,
        private _route: ActivatedRoute,
        private _router: Router,
        private _faseLeadService: LeadFaseService,
        private _leadService: LeadService,
        private _cadastroGeralService: CadastroGeralService,
        private _motivoPerdaService: MotivoPerdaService,
        private _visitaService: VisitaService,
        private location: Location
      ) {
      LeadPageComponent.self = this;
  
      if (this._route.routeConfig.path.endsWith('/novo')) {
        var temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6105','I', 200);
        if (!temDireitoInclusao) return;
      } else {
        this.temDireitoAlteracao = this._userService.temDireitoAplicativo('WEB6105','A');
      }
      
      _vendedorService.listarPermitidos().then(
        vendedores => { this.vendedoresPermitidos = vendedores },
        error => { this.vendedoresPermitidos = [] }
      );

      _faseLeadService.listarFases().then(
        fases => { 
          this.fasesLead = fases;

          if(this.lead.fase.codigo == 0) {

            var faseLead = this.fasesLead.filter(t => t.descricao == 'Identificação')[0];
    
            if(faseLead) {
              this.lead.fase.codigo = faseLead.codigo;
              this.lead.fase = faseLead;
            }
          }
        },
        error => { this.fasesLead = [] }
      );

      _cadastroGeralService.listarViasCaptacao().then(
        viasCaptacao => { this.viasCaptacao = viasCaptacao },
        error => { this.viasCaptacao = [] }
      );

      _motivoPerdaService.listarMotivosAtivos().then(
        motivosPerda => { this.motivosPerda = motivosPerda },
        error => { this.motivosPerda = [] }
      );

      _cadastroGeralService.listarFuncoes().then(
        funcoes => { this.funcoes = funcoes },
        error => { this.funcoes = [] }
      );
    }
  
    ngOnInit() {
      this.geralForm = this._formBuilder.group({});
      this.contatoForm = this._formBuilder.group({});

      let idUsina = parseFloat(this._route.snapshot.paramMap.get('idUsina'));
      let ano = parseFloat(this._route.snapshot.paramMap.get('ano'));
      let numero = parseFloat(this._route.snapshot.paramMap.get('numero'));

      if (idUsina && ano && numero) {

        var gerarLeadDeVisita = (this._route.routeConfig.path.endsWith('/gerar-lead'))

        if(!gerarLeadDeVisita) {
          this._leadService.ObterPorUsinaAnoNumero({codigo: idUsina, nome: '', sigla: '', filialCodigo: 0, tempoBtAteAObra: 0 ,tempoBtNaObra: 0
            , porcentagemRetornoObra: 0, prazoPesagem:0}, ano, numero).then(
              lead => {
                this.isInsertMode = false;
                this.lead = lead;
  
                this.detectChanges();
              },
              error => {
                this.lead = new Lead();
                this.detectChanges();
              }
            )
            .then(() => { this.detectChanges(500); });
        }

        if(gerarLeadDeVisita) {
          this._visitaService.obterLeadDeVisita(idUsina, ano, numero).then(
              lead => {
                this.isInsertMode = true;
                this.lead = lead;
  
                this.detectChanges();
              },
              error => {
                this.dialog.open(AlertDialogComponent, {
                  disableClose: true,
                  data: {
                    title: 'TopConWeb',
                    message: Tasks.formataErrosApi(error)
                  }
                });
                this.detectChanges();
              }
            )
            .then(() => { this.detectChanges(500); });
        }
        
      } else {
        let savedProgress = this.getSavedProgress();
        if (savedProgress) {
          let cdr = this._cdr;
          setTimeout(() => {
            this.dialog.open(ConfirmDialogComponent, {
              data: {
                title: 'TopConWeb',
                message: 'Existe uma lead já iniciada. Deseja continuar?',
                confirmCallback: () => {
                  this.lead = savedProgress;
                  cdr.detectChanges();
                },
                cancelCallback: () => {
                  this.clearSavedProgress();
                  this.lead = new Lead();
                  this.detectChanges();
                }
              }
            });
          }, 500);
        } else {
          this.lead = new Lead();
          this.detectChanges();
        }
      }
    }
  
    ngOnChanges(changes: SimpleChanges) {
      this._cdr.detectChanges();
    }
    
    detectChanges(delay: number = 0) {
      if (delay)
        setTimeout(() => { this._cdr.detectChanges(); }, delay);
      else
        this._cdr.detectChanges();
    }

    vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
    faseLeadFormatter = (model: LeadFase): string => model ? (model.descricao).toUpperCase() : '';
    classificacaoLeadFormatter = (model: number): string => {
      if (model === null || model === undefined || isNaN(model)) return '';
      return this.classificacaoLead.includes(model) ? classificacaoLead.filter(e => e.codigo === model)[0].descricao.toUpperCase() : '';
    };
    cadastroGeralFormatter = (model: CadastroGeral): string => model ? model.descricao.toUpperCase() : '';
    motivoPerdaFormatter = (model: MotivoPerda): string => model ? (model.descricao).toUpperCase() : '';

    numeroLeadString(): string {
      return (this.lead.usina ? this.lead.usina.codigo : '0')+'/'+this.lead.numero+'-'+this.lead.ano;
    }  
    
    isFechadoPerdido(): boolean {
      if (!this.lead.fase) return false;

      return this.lead.fase.codigo === EFaseLead.FechadoPerdido;
    }

    getSavedProgress(): Lead {
      let lStr: string = localStorage.getItem('t.tcw.novaLead');
      if (!lStr) return null;
      let l: Lead = JSON.parse(lStr);
      return l;
    }

    saveProgress() {
      if (!this.isInsertMode) return;
      localStorage.setItem('t.tcw.novaLead', JSON.stringify(this.lead));
    }

    clearSavedProgress() {
      localStorage.removeItem('t.tcw.novaLead');
    }

    changeNomeCliente(cliente: string) { if(cliente) this.lead.contatoPrincipal.nome = cliente; }
    changeDddTelefone(ddd: number) { if(ddd) this.lead.contatoPrincipal.ddd = ddd; }
    changeDddCelular(ddd: number) { if(ddd) this.lead.contatoPrincipal.dddCelular = ddd; }
    changeTelefone(numero: number) { if(numero) this.lead.contatoPrincipal.telefone = numero; }
    changeCelular(numero: number) { if(numero) this.lead.contatoPrincipal.celular = numero; }
    changeEmail(email: string) { if(email) this.lead.contatoPrincipal.email = email; }

    formataValor = Tasks.formataValor;
    formataMoeda = Tasks.formataMoeda;
  
    onGeralNext() { this.saveProgress(); }
    onContatoNext() { this.saveProgress(); }
    
    onGeralPrevious() { this.saveProgress(); }
    onContatoPrevious() { this.saveProgress(); }
  
    async onComplete() {
      let self = LeadPageComponent.self;

      let request = self.isInsertMode ?
        self._leadService.Adicionar(self.lead) :
        self._leadService.Atualizar(self.lead);

        await request.then(response => {
          var message = self.isInsertMode ? `Lead inserida com sucesso!` : response;

          self.clearSavedProgress();

          if (self.isInsertMode) {
            self.lead.usina = new Usina();
            self.lead.usina.codigo = response.usinaCodigo;
            self.lead.ano = response.ano;
            self.lead.numero = response.numero;
          }
          
          let router = self._router;
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: message,
              afterCloseCallback: async () => {
                router.navigateByUrl("pages/comercial/lead/lista");
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

    alteracaoNaoPermitida() {
      if(this.isInsertMode)
        return false;

      return (!this.temDireitoAlteracao);
    }

    maskCEP = [/\d/, /\d/, /\d/, /\d/, /\d/,'-', /\d/, /\d/, /\d/];

    private _cepErrorMessagesDefault = {key: 'minLength', message: 'cep deve conter 8 dígitos!'};
    private _cepErrorMessages: {key: string, message: string}[] = [this._cepErrorMessagesDefault];
    @Input() set cepErrorMessages(value: {key: string, message: string}[]) {
              if (!value) value = [];
              value.push(this._cepErrorMessagesDefault);
              this._cepErrorMessages = value;
           };
           get cepErrorMessages() { return this._cepErrorMessages; };


    get faseLeadColors(): string[]{
      let colors: string[] = [];
      faseLead.forEach(fase => {
        colors.push(fase.color);
      });
      return colors;
    }

    get classificacaoLeadColors(): string[]{
      let colors: string[] = [];
      classificacaoLead.forEach(classificacao => {
        colors.push(classificacao.color);
      });
      return colors;
    }
  }
  