import { Interveniente } from "../bomba/bomba.classes";
import { Vendedor } from "../vendedor/vendedor";

export class ObraIndicador {
    obraUsina: number = 0;
    obraNumero: number = 0;

    intervenienteCodigo: number = 0;
    interveniente: Interveniente;

    vendedorCodigo: number = 0;
    vendedor: Vendedor;
}