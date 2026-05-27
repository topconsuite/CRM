import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Usina } from 'app/classes/usina/usina';
import { ObraTributacaoMunicipal } from 'app/classes/obra/obra.classes';

@Component({
  selector: 'app-obra-tributacoes-municipais-dialog',
  templateUrl: './obra-tributacoes-municipais-dialog.component.html',
  styleUrls: ['./obra-tributacoes-municipais-dialog.component.scss']
})
export class ObraTributacoesMunicipaisDialogComponent implements OnInit {

  tributacoesMunicipaisForm: FormGroup;

  tributacoesISS: number[] = [0, 1];
  opcoesRetencaoISS: string[] = ['S', 'N', 'X'];


  constructor(
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ObraTributacoesMunicipaisDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      disabled: boolean,
      tributacoesMunicipais: ObraTributacaoMunicipal[],
      usinas: Usina[],
      confirmTributacoesMunicipaisCallback: Function,
      cancelTributacoesMunicipaisCallback?: Function
    }
  ) { }

  ngOnInit() {
    this.tributacoesMunicipaisForm = this._formBuilder.group({});
  }

  alteracaoNaoPermitida(): boolean {
    return this.data.disabled;
  }

  alteracaoRetencaoIssNaoPermitida(item: ObraTributacaoMunicipal): boolean {
    return item.tributacaoISS === 1;
  }

  confirmTributacoesMunicipais() {
    if (this.data.confirmTributacoesMunicipaisCallback(this.data.tributacoesMunicipais)) {
      this.dialogRef.close();
    }
  }

  cancelTributacoesMunicipais() {
    this.dialogRef.close();
    if (this.data.cancelTributacoesMunicipaisCallback) this.data.cancelTributacoesMunicipaisCallback();
  }

  usinasCodigos(): number[] {
    var codigos = this.data.usinas.map(t => t.codigo);
    codigos.push(0);
    return codigos;
  }

  getUsina(codigo: number): Usina {
    if (codigo === 0) return { codigo: 0, nome: 'DEFAULT', sigla: 'DFT', filialCodigo: 0, tempoBtAteAObra:0, tempoBtNaObra:0,porcentagemRetornoObra:0 ,prazoPesagem:0 };
    return this.data.usinas.filter(t => t.codigo === codigo)[0];
  }

  usinaPorCodigoFormatter = (model: number): string => {
    var usina = model || model === 0 ? this.getUsina(model): null;
    return usina ? (usina.codigo+' - '+usina.nome).toUpperCase() : ' - ';
  }
  tributacaoIssFormatter = (model: number): string => {
    switch (model) {
      case 0:
        return 'NORMAL';
      case 1:
        return 'ISENTO';
    }
  }
  retencaoIssFormatter = (model: string): string => {
    switch (model) {
      case 'S':
        return 'SIM';
      case 'N':
        return 'NÃO';
      case 'X':
        return 'REGRA MUNICÍPIO';
    }
  }

  
  changeTributacaoISS(newValue: number, tributacaoMunicipalItem: any) {
    if (newValue == 1)
      tributacaoMunicipalItem.retencaoISS = 'X';
      
    tributacaoMunicipalItem.tributacaoISS = newValue;
  }
  

  addTributacaoMunicipal() {
    this.data.tributacoesMunicipais.push(new ObraTributacaoMunicipal());
  }

  removeTributacaoMunicipal(tributacaoMunicipal: ObraTributacaoMunicipal): void {
    this.data.tributacoesMunicipais = this.data.tributacoesMunicipais.filter(t => t != tributacaoMunicipal);
  }

}
