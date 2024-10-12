import { Component, effect, inject, input, OnInit, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SaveButtonComponent } from '../buttons/save-button/save-button.component';
import { CloseButtonComponent } from '../buttons/close-button/close-button.component';
import { ProfileService } from "../../pages/profile/profile.service";
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-edit-profile-modal',
  standalone: true,
  imports: [FormsModule, SaveButtonComponent, CloseButtonComponent],
  templateUrl: './edit-profile-modal.component.html',
  styleUrls: ['./edit-profile-modal.component.scss'],
})
export class EditProfileModalComponent implements OnInit {
  #service: ProfileService = inject(ProfileService);
  #sanitizer: DomSanitizer = inject(DomSanitizer);

  user = input.required<any>();
  updateUser = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    description: '',
    profilePhotoBase64: '',
    profilePhotoFile: null as File | null,
  };

  closeModalWithData = output<any>();
  closeModal = output();

  ngOnInit(): void {
    this.updateUser = {
      firstName: this.user()?.user.firstName,
      lastName: this.user().user.lastName,
      email: this.user().user.email,
      phoneNumber: this.user().user.phoneNumber,
      description: this.user().user.description,
      profilePhotoBase64: this.user().user.profilePhotoBase64,
      profilePhotoFile: null,
    };
  }

  saveChanges() {
    const formData = new FormData();

    formData.append('firstName', this.updateUser.firstName);
    formData.append('lastName', this.updateUser.lastName);
    formData.append('email', this.updateUser.email);
    formData.append('phoneNumber', this.updateUser.phoneNumber);
    formData.append('description', this.updateUser.description);

    if (this.updateUser.profilePhotoFile) {
      formData.append('profilePhoto', this.updateUser.profilePhotoFile);
    }

    this.closeModalWithData.emit(formData);
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.updateUser.profilePhotoFile = file;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.updateUser.profilePhotoBase64 = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  closeEditModal() {
    this.closeModal.emit();
  }

  getImageSrc(): SafeUrl {
    const imageData = this.updateUser.profilePhotoBase64 || this.user()?.user.profilePhotoBase64;
    if (imageData) {
      // Check if the string already starts with 'data:'
      if (imageData.startsWith('data:')) {
        return this.#sanitizer.bypassSecurityTrustUrl(imageData);
      } else {
        return this.#sanitizer.bypassSecurityTrustUrl('data:image/jpeg;base64,' + imageData);
      }
    }
    return '';
  }
}
