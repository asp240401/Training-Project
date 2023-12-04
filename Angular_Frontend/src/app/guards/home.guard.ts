import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const homeGuard: CanActivateFn = (route, state) => {
  var status=localStorage.getItem("connected")
  const router: Router = inject(Router);
  if(status=="yes")
  {
    router.navigate(['details'])
    return false;
  }
  return true;
};
