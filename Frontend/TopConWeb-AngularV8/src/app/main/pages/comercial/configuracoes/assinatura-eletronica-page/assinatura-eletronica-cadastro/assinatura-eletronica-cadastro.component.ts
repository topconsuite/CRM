import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ClicksignParametro } from 'app/classes/assinatura-eletronica/clicksing-parametro';
import { ClicksignConfiguracao } from 'app/classes/assinatura-eletronica/clicksign-configuracao';
import { Tasks } from 'app/classes/_tasks/tasks';
import { Usina } from 'app/classes/usina/usina';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { AssinaturaEletronicaService } from 'app/services/assinatura-eletronica.service';
import { UsinaService } from 'app/services/usina.service';
import { UserService } from 'app/services/user.service';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { EMetodoAutenticacao, EMetodoEnvioAssinatura, EOpcoesTestemunhas } from 'app/classes/assinatura-eletronica/solicitacao-assinatura-clicksing';

@Component({
  selector: 'app-assinatura-eletronica-cadastro',
  templateUrl: './assinatura-eletronica-cadastro.component.html',
  styleUrls: ['./assinatura-eletronica-cadastro.component.scss']
})
export class AssinaturaEletronicaCadastroComponent implements OnInit, AfterViewInit {
  public static self: AssinaturaEletronicaCadastroComponent;

  @ViewChild('modalVCR', { read: ViewContainerRef, static: false }) ModalRef: ViewContainerRef;
  @ViewChild('subModalVCR', { read: ViewContainerRef, static: false }) SubModalRef: ViewContainerRef;

  assinaturaEletronicaForm: FormGroup;
  clicksignConfiguracaoForm: FormGroup;
  clicksignParametro: ClicksignParametro = new ClicksignParametro();
  confirmouAssinaturaWhatsApp: boolean = false;

  temDireitoAlteracao: boolean = false;
  envioAssinaturaWhatsApp: boolean;

  get metodoAutenticacaoLista(): EMetodoAutenticacao[] {
    return [
      EMetodoAutenticacao.Email,
      EMetodoAutenticacao.Sms,
      EMetodoAutenticacao.Whatsapp
    ];
  }

  get metodoEnvioAssinaturaLista(): EMetodoEnvioAssinatura[] {
    if(this.clicksignParametro.permiteAssinaturaWhatsApp){
      return [
        EMetodoEnvioAssinatura.Email,
        EMetodoEnvioAssinatura.Whatsapp
      ];
    }
    return [
      EMetodoEnvioAssinatura.Email
    ];
  }

  get opcoesTestemunhasLista(): EOpcoesTestemunhas[] {
    return [
      EOpcoesTestemunhas.NaoEnvia,
      EOpcoesTestemunhas.Testemunha,
      EOpcoesTestemunhas.Vendedor
    ];
  }

  get opcoesPrimeiraTestemunhaLista(): EOpcoesTestemunhas[] {
    return this.clicksignParametro.segundaTestemunha === EOpcoesTestemunhas.Vendedor
        ? [EOpcoesTestemunhas.NaoEnvia, EOpcoesTestemunhas.Testemunha]
        : [EOpcoesTestemunhas.NaoEnvia, EOpcoesTestemunhas.Testemunha, EOpcoesTestemunhas.Vendedor];
  }

  get opcoesSegundaTestemunhaLista(): EOpcoesTestemunhas[] {
      return this.clicksignParametro.primeiraTestemunha === EOpcoesTestemunhas.Vendedor
          ? [EOpcoesTestemunhas.NaoEnvia, EOpcoesTestemunhas.Testemunha]
          : [EOpcoesTestemunhas.NaoEnvia, EOpcoesTestemunhas.Testemunha, EOpcoesTestemunhas.Vendedor];
  }

  metodoAutenticacaoFormatter = (item: EMetodoAutenticacao) => {
    switch (item) {
      case EMetodoAutenticacao.Email:
        return 'Token via email';
      case EMetodoAutenticacao.Sms:
        return 'Token via sms';
      case EMetodoAutenticacao.Whatsapp:
        return 'Token via whatsapp';
      default:
        return '';
    }
  }

  metodoEnvioAssinaturaFormatter = (item: EMetodoEnvioAssinatura) => {
    switch (item) {
      case EMetodoEnvioAssinatura.Email:
        return 'Envio via email';
      case EMetodoEnvioAssinatura.Whatsapp:
        return 'Envio via whatsapp';
      default:
        return '';
    }
  }

  opcoesTestemunhasFormatter = (item: EOpcoesTestemunhas) => {
    switch (item) {
      case EOpcoesTestemunhas.NaoEnvia:
        return 'Não Envia';
      case EOpcoesTestemunhas.Testemunha:
        return 'Testemunha';
      case EOpcoesTestemunhas.Vendedor:
        return 'Vendedor';
      default:
        return '';
    }
  }

