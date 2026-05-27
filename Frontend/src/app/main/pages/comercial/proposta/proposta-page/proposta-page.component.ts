import { Component, OnInit, ViewChild, ViewContainerRef, ChangeDetectionStrategy,
        OnChanges, SimpleChanges, ChangeDetectorRef, TemplateRef, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MatHeaderCell, MatCheckboxChange, throwToolbarMixedModesError } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

// **** CLASSES ****
import { Tasks } from 'app/classes/_tasks/tasks';
import { Endereco, Municipio } from 'app/classes/endereco/endereco';
import { Usina, Uso, Pedra, Slump, ResistenciaTipo, ETipoVinculoMpaConsumo, TracoPreco } from 'app/classes/traco/traco.classes';
import { Interveniente, IntervenienteTipo, intervenienteTipos } from 'app/classes/interveniente/interveniente';
import { Obra, ObraBomba, ObraTraco, ObraTaxa, TaxaTipos, ObraTributacaoMunicipal, ObraReajuste } from 'app/classes/obra/obra.classes';
import { EBombaM3CalculoTipo, EBombaHoraCalculoTipo } from 'app/classes/bomba/bomba.classes';
import { Conta, CartaoBandeira, CondicaoPagamento, Portador, TipoCobranca, Pagamento, IPagamentoDetalhe, PagamentoDetalheDinheiro,
    PagamentoDetalheCartao, PagamentoDetalheDeposito, PagamentoDetalheBoleto, PagamentoDetalheCheque } from 'app/classes/pagamento/pagamento.classes';
import { CadastroGeral, CadastroGeralViaCaptacao, ECadastroGeralViaCaptacaoTipoIndicacao } from 'app/classes/cadastro-geral/cadastro-geral';
import { CadastroDiverso } from 'app/classes/cadastro-geral/cadastro-diverso';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { Proposta, EStatusProposta, Status, statusProposta, statusComercial, statusContrato, 
    EStatusComercial, PropostaDadosPessoais, statusObraTaxa, EStatusObraTaxa } from 'app/classes/proposta/proposta.classes';
import { ParametroProposta } from 'app/classes/parametro/parametro';
import { EContratoFinalidade, EStatusContrato, medicaoContratos } from 'app/classes/contrato/contrato';
import { ObraSimplesDTO } from 'app/classes/obra/obra';
import { ContratoTracoReajuste } from 'app/classes/contrato/contrato-traco-reajuste';
import { TituloContasAReceber } from 'app/classes/titulo/titulo.classes';
import { DemaisServicos, EFrequenciaDeCobranca, EFormaDeCobrancaDemaisServicos } from 'app/classes/demais-servicos/demais-servicos';
import { Mercadoria, Unidade } from 'app/classes/mercadoria/mercadoria';
import { ObraDemaisServicos } from 'app/classes/obra/obra-demais-servicos';
import { ContratoBombaReajuste } from 'app/classes/contrato/contrato-bomba-reajuste';
import { ContratoVersao } from 'app/classes/contrato/contrato-versao';
import { VersionamentoContratoParametro } from 'app/classes/versionamento-contrato/versionamento-contrato-parametro'
import { TracoPrecoNumeracaoProduto } from 'app/classes/traco/traco-preco-numeracao-produto';
import {TributacaoPisCofins} from "../../../../../classes/tributacao-pis-cofins/tributacao-pis-cofins";
// ******************************************************************

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { PropostaService } from 'app/services/proposta.service';
import { ContratoService } from 'app/services/contrato.service';
import { CadastroGeralService } from 'app/services/cadastro-geral.service';
import { UsinaService } from 'app/services/usina.service';
import { VendedorService } from 'app/services/vendedor.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { EnderecoService } from 'app/services/endereco.service';
import { TracoPrecoService } from 'app/services/traco-preco.service';
import { SlumpService } from 'app/services/slump.service';
import { PedraService } from 'app/services/pedra.service';
import { PecaAConcretarService } from 'app/services/peca-a-concretar.service';
import { BombaPrecoService } from 'app/services/bomba-preco.service';
import { PagamentoService } from 'app/services/pagamento.service';
import { ParametroService } from 'app/services/parametro.service';
import { ObraService } from 'app/services/obra.service';
import { ObraTaxaService } from 'app/services/obra-taxa.service';
import { ProgramacaoService } from 'app/services/programacao.service';
import { TituloContasAReceberService } from 'app/services/titulo-contas-a-receber.service';
import { DemaisServicosService } from 'app/services/demais-servicos.service';
import { UnidadeService } from 'app/services/unidade.service';
import { MercadoriaService } from 'app/services/mercadoria.service';
import {TributacaoPisCofinsService} from "../../../../../services/tributacao-pis-cofins.service";
// ******************************************************************

import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from 'app/main/components/dialog/confirm-dialog/confirm-dialog.component';
import { PropostaImportacaoDialogComponent } from 'app/main/components/dialog/proposta-importacao-dialog/proposta-importacao-dialog.component';
import { ObraTributacoesMunicipaisDialogComponent } from 'app/main/components/dialog/obra-tributacoes-municipais-dialog/obra-tributacoes-municipais-dialog.component';

import { SolicitacaoPagamentoCartaoPageComponent } from '../../pagamentos/solicitacao-pagamento-cartao-page/solicitacao-pagamento-cartao-page.component';

import { ICustomValidator } from 'app/main/components/interfaces/custom-validator';
import { TracoParticularidades } from 'app/classes/traco/traco-particularidades';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';
import { CustoServicoService } from 'app/services/custo-servico.service';
import { AssinaturaEletronicaService } from 'app/services/assinatura-eletronica.service';
import { FilialService } from 'app/services/filial.service';
import { Filial, ModeloDocumentoRemessaConcreto } from 'app/classes/filial/Filial';
import { CadastroDiversoService } from 'app/services/cadastro-diverso.service';
import { CDK_CONNECTED_OVERLAY_SCROLL_STRATEGY_PROVIDER_FACTORY } from '@angular/cdk/overlay/typings/overlay-directives';
import { Console } from 'console';
import { SelectorFlags } from '@angular/compiler/src/core';
import { formatDate } from '@angular/common';
import { ObraFrente } from 'app/classes/obra/obra-frente';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';
import { OportunidadeService } from 'app/services/oportunidade.service';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';
import { EObraTracoStatus, ObraTracoStatus, ObraTracoStatusResponse } from 'app/classes/traco/traco-status';

export interface EdicaoObraFrente {
  enderecoNome: string;
  enderecoLogradouro: string;
  enderecoNumero: number;
  enderecoComplemento: string;
  enderecoBairro: string;
  enderecoCep: string;
}

const LOADING_MESSAGE: string = '[CARREGANDO...]';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-proposta-page',
  templateUrl: './proposta-page.component.html',
  styleUrls: ['./proposta-page.component.scss']
})
export class PropostaPageComponent implements OnInit, OnChanges {

  public static self: PropostaPageComponent;

  geralForm: FormGroup;
  clienteForm: FormGroup;
  responsavelSolidarioForm: FormGroup;
  obraForm: FormGroup;
  concretoForm: FormGroup;
  bombaForm: FormGroup;
  demaisServicosForm: FormGroup;
  taxasExtrasForm: FormGroup;
  pagamentoForm: FormGroup;
  concretoItemForm: FormGroup;
  bombaItemForm: FormGroup;
  demaisServicosItemForm: FormGroup;
  taxaItemForm: FormGroup;
  pagamentoItemForm: FormGroup;
  pagamentoItemDetalheForm: FormGroup;
  solicitaAprovacoesForm: FormGroup;
  obraFrenteForm: FormGroup;

  cadObraFrente: EdicaoObraFrente;
  cadObraFrenteNovo: boolean;
  cadObraFrenteSelecionado: ObraFrente;

  parametroPropostaNova: ParametroProposta = new ParametroProposta();
  
  travarSegmentacao: boolean;

  clicouVendedor: boolean = false;
  vendedorAtualProposta: Vendedor = null;

  get statusProposta(): number[] {
    let codigos: number[] = [];
    statusProposta.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos;
  };
  get statusPropostaColors(): string[] {
    let colors: string[] = [];
    statusProposta.forEach(status => {
      colors.push(status.color);
    });
    return colors;
  };
  get statusComercial(): number[] {
    let codigos: number[] = [];
    statusComercial.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos;
  };
  get statusComercialColors(): string[] {
    let colors: string[] = [];
    statusComercial.forEach(status => {
      colors.push(status.color);
    })
    return colors;
  };
  get statusContrato(): number[] {
    let codigos: number[] = [];
    statusContrato.forEach(status => {
      codigos.push(status.codigo);
    })
    return codigos;
  };
  get statusContratoColors(): string[] {
    let colors: string[] = [];
    statusContrato.forEach(status => {
      colors.push(status.color);
    })
    return colors;
  };
  get intervenienteTipos(): string[] {
    let codigos: string[] = [];
    intervenienteTipos.forEach(intervTipo => {
      codigos.push(intervTipo.codigo);
    })
    return codigos;
  };

  get medicaoContratos(): string[] {
    let codigos: string[] = [];
    medicaoContratos.forEach(medicao => {
      codigos.push(medicao.codigo);
    })
    return codigos;
  };

  usinas: Usina[] = [];
  usos: Uso[] = [];
  pedras: Pedra[] = [];
  //slumps: Slump[] = [];
  slumpsNominais: Slump[] = [];
  resistenciaTipos: ResistenciaTipo[] = [];
  mpas: number[] = [];
  consumos: number[] = [];
  pecasConcretar: string[] = [];
  vendedores: Vendedor[] = [];
  vendedoresPermitidos: Vendedor[] = [];
  vendedoresVinculados: Vendedor[] = [];
  viasCaptacao: CadastroGeral[] = [];
  tipoObra: CadastroGeral[] = [];
  porteObra: CadastroGeral[] = [];
  bombaTipos: CadastroGeral[] = [];

  itemSemDefinicao: CadastroGeral = { codigo: 0, descricao: 'Sem definição', viaCaptacao: new CadastroGeralViaCaptacao() };
  temposAprovacao: CadastroGeral[] = [];

  bombistasTerceiros: Interveniente[] = [];
  numeracoesProduto: TracoPrecoNumeracaoProduto[] = [];

  unidades: Unidade[] = [];
  demaisServicos: DemaisServicos[] = [];
  get mercadorias(): Mercadoria[] {
    return this.demaisServicos.map(t => t.mercadoria);
  };

  segmentos: Segmentacao[] = [];
  condicoesPagamento: CondicaoPagamento[] = [];
  tiposCobranca: TipoCobranca[] = [];
  bandeiras: CartaoBandeira[] = [];
  portadores: Portador[] = [];
  finalidades: ContratoFinalidade[] = [];

  funcoes: CadastroGeral[] = [];

  tributacoes: TributacaoPisCofins[] = [];
  
  taxaOpcoesPedra: string[] = [];
  taxaOpcoesResistenciaDe: string[] = [];
  taxaOpcoesResistenciaPara: string[] = [];
  taxaOpcoesSlump: string[] = [];
  taxaOpcoesHorario: string[] = [];
  taxaOpcoesQuandoAte: string[] = [];
  taxaOpcoesQuandoDe: string[] = [];
  taxaOpcoesQuandoOperacao: string[] = [];
  taxaOpcoesTipo: string[] = [];
  taxaOpcoesTipoPessoa: CadastroDiverso[] = [];
  taxaOpcoesTipoPessoaCodigos: string[] = [];
  taxaOpcoesTipoValor: string[] = [];
  taxaOpcoesValorPor: string[] = [];
  taxaOpcoesCobrarVolume: string[] = [];
  taxaOpcoesVolume: string[] = [];
  taxaOpcoesAntecedencia: string[] = [];

  proposta: Proposta = new Proposta();
  isInsertMode: boolean = true;
  isTracoInsertMode: boolean = false;
  obraTracosPossuiStatusCustoVirtual: boolean = false;
  obraTracosStatus: ObraTracoStatusResponse = new ObraTracoStatusResponse();

  traco: ObraTraco = new ObraTraco();
  tracoShowContent: boolean = false;
  indexTraco: number = -1;

  tracoReajuste: ContratoTracoReajuste = new ContratoTracoReajuste();
  tracoReajusteShowContent: boolean = false;
  indexTracoReajuste: number = -1;

  bomba: ObraBomba = new ObraBomba();
  bombaShowContent: boolean = false;
  indexBomba: number = -1;

  itemDemaisServicos: ObraDemaisServicos = new ObraDemaisServicos();
  demaisServicosShowContent: boolean = false;
  indexDemaisServicos: number = -1;

  taxa: ObraTaxa = new ObraTaxa();
  taxaShowContent: boolean = false;
  indexTaxa: number = -1;

  pagamento: Pagamento = new Pagamento();
  pagamentoShowContent: boolean = false;
  indexPagamento: number = -1;

  utilizaGoogleMatrixAPI: boolean = true;

  modalIsOpen: boolean = false;
  private _dialogRef: MatDialogRef<any>;

  enderecoLogradouroAnterior: string = "";
  enderecoNumeroAnterior: number;
  
  temDireitoAlteracao: boolean = true;
  temDireitoCadastro: boolean = false;
  temDireitoInclusaoCliente: boolean = false;
  temDireitoAlteracaoCliente: boolean = false;
  temDireitoAlteracaoExternalId: boolean = false;
  temDireitoAprovarCadastro: boolean = false;
  temDireitoAlteracaoObservacaoTraco: boolean = false;
  temDireitoAlteracaoMedicaoContrato: boolean = false;

  possuiIntegracaoCartao: boolean = false;

  naoConsideraTodasBombas: boolean = false;

  utilizarIdentificadorClienteAoInvesDeCpfCnpj: boolean = false;

  utilizaCalculoPrecoTabelaPorUsina: boolean = false;

  bloqueiaCampoTaxaMinimaNoPrecoDeBomba: boolean = false;

  obraReajuste: ObraReajuste = new ObraReajuste();

  tracoParticularidades:TracoParticularidades = new TracoParticularidades();

  tracoDescricao: string = "";

  intervenientes: Interveniente[] = [];

  tracoModalNumeracaoProdutoSelecionado: TracoPrecoNumeracaoProduto;

  tracoJaIncluso: boolean;

  campoObrigatorioTelefone: number;
  campoObrigatorioInscricaoEstadual: string;
  campoObrigatorioRazao: string;
  campoObrigatorioNomeReduzido: string;
  campoObrigatorioEndereco: Endereco;
  campoObrigatorioProfissao: string;
  campoObrigatorioNomeContato: string;
  campoObrigatorioEmail: string;

  abaObrigatoriaRevisarTaxaExtra: boolean = false;

  filiais: Filial[] = [];
  modelosDocumentosRemessa: ModeloDocumentoRemessaConcreto[] = [];
  modelosItensDanfeERomaneio: ModeloDocumentoRemessaConcreto[] = [];

  custoServico: CustoServico = new CustoServico();
  tracoExibicaoEbitda: ObraTraco = new ObraTraco();
  bombaExibicaoEbitda: ObraBomba = new ObraBomba();

  volumeTotalConsumido: number = 0;

  contratoVersoes: ContratoVersao[] = [];
  versaoAtual: number = 0;
  contratoVersaoAtual: ContratoVersao;
  versionamentoContratoParametro: VersionamentoContratoParametro = new VersionamentoContratoParametro();

  displayedColumnsObraFrente: string[] = ['frente-nome', 'frente-endereco', 'frente-edicao', 'frente-deletar'];
  displayedFooterObraFrente: string[] = this.proposta ? (this.proposta.obra ? (this.proposta.obra.obraFrentes.length === 0 ? ['frente-footer'] : []) : []) : [];

  opcoesAdicionalZMRC: string[] = ['S', 'N'];

  intervenienteIndicadorTipo: string = 'F';
  intervenienteIndicadorCpfCnpj: string = "";

  finalidadeSelecionada = this.finalidades[0];

  errorMessages: {
    obraCep: {key: string, message: string}[],
    obraDistanciaUsina: {key: string, message: string}[]
  } = {
    obraCep: [],
    obraDistanciaUsina: []
  };
  private _valorAdicionalM3PorUsinaCep: number = 0;
  private _valorAdicionalM3PorUsinaKm: number = 0;

  parametroProposta: ParametroProposta = new ParametroProposta();

  desabilitaNumeracaoProduto: boolean = false;
  desabilitaCamposTraco: boolean = false;

  maskCEI = [/\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, /\d/, /\d/,'/',  /\d/, /\d/];

  @ViewChild('tracoModalVCR', { read: ViewContainerRef, static: false }) tracoModalVCR: ViewContainerRef;
  @ViewChild('bombaModalVCR', { read: ViewContainerRef, static: false }) bombaModalVCR: ViewContainerRef;
  @ViewChild('demaisServicosModalVCR', { read: ViewContainerRef, static: false }) demaisServicosModalVCR: ViewContainerRef;
  @ViewChild('taxaModalVCR', { read: ViewContainerRef, static: false }) taxaModalVCR: ViewContainerRef;
  @ViewChild('pagamentoItemModalVCR', { read: ViewContainerRef, static: false }) pagamentoItemModalVCR: ViewContainerRef;
  @ViewChild('pagamentoItemDetalheModalVCR', { read: ViewContainerRef, static: false }) pagamentoItemDetalheModalVCR: ViewContainerRef;
  
  @ViewChild('solicitaAprovacoesModalVCR', { read: ViewContainerRef, static: false }) solicitaAprovacoesModalVCR: ViewContainerRef;
  @ViewChild('solicitaAprovacoesModal', { static: false }) solicitaAprovacoesModal: TemplateRef<any>;
  @ViewChild('ebitdaTracoModalVCR', { read: ViewContainerRef, static: false }) ebitdaTracoModalVCR: ViewContainerRef;
  @ViewChild('ebitdaBombaModalVCR', { read: ViewContainerRef, static: false }) ebitdaBombaModalVCR: ViewContainerRef;
  @ViewChild('obraFrenteCadastroModalVCR', { read: ViewContainerRef, static: false }) obraFrenteCadastroModalVCR: ViewContainerRef;
  @ViewChild('modalVCR', { read: ViewContainerRef, static: false }) ModalRef: ViewContainerRef;

