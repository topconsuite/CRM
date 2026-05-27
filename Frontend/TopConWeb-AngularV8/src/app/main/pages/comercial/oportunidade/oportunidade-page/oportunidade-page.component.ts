import { ChangeDetectorRef, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Tasks } from 'app/classes/_tasks/tasks';
import { CadastroGeral } from 'app/classes/cadastro-geral/cadastro-geral';
import { MotivoPerda } from 'app/classes/motivo-perda/motivo-perda';
import { Concorrente } from 'app/classes/oportunidade/concorrente';
import { ClassificacaoTemperaturas, EFaseOportunidade, FasesObra, Oportunidade } from 'app/classes/oportunidade/oportunidade';
import { OportunidadeContato } from 'app/classes/oportunidade/oportunidade-contato';
import { OportunidadeFase } from 'app/classes/oportunidade/oportunidade-fase';
import { OportunidadeTipo } from 'app/classes/oportunidade/oportunidade-tipo';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { Usina } from 'app/classes/usina/usina';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { ICustomValidator } from 'app/main/components/interfaces/custom-validator';
import { CadastroGeralService } from 'app/services/cadastro-geral.service';
import { ConcorrenteService } from 'app/services/concorrente.service';
import { MotivoPerdaService } from 'app/services/motivo-perda.service';
import { OportunidadeTipoService } from 'app/services/oportunidade-tipo.service';
import { OportunidadeService } from 'app/services/oportunidade.service';
import { ParametroService } from 'app/services/parametro.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { UserService } from 'app/services/user.service';
import { VendedorService } from 'app/services/vendedor.service';
import { Location } from '@angular/common';
import { Interveniente, intervenienteTipos } from 'app/classes/interveniente/interveniente';
import { IntervenienteService } from 'app/services/interveniente.service';
import { UsinaService } from 'app/services/usina.service';
import { EnderecoService } from 'app/services/endereco.service';
import { LeadService } from 'app/services/lead.service';
import { title } from 'process';

@Component({
  selector: 'app-oportunidade-page',
  templateUrl: './oportunidade-page.component.html',
  styleUrls: ['./oportunidade-page.component.scss']
})
export class OportunidadePageComponent implements OnInit {

  public static self: OportunidadePageComponent;
  
    geralForm: FormGroup;
    obraForm: FormGroup;
    contatoForm: FormGroup;

    intervenienteTipo: string = 'F';
    intervenienteCpfCnpj: string = "";
    intervenientes: Interveniente[] = [];
    intervenienteSelecionado: Interveniente = new Interveniente();
    
    vendedoresPermitidos: Vendedor[] = [];
    funcoes: CadastroGeral[] = [];
    viaCaptacoes: CadastroGeral[] = [];
    motivoPerdas: MotivoPerda[] = [];
    concorrentes: Concorrente[] = [];
    oportunidadeTipos: OportunidadeTipo[] = [];
    segmentacoes: Segmentacao[] = [];
    fases: OportunidadeFase[] = [];
    classificacoes = ClassificacaoTemperaturas;
    obraPortes: CadastroGeral[] = [];
    obraFases = FasesObra;
    usinas: Usina[] = [];

    classificacaoSelecionada = this.classificacoes[2];
    obraFaseSelecionada: any;
    
