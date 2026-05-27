import { NgModule } from '@angular/core';
import { MatAutocompleteModule, MatInputModule, MatSelectModule, MatDatepickerModule,
    MatButtonModule, MatIconModule, MatExpansionModule, MatTabsModule,
    MatTooltipModule, MatCardModule, MatRadioModule, MatMenuModule, MatCheckboxModule, MatDividerModule } from '@angular/material';

import { FuseSharedModule } from '@fuse/shared.module';

import { TextMaskModule } from 'angular2-text-mask';

import { InputBaseComponent } from './input/input-base/input-base.component';
import { SuggestComboComponent } from './input/suggest-combo/suggest-combo.component';
import { DatePickerComponent } from './input/date-picker/date-picker.component';
import { SelectComboComponent } from './input/select-combo/select-combo.component';
import { SimpleInputComponent } from './input/simple-input/simple-input.component';
import { CpfInputComponent } from './input/cpf-input/cpf-input.component';
import { CnpjInputComponent } from './input/cnpj-input/cnpj-input.component';
import { PhoneInputComponent } from './input/phone-input/phone-input.component';
import { IntegerInputComponent } from './input/integer-input/integer-input.component';
import { DecimalInputComponent } from './input/decimal-input/decimal-input.component';
import { AddressGroupComponent } from './input/address-group/address-group.component';
import { WizardComponent } from './wizard/wizard.component';
import { WizardStepComponent } from './wizard/wizard-step/wizard-step.component';
import { ConfirmDialogButtonComponent } from './dialog/confirm-dialog-button/confirm-dialog-button.component';
import { PaginatorComponent } from './list/paginator/paginator.component';
import { FilterComponent } from './list/filter/filter.component';
import { AlertDialogComponent } from './dialog/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from './dialog/confirm-dialog/confirm-dialog.component';
import { ObraLogDialogComponent } from './dialog/obra-log-dialog/obra-log-dialog.component';
import { RadioInputComponent } from './input/radio-input/radio-input.component';
import { ProgramacaoLogDialogComponent } from './dialog/programacao-log-dialog/programacao-log-dialog.component';
import { PropostaImportacaoDialogComponent } from './dialog/proposta-importacao-dialog/proposta-importacao-dialog.component';
import { InscricaoEstadualInputComponent } from './input/inscricao-estadual-input/inscricao-estadual-input.component';
import { ViewSelectorComponent } from './list/view-selector/view-selector.component';
import { PropostaProgramacoesDialogComponent } from './dialog/proposta-programacoes-dialog/proposta-programacoes-dialog.component';
import { MovimentosBancoAVincularDialogComponent } from './dialog/movimentos-banco-avincular-dialog/movimentos-banco-avincular-dialog.component';
import { PdfViewerDialogComponent, SafePipe } from './dialog/pdf-viewer-dialog/pdf-viewer-dialog.component';
import { ObraTributacoesMunicipaisDialogComponent } from './dialog/obra-tributacoes-municipais-dialog/obra-tributacoes-municipais-dialog.component';
import { HistoricoIntervenienteDialogComponent } from './dialog/historico-interveniente-dialog/historico-interveniente-dialog.component';
import { EbitaObraTracoComponent } from './business components/ebitda-obra-traco/ebitda-obra-traco.component';
import { EbitaObraBombaComponent } from './business components/ebitda-obra-bomba/ebitda-obra-bomba.component';
import { SolicitacaoAssinaturaEletronicaDialogComponent } from './dialog/solicitacao-assinatura-eletronica-dialog/solicitacao-assinatura-eletronica-dialog/solicitacao-assinatura-eletronica-dialog.component';
import { DecimalNegativeInputComponent } from './input/decimal-negative-input/decimal-negative-input.component';
import { MonthYearPickerComponent } from './input/month-year-picker/month-year-picker.component';
import { HistoricoVisitaDialogComponent } from './dialog/historico-visita-dialog/historico-visita-dialog.component';
import { TarefaFormComponent } from './business components/tarefa-form/tarefa-form.component';
import { CompromissoFormComponent } from './business components/compromisso-form/compromisso-form.component';
import { InteracaoLeadDialogComponent } from './dialog/interacao-lead-dialog/interacao-lead-dialog.component';
import { InteracaoOportunidadeDialogComponent } from './dialog/interacao-oportunidade-dialog/interacao-oportunidade-dialog.component';
import { PropostaPropagandaComponent } from './business components/proposta-propaganda/proposta-propaganda.component';

