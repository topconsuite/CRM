import { Component, OnInit, ViewChild, ViewContainerRef, ChangeDetectionStrategy,
  OnChanges, SimpleChanges, ChangeDetectorRef, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { Observable } from 'rxjs';

// **** CLASSES ****
import { Tasks } from 'app/classes/_tasks/tasks';
import { CadastroDiverso } from 'app/classes/cadastro-geral/cadastro-diverso';
import { Usina, ParametroProgramacao } from 'app/classes/usina/usina';
import { Uso, Pedra, Slump, ResistenciaTipo, ETipoVinculoMpaConsumo } from 'app/classes/traco/traco.classes';
import { Interveniente, IntervenienteTipo, intervenienteTipos } from 'app/classes/interveniente/interveniente';
import { Obra, ObraBomba, ObraTraco } from 'app/classes/obra/obra.classes';
import { Proposta, EStatusProposta, Status, statusProposta, statusComercial, statusContrato } from 'app/classes/proposta/proposta.classes';
import { Programacao, EProgramacaoConfirmacao, confirmacaoOpcoes, EProgramacaoStatus, EProgramacaoTipoAlteracao, ProgramacaoDemaisServicos, corpoDeProvaMoldadores, corpoDeProvaTipos, corpoDeProvaMoldagemRemota, } from 'app/classes/programacao/programacao.classes';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { ObraDemaisServicos } from 'app/classes/obra/obra-demais-servicos';
import { EFrequenciaDeCobranca, EFormaDeCobrancaDemaisServicos } from 'app/classes/demais-servicos/demais-servicos';
// ******************************************************************

// **** SERVICES ***
import { UserService } from 'app/services/user.service';
import { CadastroDiversoService } from 'app/services/cadastro-diverso.service';
import { PropostaService } from 'app/services/proposta.service';
import { ProgramacaoService } from 'app/services/programacao.service';
import { UsinaService } from 'app/services/usina.service';
import { VendedorService } from 'app/services/vendedor.service';
import { PecaAConcretarService } from 'app/services/peca-a-concretar.service';
import { ParametroService } from 'app/services/parametro.service';
// ******************************************************************

import { ICustomValidator } from '../../../../components/interfaces/custom-validator';
import { AlertDialogComponent } from '../../../../components/dialog/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from '../../../../components/dialog/confirm-dialog/confirm-dialog.component';
import { replace } from 'lodash';
import { Console } from 'console';
import { ObraService } from 'app/services/obra.service';
import { EBombaM3CalculoTipo } from 'app/classes/bomba/bomba-preco';
import { ObraFrente } from 'app/classes/obra/obra-frente';
import { Endereco } from 'app/classes/endereco/endereco';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-programacao-page',
  templateUrl: './programacao-page.component.html',
  styleUrls: ['./programacao-page.component.scss']
})
export class ProgramacaoPageComponent implements OnInit, OnChanges {

  public static self: ProgramacaoPageComponent;

  readonly VOLUME_POR_CARGA_DEFAULT: number = 8.0;

  geralForm: FormGroup;
  programacaoForm: FormGroup;
  tracoForm: FormGroup;
  bombaForm: FormGroup;
  demaisServicosForm: FormGroup;

  demaisServicosShowContent: boolean = false;

  listarUsinasPorEmpresa: boolean = false;

  volumeTotalConsumido: number = 0;

  analisarLimiteDeCreditoNaProgramacao = true;
  permiteVolumeAcimaContratado = '1';

  listaObraFrentes: ObraFrente[] = [];
  obraFrentePrincipal: ObraFrente = new ObraFrente();
  obraFrenteSelecionada: ObraFrente = new ObraFrente();

  saldoObra : number = 0;

  utilizaCalculoPrecoTabelaPorUsina: boolean = false;

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

  get aConfirmarOpcoes(): EProgramacaoConfirmacao[] {
    let codigos: EProgramacaoConfirmacao[] = [];
    confirmacaoOpcoes.forEach(opcao => {
      codigos.push(opcao.codigo);
    })
    return codigos;
  }


  usinas: Usina[] = [];
  pecasConcretar: string[] = [];
  vendedores: Vendedor[] = [];
  parametroProgramacao: ParametroProgramacao = new ParametroProgramacao();
  andares: string[] = [];
  quantidadeCorpoDeProva: number[] = [];
  
