import { Component, inject } from '@angular/core';
import { Service } from '../../model/service.model';
import { User } from '../../model/user.model';
import { Review } from '../../model/review.model';
import { ReviewsService } from '../../services/reviews.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EditProfileModalComponent } from '../../components/edit-profile-modal/edit-profile-modal.component';
import { CreateOrUpdateServiceProviderService } from '../create-or-update-service-provider/create-or-update-service-provider.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, EditProfileModalComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {
  user: User = {
    firstName: 'John',
    lastName: 'Doe',
    image: 'https://via.placeholder.com/120',
    description:
      'Full-stack developer with a passion for building scalable applications.',
    phone: '+123456789',
    email: 'test@gmail.com',
  };

  services: Service[] = [
    {
      id: 'a7adf1e5-0837-4594-b9ac-d46767e147c8',
      name: 'Web Development',
      description:
        'Building responsive and fast websites using Angular, React, or Vue.js.',
      price: '$1000',
      image: 'https://via.placeholder.com/100',
    },
    {
      id: '2',
      name: 'Mobile App Development',
      description:
        'Creating mobile applications for Android and iOS platforms.',
      price: '$1500',
      image: 'https://via.placeholder.com/100',
    },
    {
      id: '3',
      name: 'UI/UX Design',
      description:
        'Designing intuitive and user-friendly interfaces for web and mobile apps.',
      price: '$800',
      image: 'https://via.placeholder.com/100',
    },
  ];

  private reviewService = inject(ReviewsService);
  private routerService = inject(Router);
  private createOrUpdateServiceProviderService = inject(
    CreateOrUpdateServiceProviderService
  );
  reviews: Review[] = this.reviewService.getRandomReviews();
  isEditModalOpen = false;

  openEditModal() {
    this.isEditModalOpen = true;
  }

  closeEditModal() {
    this.isEditModalOpen = false;
  }

  getStars(rating: number): string[] {
    const filledStars = Array(rating).fill('filled');
    const emptyStars = Array(5 - rating).fill('empty');
    return [...filledStars, ...emptyStars];
  }

  getAverageRating(): number {
    const totalRatings = this.reviews.reduce(
      (acc, review) => acc + review.rating,
      0
    );
    return totalRatings / this.reviews.length;
  }

  editProfile() {
    console.log('Editing profile...');
  }

  openService(serviceId) {
    this.routerService.navigate(['/service', serviceId]);
  }

  getServices() {
    this.createOrUpdateServiceProviderService.get().subscribe({
      next: (services: Service[]) => {
        this.services = services;
      },
      error: (error) => {
        console.error('Error fetching services:', error);
      },
    });
  }

  deleteService(serviceId: string) {
    this.createOrUpdateServiceProviderService.delete(serviceId).subscribe({
      next: () => {
        this.getServices();
      },
      error: (error) => {
        console.error('Error deleting service:', error);
      },
    });
  }

  editService(serviceId) {
    this.routerService.navigate(['/service-provider/edit', serviceId]);
  }
}
