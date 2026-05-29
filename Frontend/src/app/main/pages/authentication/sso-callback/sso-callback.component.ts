import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';

import { FuseConfigService } from '@fuse/services/config.service';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';

import { UserService } from 'app/services/user.service';
import { AlertDialogComponent } from '../../../components/dialog/alert-dialog/alert-dialog.component';

/**
 * SSO callback for Topcon Identity (Azure AD B2C).
 *
 * Reads the OAuth implicit-flow fragment posted by the Identity launcher
 *   #id_token=...&access_token=...&token_type=Bearer&expires_in=3600
 * exchanges the id_token for a local CRM bearer token via
 * grant_type=b2c, stores it, and navigates to the safe return_to.
 */
@Component({
    selector: 'fuse-sso-callback',
    // Empty template — the Fuse splash screen (kept visible until the
    // backend responds) covers the viewport during the whole flow.
    template: ''
})
export class SsoCallbackComponent implements OnInit {

    constructor(
        private fuseConfig: FuseConfigService,
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private dialog: MatDialog,
        private splashScreen: FuseSplashScreenService
    ) {
        this.fuseConfig.config = {
            layout: {
                navbar:    { hidden: true },
                toolbar:   { hidden: true },
                footer:    { hidden: true },
                sidepanel: { hidden: true }
            }
        };

        // The Fuse splash service auto-hides on the first NavigationEnd
        // (i.e. when the router lands here). Re-show it so the user sees
        // the same cold-load visual through the backend round-trip instead
        // of a blank/flickering page.
        this.splashScreen.show();
    }

    ngOnInit(): void {
        // Race-safe re-show: the auto-hide runs inside setTimeout after the
        // first NavigationEnd; this call runs after that microtask so the
        // splash stays visible regardless of ordering.
        setTimeout(() => this.splashScreen.show(), 0);

        const { idToken, returnTo } = this.parseFragmentAndQuery();

        // Strip the fragment so the token does not linger in window.location
        // history or get bookmarked by mistake.
        try {
            history.replaceState(null, '', window.location.pathname + window.location.search);
        } catch (_) { /* ignore */ }

        if (!idToken) {
            this.splashScreen.hide();
            this.router.navigate(['pages/auth/login']);
            return;
        }

        this.userService.loginWithB2C(idToken).then(() => {
            const safe = this.resolveSafeReturn(returnTo);
            // Use full reload-style navigation so any guards/route data
            // re-evaluate with the fresh token in storage. Hide the splash
            // only after the destination route has committed so the user
            // never sees a blank frame.
            this.router.navigateByUrl(safe).then(() => this.splashScreen.hide());
        }, (err) => {
            this.splashScreen.hide();
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
     * Open-redirect guard: accept relative paths and absolute URLs on
     * the same origin only.
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