  get intervalos(): number[] {
    var _intervalos: number[] = [];
    if (this.parametroProgramacao.intervaloEmMinutosEntreCargas === 0) return [0];
    var intervalo = this.parametroProgramacao.intervaloEmMinutosEntreCargas;
    for (let i = 1; i < 25; i++) {
      _intervalos.push(i * intervalo);
    }
    return _intervalos;
  };

  proposta: Proposta = new Proposta();
  programacao: Programacao = new Programacao();
  updateMode: EProgramacaoTipoAlteracao = EProgramacaoTipoAlteracao.Insert;

  get tracoSelecionado(): ObraTraco {
    if(!this.programacao || !this.programacao.obraTracoSequencia || !this.proposta
      || !this.proposta.obra || this.proposta.obra.obraTracos.length === 0) return new ObraTraco();
    
    var result = this.proposta.obra.obraTracos.filter(t => t.sequencia === this.programacao.obraTracoSequencia);
    if (result.length === 1) return result[0];
    
    return new ObraTraco();
  };

  get bombaSelecionada(): ObraBomba {
    if(!this.programacao || !this.programacao.obraBombaSequencia || !this.proposta
      || !this.proposta.obra || this.proposta.obra.obraBombas.length === 0) return new ObraBomba();
    
    var result = this.proposta.obra.obraBombas.filter(t => t.sequencia === this.programacao.obraBombaSequencia);
    if (result.length === 1) return result[0];
    
    return new ObraBomba();
  };

  private _dialogRef: MatDialogRef<any>;
  temDireitoAlteracao: boolean = true;
  temDireitoAlterarUsina: boolean = false;
  podeAlterarDataProgramacao: boolean = true;
  temDireitoColetaCp: boolean = false;

  constructor(
      private _formBuilder: FormBuilder,
      public dialog: MatDialog,
      private _cdr:ChangeDetectorRef,
      private _userService: UserService,
      private _propostaService: PropostaService,
      private _programacaoService: ProgramacaoService,
      private _usinaService: UsinaService,
      private _cadastroDiversoService: CadastroDiversoService,
      private _vendedorService: VendedorService,
      private _pecaAConcretarService: PecaAConcretarService,
      private _route: ActivatedRoute,
      private _router: Router,
      private _obraService: ObraService,
      private _parametroService: ParametroService,
      private location: Location
    ) {
    ProgramacaoPageComponent.self = this;

    if (this._route.routeConfig.path.endsWith('/nova') || this._route.routeConfig.path.endsWith('/clonar')) {
      var temDireitoInclusao = this._userService.temDireitoAplicativo('WEB6201','I', 200);
      if (!temDireitoInclusao) return;
    } else {
      this.temDireitoAlteracao = this._userService.temDireitoAplicativo('WEB6201','A');
    }

    this.temDireitoAlterarUsina = this._userService.temDireitoAplicativo('CON6207', '');
    this._userService.gravarAcessoAplicacao("Comercial", 6201);

    this.temDireitoColetaCp = this._userService.temDireitoAplicativo('CON6147', '');

    _pecaAConcretarService.listarTodos().then(
      pecasAConcretar => { this.pecasConcretar = pecasAConcretar },
      error => { this.pecasConcretar = [] }
    );
    _cadastroDiversoService.listarAndares().then(
      andares => { this.andares = andares.map(t => t.codigo) },
      error => { this.andares = [] }
    );
    _cadastroDiversoService.listarQuantidadeDeCorposDeProva().then(
      quantidadeCorpoDeProva => { this.quantidadeCorpoDeProva = quantidadeCorpoDeProva.map(t => parseInt(t.codigo)) },
      error => { this.quantidadeCorpoDeProva = [] } 
    );

    _vendedorService.listarAtivos().then(
      vendedores => { this.vendedores = vendedores },
      error => { this.vendedores = [] }
    );

    _parametroService.obterParametoN("TopCon","EmpresaUsinaObraContratoDeveSerIgualAEmpresaUsinaEntregaProgramacao").then(
      parametro => { this.listarUsinasPorEmpresa = parametro==="1"},
      error => {this.listarUsinasPorEmpresa = false}
    );

    _parametroService.obterParametoN("TopCon","AnalizaLimCredProd").then(
      parametro => { this.analisarLimiteDeCreditoNaProgramacao = parametro==="1"},
      error => {this.analisarLimiteDeCreditoNaProgramacao = true}
    );

    _parametroService.obterParametoN("TopCon","PermiteVolumeAcimaContratado").then(
      parametro => { this.permiteVolumeAcimaContratado = parametro[0]},
      error => {this.permiteVolumeAcimaContratado = '1'}
    );

    _parametroService.obterParametoN("web", "UtilizaCalculoPrecoTabelaPorUsina").then(
      parametro => { this.utilizaCalculoPrecoTabelaPorUsina= parametro === "1" },
      error => { this.utilizaCalculoPrecoTabelaPorUsina = false }
    );

    if (this._route.routeConfig.path.endsWith('/clonar')) this.updateMode = EProgramacaoTipoAlteracao.Copy;
  }

