import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';
import { guestGuard } from './auth/guest.guard';
import { authGuard } from './auth/auth.guard';
import { EventList } from './events/event-list/event-list';
import { EventDetails } from './events/event-details/event-details';
import { EventAdd } from './events/event-add/event-add';
import { EventEdit } from './events/event-edit/event-edit';
import { EventOwnerGuard } from './events/event-owner.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'register', component: Register, canActivate: [guestGuard] },
  {
    path: 'events',
    canActivate: [authGuard],
    children: [
      { path: '', component: EventList },
      { path: 'add', component: EventAdd },
      { path: ':id', component: EventDetails },
      { path: ':id/edit', component: EventEdit, canActivate: [EventOwnerGuard] },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
