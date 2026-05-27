import { CadastroGeral } from '../cadastro-geral/cadastro-geral';
import { Portador } from '../banco/portador';

export class TipoCobranca extends CadastroGeral {
    forma: string = '';
    portador: Portador;
    fixo: string = '';
    aprovacao: string = '';
}