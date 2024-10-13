import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";

@Injectable({
  providedIn: 'root'
})
export class ServiceProvidersListingService {
  #httpClient: HttpClient = inject(HttpClient);

  getAll() {
    return this.#httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.serviceProvidersRoute}`);
  }
}
