import { Component, OnInit, Inject, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { PagedList } from 'app/classes/pagination/paged-list';
import { Proposta } from 'app/classes/proposta/proposta.classes';
import { CondicaoPagamento, TipoCobranca } from 'app/classes/pagamento/pagamento.classes';
import { Usina } from 'app/classes/usina/usina';
import { Tasks } from 'app/classes/_tasks/tasks';

import { PropostaService } from 'app/services/proposta.service';

@Component({
  selector: 'app-proposta-importacao-dialog',
  templateUrl: './proposta-importacao-dialog.component.html',
  styleUrls: ['./proposta-importacao-dialog.component.scss']
})
export class PropostaImportacaoDialogComponent implements OnInit, AfterViewInit {

  propostas: PagedList<Proposta> = new PagedList<Proposta>();

  paginaAtual: number = 1;
  registrosPorPagina: number = 30;

  constructor(
    public dialogRef: MatDialogRef<PropostaImportacaoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      cpfCnpj: string,
      importCallback: (proposta: Proposta) => void
    },
    private _propostaService: PropostaService,
    private _cdr: ChangeDetectorRef,
  ) { }

  ngOnInit() {
  }
  ngAfterViewInit(): void {
    setTimeout(() => {
      this._cdr.detectChanges();
      this.getPage();
    });
  }

  closeDialog() {
    this.dialogRef.close();
  }

  import(proposta: Proposta): void {
    this._propostaService.ObterPorUsinaAnoNumero(proposta.usina, proposta.ano, proposta.numero)
    .then(propostaImportada => {
      this.data.importCallback(propostaImportada);
      this.closeDialog();
    });
  }

  getPage(pageInfo?) {
    let currentPage = this.paginaAtual;
    let pageSize = this.registrosPorPagina;

    if (pageInfo) {
      currentPage = pageInfo.currentPage;
      pageSize = pageInfo.pageSize;
    }

    this._propostaService.ListarPorCpfCnpj(this.data.cpfCnpj, currentPage, pageSize)
    .then(
      propostas => {
        this.propostas = propostas;
        this.paginaAtual = propostas.currentPage;
        this.registrosPorPagina = propostas.pageSize;
      },
      error => { this.propostas = new PagedList<Proposta>(); }
    );
  }

  currentPage() {
    if (this.propostas.currentPage <= 0) return 0;
    return this.propostas.currentPage - 1;
  }

  filtroChange(novoFiltro: string){
    this.getPage();
  }

  labelPropostaNumero(proposta: Proposta): string {
    return proposta.usina.codigo+'/'+proposta.numero.toString().padStart(6,'0')+'-'+proposta.ano;
  }

  ederecoToString = Tasks.ederecoToString;
  formataData = Tasks.formataData;

  condicaoPagamentoFormatter = (model: CondicaoPagamento): string => model ? model.descricao.toUpperCase() : '';
  tipoCobrancaFormatter = (model: TipoCobranca): string => model ? model.descricao.toUpperCase() : '';
  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';

}
