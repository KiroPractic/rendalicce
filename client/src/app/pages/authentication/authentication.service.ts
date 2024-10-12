import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";
import {JwtService} from "../../services/jwt.service";
import {Router} from "@angular/router";
import {GlobalMessageService} from "../../services/global-message.service";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  #jwtService: JwtService = inject(JwtService);
  #httpClient: HttpClient = inject(HttpClient);
  #router: Router = inject(Router);
  #globalMessageService: GlobalMessageService = inject(GlobalMessageService);

  register(payload: any) {
    return this.#httpClient.post(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.authRoute}${ApiRoutes.registerRoute}`, payload);
  }

  login(payload: any) {
    return this.#httpClient.post(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.authRoute}${ApiRoutes.loginRoute}`, payload);
  }

  logout() {
    this.#jwtService.remove();
    this.#router.navigateByUrl("/").then(() => {
      this.#globalMessageService.showInformationMessage({title: '', content: 'Uspješna odjava'});
    });
  }
}
