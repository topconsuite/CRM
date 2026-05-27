import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';

import { Tasks } from 'app/classes/_tasks/tasks';

import { UserService } from 'app/services/user.service';
import { VersionService } from 'app/services/version.service';

import { AlertDialogComponent } from '../../../components/dialog/alert-dialog/alert-dialog.component';
import { SsoService } from 'app/services/sso.service';
import * as Msal from "msal";


@Component({
    selector   : 'fuse-login',
    templateUrl: './login.component.html',
    styleUrls  : ['./login.component.scss'],
    animations : fuseAnimations
})
export class FuseLoginComponent implements OnInit {
    private msalApp: Msal.UserAgentApplication;

    isMsalInitializing: boolean = true;
    isSsoConfigured: boolean = false;

    loginForm: FormGroup;
    loginFormErrors: any;

    passwordRegistrationForm: FormGroup;
    passwordRegistrationErros: any;

    modePasswordRegistration: boolean;

    showForm: boolean = true;

    get selectedForm(): FormGroup {
        if(this.modePasswordRegistration) {
            return this.passwordRegistrationForm;
        } else {
            return this.loginForm;
        }
    };

    get selectedFormErrors(): any {
        if(this.modePasswordRegistration) {
            return this.passwordRegistrationErros;
        } else {
            return this.loginFormErrors;
        }
    };

    constructor(
        private fuseConfig: FuseConfigService,
        private formBuilder: FormBuilder,
        private router: Router,
        private userService: UserService,
        private dialog: MatDialog,
        private _versionService: VersionService,
        private _cdr: ChangeDetectorRef,
        private _ssoService: SsoService
    )
    {
        this.fuseConfig.config = {
            layout: {
                navbar   : {
                    hidden: true
                },
                toolbar  : {
                    hidden: true
                },
                footer   : {
                    hidden: true
                },
                sidepanel: {
                    hidden: true
                }
            }
        };//*/

        this.loginFormErrors = {
            user   : {},
            password: {}
        };

        this.passwordRegistrationErros = {
            user   : {},
            password: {},
            confirmPassword: {}
        };

        this.initializeMsal();
    }

    ngOnInit()
    {
        this.modePasswordRegistration = false;
        
        localStorage.removeItem("t.tcw.proposta.filtro.vendedor");
        
        this.loginForm = this.formBuilder.group({
            user   : ['', [Validators.required]],
            password: ['', Validators.required]
        });

        this.loginForm.valueChanges.subscribe(() => {
            this.onFormValuesChanged();
        });
       
        this.passwordRegistrationForm = this.formBuilder.group({
            user: ['', [Validators.required]],
            password: ['', Validators.required],
            confirmPassword: ['', Validators.required]
        });

        this.passwordRegistrationForm.valueChanges.subscribe(() => {
            this.onFormValuesChanged();
        });
    }

    onFormValuesChanged() {
        for ( const field in this.selectedFormErrors )
        {
            if ( !this.selectedFormErrors.hasOwnProperty(field) )
            {
                continue;
            }

            // Clear previous errors
            this.selectedFormErrors[field] = {};

            // Get the control
            const control = this.selectedForm.get(field);

            if ( control && control.dirty && !control.valid )
            {
                this.selectedFormErrors[field] = control.errors;
            }
        }
    }

    submit() {
        this.userService.login(
            this.loginForm.get('user').value.toUpperCase(),
            this.loginForm.get('password').value.toUpperCase()
        ).then( data => {
            this.router.navigate(['/home']);
        }, err => {
            this.showErrorDialog('ERRO', (err && err.error_description) ? err.error_description : JSON.stringify(err));
        });
    }

    submitPasswordRegistration() {
        this.userService.passwordRegister(
            this.selectedForm.get('user').value.toUpperCase(),
            this.selectedForm.get('password').value.toUpperCase(),
            this.selectedForm.get('confirmPassword').value.toUpperCase(),
        ).then( data => {
            this.dialog.open(AlertDialogComponent, { data: { title: 'TopConWeb', message: 'Senha cadastrada com sucesso' } });
            this.setModePasswordRegistration(false);
            this.router.navigate(['/pages/auth/login']);
        }, err => {
            this.showErrorDialog('ERRO', Tasks.formataErrosApi(err));
        });
    }

    transferValuesBeetwenForms() {
        if(this.modePasswordRegistration) {
            this.passwordRegistrationForm.get('user').setValue(this.loginForm.get('user').value); 
            this.passwordRegistrationForm.get('password').setValue(''); 
            this.passwordRegistrationForm.get('confirmPassword').setValue(''); 
        } else {
            this.loginForm.get('user').setValue(this.passwordRegistrationForm.get('user').value); 
            this.loginForm.get('password').setValue(''); 
        }
    }

    setModePasswordRegistration(value: boolean) {
        this.showForm = false;
        this._cdr.detectChanges();
        this.modePasswordRegistration = value;
        this.transferValuesBeetwenForms();
        this.showForm = true;
        this._cdr.detectChanges();
    }

     private initializeMsal(): void {
        this.isMsalInitializing = true;
        this.isSsoConfigured = false;

        this._ssoService.listarConfiguracaoAzureAd().then(data => {
            if (data && data.clientId) {
                this.msalApp = new Msal.UserAgentApplication({
                    auth: {
                        clientId: data.clientId,
                        authority: `https://login.microsoftonline.com/${data.tenantId}`,
                        redirectUri: data.urlRedirecionamento,
                        validateAuthority: true,
                        navigateToLoginRequestUrl: false
                    },
                    cache: {
                        cacheLocation: "localStorage",
                        storeAuthStateInCookie: true
                    }
                });

                this.msalApp.handleRedirectCallback((error, response) => {
                    if (error) {
                        this.showErrorDialog('Erro de redirecionamento SSO.', error.errorMessage);
                    }
                });

                this.isSsoConfigured = true;
            } else {
                this.isSsoConfigured = false;
            }
            this.isMsalInitializing = false;
        }).catch(err => {
            this.isSsoConfigured = false;
            this.isMsalInitializing = false;
        });
    }

    loginSso() {
        if (this.isMsalInitializing || !this.msalApp) {
            return;
        }

        const loginRequest = { scopes: ["openid", "profile"] };

        this.msalApp.loginPopup(loginRequest)
            .then(loginResponse => {
                const idToken = loginResponse.idToken.rawIdToken;
                this.exchangeTokenWithBackend(idToken);
            })
            .catch(error => {
                console.error("MSAL Login Popup Error:", error);
                if (error.errorCode !== "user_cancelled") {
                   this.showErrorDialog('Login Falhou', error.errorMessage || 'Não foi possível realizar o login');
                }
            });
    }

    private exchangeTokenWithBackend(azureToken: string): void {
        this.userService.loginWithSSo(azureToken).then(data => {
            this.router.navigate(['/home']);
        }, err => {
            this.showErrorDialog('Login Falhou', (err && err.error_description) ? err.error_description : 'Ocorreu um erro ao autenticar com o servidor.');
        });
    }

    private showErrorDialog(title: string, message: string): void {
        this.dialog.open(AlertDialogComponent, { data: { title: title, message: message } });
    }

    get version(): string {
        return this._versionService.full;
    }
}
