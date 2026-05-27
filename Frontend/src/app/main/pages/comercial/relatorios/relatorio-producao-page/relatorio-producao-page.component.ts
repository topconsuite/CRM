import { Component, OnInit, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material';

import { Tasks } from 'app/classes/_tasks/tasks';
import { CadastroGeral, CadastroGeralViaCaptacao } from 'app/classes/cadastro-geral/cadastro-geral';
import { Vendedor } from 'app/classes/vendedor/vendedor';
import { Usina } from 'app/classes/usina/usina';
import { Interveniente } from 'app/classes/interveniente/interveniente';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';

import { UsinaService } from 'app/services/usina.service';
import { VendedorService } from 'app/services/vendedor.service';
import { IntervenienteService } from 'app/services/interveniente.service';
import { RelatorioService } from 'app/services/relatorio.service';
import { UserService } from 'app/services/user.service';
import { SegmentacaoService } from 'app/services/segmentacao.service';
import { CadastroGeralService } from 'app/services/cadastro-geral.service';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';
import { ContratoService } from 'app/services/contrato.service';
import { GrupoEconomico } from 'app/classes/grupo-economico/grupo-economico';
import { GrupoEconomicoService } from 'app/services/grupo-economico.service';

enum RelatorioProducaoModelo {
  Analitico = 1,
  Sintetico = 2,
  PorProgramacao = 3,
  Volume = 4
}

@Component({
  selector: 'app-relatorio-producao-page',
  templateUrl: './relatorio-producao-page.component.html',
  styleUrls: ['./relatorio-producao-page.component.scss']
})
export class RelatorioProducaoPageComponent implements OnInit, AfterViewInit {

  modelos: CadastroGeral[] = [
    { codigo: RelatorioProducaoModelo.Analitico , descricao: 'Analítico NF', viaCaptacao: new CadastroGeralViaCaptacao() },
    { codigo: RelatorioProducaoModelo.Sintetico , descricao: 'Sintético por Cliente', viaCaptacao: new CadastroGeralViaCaptacao() },
    { codigo: RelatorioProducaoModelo.PorProgramacao , descricao: 'Analítico por Programação', viaCaptacao: new CadastroGeralViaCaptacao() },
    { codigo: RelatorioProducaoModelo.Volume , descricao: 'Volume Contratado e Entregue', viaCaptacao: new CadastroGeralViaCaptacao() }
  ];

  usinas: Usina[] = [];
  vendedoresPermitidos: Vendedor[] = [];
  vendedoresVinculados: Vendedor[] = [];
  clientes: Interveniente[] = [];
  segmentacao: Segmentacao[] = [];
  finalidades: ContratoFinalidade[] = [];
  viasCaptacao: CadastroGeral[] = [];

  gruposEconomicos: GrupoEconomico[] = [];

  modelo: CadastroGeral = this.modelos[0];
  clienteRazao: string = '';

  filtroString: string = '';
  filtro: {
    usina: Usina,
    cliente: Interveniente,
    dataDe: Date,
    dataAte: Date,
    vendedor: Vendedor,
    vendedorPadrinho: Vendedor,
    anoContrato: number,
    numContrato: number,
    segmentacao: Segmentacao,
    contratoFinalidade: ContratoFinalidade,
    viaCaptacao: CadastroGeral,
    grupoEconomico: GrupoEconomico,
    grupoEconomicoDescricao: string
  } = {
    usina: null,
    cliente: null,
    dataDe: null,
    dataAte: null,
    vendedor: null,
    vendedorPadrinho: null,
    anoContrato: 0,
    numContrato: 0,
    segmentacao: null,
    contratoFinalidade: null,
    viaCaptacao: null,
    grupoEconomico: null,
    grupoEconomicoDescricao: ''
  };

  relatorioForm: FormGroup;

  constructor(
    public dialog: MatDialog,
    private _formBuilder: FormBuilder,
    private _cdr: ChangeDetectorRef,
    private _userService: UserService,
    private _usinaService: UsinaService,
    private _vendedorService: VendedorService,
    private _intervenienteService: IntervenienteService,
    private _notaFiscalFisicaService: RelatorioService,
    private _segmentacaoService: SegmentacaoService,
    private _cadastroGeralService: CadastroGeralService,
    private _contratoService: ContratoService,
    private _grupoEconomicoService: GrupoEconomicoService
  ) {
    var temDireito = this._userService.temDireitoAplicativo('WEB6301','', 50);
    if (!temDireito) return;

    this._userService.gravarAcessoAplicacao("Comercial", 6301);

    _usinaService.listarTodos().then(
      usinas => { this.usinas = usinas },
      error => { this.usinas = [] }
    );
    _vendedorService.listarTodosPermitidos().then(
      vendedores => { this.vendedoresPermitidos = vendedores },
      error => { this.vendedoresPermitidos = [] }
    );
    _vendedorService.listarVinculados().then(
      vendedores => {
        this.vendedoresVinculados = vendedores;
        this.setVendedorVinculado();
        this._cdr.detectChanges();
      },
      error => { this.vendedoresVinculados = [] }
    );

    this._segmentacaoService.listarTodos().then(
      segmentacoes => { this.segmentacao = segmentacoes; },
      error => { this.segmentacao = []; }
    );
    this._cadastroGeralService.listarViasCaptacao().then(
      viasCaptacao => { this.viasCaptacao = viasCaptacao },
      error => { this.viasCaptacao = [] }
    );

    this._contratoService.ListarFinalidades().then(
      lista => { this.finalidades = lista },
      error => { this.finalidades = [] }
    )
  }

  ngOnInit() {
    this.relatorioForm = this._formBuilder.group({
      detalharAdicionais: [false],
      detalharViaCaptacaoIndicadores: [false]
    });
  }
  ngAfterViewInit(): void {
    this.filtro.dataDe = Tasks.addDays(Tasks.dataAtual(), -30);
    this.filtro.dataAte = Tasks.dataAtual();
    this._cdr.detectChanges();
  }

  cadastroGeralFormatter = (model: CadastroGeral): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';
  usinaFormatter = (model: Usina): string => model ? (model.codigo+' - '+model.nome).toUpperCase() : '';
  intervenienteFormatter = (model: Interveniente) => model ? model.codigo+' - '+model.razao.toUpperCase() : '';
  vendedorFormatter = (model: Vendedor) => model ? model.codigo+' - '+model.nome.toUpperCase() : '';
  segmentacaoFormatter = (model: Segmentacao) => model ? model.id+' - '+model.nome.toUpperCase() : '';
  finalidadeFormatter = (model: ContratoFinalidade): string => model ? (model.codigo+' - '+model.descricao).toUpperCase() : '';

  grupoEconomicoFormatter = (model: GrupoEconomico) => model ? model.codigo+' - '+model.descricao.toUpperCase() : '';

  montarStringFiltro(): string {
    var filtros = [];

    if (this.filtro.usina) filtros.push(`usina==${this.filtro.usina.codigo}`);
    if (this.filtro.cliente) filtros.push(`cliente==${this.filtro.cliente.codigo}`);
    if (this.filtro.dataDe) filtros.push(`dataDe==${this.filtro.dataDe.toISOString().substr(0, 10)}`);
    if (this.filtro.dataAte) filtros.push(`dataAte==${this.filtro.dataAte.toISOString().substr(0, 10)}`);
    
    if (this.filtro.vendedor) {
      filtros.push(`vendedor==${this.filtro.vendedor.codigo}`);
    } else {
      filtros.push(`vendedorIn==${this.vendedoresPermitidos.map(t => t.codigo).join(',')}`);
    }

    if (this.filtro.vendedorPadrinho) {
      filtros.push(`vendedorPadrinho==${this.filtro.vendedorPadrinho.codigo}`);
    }

    if (this.filtro.anoContrato) {
      filtros.push(`anoContrato==${this.filtro.anoContrato}`);
    }

    if (this.filtro.numContrato) {
      filtros.push(`numContrato==${this.filtro.numContrato}`);
    }

    if (this.filtro.segmentacao) {
      filtros.push(`segmentacao==${this.filtro.segmentacao.id}`);
    }

    if(this.filtro.contratoFinalidade) {
      filtros.push(`contratoFinalidade==${this.filtro.contratoFinalidade.codigo}`);
    }

    if (this.filtro.viaCaptacao) {
      filtros.push(`viaCaptacao==${this.filtro.viaCaptacao.codigo}`);
    }

    if (this.filtro.grupoEconomico) {
      filtros.push(`grupoEconomico==${this.filtro.grupoEconomico.codigo}`);
    }

    var filter = btoa(`$(${filtros.join(';')})`);

    return `filter=${filter}`;
  }

  imprimir(): void {
    var filtro = this.montarStringFiltro();
    var urlReport: string = '';
    let valorDetalharAdicionais = this.relatorioForm.get('detalharAdicionais').value;
    let valorDetalharViaCaptacaoIndicadores = this.relatorioForm.get('detalharViaCaptacaoIndicadores').value;

    if (valorDetalharAdicionais == undefined)
      valorDetalharAdicionais = false;

    if(valorDetalharViaCaptacaoIndicadores == undefined)
      valorDetalharViaCaptacaoIndicadores = false;
    
    switch (this.modelo.codigo) {
      case RelatorioProducaoModelo.Analitico:
        urlReport = this._notaFiscalFisicaService.ObterUrlRelatorioProducaoAnalitico(filtro, valorDetalharAdicionais, valorDetalharViaCaptacaoIndicadores);
        break;
      case RelatorioProducaoModelo.Sintetico:
        urlReport = this._notaFiscalFisicaService.ObterUrlRelatorioProducaoSintetico(filtro);
        break;
      case RelatorioProducaoModelo.PorProgramacao:
        urlReport = this._notaFiscalFisicaService.ObterUrlRelatorioProducaoPorProgramacao(filtro, valorDetalharViaCaptacaoIndicadores);
        break;
      case RelatorioProducaoModelo.Volume:
        urlReport = this._notaFiscalFisicaService.ObterUrlRelatorioProducaoVolume(filtro);
        break;
      default:
        return;
    }
    
    Tasks.openPdf(urlReport);
  }

  setVendedorVinculado() {
    if (this.vendedoresVinculados.length===1) {
      this.filtro.vendedor = this.vendedoresVinculados[0];
    }
  }

  private _timeoutIntervenientesPorRazao = null;
  filtrarIntervenientesPorRazao(cliente: string) {
    this.clienteRazao = cliente;

    var tamanhoMinimo = (isNaN(parseInt(cliente)) ? 3 : 0);

    if (cliente && cliente.length>tamanhoMinimo && (!this.filtro.cliente || this.filtro.cliente.razao!=cliente)) {
      
      if (this._timeoutIntervenientesPorRazao) clearTimeout(this._timeoutIntervenientesPorRazao);
      
      var filtro = 'filter=$(' + (isNaN(parseInt(cliente)) ? 'razao%=' + cliente : 'codigo==' + parseInt(cliente));

      if (this.filtro.grupoEconomico)
        filtro += ';grupoEconomicoCodigo==' + this.filtro.grupoEconomico.codigo;

      filtro += ')';

      this._timeoutIntervenientesPorRazao = setTimeout( () => {
        this._intervenienteService.listarFiltrados(filtro, true)
          .then(
            intervenientes => { this.clientes = intervenientes; },
            error => { this.clientes = []; }
          )
      }, 500);

    } else {
      this.clientes = [];
    }
  }
  
  private _timeoutGrupoEconomicoPorDescricao = null;
  filtrarGrupoEconomicoPorDescricao(grupoEconomico: string) {
    this.filtro.grupoEconomicoDescricao = grupoEconomico;

    var tamanhoMinimo = (isNaN(parseInt(grupoEconomico)) ? 3 : 0);

    if (grupoEconomico && grupoEconomico.length>tamanhoMinimo && (!this.filtro.grupoEconomico || this.filtro.grupoEconomico.descricao!=grupoEconomico)) {
      
      if (this._timeoutGrupoEconomicoPorDescricao) clearTimeout(this._timeoutGrupoEconomicoPorDescricao);
      
      var filtro = 'filter=$(' + (isNaN(parseInt(grupoEconomico)) ? 'descricao==' + grupoEconomico : 'codigo==' + parseInt(grupoEconomico)) + ')';

      this._timeoutGrupoEconomicoPorDescricao = setTimeout( () => {
        this._grupoEconomicoService.Listar(null, null, filtro)
          .then(
            gruposEconomicos => { this.gruposEconomicos = gruposEconomicos.records; },
            error => { this.gruposEconomicos = []; }
          )
      }, 500);

    } else {
      this.gruposEconomicos = [];
    }
  }
}