  constructor(
    public dialog: MatDialog,
    private _formBuilder: FormBuilder,
    private _assinaturaEletronicaService: AssinaturaEletronicaService,
    private _dialog: MatDialog,
    private _userService: UserService,
    private _usinaService: UsinaService,
    private _cdr: ChangeDetectorRef,
  ) { 
    AssinaturaEletronicaCadastroComponent.self = this;

    var temDireito = this._userService.temDireitoAplicativo('WEB6003','', 200);
    if (!temDireito) return;

    this.temDireitoAlteracao = this._userService.temDireitoAplicativo('WEB6003','A');

  }

  ngAfterViewInit(): void {
  }

  ngOnInit() {
    this.assinaturaEletronicaForm = this._formBuilder.group({
      obrigaFotoDocumentoOficial: [''],
      obrigaSelfie: [''],
      obrigaAssinaturaManuscrita: [''],
      obrigaReconhecimentoFacial: [''],
      notificaClienteNaConfirmacaoDeAssinatura: [''],
      possuiDocumentacao: [''],
      enviaAssinaturaContratada: [''],
      enviaAssinaturaResponsavelSolidario: [''],
      permiteAssinaturaWhatsApp: [''],
      emailContratada: [''],
      dddContratada: [0],
      telefoneContratada: [0]
    });

    this.clicksignConfiguracaoForm = this._formBuilder.group({
      alias: [''],
      csEmail: [''],
      token: [''],
      baseUrl: [''],
      sha256Secret: [''],
      ativo: [true],
      usinaVincular: [null]
    });

    this._assinaturaEletronicaService.ObterClicksignParametro().then(
      parametro => { 
        if (parametro !== null) this.clicksignParametro = parametro; },
      error => { this.clicksignParametro = new ClicksignParametro(); }
    );
  }

  formataErrosApi = Tasks.formataErrosApi;