  ngOnInit() {
    this.geralForm = this._formBuilder.group({});
    this.programacaoForm = this._formBuilder.group({});
    this.tracoForm = this._formBuilder.group({});
    this.bombaForm = this._formBuilder.group({});
    this.demaisServicosForm = this._formBuilder.group({});

    let idUsina = parseFloat(this._route.snapshot.paramMap.get('idUsina'));
    let propostaAno = parseFloat(this._route.snapshot.paramMap.get('propostaAno'));
    let propostaNumero = parseFloat(this._route.snapshot.paramMap.get('propostaNumero'));
    let sequencia = parseFloat(this._route.snapshot.paramMap.get('sequencia'));
    if (idUsina && propostaAno && propostaNumero) {
      this._propostaService.ObterPorUsinaAnoNumero({codigo: idUsina, nome: '', sigla: '', filialCodigo: 0, tempoBtAteAObra:0 ,tempoBtNaObra:0, porcentagemRetornoObra:0 ,prazoPesagem:0}, propostaAno, propostaNumero).then(
        proposta => {
          this.proposta = proposta;

          this.proposta.obra.obraTracos.forEach(traco => {
            this._obraService.obterConsumoTracoPorContrato(this.proposta.obra, traco).then(volumeConsumido => {
              traco.m3Consumido = volumeConsumido;
              this.volumeTotalConsumido += volumeConsumido;
            });
          });
          
          let empresaUsinaEntregaObra = Math.floor(proposta.obra.usinaEntrega.filialCodigo / 1000)
          
          if (this.listarUsinasPorEmpresa) {
            this._usinaService.listarPorEmpresa(empresaUsinaEntregaObra).then(
              usinas => { this.usinas = usinas },
              error => { this.usinas = [] }
            );
          }else{
            this._usinaService.listarListarUsinasPermitidasUsuario().then(
              usinas => { this.usinas = usinas },
              error => { this.usinas = [] }
            );
          }

          this.obraFrentePrincipal.enderecoNome = "ENDEREÇO PRINCIPAL DA OBRA";
          this.obraFrentePrincipal.usinaCodigo = proposta.usina.codigo;
          this.obraFrentePrincipal.obraCodigo = proposta.obra.numero;
          this.obraFrentePrincipal.enderecoLogradouro =  proposta.obra.endereco.logradouro;
          this.obraFrentePrincipal.enderecoNumero = proposta.obra.endereco.numero;
          this.obraFrentePrincipal.enderecoComplemento = proposta.obra.endereco.complemento;
          this.obraFrentePrincipal.enderecoBairro = proposta.obra.endereco.bairro;
          this.obraFrentePrincipal.enderecoCep = proposta.obra.endereco.cep;
          this.listaObraFrentes.push(this.obraFrentePrincipal);
          this.listaObraFrentes = this.listaObraFrentes.concat(this.proposta.obra.obraFrentes);
          this.obraFrenteSelecionada = this.obraFrentePrincipal;
          
          if (!sequencia) {
            this.programacao = new Programacao();
            this.programacao.usina = proposta.usina;
            this.programacao.propostaAno = proposta.ano;
            this.programacao.propostaNumero = proposta.numero;
            this.programacao.contratoAno = proposta.obra.anoContrato;
            this.programacao.contratoNumero = proposta.obra.numContrato;
            this.programacao.usinaEntrega = proposta.obra.usinaEntrega;
            this.programacao.obraNumero = proposta.obra.numero;
            this.programacao.obraNome = proposta.obra.nome;
            this.programacao.endereco = proposta.obra.endereco;
            this.programacao.vibradorQuantidade = proposta.obra.vibradorQuantidade;
            this.programacao.vibradorValorUnitario = proposta.obra.vibradorValorUnitario;
            this.programacao.vibradorValorTotal = this.vibradorValorTotal;

            this.programacao.tempoAteAObra = proposta.obra.tempoAteAObra;
            this.programacao.tempoBtNaObra = proposta.obra.tempoBtNaObra;
            this.programacao.tempoDescarga = proposta.obra.tempoDescarga;

            this.carregaParametroProgramacao();
          } else {
            this._programacaoService.ObterPorId(proposta.usina, proposta.obra.numero, sequencia).then(
              programacao => {
                this.programacao = programacao;
                if (programacao.obraNome === '') this.programacao.obraNome = proposta.obra.nome;
                this._cdr.detectChanges();
                this.carregaParametroProgramacao();

                
                if(this.updateMode == EProgramacaoTipoAlteracao.Copy) {
                  this.programacao.sequencia = 0;
                  this.programacao.dataConcretagem = new Date();
                  this.programacao.temNotaFicalEmitida = false;
                  this.programacao.status = EProgramacaoStatus.AguardandoConfirmacao;
                } else {
                  this.updateMode = EProgramacaoTipoAlteracao.Update;
                  
                  let dataAtual = Tasks.dataAtual();

                  if(new Date(this.programacao.dataConcretagem) <= dataAtual) {

                    this.podeAlterarDataProgramacao = false;

                    this.dialog.open(AlertDialogComponent, {
                      data: {
                        title: 'TopConWeb',
                        message: 'Não será permitido alterar a data da programação para programações do dia e anteriores a data atual!'
                      }
                    });

                  }
                  
                }

              },
              error => {
                this.dialog.open(AlertDialogComponent, {
                  data: {
                    title: 'TopConWeb',
                    message: 'HOUVE UM ERRO!'
                  }
                });
              }
            );
          }
        },
        error => {
          this.dialog.open(AlertDialogComponent, {
            data: {
              title: 'TopConWeb',
              message: 'HOUVE UM ERRO!'
            }
          });
        }
      );
      
    } else {
      this.proposta = new Proposta();
      this.programacao = new Programacao();
      this._cdr.detectChanges();
    }

  }

