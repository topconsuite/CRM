import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { Compromisso } from 'app/classes/agenda/compromisso';
import { Visita } from 'app/classes/visita/visita';
import { CompromissoService } from 'app/services/compromisso.service';
import { AlertDialogComponent } from '../../dialog/alert-dialog/alert-dialog.component';
import { ICustomValidator } from '../../interfaces/custom-validator';
import { Lead } from 'app/classes/lead/lead';
import { UserService } from 'app/services/user.service';
import { Oportunidade } from 'app/classes/oportunidade/oportunidade';

@Component({
  selector: 'app-compromisso-form',
  templateUrl: './compromisso-form.component.html',
  styleUrls: ['./compromisso-form.component.scss']
})
export class CompromissoFormComponent implements OnInit {
  public static self: CompromissoFormComponent;

  compromissoForm: FormGroup;

  @Input() itemCompromisso: Compromisso;
  @Output() cancel = new EventEmitter<boolean>();
  @Output() confirm = new EventEmitter<boolean>();

  @Input() visita: Visita;
  @Input() lead: Lead;
  @Input() oportunidade: Oportunidade;

  usuarios: { [key: string]: string } = {};
  gruposSelecionado: string[] = [];
  usuariosSelecionados: string[] = [];
  usuariosItemCompromissoAgrupamento:  { [key: string]: string } = {};
  compromissoParaOutrosUsuario: boolean = false;

  temDireitoGrupoAcesso = false;
  temDireitoGrupoInclui = false;
  temDireitoGrupoAlteracao = false;
  temDireitoGrupoDeletar = false;
  
  temDireitoGeralAcesso = false;
  temDireitoGeralInclui = false;
  temDireitoGeralAlteracao = false;
  temDireitoGeralDeletar = false;

  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;
  formataErrosApi = Tasks.formataErrosApi;
  maskHora = [/\d/, /\d/, ':', /\d/, /\d/];

  constructor(
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef,
    private _formBuilder: FormBuilder,
    private _compromissoService: CompromissoService,
    private _userService: UserService
  ) {

    this._compromissoService.ListarGruposDeUsuarioRespeitandoDireito()
    .then((usuarios) => {
      this.usuarios = this.convertKeysToUpper(usuarios);
    });

  }

  ngOnInit() {
    this.compromissoForm = this._formBuilder.group({
      diaInteiro: [''],
      compromissoParaOutrosUsuario : false
    });

    if (!this.itemCompromisso) this.itemCompromisso = new Compromisso();

    if(this.itemCompromisso.codigo > 0 && this.itemCompromisso.idAgrupamento !== '') {
      this._compromissoService.ListarUsuarioAgrupamento(this.itemCompromisso.idAgrupamento)
      .then((usuariosAgrupamento) => {
        this.usuariosItemCompromissoAgrupamento = this.convertKeysToUpper(usuariosAgrupamento);
      });
    }

    if (this.visita){
      this.itemCompromisso.anoVisita = this.visita.ano;
      this.itemCompromisso.numeroVisita = this.visita.numero;
      this.itemCompromisso.contato = this.visita.contatoPrincipal.nome;
      this.itemCompromisso.dddTelefone = this.visita.dddTelefone;
      this.itemCompromisso.telefone = this.visita.telefone;
      this.itemCompromisso.dddCelular = this.visita.dddCelular;
      this.itemCompromisso.celular = this.visita.celular;
      this.itemCompromisso.email = this.visita.email;
    }

    if (this.lead){
      this.itemCompromisso.anoLead = this.lead.ano;
      this.itemCompromisso.numeroLead = this.lead.numero;
      this.itemCompromisso.contato = this.lead.contatoPrincipal.nome;
      this.itemCompromisso.dddTelefone = this.lead.dddTelefone;
      this.itemCompromisso.telefone = this.lead.telefone;
      this.itemCompromisso.dddCelular = this.lead.dddCelular;
      this.itemCompromisso.celular = this.lead.celular;
      this.itemCompromisso.email = this.lead.email;
    }

    if (this.oportunidade){
      this.itemCompromisso.anoOportunidade = this.oportunidade.ano;
      this.itemCompromisso.numeroOportunidade = this.oportunidade.numero;
      this.itemCompromisso.contato = this.oportunidade.contatoPrincipal.nome;
      this.itemCompromisso.dddTelefone = this.oportunidade.dddTelefone;
      this.itemCompromisso.telefone = this.oportunidade.telefone;
      this.itemCompromisso.dddCelular = this.oportunidade.dddCelular;
      this.itemCompromisso.celular = this.oportunidade.celular;
      this.itemCompromisso.email = this.oportunidade.email;
    }

    this.temDireitoGrupoAcesso = this._userService.temDireitoAplicativo('WEB7009', '');
    this.temDireitoGrupoInclui = this._userService.temDireitoAplicativo('WEB7009', 'I');
    this.temDireitoGrupoAlteracao = this._userService.temDireitoAplicativo('WEB7009', 'A');
    this.temDireitoGrupoDeletar = this._userService.temDireitoAplicativo('WEB7009', 'E');

    this.temDireitoGeralAcesso = this._userService.temDireitoAplicativo('WEB7010', '');
    this.temDireitoGeralInclui = this._userService.temDireitoAplicativo('WEB7010', 'I');
    this.temDireitoGeralAlteracao = this._userService.temDireitoAplicativo('WEB7010', 'A');
    this.temDireitoGeralDeletar = this._userService.temDireitoAplicativo('WEB7010', 'E');

    this.itemCompromisso.horaInicioString = this.itemCompromisso.horaInicio.length >= 5 ? this.itemCompromisso.horaInicio.substring(0, 5).replace(':', '') : this.itemCompromisso.horaInicio;
    this.itemCompromisso.horaFimString = this.itemCompromisso.horaFim.length >= 5 ? this.itemCompromisso.horaFim.substring(0, 5).replace(':', '') : this.itemCompromisso.horaFim; 
  }

