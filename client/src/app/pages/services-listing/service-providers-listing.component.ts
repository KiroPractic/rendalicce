import {CurrencyPipe, DecimalPipe, UpperCasePipe} from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceModalComponent } from '../../components/service-modal/service-modal.component';
import {FormsModule} from "@angular/forms";
import {InputTextModule} from "primeng/inputtext";
import {ButtonModule} from "primeng/button";
import {Select} from "primeng/select";
import {ServiceProvidersListingService} from "./service-providers-listing.service";
import {MapInputComponent} from "../../components/map-input/map-input.component";
import {MapViewComponent} from "../../components/map-view/map-view.component";

@Component({
  selector: 'app-services-listing',
  standalone: true,
  imports: [CurrencyPipe, ServiceModalComponent, UpperCasePipe, FormsModule, InputTextModule, ButtonModule, Select, DecimalPipe, MapInputComponent, MapViewComponent],
  templateUrl: './service-providers-listing.component.html',
  styleUrl: './service-providers-listing.component.scss',
})
export class ServiceProvidersListingComponent {
  services = [];
  filteredServices: any = [...this.services];
  selectedCategories = [];
  selectedServiceTypes = [];
  selectedPaymentTypes = [];
  private router = inject(Router);
  private service = inject(ServiceProvidersListingService);
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

  ngOnInit() {
    this.service.getAll().subscribe((services: any) => {
      this.services = services.serviceProviders;
      this.filteredServices = services.serviceProviders;
    });
  }

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

  getCoordinates(geolocation: string): [number, number] | null {
    if (geolocation) {
      const coords = geolocation
        .split(',')
        .map((coord) => parseFloat(coord.trim()));
      if (coords.length === 2 && !isNaN(coords[0]) && !isNaN(coords[1])) {
        return [coords[0], coords[1]];
      }
    }
    return null;
  }
}
