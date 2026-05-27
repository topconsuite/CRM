import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, EventEmitter, HostListener, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Tasks } from 'app/classes/_tasks/tasks';
import { Tarefa } from 'app/classes/agenda/tarefa';
import { TarefaService } from 'app/services/tarefa.service';
import { UserService } from 'app/services/user.service';
import { ICustomValidator } from '../../interfaces/custom-validator';
import { AlertDialogComponent } from '../../dialog/alert-dialog/alert-dialog.component';
import { Visita } from 'app/classes/visita/visita';
import { Lead } from 'app/classes/lead/lead';
import { Oportunidade } from 'app/classes/oportunidade/oportunidade';
import { CompromissoService } from 'app/services/compromisso.service';

@Component({
  selector: 'app-tarefa-form',
  templateUrl: './tarefa-form.component.html',
  styleUrls: ['./tarefa-form.component.scss']
})
export class TarefaFormComponent implements OnInit {
  public static self: TarefaFormComponent;

  tarefaForm: FormGroup;

  @Input() itemTarefa: Tarefa;
  @Output() cancel = new EventEmitter<boolean>();
  @Output() confirm = new EventEmitter<boolean>();

  @Input() visita: Visita;
  @Input() lead: Lead;
  @Input() oportunidade: Oportunidade;
      
