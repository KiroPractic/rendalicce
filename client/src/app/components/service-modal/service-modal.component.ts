import { CurrencyPipe, UpperCasePipe } from '@angular/common';
import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-service-modal',
  standalone: true,
  imports: [UpperCasePipe, CurrencyPipe],
  templateUrl: './service-modal.component.html',
  styleUrl: './service-modal.component.scss'
})
export class ServiceModalComponent {
  service = input.required<any>();
  closeServiceModal = output();

  closeModal() {
    this.closeServiceModal.emit();
  }
}