  ngOnChanges(changes: SimpleChanges) {
    this._cdr.detectChanges();
  }

  carregaParametroProgramacao() {
    if (this.programacao && this.programacao.usinaEntrega) {
      this._usinaService.obterParametroProgramacao(this.programacao.usinaEntrega).then(
        param => { this.parametroProgramacao = param },
        error => { this.parametroProgramacao = new ParametroProgramacao() }
      ).then(() => this._cdr.detectChanges());
    } else {
      this.parametroProgramacao = new ParametroProgramacao();
      this._cdr.detectChanges();
    }
  }

  maskHora = [/\d/, /\d/, ':', /\d/, /\d/];

  getSequencia = (model: ObraTraco | ObraBomba): number => model ? model.sequencia : 0;

  getDisabled = (model: ObraTraco | ObraBomba): boolean => model ? model.ativo === 'N' : false;

  obraTracoSequenciaChange(newSequencia: number) {
    if (newSequencia !== this.programacao.obraTracoSequencia) {
      this.programacao.obraTracoSequencia = newSequencia;
      this.programacao.uso = this.tracoSelecionado.uso;
      this.programacao.tracoPesadoUso = null;
      this.programacao.pedra = this.tracoSelecionado.pedra;
      this.programacao.tracoPesadoPedra = null;
      this.programacao.slump = this.tracoSelecionado.slump;
      this.programacao.slumpNotaFiscal = this.tracoSelecionado.slumpNominal.codigo.toString();
      this.programacao.tracoPesadoSlump = null;
      this.programacao.resistenciaTipo = this.tracoSelecionado.resistenciaTipo;
      this.programacao.tracoPesadoResistenciaTipo = null;
      this.programacao.mpa = this.tracoSelecionado.mpa;
      this.programacao.tracoPesadoMpa = 0;
      this.programacao.consumo = this.tracoSelecionado.consumo;
      this.programacao.tracoPesadoConsumo = 0;
      this.programacao.pecaConcretar = this.tracoSelecionado.pecaConcretar;
      this.programacao.volumePorCarga = this.proposta.obra.volumePorCarga || this.VOLUME_POR_CARGA_DEFAULT;
      this.saldoObra = (this.volumeTotalTracosContratado()-this.volumeTotalTracosConsumido());
    }
  }

