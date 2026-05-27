import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { UsuarioWebFiltro } from 'app/classes/usuario/usuario-web-filtro';
import { UserService } from 'app/services/user.service';

export interface ICustomView {
  key: string;
  value: any;
}

@Component({
  selector: 'view-selector',
  templateUrl: './view-selector.component.html',
  styleUrls: ['./view-selector.component.scss']
})
export class ViewSelectorComponent implements OnInit, AfterViewInit {
  
  newViewValue: ICustomView = { key: 'Nova visualização', value: undefined };

  @Input() key: string;
  @Input() currentValue: any;

  @Output() selectedChange: EventEmitter<ICustomView> = new EventEmitter<ICustomView>();

  defaultKey: string;
  selected: ICustomView;

  list: ICustomView[] = [];

  constructor(
    private _userService: UserService
  ) { }

  ngOnInit() {
    this.setSelected(this.newViewValue);
  }
  ngAfterViewInit(): void {
    this.getList();
  }

  format(element: ICustomView): string {
    return element.key;
  }

  setSelected(newValue: ICustomView) {
    this.selected = newValue;
    this.selectedChange.emit(JSON.parse(JSON.stringify(newValue)));
  }

  saveList() {
    var _filter = new UsuarioWebFiltro();
    _filter.aplicativo = `VIEWS-${this.key}`
    _filter.json = JSON.stringify(this.list);
    _filter.filterString = this.defaultKey;

    this._userService.salvarFiltro(_filter, true);

  }
  getList() {
    this._userService.obterFiltro(`VIEWS-${this.key}`).then(
      view => {
        if(view) {
          this.list = JSON.parse(view.json || '[]');
          this.defaultKey = view.filterString;
        } else {
          this.list = JSON.parse('[]');
          this.defaultKey = "";
        }

        let d = this.list.find(t => t.key === this.defaultKey.trim());

        if (this.defaultKey && d)
          this.setSelected(d);
        
      }
    )
  }
  

  deleteDefault() {
    var _filter = new UsuarioWebFiltro();
    _filter.aplicativo = `VIEWS-${this.key}`
    _filter.json = JSON.stringify(this.list);
    _filter.filterString = "";
    this._userService.salvarFiltro(_filter, true);
  }

  save() {
    if (this.newViewValue.key === this.selected.key) {
      let newKey = prompt("Digite o nome da nova visualização:") || '';

      if (newKey.trim() === "") {
        alert("Obrigatório informar um nome!");
      } else if (this.list.map(t => t.key).includes(newKey.trim())) {
        alert("Já existe uma visualização com este nome!");
      } else {
        let newItem: ICustomView = { key: newKey, value: JSON.parse(JSON.stringify(this.currentValue)) };
        this.list.push(newItem);
        this.saveList();
        this.setSelected(newItem);
      }
    } else {
      this.selected.value = JSON.parse(JSON.stringify(this.currentValue));
      this.list.filter(t => t.key === this.selected.key)[0].value = this.selected.value;
      this.saveList();
    }
  }

  delete() {
    let confirmed = confirm(`Confirma exclusão da visualização '${this.selected.key}'?`);

    if (!confirmed)
      return;
    
    this.list = this.list.filter(t => t.key !== this.selected.key);
    this.saveList();
    this.setSelected(this.newViewValue);
  }

  saveDefault(value: boolean) {
    if (value) {
      this.defaultKey = this.selected.key;
      this.saveList();
    }
    else {
      this.defaultKey = '';
      this.deleteDefault();
    }
  }

  get isDefaultSelected(): boolean {
    if (!this.selected || !this.defaultKey) return false;
    return (this.selected.key === this.defaultKey);
  }

  get sortedList(): ICustomView[] {
    return this.list.sort((a, b) => a.key.localeCompare(b.key));
  }

}
