import { CurrencyPipe, UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceModalComponent } from '../../components/service-modal/service-modal.component';
import {FormsModule} from "@angular/forms";
import {InputTextModule} from "primeng/inputtext";
import {ButtonModule} from "primeng/button";
import {Select} from "primeng/select";

@Component({
  selector: 'app-services-listing',
  standalone: true,
  imports: [CurrencyPipe, ServiceModalComponent, UpperCasePipe, FormsModule, InputTextModule, ButtonModule, Select],
  templateUrl: './services-listing.component.html',
  styleUrl: './services-listing.component.scss',
})
export class ServicesListingComponent {
  services = [
    {
      id: '1',
      name: 'Service 1',
      description: 'Description 1',
      price: 100,
      tags: ['tag1', 'tag2'],
      category: 'category1',
      type: 'type1',
      payment: 'paymentType1',
      city: 'New York',
      image: 'https://via.placeholder.com/100',
    },
    {
      id: '2',
      name: 'Service 2',
      tags: ['tag1', 'tag2'],
      description: 'Description 2',
      price: 150,
      city: 'New York',
      category: 'category2',
      type: 'type2',
      payment: 'paymentType2',
      image: 'https://via.placeholder.com/100',
    },
  ];
  filteredServices = [...this.services];
  selectedCategories = [];
  selectedServiceTypes = [];
  selectedPaymentTypes = [];
  private router = inject(Router);
  public serviceModalOpen = false;
  public selectedService = null;
  public categories = [
    'category1', 'category2'
  ];
  public serviceTypes = [
    'type1', 'type2'
  ];
  public paymentTypes = [
    { label: 'Gotovina', value: 'paymentType1' },
    { label: 'Kartica', value: 'paymentType2' },
  ];
  public sortOptions = [
    { label: 'Rastući', value: 'asc' },
    { label: 'Padajući', value: 'desc' },
  ];
  public selectedSortOrder = this.sortOptions[0];

  // Filter by category
  filterByCategory(category: string) {
    if (this.selectedCategories.includes(category)) {
      this.selectedCategories = this.selectedCategories.filter(
        (cat) => cat !== category
      );
    } else {
      this.selectedCategories.push(category);
    }
    this.applyFilters();
  }

  // Filter by service type
  filterByServiceType(type: string) {
    console.log(type);
    if (this.selectedServiceTypes.includes(type)) {
      this.selectedServiceTypes = this.selectedServiceTypes.filter(
        (t) => t !== type
      );
    } else {
      this.selectedServiceTypes.push(type);
    }
    this.applyFilters();
  }

  // Filter by payment type
  filterByPaymentType(paymentType: string) {
    console.log(typeof paymentType);
    if (this.selectedPaymentTypes.includes(paymentType)) {
      this.selectedPaymentTypes = this.selectedPaymentTypes.filter(
        (pt) => pt !== paymentType
      );
    } else {
      this.selectedPaymentTypes.push(paymentType);
    }
    this.applyFilters();
  }

  // Apply all filters
  applyFilters() {
    this.filteredServices = this.services.filter((service) => {
      const matchesCategory = this.selectedCategories.length
        ? this.selectedCategories.includes(service.category)
        : true;
      const matchesType = this.selectedServiceTypes.length
        ? this.selectedServiceTypes.includes(service.type)
        : true;
      const matchesPayment = this.selectedPaymentTypes.length
        ? this.selectedPaymentTypes.includes(service.payment)
        : true;
      return matchesCategory && matchesType && matchesPayment;
    });
  }

  // Sort by price
  sortByPrice(sortOrder: string) {
    this.filteredServices.sort((a, b) =>
      sortOrder === 'asc' ? a.price - b.price : b.price - a.price
    );
  }

  openServiceModal(service) {
    this.serviceModalOpen = true;
    this.selectedService = service;
  }

  closeServiceModal() {
    this.serviceModalOpen = false;
    this.selectedService = null;
  }
}
