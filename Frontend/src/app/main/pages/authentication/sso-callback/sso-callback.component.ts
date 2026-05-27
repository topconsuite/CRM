import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';

import { FuseConfigService } from '@fuse/services/config.service';

import { UserService } from 'app/services/user.service';
import { AlertDialogComponent } from '../../../components/dialog/alert-dialog/alert-dialog.component';

/**
 * SSO callback for Topcon Identity (Azure AD B2C).
 *
 * Reads the OAuth implicit-flow fragment posted by the Identity launcher
 *   #id_token=...&access_token=...&token_type=Bearer&expires_in=3600
 * exchanges the id_token for a local CRM bearer token via
 * grant_type=b2c, stores it, and navigates to the safe return_to.
 *
 * See docs/sso-implementacao-crm.md (sections 3, 5) and
 * docs/sso-decisoes-implementacao.md (D1, D5, D7).
 */
@Component({
    selector: 'fuse-sso-callback',
    template: `
        <div class="sso-callback" style="display:flex;justify-content:center;align-items:center;height:100vh;font-family:sans-serif">
            <p>Entrando…</p>
        </div>
    `
})
export class SsoCallbackComponent implements OnInit {

    constructor(
        private fuseConfig: FuseConfigService,
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private dialog: MatDialog
    ) {
        this.fuseConfig.config = {
            layout: {
                navbar:    { hidden: true },
                toolbar:   { hidden: true },
                footer:    { hidden: true },
                sidepanel: { hidden: true }
            }
        };
    }

    ngOnInit(): void {
        const { idToken, returnTo } = this.parseFragmentAndQuery();

        // Strip the fragment so the token does not linger in window.location
        // history or get bookmarked by mistake.
        try {
            history.replaceState(null, '', window.location.pathname + window.location.search);
        } catch (_) { /* ignore */ }

        if (!idToken) {
            this.router.navigate(['pages/auth/login']);
            return;
        }

        this.userService.loginWithB2C(idToken).then(() => {
            const safe = this.resolveSafeReturn(returnTo);
            // Use full reload-style navigation so any guards/route data
            // re-evaluate with the fresh token in storage.
            this.router.navigateByUrl(safe);
        }, (err) => {
            const message = (err && (err.error_description || err.message)) || 'Não foi possível autenticar via Topcon Identity.';
            this.dialog.open(AlertDialogComponent, {
                data: { title: 'Login Falhou', message: message }
            });
            this.router.navigate(['pages/auth/login']);
        });
    }

    private parseFragmentAndQuery(): { idToken: string | null, returnTo: string } {
        const hash = (window.location.hash || '').replace(/^#/, '');
        const fragmentParams = new URLSearchParams(hash);
        const queryParams = new URLSearchParams(window.location.search || '');

        const idToken = fragmentParams.get('id_token') || queryParams.get('id_token');
        const returnTo = fragmentParams.get('return_to')
                      || fragmentParams.get('state')
                      || queryParams.get('return_to')
                      || queryParams.get('returnTo')
                      || '/home';

        return { idToken, returnTo };
    }

    /**
     * Open-redirect guard — see D7 in docs/sso-decisoes-implementacao.md.
     * Accept relative paths and absolute URLs on the same origin only.
     */
    private resolveSafeReturn(returnTo: string): string {
        if (!returnTo || typeof returnTo !== 'string') return '/home';
        const trimmed = returnTo.trim();
        if (!trimmed) return '/home';

        if (trimmed.startsWith('/') && !trimmed.startsWith('//')) {
            return trimmed;
        }

        try {
            const url = new URL(trimmed, window.location.origin);
            if (url.origin === window.location.origin) {
                return url.pathname + url.search;
            }
        } catch (_) { /* ignore */ }

        return '/home';
    }
}
