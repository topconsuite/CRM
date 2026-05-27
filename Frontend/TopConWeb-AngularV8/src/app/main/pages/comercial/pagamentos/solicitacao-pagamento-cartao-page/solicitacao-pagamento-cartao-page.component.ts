import { Component, OnInit, Inject, AfterViewInit, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';

import { CartaoBandeira } from 'app/classes/pagamento/cartao-bandeira';
import { SolicitacaoPagamento } from 'app/classes/pagamento/solicitacao-pagamento';
import { Obra } from 'app/classes/obra/obra';
import { Proposta } from 'app/classes/proposta/proposta';
import { Pagamento } from 'app/classes/pagamento/pagamento';
import { Tasks } from 'app/classes/_tasks/tasks';

import { PagamentoService } from 'app/services/pagamento.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { ObraService } from 'app/services/obra.service';
import { PropostaService } from 'app/services/proposta.service';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { Router } from '@angular/router';
import { UserService } from 'app/services/user.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-solicitacao-pagamento-cartao-page',
  templateUrl: './solicitacao-pagamento-cartao-page.component.html',
  styleUrls: ['./solicitacao-pagamento-cartao-page.component.scss']
})
export class SolicitacaoPagamentoCartaoPageComponent implements OnInit,AfterViewInit {

  public static self: SolicitacaoPagamentoCartaoPageComponent;

  bandeiras: CartaoBandeira[] = [];
  solicitacaoPagamento: SolicitacaoPagamento = new SolicitacaoPagamento();
  solicitacaoPagamentoForm: FormGroup;
  tipoDocumento: number = 0;
  obras: Obra[] = [];
  tipoCobrancaKeys: string[] = ["CC","CD"];
  tipoCobrancas = {"CC": "Cartão de Crédito","CD": "Cartão de Débito"};
  
  
  modalMode: boolean = false;
  intervenienteJaCadastrado: boolean = false;

  constructor(
    private _formBuilder: FormBuilder,
    public dialog: MatDialog,
    private _router: Router,
    private _cdr: ChangeDetectorRef,
    private _userService: UserService,
    private _pagamentoService: PagamentoService,
    private _obraService: ObraService,
    private _propostaService: PropostaService,
    private _intervenienteService: IntervenienteService,
    public dialogRef: MatDialogRef<SolicitacaoPagamentoCartaoPageComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: { proposta: Proposta, pagamentoSelecionado: Pagamento}) 
  {

  }

  ngOnInit() {
    this.solicitacaoPagamentoForm = this._formBuilder.group({
      solicitacaoPagamentoRadioButtonsForm: ['']
    });

    this._pagamentoService.ListarCartaoBandeirasComIntegracao(true).then(
      bandeiras => { 
        if (bandeiras.length == 0) {
          this._router.navigate(['pages/integracao-cartao']);
        } else {
          this.bandeiras = bandeiras 
        }},
      error => { this.bandeiras = [] }
    );
    if (this.data.proposta !== null && this.data.pagamentoSelecionado !== null) {
      this.intervenienteJaCadastrado = true;
    }
    this._cdr.detectChanges();
  }

  ngAfterViewInit(): void {
    if (this.data.proposta !== null && this.data.pagamentoSelecionado !== null) { 
      this.solicitacaoPagamento.intervenienteRazao = this.data.proposta.intervenienteRazao;
      this.solicitacaoPagamento.cpfCnpj = this.data.proposta.cpfCnpj;
      this.tipoDocumento = this.data.proposta.cpfCnpj.length === 11 ? 0 : 1;
      this.obras.push(this.data.proposta.obra);
      this.solicitacaoPagamento.obra = this.data.proposta.obra
      this.solicitacaoPagamento.valorTotal = this.data.pagamentoSelecionado.valor;
      this.solicitacaoPagamento.tipoCobranca = this.data.pagamentoSelecionado.tipoCobranca.forma;
      this.solicitacaoPagamento.quantidadeParcelas = this.data.pagamentoSelecionado.condicaoPagamento.quantidadeParcelas;
      this.modalMode = true;
      this.intervenienteJaCadastrado = true;
    } else {
      this.solicitacaoPagamento.tipoCobranca = "CC";
    }

    this._cdr.detectChanges();
  }

