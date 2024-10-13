import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from "@angular/router";
import { ViewServiceProviderService } from "./view-service-provider.service";
import { DatePipe, DecimalPipe, NgForOf, NgIf } from "@angular/common";
import { DialogModule } from 'primeng/dialog';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
import { ProgressSpinnerModule } from "primeng/progressspinner";

@Component({
  selector: 'app-view-service-provider',
  standalone: true,
  imports: [
    DatePipe,
    NgIf,
    NgForOf,
    DecimalPipe,
    RouterLink,
    DialogModule,
    RatingModule,
    FormsModule,
    RouterLink,
    ProgressSpinnerModule
  ],
  templateUrl: './view-service-provider.component.html',
  styleUrls: ['./view-service-provider.component.scss']
})
export class ViewServiceProviderComponent {
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  service = inject(ViewServiceProviderService);
  serviceProvider: any;
  isLoading = false;
  displayRatingModal: boolean = false;
  rating: number = 0;
  reviewText: string = '';
  image: any;

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

  fetchServiceProvider(serviceId: string) {
    this.isLoading = true;
    this.service.getById(serviceId).subscribe((service: any) => {
      this.serviceProvider = service.serviceProvider;
      this.isLoading = false;
    }, () => {
      this.isLoading = false;
    });
  }

  onAddReview() {
    if (this.rating > 0 && this.reviewText) {
      const formData = new FormData();

      formData.append('rating', this.rating.toString());
      formData.append('content', this.reviewText);

      if (this.image) {
        formData.append('contentPhoto', this.image, this.image.name);
      }

      const serviceId = this.serviceProvider.id;

      this.service.postReview(serviceId, formData).subscribe(() => {
        console.log('Review submitted successfully.');

        this.fetchServiceProvider(serviceId);

      }, (error) => {
        console.error('Error submitting review:', error);
      });

      this.displayRatingModal = false;
      this.rating = 0;
      this.reviewText = '';
      this.image = null;
    } else {
      alert('Please provide a rating and a review.');
    }
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.image = file;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.image.profilePhotoBase64 = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }
}
