export class ObraTaxa {
    usinaCodigo: number = 0;
    obraCodigo: number = 0;
    sequencia: number = 0;
    descricao: string = '';
    isPersonalizada: boolean = false;
    selecionada: string = '';
    aprovacaoSolicitante: string = '';
    aprovacaoUsuario: string = '';
    aprovacaoCiente: string = '';
    aprovada: string = '';
    tipo: string = '';
    dataInicioVigencia: Date;
    periodoDe: Date;
    periodoAte: Date;
    descricaoFormula: string = '';
    tipoPessoa: string = '';
    valorTipo: string = '';
    valor: number = 0.0;
    valorPor: string = '';
    obraMunicipioCodigo: number = 0;
    quandoDe: string = '';
    quandoOperacao: string = '';
    quandoAte: string = '';
    horarioAntesDas: string = '';
    horarioAposAs: string = ''
    cobrarVolume: string = '';
    volume: string = '';
    pedraDe: string = '';
    pedraPara: string = '';
    resistenciaDe: string = '';
    resistenciaPara: string = '';
    slumpDe: string = '';
    slumpPara: string = '';
    acimaDe: number = 0;
    antecedencia: string = '';
    quantidade: number = 0;
    idCadastro: string = '';
    idAtualizacao: string = '';
    nova: boolean = false;
    prazoToleranciaDe: number = 0;
}

export const TaxaTipos = {
    M3_FALTANTE_BOMBEADO: 'M3 FALTANTE BOMBEADO',
    ADICIONAL_DOMINGOS_E_FERIADOS: 'ADICIONAL DOMINGOS E FERIANDOS',
    ADICIONAL_DOMINGOS_E_FERIADOS_BOMBEADO: 'ADICIONAL DOMINGOS E FERIADOS BOMBEADO',
    M3_FALTANTE: 'M3 FALTANTE',
    ADICIONAL_NOTURNO: 'ADICIONAL NOTURNO',
    ADICIONAL_NOTURNO_BOMBEADO: 'ADICIONAL NOTURNO BOMBEADO',
    ACRECIMO_PARA_ALTERACAO_DE_PEDRAS: 'ACRECIMO PARA ALTERAÇÃO DE PEDRAS',
    ACRECIMO_PARA_ALTERACAO_DE_RESISTENCIA: 'ACRECIMO PARA ALTERAÇÃO DE RESISTENCIA',
    ACRECIMO_PARA_ALTERACAO_DE_SLUMP: 'ACRECIMO PARA ALTERAÇÃO DE SLUMP',
    TAXA_PERMANENCIA_NA_OBRA: 'TAXA PERMANÊNCIA NA OBRA',
    TAXA_PERMANENCIA_DE_BOMBA_NA_OBRA: 'TAXA PERMANÊNCIA DE BOMBA NA OBRA',
    ADICIONAL_KM_RODADO: 'ADICIONAL KM RODADO',
    ADICIONAL_RETORNO_CONCRETO: 'ADICIONAL RETORNO CONCRETO',
    CANCELAMENTO_DE_PROGRAMACAO: 'CANCELAMENTO DE PROGRAMAÇÃO',
    CANCELAMENTO_DE_PROGRAMACAO_BOMBEADO: 'CANCELAMENTO DE PROGRAMAÇÃO BOMBEADO',
    ADICIONAL_ZMRC: 'ZONA DE MÁXIMA RESTRIÇÃO DE CIRCULAÇÃO'
};