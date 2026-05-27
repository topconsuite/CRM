import { Component, OnInit } from '@angular/core';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';

@Component({
  selector: 'app-comercial-dashboard',
  templateUrl: './comercial-dashboard.component.html',
  styleUrls: ['./comercial-dashboard.component.scss']
})
export class ComercialDashboardComponent implements OnInit {

  constructor(
    private _fuseNavigationService: FuseNavigationService
  ) {
    this._fuseNavigationService.setCurrentNavigation('comercial');
  }

  ngOnInit() {
    
  }

}