  obraBombaSequenciaChange(newSequencia: number) {
    if (newSequencia !== this.programacao.obraBombaSequencia) {
      this.programacao.obraBombaSequencia = newSequencia;
      this.programacao.distanciaTubulacao = this.bombaSelecionada.distanciaTubulacao;
    }
  }

  necessitaConfirmacaoChange(newValue: EProgramacaoConfirmacao) {
    if (newValue !== this.programacao.necessitaConfirmacao) {
      this.programacao.necessitaConfirmacao = newValue;
      this.programacao.volumeLiberado = 0;
    }
  }

  corpoDeProvaTipoChange(newValue: string) {
    if (newValue === 'Nenhum') {
      this.programacao.corpoDeProvaQuantidade = 0;
      this.programacao.corpoDeProvaMoldador = 0;
      this.programacao.corpoDeProvaIntervalo = 0;
      this.programacao.corpoDeProvaMoldagemRemota = '';
    } else if(newValue === 'Total') {
      this.programacao.corpoDeProvaIntervalo = 0;
    }
    
    if(newValue !== 'Nenhum' && this.programacao.corpoDeProvaMoldador === 0) {
      this.programacao.corpoDeProvaMoldador = 1;
    }
    if(newValue !== 'Nenhum' && this.programacao.corpoDeProvaMoldagemRemota === '') {
      this.programacao.corpoDeProvaMoldagemRemota = 'Padrao da Central';
    }
  }

  quantidadeItemDemaisServicos(item: ObraDemaisServicos): number {
    let demaisServicos = this.programacao.demaisServicos || [];
    var selected = demaisServicos.find(t => t.sequencia === item.sequencia);

    if (!selected) return 0;
    return selected.quantidade;
  }
  valorItemDemaisServicos(item: ObraDemaisServicos): number {
    return item.precoProposto * this.quantidadeItemDemaisServicos(item);
  }
  valorTotalDemaisServicos(): number {
    var self = ProgramacaoPageComponent.self;
    let total: number = 0;
    self.proposta.obra.obraDemaisServicos.forEach(item => {
      total += self.valorItemDemaisServicos(item);
    });

    return total + self.vibradorValorTotal;
  }
  
  get corpoDeProvaMoldadores(): number[] {
    let codigos: number[] = [];
    corpoDeProvaMoldadores.forEach(intervTipo => {
      codigos.push(intervTipo.codigo);
    })
    return codigos;
  };

  get corpoDeProvaTipos(): string[] {
    let codigos: string[] = [];
    corpoDeProvaTipos.forEach(intervTipo => {
      codigos.push(intervTipo.descricao);
    })
    return codigos;
  };

  
  get corpoDeProvaMoldagemRemota(): string[] {
    let codigos: string[] = [];
    corpoDeProvaMoldagemRemota.forEach(intervTipo => {
      codigos.push(intervTipo.descricao);
    })
    return codigos;
  };

  get andar(): string {
    if (!this.programacao.andar) this.programacao.andar = '';
    return this.programacao.andar;
  }
  set andar(value: string) {
    if (!value) this.programacao.andar = '';
    else this.programacao.andar = value;
  }

  usinaEntregaChange(newUsina: Usina) {
    if (newUsina !== this.programacao.usinaEntrega) {
      this.programacao.usinaEntrega = newUsina;
      this.programacao.intervaloEmMinutosEntreCargas = 0;
      this.carregaParametroProgramacao();
    }
  }

  get vibradorValorTotal(): number {
    this.programacao.vibradorValorTotal = this.programacao.vibradorQuantidade * this.programacao.vibradorValorUnitario;
    return this.programacao.vibradorValorTotal;
  }

