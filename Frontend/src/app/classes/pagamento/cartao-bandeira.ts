import { CadastroGeral } from '../cadastro-geral/cadastro-geral';
import { Portador } from '../banco/portador';
import { Interveniente } from '../interveniente/interveniente';

export class CartaoBandeira extends CadastroGeral {
    interveniente: Interveniente;
    portador: Portador;
    tipoIntegracao: string;
}