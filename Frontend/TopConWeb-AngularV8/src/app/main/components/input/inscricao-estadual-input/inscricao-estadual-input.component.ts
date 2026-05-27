import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { InputBaseComponent } from './../input-base/input-base.component';
import { IntervenienteService } from 'app/services/interveniente.service';
import { ICustomValidator } from '../../interfaces/custom-validator';

@Component({
  selector: 'inscricao-estadual-input',
  templateUrl: './inscricao-estadual-input.component.html',
  styleUrls: ['./inscricao-estadual-input.component.scss']
})
export class InscricaoEstadualInputComponent extends InputBaseComponent implements OnInit {

  public static self: InscricaoEstadualInputComponent;
  
  isValid: boolean = false;

  valorAnterior: any;

  constructor(
      private _intervenienteService: IntervenienteService,
      private _cdr: ChangeDetectorRef
    ) {
    super();
    InscricaoEstadualInputComponent.self = this;
  }

  ngOnInit() {
  }

  setModel(newModel: any) {
    newModel = this.sanitize(newModel);
    this.modelChange.emit(newModel);
  };

  onFocus() {
    this.focus.emit();
  }
  onFocusout() {
    this.model = this.sanitize(this.model);
    this.focusout.emit();
    this._cdr.detectChanges();
  }

  sanitize(value) {
    var sanitized = (value);
    
    if (typeof(value) === 'string') {
      sanitized = this.replaceAll(sanitized, "'", "").trim();
      sanitized = this.replaceAll(sanitized, ".", "").trim();
      sanitized = this.replaceAll(sanitized, "-", "").trim();
    }

    return sanitized;
  }

  get inscricaoEstadual(): ICustomValidator {
    var _self = InscricaoEstadualInputComponent.self;

    return {
      key: 'ieInvalida',
      message: 'Inscrição estadual inválida',
      validatorFunction: (inscricaoEstatual: string): boolean => {
        if (inscricaoEstatual.trim() === '' && inscricaoEstatual.length > 0) {
          return true;
        } else if (this.valorAnterior !== inscricaoEstatual) {
          this.valorAnterior = inscricaoEstatual;
          _self._intervenienteService.inscricaoEstadualValida(inscricaoEstatual, true)
            .then(valid => {
              this.isValid = valid;
              _self.formGroup.controls[_self.name].updateValueAndValidity();
            }, err => {
              this.isValid = false;
              _self.formGroup.controls[_self.name].updateValueAndValidity();
            });
        }
        
        return !this.isValid;
      },
      params: [_self.model]
    }
  }

}
