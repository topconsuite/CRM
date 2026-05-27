import { Usina } from './../usina/usina';
import { Uso } from './../traco/uso';
import { Pedra } from './../traco/pedra';
import { Slump } from './../traco/slump';
import { ResistenciaTipo } from './../traco/resistencia-tipo';
import { EObraTracoStatus } from '../traco/traco-status';

export class ObraTraco {

  usina: Usina;
  obraCodigo: number = 0;
  propostaAno: number = 0;
  propostaNumero: number = 0;
  sequencia: number = 0;
  uso: Uso;
  pedra: Pedra;
  slump: Slump;
  slumpNominal: Slump;
  resistenciaTipo: ResistenciaTipo;
  mpa: number = 0.0;
  consumo: number = 0;
  pecaConcretar: string = '';
  m3Quantidade: number = 0.0;
  m3PrecoProposto: number = 0.0;
  m3PrecoTabela: number = 0.0;
  m3PrecoAjustado: number = 0.0;
  m3Consumido: number = 0.0;
  fck: number = 0;

  descontoPercentual: number = 0.0;

  precoConcorrencia: number = 0.0;

  valorServico: number = 0.0;
  custoProjetadoTransporte: number = 0.0;

  custoServicoReajustado: number = 0.0;
  custoServicoAnterior: number = 0.0;
  custoTraco: number = 0.0;

  justificativa: string = '';
  aprovacaoTipo: string = '';
  aprovacaoVerbal: string = '';
  aprovacaoObservacao: string = '';
  aprovacaoOperacao: string = '';
  aprovacaoCiente: string = '';
  descontoSolicitante: string = '';
  precoReajustadoAnterior: number = 0.0;
  dataUltimoReajuste: Date;
  precoReajustadoAtual: number = 0.0;
  Observacao: string = '';
  descricaoPersonalizada: string = '';
  m3QuantidadeBombeada: number = 0.00;
  ebitda: number = 0;
  margemPosTransporte: number = 0;
  totalImpostos: number = 0;
  custoBombagem: number = 0;
  impostoAplicadoFederal: number = 0;
  impostoAplicadoEstadual: number = 0;
  issDedutivel: number = 0;

  ativo: string = 'S';
  inativo: boolean = false;  
  numeracaoProduto: number = 0;

  status: EObraTracoStatus = 0;
}