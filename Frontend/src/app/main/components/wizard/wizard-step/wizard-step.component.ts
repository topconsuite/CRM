import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatTabGroup } from '@angular/material';

import { WizardComponent } from './../wizard.component';

@Component({
  selector: 'wizard-step',
  templateUrl: './wizard-step.component.html',
  styleUrls: ['./wizard-step.component.scss']
})
export class WizardStepComponent implements OnInit {

  @Input() title: string = "";
  @Input() icon: string = "";
  @Input() stepForm: FormGroup;

  @Input() hasNextButton: boolean = true;
  @Input() hasPreviousButton: boolean = true;
  @Input() hasCompleteButton: boolean = true;
  @Input() visible: boolean = true;

  @Output() onNext: EventEmitter<any> = new EventEmitter<any>();
  @Output() onPrevious: EventEmitter<any> = new EventEmitter<any>();
  @Output() onStepSelected: EventEmitter<any> = new EventEmitter<any>();


  @ViewChild('container', { static: false }) container;

  tabGroup: MatTabGroup;
  wizzard: WizardComponent;

  get isFirst(): boolean {
    return !this.wizzard.hasPrevious(this);
  };
  get isLast(): boolean {
    return !this.wizzard.hasNext(this);
  };
  get isValid(): boolean {
    return !this.stepForm.invalid && !this.isDisabled();
  };

  constructor() { }

  ngOnInit() { }

  isDisabled(): boolean {
    let previous = this.wizzard.getPrevious(this);
    return ( !previous ? false : !previous.isValid );
  }

  isSelected(): boolean {
    if (!this.wizzard || !this.tabGroup) return false;
    return (this.wizzard.visibleWizardSteps.indexOf(this) === this.tabGroup.selectedIndex);
  }

  complete() {
    if (!this.isLast) {
      var last = this.wizzard.getLast();
      if (!this.tabGroup) this.wizzard.initializeSteps();
      this.wizzard.tryCompleteWizzard = true;
      this.tabGroup.selectedIndex = this.wizzard.getVisibleTabIndex(last);
    }
    else if (this.isValid)
      this.wizzard.onComplete.emit();
  }

  next() {
    var next = this.wizzard.getNext(this);
    if (!this.tabGroup) this.wizzard.initializeSteps();
    this.tabGroup.selectedIndex = this.wizzard.getVisibleTabIndex(next);
  }

  previous() {
    var previous = this.wizzard.getPrevious(this);
    if (!this.tabGroup) this.wizzard.initializeSteps();
    this.tabGroup.selectedIndex = this.wizzard.getVisibleTabIndex(previous);
  }

  setFocus() {
    if (!this.tabGroup) this.wizzard.initializeSteps();
    this.tabGroup.selectedIndex = this.wizzard.getVisibleTabIndex(this);
  }

}
