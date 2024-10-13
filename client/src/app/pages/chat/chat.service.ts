import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ApiRoutes } from '../../enums/api-routes.enum';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  #httpClient: HttpClient = inject(HttpClient);

  getChats() {
    return this.#httpClient.get(
      `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.chatsRoute}`
    );
  }

  getChatByUserId(id: string) {
    return this.#httpClient.get(
      `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.chatsRoute}/${id}`
    );
  }

  getMessages(chatId: string) {
    return this.#httpClient.get(
      `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.chatsRoute}/${chatId}/messages`
    );
  }

  getMessagesByUserId(userId: string) {
    return this.#httpClient.get(
      `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.chatsRoute}/users/${userId}`
    );
  }

  sendMessage(message: string, chatId: string) {
    return this.#httpClient.post(
      `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.chatsRoute}/${chatId}/messages`,
      { content: message }
    );
  }

  sendCoinMessageRequest(message: string, chatId: string, serviceTransaction: any) {
    return this.#httpClient.post(
      `${environment.baseUrl}${ApiRoutes.apiRoute}${ApiRoutes.chatsRoute}/${chatId}/messages`,
      { content: message, serviceTransaction }
    );
  }
}
