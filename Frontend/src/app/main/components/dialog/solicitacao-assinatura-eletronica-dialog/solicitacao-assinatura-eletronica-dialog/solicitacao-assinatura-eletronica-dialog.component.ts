import { AfterViewInit, ChangeDetectorRef, Component, Inject, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ContratoClicksignEnvio, EStatusClicksignDocumento, statusClicksignDocumento } from 'app/classes/assinatura-eletronica/contrato-clicksing-envio';
import { EMetodoAutenticacao, EMetodoEnvioAssinatura, SolicitacaoAssinaturaClicksign } from 'app/classes/assinatura-eletronica/solicitacao-assinatura-clicksing';
import { Contrato } from 'app/classes/contrato/contrato';
import { Status, Proposta } from 'app/classes/proposta/proposta';
import { Tasks } from 'app/classes/_tasks/tasks';
import { AssinaturaEletronicaService } from 'app/services/assinatura-eletronica.service';
import { ContratoService } from 'app/services/contrato.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { UserService } from 'app/services/user.service';
import { AlertDialogComponent } from '../../alert-dialog/alert-dialog.component';
import { ClicksignParametro } from 'app/classes/assinatura-eletronica/clicksing-parametro';
import { PropostaDadosPessoais } from 'app/classes/proposta/proposta.classes';
import { DadosPessoais } from 'app/classes/assinatura-eletronica/solicitacao-dados-pessoais';

@Component({
  selector: 'app-solicitacao-assinatura-eletronica-dialog',
  templateUrl: './solicitacao-assinatura-eletronica-dialog.component.html',
  styleUrls: ['./solicitacao-assinatura-eletronica-dialog.component.scss']
})
export class SolicitacaoAssinaturaEletronicaDialogComponent implements OnInit{

  @ViewChild('scrollContainer', { static: false }) scrollContainer!: ElementRef;

  solicitacaoAssinaturaForm: FormGroup;
  solicitacaoAssinatura: SolicitacaoAssinaturaClicksign = new SolicitacaoAssinaturaClicksign();
  ultimoContratoClicksignEnvio: ContratoClicksignEnvio = null;
  clicksignParametro: ClicksignParametro = new ClicksignParametro();

  envioAssinaturaWhatsApp: boolean;

  temDireitoAdicionarDadosPessoais: boolean = false;
  temDireitoAlterarDadosPessoais: boolean = false;

  readonly MAX_DADOS_PESSOAIS = 15;

  get metodoAutenticacaoLista(): EMetodoAutenticacao[] {
    return [
      EMetodoAutenticacao.Email,
      EMetodoAutenticacao.Sms,
      EMetodoAutenticacao.Whatsapp
    ];
  }

  get metodoEnvioAssinaturaLista(): EMetodoEnvioAssinatura[] {
    return [
      EMetodoEnvioAssinatura.Email,
      EMetodoEnvioAssinatura.Whatsapp
    ];
  }

  displayedColumns: string[] = ['id', 'DataEnvio'];

  telefoneObrigatorio(i: number): boolean {
    return this.solicitacaoAssinatura.dadosPessoaisAssinatura[i].metodoAutenticacao === EMetodoAutenticacao.Sms || this.solicitacaoAssinatura.dadosPessoaisAssinatura[i].metodoAutenticacao === EMetodoAutenticacao.Whatsapp || this.solicitacaoAssinatura.dadosPessoaisAssinatura[i].metodoEnvioAssinatura === EMetodoEnvioAssinatura.Whatsapp;
  }

  get telefoneObrigatorioResponsavelSolidario(): boolean {
    return this.solicitacaoAssinatura.metodoAutenticacaoResponsavelSolidario === EMetodoAutenticacao.Sms || this.solicitacaoAssinatura.metodoAutenticacaoResponsavelSolidario === EMetodoAutenticacao.Whatsapp || this.solicitacaoAssinatura.metodoEnvioAssinaturaResponsavelSolidario === EMetodoEnvioAssinatura.Whatsapp;
  }