  usuarios: { [key: string]: string } = {};
  gruposSelecionado: string[] = [];
  usuariosSelecionados: string[] = [];
  usuariosItemTarefaAgrupamento:  { [key: string]: string } = {};
  tarefaParaOutrosUsuario: boolean = false;

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
    private _tarefaService: TarefaService,
    private _userService: UserService,
    private _compromissoService: CompromissoService
  ) { 

    this._compromissoService.ListarGruposDeUsuarioRespeitandoDireito()
    .then((usuarios) => {
      this.usuarios = this.convertKeysToUpper(usuarios);
    });

  }

  ngOnInit() {
    this.tarefaForm = this._formBuilder.group({
      diaInteiro: [''],
      tarefaParaOutrosUsuario: false
    });  

    if (!this.itemTarefa) this.itemTarefa = new Tarefa();

    if(this.itemTarefa.codigo > 0 && this.itemTarefa.idAgrupamento !== '') {
      this._tarefaService.ListarUsuarioAgrupamento(this.itemTarefa.idAgrupamento)
      .then((usuariosAgrupamento) => {
        this.usuariosItemTarefaAgrupamento = this.convertKeysToUpper(usuariosAgrupamento);
      });
    }

    if (this.visita){
      this.itemTarefa.anoVisita = this.visita.ano;
      this.itemTarefa.numeroVisita = this.visita.numero;
      this.itemTarefa.contato = this.visita.contatoPrincipal.nome;
      this.itemTarefa.dddTelefone = this.visita.dddTelefone;
      this.itemTarefa.telefone = this.visita.telefone;
      this.itemTarefa.dddCelular = this.visita.dddCelular;
      this.itemTarefa.celular = this.visita.celular;
      this.itemTarefa.email = this.visita.email;
    }

    if (this.lead){
      this.itemTarefa.anoLead = this.lead.ano;
      this.itemTarefa.numeroLead = this.lead.numero;
      this.itemTarefa.contato = this.lead.contatoPrincipal.nome;
      this.itemTarefa.dddTelefone = this.lead.dddTelefone;
      this.itemTarefa.telefone = this.lead.telefone;
      this.itemTarefa.dddCelular = this.lead.dddCelular;
      this.itemTarefa.celular = this.lead.celular;
      this.itemTarefa.email = this.lead.email;
    }

    if (this.oportunidade){
      this.itemTarefa.anoOportunidade = this.oportunidade.ano;
      this.itemTarefa.numeroOportunidade = this.oportunidade.numero;
      this.itemTarefa.contato = this.oportunidade.contatoPrincipal.nome;
      this.itemTarefa.dddTelefone = this.oportunidade.dddTelefone;
      this.itemTarefa.telefone = this.oportunidade.telefone;
      this.itemTarefa.dddCelular = this.oportunidade.dddCelular;
      this.itemTarefa.celular = this.oportunidade.celular;
      this.itemTarefa.email = this.oportunidade.email;
    }

    this.temDireitoGrupoAcesso = this._userService.temDireitoAplicativo('WEB7009', '');
    this.temDireitoGrupoInclui = this._userService.temDireitoAplicativo('WEB7009', 'I');
    this.temDireitoGrupoAlteracao = this._userService.temDireitoAplicativo('WEB7009', 'A');
    this.temDireitoGrupoDeletar = this._userService.temDireitoAplicativo('WEB7009', 'E');

    this.temDireitoGeralAcesso = this._userService.temDireitoAplicativo('WEB7010', '');
    this.temDireitoGeralInclui = this._userService.temDireitoAplicativo('WEB7010', 'I');
    this.temDireitoGeralAlteracao = this._userService.temDireitoAplicativo('WEB7010', 'A');
    this.temDireitoGeralDeletar = this._userService.temDireitoAplicativo('WEB7010', 'E');

    this.itemTarefa.horarioString = this.itemTarefa.horario.length >= 5 ? this.itemTarefa.horario.substring(0, 5).replace(':', '') : this.itemTarefa.horario;
  }

  removerDuplicatas(arrayComDuplicatas: string[]): string[] {
    // Retorna apenas os elementos cuja primeira aparição (indexOf)
    // é igual à sua posição atual (index)
    return arrayComDuplicatas.filter((item, index) => {
        return arrayComDuplicatas.indexOf(item) === index;
    });
  }

  get gruposUsuarios(): string[] {
    
    var isInsert = this.itemTarefa.codigo === 0;

    if ((isInsert && this.temDireitoGeralInclui) || (!isInsert && this.temDireitoGeralAlteracao)) {
      // 1. Pega todos os valores (com repetidos)
      const todosGrupos = Object.values(this.usuarios);
      
      // 2. O "new Set" remove duplicatas e o "[... ]" transforma de volta em array
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

  stringHHMMParaTimeSpan(timeString: string): string {
    const horas = timeString.slice(0, 2); // Extrai as duas primeiras posições como horas
    const minutos = timeString.slice(2);  // Extrai as últimas duas posições como minutos

    // Formata para o formato "HH:mm" ou "HH:mm:ss" (com segundos 00)
    return `${horas}:${minutos}:00`;
  }

  changeHorario(hora: string) { if(hora.length === 4) this.itemTarefa.horario = this.stringHHMMParaTimeSpan(hora); }

  convertKeysToUpper(data: {}): {} {
    const newData = {};
  
    // Itera sobre as chaves do objeto original
    Object.keys(data).forEach((key) => {
      // Cria a nova chave em UpperCase e atribui o valor original
      newData[key.toUpperCase()] = data[key];
    });
  
    return newData;
  }

  get horaValidator(): ICustomValidator {
    var _self = TarefaFormComponent.self;
    var _tasks = Tasks;

    if(this.itemTarefa.horarioString === "") return;

    var message = 'Informe um horário válido';

    return {
      key: 'horarioInvalido',
      message: message,
      validatorFunction: (hora: string): boolean => {
        return !(_tasks.horarioValido(hora));
      },
      params: [ this.itemTarefa.horarioString ]
    }
  }

  confirmModal(tarefa: Tarefa): void {
    if (tarefa.codigo === 0) {
      return this.addTarefa(tarefa);
    }
      
    this.updateTarefa(tarefa);
  }

  cancelModal() {
    this.tarefaForm.markAsPristine();
    this.tarefaForm.markAsUntouched();
    this.cancel.emit(true);
  }

  confirmModalCallback(): void {
    this.confirm.emit(true);
  }

  temPermissaoParaAlterar() {
    if(this.itemTarefa.idAgrupamento === "" || this.itemTarefa.codigo === 0)
      return true;

    const userName = this._userService.getUserName();
    const grupoDoUsuarioLogado = this.usuarios[userName];
    
    const existeUsuarioGrupoDiferenteUsuario = Object.values(this.usuariosItemTarefaAgrupamento).filter(x => x !== grupoDoUsuarioLogado).length > 0;

    var possuiDireito = (existeUsuarioGrupoDiferenteUsuario && this.temDireitoGeralAlteracao)
    possuiDireito = possuiDireito || (!existeUsuarioGrupoDiferenteUsuario && (this.temDireitoGeralAlteracao || this.temDireitoGrupoAlteracao))

    return possuiDireito;
  }

  updateTarefa(tarefa: Tarefa): void {
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

    this._tarefaService.Atualizar(tarefa)
    .then(success => {
    this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Tarefa alterada com sucesso!`,
          afterCloseCallback: this.confirmModalCallback()
        }
      });
    }, err => {
    this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Erro ao alterar a Tarefa.\n${this.formataErrosApi(err)}`
        }
      });
    });
  }

  addTarefa(tarefa: Tarefa): void {
    var temDireito = this._userService.temDireitoAplicativo('WEB7007','I');
    if (!temDireito) {
      this._dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: `Você não tem permissão para inserir uma nova Tarefa!`
        }
      });
      return;
    }
    
    const tarefaAgrupamento: Tarefa[] = [];

    if(this.tarefaParaOutrosUsuario) {

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
            message: `Você não tem permissão para inserir tarefas em grupo!`
          }
        });
        return;
      }

      this.usuariosSelecionados.forEach(usuario => {
        const tarefaCopia = tarefa.clone();
        tarefaCopia.usuario = usuario;
        tarefaAgrupamento.push(tarefaCopia);
      });

    } 

    (
      this.tarefaParaOutrosUsuario ? 
      this._tarefaService.AdicionarAgrupamento(tarefaAgrupamento) :
      this._tarefaService.Adicionar(tarefa)
    ).then(success => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Tarefa adicionada com sucesso!`,
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

  diaInteiroChange(evento: any, propriedade: string) {
    if(this.itemTarefa.diaInteiro) {
      this.itemTarefa.horarioString = "";
      this.itemTarefa.horario = "";
    }
  }

}