  get horaValidator(): ICustomValidator {
    var _self = ProgramacaoPageComponent.self;
    var _tasks = Tasks;

    var message = 'Informe um horário válido';

    return {
      key: 'horarioInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        return !_tasks.horarioValido(hora);
      },
      params: [_self.programacao.horarioBomba]
    }
  }

  get horaIntervaloValidator(): ICustomValidator {
    var _self = ProgramacaoPageComponent.self;
    var _intervalo = _self.parametroProgramacao.intervaloEmMinutosEntreCargas;
    var _tasks = Tasks;

    var message = 'Informe um horário válido';
    if (_intervalo > 0) message += ` e múltiplo de ${_intervalo} minutos`;

    return {
      key: 'horarioInvalido',
      message: message,
      validatorFunction: (hora: string, intervalo: number): boolean => {
        return !(_tasks.horarioValido(hora) && _tasks.horarioIntervaloValido(hora, intervalo));
      },
      params: [_self.programacao.horario, _intervalo]
    }
  }

  get volumeValidator(): ICustomValidator {
    var _self = ProgramacaoPageComponent.self;

    switch (this.permiteVolumeAcimaContratado) {      
      case '1':
        var message = 'Volume programado maior que volume contratado para o traço!';
        return {
          
          key: 'volumeInvalido',
          message: message,
          validatorFunction: (volumeContratado: number, volumeProgramado: number): boolean => {
            return volumeContratado < volumeProgramado;
          },
          params: [(_self.tracoSelecionado.m3Quantidade - _self.tracoSelecionado.m3Consumido), _self.programacao.volumeTotal]
        }
      case '2':
        return;
      case '3':
        var message = 'Volume programado maior que volume contratado!';
        return {
          
          key: 'volumeInvalido',
          message: message,
          validatorFunction: (volumeContratado: number, volumeProgramado: number): boolean => {
            return volumeContratado < volumeProgramado;
          },
          params: [_self.saldoObra, _self.programacao.volumeTotal]
        }  
    }
  }
  

  vendedorFormatter = (model: Vendedor): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  usoFormatter = (model: Uso): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  pedraFormatter = (model: Pedra): string => model ? model.descricao.toUpperCase() : '';
  slumpFormatter = (model: Slump): string => model ? model.descricao.toUpperCase() : '';
  resistenciaTipoFormatter = (model: ResistenciaTipo): string => model ? model.descricao.toUpperCase() : '';
  mpaFormatter = (model): string => model ? this.formataValor(model, 1, false) : '';
  consumoFormatter = (model): string => model ? model.toString() : '';
  pecaConcretarFormatter = (model): string => model ? model.toString() : '';
  obraFrenteFormatter = (model: ObraFrente): string => {
    if (model === null || model === undefined || isNaN(model.obraSequencia)) return '';
    const obrasFrenteFiltradas = this.listaObraFrentes.filter(e => e.obraSequencia === model.obraSequencia);    
    if (obrasFrenteFiltradas.length > 0) {
        return obrasFrenteFiltradas[0].enderecoNome.toUpperCase();
    }
    return '';
  };
  intervenienteFormatter = (model: Interveniente): string => model ? (model.codigo+' - '+(model.razao || model.nome)).toUpperCase() : '';
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
  aConfirmarFormatter = (model: EProgramacaoConfirmacao): string => {
    if (model===null || model===undefined) return '';
    return this.aConfirmarOpcoes.includes(model) ? confirmacaoOpcoes.filter(e => e.codigo===model)[0].descricao.toUpperCase() : '';
  }
  corpoDeProvaMoldadoresFormatter = (model: number): string => {
    if (!model) return '';
    return this.corpoDeProvaMoldadores.includes(model) ? corpoDeProvaMoldadores.filter(e => e.codigo===model)[0].descricao : '';
  };
  corpoDeProvaMoldagemRemotaFormatter = (model: string): string => {
    if (!model) return '';
    return this.corpoDeProvaMoldagemRemota.includes(model) ? corpoDeProvaMoldagemRemota.filter(e => e.descricao===model)[0].descricao : '';
  };

  frequenciaCobrancaFormatter = (item: EFrequenciaDeCobranca) => {
    //console.log(`${EFrequenciaDeCobranca[item]}`);
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
  formaDeCobrancaFormatter = (item: EFormaDeCobrancaDemaisServicos) => {
    //console.log(`${EFormaDeCobrancaDemaisServicos[item]}`);
    switch (item) {
      case EFormaDeCobrancaDemaisServicos.FinalConcretagem:
        return 'Final Concretagem'.toUpperCase();
      case EFormaDeCobrancaDemaisServicos.NaRemessa:
        return 'Na Remessa'.toUpperCase();
      default:
        return '';
    }
  }

  getFormControlNameDemaisServicosSelected(item: ObraDemaisServicos): string {
    let name = `selected-${item.sequencia}`;
    let previous = this.demaisServicosForm.controls[name];
    if (!previous) this.demaisServicosForm.addControl(name, new FormControl());
    return name;
  }
  getItemDemaisServicos(item: ObraDemaisServicos): ProgramacaoDemaisServicos {
    return this.programacao.demaisServicos.find(t => t.sequencia === item.sequencia) || new ProgramacaoDemaisServicos();
  }
  getItemDemaisServicosSelected(item: ObraDemaisServicos): boolean {
    return this.getItemDemaisServicos(item).sequencia > 0;
  }
  setItemDemaisServicosSelected(item: ObraDemaisServicos, selected: boolean): void {
    //if (this.getItemDemaisServicosSelected(item) === selected) return;

    let previous = this.programacao.demaisServicos.find(t => t.sequencia === item.sequencia);

    if (!previous && selected) {
      let newItem = new ProgramacaoDemaisServicos();
      newItem.usinaCodigo = this.proposta.obra.usinaCodigo;
      newItem.obraNumero = this.proposta.obra.numero;
      newItem.programacaoSequencia = this.programacao.sequencia;
      newItem.sequencia = item.sequencia;
      this.programacao.demaisServicos.push(newItem);
    } else if (previous && !selected) {
      this.programacao.demaisServicos = this.programacao.demaisServicos
        .filter(t => t.sequencia !== previous.sequencia);
    }

    this._cdr.detectChanges();
  }

  numeroPropostaString(): string {
    return (this.proposta.usina ? this.proposta.usina.codigo : '0')+'/'+this.proposta.numero+'-'+this.proposta.ano;
  }

  enderecoObraString(): string {
    return Tasks.ederecoToString(this.programacao.endereco);
  }

  isStatusPropostaAprovado(): boolean {
    return this.proposta.statusProposta===EStatusProposta.Aprovado;
  }

  isVolumeLiberadoParcial(): boolean {
    return this.programacao.necessitaConfirmacao === EProgramacaoConfirmacao.Parcial;
  }

  isDisabled(desconsideraData: boolean = false): boolean {
    let dataAtual = Tasks.dataAtual();
    let isDataFutura = new Date(this.programacao.dataConcretagem || dataAtual) >= dataAtual;
    let temVolumeEntregue = this.programacao.volumeEntregue !== 0;
    let dataValida = isDataFutura || desconsideraData;

    return !(this.temDireitoAlteracao && dataValida && !temVolumeEntregue && !this.programacao.temNotaFicalEmitida);
  }

  tracoString(traco: ObraTraco): string {
    let self = ProgramacaoPageComponent.self;
    if (!traco.resistenciaTipo || !traco.pedra || !traco.slumpNominal || !traco.uso) return '';
    //if (traco.descricaoPersonalizada !== '') return traco.descricaoPersonalizada;
    let mpaConsumo = self.isVinculoMpa(traco.resistenciaTipo) ? traco.mpa : (self.isVinculoConsumo(traco.resistenciaTipo) ? traco.consumo : '');
    let tracoDescr = '' + traco.uso.codigo + " " + traco.uso.descricao
    + ' - ' + traco.pedra.descricao
    + ' - ' + traco.slumpNominal.descricao
    + ' - ' + traco.resistenciaTipo.abreviatura + ' ' + mpaConsumo;

    if(traco.inativo) {
      tracoDescr = tracoDescr + ' - INATIVO';
    }

    return tracoDescr + " | " + traco.m3Quantidade.toFixed(1) + " m3 / " + traco.m3Consumido.toFixed(1) + " m3 / " + (traco.m3Quantidade - traco.m3Consumido).toFixed(1) + " m3";
  }

  isUndefined(tracos: ObraTraco[]) {
    let isUndefined = false;
    tracos.forEach(traco => {
      isUndefined = traco.m3Consumido === undefined;
    });

    return isUndefined;
  }

 
  bombaString(bomba: ObraBomba): string {
    var self = ProgramacaoPageComponent.self;
    if (!bomba.bombaTipo) return self.intervenienteFormatter(bomba.terceiro) || 'BOMBA DE TERCEIRO';
    if (bomba.inativo) return bomba.bombaTipo.descricao + ' - INATIVO';
    return bomba.bombaTipo.descricao;
  }

  isVinculoMpa(resistenciaTipo: ResistenciaTipo): boolean {
    if (!resistenciaTipo) return false;
    return resistenciaTipo.vinculo === ETipoVinculoMpaConsumo.MPA;
  }
  isVinculoConsumo(resistenciaTipo: ResistenciaTipo): boolean {
    if (!resistenciaTipo) return false;
    return resistenciaTipo.vinculo === ETipoVinculoMpaConsumo.CONSUMO;
  }

  formataValor = Tasks.formataValor;
  formataMoeda = Tasks.formataMoeda;

  onGeralNext() {  }
  onProgramacaoNext() {  }
  onTracoNext() {  }
  onBombaNext() {  }

  onProgramacaoPrevious() {  }
  onTracoPrevious() {  }
  onBombaPrevious() {  }
  onDemaisServicosPrevious() {}

  onDemaisServicosStepSelected() {
    this.demaisServicosShowContent = true;
  }

  async onComplete() {
    let self = ProgramacaoPageComponent.self;

    if (self.isDisabled()) return;

    if (self.proposta.obra.obraTracos.length > 0 && !self.programacao.obraTracoSequencia) {
      self.dialog.open(ConfirmDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Não foi selecionado nenhum traço.
            Confirma programação apenas de bomba?`,
          confirmCallback: async () => {
            await self.salvarProgramacao();
          }
        }
      });
    } else if (self.proposta.obra.obraBombas.length > 0 && !self.programacao.obraBombaSequencia) {
      self.dialog.open(ConfirmDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Existe(m) bomba(s) na proposta, mas não foi selecionada bomba nesta programação.
            Confirma programação sem bomba?`,
          confirmCallback: async () => {
            await self.salvarProgramacao();
          }
        }
      });
    } else {
      await self.salvarProgramacao();
    }

  }

  goBack() {
    this.location.back();
  }

  volumeTotalTracosContratado(): number {
    let total: number = 0;
    this.proposta.obra.obraTracos.forEach(item => {
      total += item.m3Quantidade;
    });

    return total;
  }

  volumeTotalTracosConsumido(): number {
    return this.volumeTotalConsumido;
  }

  async salvarProgramacao() {
    let self = ProgramacaoPageComponent.self;

    let request = self.updateMode == EProgramacaoTipoAlteracao.Insert || self.updateMode == EProgramacaoTipoAlteracao.Copy ?
      self._programacaoService.Adicionar(self.programacao) :
      self._programacaoService.Atualizar(self.programacao);

    await request.then(response => {
      let router = self._router;
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: response,
          afterCloseCallback: () => {
            //router.navigateByUrl("pages/comercial/proposta/lista");
            this.goBack(); 
          }
        }
      });
    }, error => {
      self.dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: Tasks.formataErrosApi(error)
        }
      });
    });
  }

  carregaFrenteObra(obraFrente: ObraFrente) {
    if(this.listaObraFrentes.filter(e => e.obraSequencia===obraFrente.obraSequencia).length = 0) return;

    var obraFrente = this.listaObraFrentes.filter(e => e.obraSequencia===obraFrente.obraSequencia)[0];
    this.programacao.endereco.logradouro = obraFrente.enderecoLogradouro;
    this.programacao.endereco.numero = obraFrente.enderecoNumero;
    this.programacao.endereco.complemento = obraFrente.enderecoComplemento;
    this.programacao.endereco.bairro = obraFrente.enderecoBairro;
    this.programacao.endereco.cep = obraFrente.enderecoCep;
    this.programacao.obraFrenteSequencia = obraFrente.obraSequencia;
    this.obraFrenteSelecionada = obraFrente;
  }
}
