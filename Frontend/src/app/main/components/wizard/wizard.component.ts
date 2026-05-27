import { Component, OnInit, ViewChild, ContentChildren, QueryList,
        AfterContentInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { MatTabGroup } from '@angular/material';

import { WizardStepComponent } from './wizard-step/wizard-step.component';


@Component({
  selector: 'wizard',
  templateUrl: './wizard.component.html',
  styleUrls: ['./wizard.component.scss']
})
export class WizardComponent implements OnInit, AfterContentInit {
  
  @Input() innerHeightPercent: number = 67.5;

  @ViewChild('tabGroup', { static: false }) tabGroup: MatTabGroup;

  @ContentChildren(WizardStepComponent)
  wizardSteps: QueryList<WizardStepComponent>;

  @Output() onComplete: EventEmitter<any> = new EventEmitter<any>();

  get visibleWizardSteps(): WizardStepComponent[] {
    return this.wizardSteps.filter(t => t.visible);
  }

  private _previousIndex: number = 0;

  constructor(private _cdr: ChangeDetectorRef) { }

  ngOnInit() {
    if (this.tabGroup) this._previousIndex = this.tabGroup.selectedIndex;
  }

  ngAfterContentInit() {
    this.initializeSteps();
  }

  initializeSteps() {
    if (this.tabGroup) this._previousIndex = this.tabGroup.selectedIndex;
    this.wizardSteps.forEach(step => {
      step.tabGroup = this.tabGroup;
      step.wizzard = this;
    });
  }

  tryCompleteWizzard: boolean = false;
  tabChangeComplete() {
    if (this.tryCompleteWizzard) {
      this.getLast().complete();
      this.tryCompleteWizzard = false;
    }
  }

  onTabChange(index: number) {
    for (var i = Math.min(this._previousIndex, index); i < Math.max(this._previousIndex, index); i++) {
      if (this._previousIndex < index) {
        if (this.visibleWizardSteps[i]){
          this.visibleWizardSteps[i].onNext.emit();
        }
      } else {
        if (this.visibleWizardSteps[this._previousIndex-(i-index)])
            this.visibleWizardSteps[this._previousIndex-(i-index)].onPrevious.emit();
      }
    }
    this._previousIndex = index;
    if (this.visibleWizardSteps[index]) this.visibleWizardSteps[index].onStepSelected.emit();
    this.tabChangeComplete();
  }

  getVisibleTabIndex(step: WizardStepComponent): number {
    return this.visibleWizardSteps.indexOf(step);
  }

  isStepSelected(step: WizardStepComponent): boolean {
    if (!this.tabGroup) return false;
    return this.tabGroup.selectedIndex === this.getVisibleTabIndex(step);
  }

  get selectecStep(): WizardStepComponent {
    if (!this.tabGroup) return null;
    return this.visibleWizardSteps[this.tabGroup.selectedIndex];
  }

  hasPrevious(step: WizardStepComponent): boolean {
    var visibleWizardSteps = this.wizardSteps.filter(t => t.visible);
    return (step != visibleWizardSteps[0]);
  }
  getPrevious(step: WizardStepComponent): WizardStepComponent {
    if (!this.hasPrevious(step)) return null;
    let steps = this.visibleWizardSteps;
    return steps[steps.indexOf(step)-1];
  }

  hasNext(step: WizardStepComponent): boolean {
    var visibleWizardSteps = this.wizardSteps.filter(t => t.visible);
    return (step != visibleWizardSteps[visibleWizardSteps.length - 1]);
  }
  getNext(step: WizardStepComponent): WizardStepComponent {
    if (!this.hasNext(step)) return null;
    let steps = this.visibleWizardSteps;
    return steps[steps.indexOf(step)+1];
  }

  getLast(): WizardStepComponent {
    let steps = this.visibleWizardSteps;
    return steps[steps.length - 1];
  }

  containerHeight() {
    let pctDefault = this.innerHeightPercent;
    let ch = 786.13 * pctDefault / 100;
    let h = 769;
    let difHeight = (h - window.innerHeight) + 36;
    return ((ch - difHeight) * pctDefault / ch) + 'ch';
  }

}
