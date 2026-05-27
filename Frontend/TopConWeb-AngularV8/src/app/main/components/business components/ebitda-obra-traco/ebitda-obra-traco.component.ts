import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';
import { Obra } from 'app/classes/obra/obra';
import { ObraTraco } from 'app/classes/obra/obra-traco';
import { Usina } from 'app/classes/usina/usina';
import { Tasks } from 'app/classes/_tasks/tasks';

@Component({
  selector: 'ebitda-obra-traco',
  templateUrl: './ebitda-obra-traco.component.html',
  styleUrls: ['./ebitda-obra-traco.component.scss']
})
export class EbitaObraTracoComponent implements OnInit {

  @Input() obraTraco: ObraTraco;
  @Input() custoServico: CustoServico;
  @Input() usinaEntrega: Usina;
  @Input() obra: Obra;

  constructor(private _cdr: ChangeDetectorRef) { }

  dataAtual: Date;

  ngOnInit() {
    this.dataAtual = new Date();
  }

  get getVolumePorCarga(): number {
    if(this.obra === null || this.obra === undefined) return 8;
    return this.obra.volumePorCarga == 0 ? 8 : this.obra.volumePorCarga;
  } 

  get getTempoCicloPrevisto(): number {
    if(this.obra === null || this.obra === undefined) return 0;
    if(this.usinaEntrega === null || this.usinaEntrega === undefined) return 0;

    var tempoBtNaObra = this.obra.tempoBtNaObra == 0 ? this.usinaEntrega.tempoBtNaObra : this.obra.tempoBtNaObra;

    return Math.floor(tempoBtNaObra + this.tempoDescolocamentoEDosagem)
  }
  
  get tempoDescolocamentoEDosagem(): number {
    if (this.obra === null || this.obra === undefined) return 0;

    var tempoAteObra = this.obra.tempoAteAObra == 0 ? this.usinaEntrega.tempoBtAteAObra : this.obra.tempoAteAObra;
    return Math.floor(this.usinaEntrega.prazoPesagem  + tempoAteObra + (tempoAteObra * this.usinaEntrega.porcentagemRetornoObra/100))
  }

  get getTempoBtNaObra(): number {
    if(this.obra === null || this.obra === undefined) return 0;
    if(this.usinaEntrega === null || this.usinaEntrega === undefined) return 0;
    return this.obra.tempoBtNaObra === 0 ? this.usinaEntrega.tempoBtNaObra : this.obra.tempoBtNaObra;
  } 

  formataData = Tasks.formataData;
  formataMoeda = Tasks.formataMoeda;
  convertMinutosParaHora = Tasks.convertMinutosParaHora;  
  formataValor = Tasks.formataValor;

  get getDemaisCustosPrevistosObraTracoEbitda(): number {
    if (this.custoServico === null || this.custoServico === undefined) return 0;
    return this.custoServico.custoMedioProducao + this.custoServico.custoMedioLaboratorio + this.custoServico.custoMedioComercial + this.custoServico.custoMedioAdministrativo;
  }
  
  calculaMargemPosMCC(obraTraco: ObraTraco): number {
    if (obraTraco === null || obraTraco === undefined) return 0;
    var valorServicoAtual = obraTraco.custoServicoReajustado === 0 || this.isDateGreaterOrEqual(obraTraco.dataUltimoReajuste, this.dataAtual, false) ? obraTraco.valorServico : obraTraco.custoServicoReajustado;
    return valorServicoAtual - obraTraco.totalImpostos;
  }

  calculaPorcentagemMargemPosMCC(obraTraco: ObraTraco): number {
    if (obraTraco === null || obraTraco === undefined) return 0;
    var valorServicoAtual = obraTraco.custoServicoReajustado === 0 || this.isDateGreaterOrEqual(obraTraco.dataUltimoReajuste, this.dataAtual, false) ? obraTraco.valorServico : obraTraco.custoServicoReajustado;
    return ((valorServicoAtual - obraTraco.totalImpostos) * 100) / obraTraco.m3PrecoProposto;
  }

  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }

  isDateGreaterOrEqual(date1: string | Date, date2: Date, currentDate: boolean): boolean {
    const d1 = new Date(date1);
    const d2 = new Date(date2);
    // Para comparar apenas a data, ignorando a hora:
    d1.setHours(0, 0, 0, 0);
    d2.setHours(0, 0, 0, 0);

    if (currentDate)
      return d1 >= d2;
    else
      return d1 > d2;
  }

}