  get telefoneObrigatorioPrimeiraTestemunha(): boolean {
    return this.solicitacaoAssinatura.metodoAutenticacaoPrimeiraTestemunha === EMetodoAutenticacao.Sms || this.solicitacaoAssinatura.metodoAutenticacaoPrimeiraTestemunha === EMetodoAutenticacao.Whatsapp || this.solicitacaoAssinatura.metodoEnvioAssinaturaPrimeiraTestemunha === EMetodoEnvioAssinatura.Whatsapp;
  }

  get telefoneObrigatorioSegundaTestemunha(): boolean {
    return this.solicitacaoAssinatura.metodoAutenticacaoSegundaTestemunha === EMetodoAutenticacao.Sms || this.solicitacaoAssinatura.metodoAutenticacaoSegundaTestemunha === EMetodoAutenticacao.Whatsapp || this.solicitacaoAssinatura.metodoEnvioAssinaturaSegundaTestemunha === EMetodoEnvioAssinatura.Whatsapp;
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


  constructor(
    public dialogRef: MatDialogRef<SolicitacaoAssinaturaEletronicaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      contratoAno: number,
      contratoNumero: number,
      contratoUsina: number,
      intervenienteCodigo: number,
      utilizaResponsavelSolidario: boolean,
      nomeCompletoResponsavelSolidario: string,
      emailResponsavelSolidario: string,
      cpfResponsavelSolidario: string,
      telefoneDddResponsavelSolidario: number,
      telefoneNumeroResponsavelSolidario: number,
      nomeCompletoVendedor: string,
      emailVendedor: string,
      cpfVendedor: string,
      telefoneDddVendedor: number,
      telefoneNumeroVendedor: number,
      afterConfirmCallback?: Function
    },
    private _cdr: ChangeDetectorRef,
    private _intervenienteService: IntervenienteService,
    private _formBuilder: FormBuilder,
    private _assinaturaEletronicaService: AssinaturaEletronicaService,
    private _userService: UserService,
    private _dialog: MatDialog,
  ) { 

    this.temDireitoAdicionarDadosPessoais = this._userService.temDireitoAplicativo('WEB7011', '');
    this.temDireitoAlterarDadosPessoais = this._userService.temDireitoAplicativo('WEB7012', '');
  }

  formataErrosApi = Tasks.formataErrosApi;
  formataDataHora = Tasks.formataDataHora;

