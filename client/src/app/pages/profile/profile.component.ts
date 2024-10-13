import { Component, inject } from '@angular/core';
import { User } from '../../model/user.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EditProfileModalComponent } from '../../components/edit-profile-modal/edit-profile-modal.component';
import { CreateOrUpdateServiceProviderService } from '../create-or-update-service-provider/create-or-update-service-provider.service';
import { ProfileService } from "./profile.service";
import { GlobalMessageService } from "../../services/global-message.service";
import { ToastModule } from "primeng/toast";
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ButtonModule } from "primeng/button";
import { ConfirmationService } from "primeng/api";
import { JwtService } from "../../services/jwt.service";
import {ProgressSpinnerModule} from "primeng/progressspinner";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, EditProfileModalComponent, ToastModule, ConfirmPopupModule, ButtonModule, RouterLink, ProgressSpinnerModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
  providers: [ConfirmationService]
})
export class ProfileComponent {
  user: any;
  isLoading = false; // Add loading state
  private routerService = inject(Router);
  private activateRoute = inject(ActivatedRoute);
  private service: ProfileService = inject(ProfileService);
  private globalMessageService = inject(GlobalMessageService);
  private createOrUpdateServiceProviderService = inject(CreateOrUpdateServiceProviderService);
  private confirmationService = inject(ConfirmationService);
  protected jwtService = inject(JwtService);
  isEditModalOpen = false;
  isEmailModalOpen = false;

  ngOnInit() {
    this.isLoading = true; // Set loading to true when starting the fetch
    this.activateRoute.params.subscribe((params: any) => {
      const userId = params.id;
      this.service.getAccountInformation(userId).subscribe(
        (user: User) => {
          this.user = user;
          this.isLoading = false; // Set loading to false once data is fetched
        },
        () => {
          this.isLoading = false; // Set loading to false if there's an error
        }
      );
    });
  }

  openEditModal() {
    this.isEditModalOpen = true;
  }

  closeEditModal() {
    this.isEditModalOpen = false;
  }

  openChat(userId: string) {
    this.routerService.navigate(['/chat', userId]);
  }

  getStars(rating: number): string[] {
    const filledStars = Array(rating).fill('filled');
    const emptyStars = Array(5 - rating).fill('empty');
    return [...filledStars, ...emptyStars];
  }

  calculateAverageRating(user) {
    let totalRating = 0;
    let totalReviews = 0;

    for (let serviceProvider of user?.serviceProviders || []) {
      for (let review of serviceProvider.reviews || []) {
        totalRating += review.rating;
        totalReviews++;
      }
    }

    return totalReviews > 0 ? (totalRating / totalReviews).toFixed(1) : 0;
  }

  openService(serviceId) {
    this.routerService.navigate(['/service', serviceId]);
  }

  deleteService(serviceId) {
    this.createOrUpdateServiceProviderService.delete(serviceId).subscribe(() => {
      this.service.getAccountInformation(this.user.user.id).subscribe((user: User) => {
        this.user = user;
      });
      this.globalMessageService.showInformationMessage({
        title: '',
        content: 'Uspješno ste obrisali uslugu',
      });
    });
  }

  confirm2(event: Event, serviceId: string) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Jeste li sigurni da želite obrisati ovu uslugu?',
      icon: 'pi pi-info-circle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger',
      },
      accept: () => {
        this.deleteService(serviceId);
      },
    });
  }

  updateUser(event: any) {
    this.service.updateAccountInformation(event).subscribe((user: User) => {
      this.service.getAccountInformation(this.user.user.id).subscribe((user: User) => {
        this.user = user;
      });
    });
    this.closeEditModal();
  }
}