@NgModule({
    declarations: [
        InputBaseComponent,
        SuggestComboComponent,
        SelectComboComponent,
        DatePickerComponent,
        MonthYearPickerComponent,
        SimpleInputComponent,
        CpfInputComponent,
        CnpjInputComponent,
        PhoneInputComponent,
        IntegerInputComponent,
        DecimalInputComponent,
        AddressGroupComponent,
        ConfirmDialogButtonComponent,
        WizardComponent,
        WizardStepComponent,
        PaginatorComponent,
        FilterComponent,
        AlertDialogComponent,
        ConfirmDialogComponent,
        ObraLogDialogComponent,
        RadioInputComponent,
        ProgramacaoLogDialogComponent,
        PropostaImportacaoDialogComponent,
        InscricaoEstadualInputComponent,
        ViewSelectorComponent,
        PropostaProgramacoesDialogComponent,
        MovimentosBancoAVincularDialogComponent,
        PdfViewerDialogComponent,
        SafePipe,
        ObraTributacoesMunicipaisDialogComponent,
        HistoricoIntervenienteDialogComponent,
        EbitaObraTracoComponent,
        EbitaObraBombaComponent,
        SolicitacaoAssinaturaEletronicaDialogComponent,
        DecimalNegativeInputComponent,
        MonthYearPickerComponent,
        HistoricoVisitaDialogComponent,
        TarefaFormComponent,
        CompromissoFormComponent,
        PropostaPropagandaComponent,
        InteracaoLeadDialogComponent,
        InteracaoOportunidadeDialogComponent,
        PropostaPropagandaComponent
    ],
    entryComponents: [
        AlertDialogComponent,
        ConfirmDialogComponent,
        ObraLogDialogComponent,
        ProgramacaoLogDialogComponent,
        PropostaImportacaoDialogComponent,
        PropostaProgramacoesDialogComponent,
        MovimentosBancoAVincularDialogComponent,
        PdfViewerDialogComponent,
        ObraTributacoesMunicipaisDialogComponent,
        HistoricoIntervenienteDialogComponent,
        SolicitacaoAssinaturaEletronicaDialogComponent,
        HistoricoVisitaDialogComponent,
        InteracaoLeadDialogComponent,
        InteracaoOportunidadeDialogComponent
    ],
    imports: [
        FuseSharedModule,
        TextMaskModule,
        MatAutocompleteModule, MatInputModule, MatSelectModule, MatDatepickerModule,
        MatButtonModule, MatIconModule, MatExpansionModule, MatTabsModule, MatTooltipModule,
        MatCardModule, MatRadioModule, MatMenuModule, MatCheckboxModule, MatDividerModule 
    ],
    exports: [
        SuggestComboComponent,
        SelectComboComponent,
        DatePickerComponent,
        MonthYearPickerComponent,
        SimpleInputComponent,
        CpfInputComponent,
        CnpjInputComponent,
        PhoneInputComponent,
        IntegerInputComponent,
        DecimalInputComponent,
        AddressGroupComponent,
        ConfirmDialogButtonComponent,
        WizardComponent,
        WizardStepComponent,
        PaginatorComponent,
        FilterComponent,
        AlertDialogComponent,
        ConfirmDialogComponent,
        ObraLogDialogComponent,
        RadioInputComponent,
        ProgramacaoLogDialogComponent,
        PropostaImportacaoDialogComponent,
        InscricaoEstadualInputComponent,
        ViewSelectorComponent,
        PropostaProgramacoesDialogComponent,
        MovimentosBancoAVincularDialogComponent,
        PdfViewerDialogComponent,
        ObraTributacoesMunicipaisDialogComponent,
        HistoricoIntervenienteDialogComponent,
        EbitaObraTracoComponent,
        EbitaObraBombaComponent,
        SolicitacaoAssinaturaEletronicaDialogComponent,
        DecimalNegativeInputComponent,
        HistoricoVisitaDialogComponent,
        TarefaFormComponent,
        CompromissoFormComponent,
        PropostaPropagandaComponent,
        InteracaoLeadDialogComponent,
        InteracaoOportunidadeDialogComponent
    ]
})
export class ComponentsModule
{
}
