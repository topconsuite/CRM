import { Usina } from './../usina/usina';
import { CadastroGeral } from './../cadastro-geral/cadastro-geral';
import { Interveniente } from './../interveniente/interveniente';
import { EBombaM3CalculoTipo, EBombaHoraCalculoTipo } from '../bomba/bomba.classes';

export class ObraBomba {
  usinaCodigo: number = 0;
  obraCodigo: number = 0;
  propostaAno: number = 0;
  propostaNumero: number = 0;
  sequencia: number = 0;
  bombaTipo: CadastroGeral;
  bombaPropria: boolean = true;
  terceiro: Interveniente;
  faturamentoDireto: boolean = false;
  alugadaPeloCliente: boolean = false;
  
  taxaMinimaPrecoTabela: number = 0.0;
  m3TabelaAte: number = 0;
  m3PrecoTabela: number = 0.0;

  taxaMinimaPrecoProposto: number = 0.0;
  m3PropostoAte: number = 0.0;
  m3PrecoProposto: number = 0.0;

  taxaMinimaReajustadaAnterior: number = 0.0;
  m3ReajustadoAteAnterior: number = 0.0;
  m3PrecoReajustadoAnterior: number = 0.0;

  taxaMinimaReajustadaAtual: number = 0.0;
  m3ReajustadoAteAtual: number = 0.0;
  m3PrecoReajustadoAtual: number = 0.0;

  dataUltimoReajuste: Date;

  descontoPercentual: number = 0.0;

  distanciaTubulacao: number = 0;
  valorAdicionalTubulacao: number = 0;

  justificativa: string = '';
  aprovacaoVerbal: string = '';
  descontoSolicitante: string = '';

  aprovacaoObservacao: string = '';
  aprovacaoOperacao: string = '';
  aprovacaoCiente: string = '';
  statusAprovacao: number = 0;

  tipoCalculo: EBombaM3CalculoTipo = EBombaM3CalculoTipo.taxaMinimaOuExcedente;

  horaTaxaMinimaPrecoTabela: number = 0.0;
  horaTabelaAte: number = 0;
  horaPrecoTabela: number = 0.0;

  horaTaxaMinimaPrecoProposto: number = 0.0;
  horaPropostoAte: number = 0.0;
  horaPrecoProposto: number = 0.0;

  horaDescontoPercentual: number = 0.0;

  horaTipoCalculo: EBombaHoraCalculoTipo = EBombaHoraCalculoTipo.semCobranca;

  totalImpostos: number = 0;
  ebitda: number = 0;

  ativo: string = 'S';
  inativo: boolean = false;

}