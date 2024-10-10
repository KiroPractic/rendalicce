import {MessageService} from "primeng/api";
import {inject, Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class GlobalMessageService {
  readonly messageService: MessageService = inject(MessageService);

  showSuccessMessage(message: Message) {
    this.messageService.add({
      severity: 'success',
      summary: '',
      detail: message.content,
      life: 20000
    })
  }

  showErrorMessage(message: Message) {
    this.messageService.add({
      severity: 'error',
      summary: '',
      detail: message.content,
      life: 5000
    })
  }

  showWarningMessage(message: Message) {
    this.messageService.add({
      severity: 'warn',
      summary: '',
      detail: message.content,
      life: 5000
    })
  }

  showInformationMessage(message: Message) {
    this.messageService.add({
      severity: 'info',
      summary: '',
      detail: message.content,
      life: 5000
    })
  }
}

export class Message {
  title?: string;
  content?: string;
}
