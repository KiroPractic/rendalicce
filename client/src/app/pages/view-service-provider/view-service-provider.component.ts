import {Component, inject} from '@angular/core';
import {ActivatedRoute, RouterLink} from "@angular/router";
import {ViewServiceProviderService} from "./view-service-provider.service";
import {DatePipe, DecimalPipe, NgForOf, NgIf} from "@angular/common";
import { DialogModule } from 'primeng/dialog';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
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
    DialogModule,
    RatingModule,
    FormsModule,
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

  onAddReview() {
    if (this.rating > 0 && this.reviewText) {
      console.log('Rating:', this.rating);
      console.log('Review:', this.reviewText);

      this.displayRatingModal = false;

      this.rating = 0;
      this.reviewText = '';
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