  constructor(
      private _formBuilder: FormBuilder,
      public dialog: MatDialog,
      private _cdr:ChangeDetectorRef,
      private _userService: UserService,
      private _propostaService: PropostaService,
      private _contratoService: ContratoService,
      private _cadastroGeralService: CadastroGeralService,
      private _usinaService: UsinaService,
      private _vendedorService: VendedorService,
      private _intervenienteService: IntervenienteService,
      private _enderecoService: EnderecoService,
      private _tracoPrecoService: TracoPrecoService,
      private _slumpService: SlumpService,
      private _pedraService: PedraService,
      private _pecaAConcretarService: PecaAConcretarService,
      private _bombaPrecoService: BombaPrecoService,
      private _pagamentoService: PagamentoService,
      private _parametroService: ParametroService,
      private _obraService: ObraService,
      private _obraTaxaService: ObraTaxaService,
      private _programacaoService: ProgramacaoService,
      private _contasAReceberService: TituloContasAReceberService,
      private _demaisServicosService: DemaisServicosService,
      private _mercadoriaService: MercadoriaService,
      private _unidadeService: UnidadeService,
      private _route: ActivatedRoute,
      private _router: Router,
      private _custoServicoService: CustoServicoService,
      private _filialService: FilialService,
      private _cadastroDiversoService: CadastroDiversoService,
      private _segmentacaoService: SegmentacaoService,
      private _tributacaoPisCofins: TributacaoPisCofinsService,
      private _oportunidadeService: OportunidadeService,
      private location: Location
    ) {
    PropostaPageComponent.self = this;

    this.temDireitoCadastro = (this._userService.temDireitoAplicativo('WEB6127','') || this._userService.temDireitoAplicativo('WEB6118',''))
    
    this.temDireitoAprovarCadastro = this._userService.temDireitoAplicativo('WEB6118','');

    this.temDireitoInclusaoCliente = this._userService.temDireitoAplicativo('WEB6001', 'I');
    this.temDireitoAlteracaoCliente = this._userService.temDireitoAplicativo('WEB6001','A');
    this.temDireitoAlteracaoExternalId = this._userService.temDireitoAplicativo('CON7036','A');
    
    this.temDireitoAlteracaoMedicaoContrato = this._userService.temDireitoAplicativo('WEB7008', '');

    if (this._route.routeConfig.path.endsWith('/proposta/nova')) {
      var temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6101','I', 200);
      if (!temDireitoInclusao) return;
    } else {
      this.temDireitoAlteracao = this._userService.temDireitoAplicativo('WEB6101','A') || this.temDireitoCadastro;
    }

    this._userService.gravarAcessoAplicacao("Comercial", 6101);
    this.temDireitoAlteracaoObservacaoTraco = this._userService.temDireitoAplicativo('WEB6102','A');
	
    _usinaService.listarListarUsinasPermitidasUsuario().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );
    _vendedorService.listarAtivos().then(
      vendedores => { this.vendedores = vendedores },
      error => { this.vendedores = [] }
    );
    _vendedorService.listarPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores },
      error => { this.vendedoresPermitidos = [] }
    );
    _vendedorService.listarVinculados().then(
      vendedores => {
        this.vendedoresVinculados = vendedores;
        this.setVendedorVinculado();
        this.detectChanges();
      },
      error => { this.vendedoresVinculados = [] }
    );
    _cadastroGeralService.listarViasCaptacao().then(
      viasCaptacao => { this.viasCaptacao = viasCaptacao },
      error => { this.viasCaptacao = [] }
    );
    _cadastroGeralService.listarTipoObra().then(
      tipoObra => { this.tipoObra = tipoObra },
      error => { this.tipoObra = [] }
    );
    _cadastroGeralService.listarPorteObra().then(
      porteObra => { this.porteObra = porteObra },
      error => { this.porteObra = [] }
    );

    _cadastroGeralService.listarTemposAprovacaoMedicao().then(
      temposAprovacao => { this.temposAprovacao = [this.itemSemDefinicao, ...temposAprovacao] },
      error => { this.temposAprovacao = [] }
    );

    _cadastroGeralService.listarFuncoes().then(
      funcoes => { this.funcoes = funcoes },
      error => { this.funcoes = [] }
    );
    _pecaAConcretarService.listarTodos().then(
      pecasAConcretar => { this.pecasConcretar = pecasAConcretar },
      error => { this.pecasConcretar = [] }
    );
    _pagamentoService.ListarCartaoBandeiras().then(
      bandeiras => { this.bandeiras = bandeiras },
      error => { this.bandeiras = [] }
    );
    _pagamentoService.ListarPortadoresVinculadosComContas().then(
      portadores => { this.portadores = portadores },
      error => { this.portadores = [] }
    );
    _parametroService.obterParametroPropostaPorDataBase(new Date()).then(
      parametroProposta => { this.parametroProposta = parametroProposta },
      error => { this.parametroProposta = new ParametroProposta() }
    );
    _pagamentoService.ListarCartaoBandeirasComIntegracao().then(
      bandeiras => { this.possuiIntegracaoCartao = bandeiras.length > 0; },
      error => { this.possuiIntegracaoCartao = false }
    );
    _parametroService.obterParametoN("TopCon","NaoConsideraTodasBombas").then(
      parametro => { this.naoConsideraTodasBombas = parametro==="1"},
      error => {this.naoConsideraTodasBombas = false}
    );

    _parametroService.obterParametoN("TopCon", "UtilizarIdentificadorClienteAoInvesDeCpfCnpj").then(
      parametro => { this.utilizarIdentificadorClienteAoInvesDeCpfCnpj = parametro === "1"},
      error => {this.utilizarIdentificadorClienteAoInvesDeCpfCnpj = false}
    );

    _parametroService.obterParametoN("web", "UtilizaCalculoPrecoTabelaPorUsina").then(
      parametro => { this.utilizaCalculoPrecoTabelaPorUsina= parametro === "1" },
      error => { this.utilizaCalculoPrecoTabelaPorUsina = false }
    )

    _parametroService.obterParametoN("web", "BloqueiaCampoTaxaMinimaNoPrecoDeBomba").then(
      parametro => { this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba= parametro === "1" },
      error => { this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba = false }
    )

    _filialService.listar().then(
      lista => { this.filiais = lista; },
      error => { this.filiais = []; }
    );

    _segmentacaoService.listarTodos().then(
      lista => { this.segmentos = lista; },
      error => { this.segmentos = [] }
    )

    _contratoService.ListarFinalidades().then(
      finalidades => { this.finalidades = finalidades; },
      error => { this.finalidades = [] }
    )

    _cadastroDiversoService.listarModeloDocumentoRemessaConcreto().then(
      modelos => { modelos.forEach((modelo) => { 
        var newModelo = new ModeloDocumentoRemessaConcreto();
        newModelo.codigo = parseInt(modelo.codigo);
        newModelo.descricao = modelo.descricao;
        this.modelosDocumentosRemessa.push(newModelo); 
      }) },
      error => { this.modelosDocumentosRemessa = []; }
    );

    _tributacaoPisCofins.listarTributacoesDeSaida().then(
        tributacoes => { this.tributacoes = tributacoes },
        error => { this.tributacoes = [] }
    );

    const opcoesmodelosDescricaoItensDanfeRomaneio: ModeloDocumentoRemessaConcreto[] = [
      { codigo: 0, descricao: 'Padrão' },
      { codigo: 1, descricao: 'Materiais' },
      { codigo: 2, descricao: 'Traço' }
    ]
  
    opcoesmodelosDescricaoItensDanfeRomaneio.forEach(modelo => {      
      this.modelosItensDanfeERomaneio.push(modelo);
    });
    
    this.setDataValidadeProposta();
  }


  ngOnInit() {
    document.addEventListener("visibilitychange", () => {
      if (document.hidden)
        this.saveProgress();
    });

    this.geralForm = this._formBuilder.group({});
    this.clienteForm = this._formBuilder.group({});
    this.responsavelSolidarioForm = this._formBuilder.group({
      utilizaResponsavelSolidario: ['']
    });
    this.obraForm = this._formBuilder.group({
      utilizaEnderecoClienteComoEnderecoObra: ['']
    });
    this.concretoForm = this._formBuilder.group({});
    this.bombaForm = this._formBuilder.group({});
    this.demaisServicosForm = this._formBuilder.group({});
    this.taxasExtrasForm = this._formBuilder.group({});
    this.pagamentoForm = this._formBuilder.group({});
    this.concretoItemForm = this._formBuilder.group({});
    this.bombaItemForm = this._formBuilder.group({
      bombaPropria: [''],
      faturamentoDireto: [''],
      alugadaPeloCliente: ['']
    });
    this.demaisServicosItemForm = this._formBuilder.group({
      atualizaEstoque: ['']
    });
    this.taxaItemForm = this._formBuilder.group({});
    this.pagamentoItemForm = this._formBuilder.group({});
    this.pagamentoItemDetalheForm = this._formBuilder.group({});
    this.solicitaAprovacoesForm = this._formBuilder.group({});
    this.obraFrenteForm = this._formBuilder.group({});

    let idUsina = parseFloat(this._route.snapshot.paramMap.get('idUsina'));
    let ano = parseFloat(this._route.snapshot.paramMap.get('ano'));
    let numero = parseFloat(this._route.snapshot.paramMap.get('numero'));
    if (idUsina && ano && numero) {

      var gerarPropostaDeOportunidade = (this._route.routeConfig.path.endsWith('/gerar-proposta'))

      if (!gerarPropostaDeOportunidade) {
        this._propostaService.ObterPorUsinaAnoNumero({codigo: idUsina, nome: '', sigla: '', filialCodigo: 0, tempoBtAteAObra:0 ,tempoBtNaObra:0, porcentagemRetornoObra:0 ,prazoPesagem:0}, ano, numero).then(
          proposta => {
            this.isInsertMode = false;
            this.proposta = proposta;
            
            this.vendedorAtualProposta = proposta.vendedor;
            this.setFinalidadePadrao();
            this.getObraTracoStatus();
  
            if(this.proposta.obra.obraReajuste == undefined || this.proposta.obra.obraReajuste.mensagemReajuste == ""){
              this.proposta.obra.obraReajuste = new ObraReajuste();
              this.proposta.obra.obraReajuste.mensagemReajuste = this.parametroProposta.mensagemReajustePadrao;
            }
  
            this.proposta.obra.obraTracos.forEach(traco => {
              this._obraService.obterConsumoTracoPorContrato(this.proposta.obra, traco).then(volumeConsumido => {
                traco.m3Consumido = volumeConsumido;
                this.volumeTotalConsumido += volumeConsumido;
              });
            });
  
            /*this._programacaoService.ObterVolumeTotalProgramadoProposta(idUsina, this.proposta.obra.numero).then(volumeTotalProgramado => { //aqui
              this.volumeTotalConsumido = volumeTotalProgramado;});*/
            
            if ( (this.proposta.intervenienteTipo === 'F' && this.proposta.cpfCnpj.length === 11)
                || (this.proposta.intervenienteTipo !== 'F' && this.proposta.cpfCnpj.length === 14) ) {
              this._intervenienteService.obterPermitidoPorCpfCnpj(this.proposta.cpfCnpj,this.proposta.inscricaoEstadual)
              .then(interveniente => {
                if (interveniente) {                
                  this.carregaDadosInterveniente(interveniente);
                }
              })
              .then(() => {
                this.detectChanges();
              });
            }
  
            this.contratoVersoes = [];
            this.versaoAtual = 0;
            if (this.proposta.usina.codigo) {
              this._contratoService.ListarVersoesContrato(this.proposta.usina.codigo, this.proposta.obra.anoContrato, this.proposta.obra.numContrato,true)
              .then(contratoVersoes => {
                this.contratoVersoes = contratoVersoes.filter(t => t !== null);
        
                if (this.contratoVersoes.length > 0) {
                  this.versaoAtual = Math.max(...this.contratoVersoes.map(v => v.numeroVersao));
                  this.contratoVersaoAtual = this.contratoVersoes.find(v => v.numeroVersao === this.versaoAtual);
  
                  if (this.contratoVersaoAtual.dataVersaoCriada == null)
                    this.contratoVersaoAtual.dataVersaoCriada = this.proposta.obra.contrato.dataContrato;
                }
              }, err => this.contratoVersoes = [])
              .then(() => {
                this.detectChanges();
              });
            }
            
            this._contratoService.ObterContratoVersaoParametro().then(
              parametro => { 
                if (parametro !== null) this.versionamentoContratoParametro = parametro; },
              error => { this.versionamentoContratoParametro = new VersionamentoContratoParametro(); }
            );
  
            this.validaDistanciaKmUsinaCepAprovada(this.proposta.obra.usinaEntrega, this.proposta.obra.endereco.cep, this.proposta.obra.distanciaUsina);
            
            if(this.proposta.obra.indicador) {
              if(this.proposta.obra.indicador.interveniente) {
                this.intervenienteIndicadorCpfCnpj = this.proposta.obra.indicador.interveniente.cpfCnpj;
                this.intervenienteIndicadorTipo = this.proposta.obra.indicador.interveniente.intervenienteTipo;
              }
            }
            
            if (this.proposta.tempoAprovacaoMedicao != 0)
              this.proposta.tempoAprovacaoMedicaoCadastro = this.temposAprovacao.filter(e => Number(e.descricao) === this.proposta.tempoAprovacaoMedicao)[0];
            else
              this.proposta.tempoAprovacaoMedicaoCadastro = this.temposAprovacao[0];
  
            this.detectChanges();
          },
          error => {
            this.proposta = new Proposta();
            this.setVendedorVinculado();
            this.setSegmentoPadrao();
            this.setFinalidadePadrao();

            this.setMedicaoContratoPadrao();
            this.setTempoAprovacaoMedicaoPadrao();

            this.detectChanges();
          }
        )
        .then(() => { this.detectChanges(500); });
      }

      if(gerarPropostaDeOportunidade) {
        this._oportunidadeService.ObterPropostaDeOportunidade(idUsina, ano, numero).then(
            proposta => {
              this.isInsertMode = true;
              this.proposta = proposta;

              this.proposta.data = new Date();
              this.proposta.cpfCnpj = "";
              this.proposta.obra.obraReajuste = new ObraReajuste();

              if(this.proposta.intervenienteCodigo > 0){
                this._intervenienteService.obterPorCodigoInterveniente(this.proposta.intervenienteCodigo)
                .then(interveniente => {
                  if (interveniente) {
                    this.carregaDadosInterveniente(interveniente);
          
                    var vendedor = interveniente.vendedor ? interveniente.vendedor.codigo : 0;
                    if (vendedor !== 0 && vendedor !== this.proposta.vendedor.codigo) {
                      var self = PropostaPageComponent.self;
                      self.dialog.open(AlertDialogComponent, {
                        disableClose: true,
                        data: {
                          title: 'TopConWeb',
                          message: `O cliente é do vendedor: ${self.vendedorFormatter(interveniente.vendedor)}`
                        }
                      });
                    }
          
                  } else {
                    this.proposta.intervenienteCodigo = 0;
                    if (this.proposta.interveniente) this.proposta.interveniente.codigo = 0;
                    else this.proposta.interveniente = new Interveniente();
                  }
                });
              }

              if(!this.proposta.obra.usinaEntrega && this.proposta.obra.endereco.cep != "", this.proposta.obra.distanciaUsina > 0)
                this.validaDistanciaKmUsinaCepAprovada(this.proposta.obra.usinaEntrega, this.proposta.obra.endereco.cep, this.proposta.obra.distanciaUsina);
              
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
              this.proposta = new Proposta();
              this.setVendedorVinculado();
              this.setSegmentoPadrao();
              this.setFinalidadePadrao();
              
              this.setMedicaoContratoPadrao();
              this.setTempoAprovacaoMedicaoPadrao();

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
              message: 'Existe uma proposta já iniciada. Deseja continuar?',
              confirmCallback: () => {
                this.proposta = savedProgress;
                this.setFinalidadePadrao();
                this.getObraTracoStatus();
                this.travarSegmentacao = true;
                this.usaAdicionalZmrcChange();
                cdr.detectChanges();
              },
              cancelCallback: () => {
                this.clearSavedProgress();
                this.proposta = new Proposta();
                this.setVendedorVinculado();
                this.setSegmentoPadrao();
                this.setFinalidadePadrao();

                this.setMedicaoContratoPadrao();
                this.setTempoAprovacaoMedicaoPadrao();

                this.detectChanges();
              }
            }
          });
        }, 500);
      } else {
        this.proposta = new Proposta();
        this.setVendedorVinculado();
        this.setSegmentoPadrao();
        this.setFinalidadePadrao();

        this.setMedicaoContratoPadrao();
        this.setTempoAprovacaoMedicaoPadrao();

        this.detectChanges();
      }
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    this.detectChanges();
  }

  detectChanges(dalay: number = 0) {
    if (dalay)
      setTimeout(() => { this._cdr.detectChanges(); }, dalay);
    else
      this._cdr.detectChanges();
  }

  getTracoStatusString(traco: ObraTraco) {

    if(traco.status === EObraTracoStatus.CustoVirtual)
      return '[C. VIRT]';

    return '';
  }

  getObraTracoStatus() {
    if(this.proposta.obra.obraTracos.length === 0) {
      this.obraTracosPossuiStatusCustoVirtual = false;
    } else {
      if(this.proposta.obra.usinaEntrega) {
        this._tracoPrecoService.VerificarObraPossuiTracoStatusCustoVirtual(new Date(), this.proposta.obra.usinaEntrega, this.proposta.obra)
        .then(result => {
          this.obraTracosPossuiStatusCustoVirtual = result.possuiCustoVirtual;
          this.obraTracosStatus = result;

          this.obraTracosStatus.tracos.forEach(status => {

            var obraTraco = this.proposta.obra.obraTracos.filter(x => x.sequencia === status.sequencia)[0];

            if(obraTraco)
              obraTraco.status = status.status;

          });

        });
      } else {
        this.obraTracosPossuiStatusCustoVirtual = false;
        this.obraTracosStatus = new ObraTracoStatusResponse();
      } 
    }
  }

  setVendedorVinculado() {
    if (this.vendedoresVinculados.length===1) {
      this.proposta.vendedor = this.vendedoresVinculados[0];
    }  

  }

  async setFinalidadePadrao() {

    if(this.finalidades.length == 0)
      await this.carregaFinalidades();

    if(!this.proposta.contratoFinalidade)
      this.proposta.contratoFinalidade = EContratoFinalidade.PrestacaoServico;

    this.finalidadeSelecionada = this.finalidades.filter(x => x.codigo === this.proposta.contratoFinalidade)[0];

  }

  async setSegmentoPadrao() {
    
    if(this.segmentos.length == 0)
      await this.carregaSegmentacoes();

    if(!this.proposta.segmento) {
      this.proposta.segmentacao = 1;
      this.proposta.segmento = this.segmentos.filter(e => e.id===1)[0];
    }
  }

  async setMedicaoContratoPadrao() {
    if (!this.proposta.aprovacaoMedicao) {
      this.proposta.aprovacaoMedicao = 'N';
    }
  }

  async setTempoAprovacaoMedicaoPadrao() {
    if (!this.proposta.tempoAprovacaoMedicaoCadastro) {
      this.proposta.tempoAprovacaoMedicao = 0;
      this.proposta.tempoAprovacaoMedicaoCadastro = this.temposAprovacao.filter(e => e.codigo===0)[0];
    }
  }

  async setDataValidadeProposta() {          
    let dataAtual = new Date();
    await this._parametroService.obterParametroPropostaPorDataBase(new Date()).then(
      parametroProposta => { this.parametroPropostaNova = parametroProposta },
      error => { this.parametroProposta = new ParametroProposta() }
    );
   
    if (this.parametroPropostaNova.validadeProposta > 0) { 
      dataAtual.setDate(dataAtual.getDate() + this.parametroPropostaNova.validadeProposta);
      this.proposta.validadeProposta = dataAtual;      
    }
  }

  get demaisServicosValorMinimoValidator(): ICustomValidator {
    var _self = PropostaPageComponent.self;
    
    var message = `Valor abaixo do permitido! (mínimo: ${Tasks.formataMoeda(_self.itemDemaisServicos.precoMinimo)})`;

    return {
      key: 'valorAbaixoDoMinimo',
      message: message,
      validatorFunction: (itemDemaisServicos: ObraDemaisServicos): boolean => {
        return itemDemaisServicos.precoProposto < itemDemaisServicos.precoMinimo;
      },
      params: [_self.itemDemaisServicos]
    }
  }

  get temDireitoAprovacaoComercial(): boolean {
    return this._userService.temDireitoAplicativo('WEB6998','');  
  }

  get temDireitoAcessoMargemPosTransporte(): boolean{
    return this._userService.temDireitoAplicativo('WEB6103', '');
  }

  get temDireitoAlteracaoDataEncerramentoContrato(): boolean {
    const { _userService } = this;

    return _userService.temDireitoAplicativo('WEB6118', '') ||
    _userService.temDireitoAplicativo('WEB6309', '');
  }

  get intervenienteValidator(): ICustomValidator {
    var _self = PropostaPageComponent.self;
    var _tasks = Tasks;
    var interveniente = _self.proposta.interveniente;

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
      params: [_self.proposta.interveniente, _self.proposta.cpfCnpj]
    }
  }

  m3AtePrecoPropostoValorMinimoValidator(bomba: ObraBomba): ICustomValidator {
    var message = `Valor abaixo do permitido! (mínimo: ${bomba.m3TabelaAte})`;

    return {
      key: 'valorAbaixoDoMinimo',
      message: message,
      validatorFunction: (bomba: ObraBomba): boolean => {
        return this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba && bomba.m3PropostoAte < bomba.m3TabelaAte;
      },
      params: [bomba]
    }
  }

  horasAtePrecoPropostoValorMinimoValidator(bomba: ObraBomba): ICustomValidator {
    var message = `Valor abaixo do permitido! (mínimo: ${bomba.horaTabelaAte})`;

    return {
      key: 'valorAbaixoDoMinimo',
      message: message,
      validatorFunction: (bomba: ObraBomba): boolean => {
        return this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba && bomba.horaPropostoAte < bomba.horaTabelaAte;
      },
      params: [bomba]
    }
  }

  vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  segmentacaoFormatter = (model: Segmentacao): string => model ? model.nome.toUpperCase() : '';
  cadastroGeralFormatter = (model: CadastroGeral): string => model ? model.descricao.toUpperCase() : '';
  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  usoFormatter = (model: Uso): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  pedraFormatter = (model: Pedra): string => model ? model.descricao.toUpperCase() : '';
  slumpFormatter = (model: Slump): string => model ? model.descricao.toUpperCase() : '';
  resistenciaTipoFormatter = (model: ResistenciaTipo): string => model ? model.descricao.toUpperCase() : '';
  mpaFormatter = (model): string => model ? this.formataValor(model, 1, false) : '';
  consumoFormatter = (model): string => model ? model.toString() : '';
  numeracaoProdutoFormatter = (model: TracoPrecoNumeracaoProduto): string => {
    if (model===null || model===undefined || model.numeracao === 0) return '';
    
    if(!model)
      return '';

    if(model.status == 7105)
      return ('[CUSTO VIRTUAL] ' + model.numeracao + ' - '+ model.usoDescricao).toUpperCase();

    return model ? (model.numeracao+' - '+model.usoDescricao).toUpperCase() : '';
  };
  pecaConcretarFormatter = (model): string => model ? model.toString() : '';
  intervenienteFormatter = (model: Interveniente): string => model ? (model.codigo > 0 ? (model.codigo+' - '+(model.razao || model.nome)).toUpperCase() :'') : '';
  statusPropostaFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.statusProposta.includes(model) ? statusProposta.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  };
  statusComercialFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.statusComercial.includes(model) ? statusComercial.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  };
  statusContratoFormatter = (model: number): string => {
    if (model===null || model===undefined || isNaN(model)) return '';
    return this.statusContrato.includes(model) ? statusContrato.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  };
  intervenienteTipoFormatter = (model: string): string => {
    if (!model) return '';
    return this.intervenienteTipos.includes(model) ? intervenienteTipos.filter(e => e.codigo===model)[0].descricao : '';
  };
  medicaoContratoFormatter = (model: string): string => {
    if (!model) return '';
    return medicaoContratos.filter(e => e.codigo===model)[0].descricao.toUpperCase();
  };

  condicaoPagamentoFormatter = (model: CondicaoPagamento): string => model ? model.descricao.toUpperCase() : '';
  tipoCobrancaFormatter = (model: TipoCobranca): string => model ? model.descricao.toUpperCase() : '';
  bandeiraFormatter = (model: CartaoBandeira): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  portadorFormatter = (model: Portador): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  bancoFormatter = (model: Conta): string => model ? (model.bancoCodigoOficial+' - '+model.nome).toUpperCase() : '';
  numeroContaFormatter = (model: Conta): string => model ? (model.numeroConta+'-'+model.dvConta).toUpperCase() : '';
  taxaTipoPessoaFormatter = (model: string): string => {
    return this.taxaOpcoesTipoPessoaCodigos.includes(model) ? this.taxaOpcoesTipoPessoa.filter(e => e.codigo===model)[0].descricao : '';
  };
  bombaTipoFormatter = (model: CadastroGeral): string => 
    model ? model.descricao.toUpperCase().replace('BOMBA ', '').replace('BRITA ', 'B') : '';
  unidadeFormatter = (model: Unidade): string => model ? ('('+model.sigla+') '+model.descricao).toUpperCase() : '';
  mercadoriaFormatter = (model: Mercadoria): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  OpcoesAdicionalZMRCFormatter = (model: string): string => model === "S" ? "SIM" : "NÃO";
  tributacaoPisCofinsFormatter = (model: TributacaoPisCofins): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  finalidadeFormatter = (model: any): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';

  get frequenciaCobrancaLista(): EFrequenciaDeCobranca[] {
    return [
      EFrequenciaDeCobranca.Bombeamento,
      EFrequenciaDeCobranca.Contrato,
      EFrequenciaDeCobranca.M3,
      EFrequenciaDeCobranca.M3Bombeado,
      EFrequenciaDeCobranca.Programacao,
      EFrequenciaDeCobranca.Remessa
    ];
  }

  obraEnderecoFormatter = (item: ObraFrente) => {
    
    var enderecoString = item.enderecoLogradouro;

    if(item.enderecoNumero == 0) {
      enderecoString = enderecoString + ', S/N';
    } else {
      enderecoString = enderecoString + ', ' + item.enderecoNumero;
    }

    if(item.enderecoComplemento !== '') {
      enderecoString = enderecoString + ', ' + item.enderecoComplemento;
    }
    
    enderecoString = enderecoString + ', ' + item.enderecoBairro;

    enderecoString = enderecoString + ' - CEP: ' + item.enderecoCep;

    return enderecoString;

  }

  frequenciaCobrancaFormatter = (item: EFrequenciaDeCobranca) => {
    switch (item) {
      case EFrequenciaDeCobranca.Bombeamento:
        return 'Bombeamento'.toUpperCase();
      case EFrequenciaDeCobranca.Contrato:
        return 'Contrato'.toUpperCase();
      case EFrequenciaDeCobranca.M3:
        return 'M3'.toUpperCase();
      case EFrequenciaDeCobranca.M3Bombeado:
        return 'M3 Bombeado'.toUpperCase();
      case EFrequenciaDeCobranca.Programacao:
        return 'Programacao'.toUpperCase();
      case EFrequenciaDeCobranca.Remessa:
        return 'Remessa'.toUpperCase();
      default:
        return '';
    }
  }

  get formaDeCobrancaLista(): EFormaDeCobrancaDemaisServicos[] {
    return [
      EFormaDeCobrancaDemaisServicos.FinalConcretagem,
      EFormaDeCobrancaDemaisServicos.NaRemessa
    ];
  }

  formaDeCobrancaFormatter = (item: EFormaDeCobrancaDemaisServicos) => {
    switch (item) {
      case EFormaDeCobrancaDemaisServicos.FinalConcretagem:
        return 'Final Concretagem'.toUpperCase();
      case EFormaDeCobrancaDemaisServicos.NaRemessa:
        return 'Na Remessa'.toUpperCase();
      default:
        return '';
    }
  }

  modeloDocumentoRemessaFormatter = (model: number) : string => {
    if(this.modelosDocumentos.length === 0) return '';
    return this.modelosDocumentos.includes(model) ? this.modelosDocumentosRemessa.filter(e => e.codigo === model)[0].descricao : '';
  };

  get modelosDocumentos(): number[] {
    let codigos: number[] = [];
    this.modelosDocumentosRemessa.forEach(modelo => {
      codigos.push(modelo.codigo);
    })
    return codigos;
  };

  descricaoItensdanfeERomaneioFormatter = (model: number) : string => {
    if (this.modelosDescricaoItensDanfeERomaneio.includes(model)) {
      return this.modelosItensDanfeERomaneio.filter(e => e.codigo === model)[0].descricao;
    }
    return '';
  };

  get modelosDescricaoItensDanfeERomaneio() : number[] {
    return this.modelosItensDanfeERomaneio.map(modelo => modelo.codigo);
  }

  usinasCodigos(): number[] {
    var codigos = this.usinas.map(t => t.codigo);
    codigos.push(0);
    return codigos;
  }

  getSavedProgress(): Proposta {
    let pStr: string = localStorage.getItem('t.tcw.novaProposta');
    if (!pStr) return null;
    let p: Proposta = JSON.parse(pStr);
    return p;
  }
  saveProgress() {
    if (!this.isInsertMode) return;
    localStorage.setItem('t.tcw.novaProposta', JSON.stringify(this.proposta));
  }
  clearSavedProgress() {
    localStorage.removeItem('t.tcw.novaProposta');
  }

  numeroPropostaString(): string {
    return (this.proposta.usina ? this.proposta.usina.codigo : '0')+'/'+this.proposta.numero+'-'+this.proposta.ano;
  }

  ederecoObraString(): string {
    return Tasks.ederecoToString(this.proposta.obra.endereco);
  }

  private _utilizaDados(key: string, property: string, value: boolean) {
    if (this.proposta[property] !== value) {
      if (!this.proposta[key]) {
        this.proposta[key] = new PropostaDadosPessoais();
        this.proposta[key].usinaCodigo = (this.proposta.usina ? this.proposta.usina.codigo : 999);
        this.proposta[key].propostaAno = this.proposta.ano;
        this.proposta[key].propostaNumero = this.proposta.numero;
      }
      this.proposta[key].intervenienteTipo = !value ? this.proposta.intervenienteTipo : 'F';
      this.proposta[key].cpfCnpj = !value ? this.proposta.cpfCnpj : '';
      this.proposta[key].inscricaoEstadual = !value ? this.proposta.inscricaoEstadual : '';
      this.proposta[key].inscricaoMunicipal = !value ? this.proposta.inscricaoMunicipal : '';
      this.proposta[key].intervenienteTipo = !value ? this.proposta.intervenienteTipo : '';
      this.proposta[key].nome = !value ? this.proposta.intervenienteNome : '';
      this.proposta[key].orgaoExpedidor = !value ? this.proposta.orgaoExpedidor : '';
      this.proposta[key].razao = !value ? this.proposta.intervenienteRazao : '';
      this.proposta[key].rg = !value ? this.proposta.rg : '';
      this.proposta[key].email = !value ? this.proposta.email : '';
      this.proposta[property] = value;
    }
  }
  private _utilizaEndereco(key: string, property: string, value: boolean) {
    if (this.proposta[property] !== value) {
      if (!this.proposta[key]) {
        this.proposta[key] = new PropostaDadosPessoais();
        this.proposta[key].usinaCodigo = this.proposta.usina.codigo;
        this.proposta[key].propostaAno = this.proposta.ano;
        this.proposta[key].propostaNumero = this.proposta.numero;
      }
      if (!value) this.proposta[key].endereco = JSON.parse(JSON.stringify(this.proposta.endereco));
      else this.proposta[key].endereco = new Endereco();
      this.proposta[property] = value;
    }
  }

  get utilizaResponsavelSolidario(): boolean {
    return this.proposta.utilizaResponsavelSolidario;
  }
  set utilizaResponsavelSolidario(value: boolean) {
    var previousValue = this.proposta.utilizaResponsavelSolidario;
    this._utilizaDados('responsavelSolidario', 'utilizaResponsavelSolidario', value);
    this.proposta.utilizaResponsavelSolidario = previousValue;
    this._utilizaEndereco('responsavelSolidario', 'utilizaResponsavelSolidario', value);
  }

  private _utilizaEnderecoClienteComoEnderecoObra: boolean = true;
  get utilizaEnderecoClienteComoEnderecoObra(): boolean {
    if (this.propostaImportada) return false;
    return Tasks.ederecoToString(this.proposta.obra.endereco)===Tasks.ederecoToString(this.proposta.endereco)
              && this._utilizaEnderecoClienteComoEnderecoObra;
  }
  set utilizaEnderecoClienteComoEnderecoObra(value: boolean) {
    var cepAnterior = this.proposta.obra.endereco.cep;
    this.enderecoLogradouroAnterior = this.proposta.obra.endereco.logradouro;
    this.enderecoNumeroAnterior = this.proposta.obra.endereco.numero;
    if (value) this.proposta.obra.endereco=JSON.parse(JSON.stringify(this.proposta.endereco));
    if (this._utilizaEnderecoClienteComoEnderecoObra !== value &&
      cepAnterior !== this.proposta.obra.endereco.cep) this.carregaObraDistanciaUsina();
    if (this._utilizaEnderecoClienteComoEnderecoObra !== value &&
      (cepAnterior !== this.proposta.obra.endereco.cep || this.enderecoLogradouroAnterior !== this.proposta.obra.endereco.logradouro || this.enderecoNumeroAnterior !== this.proposta.obra.endereco.numero))
      this.carregaDistanciaUsinaViaGoogleApi();
      
    this._utilizaEnderecoClienteComoEnderecoObra = value;
  }

  get propostaImportada(): boolean {
    return this.proposta.origemObraCodigo > 0 && this.proposta.origemUsinaCodigo > 0;
  }

  utilizaCobrancaPorM3(bomba: ObraBomba): boolean {
    return (bomba.tipoCalculo !== EBombaM3CalculoTipo.semCobranca);
  }
  utilizaCobrancaPorHora(bomba: ObraBomba): boolean {
    return (bomba.horaTipoCalculo !== EBombaHoraCalculoTipo.semCobranca);
  }

  isStatusPropostaAprovado(): boolean {
    return this.proposta.statusProposta===EStatusProposta.Aprovado;
  }

  isStatusPropostaEmNegociacao(): boolean {
    return this.proposta.statusProposta===EStatusProposta.EmNegociacao;
  }

  isContratoAprovado(): boolean {
    return this.proposta.statusContrato === EStatusContrato.Aprovado;
  }

  isViaCaptacaoVendedor(): boolean {
    
    if(!this.proposta.obra.viaCaptacao)
      return false;

    if(this.proposta.obra.viaCaptacao.viaCaptacao) {
      return this.proposta.obra.viaCaptacao.viaCaptacao.tipoIndicacao == ECadastroGeralViaCaptacaoTipoIndicacao.Vendedor;
    } else {
      return false;
    }
  }

  isViaCaptacaoCliente(): boolean {
    
    if(!this.proposta.obra.viaCaptacao)
      return false;

    if(this.proposta.obra.viaCaptacao.viaCaptacao) {
      return this.proposta.obra.viaCaptacao.viaCaptacao.tipoIndicacao == ECadastroGeralViaCaptacaoTipoIndicacao.Cliente;
    } else {
      return false;
    }
  }

  allowDocumentoDiferenteRemessa(): boolean {
    var saida = false;
    this.filiais.forEach((filial) => {
      if(filial.permiteDocumentoDiferentePadraoRemessa === 'S') { saida = true; }
    });
    return saida;
  }

  allowDocumentoDiferenteBomba(): boolean {
    var saida = false;
    this.filiais.forEach(filial => {
      if(filial.permiteDocumentoDiferentePadraoBomba === 'S') { saida =  true; }
    });
    return saida;
  }

  get possuiPagamentoAntecipadoAprovado(): boolean {
    return this.proposta.obra.obraPagamentos.filter(t => t.necessitaAprovacao && t.idAprovacao !== "").length > 0;
  }

  isContratoEncerrado(): boolean {
    return this.proposta.isContratoEncerrado && !!this.proposta.obra.contrato.dataEncerramento;
  }
  
  alteracaoNaoPermitida(consideraAprovacaoContrato?: boolean): boolean {
    if(this.proposta.obra.numContrato > 0) { 
      return true;
    }
    
    if (!consideraAprovacaoContrato)
      return this.isContratoEncerrado() || !this.temDireitoAlteracao;
    
    return (this.isContratoEncerrado() || !this.temDireitoAlteracao)
      || (this.isContratoAprovado() && !this.temDireitoCadastro);
  }

  get alteracaoDadosClienteNaoPermitida(): boolean {
    if(this.proposta.intervenienteCodigo != 0){
      if (this.temDireitoAlteracaoCliente) {
        return true;
      } 
    } else {
      if (this.temDireitoInclusaoCliente) {
        return true;
      } else {
        return false;
      }
    }
  }

  get inclusaoDadosClienteNaoPermitida(): boolean {
    return this.temDireitoInclusaoCliente;
  }

  validaCampo(campo: string): boolean {
    if (this.proposta.intervenienteCodigo === 0) return true;

    switch (campo){
      case ('telefone'):
        if (this.campoObrigatorioTelefone !== 0) return true;
        break;

      case ('inscricaoEstadual'):
        if (this.isStatusPropostaAprovado) {
          if (this.campoObrigatorioInscricaoEstadual !== "")
            return true;
          else
            return false;
        }
        break;

      case ('razao'):
        if (this.campoObrigatorioRazao !== "") return true;
        break;

      case ('nome'):
        if (this.campoObrigatorioNomeReduzido !== "") return true;
        break;

      case ('endereco'):
        if (this.isStatusPropostaAprovado) {
          if (this.campoObrigatorioEndereco) {
            if (this.campoObrigatorioEndereco.logradouro !== "" && this.campoObrigatorioEndereco.cep !== ""
             && this.campoObrigatorioEndereco.bairro !== "" && this.campoObrigatorioEndereco.municipio
             && (this.campoObrigatorioEndereco.complemento !== "" || this.campoObrigatorioEndereco.numero !== 0)
            )
              return true;
          } else
            return false;
        }
        break;

      case ('profissao'):
        if (this.isStatusPropostaAprovado && this.parametroProposta.obrigaProfissao) {
          if (this.campoObrigatorioProfissao !== "") 
            return true;
          else
            return false;
        }
        break;

      case ('contato'):
        if (this.campoObrigatorioNomeContato !== "") return true;
        break;
      
      case ('email'):
        if (this.isStatusPropostaAprovado() && (this.proposta.intervenienteTipo!=='F' || this.parametroProposta.obrigaEmailPessoaFisica)){
          if (this.campoObrigatorioEmail !== "")
            return true;
          else
            return false;
        }
        break;
    }

    return false;
  }

  utilizaIdentificadorClienteAoInvesDeCpfCnpj(): boolean {
    return this.utilizarIdentificadorClienteAoInvesDeCpfCnpj;
  }

  validaCliqueVendedor(){
    this.clicouVendedor = true;
  }

  setCpfCnpj(cpfCnpj: string) {
    if (cpfCnpj !== this.proposta.cpfCnpj) {
      this.proposta.cpfCnpj = cpfCnpj;

      if ( (this.proposta.intervenienteTipo === 'F' && cpfCnpj.length === 11)
          || (this.proposta.intervenienteTipo !== 'F' && cpfCnpj.length === 14) ) {
          
        this._intervenienteService.obterPermitidoPorCpfCnpj(cpfCnpj,this.proposta.inscricaoEstadual)
        .then(interveniente => {
          
          if (interveniente) {
            this.carregaDadosInterveniente(interveniente);

            var vendedor = interveniente.vendedor ? interveniente.vendedor.codigo : 0;
            if (vendedor !== 0 && vendedor !== this.proposta.vendedor.codigo) {
              var self = PropostaPageComponent.self;
              self.dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `O cliente é do vendedor: ${self.vendedorFormatter(interveniente.vendedor)}`
                }
              });
            }

          } else {
            this.proposta.intervenienteCodigo = 0;
            if(this.proposta.interveniente)this.limpaDadosInterveniente()
            else this.proposta.interveniente = new Interveniente();
          }
        },error => {          
          var self = PropostaPageComponent.self;
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Não foi possível carregar os dados do Cliente!`
            }
          });
          if (this.proposta.intervenienteTipo === 'F') this.clienteForm.controls['cpf'].setValue('');
          else if (this.proposta.intervenienteTipo !== 'F') this.clienteForm.controls['cnpj'].setValue('');          
        })
        .then(() => {
          var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
          if (control) control.updateValueAndValidity();
        });
      }
    }

    var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
    if (control) control.updateValueAndValidity();
    
  }

  setCpfCnpjIndicador(cpfCnpj: string) {
    if (cpfCnpj !== this.intervenienteIndicadorCpfCnpj) {
      this.intervenienteIndicadorCpfCnpj = cpfCnpj;

      if ( (this.intervenienteIndicadorTipo === 'F' && cpfCnpj.length === 11)
          || (this.intervenienteIndicadorTipo !== 'F' && cpfCnpj.length === 14) ) {
          
        this._intervenienteService.obterPermitidoPorCpfCnpj(cpfCnpj, "")
        .then(interveniente => {
          
          if (interveniente) {

            this.intervenienteIndicadorCpfCnpj = interveniente.cpfCnpj;
            this.proposta.obra.indicador.interveniente = interveniente;
            this.proposta.obra.indicador.intervenienteCodigo = interveniente.codigo;

          } else {

            this.proposta.obra.indicador.intervenienteCodigo = 0;
            if (this.proposta.obra.indicador.interveniente) this.proposta.obra.indicador.interveniente.codigo = 0;
            else this.proposta.obra.indicador.interveniente = new Interveniente();

          }
        },error => {          
          var self = PropostaPageComponent.self;
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Não foi possível carregar os dados do Cliente indicador!`
            }
          });
          if (this.intervenienteIndicadorTipo === 'F') this.geralForm.controls['cpf'].setValue('');
          else if (this.intervenienteIndicadorTipo !== 'F') this.geralForm.controls['cnpj'].setValue('');          
        })
        .then(() => {
          var control = this.geralForm.controls['cpf'] || this.geralForm.controls['cnpj'];
          if (control) control.updateValueAndValidity();
        });
      }
    }

    var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
    if (control) control.updateValueAndValidity();
    
  }

  private _timeoutIdentificador = null;
  setIdentificador(cpfCnpj: string) {
    this.proposta.cpfCnpj = cpfCnpj;
    this.proposta.intervenienteTipo = 'F'

    if (this._timeoutIdentificador) clearTimeout(this._timeoutIdentificador);

    if (cpfCnpj.toString() === this.proposta.interveniente.cpfCnpj) return

    this._timeoutIdentificador = setTimeout( () => {
      this._intervenienteService.obterPermitidoPorCpfCnpj(cpfCnpj,this.proposta.inscricaoEstadual)
      .then(interveniente => {
        
        if (interveniente) {
          this.carregaDadosInterveniente(interveniente);
          
          var vendedor = interveniente.vendedor ? interveniente.vendedor.codigo : 0;
          if (vendedor !== 0 && vendedor !== this.proposta.vendedor.codigo) {
            var self = PropostaPageComponent.self;
            self.dialog.open(AlertDialogComponent, {
              disableClose: true,
              data: {
                title: 'TopConWeb',
                message: `O cliente é do vendedor: ${self.vendedorFormatter(interveniente.vendedor)}`
              }
            });
          }

        } else {
          this.proposta.intervenienteCodigo = 0;
          if (this.proposta.interveniente) this.proposta.interveniente.codigo = 0;
          else this.proposta.interveniente = new Interveniente();
        }
      },error => { 
        var self = PropostaPageComponent.self;
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Não foi possível carregar os dados do Cliente!`
          }
        });        
        if (this.proposta.intervenienteTipo === 'F') this.clienteForm.controls['cpf'].setValue('');
        else if (this.proposta.intervenienteTipo !== 'F') this.clienteForm.controls['cnpj'].setValue(''); 
      })
      .then(() => {
        var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
        if (control) control.updateValueAndValidity();
      });
    }, 2200);
    
    var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
    if (control) control.updateValueAndValidity();
    
  }

  SetClienteCodigo(codigo: number) {
    
    if (codigo !== this.proposta.intervenienteCodigo) {
      this.proposta.intervenienteCodigo = codigo;

      this._intervenienteService.obterPorCodigoInterveniente(codigo)
      .then(interveniente => {
        if (interveniente) {
          this.carregaDadosInterveniente(interveniente);

          var vendedor = interveniente.vendedor ? interveniente.vendedor.codigo : 0;
          if (vendedor !== 0 && vendedor !== this.proposta.vendedor.codigo) {
            var self = PropostaPageComponent.self;
            self.dialog.open(AlertDialogComponent, {
              disableClose: true,
              data: {
                title: 'TopConWeb',
                message: `O cliente é do vendedor: ${self.vendedorFormatter(interveniente.vendedor)}`
              }
            });
          }

        } else {
          this.proposta.intervenienteCodigo = 0;
          if (this.proposta.interveniente) this.proposta.interveniente.codigo = 0;
          else this.proposta.interveniente = new Interveniente();
        }
      },error => { 
        var self = PropostaPageComponent.self;
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Não foi possível carregar os dados do Cliente!`
          }
        });
        this.clienteForm.controls['interveniente'].setValue('');
        this.proposta.intervenienteCodigo = 0;
        this.proposta.inscricaoEstadual = "";
      })
      .then(() => {
        var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
        if (control) control.updateValueAndValidity();
      });
      
    }

    var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
    if (control) control.updateValueAndValidity();
    
  }

  SetFinalidadeContrato() {

    this.proposta.contratoFinalidade = this.finalidadeSelecionada.codigo;

  }

  SetVendedorIndicadorCodigo() {
    
    this.proposta.obra.indicador.vendedorCodigo = this.proposta.obra.indicador.vendedor.codigo;

  }

  SetClienteIndicadorCodigo(codigo: number) {
    
    if (codigo !== this.proposta.obra.indicador.intervenienteCodigo) {
      this.proposta.obra.indicador.intervenienteCodigo = codigo;

      this._intervenienteService.obterPorCodigoInterveniente(codigo)
      .then(interveniente => {
        if (interveniente) {
          
          this.intervenienteIndicadorTipo = interveniente.intervenienteTipo;
          this.intervenienteIndicadorCpfCnpj = interveniente.cpfCnpj;
          this.proposta.obra.indicador.interveniente = interveniente;
          this.proposta.obra.indicador.intervenienteCodigo = interveniente.codigo;

        } else {

          this.proposta.obra.indicador.intervenienteCodigo = 0;
          if (this.proposta.obra.indicador.interveniente) this.proposta.obra.indicador.interveniente.codigo = 0;
          else this.proposta.obra.indicador.interveniente = new Interveniente();

        }
      },error => { 
        var self = PropostaPageComponent.self;
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: `Não foi possível carregar os dados do Cliente!`
          }
        });
        this.clienteForm.controls['interveniente'].setValue('');
        this.proposta.intervenienteCodigo = 0;
        this.proposta.inscricaoEstadual = "";
      })
      .then(() => {
        var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
        if (control) control.updateValueAndValidity();
      });
      
    }

    var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
    if (control) control.updateValueAndValidity();
    
  }

  private _timeoutIntervenientesPorRazao = null;
  filtrarIntervenientesPorRazao(cliente: string) {
    
    var tamanhoMinimo = (isNaN(parseInt(cliente)) ? 3 : 0);

    if (cliente && cliente.length>tamanhoMinimo && (!this.proposta.interveniente || this.proposta.interveniente.razao!=cliente)) {
      
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
              var self = PropostaPageComponent.self;
              self.dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                  title: 'TopConWeb',
                  message: `Não foi possível carregar os dados do Cliente!`
                }
              });
              this.clienteForm.controls['interveniente'].setValue('');
            }
          )
      }, 500);

    } else {
      this.intervenientes = [];
    }
  }

  limpaDadosInterveniente() {
      // Instancia um novo objeto para limpar a referência
      this.proposta.interveniente = new Interveniente();

      // Reseta os campos da proposta com valores "zerados"
      this.proposta.intervenienteCodigo = 0;
      this.proposta.intervenienteNome = '';
      this.proposta.intervenienteRazao = '';
      this.proposta.rg = '';
      this.proposta.orgaoExpedidor = '';
      this.proposta.inscricaoEstadual = '';
      this.proposta.inscricaoMunicipal = '';
      this.proposta.endereco = new Endereco(); 
      this.proposta.profissao = '';
      this.proposta.empresaTrabalho = '';
      this.proposta.telefoneDdd = 0;
      this.proposta.telefoneNumero = 0;
      this.proposta.celularDdd = 0;
      this.proposta.celularNumero = 0;
      this.proposta.telefoneComercialDdd = 0;
      this.proposta.telefoneComercialNumero = 0;
      this.proposta.ramal = 0;
      this.proposta.nomeMae = '';
      this.proposta.nomeConjuge = '';
      this.proposta.contato = '';
      this.proposta.email = '';
      this.proposta.emailCobranca = '';
      this.proposta.observacao = '';
      this.proposta.intervenienteIdExterno = '';

    
      this.proposta.interveniente.idExterno = '';

      // Reseta as variáveis de obrigatoriedade/validação
      this.campoObrigatorioTelefone = 0;
      this.campoObrigatorioInscricaoEstadual = '';
      this.campoObrigatorioRazao = '';
      this.campoObrigatorioNomeReduzido = '';
      this.campoObrigatorioEndereco = new Endereco();
      this.campoObrigatorioProfissao = '';
      this.campoObrigatorioNomeContato = '';
      this.campoObrigatorioEmail = '';
  }

  carregaDadosInterveniente(interveniente: Interveniente) {
    if (interveniente.intervenienteTipo === '') {
      interveniente.intervenienteTipo = interveniente.cpfCnpj.length === 14 ? intervenienteTipos[1].codigo :intervenienteTipos[0].codigo
    }
    this.proposta.interveniente = interveniente;
    this.proposta.cpfCnpj = interveniente.cpfCnpj;
    this.proposta.intervenienteCodigo = interveniente.codigo;
    this.proposta.intervenienteNome = interveniente.nome;
    this.proposta.intervenienteRazao = interveniente.razao;
    this.proposta.intervenienteTipo = interveniente.intervenienteTipo;
    this.proposta.rg = interveniente.rg;
    this.proposta.orgaoExpedidor = interveniente.orgaoExpedidor;
    this.proposta.inscricaoEstadual = interveniente.inscricaoEstadual;
    this.proposta.inscricaoMunicipal = interveniente.inscricaoMunicipal;
    this.proposta.endereco = interveniente.endereco;
    this.proposta.profissao = interveniente.profissao;
    this.proposta.empresaTrabalho = interveniente.empresaTrabalho;
    this.proposta.telefoneDdd = interveniente.telefoneDdd;
    this.proposta.telefoneNumero = interveniente.telefoneNumero;
    this.proposta.celularDdd = interveniente.celularDdd;
    this.proposta.celularNumero = interveniente.celularNumero;
    this.proposta.telefoneComercialDdd = interveniente.telefoneComercialDdd;
    this.proposta.telefoneComercialNumero = interveniente.telefoneComercialNumero;
    this.proposta.ramal = interveniente.ramal;
    this.proposta.nomeMae = interveniente.nomeMae;
    this.proposta.nomeConjuge = interveniente.nomeConjuge;
    this.proposta.contato = interveniente.contato;
    this.proposta.email = interveniente.email;
    this.proposta.emailCobranca = interveniente.emailCobranca;
    this.proposta.observacao = interveniente.observacao;
    this.proposta.intervenienteIdExterno = interveniente.idExterno;
	this.proposta.interveniente.idExterno = interveniente.idExterno;

    this.campoObrigatorioTelefone = interveniente.telefoneNumero;
    this.campoObrigatorioInscricaoEstadual = interveniente.inscricaoEstadual;
    this.campoObrigatorioRazao = interveniente.razao;
    this.campoObrigatorioNomeReduzido = interveniente.nome;
    this.campoObrigatorioEndereco = interveniente.endereco;
    this.campoObrigatorioProfissao = interveniente.profissao;
    this.campoObrigatorioNomeContato = interveniente.contato;
    this.campoObrigatorioEmail = interveniente.email;
  }

  closeModal() {
    let self = PropostaPageComponent.self;

    self.concretoItemForm.markAsPristine();
    self.concretoItemForm.markAsUntouched();

    self.bombaItemForm.markAsPristine();
    self.bombaItemForm.markAsUntouched();

    self.taxaItemForm.markAsPristine();
    self.taxaItemForm.markAsUntouched();

    self.pagamentoItemForm.markAsPristine();
    self.pagamentoItemForm.markAsUntouched();

    self.pagamentoItemDetalheForm.markAsPristine();
    self.pagamentoItemDetalheForm.markAsUntouched();

    self.solicitaAprovacoesForm.markAsPristine();
    self.solicitaAprovacoesForm.markAsUntouched();

    self._dialogRef.close();
    self.modalIsOpen = false;
    self.isTracoInsertMode = false;
  }

  tracoString(traco: ObraTraco): string {
    if (!traco.resistenciaTipo || !traco.pedra || !traco.slumpNominal || !traco.uso) return '';
    if (traco.descricaoPersonalizada !== '') return traco.descricaoPersonalizada;
    let vinculo = traco.resistenciaTipo.vinculo;
    let mpaConsumo = vinculo === ETipoVinculoMpaConsumo.MPA ? traco.mpa : (vinculo === ETipoVinculoMpaConsumo.CONSUMO ? traco.consumo : '');
    return traco.resistenciaTipo.abreviatura+' '+mpaConsumo+' / '+traco.pedra.descricao+' / '+traco.slumpNominal.descricao+' / '+traco.uso.descricao;
  }
  tracoReajusteString(traco: ContratoTracoReajuste): string {
    if (!traco.resistenciaTipo || !traco.pedra || !traco.slump || !traco.uso) return '';
    let vinculo = traco.resistenciaTipo.vinculo;
    let mpaConsumo = vinculo === ETipoVinculoMpaConsumo.MPA ? traco.mpa : (vinculo === ETipoVinculoMpaConsumo.CONSUMO ? traco.consumo : '');
    return traco.resistenciaTipo.abreviatura+' '+mpaConsumo+' / '+traco.pedra.descricao+' / '+traco.slump.descricao+' / '+traco.uso.descricao;
  }
  bombaString(bomba: ObraBomba): string {
    if (!bomba.bombaTipo) return this.intervenienteFormatter(bomba.terceiro) || 'BOMBA DE TERCEIRO';
    return bomba.bombaTipo.descricao;
  }
  pagamentoString(pagamento: Pagamento): string {
    if (!pagamento.condicaoPagamento) return '';
    return pagamento.condicaoPagamento.descricao;
  }

  idExternoChange(idExterno: string) {
    this.proposta.interveniente.idExterno = idExterno;
  }

  tracoAtualTemProgramacao: boolean = false;
  async tracoTemProgramacao(traco: ObraTraco) {
    let self = PropostaPageComponent.self;
    
    return await this._programacaoService.ListarPorPropostaTraco(traco)
    .then(
      programacoes => {
        self.tracoAtualTemProgramacao = programacoes.length > 0;
        return programacoes.length > 0;
      },
      error => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'Erro ao verificar se existem programações!'
          }
        });
        self.tracoAtualTemProgramacao = false;
        return false;
      }
    ).then(res => {
      self._cdr.detectChanges();
      return res;
    });
  }

  tracoAtualTemAprovacaoTabelaVenda: boolean = false;
  async tracoTemAprovacaoTabelaVenda(traco: ObraTraco) {
    let self = PropostaPageComponent.self;

    return await this._tracoPrecoService.VerificaTracoPendenteAprovacaoTabelaDeVenda
    (this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, this.traco.slump, this.traco.resistenciaTipo, this.traco.mpa, this.traco.consumo)
    .then(
      resultado => {
        self.tracoAtualTemAprovacaoTabelaVenda = resultado;
      },
      error => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'Erro ao verificar se existem traço esta pendente de aprovação de tabela de venda!'
          }
        });
        self.tracoAtualTemAprovacaoTabelaVenda = false;
      }
    ).then(res => {
      self._cdr.detectChanges();
    });
  }

  bombaAtualTemProgramacao: boolean = false;
  async bombaTemProgramacao(bomba: ObraBomba) {
    let self = PropostaPageComponent.self;
    
    return await this._programacaoService.ListarPorPropostaBomba(bomba)
    .then(
      programacoes => {
        self.bombaAtualTemProgramacao = programacoes.length > 0;
        return programacoes.length > 0;
      },
      error => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'Erro ao verificar se existem programações!'
          }
        });
        self.bombaAtualTemProgramacao = false;
        return false;
      }
    ).then(res => {
      self._cdr.detectChanges();
      return res;
    });
  }

  demaisServicoAtualTemProgramacao: boolean = false;
  async demaisServicoTemProgramacao(demaisServico: ObraDemaisServicos) {
    let self = PropostaPageComponent.self;
    
    return await this._programacaoService.ListarPorItemDemaisServicos(demaisServico)
    .then(
      programacoes => {
        self.demaisServicoAtualTemProgramacao = programacoes.length > 0;
        return programacoes.length > 0;
      },
      error => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'Erro ao verificar se existem programações!'
          }
        });
        self.demaisServicoAtualTemProgramacao = false;
        return false;
      }
    ).then(res => {
      self._cdr.detectChanges();
      return res;
    });
  }

  get exibeBotaoImportacaoProposta(): boolean {
    return this.isInsertMode && (this.proposta.cpfCnpj.length === 11 || this.proposta.cpfCnpj.length === 14);
  }
  openImportacaoPropostaModal() {
    var self = PropostaPageComponent.self;

    self.dialog.open(PropostaImportacaoDialogComponent, {
      data: {
        cpfCnpj: self.proposta.cpfCnpj,
        importCallback: (proposta: Proposta) => {
          self.proposta.utilizaDadosFaturamento = proposta.utilizaDadosFaturamento;
          self.proposta.utilizaEnderecoFaturamento = proposta.utilizaEnderecoFaturamento;
          self.proposta.utilizaDadosCobranca = proposta.utilizaDadosCobranca;
          self.proposta.utilizaEnderecoCobranca = proposta.utilizaEnderecoCobranca;
          self.proposta.utilizaResponsavelSolidario= proposta.utilizaResponsavelSolidario;
          self.proposta.faturamento = proposta.faturamento;
          self.proposta.cobranca = proposta.cobranca;
          self.proposta.responsavelSolidario = proposta.responsavelSolidario;
          self.proposta.codigoObraPrefeitura = proposta.codigoObraPrefeitura;
          self.proposta.origemObraCodigo = proposta.obra.numero;
          self.proposta.origemUsinaCodigo = proposta.obra.usinaCodigo;
          self.proposta.obra.viaCaptacao = proposta.obra.viaCaptacao;
          self.proposta.obra.tipoObra = proposta.obra.tipoObra;
          self.proposta.obra.porteObra = proposta.obra.porteObra;
          self.proposta.obra.nome = proposta.obra.nome;
          self.proposta.obra.endereco = proposta.obra.endereco;
          self.proposta.obra.contatoPrincipalNome = proposta.obra.contatoPrincipalNome;
          self.proposta.obra.contatoPrincipalFuncao = proposta.obra.contatoPrincipalFuncao;
          self.proposta.obra.contatoPrincipalCelularDdd = proposta.obra.contatoPrincipalCelularDdd;
          self.proposta.obra.contatoPrincipalCelularNumero = proposta.obra.contatoPrincipalCelularNumero;
          self.proposta.obra.contatoPrincipalTelefoneDdd = proposta.obra.contatoPrincipalTelefoneDdd;
          self.proposta.obra.contatoPrincipalTelefoneNumero = proposta.obra.contatoPrincipalTelefoneNumero;
          self.proposta.obra.radioNextel = proposta.obra.radioNextel;
          self.proposta.obra.usinaEntrega = proposta.obra.usinaEntrega;
          self.proposta.obra.distanciaUsina = proposta.obra.distanciaUsina;
          self.proposta.obra.referenciaAcesso = proposta.obra.referenciaAcesso;
          self.proposta.obra.observacaoNf = proposta.obra.observacaoNf;
          self.proposta.obra.condicaoPagamento = proposta.obra.condicaoPagamento;
          self.proposta.obra.tipoCobranca = proposta.obra.tipoCobranca;
          self.proposta.obra.obraTributacoesMunicipais = proposta.obra.obraTributacoesMunicipais;
          self.proposta.obra.codigoBeneficioFiscal = proposta.obra.codigoBeneficioFiscal;
          self.proposta.obra.volumeEstimado = proposta.obra.volumeEstimado;
          self.proposta.obra.volumePorCarga = proposta.obra.volumePorCarga;
        }
      }
    });
  }

  confirmTraco: Function;
  cancelTraco: Function;
  openTracoModal(content, confirmTracoCallback: Function, cancelTracoCallback?: Function, traco?: ObraTraco) {
    this.tracoDescricao = '';
    this.tracoParticularidades = new TracoParticularidades();
    this.tracoModalNumeracaoProdutoSelecionado = new TracoPrecoNumeracaoProduto();

    this.desabilitaCamposTraco = false;
    this.desabilitaNumeracaoProduto = false;

    let obraTracos = this.proposta.obra.obraTracos;

    this.isTracoInsertMode = !traco;

    if (traco) {
      this.indexTraco = obraTracos.indexOf(traco);
      this.traco = JSON.parse(JSON.stringify(obraTracos[this.indexTraco]));
      this.tracoModalNumeracaoProdutoSelecionado.numeracao = 0;
      this.proposta.obra.enderecoMunicipioCodigo = this.proposta.obra.endereco.municipio.codigo;
      
      if(traco.numeracaoProduto === 0) {
        this._tracoPrecoService.ObterPorDataUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
        (this.proposta.obra.usinaEntrega, traco.uso, traco.pedra, traco.slump, traco.resistenciaTipo, traco.mpa, traco.consumo, this.proposta.obra, true)
        .then(tracoPreco => {
          if (tracoPreco) {
            if (tracoPreco.numeracaoProduto > 0) {
              this.tracoModalNumeracaoProdutoSelecionado.numeracao = tracoPreco.numeracaoProduto;
              this.tracoModalNumeracaoProdutoSelecionado.usoDescricao = tracoPreco.uso.descricao;
            }
            this.traco.numeracaoProduto = tracoPreco.numeracaoProduto;
          }
        });
      } else {
        this.tracoModalNumeracaoProdutoSelecionado.numeracao = this.traco.numeracaoProduto;
        this.tracoModalNumeracaoProdutoSelecionado.usoDescricao = this.traco.uso.descricao;
      }

      if(this.tracoModalNumeracaoProdutoSelecionado.status === 7105)
        traco.status === EObraTracoStatus.CustoVirtual;
      
    } else {
      this.tracoModalNumeracaoProdutoSelecionado = new TracoPrecoNumeracaoProduto();
      this.traco = new ObraTraco();
      this.traco.usina = this.proposta.usina;
      this.traco.obraCodigo = this.proposta.obra.numero;
      this.traco.propostaAno = this.proposta.ano;
      this.traco.propostaNumero = this.proposta.numero;
      this.traco.sequencia = this.getFreeSequence(obraTracos.map(t => t.sequencia));
      this.traco.numeracaoProduto = 0;
    }

    this.tracoTemProgramacao(this.traco);
    this.tracoTemAprovacaoTabelaVenda(this.traco);

    this.carregaTracoNumeracoesProduto(false);

    this.carregaTracoUsos(false);

    this.carregaTracoParticularidades();

    this.confirmTraco = confirmTracoCallback;
    this.cancelTraco = cancelTracoCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.tracoModalVCR });
    this._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    this.modalIsOpen = true;
  }

  confirmBomba: Function;
  cancelBomba: Function;
  openBombaModal(content, confirmBombaCallback: Function, cancelBombaCallback?: Function, bomba?: ObraBomba) {
    let obraBombas = this.proposta.obra.obraBombas;

    if (bomba) {
      this.indexBomba = obraBombas.indexOf(bomba);
      this.bomba = JSON.parse(JSON.stringify(obraBombas[this.indexBomba]));
      this.carregaBombaPreco(true);
    } else {
      this.indexBomba = -1;
      this.bomba = new ObraBomba();
      this.bomba.usinaCodigo = this.proposta.usina ? this.proposta.usina.codigo : 0;
      this.bomba.obraCodigo = this.proposta.obra.numero;
      this.bomba.propostaAno = this.proposta.ano;
      this.bomba.propostaNumero = this.proposta.numero;
      this.bomba.sequencia = this.getFreeSequence(obraBombas.map(t => t.sequencia));
    }

    this.bombaTemProgramacao(this.bomba);

    this.carregaBombaTipos();

    this.confirmBomba = (bomba: ObraBomba) => {
      if (bomba.bombaPropria) {
        this._bombaPrecoService
          .ObterValorAdicional(this.proposta.obra.usinaEntrega, bomba.bombaTipo, bomba.distanciaTubulacao)
          .then(valor => bomba.valorAdicionalTubulacao = valor)
          .then(() => confirmBombaCallback(bomba));
      } else {
        bomba.valorAdicionalTubulacao = 0;
        confirmBombaCallback(bomba);
      }
    };
    this.cancelBomba = cancelBombaCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.bombaModalVCR });
    this._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    this.modalIsOpen = true;
  }

  confirmDemaisServicos: Function;
  cancelDemaisServicos: Function;
  openDemaisServicosModal(content, confirmCallback: Function, cancelCallback?: Function, itemDemaisServicos?: ObraDemaisServicos) {
    let demaisServicos = this.proposta.obra.obraDemaisServicos;

    if (itemDemaisServicos) {
      this.indexDemaisServicos = demaisServicos.indexOf(itemDemaisServicos);
      this.itemDemaisServicos = JSON.parse(JSON.stringify(demaisServicos[this.indexDemaisServicos]));
    } else {
      this.itemDemaisServicos = new ObraDemaisServicos();
      this.itemDemaisServicos.usinaCodigo = this.proposta.usina ? this.proposta.usina.codigo : 0;
      this.itemDemaisServicos.obraNumero = this.proposta.obra.numero;
      this.itemDemaisServicos.usinaEntrega = this.proposta.obra.usinaEntrega;
      this.itemDemaisServicos.sequencia = this.getFreeSequence(demaisServicos.map(t => t.sequencia));
    }

    this.demaisServicoTemProgramacao(this.itemDemaisServicos);

    this.carregaDemaisServicos();

    this.confirmDemaisServicos = confirmCallback;
    this.cancelDemaisServicos = cancelCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.demaisServicosModalVCR });
    this._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    this.modalIsOpen = true;
  }

  confirmTaxa: Function;
  cancelTaxa: Function;
  openTaxaModal(content, confirmTaxaCallback: Function, cancelTaxaCallback: Function, taxa: ObraTaxa) {
    
    if (this.taxaOpcoesCobrarVolume.length === 0) {
      this._obraTaxaService.listarOpcoes('cobrar_volume').then(
        opcoes => { this.taxaOpcoesCobrarVolume = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesCobrarVolume = [] }
      );
    }

    if (this.taxaOpcoesHorario.length === 0) {
      this._obraTaxaService.listarOpcoes('horario').then(
        opcoes => { this.taxaOpcoesHorario = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesHorario = [] }
      );
    }

    if (this.taxaOpcoesPedra.length === 0) {
      this._pedraService.listarTodos().then(
        pedras => { this.taxaOpcoesPedra = pedras.map(t => t.descricao) },
        error => { this.taxaOpcoesPedra = [] }
      );
    }

    if (this.taxaOpcoesQuandoAte.length === 0) {
      this._obraTaxaService.listarOpcoes('quando_ate').then(
        opcoes => { this.taxaOpcoesQuandoAte = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesQuandoAte = [] }
      );
    }

    if (this.taxaOpcoesQuandoDe.length === 0) {
      this._obraTaxaService.listarOpcoes('quando_de').then(
        opcoes => { this.taxaOpcoesQuandoDe = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesQuandoDe = [] }
      );
    }

    if (this.taxaOpcoesQuandoOperacao.length === 0) {
      this._obraTaxaService.listarOpcoes('quando_oper').then(
        opcoes => { this.taxaOpcoesQuandoOperacao = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesQuandoOperacao = [] }
      );
    }

    if (this.taxaOpcoesResistenciaDe.length === 0) {
      this._obraTaxaService.listarOpcoes('da_resistenc').then(
        opcoes => { this.taxaOpcoesResistenciaDe = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesResistenciaDe = [] }
      );
    }

    if (this.taxaOpcoesResistenciaPara.length === 0) {
      this._obraTaxaService.listarOpcoes('para_resistenc').then(
        opcoes => { this.taxaOpcoesResistenciaPara = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesResistenciaPara = [] }
      );
    }

    if (this.taxaOpcoesSlump.length === 0) {
      this._slumpService.listarTodos().then(
        slumps => { this.taxaOpcoesSlump = slumps.map(t => t.amplitudeDe.toFixed(0)) },
        error => { this.taxaOpcoesSlump = [] }
      );
    }

    if (this.taxaOpcoesTipo.length === 0) {
      this._obraTaxaService.listarOpcoes('taxa_adicional').then(
        opcoes => { this.taxaOpcoesTipo = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesTipo = [] }
      );
    }

    if (this.taxaOpcoesTipoPessoa.length === 0) {
      this._obraTaxaService.listarOpcoes('tipo_pessoa').then(
        opcoes => {
          this.taxaOpcoesTipoPessoaCodigos = opcoes.map(t => t.codigo);
          this.taxaOpcoesTipoPessoa = opcoes;
        },
        error => {
          this.taxaOpcoesTipoPessoa = [];
          this.taxaOpcoesTipoPessoaCodigos = [];
        }
      );
    }

    if (this.taxaOpcoesTipoValor.length === 0) {
      this._obraTaxaService.listarOpcoes('tipo_valor').then(
        opcoes => { this.taxaOpcoesTipoValor = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesTipoValor = [] }
      );
    }

    if(this.taxaOpcoesAntecedencia.length === 0) {
      this._obraTaxaService.listarOpcoes('antecedencia').then(
        opcoes => { this.taxaOpcoesAntecedencia = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesTipoValor = [] }
      );
    }

    this._obraTaxaService.listarOpcoes('valor_por').then(
      opcoes => { this.taxaOpcoesValorPor = opcoes.filter(valorPorTaxaFiltro).map(t => t.descricao) },
      error => { this.taxaOpcoesValorPor = [] }
    );

    const valorPorTaxaFiltro = (opcoesValorPor: CadastroDiverso) => {
      const { codigo } = opcoesValorPor;
      switch (taxa.tipo) {
        case TaxaTipos.TAXA_PERMANENCIA_NA_OBRA: 
        case TaxaTipos.TAXA_PERMANENCIA_DE_BOMBA_NA_OBRA:
          return codigo === 'H' || codigo === 'V';
        case TaxaTipos.ADICIONAL_KM_RODADO:
          return codigo === 'K';
        case TaxaTipos.ADICIONAL_RETORNO_CONCRETO:
        case TaxaTipos.ADICIONAL_ZMRC:
        case TaxaTipos.ADICIONAL_NOTURNO:
        case TaxaTipos.ADICIONAL_DOMINGOS_E_FERIADOS:
          return codigo === 'M' || codigo === 'V';
        case TaxaTipos.ADICIONAL_NOTURNO_BOMBEADO:
        case TaxaTipos.ADICIONAL_DOMINGOS_E_FERIADOS_BOMBEADO:
          return codigo === 'H';
        case TaxaTipos.M3_FALTANTE:
        case TaxaTipos.M3_FALTANTE_BOMBEADO:
        case TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_SLUMP:
        case TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_PEDRAS:
        case TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_RESISTENCIA:
          return codigo === 'M';
        case TaxaTipos.CANCELAMENTO_DE_PROGRAMACAO:
        case TaxaTipos.CANCELAMENTO_DE_PROGRAMACAO_BOMBEADO:
          return codigo === 'V' || codigo === 'P';
        default:
          return codigo !== 'H' && codigo !== 'K' && codigo !== 'R';
      }
    }

    if (this.taxaOpcoesVolume.length === 0) {
      this._obraTaxaService.listarOpcoes('volume').then(
        opcoes => { this.taxaOpcoesVolume = opcoes.map(t => t.descricao) },
        error => { this.taxaOpcoesVolume = [] }
      );
    }
    
    let obraTaxas = this.proposta.obra.obraTaxas;

    this.indexTaxa = obraTaxas.indexOf(taxa);
    this.taxa = JSON.parse(JSON.stringify(obraTaxas[this.indexTaxa]));

    this.confirmTaxa = confirmTaxaCallback;
    this.cancelTaxa = cancelTaxaCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.taxaModalVCR });
    this._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    this.modalIsOpen = true;
  }

  confirmPagamento: Function;
  cancelPagamento: Function;
  async openPagamentoModal(content, confirmPagamentoCallback: Function, cancelPagamentoCallback?: Function, pagamento?: Pagamento) {
    let pagamentos = this.proposta.obra.obraPagamentos;

    if (pagamento) {
      this.indexPagamento = pagamentos.indexOf(pagamento);
      this.pagamento = JSON.parse(JSON.stringify(pagamentos[this.indexPagamento]));
    } else {
      this.pagamento = new Pagamento();
      let valorSugerido = this.valorTotalProposta() - this.valorTotalPagamentos();
      if (valorSugerido < 0) valorSugerido = 0;
      this.pagamento.valor = valorSugerido;
      this.pagamento.sequencia = this.getFreeSequence(pagamentos.map(t => t.sequencia), false, 99);
    }

    await this.carregaCondicoesPagamento();

    this.confirmPagamento = confirmPagamentoCallback;
    this.cancelPagamento = cancelPagamentoCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.pagamentoItemModalVCR });
    this._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    this.modalIsOpen = true;
  }
  openPagamentoDetalheModal(content, confirmPagamentoCallback: Function, cancelPagamentoCallback?: Function, pagamento?: Pagamento) {
    let pagamentos = this.proposta.obra.obraPagamentos;

    if (pagamento) {
      this.indexPagamento = pagamentos.indexOf(pagamento);
      this.pagamento = JSON.parse(JSON.stringify(pagamentos[this.indexPagamento]));
    } else {
      this.pagamento = new Pagamento();
    }
    /*
    if (this.pagamento.tipoCobranca && this.pagamento.detalhes && this.pagamento.detalhes.length===0) {
      this.addPagamentoDetalhe(this.pagamento);
    }
    */
    this.confirmPagamento = confirmPagamentoCallback;
    this.cancelPagamento = cancelPagamentoCallback || this.closeModal;

    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.pagamentoItemDetalheModalVCR });
    this._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    this.modalIsOpen = true;
  }

  openTributacoesMunicipaisModal() {
    this.dialog.open(ObraTributacoesMunicipaisDialogComponent, {
      data: {
        disabled: this.alteracaoNaoPermitida(),
        tributacoesMunicipais: JSON.parse(JSON.stringify(this.proposta.obra.obraTributacoesMunicipais)),
        usinas: this.usinas,
        confirmTributacoesMunicipaisCallback: this.updateTributacoesMunicipais
      }
    });
  }

  confirmSolicitaAprovacoes: Function;
  cancelSolicitaAprovacoes: Function;
  openSolicitaAprovacoesModal() {
    let self = PropostaPageComponent.self;

    self.confirmSolicitaAprovacoes = () => {
      self.closeModal();
      self.onComplete();
    };
    self.cancelSolicitaAprovacoes = self.closeModal;

    self._dialogRef = self.dialog.open(self.solicitaAprovacoesModal, { viewContainerRef: self.solicitaAprovacoesModalVCR });
    self._dialogRef.keydownEvents().subscribe(event => {
      var element = event.target as HTMLElement;
      if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
    })
    self.modalIsOpen = true;
    self.detectChanges();
  }

  openSolicitaPagamentoCartaoModal() {
    let self = PropostaPageComponent.self;

    self.dialog.open(SolicitacaoPagamentoCartaoPageComponent, {
      data: { proposta: self.proposta,
              pagamentoSelecionado: self.pagamento }
    });
  }

  openEbitdaTracoModal(content, obraTraco: ObraTraco) {
    let self = PropostaPageComponent.self;
    
    this._custoServicoService.ObterCustoServicoVigentePorUsina(self.proposta.obra.usinaEntrega.codigo).then( 
      custoServico => { this.custoServico = custoServico;}
      );

    this._obraService.CalcularEbitdaObraTraco(obraTraco, self.proposta.obra).then(
      obraTracoEbitda => { this.tracoExibicaoEbitda = obraTracoEbitda; }
      ).then(() =>   {
        self.modalIsOpen = true  
        this._cdr.detectChanges();
        });

      self._dialogRef = self.dialog.open(content, { viewContainerRef: self.ebitdaTracoModalVCR });
      self._dialogRef.keydownEvents().subscribe(event => {
        var element = event.target as HTMLElement;
        if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
      })
      self.detectChanges();
  }

  openEbitdaBombaModal(content, obraBomba: ObraBomba) {
    let self = PropostaPageComponent.self;
    
    this._custoServicoService.ObterCustoServicoVigentePorUsina(self.proposta.obra.usinaEntrega.codigo).then( 
      custoServico => { this.custoServico = custoServico;}
      );

    this._obraService.CalcularEbitdaObraBomba(obraBomba, self.proposta.obra).then(
      obraBombaEbitda => { this.bombaExibicaoEbitda = obraBombaEbitda; }
      ).then(() =>   {
        self.modalIsOpen = true  
        this._cdr.detectChanges();
        });

      self._dialogRef = self.dialog.open(content, { viewContainerRef: self.ebitdaBombaModalVCR });
      self._dialogRef.keydownEvents().subscribe(event => {
        var element = event.target as HTMLElement;
        if (event.keyCode === 13 && element.tagName.toLowerCase()!=='button') event.preventDefault();
      })
      self.detectChanges();
  }

  quantidadeBombeada(): number {
    let volumeBombeavel: number = 0;
    this.proposta.obra.obraTracos.filter(t => t.slump.codigo >= 9).forEach(traco => {
      volumeBombeavel += traco.m3QuantidadeBombeada;
    });
    return volumeBombeavel;
  }
  

  range(size: number, startAt: number = 0): number[] {
    return Array.from({length: size}, (x, i) => startAt + i);
  }

  getFreeSequence(sequence: number[], next: boolean = true, limit?: number): number {
    let self = PropostaPageComponent.self;
    
    var max = Math.max(...sequence, 0);

    var result = 0;
    
    if (next) {
      result = max + 1;
    } else {
      var possible = self.range(limit || max, 1);
      var notUsed = possible.filter(t => !sequence.includes(t));

      if (notUsed.length > 0)
        result = Math.min(...notUsed);
      else
        result = max + 1;
    }

    if (!limit || result <= limit)
      return result;
      
    var message = `Sequencia máxima atingida: ${limit}!`;

    self.dialog.open(AlertDialogComponent, {
      disableClose: true,
      data: {
        title: 'TopConWeb',
        message: message
      }
    });

    throw message;
  }

  addTraco(newTraco: ObraTraco): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraTracos.push(newTraco);
    self.getObraTracoStatus();
    self.closeModal();
    this.isTracoInsertMode = false;
  }
  addBomba(newBomba: ObraBomba): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraBombas.push(newBomba);
    self.closeModal();
  }
  addDemaisServicos(newItem: ObraDemaisServicos): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraDemaisServicos.push(newItem);
    self.closeModal();
  }
  addPagamento(newPagamento: Pagamento): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraPagamentos.push(newPagamento);
    if (self.proposta.obra.obraPagamentos.length === 1) {
      self.proposta.obra.condicaoPagamento = newPagamento.condicaoPagamento;
      self.proposta.obra.tipoCobranca = newPagamento.tipoCobranca;
    }
    self.closeModal();
  }
  addPagamentoDetalhe(pagamento: Pagamento) {
    var self = PropostaPageComponent.self;

    switch (pagamento.tipoCobranca.forma) {
        case 'CC':
        case 'CD':
          let detalheCartao = new PagamentoDetalheCartao();
          detalheCartao.detalheSequencia = self.getFreeSequence(pagamento.detalhes.map(t => t.detalheSequencia), false, 1);
          detalheCartao.valor = pagamento.valor - pagamento.detalhes.map(t => t.valor).reduce((p, c) => p+c, 0);
          if (detalheCartao.valor < 0) detalheCartao.valor = 0;
          detalheCartao.quantidadeParcelas = pagamento.condicaoPagamento.quantidadeParcelas;
          pagamento.detalhes.push(detalheCartao);
          break;
        case 'DP':
          let detalheDeposito = new PagamentoDetalheDeposito();
          detalheDeposito.detalheSequencia = self.getFreeSequence(pagamento.detalhes.map(t => t.detalheSequencia), false, 1);
          detalheDeposito.valor = pagamento.valor - pagamento.detalhes.map(t => t.valor).reduce((p, c) => p+c, 0);
          if (detalheDeposito.valor < 0) detalheDeposito.valor = 0;
          pagamento.detalhes.push(detalheDeposito);
          break;
        case 'CH':
        case 'CP':
          let detalheCheque = new PagamentoDetalheCheque();
          detalheCheque.detalheSequencia = self.getFreeSequence(pagamento.detalhes.map(t => t.detalheSequencia), false, 1);
          detalheCheque.valor = pagamento.valor - pagamento.detalhes.map(t => t.valor).reduce((p, c) => p+c, 0);
          if (detalheCheque.valor < 0) detalheCheque.valor = 0;
          pagamento.detalhes.push(detalheCheque);
          break;
        case 'BE':
          let detalheBoleto = new PagamentoDetalheBoleto();
          detalheBoleto.detalheSequencia = self.getFreeSequence(pagamento.detalhes.map(t => t.detalheSequencia), false, 1);
          detalheBoleto.valor = pagamento.valor - pagamento.detalhes.map(t => t.valor).reduce((p, c) => p+c, 0);
          if (detalheBoleto.valor < 0) detalheBoleto.valor = 0;
          pagamento.detalhes.push(detalheBoleto);
          break;
        case 'DN':
          let detalheDinheiro = new PagamentoDetalheDinheiro();
          detalheDinheiro.detalheSequencia = self.getFreeSequence(pagamento.detalhes.map(t => t.detalheSequencia), false, 1);
          detalheDinheiro.valor = pagamento.valor - pagamento.detalhes.map(t => t.valor).reduce((p, c) => p+c, 0);
          if (detalheDinheiro.valor < 0) detalheDinheiro.valor = 0;
          pagamento.detalhes.push(detalheDinheiro);
          break;
        default:
          break;
      }
  }

  async removeTraco(traco: ObraTraco) {
    var self = PropostaPageComponent.self;
    var temProgramacao = await this.tracoTemProgramacao(traco);

    if (temProgramacao) {
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: 'Traço não pode ser excluído pois já existe programação!'
        }
      });
      return;
    }

    this.proposta.obra.obraTracos = this.proposta.obra.obraTracos
      .filter(i => {
        if (i != traco) return i;
      });
    this.getObraTracoStatus();
    this.detectChanges();
    this.concretoForm.controls['qtdTracos'].updateValueAndValidity();
  }
  async removeBomba(bomba: ObraBomba) {
    var self = PropostaPageComponent.self;
    var temProgramacao = await this.bombaTemProgramacao(bomba);

    if (temProgramacao) {
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: 'Bomba não pode ser excluída pois já existe programação!'
        }
      });
      return;
    }

    this.proposta.obra.obraBombas = this.proposta.obra.obraBombas
      .filter(i => {
        if (i != bomba) return i;
      });
    this.detectChanges();
  }
  async removeDemaisServicos(item: ObraDemaisServicos) {
    var self = PropostaPageComponent.self;
    var temProgramacao = await this.demaisServicoTemProgramacao(item);

    if (temProgramacao) {
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: 'Item não pode ser excluído pois já existe programação!'
        }
      });
      return;
    }

    this.proposta.obra.obraDemaisServicos = this.proposta.obra.obraDemaisServicos
      .filter(i => {
        if (i != item) return i;
      });
    
    this.detectChanges();
  }
  removePagamento(pagamento: Pagamento): void {
    var self = PropostaPageComponent.self;
    if (this.proposta.obra.obraPagamentos.filter(t => t.ativoSimNao === 'S').length == 1) {
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: 'Pagamento não pode ser excluído! A proposta deve possuir pelo menos 1 pagamento ativo!'
        }
      });
      return;
    }

    this.proposta.obra.obraPagamentos = this.proposta.obra.obraPagamentos
      .filter(i => {
        if (i != pagamento) return i;
      });
    this.detectChanges();
    this.pagamentoForm.controls['qtdPagamentos'].updateValueAndValidity();
  }
  removePagamentoDetalhe(pagamento: Pagamento, detalhe: IPagamentoDetalhe): void {
    pagamento.detalhes = pagamento.detalhes
      .filter(i => {
        if (i != detalhe) return i;
      });
  }

  updateTraco(traco: ObraTraco): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraTracos[self.indexTraco] = traco;
    self.getObraTracoStatus();
    self.closeModal();
  }
  updateBomba(bomba: ObraBomba): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraBombas[self.indexBomba] = bomba;
    self.closeModal();
  }
  updateDemaisServicos(item: ObraDemaisServicos): void {
    let self = PropostaPageComponent.self;
    self.proposta.obra.obraDemaisServicos[self.indexDemaisServicos] = item;
    self.closeModal();
  }
  updateTaxa(taxa: ObraTaxa): void {
    let self = PropostaPageComponent.self;
    if (self.taxaFoiAlterada(taxa)) taxa.aprovada = '';
    taxa.descricao = self.montaDescricaoTaxaExtra(taxa);
    taxa.isPersonalizada = true;
    self.proposta.obra.obraTaxas[self.indexTaxa] = taxa;
    self.closeModal();
  }
  async updatePagamento(pagamento: Pagamento) {

    let self = PropostaPageComponent.self;

    var confirm = () => {
      var pagamentoModificado= self.proposta.obra.obraPagamentos[self.indexPagamento];

      if (pagamentoModificado && pagamentoModificado.condicaoPagamento.codigo !== pagamento.condicaoPagamento.codigo && pagamento.valorApropriado > 0 && pagamento.idAprovacao === '' && pagamentoModificado.sequencia===pagamento.sequencia) {
        pagamentoModificado.ativoSimNao='N';

        pagamento.sequencia = this.getFreeSequence(self.proposta.obra.obraPagamentos.map(t => t.sequencia), false, 99);
        pagamento.detalhes = [];
        self.proposta.obra.obraPagamentos.push(pagamento);        
      
      } else {
        if (self.proposta.obra.obraPagamentos.length === 1 || self.proposta.obra.obraPagamentos.filter(t => t.ativoSimNao === 'S').length === 1 ) {
          self.proposta.obra.condicaoPagamento = pagamento.condicaoPagamento;
          self.proposta.obra.tipoCobranca = pagamento.tipoCobranca;
        }
        self.proposta.obra.obraPagamentos[self.indexPagamento] = pagamento;
      }

      self.closeModal();
    };
    
    let formasCartao: string[] = ['CC','CD'];
    var temDetalhamentoCartao = pagamento.detalhes && pagamento.detalhes.length > 0
      && formasCartao.includes(pagamento.tipoCobranca.forma);

    if (temDetalhamentoCartao) {
      var mensagemDuplicidade: string[] = [];


      for (let index = 0; index < pagamento.detalhes.length; index++) {
        var duplicadoNoProprioItem: boolean = false;
        var duplicadoEmOutroItem: boolean = false;

        var i = <PagamentoDetalheCartao>pagamento.detalhes[index];
        
        pagamento.detalhes.forEach(other => {
          var o = <PagamentoDetalheCartao>other;
          if (i.detalheSequencia !== o.detalheSequencia && i.numeroCartao === o.numeroCartao && i.numeroAutorizacao === o.numeroAutorizacao) {
            duplicadoNoProprioItem = true;
          }
        });

        self.proposta.obra.obraPagamentos.forEach(t => {
          if (t.sequencia !== pagamento.sequencia && t.detalhes && t.detalhes.length > 0 && formasCartao.includes(t.tipoCobranca.forma)) {
            t.detalhes.forEach(other => {
              var o = <PagamentoDetalheCartao>other;
              if (i.numeroCartao === o.numeroCartao && i.numeroAutorizacao === o.numeroAutorizacao) {
                duplicadoEmOutroItem = true;
              }
            });
          }
        });

        var erroBackend = false;
        var obrasDuplicidadeCartao = await self._obraService.ListarPorNumeroCartaoAutorizacaoDuplicado(self.proposta.obra, i.numeroCartao, i.numeroAutorizacao)
        .then(obras => obras, err => {
          erroBackend = true;
          return <ObraSimplesDTO[]>[];
        });

        if (obrasDuplicidadeCartao.length > 0) {
          obrasDuplicidadeCartao.forEach(o => {
            var info = "Duplicidade";
            if (o.numContrato > 0) info += ` Contrato '${o.numContrato}/${o.anoContrato}'`;
            if (o.numChamada > 0) info += ` Proposta '${o.numChamada}/${o.anoChamada}'`;
            if (o.intervenienteNome) info += ` Cliente '${o.intervenienteNome}'`;
            if (o.vendedorNome) info += ` Vendedor '${o.vendedorNome}'`;
            mensagemDuplicidade.push(`*Numero cartão '${i.numeroCartao}' Autorização '${i.numeroAutorizacao}' -> ${info}`);
          });
        } else if (erroBackend) {
          mensagemDuplicidade.push(`*Numero cartão '${i.numeroCartao}' Autorização '${i.numeroAutorizacao}' -> Erro ao tentar validar duplicidade de cartão`);
        }

        if (duplicadoNoProprioItem) {
          mensagemDuplicidade.push(`*Numero cartão '${i.numeroCartao}' Autorização '${i.numeroAutorizacao}' -> Duplicidade em outro(s) detalhamento(s) do mesmo pagamento`);
        }
        if (duplicadoEmOutroItem) {
          mensagemDuplicidade.push(`*Numero cartão '${i.numeroCartao}' Autorização '${i.numeroAutorizacao}' -> Duplicidade em outro(s) pagamento(s) da mesma proposta`);
        }
      }

      if (mensagemDuplicidade.length === 0) {
        var listaDuplicados = self.proposta.obra.numContrato ?
          self._contasAReceberService.ListarPorNumeroCartaoAutorizacaoDuplicado(self.proposta.obra.usinaCodigo, self.proposta.obra.anoContrato, self.proposta.obra.numContrato, i.numeroCartao, i.numeroAutorizacao) :
          self._contasAReceberService.ListarPorNumeroCartaoAutorizacao(i.numeroCartao, i.numeroAutorizacao);
        
         var titulosDuplicidateCartao = await listaDuplicados
        .then(obras => obras, err => {
          erroBackend = true;
          return <TituloContasAReceber[]>[];
        });

        if (titulosDuplicidateCartao.length > 0) {
          titulosDuplicidateCartao.forEach(car => {
            var info = "Duplicidade";
            info += ` Nº Documento: '${car.documentoNumero}'(Contas a receber)`;
            if (car.contratoNumero > 0) info += ` Contrato '${car.contratoNumero}/${car.contratoAno}'`;
            mensagemDuplicidade.push(`*Numero cartão '${i.numeroCartao}' Autorização '${i.numeroAutorizacao}' -> ${info}`);
          });
        } else if (erroBackend) {
          mensagemDuplicidade.push(`*Numero cartão '${i.numeroCartao}' Autorização '${i.numeroAutorizacao}' -> Erro ao tentar validar duplicidade de cartão`);
        }
      }

      if (mensagemDuplicidade.length > 0) {
        self.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `INCONSCISTÊNCIAS:\n${mensagemDuplicidade
              .filter((value, index, self) => self.indexOf(value) === index) //essa linha é uma espécie de 'DISTINCT'
              .join("\n")}`
          }
        });
      } else {
        confirm();
      }
    } else {
      confirm();
    }
  }
  updateTributacoesMunicipais(tributacoesMunicipais: ObraTributacaoMunicipal[]): boolean {
    let self = PropostaPageComponent.self;

    var usinaDuplicada = false;

    tributacoesMunicipais.forEach(t => {
      var repeticoes = tributacoesMunicipais.filter(u => u.usinaEntregaCodigo === t.usinaEntregaCodigo).length;
      if (repeticoes > 1) usinaDuplicada = true

      if (!usinaDuplicada && t.usinaEntregaCodigo === 0) {
        self.proposta.codigoObraPrefeitura = t.codigoObraPrefeitura;
      }
    });

    if (usinaDuplicada) {
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: 'Usina duplicada!'
        }
      });
      return false;
    }
    
    self.proposta.obra.obraTributacoesMunicipais = tributacoesMunicipais;
    return true;
  }

  volumeTotalTracos(): number {
    let total: number = 0;
    this.proposta.obra.obraTracos.forEach(item => {
      total += item.m3Quantidade;
    });

    return total;
  }
  valorTotalTracos(): number {
    let total: number = 0;
    this.proposta.obra.obraTracos.forEach(item => {
      total += (item.m3PrecoProposto * item.m3Quantidade);
    });

    return total;
  }
  volumeTotalTracosConsumido(): number {
    return this.volumeTotalConsumido;
  }
  valorTotalBomba(bomba: ObraBomba): number {
    var valorPorM3 = (this.utilizaCobrancaPorM3(bomba) ? (bomba.taxaMinimaReajustadaAtual != 0 ? bomba.taxaMinimaReajustadaAtual : bomba.taxaMinimaPrecoProposto) : 0.0);
    var valorPorHora = (bomba.bombaPropria && this.utilizaCobrancaPorHora(bomba) ? bomba.horaTaxaMinimaPrecoProposto : 0.0);
    return valorPorM3 + valorPorHora;
  }
  valorTotalBombas(): number {
    let total: number = 0;
    let volumeBombeavel: number = 0;
    let bombasUtilizadasNoCalculo: ObraBomba[] = [];
    
    let taxaMinima: number = 0;
    let m3Ate: number = 0;
    let m3Preco: number = 0;

    this.proposta.obra.obraTracos.filter(t => t.slump.codigo >= 9).forEach(traco => {
      volumeBombeavel += traco.m3QuantidadeBombeada;
    });

    bombasUtilizadasNoCalculo = this.proposta.obra.obraBombas;
    if(this.naoConsideraTodasBombas)
      bombasUtilizadasNoCalculo = bombasUtilizadasNoCalculo.slice(0,1);

    bombasUtilizadasNoCalculo.forEach(t => {
      taxaMinima = t.taxaMinimaReajustadaAtual != 0 ? t.taxaMinimaReajustadaAtual : t.taxaMinimaPrecoProposto;
      m3Ate = t.m3ReajustadoAteAtual != 0 ? t.m3ReajustadoAteAtual : t.m3PropostoAte;
      m3Preco = t.m3PrecoReajustadoAtual != 0 ? t.m3PrecoReajustadoAtual : t.m3PrecoProposto;

      switch (t.tipoCalculo) {
        case EBombaM3CalculoTipo.taxaMinimaOuExcedente:
          total += ((volumeBombeavel * m3Preco) + t.valorAdicionalTubulacao);
          break;
        case EBombaM3CalculoTipo.taxaMinimaMaisExcedente:
          total += (taxaMinima
            + (m3Ate > 0 && volumeBombeavel > m3Ate ?
                (volumeBombeavel - m3Ate) * m3Preco : 0)
            + t.valorAdicionalTubulacao);
            break;
        default:
          total += t.valorAdicionalTubulacao;
      }

      if (t.bombaPropria && this.utilizaCobrancaPorHora(t)) {
        total += t.horaTaxaMinimaPrecoProposto;
      }
    });

    return total;
  }
  valorItemDemaisServicos(item: ObraDemaisServicos): number {
    switch (item.frequenciaDeCobranca) {
      case EFrequenciaDeCobranca.Bombeamento:
        return this.proposta.obra.obraBombas.length > 0 ? item.precoProposto * item.quantidade : 0;
      case EFrequenciaDeCobranca.M3Bombeado:
        return this.proposta.obra.obraBombas.length > 0 ? item.precoProposto * item.quantidade * (this.volumeTotalTracos() || this.proposta.obra.volumeEstimado) : 0;
      case EFrequenciaDeCobranca.M3:
        return item.precoProposto * item.quantidade * (this.volumeTotalTracos() || this.proposta.obra.volumeEstimado);
      case EFrequenciaDeCobranca.Programacao:
        return item.precoProposto * item.quantidade;
      case EFrequenciaDeCobranca.Remessa:
       var viagens = Math.ceil((this.volumeTotalTracos() || this.proposta.obra.volumeEstimado) / (this.proposta.obra.volumePorCarga || 8));
        return item.precoProposto * item.quantidade * viagens;
      case EFrequenciaDeCobranca.Contrato:
        return item.precoProposto * item.quantidade;
      default:
        return item.precoProposto * item.quantidade;
    }
  }
  valorTotalDemaisServicos(): number {
    var self = PropostaPageComponent.self;
    let total: number = 0;
    self.proposta.obra.obraDemaisServicos.forEach(item => {
      total += self.valorItemDemaisServicos(item);
    });

    return total + self.vibradorValorTotal;
  }
  valorExtras(): number {
    return this.proposta.valorExtras;
  }
  valorTotalProposta(): number {
    return this.valorTotalTracos() + this.valorTotalBombas() + this.valorExtras() + this.valorTotalDemaisServicos();
  }
  valorTotalPagamentos(): number {
    let total: number = 0;
    this.proposta.obra.obraPagamentos.filter(t => t.ativoSimNao === 'S').forEach(item => {
      total += item.valor;
    });

    return total;
  }

  valorTotalPagamentosInativos(): number {
    let total: number = 0;
    this.proposta.obra.obraPagamentos.filter(t => t.ativoSimNao === 'N').forEach(item => {
      total += item.valor;
    });

    return total;
  }

  temDetalhamento(pagamento: Pagamento): boolean {
    let formasDetalhadas: string[] = ["DP", "CC", "CD", "CH", "CP", "DN"];
    return formasDetalhadas.includes(pagamento.tipoCobranca.forma);
  }

  isVinculoMpa(resistenciaTipo: ResistenciaTipo): boolean {
    if (!resistenciaTipo) return false;
    return resistenciaTipo.vinculo === ETipoVinculoMpaConsumo.MPA;
  }
  isVinculoConsumo(resistenciaTipo: ResistenciaTipo): boolean {
    if (!resistenciaTipo) return false;
    return resistenciaTipo.vinculo === ETipoVinculoMpaConsumo.CONSUMO;
  }

  isPagamentoInativo(pagamento: Pagamento): boolean {
    return (pagamento.ativoSimNao === 'N');
  }
  isPagamentoAprovado(pagamento: Pagamento): boolean {
    return (pagamento.idAprovacao !== '');
  }
  isBoletoImpresso(pagamento: Pagamento): boolean {
    let formasBoleto: string[] = ['BE','BL'];
    if (formasBoleto.includes(pagamento.tipoCobranca.forma)) {
      if (pagamento.detalhes && pagamento.detalhes.length === 1) {
        let detalhe = <PagamentoDetalheBoleto>pagamento.detalhes[0];
        return (detalhe.dataHoraImpressao !== null && detalhe.dataHoraImpressao !== undefined);
      }
    }
    return false;
  }
  dataHoraImpressaoBoleto(pagamento: Pagamento): string {
    if (!this.isBoletoImpresso(pagamento)) return '';
    let detalhe = <PagamentoDetalheBoleto>pagamento.detalhes[0];
    return Tasks.formataDataHora(detalhe.dataHoraImpressao);
  }

  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;
  formataData = Tasks.formataData;

  descontoPercentualTraco(traco: ObraTraco): number {
    traco.descontoPercentual = 0;
    if (traco.m3PrecoTabela === 0) return 0;
    traco.descontoPercentual = (1 - (traco.m3PrecoProposto / (traco.m3PrecoAjustado === 0 ? traco.m3PrecoTabela : traco.m3PrecoAjustado))) * 100;
    return traco.descontoPercentual;
  }
  descontoPercentualReajusteTraco(traco: ObraTraco): number {
    if (!this.proposta.obra.contrato.contratoTracoReajustes) return 0;
    var contratoReajuste = this.proposta.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste)[0];
    if (!contratoReajuste) return 0;
    return (1 - (traco.precoReajustadoAtual / contratoReajuste.precoRecalculado)) * 100;
  }
  
  descontoPercentualBomba(bomba: ObraBomba): number {
    bomba.descontoPercentual = 0;
    if (bomba.taxaMinimaPrecoTabela === 0) return 0;
    bomba.descontoPercentual = (1 - ((bomba.taxaMinimaReajustadaAtual != 0 ? bomba.taxaMinimaReajustadaAtual : bomba.taxaMinimaPrecoProposto) / bomba.taxaMinimaPrecoTabela)) * 100;
    return bomba.descontoPercentual;
  }

  descontoPercentualBombaM3(bomba: ObraBomba): number {
    if (bomba.m3PrecoTabela === 0) return 0;
    return (1 - ((bomba.m3PrecoReajustadoAtual != 0 ? bomba.m3PrecoReajustadoAtual : bomba.m3PrecoProposto) / bomba.m3PrecoTabela)) * 100;
  }

  descontoPercentualBombaHora(bomba: ObraBomba): number {
    bomba.horaDescontoPercentual = 0;
    if (bomba.horaTaxaMinimaPrecoTabela === 0) return 0;
    bomba.horaDescontoPercentual = (1 - (bomba.horaTaxaMinimaPrecoProposto / bomba.horaTaxaMinimaPrecoTabela)) * 100;
    return bomba.horaDescontoPercentual;
  }

  taxaMinimaPrecoPropostoPorM3(bomba: ObraBomba): number {
    if (this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba) bomba.taxaMinimaPrecoProposto = bomba.m3PropostoAte * bomba.m3PrecoProposto;
    return bomba.taxaMinimaPrecoProposto;
  }

  taxaMinimaPrecoReajustadoPorM3(bomba: ObraBomba): number {
    if (this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba) bomba.taxaMinimaReajustadaAtual = bomba.m3ReajustadoAteAtual * bomba.m3PrecoReajustadoAtual;
    return bomba.taxaMinimaReajustadaAtual;
  }

  taxaMinimaPrecoPropostoPorHora(bomba: ObraBomba): number {
    if (this.bloqueiaCampoTaxaMinimaNoPrecoDeBomba) bomba.horaTaxaMinimaPrecoProposto = bomba.horaPropostoAte * bomba.horaPrecoProposto;
    return bomba.horaTaxaMinimaPrecoProposto;
  }

  possuiContratoReajuste(traco: ObraTraco): boolean {
    if(!this.proposta.obra || !this.proposta.obra.contrato || !this.proposta.obra.contrato.contratoTracoReajustes) return false;
    return this.proposta.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste).length > 0;
  }
  precoUltimoContratoReajuste(traco: ObraTraco): number {
    var contratoReajuste = this.proposta.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste)[0];
    if (!contratoReajuste) return 0;
    return contratoReajuste.precoRecalculado
  }

  possuiReajusteBomba(bomba: ObraBomba): boolean {
    if(!this.proposta.obra || !this.proposta.obra.contrato || !this.proposta.obra.contrato.contratoBombaReajustes) return false;
    return this.proposta.obra.contrato.contratoBombaReajustes.filter(t => t.obraBombaReajusteSequencia === bomba.sequencia && t.dataVigencia === bomba.dataUltimoReajuste).length > 0;
  }
  precoUltimoReajusteBomba(bomba: ObraBomba): number {
    var bombaReajuste = this.proposta.obra.contrato.contratoBombaReajustes.filter(t => t.obraBombaReajusteSequencia === bomba.sequencia && t.dataVigencia === bomba.dataUltimoReajuste)[0];
    if (!bombaReajuste) return 0;
    return bombaReajuste.valorReajustado
  }

  percentualDescontoAcrescimoFormatadoTracoReajustado(traco: ObraTraco, obra: Obra): string {
    var contratoReajuste = this.proposta.obra.contrato.contratoTracoReajustes.filter(t => t.obraTracoSequencia === traco.sequencia && t.dataVigencia === traco.dataUltimoReajuste)[0];
    if (!contratoReajuste) return "";
    return this.formataValor(Math.abs((1 - (traco.precoReajustadoAtual / (contratoReajuste.precoRecalculado))) * 100), 2, true);
  }
  percentualDescontoAcrescimoFormatadoTraco(traco: ObraTraco): string {
    return this.formataValor(Math.abs(this.descontoPercentualTraco(traco)), 2, true);
  }
  percentualDescontoAcrescimoFormatadoBomba(bomba: ObraBomba): string {
    return this.formataValor(Math.abs(this.descontoPercentualBomba(bomba)), 2, true);
  }
  percentualDescontoAcrescimoFormatadoBombaM3(bomba: ObraBomba): string {
    return this.formataValor(Math.abs(this.descontoPercentualBombaM3(bomba)), 2, true);
  }
  percentualDescontoAcrescimoFormatadoBombaHora(bomba: ObraBomba): string {
    return this.formataValor(Math.abs(this.descontoPercentualBombaHora(bomba)), 2, true);
  }

  labelDescontoAcrescimoTraco(traco: ObraTraco): string {
    if (this.descontoPercentualTraco(traco) < 0)
      return 'Acréscimo';
    return 'Desconto';
  }
  labelDescontoAcrescimoTracoReajuste(traco: ObraTraco): string {
    if (this.descontoPercentualReajusteTraco(traco) < 0)
      return 'Acréscimo';
    return 'Desconto';
  }
  labelDescontoAcrescimoBomba(bomba: ObraBomba): string {
    if (this.descontoPercentualBomba(bomba) < 0)
      return 'Acréscimo';
    return 'Desconto';
  }
  labelDescontoAcrescimoBombaHora(bomba: ObraBomba): string {
    if (this.descontoPercentualBombaHora(bomba) < 0)
      return 'Acréscimo';
    return 'Desconto';
  }
  labelDescontoAcrescimoBombaReajuste(bomba: ObraBomba): string {
      if (this.possuiReajusteBomba(bomba))
        return ' reajustado';
      return '';
    }

  labelAcimaDe(taxa: ObraTaxa): string {
    switch (taxa.tipo) {
      case TaxaTipos.TAXA_PERMANENCIA_NA_OBRA:
        return 'Minutos acima de';
      case TaxaTipos.ADICIONAL_KM_RODADO:
        return 'KM rodado acima de';
      default:
        return '';
    }
  }

  onTracoStepSelected() {
    this.tracoShowContent = true;
  }
  onTracoReajusteStepSelected() {
    this.tracoReajusteShowContent = true;
 
    if(this.isInsertMode){
      this.proposta.obra.obraReajuste = new ObraReajuste();
      this.proposta.obra.obraReajuste.mensagemReajuste = this.parametroProposta.mensagemReajustePadrao;
    }
  }
  onBombaStepSelected() {
    this.bombaShowContent = true;
  }
  onDemaisServicosStepSelected() {
    this.demaisServicosShowContent = true;
  }
  onTaxaExtraStepSelected() {
    this.abaObrigatoriaRevisarTaxaExtra = false;
  }
  onPagamentoStepSelected() {
    this.pagamentoShowContent = true;
  }
  
  
  tracoNumeracaoProdutoChange(tracoPrecoNumeracaoProduto: TracoPrecoNumeracaoProduto) {
    if (!tracoPrecoNumeracaoProduto){
      this.desabilitaCamposTraco = false;

      this.traco.uso = null;
      this.traco.pedra = null;
      this.traco.slump = null;
      this.traco.slumpNominal = null;
      this.traco.resistenciaTipo = null;
      this.traco.mpa = 0.0;
      this.traco.consumo = 0;
      this.traco.status = 0;
      this.tracoDescricao = '';
    }

    if (tracoPrecoNumeracaoProduto){
      this.desabilitaCamposTraco = true;

      if(!this.tracoModalNumeracaoProdutoSelecionado){
        this.tracoModalNumeracaoProdutoSelecionado = new TracoPrecoNumeracaoProduto();
      }
      this.tracoModalNumeracaoProdutoSelecionado.numeracao = tracoPrecoNumeracaoProduto.numeracao;
      this.tracoModalNumeracaoProdutoSelecionado.usoDescricao = tracoPrecoNumeracaoProduto.usoDescricao;
      this.tracoModalNumeracaoProdutoSelecionado.status = tracoPrecoNumeracaoProduto.status;
      
      if(this.traco.numeracaoProduto !== tracoPrecoNumeracaoProduto.numeracao) 
        this.traco.descricaoPersonalizada = '';
      
      this.traco.numeracaoProduto = tracoPrecoNumeracaoProduto.numeracao;
      this.traco.status = tracoPrecoNumeracaoProduto.status;
      this.tracoParticularidades = new TracoParticularidades();
      this.carregaPreenchimentoTraco();
    } 
  }
  placeholderNumeracaoProduto: string = "Numeração Produto";
  carregaTracoNumeracoesProduto(dispararChange: boolean = true) {
    let self = PropostaPageComponent.self;

    self.numeracoesProduto = [];
    if (self.proposta.obra.usinaEntrega) {
      let placeholder = "Numeração Produto";
      self.placeholderNumeracaoProduto = LOADING_MESSAGE+placeholder;
      self._tracoPrecoService.ListarNumeracoesProdutoPorDataUsina(self.proposta.obra.usinaEntrega, self.proposta.segmentacao, true)
      .then(numeracoesProduto => {
          self.numeracoesProduto = numeracoesProduto.filter(t => t !== null);

          if (dispararChange) {
            if (self.tracoModalNumeracaoProdutoSelecionado && !numeracoesProduto.filter(t => t !== null)) {
              self.tracoModalNumeracaoProdutoSelecionado = null;
            }
          }
      }, err => self.numeracoesProduto = [])
      .then(() => {
        self.placeholderNumeracaoProduto = placeholder;
        self.detectChanges();
      });
    }
    self.carregaPreenchimentoTraco(dispararChange);
  }

  carregaPreenchimentoTraco(dispararChange: boolean = true) {
    let self = PropostaPageComponent.self;

    if (self.proposta.obra.usinaEntrega && self.tracoModalNumeracaoProdutoSelecionado.numeracao > 0) {
      this.proposta.obra.enderecoMunicipioCodigo = this.proposta.obra.endereco.municipio.codigo;
      self._tracoPrecoService.ObterPorNumeracaoProduto(self.proposta.obra.usinaEntrega, self.tracoModalNumeracaoProdutoSelecionado.numeracao, this.proposta.obra, false)
      .then(tracoPreco => {
          self.traco.uso = tracoPreco.uso;
          self.traco.pedra = tracoPreco.pedra;
          self.traco.slump = tracoPreco.slump;
          self.tracoSlumpNominalChange(tracoPreco.slump);
          self.traco.resistenciaTipo = tracoPreco.resistenciaTipo;
          self.carregaTracoMpasConsumos(dispararChange);
          self.traco.mpa = tracoPreco.mpa;
          self.traco.consumo = tracoPreco.consumo;
          self.traco.numeracaoProduto = tracoPreco.numeracaoProduto;
          self.tracoModalNumeracaoProdutoSelecionado.usoDescricao = tracoPreco.uso.descricao;
          self.verificaTracoJaIncluso();
          if (!self.tracoJaIncluso){
            self.traco.m3PrecoTabela = tracoPreco.m3Preco;
            self.traco.m3PrecoAjustado = tracoPreco.m3PrecoRecalculo;
            self.traco.custoTraco = tracoPreco.custoMaterial;
            self.carregaDescricaoPersonalizada();
            self.carregaPrecoPropostoDefault();
          }
      });
    }
  }

  tracoUsoChange(uso: Uso) {
    if (!this.traco.uso || !uso || this.traco.uso.codigo!==uso.codigo) {
      this.tracoModalNumeracaoProdutoSelecionado = null;
      this.traco.uso = uso;
      this.traco.pedra = null;
      this.traco.slump = null;
      this.traco.slumpNominal = null;
      this.traco.resistenciaTipo = null;
      this.traco.mpa = 0.0;
      this.traco.consumo = 0;
      this.tracoDescricao = '';
      this.tracoParticularidades = new TracoParticularidades();
      this.carregaTracoPedras();
    }

    if(!this.traco.uso)
      this.desabilitaNumeracaoProduto = false;
    else
      this.desabilitaNumeracaoProduto = true;
  }
  placeholderUso: string = "Uso";
  carregaTracoUsos(dispararChange: boolean = true) {
    this.usos = [];
    if (this.proposta.obra.usinaEntrega) {
      let placeholder = "Uso";
      this.placeholderUso = LOADING_MESSAGE+placeholder;
      this._tracoPrecoService.ListarUsosPorDataUsina(this.proposta.obra.usinaEntrega, this.proposta.segmentacao, true)
      .then(usos => {
        this.usos = usos.filter(t => t !== null);
        
        if (dispararChange) {
          if (this.traco.uso && !usos.filter(t => t !== null).map(t => t.codigo).includes(this.traco.uso.codigo)) {
            this.traco.pedra = null;
          }
  
          if (!this.traco.uso && usos.length === 1){
            this.tracoUsoChange(usos[0]);
          }
        }
      }, err => this.usos = [])
      .then(() => {
        this.placeholderUso = placeholder;
        this.detectChanges();
      });
    }
    this.carregaTracoPedras(dispararChange);
  }

  tracoPedraChange(pedra: Pedra) {
    if (!this.traco.pedra || !pedra || this.traco.pedra.codigo !== pedra.codigo) {
      this.tracoModalNumeracaoProdutoSelecionado = null;
      this.traco.pedra = pedra;
      this.traco.slump = null;
      this.traco.slumpNominal = null;
      this.traco.resistenciaTipo = null;
      this.traco.mpa = 0.0;
      this.traco.consumo = 0;
      this.carregaTracoSlumpsNominais();
    }
  }
  placeholderPedra: string = "Pedra";
  showPedra: boolean = true;
  carregaTracoPedras(dispararChange: boolean = true) {
    const SEM_BRITA: number = 99;

    this.pedras = [];
    if (this.proposta.obra.usinaEntrega && this.traco.uso) {
      let placeholder = "Pedra";
      this.placeholderPedra = LOADING_MESSAGE+placeholder;
      this._tracoPrecoService.ListarPedrasPorDataUsinaUso(this.proposta.obra.usinaEntrega, this.traco.uso, true)
      .then(pedras => {
        this.pedras = pedras;
        
        if (dispararChange) {
          if (this.traco.pedra && !pedras.map(t => t.codigo).includes(this.traco.pedra.codigo)) {
            this.traco.pedra = null;
          }
          
          if (!this.traco.pedra && pedras.length === 1){
            this.tracoPedraChange(pedras[0]);
          }
        }

        this.showPedra = !(this.pedras.length === 1 && this.traco.pedra && this.traco.pedra.codigo === SEM_BRITA);
      }, err => this.pedras = [])
      .then(() => {
        this.placeholderPedra = placeholder;
        this.detectChanges();
      });
    }
    //this.carregaTracoSlumps();
    this.carregaTracoSlumpsNominais(dispararChange);
  }

  /*tracoSlumpChange(slump: Slump) {
    if (!this.traco.slump || !slump || this.traco.slump.codigo !== slump.codigo) {
      this.traco.slump = slump;
      this.traco.slumpNominal = null;
      this.traco.resistenciaTipo = null;
      this.traco.mpa = 0.0;
      this.traco.consumo = 0;
    }
  }
  placeholderSlump: string = "Slump real";
  carregaTracoSlumps() {
    this.slumps = [];
    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra) {
      let placeholder = "Slump real";
      this.placeholderSlump = LOADING_MESSAGE+placeholder;
      this._tracoPrecoService.ListarSlumpsPorDataUsinaUsoPedra(new Date(this.proposta.data), this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, true)
      .then(slumps => this.slumps = slumps, err => this.slumps = [])
      .then(() => {
        this.placeholderSlump = placeholder;
        this.detectChanges();
      });
    }
    this.carregaTracoSlumpsNominais();
  }*/

  tracoSlumpNominalChange(slumpNominal: Slump) {
    if (!this.traco.slumpNominal || !slumpNominal || this.traco.slumpNominal.codigo !== slumpNominal.codigo) {
      this.traco.slumpNominal = slumpNominal;
      this.traco.resistenciaTipo = null;
      if (slumpNominal) this.traco.slump = {
        codigo: slumpNominal.amplitudeDe,
        descricao:'',
        amplitudeDe: slumpNominal.amplitudeDe,
        variacao: slumpNominal.variacao
      };
      this.traco.mpa = 0.0;
      this.traco.consumo = 0;
      this.traco.slump.codigo < 9 ?  this.traco.m3QuantidadeBombeada = 0 : this.traco.m3QuantidadeBombeada = this.traco.m3Quantidade;
      this.carregaTracoResistenciaTipos();
    }
  }
  placeholderSlumpNominal: string = "Slump";
  carregaTracoSlumpsNominais(dispararChange: boolean = true) {
    this.slumpsNominais = [];
    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra /*&& this.traco.slump*/) {
      let placeholder = "Slump";
      this.placeholderSlumpNominal = LOADING_MESSAGE+placeholder;
      //this._slumpService.listarPorSlumpReal(this.traco.slump, true)
      this._tracoPrecoService.ListarSlumpsNominaisPorDataUsinaUsoPedra(this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, true)
      .then(slumpsNominais => {
        this.slumpsNominais = slumpsNominais;
        
        if (dispararChange) {
          if (this.traco.slumpNominal && !slumpsNominais.map(t => t.codigo).includes(this.traco.slumpNominal.codigo)) {
            this.traco.slumpNominal = null;
            this.traco.slump = null;
          }

          if (!this.traco.slumpNominal && slumpsNominais.length === 1){
            this.tracoSlumpNominalChange(slumpsNominais[0]);
          }
        }

      }, err => this.slumpsNominais = [])
      .then(() => {
        this.placeholderSlumpNominal = placeholder;
        this.detectChanges();
      });
    }
    this.carregaTracoResistenciaTipos(dispararChange);
  }

  tracoResistenciaTipoChange(resistenciaTipo: ResistenciaTipo) {
    if (!this.traco.resistenciaTipo || !resistenciaTipo || this.traco.resistenciaTipo.codigo !== resistenciaTipo.codigo) {
      this.traco.resistenciaTipo = resistenciaTipo;
      this.traco.mpa = 0.0;
      this.traco.consumo = 0;
      this.carregaTracoMpasConsumos();
      if(this.traco.mpa == 0 && this.traco.consumo == 0){
        this.verificaTracoJaIncluso();
        if (this.tracoJaIncluso) {
          this.traco.mpa = 0;
          this.traco.consumo = 0;
          this.tracoDescricao = "";
          this.traco.m3PrecoTabela = 0;
          this.traco.m3PrecoAjustado = 0;
          this.traco.m3PrecoProposto = 0;
          this.traco.custoTraco = 0;
        }
        else this.carregaTracoPreco();
      }
      else this.carregaTracoPreco();
    }
  }
  placeholderResistenciaTipo: string = "Tipo de resistência";
  carregaTracoResistenciaTipos(dispararChange: boolean = true) {
    this.resistenciaTipos = [];
    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra && this.traco.slump && this.traco.slumpNominal) {
      let placeholder = "Tipo de resistência";
      this.placeholderResistenciaTipo = LOADING_MESSAGE+placeholder;
      this._tracoPrecoService.ListarResistenciaTiposPorDataUsinaUsoPedraSlump(this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, this.traco.slump, true)
      .then(resistenciaTipos => {
        this.resistenciaTipos = resistenciaTipos;
        
        if (dispararChange) {
          if (this.traco.resistenciaTipo && !resistenciaTipos.map(t => t.codigo).includes(this.traco.resistenciaTipo.codigo)) {
            this.traco.resistenciaTipo = null;
          }

          if (!this.traco.resistenciaTipo && resistenciaTipos.length === 1){
            this.tracoResistenciaTipoChange(resistenciaTipos[0]);
          }
        }

      }, err => this.resistenciaTipos = [])
      .then(() => {
        this.placeholderResistenciaTipo = placeholder;
        this.detectChanges();
      });
    }
    this.carregaTracoMpasConsumos(dispararChange);
  }

  tracoMpaChange(mpa: number) {
    if (this.traco.mpa !== mpa) {
      this.traco.mpa = mpa;
      this.traco.consumo = 0;
      this.verificaTracoJaIncluso();
      if (this.tracoJaIncluso) {
        this.traco.mpa = 0;
        this.tracoDescricao = "";
        this.traco.m3PrecoTabela = 0;
        this.traco.m3PrecoAjustado = 0;
        this.traco.m3PrecoProposto = 0;
        this.traco.custoTraco = 0;
      }
      else this.carregaTracoPreco();
    }
  }
  placeholderMpa: string = "MPA";
  carregaTracoMpas(dispararChange: boolean = true) {
    this.mpas = [];
    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra && this.traco.slump && this.traco.slumpNominal && this.traco.resistenciaTipo) {
      let placeholder = "MPA";
      this.placeholderMpa = LOADING_MESSAGE+placeholder;
      this._tracoPrecoService.ListarMpasPorDataUsinaUsoPedraSlumpResistenciaTipo(this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, this.traco.slump, this.traco.resistenciaTipo, true)
      .then(mpas => {
        this.mpas = mpas;
        
        if (dispararChange) {
          if (this.traco.mpa && !mpas.includes(this.traco.mpa)) {
            this.traco.mpa = 0.0;
          }

          if (!this.traco.mpa && mpas.length === 1){
            this.tracoMpaChange(mpas[0]);
            this.detectChanges();
          }
        }
      }, err => this.mpas = [])
      .then(() => {
        this.placeholderMpa = placeholder;
        this.detectChanges();
      });
    }
  }

  tracoConsumoChange(consumo: number) {
    if (this.traco.consumo !== consumo) {
      this.traco.consumo = consumo;
      this.traco.mpa = 0.0;
      this.verificaTracoJaIncluso();
      if (this.tracoJaIncluso) {
        this.traco.consumo = 0;
        this.tracoDescricao = "";
        this.traco.m3PrecoTabela = 0;
        this.traco.m3PrecoAjustado = 0;
        this.traco.m3PrecoProposto = 0;
        this.traco.custoTraco = 0;
      }
      else this.carregaTracoPreco();
    }
  }
  placeholderConsumo: string = "Consumo";
  carregaTracoConsumos(dispararChange: boolean = true) {
    this.consumos = [];
    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra && this.traco.slump && this.traco.slumpNominal && this.traco.resistenciaTipo) {
      let placeholder = "Consumo";
      this.placeholderConsumo = LOADING_MESSAGE+placeholder;
      this._tracoPrecoService.ListarConsumosPorDataUsinaUsoPedraSlumpResistenciaTipo(this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, this.traco.slump, this.traco.resistenciaTipo, true)
      .then(consumos => {
        this.consumos = consumos;
        
        if (dispararChange) {
          if (this.traco.consumo && !consumos.includes(this.traco.consumo)) {
            this.traco.consumo = 0;
          }

          if (!this.traco.consumo && consumos.length === 1){
            this.tracoConsumoChange(consumos[0]);
            this.detectChanges();
          }
        }

      }, err => this.consumos = [])
      .then(() => {
        this.placeholderConsumo = placeholder;
        this.detectChanges();
      });
    }
  }

  carregaTracoMpasConsumos(dispararChange: boolean = true) {
    this.carregaTracoMpas(dispararChange);
    this.carregaTracoConsumos(dispararChange);
  }

  isCarregandoTraco(): boolean {
    return this.placeholderConsumo.startsWith(LOADING_MESSAGE)
      || this.placeholderMpa.startsWith(LOADING_MESSAGE)
      || this.placeholderResistenciaTipo.startsWith(LOADING_MESSAGE)
      || this.placeholderSlumpNominal.startsWith(LOADING_MESSAGE)
      || this.placeholderPedra.startsWith(LOADING_MESSAGE)
      || this.placeholderUso.startsWith(LOADING_MESSAGE);
  }

  isTracoCustoVirtual(): boolean {
    return this.traco.status === EObraTracoStatus.CustoVirtual || (this.tracoModalNumeracaoProdutoSelecionado ? this.tracoModalNumeracaoProdutoSelecionado.status === 7105 : false);
  }

  bloquearTracoCustoVirtual(): boolean {
    return (this.proposta.obra.numContrato > 0 || this.proposta.statusProposta !== 1) && this.isTracoCustoVirtual()
  }

  motivoTracoCustoVirtualBloqueado(): string {
    if(this.proposta.obra.numContrato > 0)
      return 'Traço com status Custo Virtual não pode ser adicionado a proposta com contrato gerado'

    if(this.proposta.statusProposta !== 1)
      return 'Traço com status Custo Virtual não pode ser adicionado a proposta com status Aprovado'

    return ''

  }

  isTracoArquivado(): boolean {
    if (!this.traco.resistenciaTipo || !this.traco.slump || !this.traco.pedra || !this.traco.uso)
      return false;
    
    if (this.isCarregandoTraco())
      return false;
    
    switch (this.traco.resistenciaTipo.vinculo) {
      case ETipoVinculoMpaConsumo.SEM_VINCULO:
        return !this.resistenciaTipos.map(t => t.codigo).includes(this.traco.resistenciaTipo.codigo);
      case ETipoVinculoMpaConsumo.MPA:
        return this.traco.mpa > 0 && !this.mpas.includes(this.traco.mpa);
      case ETipoVinculoMpaConsumo.CONSUMO:
        return this.traco.consumo > 0 && !this.consumos.includes(this.traco.consumo);
      default:
        return false;
    }
  }

  isTracoBombeavel(): boolean {
    if (!this.traco.slump) return false;

    return this.traco.slump.codigo >= 9;
  }

  private _tracoM3QuantidadeAnterior: number = 0;
  private _tracoM3BombeadoPreenchido : boolean = false;
  tracoM3QuantidadeFocus() {    
    this._tracoM3QuantidadeAnterior = this.traco.m3Quantidade;
  }
  tracoM3QuantidadeFocusout() {
    if (this._tracoM3QuantidadeAnterior !== this.traco.m3Quantidade) {
      this.carregaPrecoPropostoDefault();
    }

    if (this.isTracoInsertMode && !this._tracoM3BombeadoPreenchido) {
      this.isTracoBombeavel() ? this.traco.m3QuantidadeBombeada = this.traco.m3Quantidade : this.traco.m3QuantidadeBombeada = 0;
    } else {
      if (this.traco.m3QuantidadeBombeada === this._tracoM3QuantidadeAnterior && this.isTracoBombeavel()) {
        this.traco.m3QuantidadeBombeada = this.traco.m3Quantidade;
      }
    }
    
  }

  tracoM3BombeadoQuantidadeFocusOut() {  
    if (this.traco.m3QuantidadeBombeada !== this.traco.m3Quantidade && this.isTracoBombeavel()) {
      this._tracoM3BombeadoPreenchido = true;
    }
  }

  private _m3PrecoAjustadoAnterior: number = 0;
  carregaPrecoPropostoDefault() {
    this._m3PrecoAjustadoAnterior = this.traco.m3PrecoAjustado;
    this.traco.m3PrecoAjustado = this.traco.m3PrecoTabela + this._valorAdicionalM3PorUsinaCep + this._valorAdicionalM3PorUsinaKm;

    var valorAdicionalM3PorVolume = 0;

    if (this.proposta.obra.usinaEntrega && this.traco.m3PrecoTabela) {
      this._tracoPrecoService.ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela
      (this.proposta.obra.usinaEntrega, this.traco.m3Quantidade, this.traco.m3PrecoTabela, true)
      .then(valorAdicional => {
        valorAdicionalM3PorVolume = valorAdicional;
        
        this.traco.m3PrecoAjustado = this.traco.m3PrecoTabela + this._valorAdicionalM3PorUsinaCep
          + this._valorAdicionalM3PorUsinaKm + valorAdicionalM3PorVolume;

          if (this.isTracoInsertMode) {
            this.traco.m3PrecoProposto = this.traco.m3PrecoAjustado;
          }
        
        this.detectChanges();
      })
      .then(() => {
        if (this.proposta.obra.condicaoPagamento && this.proposta.obra.usinaEntrega) {
          this._pagamentoService.ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela
          (this.proposta.obra.condicaoPagamento, this.proposta.obra.usinaEntrega, this.traco.m3PrecoTabela, true)
          .then(valorAdicional => {

            this.traco.m3PrecoAjustado = this.traco.m3PrecoTabela + this._valorAdicionalM3PorUsinaCep
              + this._valorAdicionalM3PorUsinaKm + valorAdicionalM3PorVolume + valorAdicional;

            if (this.isTracoInsertMode) {
              this.traco.m3PrecoProposto = this.traco.m3PrecoAjustado;
            }

              this.detectChanges();
          });
        }
      });
    }

    if (this.isTracoInsertMode) {
      this.traco.m3PrecoProposto = this.traco.m3PrecoAjustado;
    }
  }

  atualizaCustoServicoReajustado() {
    if (this.traco.custoTraco) {
      this.traco.custoServicoReajustado = this.traco.precoReajustadoAtual - this.traco.custoTraco;
    }
  }

  atualizaCustoServicoReajustadoItem(obraTraco: ObraTraco) {
    if (obraTraco.custoTraco) {
      obraTraco.custoServicoReajustado = obraTraco.precoReajustadoAtual - obraTraco.custoTraco;
    }
  }

  get valorServicoReajustadoTracoFormatado(): string {
    if (this.traco.custoTraco === undefined || this.traco.custoTraco === 0) {
      this.traco.custoTraco = this.traco.precoReajustadoAtual - this.traco.custoServicoReajustado
    }
    return this.formataValor(this.traco.m3PrecoTabela - this.traco.custoTraco - (this.traco.m3PrecoTabela - this.traco.precoReajustadoAtual), 2, true);
  }
  
  get valorServicoTracoFormatado(): string {
    if (this.traco.custoTraco === undefined || this.traco.custoTraco === 0) {
      if (this.traco.dataUltimoReajuste === undefined || this.traco.dataUltimoReajuste === null) {
        this.traco.custoTraco = this.traco.m3PrecoProposto - this.traco.valorServico 
      } else {
        this.traco.custoTraco = this.traco.precoReajustadoAtual - this.traco.custoServicoReajustado
      }
    }
    return this.formataValor(this.traco.m3PrecoTabela - this.traco.custoTraco - (this.traco.m3PrecoTabela - this.traco.m3PrecoProposto), 2, true);
  }

  AtualizaPrecoTracos() {
    this.proposta.obra.obraTracos.forEach(traco => {
      this.inserePrecoTraco(traco);
    });
  }

  carregaTracoPreco() {
    this.tracoDescricao = '';
    this.tracoParticularidades = new TracoParticularidades();
    this.traco.aprovacaoObservacao = '';
    this.traco.justificativa = '';

    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra && this.traco.slump && this.traco.slumpNominal && this.traco.resistenciaTipo
        && (this.traco.resistenciaTipo.vinculo===ETipoVinculoMpaConsumo.SEM_VINCULO || this.traco.mpa>0 || this.traco.consumo>0 ) ) {
      this.inserePrecoTraco(this.traco);
      this.carregaDescricaoPersonalizada();
      this.carregaTracoParticularidades();
    }
  }

  inserePrecoTraco(traco: ObraTraco){
    this.proposta.obra.enderecoMunicipioCodigo = this.proposta.obra.endereco.municipio.codigo;

    this._tracoPrecoService.ObterPorDataUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
    (this.proposta.obra.usinaEntrega, traco.uso, traco.pedra, traco.slump, traco.resistenciaTipo, traco.mpa, traco.consumo, this.proposta.obra, true)
    .then(tracoPreco => {
      if (tracoPreco) {
        traco.m3PrecoTabela = tracoPreco.m3Preco;
        traco.m3PrecoAjustado = tracoPreco.m3PrecoRecalculo;
        traco.custoTraco = tracoPreco.custoMaterial;
        if (tracoPreco.numeracaoProduto > 0) 
        {
          this.tracoModalNumeracaoProdutoSelecionado = new TracoPrecoNumeracaoProduto();
          this.tracoModalNumeracaoProdutoSelecionado.numeracao = tracoPreco.numeracaoProduto;
          this.tracoModalNumeracaoProdutoSelecionado.usoDescricao = tracoPreco.uso.descricao;
          this.traco.numeracaoProduto = tracoPreco.numeracaoProduto;
          this._tracoPrecoService.ObterStatusPorNumeracaoProduto(this.proposta.obra.usinaEntrega.codigo, this.traco.numeracaoProduto, this.proposta.obra)
          .then(status => {
            this.traco.status = status
            this.tracoModalNumeracaoProdutoSelecionado.status = status
            this.detectChanges();
          })
        }
        this.carregaPrecoPropostoDefault();
      }
    }, err => {})
    .then(() => {
      this.detectChanges();
    });
  }

  carregaDescricaoPersonalizada() {
    if (!this.traco.resistenciaTipo || !this.traco.pedra || !this.traco.slumpNominal || !this.traco.uso || (this.traco.mpa + this.traco.consumo)===0) this.tracoDescricao = '';

    this._mercadoriaService.obterTracoMercadoriaComDescricaoPersonalizada(
      this.traco.uso, this.traco.pedra, this.traco.slump, this.traco.resistenciaTipo, this.traco.mpa, this.traco.consumo
    ).then(mercadoria => {
      if (!mercadoria)
        this.tracoDescricao = this.tracoString(this.traco); 
      else 
      this.tracoDescricao = mercadoria.descricao;
    }, err =>{});
  }

  carregaTracoParticularidades(){
    if (this.proposta.obra.usinaEntrega && this.traco.uso && this.traco.pedra && this.traco.slump && this.traco.slumpNominal && this.traco.resistenciaTipo
      && (this.traco.resistenciaTipo.vinculo===ETipoVinculoMpaConsumo.SEM_VINCULO || this.traco.mpa>0 || this.traco.consumo>0 ) ) {
        this.tracoParticularidades = new TracoParticularidades();
        
        this._tracoPrecoService.ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoVersaoMpaConsumo(
        this.proposta.obra.usinaEntrega, this.traco.uso, this.traco.pedra, this.traco.slump, this.traco.resistenciaTipo, this.traco.mpa, this.traco.consumo
      ).then(tracoParticularidades => {
        if (tracoParticularidades) {
          this.tracoParticularidades = tracoParticularidades;
        }
      }, err => {})
      .then(() => {
        this.detectChanges();
      });
    }
  }

  bombaTipoChange(bombaTipo: CadastroGeral) {
    if (!this.bomba.bombaTipo || !bombaTipo || this.bomba.bombaTipo.codigo !== bombaTipo.codigo) {
      this.bomba.bombaTipo = bombaTipo;
      this.bomba.terceiro = null;
      this.carregaTerceiros();
      this.carregaBombaPreco();
    }
  }
  placeholderBombaTipo: string = "Tipo bomba";
  carregaBombaTipos() {
    this.bombaTipos = [];
    if (this.proposta.obra.usinaEntrega) {
      let placeholder = "Tipo bomba";
      this.placeholderBombaTipo = LOADING_MESSAGE+placeholder;
      this._bombaPrecoService.ListarBombaTiposPorUsina(this.proposta.obra.usinaEntrega, true)
      .then(bombaTipos => this.bombaTipos = bombaTipos, err => this.bombaTipos = [])
      .then(() => {
        this.placeholderBombaTipo = placeholder;
        this.detectChanges();
      });
    }
    this.carregaTerceiros();
  }

  carregaDemaisServicos() {
    this.demaisServicos = [];
    this.unidades = [];

    this._unidadeService.listarTodos()
    .then(unidades => this.unidades = unidades, err => this.unidades = [])
    .then(() => {
      this.detectChanges();
    });

    if (this.proposta.obra.usinaEntrega) {
      this._demaisServicosService.listarPorUsina(this.proposta.obra.usinaEntrega)
      .then(demaisServicos => this.demaisServicos = demaisServicos.records, err => this.demaisServicos = [])
      .then(() => {
        this.detectChanges();
      });
    }
  }

  carregaItemDemaisServicos(mercadoria: Mercadoria) {
    if (mercadoria) {
      var selected = this.demaisServicos.find(t => t.mercadoria.codigo == mercadoria.codigo && t.usina.codigo == this.proposta.obra.usinaEntrega.codigo);

      this.itemDemaisServicos.codigo = selected.codigo;
      this.itemDemaisServicos.atualizaEstoque = selected.atualizaEstoque;
      this.itemDemaisServicos.formaDeCobranca = selected.formaDeCobranca;
      this.itemDemaisServicos.frequenciaDeCobranca = selected.frequenciaDeCobranca;
      this.itemDemaisServicos.numeroDeCasasDecimais = selected.numeroDeCasasDecimais;
      this.itemDemaisServicos.precoMinimo = selected.precoMinimo;
      this.itemDemaisServicos.precoSugerido = selected.precoSugerido;
      this.itemDemaisServicos.precoProposto = selected.precoSugerido;
      this.itemDemaisServicos.unidade = selected.unidade;
      this._cdr.detectChanges();
    }
  }

  bombaPropriaChange(isBombaPropria: boolean) {
    if (this.bomba.bombaPropria !== isBombaPropria) {
      this.bomba.bombaPropria = isBombaPropria;
      this.bomba.terceiro = null;
      this.bomba.faturamentoDireto = !isBombaPropria;
      this.carregaTerceiros();
      this.carregaBombaPreco();
    }
  }

  bombaFaturamentoDiretoChange(isFaturamentoDireto: boolean) {
    if (this.bomba.faturamentoDireto !== isFaturamentoDireto) {
      this.bomba.faturamentoDireto = isFaturamentoDireto;
      this.bomba.terceiro = null;
      this.carregaTerceiros();
      this.carregaBombaPreco();
    }
  }

  terceiroChange(terceiro: Interveniente) {
    if (!this.bomba.terceiro || !terceiro || this.bomba.terceiro.codigo !== terceiro.codigo) {
      this.bomba.terceiro = terceiro;
      this.carregaBombaPreco();
    }
  }
  placeholderTerceiro: string = "Terceiro";
  carregaTerceiros() {
    this.bombistasTerceiros = [];
    if (this.proposta.obra.usinaEntrega && this.bomba.bombaTipo) {
      let placeholder = "Terceiro";
      this.placeholderTerceiro = LOADING_MESSAGE+placeholder;
      this._bombaPrecoService.ListarTerceirosPorBombaTipo(this.bomba.bombaTipo, true)
      .then(terceiros => this.bombistasTerceiros = terceiros, err => this.bombistasTerceiros = [])
      .then(() => {
        this.placeholderTerceiro = placeholder;
        this.detectChanges();
      });
    }
  }

  get bombaAtualTemNovaTabelaPreco(): boolean {
    let b = this.proposta.obra.obraBombas[this.indexBomba];

    if (!b || !this.bomba) return false;

    return this.bomba.horaPrecoTabela !== b.horaPrecoTabela
      || this.bomba.horaTabelaAte !== b.horaTabelaAte
      || this.bomba.horaTaxaMinimaPrecoTabela !== b.horaTaxaMinimaPrecoTabela
      || this.bomba.m3PrecoTabela !== b.m3PrecoTabela
      || this.bomba.m3TabelaAte !== b.m3TabelaAte
      || this.bomba.taxaMinimaPrecoTabela !== b.taxaMinimaPrecoTabela
  }

  carregaBombaPreco(apenasPrecoTabela: boolean = false) {
    if (this.bomba.bombaPropria && this.proposta.obra.usinaEntrega && this.bomba.bombaTipo) {
      this._bombaPrecoService.ObterPorUsinaBombaTipoData(this.proposta.obra.usinaEntrega, this.bomba.bombaTipo, new Date(), true)
      .then(bombaPreco => {
        if (bombaPreco) {
          this.bomba.m3TabelaAte = bombaPreco.m3Ate;
          this.bomba.taxaMinimaPrecoTabela = bombaPreco.taxaMinimaPreco;
          this.bomba.m3PrecoTabela = bombaPreco.m3Preco;

          this.bomba.tipoCalculo = bombaPreco.tipoCalculo;
          
          this.bomba.horaTabelaAte = bombaPreco.horaAte;
          this.bomba.horaTaxaMinimaPrecoTabela = bombaPreco.horaTaxaMinimaPreco;
          this.bomba.horaPrecoTabela = bombaPreco.horaPreco;

          this.bomba.horaTipoCalculo = bombaPreco.horaTipoCalculo;

          if (!apenasPrecoTabela) {
            this.bomba.m3PropostoAte = bombaPreco.m3Ate;
            this.bomba.taxaMinimaPrecoProposto = bombaPreco.taxaMinimaPreco;
            this.bomba.m3PrecoProposto = bombaPreco.m3Preco;
  
            this.bomba.horaPropostoAte = bombaPreco.horaAte;
            this.bomba.horaTaxaMinimaPrecoProposto = bombaPreco.horaTaxaMinimaPreco;
            this.bomba.horaPrecoProposto = bombaPreco.horaPreco;
          }
        }
      }, err => {})
      .then(() => {
        this.detectChanges();
      });
    } else if (!this.bomba.bombaPropria && this.bomba.terceiro && this.bomba.bombaTipo) {
      this._bombaPrecoService.ObterPorBombistaBombaTipoData(this.bomba.terceiro, this.bomba.bombaTipo, new Date())
      .then(bombaPreco => {
        if (bombaPreco) {
          this.bomba.m3TabelaAte = bombaPreco.m3Ate;
          this.bomba.taxaMinimaPrecoTabela = bombaPreco.taxaMinimaPreco;
          this.bomba.m3PrecoTabela = bombaPreco.m3Preco;
          
          this.bomba.horaTabelaAte = 0;
          this.bomba.horaTaxaMinimaPrecoTabela = 0.0;
          this.bomba.horaPrecoTabela = 0.0;

          if (!apenasPrecoTabela) {
            if (!this.bomba.faturamentoDireto) {
              this.bomba.m3PropostoAte = bombaPreco.m3Ate;
              this.bomba.taxaMinimaPrecoProposto = bombaPreco.taxaMinimaPreco;
              this.bomba.m3PrecoProposto = bombaPreco.m3Preco;
              this.bomba.tipoCalculo = bombaPreco.tipoCalculo;
            } else {
              this.bomba.m3PropostoAte = 0;
              this.bomba.taxaMinimaPrecoProposto = 0;
              this.bomba.m3PrecoProposto = 0;
            }
  
            this.bomba.horaPropostoAte = 0;
            this.bomba.horaTaxaMinimaPrecoProposto = 0;
            this.bomba.horaPrecoProposto = 0;
          }
        }
      }, err => {})
      .then(() => {
        this.detectChanges();
      });
    } else {
      if (!apenasPrecoTabela) {
        this.bomba.m3PropostoAte = 0;
        this.bomba.taxaMinimaPrecoProposto = 0;
        this.bomba.m3PrecoProposto = 0;

        this.bomba.horaPropostoAte = 0;
        this.bomba.horaTaxaMinimaPrecoProposto = 0;
        this.bomba.horaPrecoProposto = 0;
      }
    }
  }

  async validaMudancaValorBomba(stepBomba) {
    let self = PropostaPageComponent.self;
 
    if (this.proposta.obra.obraBombas.length > 0) {
      this.proposta.obra.obraBombas.forEach(bomba => {
        if (bomba.bombaPropria && this.proposta.obra.usinaEntrega && bomba.bombaTipo) {
          this._bombaPrecoService.ObterPorUsinaBombaTipoData(this.proposta.obra.usinaEntrega, bomba.bombaTipo, new Date(), true)
          .then(bombaPreco => {
            if (bombaPreco) {
              if (bomba.tipoCalculo != bombaPreco.tipoCalculo){
                self.dialog.open(AlertDialogComponent, {
                  disableClose: true,
                  data: {
                    title: 'BOMBAS',
                    message: 'Tipo de cálculo de bomba alterado! Edite e atualize o valor de bomba para recálculo do valor!'
                  }
                });
          
                stepBomba.setFocus();
              }
            }
          }, err => {})
          .then(() => {
            this.detectChanges();
          });
        } else if (!bomba.bombaPropria && bomba.terceiro && bomba.bombaTipo) {
          this._bombaPrecoService.ObterPorBombistaBombaTipoData(bomba.terceiro, bomba.bombaTipo, new Date())
          .then(bombaPreco => {
            if (bombaPreco) {
              if (bomba.tipoCalculo != bombaPreco.tipoCalculo) {
                self.dialog.open(AlertDialogComponent, {
                  disableClose: true,
                  data: {
                    title: 'BOMBAS',
                    message: 'Tipo de cálculo de bomba alterado! Edite e atualize o valor de bomba para recálculo do valor!'
                  }
                });
          
                stepBomba.setFocus();
              }
            }
          }, err => {})
          .then(() => {
            this.detectChanges();
          });
        }
      });
    }
  }

  condicaoPagamentoChange(condicaoPagamento: CondicaoPagamento) {
    if (!this.pagamento.condicaoPagamento || !condicaoPagamento
        || this.pagamento.condicaoPagamento.codigo!==condicaoPagamento.codigo) {
      this.pagamento.condicaoPagamento = condicaoPagamento;
      this.pagamento.tipoCobranca = null;
    }
  }
  placeholderCondicaoPagamento: string = "Condição de pagamento";
  async carregaCondicoesPagamento() {
    this.condicoesPagamento = [];
    if (this.proposta.obra.usinaEntrega && this.proposta.data) {
      let placeholder = "Condição de pagamento";
      this.placeholderCondicaoPagamento = LOADING_MESSAGE+placeholder;
      await this._pagamentoService.ListarCondicoesPagamentoPorUsinaDataIntervenienteTipo
        (this.proposta.obra.usinaEntrega, new Date(this.proposta.data),this.proposta.intervenienteTipo, this.proposta.segmentacao == null ? 0 : this.proposta.segmentacao, true)
      .then(condicoes => this.condicoesPagamento = condicoes, err => this.condicoesPagamento = [])
      .then(() => {
        this.placeholderCondicaoPagamento = placeholder;
        this.detectChanges();
      });
    }
    await this.carregaTiposCobranca();
  }

  async carregaSegmentacoes() {
    this.segmentos = [];
    await this._segmentacaoService.listarTodos()
    .then(segmentos => this.segmentos = segmentos, err => this.segmentos = [])
    .then(() => {
      this.detectChanges();
    })
  }

  async carregaFinalidades() {
    this.finalidades = [];

    await this._contratoService.ListarFinalidades().then(
      finalidades => { this.finalidades = finalidades; },
      error => { this.finalidades = [] }
    )
    .then(() => {
      this.detectChanges();
    })
  }

  async carregaTaxaExtras() {
    let self = PropostaPageComponent.self;

    if (self.isInsertMode && self.proposta.obra.obraTaxas.length === 0) {
      await self._obraTaxaService.listarTaxaPadraoByIdUsinaSegmento(self.proposta.obra.usinaEntrega, self.proposta.segmento.id)
      .then(taxas => {
        self.proposta.obra.obraTaxas = taxas;
      }, error => {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'Erro ao carregar as taxas extras!'
          }
        });
      })
      .then(() => {
        self.detectChanges();
      });  
    }
  }

  placeholderTipoCobranca: string = "Tipo de cobrança";
  async carregaTiposCobranca(condicaoPagamento?: CondicaoPagamento, tipoCobranca?: TipoCobranca) {
    this.tiposCobranca = [];
    if (condicaoPagamento || this.pagamento.condicaoPagamento) {
      let placeholder = "Tipo de cobrança";
      this.placeholderTipoCobranca = LOADING_MESSAGE+placeholder;
      await this._pagamentoService.ListarTiposCobrancaPorCondicaoPagamento(condicaoPagamento || this.pagamento.condicaoPagamento, true)
      .then(tiposCobranca => this.tiposCobranca = tiposCobranca, err => this.tiposCobranca = [])
      .then(() => {
        this.placeholderTipoCobranca = placeholder;
        if (tipoCobranca && this.tiposCobranca.filter(t => t.codigo === tipoCobranca.codigo).length===0)  {
          this.proposta.obra.tipoCobranca = null;
        }
        this.detectChanges();
      });
    }
  }

  changeCodigoObraPrefeitura(newValue: string) {
    var valorAnterior = this.proposta.codigoObraPrefeitura;
    this.proposta.codigoObraPrefeitura = newValue;
    if (valorAnterior !== newValue) {
      var usinaDefault = this.proposta.obra.obraTributacoesMunicipais.filter(t => t.usinaEntregaCodigo === 0);
      
      if (usinaDefault.length > 0) {
        usinaDefault.forEach(t => {
          t.codigoObraPrefeitura = newValue;
        });
      } else {
        var newUsinaDefault = new ObraTributacaoMunicipal();
        newUsinaDefault.codigoObraPrefeitura = newValue;
        this.proposta.obra.obraTributacoesMunicipais.push(newUsinaDefault);
      }
    }
  }

  changeSegmento(newSegmento: Segmentacao) {
    this.proposta.segmentacao = newSegmento.id;
    this.proposta.segmento = newSegmento;
  }
  
  async changeUsinaEntrega(newUsina: Usina) {
    var usinaAnterior = this.proposta.obra.usinaEntrega;
    this.proposta.obra.usinaEntrega = newUsina;
    if (usinaAnterior !== newUsina) {
      this.carregaObraDistanciaUsina();
      this.carregaDistanciaUsinaViaGoogleApi();
      if(this.isInsertMode === false) {
        this.dialog.open(AlertDialogComponent, {
        disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'Taxas foram alteradas para taxas padrão da usina selecionada!'
          }
        });
        this.abaObrigatoriaRevisarTaxaExtra = true;
        await this._obraTaxaService.listarTaxaPadraoByIdUsinaSegmento(this.proposta.obra.usinaEntrega, this.proposta.segmento.id)
        .then(taxas => {
          this.proposta.obra.obraTaxas = taxas;
        }, error => {
          this.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Erro ao carregar as taxas extras!'
            }
          });
        })
        .then(() => {
          this.detectChanges();
        });
      }
      
    }
    this.obraForm.controls['obraCep'].updateValueAndValidity();
    this.obraForm.controls['obraDistanciaUsina'].updateValueAndValidity();

    if(this.isInsertMode) {
      this.carregaTempoDescarga();
    }
  }
  cepAnterior: string = '';
  changeEnderecoObra(newEndereco: Endereco) {
    this.proposta.obra.endereco = newEndereco;
    if (this.cepAnterior !== newEndereco.cep && newEndereco.cep.length >= 8 && this.cepAnterior !== '') { 
      this.carregaObraDistanciaUsina();
    }
    this.cepAnterior = this.proposta.obra.endereco.cep;
  }

  disableObraDistanciaUsina: boolean = true;
  carregaObraDistanciaUsina() {
    var self = PropostaPageComponent.self;

    self._enderecoService
    .obterDistanciaKmPorUsinaCep(self.proposta.obra.usinaEntrega, self.proposta.obra.endereco.cep)
    .then(km => {
      self.proposta.obra.distanciaUsina = km;

      self.validaDistanciaKmUsinaCepAprovada(self.proposta.obra.usinaEntrega, self.proposta.obra.endereco.cep, self.proposta.obra.distanciaUsina);
    }, err => {
      self.disableObraDistanciaUsina = false;
      self.detectChanges();
    });
    
  }

  carregaTempoDescarga() {
    var self = PropostaPageComponent.self;

    self._obraService.ObterTempoDescarga(self.proposta.obra.usinaEntrega.codigo)
      .then(tempoDescarga => {
        self.proposta.obra.tempoDescarga = tempoDescarga;
        self.detectChanges();
      }, err => {
        self.proposta.obra.tempoDescarga = 0;
        self.detectChanges();
      })
  }

  carregaDistanciaUsinaViaGoogleApi() {
    var self = PropostaPageComponent.self;

    if (self.utilizaGoogleMatrixAPI === true && self.proposta.obra.usinaEntrega) {
      var enderecoObra = `${self.proposta.obra.endereco.logradouro.trim().replace(" ","+")}
       +${self.proposta.obra.endereco.numero.toString()} 
       +${self.proposta.obra.endereco.bairro.trim().replace(" ","+")}
       +${self.proposta.obra.endereco.cep.trim().replace("-","")}`

      self._enderecoService.obterDistanciaKmEntreUsinaEObraViaGoogleApi(self.proposta.obra.usinaEntrega, enderecoObra, true)
      .then(response => {
        self.proposta.obra.distanciaUsinaGoogleApi = response.distanciaEmKm !== null ? response.distanciaEmKm : 0;
        self.utilizaGoogleMatrixAPI = response.utilizaGoogleApi;
        self.detectChanges();
      }, err => {
          self.proposta.obra.distanciaUsinaGoogleApi = 0;
      });
    }

  }

  logradouroFocusIn() {
    this.enderecoLogradouroAnterior = this.proposta.obra.endereco.logradouro;
  }
  logradouroFocusOutEvent: number;
  logradouroFocusOut() {
    if (!this.logradouroFocusOutEvent || ((new Date()).getTime() - this.logradouroFocusOutEvent)>150) {
      if(this.enderecoLogradouroAnterior !== this.proposta.obra.endereco.logradouro){
        this.proposta.obra.distanciaUsinaGoogleApi = 0;
        this.carregaDistanciaUsinaViaGoogleApi();
      }
      this.logradouroFocusOutEvent = (new Date()).getTime();
    }
  }
  numeroFocusIn() {
    this.enderecoNumeroAnterior = this.proposta.obra.endereco.numero;
  }
  numeroFocusOutEvent: number;
  numeroFocusOut() {
    if (!this.numeroFocusOutEvent || ((new Date()).getTime() - this.numeroFocusOutEvent)>150) {
      if(this.enderecoNumeroAnterior !== this.proposta.obra.endereco.numero){
        this.proposta.obra.distanciaUsinaGoogleApi = 0;
        this.carregaDistanciaUsinaViaGoogleApi();
      }
      this.numeroFocusOutEvent = (new Date()).getTime();
    }
  }

  validaDistanciaKmUsinaCepAprovada(usinaEntrega: Usina, cep: string, km: number) {
    var self = PropostaPageComponent.self;

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

  get vibradorValorTotal(): number {
    return this.proposta.obra.vibradorQuantidade * this.proposta.obra.vibradorValorUnitario;
  }

  get temObraTaxaReprovada(): boolean {
    return (this.proposta.obra.obraTaxas.filter(t => t.aprovada === 'X').length > 0);
  }
  get temObraTaxaPersonalizada(): boolean {
    return (this.proposta.obra.obraTaxas.filter(t => t.isPersonalizada && t.selecionada === 'S').length > 0);
  }

  classTaxaExtra(obraTaxa: ObraTaxa): string {
    //if (obraTaxa.aprovada === 'X') return 'obra-taxa-reprovada';
    if (!this.isTaxaSelecionada(obraTaxa)) return 'obra-taxa-deselecionada';
    if (obraTaxa.isPersonalizada) return 'obra-taxa-personalizada';
    return 'obra-taxa';
  }

  disableCampoTaxaExtra(obraTaxa: ObraTaxa, idCampo: string): boolean {
    return !obraTaxa.descricaoFormula.includes(idCampo);
  }

  montaDescricaoTaxaExtra(obraTaxa: ObraTaxa): string {
    return obraTaxa.descricaoFormula
      .replace('#21', obraTaxa.prazoToleranciaDe.toFixed(0))
      .replace('#20', obraTaxa.quantidade.toFixed(0))
      .replace('#19', obraTaxa.antecedencia)
      .replace('#18', obraTaxa.acimaDe.toFixed(0))
      .replace('#17', obraTaxa.horarioAntesDas)
      .replace('#16', obraTaxa.slumpPara)
      .replace('#15', obraTaxa.slumpDe)
      .replace('#14', obraTaxa.resistenciaPara)
      .replace('#13', obraTaxa.resistenciaDe)
      .replace('#12', obraTaxa.pedraPara)
      .replace('#11', obraTaxa.pedraDe)
      .replace('#10', obraTaxa.valorPor)
      .replace('#9', Tasks.formataValor(obraTaxa.valor, 2, false))
      .replace('#8', obraTaxa.valorTipo)
      .replace('#7', obraTaxa.volume)
      .replace('#6', obraTaxa.cobrarVolume)
      .replace('#5', obraTaxa.horarioAposAs)
      .replace('#4', obraTaxa.quandoAte)
      .replace('#3', obraTaxa.quandoOperacao)
      .replace('#2', obraTaxa.quandoDe)
      .replace('#1', obraTaxa.tipo);
  }

  taxaValidadeEhPorPeriodo(obraTaxa: ObraTaxa): boolean {
    return obraTaxa.periodoDe === null || obraTaxa.periodoAte === null
      || (new Date(obraTaxa.periodoDe) > new Date('1900/01/01'))
      || (new Date(obraTaxa.periodoAte) > new Date('1900/01/01'));
  }
  taxaStatusAprovacao(obraTaxa: ObraTaxa): Status {
    
    if (obraTaxa.nova) return statusObraTaxa[EStatusObraTaxa.Nova];

    switch (obraTaxa.aprovada) {
      case 'S': return statusComercial[EStatusComercial.Aprovado];
      case 'N': return statusComercial[EStatusComercial.Aguardando];
      case 'X': return statusComercial[EStatusComercial.Reprovado];
      default: return statusComercial[EStatusComercial.NaoNecessita];
    }
  }

  taxaFoiAlterada(obraTaxa: ObraTaxa): boolean {
    let obraTaxas = this.proposta.obra.obraTaxas;
    return JSON.stringify(obraTaxa) !== JSON.stringify(obraTaxas[this.indexTaxa]);
  }

  setTaxaSelecionada(obraTaxa: ObraTaxa, value: boolean) {
    if (obraTaxa.selecionada === 'S' && !value) obraTaxa.aprovada = '';
    if (obraTaxa.selecionada !== 'S' && value) obraTaxa.aprovada = '';
    obraTaxa.selecionada = (value ? 'S' : 'N');
  }
  isTaxaSelecionada(obraTaxa: ObraTaxa): boolean {
    return (obraTaxa.selecionada === 'S');
  }

  exibirCamposVolume(obraTaxa: ObraTaxa): boolean {
    switch (obraTaxa.tipo) {
      case TaxaTipos.M3_FALTANTE:
      case TaxaTipos.M3_FALTANTE_BOMBEADO:
      case TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_PEDRAS:
      case TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_RESISTENCIA:
      case TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_SLUMP:
      case TaxaTipos.ADICIONAL_ZMRC:
      case TaxaTipos.ADICIONAL_NOTURNO:
        return true
      default:
        return false;
    }
  }
  exibirCamposHorario(obraTaxa: ObraTaxa): boolean {
    switch (obraTaxa.tipo) {
      case TaxaTipos.ADICIONAL_DOMINGOS_E_FERIADOS:
      case TaxaTipos.ADICIONAL_ZMRC:
      case TaxaTipos.ADICIONAL_NOTURNO:
        return true
      default:
        return false;
    }
  }
  exibirCamposPedra(obraTaxa: ObraTaxa): boolean {
    return (obraTaxa.tipo === TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_PEDRAS);
  }
  exibirCamposResistencia(obraTaxa: ObraTaxa): boolean {
    return (obraTaxa.tipo === TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_RESISTENCIA);
  }
  exibirCamposSlump(obraTaxa: ObraTaxa): boolean {
    return (obraTaxa.tipo === TaxaTipos.ACRECIMO_PARA_ALTERACAO_DE_SLUMP);
  }
  exibirCampoAcimaDe(obraTaxa: ObraTaxa): boolean {
    switch (obraTaxa.tipo) {
      case TaxaTipos.TAXA_PERMANENCIA_NA_OBRA:
      case TaxaTipos.ADICIONAL_KM_RODADO:
      case TaxaTipos.TAXA_PERMANENCIA_DE_BOMBA_NA_OBRA:
        return true
      default:
        return false;
    }
  }

  exibirCampoAntecedencia(obrataxa: ObraTaxa) {
    switch (obrataxa.tipo) {
      case TaxaTipos.CANCELAMENTO_DE_PROGRAMACAO:
      case TaxaTipos.CANCELAMENTO_DE_PROGRAMACAO_BOMBEADO:
        return true;
      default:
        return false;
    }
  }

  exibirCampoQuantidade(obrataxa: ObraTaxa) {
    switch (obrataxa.tipo) {
      case TaxaTipos.CANCELAMENTO_DE_PROGRAMACAO:
      case TaxaTipos.CANCELAMENTO_DE_PROGRAMACAO_BOMBEADO:
        return true;
      default:
        return false;
    }
  }

  onGeralNext() {
    if (!this.clicouVendedor && this.proposta.vendedor !== this.vendedorAtualProposta && this.vendedorAtualProposta !== null){
      this.proposta.vendedor = this.vendedorAtualProposta;
    }
    this.clicouVendedor = false;

    this.saveProgress();
    this.travarSegmentacao = true;
    if (this.proposta.cpfCnpj !== '') {
      var control = this.clienteForm.controls['cpf'] || this.clienteForm.controls['cnpj'];
      if (control) {
        control.markAsDirty();
        control.markAsTouched();
        control.updateValueAndValidity()
      };
    }
  }
  onVendedorNext() { this.saveProgress(); }
  onClienteNext() {
    if (!this.proposta.obra.nome) {
      const nomeInterveniente = this.proposta.intervenienteRazao || this.proposta.intervenienteNome;
      this.proposta.obra.nome = nomeInterveniente ? nomeInterveniente.substring(0, 30) : nomeInterveniente;
    }
    if (this.proposta.obra.contatoPrincipalNome === '') this.proposta.obra.contatoPrincipalNome = this.proposta.contato;
    if (this.proposta.obra.contatoPrincipalTelefoneDdd === 0) this.proposta.obra.contatoPrincipalTelefoneDdd = this.proposta.telefoneDdd;
    if (this.proposta.obra.contatoPrincipalTelefoneNumero === 0) this.proposta.obra.contatoPrincipalTelefoneNumero = this.proposta.telefoneNumero;
    if (this.proposta.obra.contatoPrincipalCelularDdd === 0) this.proposta.obra.contatoPrincipalCelularDdd = this.proposta.celularDdd;
    if (this.proposta.obra.contatoPrincipalCelularNumero === 0) this.proposta.obra.contatoPrincipalCelularNumero = this.proposta.celularNumero;
    this.saveProgress();
  }
  onResponsavelSolidarioNext() { this.saveProgress(); }
  async onObraNext(stepObra) {
    
    if (this.proposta.obra.endereco.cep.length===8) {
      await this._enderecoService.obterValorAdicionalM3PorUsinaCep(this.proposta.obra.usinaEntrega, this.proposta.obra.endereco.cep)
      .then(
        result => {
          this._valorAdicionalM3PorUsinaCep = result;
        },
        error => {
          this._valorAdicionalM3PorUsinaCep = 0;
          if (error.errors)
            Tasks.setValidationErrors(error.errors, this.errorMessages, 'obraCep', this.obraForm);
          stepObra.setFocus();
        }
      );
    }

    if (this.proposta.obra.distanciaUsina > 0) {
      await this._usinaService.obterValorAdicionalM3PorUsinaKm(this.proposta.obra.usinaEntrega, this.proposta.obra.distanciaUsina)
      .then(
        result => {
          this._valorAdicionalM3PorUsinaKm = result;
        },
        error => {
          this._valorAdicionalM3PorUsinaKm = 0;
          if (error.errors)
            Tasks.setValidationErrors(error.errors, this.errorMessages, 'obraDistanciaUsina', this.obraForm);
          stepObra.setFocus();
        }
      );
    }

    var tempoAteObra = this.proposta.obra.tempoAteAObra == 0 ? this.proposta.obra.usinaEntrega.tempoBtAteAObra : this.proposta.obra.tempoAteAObra;
    var tempoBtNaObra = this.proposta.obra.tempoBtNaObra == 0 ? this.proposta.obra.usinaEntrega.tempoBtNaObra : this.proposta.obra.tempoBtNaObra;

    this.proposta.obra.tempoCicloPrevisto = (this.proposta.obra.usinaEntrega.prazoPesagem + 
      tempoBtNaObra + tempoAteObra + 
      (tempoAteObra * (this.proposta.obra.usinaEntrega.porcentagemRetornoObra/100)));


    await this.carregaCondicoesPagamento();
    await this.carregaTaxaExtras();
    this.usaAdicionalZmrcChange();

    this.saveProgress();
  }
  async onConcretoNext() {
    this.proposta.obra.obraBombas.forEach(async bomba => {
      if (bomba.bombaPropria) {
        await this._bombaPrecoService
          .ObterValorAdicional(this.proposta.obra.usinaEntrega, bomba.bombaTipo, bomba.distanciaTubulacao)
          .then(valor => bomba.valorAdicionalTubulacao = valor)
          .then(() => this.saveProgress());
      } else {
        bomba.valorAdicionalTubulacao = 0;
        this.saveProgress();
      }
    });
    this.saveProgress();
  }
  onBombaNext(stepBomba) {
    this.saveProgress();

    this.validaMudancaValorBomba(stepBomba);
  }

  async onDemaisServicosNext() {
    this.saveProgress();

    let self = PropostaPageComponent.self;

    if (self.isInsertMode && self.proposta.obra.obraTaxas.length === 0) {
        await self._obraTaxaService.listarTaxaPadraoByIdUsinaSegmento(self.proposta.obra.usinaEntrega, self.proposta.segmento.id)
        .then(taxas => {
          self.proposta.obra.obraTaxas = taxas;
        }, error => {
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Erro ao carregar as taxas extras!'
            }
          });
        })
        .then(() => {
          self.detectChanges();
        });
    }
  }
  onTaxasExtrasNext() {
    this.saveProgress();

    let self = PropostaPageComponent.self;

    var temBomba = self.proposta.obra.obraBombas.length > 0;

    var valorM3Faltante = self._obraTaxaService
      .obterValorM3Faltante(temBomba, this.volumeTotalTracos(), self.proposta.obra.volumePorCarga, self.proposta.obra.obraTaxas);

    var valorAdicionalPorKmRodado = self._obraTaxaService
      .obterValorAdicionalPorKmRodado(self.proposta.obra.distanciaUsina, this.volumeTotalTracos(), self.proposta.obra.volumePorCarga, self.proposta.obra.obraTaxas,temBomba);
    
    //self.proposta.valorExtras = valorM3Faltante + self.vibradorValorTotal + valorAdicionalPorKmRodado + self.valorTotalDemaisServicos();
    self.proposta.valorExtras = valorM3Faltante + valorAdicionalPorKmRodado;

    var valorTotal = self.valorTotalProposta();
    
    if (!self.proposta.obra.obraPagamentos || self.proposta.obra.obraPagamentos.length === 0) {
      self.proposta.obra.obraPagamentos = [];
      var pagamento = new Pagamento();
      pagamento.sequencia = 1;
      pagamento.condicaoPagamento = self.proposta.obra.condicaoPagamento;
      pagamento.tipoCobranca = self.proposta.obra.tipoCobranca;
      pagamento.valor = valorTotal;
      self.proposta.obra.obraPagamentos.push(pagamento);

    } else if (self.proposta.obra.obraPagamentos.length === 1) {
      var pagamento = self.proposta.obra.obraPagamentos.filter(t => t.ativoSimNao==='S')[0];
      if (pagamento == undefined)  self.proposta.obra.obraPagamentos[0];

      if (self.proposta.obra.condicaoPagamento.codigo !== pagamento.condicaoPagamento.codigo && pagamento.valorApropriado > 0 && pagamento.idAprovacao === '') {
        pagamento.ativoSimNao='N';

        var novoPagamento = new Pagamento();
        novoPagamento.sequencia = this.getFreeSequence(self.proposta.obra.obraPagamentos.map(t => t.sequencia), false, 99);
        novoPagamento.condicaoPagamento = self.proposta.obra.condicaoPagamento;
        novoPagamento.tipoCobranca = self.proposta.obra.tipoCobranca;
        novoPagamento.valor = pagamento.valor;
        novoPagamento.ativoSimNao='S';
        self.proposta.obra.obraPagamentos.push(novoPagamento);
      } else if (pagamento.idAprovacao === '' && !self.isBoletoImpresso(pagamento) && (!pagamento.detalhes || pagamento.detalhes.length === 0)) {
        pagamento.condicaoPagamento = self.proposta.obra.condicaoPagamento;
        pagamento.tipoCobranca = self.proposta.obra.tipoCobranca;
        pagamento.valor = valorTotal;
      } else if (pagamento.condicaoPagamento.codigo !== self.proposta.obra.condicaoPagamento.codigo || pagamento.tipoCobranca.codigo !== self.proposta.obra.tipoCobranca.codigo) {

        var mensagem = `Alteração de condição de pagamento e/ou tipo de cobrança realizada na aba`;
        mensagem += ` de concreto não pode ser aplicada ao pagamento informado na aba de pagamentos pois:`;
        var separador = '';

        if (pagamento.idAprovacao !== '') {
          mensagem += `${separador} PAGAMENTO JÁ APROVADO (ID:'${pagamento.idAprovacao}')`;
          separador = ' |';
        }
        if (self.isBoletoImpresso(pagamento)) {
          mensagem += `${separador} BOLETO JÁ IMPRESSO`;
          separador = ' |';
        }
        if (pagamento.detalhes && pagamento.detalhes.length > 0 && pagamento.necessitaAprovacao) {
          mensagem += `${separador} EXISTE(M) DETALHAMENTO(S) INFORMADO(S)`;
          separador = ' |';
        }

        if (separador !== '') {
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'PROBLEMA NA ALTERAÇÃO DE PAGAMENTO',
              message: mensagem
            }
          });
        }
        
      }
    } else {
      var pagamentosIguaisAoPrincipal = self.proposta.obra.obraPagamentos
        .filter(t => t.condicaoPagamento.codigo === self.proposta.obra.condicaoPagamento.codigo
          && t.tipoCobranca.codigo === self.proposta.obra.tipoCobranca.codigo);
      
      if (pagamentosIguaisAoPrincipal.length === 0) {
        var mensagem = `Alteração de condição de pagamento e/ou tipo de cobrança realizada na aba`;
        mensagem += ` de concreto não pode ser aplicada na aba de pagamentos pois há mais de um`;
        mensagem += ` pagamento informado! A ALTERAÇÃO DEVE SER REFEITA NESTA ABA!`;

        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'PROBLEMA NA ALTERAÇÃO DE PAGAMENTO',
            message: mensagem
          }
        });
      }
    }
    
    self.detectChanges();
  }

  taxaAtualZmrc(taxa: ObraTaxa): boolean{
    return taxa.tipo === TaxaTipos.ADICIONAL_ZMRC
     // && this.proposta.obra.usaAdicionalZMRC === "N"
     // && taxa.selecionada === "N"
  }

  taxaAtualZmrcENaoUsaZMRC(taxa: ObraTaxa): boolean{
    return taxa.tipo === TaxaTipos.ADICIONAL_ZMRC
     // && this.proposta.obra.usaAdicionalZMRC === "S"
      && taxa.selecionada === "N"
  }

  usaAdicionalZmrcChange() {
    this.proposta.obra.obraTaxas.map(t => {
      if(t.tipo === TaxaTipos.ADICIONAL_ZMRC){
        t.selecionada = this.proposta.obra.usaAdicionalZMRC;
      }
    });
  }

  onClientePrevious() { this.saveProgress(); }
  onResponsavelSolidarioPrevious() { this.saveProgress(); }
  onObraPrevious() { this.saveProgress(); }
  onConcretoPrevious() { this.saveProgress(); }
  onBombaPrevious() { this.saveProgress(); }
  onDemaisServicosPrevious() { this.saveProgress(); }
  onTaxasExtrasPrevious() { this.saveProgress(); }
  onPagamentoPrevious() { this.saveProgress(); }

  inconsistenciaTracos: { sequencia: number, mensagem: string }[] = [];
  solicitaAprovacaoTracos: { obraTraco: ObraTraco, mensagem: string }[] = [];
  private get _solicitaAprovacaoTracos(): { obraTraco: ObraTraco, mensagem: string }[] {
    var result: { obraTraco: ObraTraco, mensagem: string }[] = [];
    this.inconsistenciaTracos.forEach(i => {
      result.push({
        obraTraco: this.proposta.obra.obraTracos.filter(t => t.sequencia === i.sequencia)[0],
        mensagem: i.mensagem
      });
    });
    return result;
  }

  inconsistenciaBombas: { sequencia: number, mensagem: string }[] = [];
  solicitaAprovacaoBombas: { obraBomba: ObraBomba, mensagem: string }[] = [];
  private get _solicitaAprovacaoBombas(): { obraBomba: ObraBomba, mensagem: string }[] {
    var result: { obraBomba: ObraBomba, mensagem: string }[] = [];
    this.inconsistenciaBombas.forEach(i => {
      result.push({
        obraBomba: this.proposta.obra.obraBombas.filter(t => t.sequencia === i.sequencia)[0],
        mensagem: i.mensagem
      });
    });
    return result;
  }

  outrasInconsistencia: {key: string, message: string }[] = [];

  isJustificativaTracoRequired(traco: ObraTraco): boolean {
    let controlPrecoProposto = this.solicitaAprovacoesForm.controls[`tracoPrecoUnitarioProposto-${traco.sequencia}`];
    
    if (!controlPrecoProposto) return false;
    
    return !controlPrecoProposto.dirty;
  }
  isJustificativaBombaRequired(bomba: ObraBomba): boolean {
    let controlTaxaMinima = this.solicitaAprovacoesForm.controls[`bombaPrecoPropostoTaxaMinima-${bomba.sequencia}`];
    let controlM3ate = this.solicitaAprovacoesForm.controls[`bombaPrecoPropostoM3ate-${bomba.sequencia}`];
    let controlPorM3 = this.solicitaAprovacoesForm.controls[`bombaPrecoPropostoPorM3-${bomba.sequencia}`];
    
    if (this.utilizaCobrancaPorM3(bomba) && (!controlTaxaMinima || !controlM3ate || !controlPorM3)) return false;

    let controlHoraTaxaMinima = this.solicitaAprovacoesForm.controls[`bombaPrecoPropostoHoraTaxaMinima-${bomba.sequencia}`];
    let controlHoraAte = this.solicitaAprovacoesForm.controls[`bombaPrecoPropostoHoraAte-${bomba.sequencia}`];
    let controlPorHora = this.solicitaAprovacoesForm.controls[`bombaPrecoPropostoPorHora-${bomba.sequencia}`];
    
    if (this.utilizaCobrancaPorHora(bomba) && (!controlHoraTaxaMinima || !controlHoraAte || !controlPorHora)) return false;

    var required = (this.utilizaCobrancaPorM3(bomba) || this.utilizaCobrancaPorHora(bomba));

    if (this.utilizaCobrancaPorM3(bomba)){
      required = (required && !controlTaxaMinima.dirty && !controlM3ate.dirty && !controlPorM3.dirty);
    }

    if (this.utilizaCobrancaPorHora(bomba)){
      required = (required && !controlHoraTaxaMinima.dirty && !controlHoraAte.dirty && !controlPorHora.dirty);
    }

    return required;
  }
  hasContratoReajuste(): boolean{
    if (!this.proposta) return false;
    if (!this.proposta.obra) return false;
    if (!this.proposta.obra.contrato) return false;
    if (!this.proposta.obra.contrato.contratoTracoReajustes) return false;

    return this.contratoTracoReajustesFiltered().length > 0;
  }
  contratoTracoReajustesFiltered(): ContratoTracoReajuste[]{
    if (!this.proposta.obra.contrato) return [];

    return this.proposta.obra.contrato.contratoTracoReajustes.filter(t => t.dataConfirmacao).sort((a, b) => {
      if (a.obraTracoSequencia < b.obraTracoSequencia) return -1;
      if (a.obraTracoSequencia > b.obraTracoSequencia) return 1;
      return 0;
    });
  }
  hasContratoBombaReajuste(): boolean{ 
    if (!this.proposta) return false;
    if (!this.proposta.obra) return false;
    if (!this.proposta.obra.contrato) return false;
    if (!this.proposta.obra.contrato.contratoBombaReajustes) return false;
    
    return this.proposta.obra.contrato.contratoBombaReajustes.length > 0;
  }
  contratoBombaReajusteUltimasVigencias(bomba: ObraBomba): ContratoBombaReajuste[] {
    if (!this.proposta.obra.contrato.contratoBombaReajustes) return null;
    
    var bombasFiltradas = this.proposta.obra.contrato.contratoBombaReajustes
      .filter((t => t.obraBombaReajusteSequencia === bomba.sequencia));

    var reajustesVigentes = bombasFiltradas.filter(t => new Date(t.dataVigencia) <= Tasks.dataAtual());
    reajustesVigentes = reajustesVigentes
      .filter(t => (new Date(t.dataVigencia)).valueOf() === Math.max(...reajustesVigentes.map(t => (new Date(t.dataVigencia)).valueOf())));
    return reajustesVigentes;

  }
  async onComplete() {
    let self = PropostaPageComponent.self;

    self.proposta.origem = "proposta";

    if (self.proposta.tempoAprovacaoMedicao) {
      if (self.proposta.tempoAprovacaoMedicaoCadastro.codigo != 0) {
        if (self.proposta.aprovacaoMedicao === 'S')
          self.proposta.tempoAprovacaoMedicao = Number(self.proposta.tempoAprovacaoMedicaoCadastro.descricao);
        else
          self.proposta.tempoAprovacaoMedicao = 0;
      }
    }
    
    let request = self.isInsertMode ?
      self._propostaService.Adicionar(self.proposta) :
      self._propostaService.Atualizar(self.proposta);

    await request.then(response => {
      
      var message = self.isInsertMode ? `Proposta inserida com sucesso!` : response;
      
      self.clearSavedProgress();

      if (self.isInsertMode) {
        self.proposta.usina = new Usina();
        self.proposta.usina.codigo = response.usinaCodigo;
        self.proposta.ano = response.ano;
        self.proposta.numero = response.numero;
      }

       if (this.possuiIntegracaoCartao) {
         self._obraService.AprovarAutomaticamentePagamentosDaCieloLio(self.proposta.usina.codigo, self.proposta.ano, self.proposta.numero);
      }
     

      let router = self._router;
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: message,
          afterCloseCallback: async () => {
            if (self.proposta.statusProposta === EStatusProposta.Aprovado
                && self.proposta.obra.numContrato === 0) {
              await self._contratoService.GerarContrato(self.proposta)
              .then(contrato => {
              }, error => {
              })
              .then(() => {
                router.navigateByUrl("pages/comercial/proposta/lista");
              });
            } else {
              router.navigateByUrl("pages/comercial/proposta/lista");
            }
          }
        }
      });
    }, error => {
      if (error && error.error){
        if (error.error.startsWith("Tipo de cálculo")){
          return;
        } else if (error.error.startsWith("E-mail")){
          self.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: "E-mail do cliente é obrigatório!"
            }
          });
        }
        return;
      }

      if (error && error.errors) {
        self.inconsistenciaTracos = [];
        self.inconsistenciaBombas = [];
        self.outrasInconsistencia = [];

        error.errors.forEach(e => {
          if (e.key.startsWith("obraTraco") && e.key.endsWith("Desconto")) {
            let sequencia = parseInt(e.key.split("-")[1]);
            self.inconsistenciaTracos.push({ sequencia: sequencia, mensagem: e.message });
          } else if (e.key.startsWith("obraBomba") && e.key.endsWith("Desconto")) {
            let sequencia = parseInt(e.key.split("-")[1]);
            self.inconsistenciaBombas.push({ sequencia: sequencia, mensagem: e.message });
          } else {
            self.outrasInconsistencia.push(e);
          }
        });
        self.solicitaAprovacaoTracos = self._solicitaAprovacaoTracos;
        self.solicitaAprovacaoBombas = self._solicitaAprovacaoBombas;
        
        self.openSolicitaAprovacoesModal();
        self.detectChanges();
      } else {
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'OCORREU UM ERRO!'
          }
        });
      }
    });
  }

  goBack() {
    this.location.back();
  }

  verificaTracoJaIncluso() {
    let self = PropostaPageComponent.self;  
    
    self.tracoJaIncluso = false
    
    self.proposta.obra.obraTracos.forEach(traco => {
      if (
        self.traco.uso && traco.uso.codigo == self.traco.uso.codigo &&
        self.traco.pedra && traco.pedra.codigo == self.traco.pedra.codigo &&
        self.traco.slumpNominal && traco.slumpNominal.codigo == self.traco.slumpNominal.codigo &&
        self.traco.resistenciaTipo && traco.resistenciaTipo.codigo == self.traco.resistenciaTipo.codigo &&
        traco.consumo == self.traco.consumo &&
        traco.mpa == self.traco.mpa && traco.sequencia != self.traco.sequencia
      ) {
        self.tracoJaIncluso = true;

        this.detectChanges();
        self.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'A inclusão deste traço não é permitida, pois o mesmo já se encontra na proposta atual.'
          }           
        });

        return;
      }
    });
  };

  get formatarGrupoEconomico(): string {
    return this.proposta.interveniente.grupoEconomico.codigo + "-" + this.proposta.interveniente.grupoEconomico.descricao;
  }

  obraFrenteExcluirConfirmacao(obraFrente: ObraFrente) {

    var self = PropostaPageComponent.self;
    self.cadObraFrenteSelecionado = obraFrente;

    self.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'TopConWeb',
        message: 'Tem certeza que deseja excluir á frente de obra ' + obraFrente.enderecoNome + ' ?' ,
        confirmCallback: () => {
          this.obraFrenteExcluirSelecionada();
        }
      }
    });

  }

  obraFrenteExcluirSelecionada() {

    if(!this.isInsertMode && this.cadObraFrenteSelecionado.obraSequencia > 0) {
      this._obraService.VerificarObraFrentePossuiProgramacao(this.proposta.obra, this.cadObraFrenteSelecionado.obraSequencia)
      .then(emUso => {
        if(!emUso) {
          this.proposta.obra.obraFrentes = this.proposta.obra.obraFrentes.filter(x => x.enderecoNome != this.cadObraFrenteSelecionado.enderecoNome);
          this.proposta.obra.obraFrentes = [... this.proposta.obra.obraFrentes];
          
          this.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: 'Deletado com sucesso !'
            }
          });

        } else {
          this.dialog.open(AlertDialogComponent, {
            disableClose: true,
            data: {
              title: 'TopConWeb',
              message: `Frente de obra possuí programação(ões) cadastrada(s) !`
            }
          });
        }
      },
      error => {
        this.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'OCORREU UM ERRO!'
          }
        });
      })
  	  
    } else {
      this.proposta.obra.obraFrentes = this.proposta.obra.obraFrentes.filter(x => x.enderecoNome != this.cadObraFrenteSelecionado.enderecoNome);
      this.proposta.obra.obraFrentes = [... this.proposta.obra.obraFrentes];

      this.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: 'Deletado com sucesso !'
        }
      });
    }

  }

  /* --- Modal Obra Frente --- */

  showModalObraFrente(content, obraFrente: ObraFrente = undefined) {
    
    if(obraFrente) {

      this.cadObraFrente = {
        enderecoNome: obraFrente.enderecoNome,
        enderecoLogradouro: obraFrente.enderecoLogradouro,
        enderecoNumero: obraFrente.enderecoNumero,
        enderecoComplemento: obraFrente.enderecoComplemento,
        enderecoBairro: obraFrente.enderecoBairro,
        enderecoCep: obraFrente.enderecoCep
      };

      this.cadObraFrenteNovo = false;
      this.cadObraFrenteSelecionado = obraFrente;
    } else {
      this.cadObraFrenteSelecionado = new ObraFrente();
      this.cadObraFrente = this.cadObraFrenteSelecionado;
      this.cadObraFrenteNovo = true;
    }

    this.obraFrenteForm = this._formBuilder.group({});

    this.showModal(content, this.obraFrenteModalSalvar, this.obraFrenteModalCancelar)

  }

  obraFrenteModalSalvar() {
    
    this.cadObraFrenteSelecionado.enderecoNome = this.cadObraFrente.enderecoNome;
    this.cadObraFrenteSelecionado.enderecoLogradouro = this.cadObraFrente.enderecoLogradouro;
    this.cadObraFrenteSelecionado.enderecoNumero = this.cadObraFrente.enderecoNumero;
    this.cadObraFrenteSelecionado.enderecoComplemento = this.cadObraFrente.enderecoComplemento;
    this.cadObraFrenteSelecionado.enderecoBairro = this.cadObraFrente.enderecoBairro;
    this.cadObraFrenteSelecionado.enderecoCep = this.cadObraFrente.enderecoCep;

    if(this.cadObraFrenteNovo) {
      this.proposta.obra.obraFrentes.push(this.cadObraFrenteSelecionado);
      this.proposta.obra.obraFrentes = [... this.proposta.obra.obraFrentes];
    }

    this.closeModalGeneric();
    this.detectChanges();

  }

  obraFrenteModalCancelar() {
    this.closeModalGeneric()
  }

  private _initialized: boolean = false;
  private _cepAnterior: string = '';
  private _cepInexistente: boolean = false;
  cepMunicipioDifereProposta: boolean = false;

  maskCEP = [/\d/, /\d/, /\d/, /\d/, /\d/,'-', /\d/, /\d/, /\d/];

  private _cepErrorMessagesDefault = {key: 'minLength', message: 'cep deve conter 8 dígitos!'};
  private _cepErrorMessages: {key: string, message: string}[] = [this._cepErrorMessagesDefault];
  @Input() set cepErrorMessages(value: {key: string, message: string}[]) {
              if (!value) value = [];
              value.push(this._cepErrorMessagesDefault);
              this._cepErrorMessages = value;
           };
           get cepErrorMessages() { return this._cepErrorMessages; };

  cepChange(cep: string) {

    if (!this._initialized) {
      this._cepAnterior = cep;
      this._initialized = true;
      return;
    }
    
    if (cep.length===8 && this._cepFocused && this._cepAnterior !== cep) {
      this._cepAnterior = cep;
      this._enderecoService.obterPorCep(cep)
      .then(endereco => {

        if (endereco) {

          this._cepInexistente = false;

          if(endereco.municipio.codigo != this.proposta.obra.endereco.municipio.codigo) {
            this.cepMunicipioDifereProposta = true;
          } else {
            this.cepMunicipioDifereProposta = false;

            if(endereco.logradouro != "")
              this.cadObraFrente.enderecoLogradouro = endereco.logradouro;

            if(endereco.bairro != "")
              this.cadObraFrente.enderecoBairro = endereco.bairro;

            if(endereco.numero != 0)
              this.cadObraFrente.enderecoNumero = endereco.numero;

            if(endereco.complemento != "")
              this.cadObraFrente.enderecoComplemento = endereco.complemento;
          }

        } else {

          this._cepInexistente = true;
          this.cepMunicipioDifereProposta = false;

        }

      },
      erro => {
        this._cepInexistente = true;
        this.dialog.open(AlertDialogComponent, {
          disableClose: true,
          data: {
            title: 'TopConWeb',
            message: 'OCORREU UM ERRO!'
          }
        });
      });
    } else {
      this._cepAnterior = cep;
    }
  }

  private _cepFocused: boolean = false;

  onFocus() {
    this._cepFocused = true;
  }

  onFocusout() {
    this._cepFocused = false;
  }

  get cepInexistenteValidator(): ICustomValidator {
    var _self = this;

    return {
      key: 'cepInexistente',
      message: 'CEP inválido!',
      validatorFunction: (): boolean => {
        return _self.cadObraFrente.enderecoCep.length === 8 && _self._cepInexistente;
      },
      params: []
    }
  }

  get getEnderecoNumeroComplementoValidator(): ICustomValidator {
    var _self = this;

    return {
      key: 'endNumeroComplementoVazio',
      message: 'Necessário informar Número ou Complemento',
      validatorFunction: (): boolean => {
        return _self.cadObraFrente.enderecoComplemento.trim().length === 0 && _self.cadObraFrente.enderecoNumero === 0;
      },
      params: []
    }
  }

  get getEnderecoNomeValidator(): ICustomValidator {
    var _self = this;

    return {
      key: 'enderecoNomeDuplicate',
      message: 'Nome abreviado já cadastrado',
      validatorFunction: (): boolean => {
        return _self.cadObraFrente.enderecoNome.length > 0 
              && _self.proposta.obra.obraFrentes.filter(x => x.enderecoNome === _self.cadObraFrente.enderecoNome).length > 0
              && _self.cadObraFrenteNovo;
      },
      params: []
    }
  }


  // --------------- Modal ---------------------------------------------------------------------------------------

  confirmModal: Function;
  cancelModal: Function;

  showModal(content, confirmCallback: Function, cancelCallback?: Function) {

    let self = PropostaPageComponent.self;

    self.confirmModal = confirmCallback;
    self.cancelModal = cancelCallback || self.closeModal;

    self.openModal(content);

    self.modalIsOpen = true;

  }

  openModal(content) {
    this._dialogRef = this.dialog.open(content, { viewContainerRef: this.ModalRef });
    this.modalIsOpen = true;
  }


  closeModalGeneric() {
    let self = PropostaPageComponent.self;
    
    if (self._dialogRef) self._dialogRef.close();

    self.dialog.closeAll();

    self.modalIsOpen = false;
  }

  get utilizaVersaoContrato(): boolean{
    let self = PropostaPageComponent.self;
    return Object.values(self.versionamentoContratoParametro).some(valor => valor === true);
  }


}
