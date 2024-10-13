import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";

@Injectable({
  providedIn: 'root'
})
export class ViewServiceSeekerService {
  httpClient: HttpClient = inject(HttpClient);

  getById(id: any) {
    return this.httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}/service-seekers/${id}`);
  }
}
