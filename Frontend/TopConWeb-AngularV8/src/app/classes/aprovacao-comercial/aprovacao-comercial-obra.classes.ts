import { Obra } from "../obra/obra";
import { Usina } from "../usina/usina";

export class AprovacaoComercialObra {
    id: string = '';
    nivelAutoridade: number = 0;
    usuarioId: string = '';
    mensagem: string = '';
    aprovacaoDt: Date = new Date();
    aprovacaoStatus: boolean = false;
    usinaCodigo: number = 0;
    usina: Usina = new Usina();
    obraNumero: number = 0;
    obra: Obra = new Obra();
  }
  
export class AprovacaoComercialObraTraco {
    id: string = '';
    nivelAutoridade: number = 0;
    usuarioId: string = '';
    mensagem: string = '';
    aprovacaoDt: Date = new Date();
    aprovacaoStatus: boolean = false;
    usinaCodigo: number = 0;
    obraNumero: number = 0;
    sequencia: number = 0;
  }
  
  export class AprovacaoComercialObraBomba {
    id: string = '';
    nivelAutoridade: number = 0;
    usuarioId: string = '';
    mensagem: string = '';
    aprovacaoDt: Date = new Date();
    aprovacaoStatus: boolean = false;
    usinaCodigo: number = 0;
    usina: Usina = new Usina();
    obraNumero: number = 0;
    obra: Obra = new Obra();
    sequencia: number = 0;
  }
  