    oportunidade: Oportunidade = new Oportunidade();

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
        private _route: ActivatedRoute,
        private _router: Router,
        private _parametroService: ParametroService,
        private _cadastroGeralService: CadastroGeralService,
        private _oportunidadeService: OportunidadeService,
        private _oportunidadeTipoService: OportunidadeTipoService,
        private _motivoPerdaService: MotivoPerdaService,
        private _concorrenteService: ConcorrenteService,
        private _segmentacaoService: SegmentacaoService,
        private _intervenienteService: IntervenienteService,
        private _usinaService: UsinaService,
        private _enderecoService: EnderecoService,
        private _leadService: LeadService,
        private location: Location
      ) {
        OportunidadePageComponent.self = this;
  
      this.isInsertMode = this._route.routeConfig.path.endsWith('/nova');

      if (this.isInsertMode) {
        var temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6106','I', 200);
        if (!temDireitoInclusao) return;
      } else {
        this.temDireitoAlteracao = this._userService.temDireitoAplicativo('WEB6106','A');
      }
      
      _vendedorService.listarPermitidos().then(
        vendedores => { this.vendedoresPermitidos = vendedores },
        error => { this.vendedoresPermitidos = [] }
      );

      _cadastroGeralService.listarFuncoes().then(
        funcoes => { this.funcoes = funcoes },
        error => { this.funcoes = [] }
      )

      _cadastroGeralService.listarViasCaptacao().then(
        viaCaptacoes => { this.viaCaptacoes = viaCaptacoes },
        error => { this.viaCaptacoes = [] }
      )
      
      _oportunidadeTipoService.ListarAtivos().then(
        tipos => { this.oportunidadeTipos = tipos },
        error => { this.oportunidadeTipos = []; }
      )

      _motivoPerdaService.listarMotivosAtivos().then(
        motivosPerda => { this.motivoPerdas = motivosPerda },
        error => { this.motivoPerdas = [] }
      );

      _concorrenteService.ListarAtivos().then(
        concorrentes => { this.concorrentes = concorrentes },
        error => { this.concorrentes = [] }
      );

      _segmentacaoService.listarTodos().then(
        segmentacoes => { 
          this.segmentacoes = segmentacoes 
          if(this.oportunidade.segmentacaoCodigo == 0) {
            this.oportunidade.segmentacao = segmentacoes[0];
            this.oportunidade.segmentacaoCodigo = segmentacoes[0].id;
          }
        },
        error => { this.segmentacoes = [] }
      );

      _oportunidadeService.ListarFases().then(
        fases => { 
          this.fases = fases;

          if(this.oportunidade.faseCodigo == 0) {

            var faseQualificacao = this.fases.filter(t => t.descricao == 'Qualificação')[0];
    
            if(faseQualificacao) {
              this.oportunidade.faseCodigo = faseQualificacao.codigo;
              this.oportunidade.fase = faseQualificacao;
            }
    
          }

        },
        error => { this.fases = [] }
      );

      _cadastroGeralService.listarPorteObra().then(
        obraPortes => { this.obraPortes = obraPortes },
        error => { this.obraPortes = [] }
      );

      _usinaService.listarListarUsinasPermitidasUsuario()
      .then(
        usinas => { this.usinas = usinas },
        error => { this.usinas = [] }
      );

      

    }
  
    ngOnInit() {
      this.geralForm = this._formBuilder.group({});
      this.contatoForm = this._formBuilder.group({});
      this.obraForm = this._formBuilder.group({});

      this.oportunidade = new Oportunidade();

      if(!this.isInsertMode) {

        let idUsina = parseFloat(this._route.snapshot.paramMap.get('idUsina'));
        let ano = parseFloat(this._route.snapshot.paramMap.get('ano'));
        let numero = parseFloat(this._route.snapshot.paramMap.get('numero'));

        if (idUsina && ano && numero) {
          
          var gerarOportunidadeDeLead = (this._route.routeConfig.path.endsWith('/gerar-oportunidade'));

          if (!gerarOportunidadeDeLead) {
            this._oportunidadeService.ObterPorId(idUsina, ano, numero, false).then(
              (oportunidade) => {
  
                this.oportunidade = oportunidade;
                this.obraFaseSelecionada = this.obraFases.filter(t => t.codigo == oportunidade.obraFase)[0];
                this.classificacaoSelecionada = this.classificacoes.filter(t => t.codigo == oportunidade.classificacao)[0];
  
                if(!this.oportunidade.contatoPrincipal) this.oportunidade.contatoPrincipal = new OportunidadeContato();
                if(!this.oportunidade.contatoSecundario) this.oportunidade.contatoSecundario = new OportunidadeContato();
  
              },
              (error) => {
                var self = OportunidadePageComponent.self;
                self.dialog.open(AlertDialogComponent, {
                  disableClose: true,
                  data: {
                    title: 'TopConWeb',
                    message: `Erro ao buscar a oportunidade informada.`
                  }
                });
              }
            )
          } else {
            this._leadService.obterOportunidadeDeLead(idUsina, ano, numero).then(
              oportunidade => {
                this.isInsertMode = true;
                this.oportunidade = oportunidade
                this.oportunidade.faseCodigo = this.oportunidade.fase.codigo;
                this.classificacaoSelecionada = this.classificacoes.filter(t => t.codigo === this.oportunidade.classificacao)[0];
                this.oportunidade.classificacao = this.classificacaoSelecionada.codigo;
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
                this.detectChanges()
              }
            )
            .then(() => { this.detectChanges(500); });
          }
        }
        
      } else {

        let savedProgress = this.getSavedProgress();
        if (savedProgress) {
          let cdr = this._cdr;
          setTimeout(() => {
            this.dialog.open(ConfirmDialogComponent, {
              data: {
                title: 'TopConWeb',
                message: 'Existe uma oportunidade já iniciada. Deseja continuar?',
                confirmCallback: () => {
                  this.oportunidade = savedProgress;
                  cdr.detectChanges();
                },
                cancelCallback: () => {
                  this.clearSavedProgress();
                  this.oportunidade = new Oportunidade();
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

    getSavedProgress(): Oportunidade {
      let pStr: string = localStorage.getItem('t.tcw.novaOportunidade');
      if (!pStr) return null;
      let p: Oportunidade = JSON.parse(pStr);
      return p;
    }

    get intervenienteTipos(): string[] {
      let codigos: string[] = [];
      intervenienteTipos.forEach(intervTipo => {
        codigos.push(intervTipo.codigo);
      })
      return codigos;
    };

    get statusOportunidadeColors(): string[] {
      let colors: string[] = [];
      this.fases.forEach(status => {
        colors.push(status.cor);
      });
      return colors;
    };

    get classificacaoOportunidadeColors(): string[] {
      let colors: string[] = [];
      this.classificacoes.forEach(status => {
        colors.push(status.color);
      });
      return colors;
    };

    get intervenienteValidator(): ICustomValidator {
      var _self = OportunidadePageComponent.self;
      var _tasks = Tasks;
      var interveniente = _self.intervenienteSelecionado;
  
      var message = 'Cliente Bloqueado: '
        + (interveniente && interveniente.bloqueioMotivo ? interveniente.bloqueioMotivo.descricao + ' ' : '')
        + (interveniente ? interveniente.bloqueioObservacao : '');
  
      return {
        key: 'clienteBloqueado',
        message: message,
        validatorFunction: (interveniente: Interveniente, cpfCnpj: string): boolean => {
          return (cpfCnpj.length===11 || cpfCnpj.length===14)
            && interveniente && interveniente.bloqueioMotivo
            && cpfCnpj===interveniente.cpfCnpj;
        },
        params: [_self.intervenienteSelecionado, _self.oportunidade.cliente]
      }
    }

    SetClienteCodigo(codigo: number) {
    
      if (codigo !== this.oportunidade.intervenienteCodigo) {
        this.oportunidade.intervenienteCodigo = codigo;
  
        this._intervenienteService.obterPorCodigoInterveniente(codigo)
        .then(interveniente => {

          if (interveniente) {
            
            this.intervenienteTipo = interveniente.intervenienteTipo;
            this.intervenienteCpfCnpj = interveniente.cpfCnpj;
            this.intervenienteSelecionado = interveniente;
            this.oportunidade.intervenienteCodigo = interveniente.codigo;
            this.oportunidade.cliente = interveniente.razao;
            this.oportunidade.dddTelefone = interveniente.telefoneDdd;
            this.oportunidade.telefone = interveniente.telefoneNumero;
            this.oportunidade.dddCelular = interveniente.celularDdd;
            this.oportunidade.celular = interveniente.celularNumero;
            this.oportunidade.email = interveniente.email;

            this._cdr.detectChanges();
  
          } else {
  
            this.oportunidade.intervenienteCodigo = 0;
            if (this.intervenienteSelecionado) this.intervenienteSelecionado.codigo = 0;
            else this.intervenienteSelecionado = new Interveniente();
  
          }
        },error => { 
          var self = OportunidadePageComponent.self;

          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Não foi possível carregar os dados do Cliente!`
            }
          });
          this.oportunidade.intervenienteCodigo = 0;
          this.oportunidade.cliente = '';
        });
        
      }
      
    }

    setCpfCnpj(cpfCnpj: string) {
      if (cpfCnpj !== this.intervenienteCpfCnpj) {
        this.intervenienteCpfCnpj = cpfCnpj;
  
        if ( (this.intervenienteTipo === 'F' && cpfCnpj.length === 11)
            || (this.intervenienteTipo !== 'F' && cpfCnpj.length === 14) ) {
            
          this._intervenienteService.obterPermitidoPorCpfCnpj(cpfCnpj, "")
          .then(interveniente => {

            if (interveniente) {
            
              this.intervenienteTipo = interveniente.intervenienteTipo;
              this.intervenienteCpfCnpj = interveniente.cpfCnpj;
              this.intervenienteSelecionado = interveniente;
              this.oportunidade.intervenienteCodigo = interveniente.codigo;
              this.oportunidade.cliente = interveniente.razao;
    
            } else {
    
              this.oportunidade.intervenienteCodigo = 0;
              if (this.intervenienteSelecionado) this.intervenienteSelecionado.codigo = 0;
              else this.intervenienteSelecionado = new Interveniente();
    
            }

          },error => {          
            var self = OportunidadePageComponent.self;
            self.dialog.open(AlertDialogComponent, {
              disableClose: true,
              data: {
                title: 'TopConWeb',
                message: `Não foi possível carregar os dados do Cliente !`
              }
            });
            if (this.intervenienteTipo === 'F') this.geralForm.controls['cpf'].setValue('');
            else if (this.intervenienteTipo !== 'F') this.geralForm.controls['cnpj'].setValue('');          
          })
        }
      }

      
    }

    saveProgress() {
      if (!this.isInsertMode) return;
      localStorage.setItem('t.tcw.novaOportunidade', JSON.stringify(this.oportunidade));
    }

    clearSavedProgress() {
      localStorage.removeItem('t.tcw.novaOportunidade');
    }

    numeroOportunidade(): string {
      return (this.oportunidade.usina ? this.oportunidade.usina.codigo : '0')+'/'+this.oportunidade.numero+'-'+this.oportunidade.ano;
    }

    private _timeoutIntervenientesPorRazao = null;
    filtrarIntervenientesPorRazao(cliente: string) {
      
      var tamanhoMinimo = (isNaN(parseInt(cliente)) ? 3 : 0);
  
      if (cliente && cliente.length > tamanhoMinimo && (this.oportunidade.cliente !== cliente)) {
        
        if (this._timeoutIntervenientesPorRazao) clearTimeout(this._timeoutIntervenientesPorRazao);
        
        var filtro = 'filter=$(' + (isNaN(parseInt(cliente)) ? 'razao%=' + cliente : 'codigo==' + parseInt(cliente)) + ')';
  
        this._timeoutIntervenientesPorRazao = setTimeout( () => {
          this._intervenienteService.listarFiltradosPermitidos(filtro, true)
            .then(
              intervenientes => {
                this.intervenientes = intervenientes.sort((a, b) => a.razao.localeCompare(b.razao));
              },
              error => { 
                this.intervenientes = []; 
                var self = OportunidadePageComponent.self;
                self.dialog.open(AlertDialogComponent, {
                  disableClose: true,
                  data: {
                    title: 'TopConWeb',
                    message: `Não foi possível carregar os dados do Cliente!`
                  }
                });
              }
            )
        }, 500);
  
      } else {
        this.intervenientes = [];
      }
    }

    faseFormatter = (model: OportunidadeFase): string => model ? (model.descricao).toUpperCase() : '';
    vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
    segmentacaoFormatter = (model:Segmentacao): string => model ? (model.nome).toUpperCase() : '';
    cadastroGeralFormatter = (model: CadastroGeral): string => model ? model.descricao.toUpperCase() : '';
    oportunidadeTipoFormatter = (model: OportunidadeTipo): string => model ? model.descricao.toUpperCase() : '';
    intervenienteFormatter = (model: Interveniente): string => model ? (model.codigo > 0 ? (model.codigo+' - '+(model.razao || model.nome)).toUpperCase() :'') : '';
    genericFormatter = (model: any): string => model ? (model.codigo == 99 ? model.descricao : model.descricao).toUpperCase() : '';
    motivoPerdaFormatter = (model: MotivoPerda) => model ? (model.codigo + ' - ' + model.descricao) : '';
    concorrenteFormatter = (model: Concorrente) => model ? (model.codigo + ' - ' + model.descricao) : '';
    usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
    intervenienteTipoFormatter = (model: string): string => {
      if (!model) return '';
      return this.intervenienteTipos.includes(model) ? intervenienteTipos.filter(e => e.codigo===model)[0].descricao : '';
    };

    changeVendedor(model: Vendedor) { this.oportunidade.vendedorCodigo = model ? model.codigo : 0; }
    changeSegmentacao(model: Segmentacao) { this.oportunidade.segmentacaoCodigo = model ? model.id : 0; }
    changeOportunidadeTipo(model: OportunidadeTipo) { this.oportunidade.oportunidadeTipoCodigo = model ? model.codigo : 0; }
    changeViaCaptacao(model: CadastroGeral) { this.oportunidade.viaCaptacaoCodigo = model ? model.codigo : 0; }
    changeFase(model: OportunidadeFase) { this.oportunidade.faseCodigo = model ? model.codigo : 0; }
    changeClassificao(model: any) { this.oportunidade.classificacao = model ? model.codigo : 0; }
    changeMotivoPerda(model: MotivoPerda) { this.oportunidade.motivoPerdaCodigo = model ? model.codigo : 0; }
    changeConcorrente(model: Concorrente) { this.oportunidade.concorrenteCodigo = model ? model.codigo : 0; }

    changeObraPorte(model: CadastroGeral) { this.oportunidade.porteObraCodigo =  model ? model.codigo : 0; }
    changeObraFase(model: any) { this.oportunidade.obraFase =  model ? model.codigo : 0; }

    changeFuncao(funcao: CadastroGeral, contato: OportunidadeContato) { contato.funcaoCodigo =  funcao ? funcao.codigo : 0; }
    changeNomeCliente(cliente: string) { if(cliente) this.oportunidade.contatoPrincipal.nome = cliente; }
    changeDddTelefone(ddd: number) { if(ddd) this.oportunidade.contatoPrincipal.dddTelefone = ddd; }
    changeDddCelular(ddd: number) { if(ddd) this.oportunidade.contatoPrincipal.dddCelular = ddd; }
    changeTelefone(numero: number) { if(numero) this.oportunidade.contatoPrincipal.telefone = numero; }
    changeCelular(numero: number) { if(numero) this.oportunidade.contatoPrincipal.celular = numero; }
    changeEmail(email: string) { if(email) this.oportunidade.contatoPrincipal.email = email; }

    isMotivoPerdaObrigatorio() { 
      return this.oportunidade.fase ? this.oportunidade.fase.descricao.toUpperCase().includes('PERDIDO') : false 
    }

    changeUsinaEntrega(newUsina: Usina) {

      this.oportunidade.usinaEntregaCodigo = newUsina.codigo;
      
      var usinaAnterior = this.oportunidade.usinaEntrega;

      this.oportunidade.usinaEntrega = newUsina;
      if (usinaAnterior !== newUsina) {
        this.carregaObraDistanciaUsina();
        this.carregaDistanciaUsinaViaGoogleApi();
      }
      
    }
  
    formataValor = Tasks.formataValor;
    formataMoeda = Tasks.formataMoeda;
  
    onGeralNext() { this.saveProgress();  }
    onObraNext() { this.saveProgress(); }
    onContatoNext() { this.saveProgress();  }
    
    onGeralPrevious() { this.saveProgress(); }
    onObraPrevious() { this.saveProgress(); }
    onContatoPrevious() { this.saveProgress(); }
  
    async onComplete() {

      let self = OportunidadePageComponent.self;

      let request = self.isInsertMode ?
        self._oportunidadeService.Adicionar(self.oportunidade) :
        self._oportunidadeService.Atualizar(self.oportunidade);

      await request.then(
        resultado => {

          var message = self.isInsertMode ? `Oportunidade inserida com sucesso!` : resultado;
    
          self.clearSavedProgress();

          if (self.isInsertMode) {
            self.oportunidade.usina = new Usina();
            self.oportunidade.usinaCodigo = resultado.usinaCodigo;
            self.oportunidade.usina.codigo = self.oportunidade.usinaCodigo;
            self.oportunidade.ano = resultado.ano;
            self.oportunidade.numero = resultado.numero;
          }

          let router = self._router;
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: message,
              afterCloseCallback: async () => {
                router.navigateByUrl("pages/comercial/oportunidade/lista");
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
      this.oportunidade = new Oportunidade();
      this.location.back();
    }

    alteracaoNaoPermitida() {

      if(this.isInsertMode)
        return false;

      return (!this.temDireitoAlteracao);

    }

    utilizaGoogleMatrixAPI: boolean = true;
    disableObraDistanciaUsina: boolean = true;
    carregaObraDistanciaUsina() {
      var self = OportunidadePageComponent.self;
  
      self._enderecoService
      .obterDistanciaKmPorUsinaCep(self.oportunidade.usinaEntrega, self.oportunidade.endereco.cep)
      .then(km => {
        self.oportunidade.distanciaUsina = km;
  
        self.validaDistanciaKmUsinaCepAprovada(self.oportunidade.usinaEntrega, self.oportunidade.endereco.cep, self.oportunidade.distanciaUsina);
      }, err => {
        self.disableObraDistanciaUsina = false;
        self.detectChanges();
      });
      
    }

    carregaDistanciaUsinaViaGoogleApi() {
      var self = OportunidadePageComponent.self;
  
      if (self.utilizaGoogleMatrixAPI === true && self.oportunidade.usinaEntrega) {
        var enderecoObra = `${self.oportunidade.endereco.logradouro.trim().replace(" ","+")}
         +${self.oportunidade.endereco.numero.toString()} 
         +${self.oportunidade.endereco.bairro.trim().replace(" ","+")}
         +${self.oportunidade.endereco.cep.trim().replace("-","")}`
  
        self._enderecoService.obterDistanciaKmEntreUsinaEObraViaGoogleApi(self.oportunidade.usinaEntrega, enderecoObra, true)
        .then(response => {
          self.oportunidade.distanciaUsina = response.distanciaEmKm !== null ? response.distanciaEmKm : 0;
          self.utilizaGoogleMatrixAPI = response.utilizaGoogleApi;
          self.detectChanges();
        }, err => {
            self.oportunidade.distanciaUsina = 0;
        });
      }
  
    }

    validaDistanciaKmUsinaCepAprovada(usinaEntrega: Usina, cep: string, km: number) {
      var self = OportunidadePageComponent.self;
  
      if(!usinaEntrega)
        return;

      if (km > 0) {
        self._enderecoService
        .distanciaKmUsinaCepAprovada(usinaEntrega, cep)
        .then(aprovada => {
          self.disableObraDistanciaUsina = aprovada;
          self.detectChanges();
        }, err => {
          self.disableObraDistanciaUsina = false;
          self.detectChanges();
        });
      } else {
        self.disableObraDistanciaUsina = false;
        self.detectChanges();
      }
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