  bandeiraFormatter = (model: CartaoBandeira): string => model ? (`${model.codigo} - ${model.descricao}`).toUpperCase() : '';
  obraFormatter = (model: Obra): string => model ? `${model.usinaCodigo} - ${model.numero} - ${Tasks.ederecoToString(model.endereco)}` : '';
  cobrancaFormatter = (model: string): string => model ? `${this.tipoCobrancas[model] || ''}` : ''; 
  
  radioButtonChange() {
    this.limpaCampos();
  }

  limpaCampos() {
    this.solicitacaoPagamento.intervenienteRazao = "";
    this.obras = [];
    this.intervenienteJaCadastrado = false;
    this.solicitacaoPagamento.obra = null;
  }

  carregaDados(cpfCnpj : string, inscricaoEstadual : string) {
    this.limpaCampos();
    if ( (this.tipoDocumento === 0 && cpfCnpj.length === 11 ) 
        || (this.tipoDocumento === 1 && cpfCnpj.length === 14 ) ) {
          this._intervenienteService.obterPorCpfCnpj(cpfCnpj,inscricaoEstadual)
          .then(interveniente => {
            if (interveniente != null) {
              this.solicitacaoPagamento.intervenienteRazao = interveniente.nome;
              this.intervenienteJaCadastrado = true;
              this._propostaService.ListarPorCpfCnpj(cpfCnpj,1,100)
              .then(pagedList => {
                if (pagedList.recordCount > 0) {
                  var propostas = pagedList.records;
                  this.obras = propostas.map(t => t.obra);
                }
              });
            }else {
              this._propostaService.ListarPorCpfCnpj(cpfCnpj,1,100)
              .then(pagedList => {
                if (pagedList.recordCount > 0) {
                  var propostas = pagedList.records;
                  var inteveniente = propostas.map(t => t.interveniente).pop();
                  this.solicitacaoPagamento.intervenienteRazao = inteveniente.nome;
                  this.intervenienteJaCadastrado = true;
                  this.obras = propostas.map(t => t.obra);
                }
              });
            }
          });
    }
  }

  cancelarSolicitacao(){
    this.closeModal();
  }
  
  closeModal(){
    this.limparForm();
    this.dialogRef.close();
  }

  limparForm(){
    this.solicitacaoPagamentoForm.markAsPristine();
    this.solicitacaoPagamentoForm.markAsUntouched();
    this.solicitacaoPagamento = new SolicitacaoPagamento();
    this.solicitacaoPagamento.tipoCobranca = "CC";
  }

  enviarSolicitacao(){
    this._pagamentoService.SolicitarPagamento(this.solicitacaoPagamento)
    .then(
      solicitacao => {                
        this.dialog.open(AlertDialogComponent, {
        data: {
          title: 'TopConWeb',
          message: 'Pagamento solicitado com sucesso!'
        } });
        if (this.modalMode) {
          this.closeModal();
        } else {
          this.limparForm();
        } },
      error => {
        this.dialog.open(AlertDialogComponent, {
          data: {
            title: 'TopConWeb',
            message: 'HOUVE UM ERRO!'
          }
        });
      }
    );
  }

}

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-solicitacao-pagamento-cartao-page',
  templateUrl: './solicitacao-pagamento-cartao-page.component.html',
  styleUrls: ['./solicitacao-pagamento-cartao-page.component.scss']
})
export class SolicitacaoPagamentoCartaoPageComponentNotModal extends SolicitacaoPagamentoCartaoPageComponent {
  
  constructor(
     _formBuilder: FormBuilder,
     _cdr: ChangeDetectorRef,
     _dialog: MatDialog,
     _router: Router,
     _userService: UserService,
     _pagamentoService: PagamentoService,
     _obraService: ObraService,
     _propostaService: PropostaService,
     _intervenienteService: IntervenienteService
    )
    {
      super(
        _formBuilder,
        _dialog,
        _router,
        _cdr,
        _userService,
        _pagamentoService,
        _obraService,
        _propostaService,
        _intervenienteService, 
        null,
        { proposta: null, pagamentoSelecionado: null}
        );

      var temDireito = _userService.temDireitoAplicativo('WEB6999','', 200);
      if (temDireito) return;
    }

}

