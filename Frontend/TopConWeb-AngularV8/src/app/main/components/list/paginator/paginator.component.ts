import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';

import { PagedList } from 'app/classes/pagination/paged-list';

@Component({
  selector: 'paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss']
})
export class PaginatorComponent implements OnInit {

  @Input() pagedList: PagedList<any> = new PagedList<any>();
  @Input() pageSizeOptions: number[] = [10, 30, 50, 100, 200];
  @Input() currentPage: number = 1;
  @Input() pageSize: number = 30;
  @Input() showPageSizeSelection: boolean = true;

  @Output() pageChange: EventEmitter<{currentPage: number; pageSize: number}> =
    new EventEmitter<{currentPage: number; pageSize: number}>();

  constructor(private _cdr: ChangeDetectorRef) { }

  ngOnInit() {
  }

  get pageInfoLabel(): string {
    if (this.isSmallScreen) return this.pagedList.currentPage + ' de ' + this.pagedList.pageCount;

    let currentPage = this.pagedList.currentPage <= 0 ? 0 : this.pagedList.currentPage - 1;
    let from = (currentPage * this.pagedList.pageSize) + 1;
    let until = from + this.pagedList.records.length - 1;
    return from + ' a ' + until + ' de ' + this.pagedList.recordCount;
  }

  get disablePrevious(): boolean {
    return this.pagedList.currentPage <= 1;
  }
  get disableNext(): boolean {
    return this.pagedList.currentPage >= this.pagedList.pageCount;
  }

  get isSmallScreen(): boolean {
    return window.innerWidth <= 600;
  }

  get navigationButtonClass(): string {
    if (this.isSmallScreen) return 'mat-icon-button';
    return 'mat-button';
  }
  disabledIconClass(disabled: boolean): string {
    return disabled ? 'disabled-icon' : 'enabled-icon';
  }

  previous() {
    let currentPage = this.currentPage-1;
    this.pageChange.emit({currentPage: currentPage, pageSize: this.pageSize});
    this._cdr.detectChanges();
  }
  next() {
    let currentPage = this.currentPage+1;
    this.pageChange.emit({currentPage: currentPage, pageSize: this.pageSize});
    this._cdr.detectChanges();
  }
  first() {
    let currentPage = 1;
    this.pageChange.emit({currentPage: currentPage, pageSize: this.pageSize});
    this._cdr.detectChanges();
  }
  last() {
    let currentPage = this.pagedList.pageCount;
    this.pageChange.emit({currentPage: currentPage, pageSize: this.pageSize});
    this._cdr.detectChanges();
  }

  pageSizeChange(newValue: number) {
    this.pageChange.emit({currentPage: this.currentPage, pageSize: newValue});
    this._cdr.detectChanges();
  }

}