  stringHHMMParaTimeSpan(timeString: string): string {
    const horas = timeString.slice(0, 2); // Extrai as duas primeiras posições como horas
    const minutos = timeString.slice(2);  // Extrai as últimas duas posições como minutos

    // Formata para o formato "HH:mm" ou "HH:mm:ss" (com segundos 00)
    return `${horas}:${minutos}:00`;
  }

  get gruposUsuarios(): string[] {
    
    var isInsert = this.itemCompromisso.codigo === 0;

    if ((isInsert && this.temDireitoGeralInclui) || (!isInsert && this.temDireitoGeralAlteracao)) {
      // 1. Pega todos os valores (com repetidos)
      const todosGrupos = Object.values(this.usuarios);
      
      return this.removerDuplicatas(todosGrupos);
    }
  
    if ((isInsert && this.temDireitoGrupoInclui) || (!isInsert && this.temDireitoGrupoAlteracao)) {
      const userName = this._userService.getUserName();
      const grupoDoUsuarioLogado = this.usuarios[userName]; // Ex: "VENDEDOR"
  
      if (!grupoDoUsuarioLogado) return [];

      return [grupoDoUsuarioLogado];
    }
  
    return [];
  }

  removerDuplicatas(arrayComDuplicatas: string[]): string[] {
    // Retorna apenas os elementos cuja primeira aparição (indexOf)
    // é igual à sua posição atual (index)
    return arrayComDuplicatas.filter((item, index) => {
        return arrayComDuplicatas.indexOf(item) === index;
    });
  }

  get usuariosDoGrupo(): string[] {
    if (this.gruposSelecionado.length == 0) {
      return [];
    }
    
    const keysArray = (this.gruposSelecionado.length == 0 ? 
      Object.keys(this.usuarios) : 
      Object.keys(this.usuarios).filter(key => this.gruposSelecionado.filter(x => x === this.usuarios[key]).length > 0))

    const keys = keysArray.map(key => key.toUpperCase());

    const result = this.removerDuplicatas(keys);
  
    return result;
  }

  changeHoraInicio(hora: string) { if(hora.length === 4) this.itemCompromisso.horaInicio = this.stringHHMMParaTimeSpan(hora); }
  changeHoraFim(hora: string) { if(hora.length === 4) this.itemCompromisso.horaFim = this.stringHHMMParaTimeSpan(hora); }

