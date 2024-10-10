import { JwtService } from "../services/jwt.service";
import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

export const authenticationGuard: CanActivateFn = (_, __) => {
  const jwtService: JwtService = inject(JwtService);

  if (jwtService.rawToken() && !jwtService.isTokenExpired()) return true;
  else {
    inject(Router).navigateByUrl("/login");
    return false;
  }
}
