import { Component, OnInit, Inject, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { PagedList } from 'app/classes/pagination/paged-list';
import { Proposta } from 'app/classes/proposta/proposta.classes';
import { CondicaoPagamento, TipoCobranca } from 'app/classes/pagamento/pagamento.classes';
import { Usina } from 'app/classes/usina/usina';
import { Tasks } from 'app/classes/_tasks/tasks';

import { PropostaService } from 'app/services/proposta.service';
import { Programacao, statusProgramacao } from 'app/classes/programacao/programacao';
import { ProgramacaoService } from 'app/services/programacao.service';
import { Obra } from 'app/classes/obra/obra';
import { ETipoVinculoMpaConsumo } from 'app/classes/traco/traco.classes';
import { Endereco } from 'app/classes/endereco/endereco';
import { ObraConsulta } from 'app/classes/obra/obra.classes';

@Component({
  selector: 'app-proposta-programacoes-dialog',
  templateUrl: './proposta-programacoes-dialog.component.html',
  styleUrls: ['./proposta-programacoes-dialog.component.scss']
})
export class PropostaProgramacoesDialogComponent implements OnInit, AfterViewInit {

  programacoes: PagedList<Programacao> = new PagedList<Programacao>();

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;
  filtroString: string;

  constructor(
    public dialogRef: MatDialogRef<PropostaProgramacoesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      propostaAno: number,
      propostaNumero: number
    },
    private _programacaoService: ProgramacaoService,
    private _cdr: ChangeDetectorRef,
  ) { }

  ngOnInit() {
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this._cdr.detectChanges();
      this.filtroString = `filter=$(propostaAno==${this.data.propostaAno};propostaNumero==${this.data.propostaNumero})`
      this.getPage();
    });
  }

  ederecoToString = Tasks.ederecoToString;
  formataData = Tasks.formataData;
  formataHora = Tasks.formataHora;
  formataValor = Tasks.formataValor;

  closeDialog() {
    this.dialogRef.close();
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;

    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    }

    this._programacaoService.ListarComPropostaContratoEmOrdemCrescente(currentPage, pageSize, this.filtroString)
    .then(
      programacoes => {
        this.programacoes = programacoes;
        this.paginaAtual = programacoes.currentPage;
        this.registrosPorPagina = programacoes.pageSize;
      },
      error => { this.programacoes = new PagedList<Programacao>(); }
    )
    .then(() => {
      this._cdr.detectChanges();
    });
  }

  currentPage() {
    if (this.programacoes.currentPage <= 0) return 0;
    return this.programacoes.currentPage - 1;
  }

  filtroChange(novoFiltro: string){
    this.filtroString = novoFiltro;
    this.getPage();
  }

  labelPropostaNumero(programacao: Programacao): string {
    if (this.isSmallScreen) {
      return programacao.propostaNumero.toString().padStart(6,'0');
    } else {
      return programacao.propostaNumero.toString().padStart(6,'0')+'-'+programacao.propostaAno;
    }
  }
  labelClienteObra(programacao: Programacao): string {
    if (this.isSmallScreen) {
      return programacao.proposta.intervenienteRazao.substr(0, 15)//+'... / '+Tasks.ederecoToString(programacao.endereco).substr(0, 25)+'...';
    } else {
      return programacao.proposta.intervenienteRazao//+' / '+Tasks.ederecoToString(programacao.endereco);
    }
  }
  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }
  tracoString(programacao: Programacao): string {
    if (!programacao.resistenciaTipo || !programacao.pedra || !programacao.slump || !programacao.uso || (programacao.mpa + programacao.consumo)===0) return '';
    let vinculo = programacao.resistenciaTipo.vinculo;
    let mpaConsumo = vinculo === ETipoVinculoMpaConsumo.MPA ? programacao.mpa : (vinculo === ETipoVinculoMpaConsumo.CONSUMO ? programacao.consumo : '');
    return programacao.resistenciaTipo.abreviatura+' '+mpaConsumo+' / '+programacao.pedra.descricao+' / '+programacao.slump.descricao+' / '+programacao.uso.descricao;
  }

  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';

  statusProgramacaoFormatter = (model: number): string => {
    if (model === null || model === undefined || isNaN(model)) return '';
    if (model === 0 ) return '';
    return statusProgramacao.filter(e => e.codigo === model)[0].descricao.toUpperCase();
  };

}
