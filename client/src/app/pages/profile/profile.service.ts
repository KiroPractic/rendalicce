import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";
import {User} from "../../model/user.model";

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  #httpClient: HttpClient = inject(HttpClient);

  getUser() {
    return this.#httpClient.get<User>(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.accountRoute}`);
  }
  getAccountInformation(id: string) {
    return this.#httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.usersRoute}/${id}`);
  }
}
