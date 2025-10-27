import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';
import { guestGuard } from './auth/guest.guard';
import { authGuard } from './auth/auth.guard';
import { EventList } from './events/event-list/event-list';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'register', component: Register, canActivate: [guestGuard] },
  {
    path: 'events',
    canActivate: [authGuard],
    children: [{ path: '', component: EventList }],
  },
  { path: '**', redirectTo: 'login' },
];
