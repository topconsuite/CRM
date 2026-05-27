import { EStatusComercial } from '../proposta/proposta';
import { EStatusProjecao, EObraDemaisStatusComercial } from './obra';
import { EStatusCadastro, EStatusEngenharia, EStatusFinanceiro } from './obra.classes';

export enum ESituacaoAprovacaoComercialAlcadaUsuario
{

    NaoUtiliza = 0,
    AguardandoAprovacao = 1,
    AguardandoAprovacaoOutroNivel = 2

}

export class ObraConsulta {
    usina: number = 0;
    contratoAno: number = 0;
    contratoNumero: number = 0;
    propostaAno: number = 0;
    propostaNumero: number = 0;
    vendedorCodigo: number = 0;
    vendedorNome: string = '';
    clienteCodigo: number = 0;
    clienteRazao: string = '';
    clienteCpfCnpj: string = '';
    clienteTelefoneDdd: number = 0;
    clienteTelefoneNumero: number = 0;
    clienteCelularDdd: number = 0;
    clienteCelularNumero: number = 0;
    clienteTelefoneComercialDdd: number = 0;
    clienteTelefoneComercialNumero: number = 0;
    clienteLimiteValor: number = 0;
    clienteLimiteData: Date;
    clienteSaldoContasAReceber: number = 0;
    clienteValorTotalNfsNaoFaturadas: number = 0;
    valorProgramado: number = 0;
    limiteCreditoDisponivel: number = 0;
    limiteCreditoSaldo: number = 0;
    limiteCreditoAnalise: string = '';
    contratoData: Date;
    volumeTotal: number = 0;
    status: number = 0;
    statusDescricao: string = '';
    analistaCodigo: number = 0;
    analistaNomeReduzido: string = '';
    contratoValorTotal: number = 0;
    usinaEntrega: number = 0;
    usinaEntregaSigla: string = '';
    tipoCobrancaCodigo: number = 0;
    tipoCobrancaDescricao: string = '';
    obraNumero: number = 0;
    obraNome: string = '';
    obraContato: string = '';
    obraMunicipio: string = '';
    dataConcretagem: Date;
    horario: string = '';
    statusComercial: EStatusComercial = EStatusComercial.NaoNecessita;
    statusCadastro: EStatusCadastro = EStatusCadastro.PreCadastro;
    statusEngenharia: EStatusEngenharia = EStatusEngenharia.NaoNecessita;
    statusFinanceiro: EStatusFinanceiro = EStatusFinanceiro.NaoNecessita;
    volumeStatusComercial: EObraDemaisStatusComercial = EObraDemaisStatusComercial.NaoNecessita;
    condicaoPagamentoCodigo: number = 0;
    condicaoPagamentoDescricao: string = '';
    statusAprovComAlcada: ESituacaoAprovacaoComercialAlcadaUsuario = ESituacaoAprovacaoComercialAlcadaUsuario.NaoUtiliza;    
    statusProjecao: EStatusProjecao = EStatusProjecao.NaoPossui;
    grupoEconomicoCodigo: number = 0;
    grupoEconomicoDescricao: string = '';
}