import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from "@angular/router";
import { ViewServiceProviderService } from "./view-service-provider.service";
import { DatePipe, DecimalPipe, NgForOf, NgIf } from "@angular/common";
import {ProgressSpinnerModule} from "primeng/progressspinner";

@Component({
  selector: 'app-view-service-provider',
  standalone: true,
  imports: [
    DatePipe,
    NgIf,
    NgForOf,
    DecimalPipe,
    RouterLink,
    ProgressSpinnerModule
  ],
  templateUrl: './view-service-provider.component.html',
  styleUrl: './view-service-provider.component.scss'
})
export class ViewServiceProviderComponent {
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  service = inject(ViewServiceProviderService);
  serviceProvider: any;
  isLoading = false;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.isLoading = true;
      this.service.getById(params['id']).subscribe((service: any) => {
        this.serviceProvider = service.serviceProvider;
        this.isLoading = false;
      }, () => {
        this.isLoading = false;
      });
    });
  }

  onAddReview() {
    console.log('Add Review');
  }
}
