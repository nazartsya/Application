import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (_route, _state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.isLoggedIn$.pipe(
    map((isLogged) => {
      if (!isLogged) {
        router.navigate(['/login']);
        return false;
      }
      return true;
    })
  );
};
