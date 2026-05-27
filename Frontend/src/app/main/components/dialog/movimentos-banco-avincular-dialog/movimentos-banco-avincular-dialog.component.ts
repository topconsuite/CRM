import { Component, OnInit, Inject, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { PagamentoService } from 'app/services/pagamento.service';

import { Tasks } from 'app/classes/_tasks/tasks';
import { Conta } from 'app/classes/pagamento/pagamento.classes';
import { MovimentoBanco } from 'app/classes/movimento-banco/movimento-banco';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-movimentos-banco-avincular-dialog',
  templateUrl: './movimentos-banco-avincular-dialog.component.html',
  styleUrls: ['./movimentos-banco-avincular-dialog.component.scss']
})
export class MovimentosBancoAVincularDialogComponent implements OnInit {

  form: FormGroup;

  movimentosBanco: MovimentoBanco[] = [];

  formataMoeda = Tasks.formataMoeda;
  formataData = Tasks.formataData;

  porPagina = 5;
  paginaAtual = 1;
  get paginas(): number {
    let mod = this.movimentosBanco.length % this.porPagina;
    let pages = Math.floor(this.movimentosBanco.length / this.porPagina);

    return pages + (mod > 0 ? 1 : 0);
  }
  get registrosPagina(): MovimentoBanco[] {
    return this.movimentosBanco.filter((item, index) => {
      let max = (this.paginaAtual * this.porPagina) - 1;
      let min = (max - this.porPagina) + 1;
      return index >= min && index <= max;
    });
  }

  constructor(
    private _formBuilder: FormBuilder,
    private _cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<MovimentosBancoAVincularDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      conta: Conta,
      valorAVincular: number,
      dataOperacao: Date,
      pagamentoService: PagamentoService,
      confirmCallback?: Function,
      cancelCallback?: Function
    }
  ) {
    this.form = this._formBuilder.group({});
    this.carregarMovimentosBancoNaoVinculados();
  }

  ngOnInit() {
    this.form = this._formBuilder.group({});
  }

  selectionChange(item: MovimentoBanco): void {
    item['expanded'] = !item['expanded'];
  }

  get selecionados(): MovimentoBanco[] {
    return this.movimentosBanco.filter(t => t['selecionado']);
  }
  get valorSelecionado(): number {
    return this.movimentosBanco
      .filter(t => t['selecionado'])
      .map(t => t.saldo).reduce((a, b) => a + b, 0);
  }

  confirm() {
    this.dialogRef.close();
    if (this.data.confirmCallback) this.data.confirmCallback(this.selecionados);
  }

  cancel() {
    this.dialogRef.close();
    if (this.data.cancelCallback) this.data.cancelCallback();
  }

  carregarMovimentosBancoNaoVinculados() {
    this.paginaAtual = 1;
    if (this.data.dataOperacao)
      this.data.dataOperacao = new Date(this.data.dataOperacao);

    this.data.pagamentoService
      .ListarMovimentosBancoNaoVinculadosComContasAReceber(this.data.conta.empresaCodigo, this.data.conta.codigo, this.data.dataOperacao)
      .then(res => {
        this.movimentosBanco = res;
      }, err => {
        this.movimentosBanco = [];
      })
      .then(() => {
        this._cdr.detectChanges();
      });
  }

  get isSmallScreen(): boolean {
    return window.innerWidth <= 600;
  }

  get pageInfoLabel(): string {
    if (this.isSmallScreen) return this.paginaAtual + ' de ' + this.paginas;

    let currentPage = this.paginaAtual <= 0 ? 0 : this.paginaAtual - 1;
    let from = (currentPage * this.porPagina) + 1;
    let until = from + this.porPagina - 1;
    return from + ' a ' + until + ' de ' + this.movimentosBanco.length;
  }

  disabledIconClass(disabled: boolean): string {
    return disabled ? 'disabled-icon' : 'enabled-icon';
  }
  get navigationButtonClass(): string {
    if (this.isSmallScreen) return 'mat-icon-button';
    return 'mat-button';
  }

  get disablePrevious(): boolean {
    return this.paginaAtual <= 1;
  }
  get disableNext(): boolean {
    return this.paginaAtual >= this.paginas;
  }

  previous() {
    this.paginaAtual--;
    this._cdr.detectChanges();
  }
  next() {
    this.paginaAtual++;
    this._cdr.detectChanges();
  }
  first() {
    this.paginaAtual = 1;
    this._cdr.detectChanges();
  }
  last() {
    this.paginaAtual = this.paginas;
    this._cdr.detectChanges();
  }

}
