import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";

@Injectable({
  providedIn: 'root'
})
export class CreateOrUpdateServiceSeekerService {
  #httpClient: HttpClient = inject(HttpClient);

  getById(id: string) {
    return this.#httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.serviceSeeker}/${id}`);
  }

  create(payload: any) {
    return this.#httpClient.post(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.serviceSeeker}`, payload);
  }

  update(id: string, payload: any) {
    return this.#httpClient.put(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.serviceSeeker}/${id}`, payload);
  }

  delete(id: string) {
    return this.#httpClient.delete(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.serviceSeeker}/${id}`);
  }
}
