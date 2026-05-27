import { Component } from '@angular/core';
import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';

import { locale as brazilianPortuguese } from './i18n/pt-br';
import { locale as english } from './i18n/en';
import { locale as turkish } from './i18n/tr';

@Component({
    selector   : 'fuse-home',
    templateUrl: './home.component.html',
    styleUrls  : ['./home.component.scss']
})
export class FuseHomeComponent
{
    constructor(
        private translationLoader: FuseTranslationLoaderService,
        private _fuseNavigationService: FuseNavigationService
    ) {
        this.translationLoader.loadTranslations(brazilianPortuguese, english, turkish);
        
        //this._fuseNavigationService.setCurrentNavigation('main');
        this._fuseNavigationService.setCurrentNavigation('comercial');
    }
}
