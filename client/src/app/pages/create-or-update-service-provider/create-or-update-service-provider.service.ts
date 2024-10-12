import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CreateOrUpdateServiceProviderService {
  #httpClient: HttpClient = inject(HttpClient);

  getById(id: string) {
    return this.#httpClient.get(`https://api.example.com/service-provider/${id}`);
  }

  create(payload: any) {
    return this.#httpClient.post('https://api.example.com/service-provider', payload);
  }

  update(id: string, payload: any) {
    return this.#httpClient.put(`https://api.example.com/service-provider/${id}`, payload);
  }
}
