import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef
  , ContentChildren, QueryList } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';

import { SimpleInputComponent } from '../../input/simple-input/simple-input.component';
import { CnpjInputComponent } from '../../input/cnpj-input/cnpj-input.component';
import { CpfInputComponent } from '../../input/cpf-input/cpf-input.component';
import { DatePickerComponent } from '../../input/date-picker/date-picker.component';
import { DecimalInputComponent } from '../../input/decimal-input/decimal-input.component';
import { DecimalNegativeInputComponent } from '../../input/decimal-negative-input/decimal-negative-input.component';
import { IntegerInputComponent } from '../../input/integer-input/integer-input.component';
import { PhoneInputComponent } from '../../input/phone-input/phone-input.component';
import { SelectComboComponent } from '../../input/select-combo/select-combo.component';
import { SuggestComboComponent } from '../../input/suggest-combo/suggest-combo.component';
import { UserService } from 'app/services/user.service';
import { UsuarioWebFiltro } from 'app/classes/usuario/usuario-web-filtro';

@Component({
  selector: 'filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {

  @ContentChildren(SimpleInputComponent) simpleInputs: QueryList<SimpleInputComponent>;
  @ContentChildren(CnpjInputComponent) cnpjInputs: QueryList<CnpjInputComponent>;
  @ContentChildren(CpfInputComponent) cpfInputs: QueryList<CpfInputComponent>;
  @ContentChildren(DatePickerComponent) datePickerInputs: QueryList<DatePickerComponent>;
  @ContentChildren(DecimalInputComponent) decimalInputs: QueryList<DecimalInputComponent>;
  @ContentChildren(DecimalNegativeInputComponent) decimalNegativeInputs: QueryList<DecimalNegativeInputComponent>;
  @ContentChildren(IntegerInputComponent) integerInputs: QueryList<IntegerInputComponent>;
  @ContentChildren(PhoneInputComponent) phoneInputs: QueryList<PhoneInputComponent>;
  @ContentChildren(SelectComboComponent) selectInputs: QueryList<SelectComboComponent>;
  @ContentChildren(SuggestComboComponent) suggestInputs: QueryList<SuggestComboComponent>;

  private _defaultModel = null;
  get defaultModel(): any {
    if (!this._defaultModel) this._defaultModel = JSON.parse(JSON.stringify(this.model));
    return this._defaultModel;
  }

  @Input() model: any = {};
  @Input() application: string = "";
  @Input() defaultFilter: string = "";
  @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();

  @Output() filterChange: EventEmitter<String> = new EventEmitter<String>();

  form: FormGroup;

  @Input() isOpen: boolean = false;
  @Output() isOpenChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() isClear: boolean = true;
  @Output() isClearChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  private _dialogRef: MatDialogRef<any> = null;

  get inputs(): any[] {
    let _inputs: any[] = [];

    if (this.simpleInputs) this.simpleInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.cnpjInputs) this.cnpjInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.cpfInputs) this.cpfInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.datePickerInputs) this.datePickerInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.decimalInputs) this.decimalInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.decimalNegativeInputs) this.decimalNegativeInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.integerInputs) this.integerInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.phoneInputs) this.phoneInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.selectInputs) this.selectInputs.forEach(input => {
      _inputs.push(input);
    })
    if (this.suggestInputs) this.suggestInputs.forEach(input => {
      _inputs.push(input);
    })

    return _inputs;
  }

  constructor(
    public dialog: MatDialog,
    private _formBuilder: FormBuilder,
    private _cdr: ChangeDetectorRef,
    private _userService: UserService
  ) {
    this.form = this._formBuilder.group({});
  }

  ngOnInit() {
    this.getApplicationFilter();
  }
  
  get filterButtonColor(): string {
    return JSON.stringify(this.defaultModel)==JSON.stringify(this.model)||!this.defaultModel ? 'black' : 'blue';
  }

  get filteredString(): string {
    const operators: string[] = ['==','>>','>=','<<','<=','%='];
    const operatorIndex = 0;
    const nameIndex = 1;
    let f = '';

    //this._cdr.detectChanges();
    this.inputs.forEach(input => {
      let operatorAndNameArray: string[] = input.name.split('|');
      if (input.model && operatorAndNameArray.length >= 2 && operators.includes(operatorAndNameArray[operatorIndex])) {
          var name = '';

          for (let i = nameIndex; i < operatorAndNameArray.length; i++) {
            if (operatorAndNameArray[i]){
              name = name + '|' + operatorAndNameArray[i];
            }
          }

          if (name) f = f + ';' + name.substr(1) + operatorAndNameArray[operatorIndex] + input.model;
      }
    });

    if (f) f = 'filter=$(' + f.substr(1) + ')';

    return f;
  }

  aplyFilter() {
    this.modelChange.emit(this.model);
    this.filterChange.emit(this.filteredString);
    this.isOpen = false;
    this.isOpenChange.emit(false);
    this.isClear = false;
    this.isClearChange.emit(false);
    this.saveApplicationFilter(JSON.stringify(this.model));
    if (this._dialogRef) this._dialogRef.close();
  }

  aplyFilterInit(filterString: string) {
    this.modelChange.emit(this.model);
    this.filterChange.emit(filterString);
    this.isOpen = false;
    this.isOpenChange.emit(false);
    this.isClear = false;
    this.isClearChange.emit(false);
    if (this._dialogRef) this._dialogRef.close();
  }

  clearFilter() {
    this.inputs.forEach(input => {

      let typeModel = input.modelDdd !== undefined ? typeof(input.modelDdd) : typeof(input.model);

      switch (typeModel) {
        case 'string':
          input.model = '';
          break;
        case 'number':
          if (input.modelDdd !== undefined) {
            input.modelDdd = 0;
            input.modelNumber = 0;
          } else input.model = 0;
          break;
        default:
          if (input instanceof SuggestComboComponent) {
            input.input.nativeElement.value = '';
          }
          input.model = null;
          break;
      }

    });

    Object.assign(this.model, this.defaultModel);

    this.modelChange.emit(this.model);
    this._cdr.detectChanges(); 
    this.saveApplicationFilter(JSON.stringify(this.model), true);
    
  }

  openFilter(template) {
    this.isOpen = true;
    this.isOpenChange.emit(true);
    this._dialogRef = this.dialog.open(template);
    this._cdr.detectChanges();
  }

  saveApplicationFilter(json: string, isAfterClear: boolean = false) {
    if (!this.application) {
      return;
    }

    var _filter = new UsuarioWebFiltro();
    _filter.aplicativo = this.application;
    _filter.json = json;
    _filter.filterString = !isAfterClear ? this.filteredString : this.defaultFilter;

    this._userService.salvarFiltro(_filter, true).then();
    
    if (isAfterClear) {
      this.modelChange.emit(this.model);
      this._cdr.detectChanges();
      this.filterChange.emit(_filter.filterString);
      this.isOpen = false;
      this.isOpenChange.emit(false);
      this.isClear = true;
      this.isClearChange.emit(true);
      if (this._dialogRef) {
          this._dialogRef.close();
      }
    }
  }

  getApplicationFilter() {
    try {
      if (!this.application) {
        Object.assign(this.model, this.defaultModel);
        this.aplyFilterInit(this.defaultFilter);
        return;
      }

      this._userService.obterFiltro(this.application).then(
        filtro => {

          if (filtro && filtro.json) {
            Object.assign(this.model, JSON.parse(filtro.json));
            this.aplyFilterInit(filtro.filterString);
          } else {
            Object.assign(this.model, this.defaultModel);
            this.aplyFilterInit(this.defaultFilter);
          }

          this._cdr.detectChanges(); 
        }
      ).catch(err => {
        Object.assign(this.model, this.defaultModel);
        this.aplyFilterInit(this.defaultFilter);
        console.error("Erro crítico em getApplicationFilter:", err);
      });

    } catch (error) {
      console.error("Erro crítico em getApplicationFilter:", error);
    }
  }

}
