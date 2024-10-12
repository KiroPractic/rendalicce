import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  #httpClient: HttpClient = inject(HttpClient);

  getAccountInformation(id: string) {
    return this.#httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.usersRoute}/${id}`);
  }
}
