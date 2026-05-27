import { Injectable, Injector } from '@angular/core';
import { RequestOptionsArgs, Headers } from '@angular/http';
import { Router } from '@angular/router';

import { AuthService } from './auth/auth.service';
import { BaseService } from './base.service';
import { UsuarioWebFiltro } from 'app/classes/usuario/usuario-web-filtro';

const lsKeyUser: string = 't.tcw.user';
const lsKeyLogged: string = 't.tcw.isLogged';

@Injectable()
export class UserService extends BaseService {

    private get _usuario() {
        let user: any = localStorage.getItem(lsKeyUser);
        if (user) user = JSON.parse(user);
        return user || {
            id: '',
            nome: '',
            direitos: {}
        }
    };
    
    constructor(
        injector: Injector,
        private _router: Router,
        private _auth: AuthService
    ) {
        super(injector);
    }

    login(user: string, password: string) {
        this._auth.deleteToken();

        let deferred = this.createDeferred();
        let promise = deferred.promise;
        
        let data = 'grant_type=password&username='+user+'&password='+password+'&api_version='+this.constants.API_VERSION;
        let options: RequestOptionsArgs = { headers: new Headers()};
        options.headers.set('Content-Type', 'application/x-www-form-urlencoded');

        this.http.post(this.apiBaseUrlService.getUrl() + 'security/token', data, options)
        .subscribe(response => {
            let data = this.getResponseData(response);
            if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                if (data.access_token) {
                    this._auth.setToken(data.access_token);
                    localStorage.setItem(lsKeyUser, data.user);
                    localStorage.setItem(lsKeyLogged, 'true');
                }
                deferred.resolve(data);
            }else{
                deferred.reject(data);
            }
        }, error => {
            let data = this.getErrorData(error);
            deferred.reject(data);
        });

        return promise;
    }

     loginWithSSo(azureToken: string, hideLoading?: boolean){
        this._auth.deleteToken();

        let deferred = this.createDeferred();
        let promise = deferred.promise;

        let data = 'grant_type=azure&assertion='+azureToken+'&api_version='+this.constants.API_VERSION;
        let options: RequestOptionsArgs = { headers: new Headers()};
        options.headers.set('Content-Type', 'application/x-www-form-urlencoded');

        this.http.post(this.apiBaseUrlService.getUrl() + 'security/token', data, options)
        .subscribe(response => {
            let data = this.getResponseData(response);
            if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                if (data.access_token) {
                    this._auth.setToken(data.access_token);
                    localStorage.setItem(lsKeyUser, data.user);
                    localStorage.setItem(lsKeyLogged, 'true');
                }
                deferred.resolve(data);
            }else{
                deferred.reject(data);
            }
        }, error => {
            let data = this.getErrorData(error);
            deferred.reject(data);
        });

        return promise;
    }

    loginWithB2C(idToken: string, hideLoading?: boolean) {
        // Exchange the B2C id_token for a local CRM bearer token via the
        // OAuth grant_type=b2c (see Security/AuthAuthorizationServerProvider
        // and docs/sso-decisoes-implementacao.md, D1/D4).
        this._auth.deleteToken();

        let deferred = this.createDeferred();
        let promise = deferred.promise;

        let body = 'grant_type=b2c&assertion=' + encodeURIComponent(idToken)
                 + '&api_version=' + this.constants.API_VERSION;
        let options: RequestOptionsArgs = { headers: new Headers() };
        options.headers.set('Content-Type', 'application/x-www-form-urlencoded');

        this.http.post(this.apiBaseUrlService.getUrl() + 'security/token', body, options)
        .subscribe(response => {
            let data = this.getResponseData(response);
            if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK) {
                if (data.access_token) {
                    this._auth.setToken(data.access_token);
                    localStorage.setItem(lsKeyUser, data.user);
                    localStorage.setItem(lsKeyLogged, 'true');
                }
                deferred.resolve(data);
            } else {
                deferred.reject(data);
            }
        }, error => {
            let data = this.getErrorData(error);
            deferred.reject(data);
        });

        return promise;
    }

    logout(): void {
        this._auth.deleteToken();
        localStorage.removeItem(lsKeyLogged);
        this.router.navigate(['pages/auth/login']);
    }

    getUserName(): string {
        return this._usuario.nome;
    }

    isLogged(): boolean {
        return (localStorage.getItem(lsKeyLogged) && this.getUserName() !== '');
    }

    setUser(user) {
        localStorage.setItem(lsKeyUser, JSON.stringify(user));
    }
    getUserDireitos() {
        return this._usuario.direitos;
    }
    temDireitoAplicativo(aplicativo: string, tipoDireito: string, redirecionaSemPermissaoAfterMiliseconds: number = 0) {
        var temDireito = this._usuario.direitos[aplicativo] !== undefined
            && ( (this._usuario.direitos[aplicativo] || '').toUpperCase().indexOf(tipoDireito.toUpperCase()) !== -1 );

        var self = this;

        if (redirecionaSemPermissaoAfterMiliseconds && !temDireito) {
            setTimeout(() => {
                self._router.navigate(['pages/sem-permissao']);
            }, redirecionaSemPermissaoAfterMiliseconds);
        }
        
        return temDireito;
    }

    passwordRegister(user: string, password: string, confirmPassword: string) {

        var body = { IdUsuario: user, Senha: password, SenhaConfirmacao: confirmPassword };
        return this.makePostPrommisse<any>('v1/usuario/cadastrar/senha',JSON.stringify(body));
    }

    gravarAcessoAplicacao(aplicacao: string, programa: number, hideLoading?: boolean) {
        return this.makePostPrommisse<any>(`v1/usuario/acesso-aplicacao`
            +`/aplicativo/${aplicacao}`
            +`/programa/${programa}`, "", hideLoading);
    }

    obterFiltro(aplicativo: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<UsuarioWebFiltro>(`v1/usuario/filtro/aplicativo/${aplicativo}`, hideLoading);
    }

    salvarFiltro(filtro: UsuarioWebFiltro, hideLoading?: boolean) {
        return this.makePostPrommisse<any>(`v1/usuario/filtro`, JSON.stringify(filtro), hideLoading);
    }



}