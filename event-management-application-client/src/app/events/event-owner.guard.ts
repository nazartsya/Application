import { inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { EventsService } from './events.service';
import { AuthService } from '../auth/auth.service';
import { catchError, map, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EventOwnerGuard implements CanActivate {
  private events = inject(EventsService);
  private auth = inject(AuthService);
  private router = inject(Router);

  canActivate(route: ActivatedRouteSnapshot) {
    const eventId = route.paramMap.get('id')!;
    const userId = this.auth.getUserId();

    return this.events.getEvent(eventId).pipe(
      map((event) => {
        if (event.createdBy === userId) return true;
        this.router.navigate(['/events']);
        return false;
      }),
      catchError(() => {
        this.router.navigate(['/events']);
        return of(false);
      })
    );
  }
}