  convertKeysToUpper(data: {}): {} {
    const newData = {};
  
    // Itera sobre as chaves do objeto original
    Object.keys(data).forEach((key) => {
      // Cria a nova chave em UpperCase e atribui o valor original
      newData[key.toUpperCase()] = data[key];
    });
  
    return newData;
  }
  horaValidator(hora: string): ICustomValidator {
    var _self = CompromissoFormComponent.self;
    var _tasks = Tasks;

    if(hora === "") return;

    var message = 'Informe uma hora válida';

    return {
      key: 'horaInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        return !(_tasks.horarioValido(hora));
      },
      params: [hora]
    }
  }

  confirmModal(compromisso: Compromisso): void {
    if (compromisso.codigo === 0) {
      return this.addCompromisso(compromisso);
    }
      
    this.updateCompromisso(compromisso);
  }

  temPermissaoParaAlterar() {

    if(this.itemCompromisso.idAgrupamento === "" || this.itemCompromisso.codigo === 0)
      return true;

    const userName = this._userService.getUserName();
    const grupoDoUsuarioLogado = this.usuarios[userName];
    
    const existeUsuarioGrupoDiferenteUsuario = Object.values(this.usuariosItemCompromissoAgrupamento).filter(x => x !== grupoDoUsuarioLogado).length > 0;

    var possuiDireito = (existeUsuarioGrupoDiferenteUsuario && this.temDireitoGeralAlteracao)
    possuiDireito = possuiDireito || (!existeUsuarioGrupoDiferenteUsuario && (this.temDireitoGeralAlteracao || this.temDireitoGrupoAlteracao))

    return possuiDireito;
  }


  cancelModal() {
    this.compromissoForm.markAsPristine();
    this.compromissoForm.markAsUntouched();
    this.cancel.emit(true);
  }

  confirmModalCallback(): void {
    this.confirm.emit(true);
  }

  updateCompromisso(compromisso: Compromisso): void {
    var temDireito = this._userService.temDireitoAplicativo('WEB7007','A');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão de alteração!`
        }
      });
      return;
    }

     this._compromissoService.Atualizar(compromisso)
     .then(success => {
      this._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Compromisso alterada com sucesso!`,
           afterCloseCallback: this.confirmModalCallback()
         }
       });
     }, err => {
      this._dialog.open(AlertDialogComponent, {
         disableClose: true,
         data: {
           title: 'TopConWeb',
           message: `Erro ao alterar a Compromisso.\n${this.formataErrosApi(err)}`
         }
       });
     });
  }

  addCompromisso(compromisso: Compromisso): void {
    var temDireito = this._userService.temDireitoAplicativo('WEB7007','I');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para inserir um novo compromisso!`
        }
      });
      return;
    }

    const compromissoAgrupamento: Compromisso[] = [];

    if(this.compromissoParaOutrosUsuario) {

      if(this.usuariosSelecionados.length === 0) {
          this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Nenhum usuário selecionado!`
          }
        });
        return;
      }

      if (!this.temDireitoGeralInclui && !this.temDireitoGrupoInclui) {
        this._dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: `Você não tem permissão para inserir compromissos em grupo!`
          }
        });
        return;
      }

      this.usuariosSelecionados.forEach(usuario => {
        const compromissoCopia = compromisso.clone();
        compromissoCopia.usuario = usuario;
        compromissoAgrupamento.push(compromissoCopia);
      });

    }

    (
      this.compromissoParaOutrosUsuario ?
      this._compromissoService.AdicionarAgrupamento(compromissoAgrupamento) :
      this._compromissoService.Adicionar(compromisso)
    )
    .then(success => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Compromisso adicionada com sucesso!`,
          afterCloseCallback: this.confirmModalCallback()
        }
      });
    }, err => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `${this.formataErrosApi(err)}`
        }
      });
    });
  }

  remainingCharacters(text: string, length: number) {
    return (text.length <= length) ? (length - text.length) + " caracteres restantes" : "limite de caracteres ultrapassado(" + (text.length - length) + ")" ;
  }

  diaInteiroChange(evento: any) {
    if(this.itemCompromisso.diaInteiro) {
      this.itemCompromisso.horaFimString = "";
      this.itemCompromisso.horaFim = "";
      this.itemCompromisso.horaInicioString = "";
      this.itemCompromisso.horaInicio = "";
    }
  }

}
