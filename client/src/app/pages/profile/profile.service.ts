import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ApiRoutes} from "../../enums/api-routes.enum";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  #httpClient: HttpClient = inject(HttpClient);

  getUser() {
    return this.#httpClient.get<any>(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.accountRoute}`);
  }
  getAccountInformation(id: string) {
    return this.#httpClient.get(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.usersRoute}/${id}`);
  }

  updateAccountInformation(payload: any) {
    return this.#httpClient.post(`${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.accountRoute}`, payload);
  }

  getTransactionCount(id: string): Observable<number> {
    return this.#httpClient
      .get<{ transactionCount: number }>(
        `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.serviceProvidersRoute}/${id}/transaction-count`
      )
      .pipe(
        map(response => response.transactionCount) // Extract the transactionCount from the response
      );
  }
}