  async ngOnInit() {
    this.solicitacaoAssinaturaForm = this._formBuilder.group({});
    this.adicionarDadosPessoaisAssinatura();

    try{
      const [item, parametro] = await Promise.all([
        this._assinaturaEletronicaService.ObterUltimoContratoClicksignEnvio(this.data.contratoUsina, this.data.contratoAno, this.data.contratoNumero),
        this._assinaturaEletronicaService.ObterClicksignParametro()
      ]);

      this.ultimoContratoClicksignEnvio =  item;
      this.clicksignParametro = parametro || new ClicksignParametro();

      if (this.ultimoContratoClicksignEnvio == null) {
        this.solicitacaoAssinatura.contratoAno = this.data.contratoAno
        this.solicitacaoAssinatura.contratoNumero = this.data.contratoNumero
        this.solicitacaoAssinatura.contratoUsina = this.data.contratoUsina

        this.solicitacaoAssinatura.nomeCompletoResponsavelSolidario = this.data.nomeCompletoResponsavelSolidario;
        this.solicitacaoAssinatura.emailResponsavelSolidario =this.data.emailResponsavelSolidario.split(";")[0];
        this.solicitacaoAssinatura.cpfResponsavelSolidario = this.data.cpfResponsavelSolidario;
        this.solicitacaoAssinatura.telefoneDddResponsavelSolidario = this.data.telefoneDddResponsavelSolidario;
        this.solicitacaoAssinatura.telefoneNumeroResponsavelSolidario = this.data.telefoneNumeroResponsavelSolidario;

        if(this.enviaVendedorPrimeiraTestemunha()){
          this.solicitacaoAssinatura.nomeCompletoPrimeiraTestemunha = this.data.nomeCompletoVendedor;
          this.solicitacaoAssinatura.emailPrimeiraTestemunha = this.data.emailVendedor;
          this.solicitacaoAssinatura.cpfPrimeiraTestemunha = this.data.cpfVendedor;
          this.solicitacaoAssinatura.telefoneDddPrimeiraTestemunha = this.data.telefoneDddVendedor;
          this.solicitacaoAssinatura.telefoneNumeroPrimeiraTestemunha = this.data.telefoneNumeroVendedor;
        }else if(this.enviaVendedorSegundaTestemunha()){
          this.solicitacaoAssinatura.nomeCompletoSegundaTestemunha = this.data.nomeCompletoVendedor;
          this.solicitacaoAssinatura.emailSegundaTestemunha = this.data.emailVendedor;
          this.solicitacaoAssinatura.cpfSegundaTestemunha = this.data.cpfVendedor;
          this.solicitacaoAssinatura.telefoneDddSegundaTestemunha = this.data.telefoneDddVendedor;
          this.solicitacaoAssinatura.telefoneNumeroSegundaTestemunha = this.data.telefoneNumeroVendedor;
        }

        const interv = await this._intervenienteService.obterPorCodigoInterveniente(this.data.intervenienteCodigo);
        if (interv && interv.intervenienteTipo === "F") {
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].nomeCompleto = interv.razao;
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].email = interv.email.split(";")[0];
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].cpf = interv.cpfCnpj;
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].telefoneDdd = interv.celularDdd;
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].telefoneNumero = interv.celularNumero;
        }
      }
    } catch (error) {
      this.clicksignParametro = new ClicksignParametro(); 
    } finally {
      this._cdr.detectChanges();
    }
  }

  adicionarDadosPessoaisAssinatura() {
    if (this.solicitacaoAssinatura.dadosPessoaisAssinatura.length < this.MAX_DADOS_PESSOAIS){
      this.solicitacaoAssinatura.dadosPessoaisAssinatura.push(new DadosPessoais()); 
      setTimeout(() => this.scrollParaFim(), 200);
    }else{
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'Limite de assinantes atingido!',
          message: 'É possível adicionar no máximo '+ this.MAX_DADOS_PESSOAIS + ' assinantes para os Dados Pessoais.'
        }
      });
    }
  }

  scrollParaFim() {
    if (this.scrollContainer) {
      this.scrollContainer.nativeElement.scrollLeft = this.scrollContainer.nativeElement.scrollWidth;
    }
  }

  closeDialog() {
    this.dialogRef.close();
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  reenviarSolicitacao(){
    this.ultimoContratoClicksignEnvio = null;
    this.solicitacaoAssinatura.contratoAno = this.data.contratoAno
    this.solicitacaoAssinatura.contratoNumero = this.data.contratoNumero
    this.solicitacaoAssinatura.contratoUsina = this.data.contratoUsina
    this._intervenienteService.obterPorCodigoInterveniente(this.data.intervenienteCodigo)
      .then(interv => { 
        if (interv.intervenienteTipo === "F") {
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].nomeCompleto = interv.nome;
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].email = interv.email.split(";")[0];
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].cpf = interv.cpfCnpj;
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].telefoneDdd = interv.celularDdd;
          this.solicitacaoAssinatura.dadosPessoaisAssinatura[0].telefoneNumero = interv.celularNumero;
        }
      }).then(() => {this._cdr.detectChanges()});
    
      this.solicitacaoAssinatura.nomeCompletoResponsavelSolidario = this.data.nomeCompletoResponsavelSolidario;
      this.solicitacaoAssinatura.emailResponsavelSolidario =this.data.emailResponsavelSolidario.split(";")[0];
      this.solicitacaoAssinatura.cpfResponsavelSolidario = this.data.cpfResponsavelSolidario;
      this.solicitacaoAssinatura.telefoneDddResponsavelSolidario = this.data.telefoneDddResponsavelSolidario;
      this.solicitacaoAssinatura.telefoneNumeroResponsavelSolidario = this.data.telefoneNumeroResponsavelSolidario;
      if(this.enviaVendedorPrimeiraTestemunha()){
        this.solicitacaoAssinatura.nomeCompletoPrimeiraTestemunha = this.data.nomeCompletoVendedor;
        this.solicitacaoAssinatura.emailPrimeiraTestemunha = this.data.emailVendedor;
        this.solicitacaoAssinatura.cpfPrimeiraTestemunha = this.data.cpfVendedor;
        this.solicitacaoAssinatura.telefoneDddPrimeiraTestemunha = this.data.telefoneDddVendedor;
        this.solicitacaoAssinatura.telefoneNumeroPrimeiraTestemunha = this.data.telefoneNumeroVendedor;
      }else if(this.enviaVendedorSegundaTestemunha()){
        this.solicitacaoAssinatura.nomeCompletoSegundaTestemunha = this.data.nomeCompletoVendedor;
        this.solicitacaoAssinatura.emailSegundaTestemunha = this.data.emailVendedor;
        this.solicitacaoAssinatura.cpfSegundaTestemunha = this.data.cpfVendedor;
        this.solicitacaoAssinatura.telefoneDddSegundaTestemunha = this.data.telefoneDddVendedor;
        this.solicitacaoAssinatura.telefoneNumeroSegundaTestemunha = this.data.telefoneNumeroVendedor;
      }
      this._cdr.detectChanges()
  }

  setCpfCnpj(cpf: string, i: number) {
    if (cpf !== this.solicitacaoAssinatura.dadosPessoaisAssinatura[i].cpf) {
      this.solicitacaoAssinatura.dadosPessoaisAssinatura[i].cpf = cpf;
    }
  }

  setCpfCnpjResponsavelSolidario(cpf: string) {
    if (cpf !== this.solicitacaoAssinatura.cpfResponsavelSolidario) {
      this.solicitacaoAssinatura.cpfResponsavelSolidario = cpf;
    }
  }

  setCpfCnpjPrimeiraTestemunha(cpf: string) {
    if (cpf !== this.solicitacaoAssinatura.cpfPrimeiraTestemunha) {
      this.solicitacaoAssinatura.cpfPrimeiraTestemunha = cpf;
    }
  }

  setCpfCnpjSegundaTestemunha(cpf: string) {
    if (cpf !== this.solicitacaoAssinatura.cpfSegundaTestemunha) {
      this.solicitacaoAssinatura.cpfSegundaTestemunha = cpf;
    }
  }

  botaoCancelarEnabled(): boolean {
    if (this.ultimoContratoClicksignEnvio == null || this.ultimoContratoClicksignEnvio == undefined) return false;
    return this.ultimoContratoClicksignEnvio.statusClicksignDocumento === EStatusClicksignDocumento.Processando;
  }
  
  botaoReenviarEnabled(): boolean {
    if (this.ultimoContratoClicksignEnvio == null || this.ultimoContratoClicksignEnvio == undefined) return false;
    return this.ultimoContratoClicksignEnvio.statusClicksignDocumento === EStatusClicksignDocumento.Cancelado;
  } 

  fecharSolicitacao() {
    this.closeModal();
  }
  
  closeModal(){
    this.limparForm();
    this.dialogRef.close();
  }

  limparForm() {
    this.solicitacaoAssinaturaForm.markAsPristine();
    this.solicitacaoAssinaturaForm.markAsUntouched();
    this.solicitacaoAssinatura = new SolicitacaoAssinaturaClicksign();
  }

  enviarSolicitacao() {
    this._assinaturaEletronicaService.SolicitarAssinaturaClicksign(this.solicitacaoAssinatura)
    .then(success => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Assinatura Solicitada com sucesso!`,
          afterCloseCallback: this.data.afterConfirmCallback
        }
      });
    }, err => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'Falha ao solicitar assinatura!',
          message: `${this.formataErrosApi(err)}`
        }
      });
    });
  }

  cancelarDocumento() {
    this._assinaturaEletronicaService.CancelarDocumentoClicksign(this.ultimoContratoClicksignEnvio.id)
    .then(success => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Documento cancelado com sucesso!`,
          afterCloseCallback:  this.dialogRef.close()
        }
      });
    }, err => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'Falha ao cancelar assinatura!',
          message: `${this.formataErrosApi(err)}`
        }
      });
    });
  }

  getstatusClicksignDocumento(statusCode: number): Status {
    return statusClicksignDocumento.filter(t => t.codigo === statusCode)[0];
  }

  responsavelSolidarioExists(): boolean {
    return this.data.utilizaResponsavelSolidario;
  }

  metodoEnvioAssinaturaChange(metodoEnvioAssinatura: EMetodoEnvioAssinatura, i: number) {
    this.envioAssinaturaWhatsApp = metodoEnvioAssinatura === EMetodoEnvioAssinatura.Whatsapp;
    if(this.envioAssinaturaWhatsApp){
      this.solicitacaoAssinatura.dadosPessoaisAssinatura[i].metodoAutenticacao = EMetodoAutenticacao.Whatsapp;
    }
    this._cdr.detectChanges()
  }

  metodoEnvioAssinaturaResponsavelSolidarioChange(metodoEnvioAssinatura: EMetodoEnvioAssinatura) {
    this.envioAssinaturaWhatsApp = metodoEnvioAssinatura === EMetodoEnvioAssinatura.Whatsapp;
    if(this.envioAssinaturaWhatsApp){
      this.solicitacaoAssinatura.metodoAutenticacaoResponsavelSolidario = EMetodoAutenticacao.Whatsapp;
    }
    this._cdr.detectChanges()
  }

  metodoEnvioAssinaturaPrimeiraTestemunhaChange(metodoEnvioAssinatura: EMetodoEnvioAssinatura) {
    this.envioAssinaturaWhatsApp = metodoEnvioAssinatura === EMetodoEnvioAssinatura.Whatsapp;
    if(this.envioAssinaturaWhatsApp){
      this.solicitacaoAssinatura.metodoAutenticacaoPrimeiraTestemunha = EMetodoAutenticacao.Whatsapp;
    }
    this._cdr.detectChanges()
  }

  metodoEnvioAssinaturaSegundaTestemunhaChange(metodoEnvioAssinatura: EMetodoEnvioAssinatura) {
    this.envioAssinaturaWhatsApp = metodoEnvioAssinatura === EMetodoEnvioAssinatura.Whatsapp;
    if(this.envioAssinaturaWhatsApp){
      this.solicitacaoAssinatura.metodoAutenticacaoSegundaTestemunha = EMetodoAutenticacao.Whatsapp;
    }
    this._cdr.detectChanges()
  }

  enviaPrimeiraTestemunha(): boolean {
    return this.clicksignParametro.primeiraTestemunha != 0;
  }

  enviaSegundaTestemunha(): boolean {
    return this.clicksignParametro.segundaTestemunha != 0;
  }

  enviaVendedorPrimeiraTestemunha(): boolean {
    return this.clicksignParametro.primeiraTestemunha == 2;
  }

  enviaVendedorSegundaTestemunha(): boolean {
    return this.clicksignParametro.segundaTestemunha == 2;
  }

  enviaVendedor(): boolean {
    return this.enviaVendedorPrimeiraTestemunha() || this.enviaVendedorSegundaTestemunha();
  }

  validaMetodoAutenticacaoDadosPessoais(): boolean{
    return this.solicitacaoAssinatura.dadosPessoaisAssinatura.every(dados => dados.metodoAutenticacao !== null && dados.metodoAutenticacao !== undefined);
  }
}