  confirmacaoCienciaCobrancaAssinaturaWhatsApp(selected: boolean) {
    if(!selected){
      this.clicksignParametro.metodoEnvioAssinaturaContratada = EMetodoEnvioAssinatura.Email;
      return;
    }

    var dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'TopConWeb - Confirmação',
        message: 'Solicitar assinatura via WhatsApp possui cobrança adicional, tem certeza que desejar ativar ?',
        cancelCallback: () => { this.clicksignParametro.permiteAssinaturaWhatsApp = false; }
      },
      disableClose: false
    });

    dialogRef.backdropClick().subscribe(result => {
      this.clicksignParametro.permiteAssinaturaWhatsApp = false;
    });

    dialogRef.keydownEvents().subscribe(key => {
      if(key.keyCode == 27 || key.code == 'Escape') {
        this.clicksignParametro.permiteAssinaturaWhatsApp = false;
      }
    })

  }

  salvarConfiguracao() {
    if(this.assinaturaEletronicaForm.invalid) return;
    
    this._assinaturaEletronicaService.AtualizarClicksignParametro(this.clicksignParametro)
    .then(success => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Parâmetros atualizados com sucesso!`
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

  metodoEnvioAssinaturaContratadaChange(metodoEnvioAssinatura: EMetodoEnvioAssinatura) {
    this.envioAssinaturaWhatsApp = metodoEnvioAssinatura === EMetodoEnvioAssinatura.Whatsapp;
    if(this.envioAssinaturaWhatsApp){
      this.clicksignParametro.metodoAutenticacaoContratada = EMetodoAutenticacao.Whatsapp;
    }
  }

  possuiEnvioOuAutenticaoCelular() : boolean{
    if (this.clicksignParametro.metodoAutenticacaoContratada === EMetodoAutenticacao.Whatsapp || this.clicksignParametro.metodoAutenticacaoContratada === EMetodoAutenticacao.Sms 
        || this.clicksignParametro.metodoEnvioAssinaturaContratada === EMetodoEnvioAssinatura.Whatsapp){
      return true;
    }
    return false;
  }

  // --------------- Modal ClickSign Configuracao -------------------------

  modalIsOpen: boolean = false;
  subModalIsOpen: boolean = false;

  private _dialogRef: MatDialogRef<any>;
  private _subDialogRef: MatDialogRef<any>;

  clicksignConfiguracoes: ClicksignConfiguracao[] = [];
  configuracaoEmEdicao: ClicksignConfiguracao = new ClicksignConfiguracao();
  configuracaoNovo: boolean = false;
  configuracaoSelecionada: ClicksignConfiguracao = new ClicksignConfiguracao();

  usinasVinculadas: Usina[] = [];
  usinasDisponiveis: Usina[] = [];
  usinaSelecionada: Usina = null;

  usinaFormatter = (model: Usina): string => model ? (model.codigo + ' - ' + model.nome).toUpperCase() : '';

  openModal(content) {
    this._dialogRef = this._dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
  }

  openSubModal(content) {
    this._subDialogRef = this._dialog.open(content, { viewContainerRef: this.SubModalRef });
    this.subModalIsOpen = true;
  }

  closeModal() {
    let self = AssinaturaEletronicaCadastroComponent.self;

    if (self._dialogRef) self._dialogRef.close();
    self.closeSubModal();
    self._dialog.closeAll();
    self.modalIsOpen = false;
  }

  closeSubModal() {
    let self = AssinaturaEletronicaCadastroComponent.self;

    if (self._subDialogRef) self._subDialogRef.close();
    self.subModalIsOpen = false;
  }

  openClicksignConfigModal(content) {
    this._assinaturaEletronicaService.ListarClicksignConfiguracoes().then(
      lista => { this.clicksignConfiguracoes = lista; },
      err => { this.clicksignConfiguracoes = []; }
    );
    this.openModal(content);
  }

  openConfiguracaoFormModal(content, configuracao?: ClicksignConfiguracao) {
    let self = AssinaturaEletronicaCadastroComponent.self;

    self.configuracaoNovo = !configuracao;
    self.configuracaoEmEdicao = configuracao
      ? Object.assign(new ClicksignConfiguracao(), configuracao)
      : new ClicksignConfiguracao();

    self.openSubModal(content);
  }

  salvarClicksignConfiguracao() {
    let self = AssinaturaEletronicaCadastroComponent.self;

    self._assinaturaEletronicaService.SalvarClicksignConfiguracao(self.configuracaoEmEdicao).then(
      () => {
        self.closeSubModal();
        self._assinaturaEletronicaService.ListarClicksignConfiguracoes().then(
          lista => { self.clicksignConfiguracoes = lista; },
          err => {}
        );
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: 'Configuração salva com sucesso!' }
        });
      },
      err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: `${self.formataErrosApi(err)}` }
        });
      }
    );
  }

  removerConfiguracao(configuracao: ClicksignConfiguracao) {
    let self = AssinaturaEletronicaCadastroComponent.self;

    self._assinaturaEletronicaService.RemoverClicksignConfiguracao(configuracao.id).then(
      () => {
        self.clicksignConfiguracoes = self.clicksignConfiguracoes.filter(c => c.id !== configuracao.id);
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: 'Configuração removida com sucesso!' }
        });
      },
      err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: `${self.formataErrosApi(err)}` }
        });
      }
    );
  }

  openUsinasModal(content, configuracao: ClicksignConfiguracao) {
    let self = AssinaturaEletronicaCadastroComponent.self;

    self.configuracaoSelecionada = configuracao;
    self.usinaSelecionada = null;

    self._assinaturaEletronicaService.ListarUsinasPorConfiguracao(configuracao.id).then(
      usinas => { self.usinasVinculadas = usinas; },
      err => { self.usinasVinculadas = []; }
    );

    self._usinaService.listarTodos().then(
      usinas => { self.usinasDisponiveis = usinas; },
      err => { self.usinasDisponiveis = []; }
    );

    self.openSubModal(content);
  }

  vincularUsina() {
    let self = AssinaturaEletronicaCadastroComponent.self;

    if (!self.usinaSelecionada) return;

    self._assinaturaEletronicaService.VincularUsina(self.configuracaoSelecionada.id, self.usinaSelecionada.codigo).then(
      () => {
        const jaVinculada = self.usinasVinculadas.find(u => u.codigo === self.usinaSelecionada.codigo);
        if (!jaVinculada) {
          self.usinasVinculadas = [...self.usinasVinculadas, self.usinaSelecionada];
        }
        self.usinaSelecionada = null;
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: 'Usina vinculada com sucesso!' }
        });
      },
      err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: `${self.formataErrosApi(err)}` }
        });
      }
    );
  }

  desvincularUsina(usina: Usina) {
    let self = AssinaturaEletronicaCadastroComponent.self;

    self._assinaturaEletronicaService.DesvincularUsina(self.configuracaoSelecionada.id, usina.codigo).then(
      () => {
        self.usinasVinculadas = self.usinasVinculadas.filter(u => u.codigo !== usina.codigo);
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: 'Usina desvinculada com sucesso!' }
        });
      },
      err => {
        self._dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: { title: 'TopConWeb', message: `${self.formataErrosApi(err)}` }
        });
      }
    );
  }
}
