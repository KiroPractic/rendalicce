import { Component, inject } from '@angular/core';
import { Service } from '../../model/service.model';
import { User } from '../../model/user.model';
import { Review } from '../../model/review.model';
import { ReviewsService } from '../../services/reviews.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
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
  };

  services: Service[] = [
    {
      id: 1,
      name: 'Web Development',
      description:
        'Building responsive and fast websites using Angular, React, or Vue.js.',
      price: '$1000',
      image: 'https://via.placeholder.com/100',
    },
    {
      id: 2,
      name: 'Mobile App Development',
      description:
        'Creating mobile applications for Android and iOS platforms.',
      price: '$1500',
      image: 'https://via.placeholder.com/100',
    },
    {
      id: 3,
      name: 'UI/UX Design',
      description:
        'Designing intuitive and user-friendly interfaces for web and mobile apps.',
      price: '$800',
      image: 'https://via.placeholder.com/100',
    },
  ];

  private reviewService = inject(ReviewsService);
  reviews: Review[] = this.reviewService.getRandomReviews();

  getStars(rating: number): string[] {
    const filledStars = Array(rating).fill('filled');
    const emptyStars = Array(5 - rating).fill('empty');
    return [...filledStars, ...emptyStars];
  }
}
