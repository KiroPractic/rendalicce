import { CommonModule } from '@angular/common';
import { Component, effect, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from '../../services/jwt.service';
import { ProfileService } from '../profile/profile.service';
import { ChatService } from './chat.service';
import { User } from '../../model/user.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit {
  private activateRoute = inject(ActivatedRoute);
  private jwtService = inject(JwtService);
  private profileService = inject(ProfileService);
  private router = inject(Router);
  private chatService = inject(ChatService);
  public chats = [];
  selectedUser = null;
  messages = [];
  otherParticipants = [];
  newMessage = '';
  user: User | null = null;

  async ngOnInit() {
    await this.loadUser();
    this.getChats();
  }

  async loadUser() {
    const token = this.jwtService.token();
    if (token) {
      this.user = await this.profileService.getUser().toPromise();
    }
  }

  getChats() {
    this.chatService.getChats().subscribe({
      next: (data: any) => {
        this.chats = data.chats;
        this.getUsersChats();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getUsersChats() {
    const participants = this.chats.flatMap((chat) =>
      chat.participants
        .filter((participant) => participant.id !== this.user.id)
        .map((participant) => ({
          ...participant,
          chatId: chat.id,
        }))
    );

    const uniqueParticipants = Array.from(
      new Map(
        participants.map((participant) => [participant.id, participant])
      ).values()
    );

    this.otherParticipants = uniqueParticipants;

    this.activateRoute.params.subscribe({
      next: (params) => {
        if (params['userId']) {
          this.getSelectedUserByUserId(params['userId']);
        }
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  getSelectedUserByUserId(userId: string) {
    this.selectedUser = this.otherParticipants.find(
      (participant) => participant.id === userId
    );
    if (!this.selectedUser) {
      this.createChat(userId);
    }
    this.getMessages(this.selectedUser.chatId);
  }

  getSelectedUser(userId: string, chatId: string) {
    this.selectedUser = this.otherParticipants.find(
      (participant) => participant.id === userId
    );
    if (!this.selectedUser) {
      this.createChat(userId);
    }
    this.getMessages(chatId);
    this.router.navigate(['/chat', userId], {
      relativeTo: this.activateRoute,
    });
  }

  getMessages(chatId: string) {
    this.chatService.getMessages(chatId).subscribe({
      next: (data: any) => {
        this.messages = data.chat.messages.sort(
          (a, b) =>
            new Date(a.createdOn).getTime() - new Date(b.createdOn).getTime()
        );
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  openProfile(userId) {
    this.router.navigate(['/profile', userId]);
  }

  sendMessage(chatId, message) {
    this.chatService.sendMessage(message, chatId).subscribe({
      next: (data: any) => {
        this.newMessage = '';
        this.messages = data.chat.messages.sort(
          (a, b) =>
            new Date(a.createdOn).getTime() - new Date(b.createdOn).getTime()
        );
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  createChat(userId) {
    this.chatService.getMessagesByUserId(userId).subscribe({
      next: (data: any) => {
        this.getChats();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
}
