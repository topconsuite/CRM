import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from 'app/services/user.service';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { Tasks } from 'app/classes/_tasks/tasks';
import { ContratoService } from 'app/services/contrato.service';
import { VersionamentoContratoParametro } from 'app/classes/versionamento-contrato/versionamento-contrato-parametro'

import { ParametroService } from 'app/services/parametro.service';
import { parse } from 'querystring';


@Component({
  selector: 'app-versionamento-contrato-page',
  templateUrl: './versionamento-contrato-page.component.html',
  styleUrls: ['./versionamento-contrato-page.component.scss']
})
export class VersionamentoContratoPageComponent implements OnInit, AfterViewInit {

  versionamentoContratoForm: FormGroup;
  versionamentoContratoParametro: VersionamentoContratoParametro = new VersionamentoContratoParametro();

  constructor(
    private _formBuilder: FormBuilder,
    private _parametroService: ParametroService,
    private _contratoService: ContratoService,
    private _dialog: MatDialog,
    private _userService: UserService,) { 

      var temDireito = this._userService.temDireitoAplicativo('WEB7000','', 200);
      if (!temDireito) return;  
    }

  ngAfterViewInit(): void {
  }

  ngOnInit() {   
    this.versionamentoContratoForm = this._formBuilder.group({
      versionamentoTraco: [''],
      versionamentoBomba: [''],
      versionamentoTaxaExtra: [''],
      versionamentoCondicaoPagamento: [''],
      versionamentoEnderecoObra: [''],
      versionamentoDemaisServicos: [''],
      versionamentoReajusteContrato: ['']
    });   

    this._contratoService.ObterContratoVersaoParametro().then(
      parametro => { 
        if (parametro !== null) this.versionamentoContratoParametro = parametro; },
      error => { this.versionamentoContratoParametro = new VersionamentoContratoParametro(); }
    );
  }

  formataErrosApi = Tasks.formataErrosApi;


  salvarConfiguracao() {
    if(this.versionamentoContratoForm.invalid) return;
    
    this._parametroService.Atualizar("web", "VersionamentoTraco", this.versionamentoContratoForm.get('versionamentoTraco').value);
    this._parametroService.Atualizar("web", "VersionamentoBomba", this.versionamentoContratoForm.get('versionamentoBomba').value);
    this._parametroService.Atualizar("web", "VersionamentoTaxaExtra", this.versionamentoContratoForm.get('versionamentoTaxaExtra').value);
    this._parametroService.Atualizar("web", "VersionamentoCondicaoPagamento", this.versionamentoContratoForm.get('versionamentoCondicaoPagamento').value);
    this._parametroService.Atualizar("web", "VersionamentoEnderecoObra", this.versionamentoContratoForm.get('versionamentoEnderecoObra').value);
    this._parametroService.Atualizar("web", "VersionamentoDemaisServicos", this.versionamentoContratoForm.get('versionamentoDemaisServicos').value)
    this._parametroService.Atualizar("web", "VersionamentoReajusteContrato", this.versionamentoContratoForm.get('versionamentoReajusteContrato').value)
    .then(
      success => {
      this._dialog.open(AlertDialogComponent, {
        disableClose: true,
        data: {
          title: 'TopConWeb',
          message: `Parametros atualizados com sucesso!`
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
}
