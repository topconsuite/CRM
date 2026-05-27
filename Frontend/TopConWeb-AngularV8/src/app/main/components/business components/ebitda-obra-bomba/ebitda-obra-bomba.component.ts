import { Component, Input, OnInit } from '@angular/core';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';
import { ObraBomba } from 'app/classes/obra/obra-bomba';
import { Tasks } from 'app/classes/_tasks/tasks';

@Component({
  selector: 'ebitda-obra-bomba',
  templateUrl: './ebitda-obra-bomba.component.html',
  styleUrls: ['./ebitda-obra-bomba.component.scss']
})
export class EbitaObraBombaComponent implements OnInit {

  @Input() obraBomba: ObraBomba;
  @Input() custoServico: CustoServico;
  @Input() quantidadeBombeada: number;
  
  constructor() { }

  ngOnInit() {
  }

  formataData = Tasks.formataData;
  formataMoeda = Tasks.formataMoeda;
  convertMinutosParaHora = Tasks.convertMinutosParaHora;


  get isSmallScreen(): boolean {
    return (window.innerWidth <= 600);
  }
}
