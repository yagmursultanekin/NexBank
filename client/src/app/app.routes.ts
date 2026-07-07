import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard').then(m => m.DashboardComponent)
  },
  {
  path: 'register',
  loadComponent: () =>
    import('./features/auth/register/register').then(m => m.RegisterComponent)
},

{
  path: 'accounts/:id',
  loadComponent: () =>
    import('./features/accounts/account-detail/account-detail').then(m => m.AccountDetailComponent)
}

];