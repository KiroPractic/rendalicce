import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent {
  private activateRoute = inject(ActivatedRoute);
  private router = inject(Router);
  users = [
    {
      id: '1',
      firstName: 'John',
      lastName: 'Doe',
      image: 'https://via.placeholder.com/120',
      description:
        'Full-stack developer with a passion for building scalable applications.',
      phone: '+123456789',
      email: 'test@gmail.com',
    },
  ];
  selectedUser = null;
  messages = [];
  newMessage = '';

  currentUser = {
    id: 1,
    name: 'My Name',
    image: 'path_to_current_user_image',
  };

  constructor() {
    this.activateRoute.params.subscribe({
      next: (params) => {
        if (params['userId']) {
          this.getSelectedUser(params['userId']);
        }
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getSelectedUser(userId: string) {
    this.selectedUser = this.getUserById(userId);
    this.loadMessagesForUser(userId);

    this.router.navigate(['/chat', userId], {
      relativeTo: this.activateRoute,
    });
  }
  getUserById(userId: string) {
    return this.users.find((user) => user.id === userId);
  }

  loadMessagesForUser(userId: string) {
    this.messages = [
      { senderId: 1, text: 'Kupujem kauč', date: new Date() },
      { senderId: userId, text: 'Dobro', date: new Date() },
    ];
  }

  openProfile(userId: string) {
    this.router.navigate(['/profile', userId]);
  }

  sendMessage() {
    if (this.newMessage.trim()) {
      this.messages.push({
        senderId: this.currentUser.id,
        text: this.newMessage,
      });
      this.newMessage = '';
    }
  }
}
