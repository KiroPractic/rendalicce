import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";

@Injectable({
  providedIn: 'root'
})
export class ViewServiceProviderService {
  httpClient: HttpClient = inject(HttpClient);

  getById(id: any) {
    return this.httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}/service-providers/${id}`);
  }

  postReview(id: any, formData: FormData) {
    return this.httpClient.post(`${environment.baseUrl}${ApiRoutes.apiRoute}/service-providers/${id}/reviews`, formData);
  }
}
