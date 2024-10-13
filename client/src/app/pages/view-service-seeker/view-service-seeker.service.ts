import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ViewServiceSeekerService {
  httpClient: HttpClient = inject(HttpClient);

  getById(id: any) {
    return this.httpClient.get(`${environment.baseUrl}/service-seekers/${id}`);
  }
}
