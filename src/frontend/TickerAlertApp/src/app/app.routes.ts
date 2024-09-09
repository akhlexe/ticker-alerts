import { Routes } from '@angular/router';
import { AlertsComponent } from './features/alerts/alerts.component';
import { authGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/pages/not-found/not-found.component';
import { LoginComponent } from './core/pages/login/login.component';
import { RegisterComponent } from './core/pages/register/register.component';
import { HomeComponent } from './features/home/home.component';
import { FinancialAssetsComponent } from './features/financial-assets/financial-assets.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: '', canActivate: [authGuard], component: AlertsComponent },
    { path: 'alerts', canActivate: [authGuard], component: AlertsComponent },
    { path: 'financial-assets/:id', canActivate: [authGuard], component: FinancialAssetsComponent },
    { path: 'home', canActivate: [authGuard], component: HomeComponent },
    { path: '404', component: NotFoundComponent },
    { path: '**', redirectTo: '/404' }
];
