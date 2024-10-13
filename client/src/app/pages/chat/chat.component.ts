import { CommonModule } from '@angular/common';
import {
  Component,
  effect,
  ElementRef,
  inject,
  OnInit,
  OnDestroy,
  ViewChild,
  AfterViewInit,
  AfterViewChecked,
} from '@angular/core';
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
export class ChatComponent implements OnInit, OnDestroy {
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
  isLoading = false;
  intervalId: any;
  isInitialLoad = true;
  @ViewChild('messageList') messageList!: ElementRef | null;
  private shouldScrollToBottom = false;

  async ngOnInit() {
    this.messages = [];
    await this.loadUser();
    this.getChats();
  }

  scrollToBottom() {
    if (this.messageList) {
      try {
        this.messageList.nativeElement.scrollTop =
          this.messageList.nativeElement.scrollHeight;
      } catch (err) {
        console.error('Scroll error', err);
      }
    }
  }

  isUserAtBottom(): boolean {
    if (this.messageList) {
      const threshold = 100;
      const position =
        this.messageList.nativeElement.scrollTop +
        this.messageList.nativeElement.clientHeight;
      const height = this.messageList.nativeElement.scrollHeight;
      return position > height - threshold;
    }
    return false;
  }

  ngAfterViewChecked() {
    if (
      this.shouldScrollToBottom &&
      !this.isUserAtBottom() &&
      this.isInitialLoad
    ) {
      this.scrollToBottom();
      this.shouldScrollToBottom = false;
      this.isInitialLoad = false;
    }
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
    this.messages = [];
    this.selectedUser = this.otherParticipants.find(
      (participant) => participant.id === userId
    );
    if (!this.selectedUser) {
      this.createChat(userId);
    }
    this.startMessagePolling(this.selectedUser.chatId);
  }

  getSelectedUser(userId: string, chatId: string) {
    this.messages = [];
    this.isInitialLoad = true;
    this.selectedUser = this.otherParticipants.find(
      (participant) => participant.id === userId
    );
    if (!this.selectedUser) {
      this.createChat(userId);
    }
    this.startMessagePolling(chatId);
    this.router.navigate(['/chat', userId], {
      relativeTo: this.activateRoute,
    });
  }

  startMessagePolling(chatId: string) {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
    this.getMessages(chatId);
    this.intervalId = setInterval(() => {
      this.getMessages(chatId);
    }, 1000);
  }

  getMessages(chatId: string) {
    this.chatService.getMessages(chatId).subscribe({
      next: (data: any) => {
        this.messages = data.chatMessages.sort(
          (a, b) =>
            new Date(a.createdOn).getTime() - new Date(b.createdOn).getTime()
        );
        this.shouldScrollToBottom = true;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  openProfile(userId: string) {
    this.router.navigate(['/profile', userId]);
  }

  sendMessage(chatId: string, message: string) {
    this.isLoading = true;
    this.chatService.sendMessage(message, chatId).subscribe({
      next: (data: any) => {
        this.newMessage = '';
        this.messages = data.chat.messages.sort(
          (a, b) =>
            new Date(a.createdOn).getTime() - new Date(b.createdOn).getTime()
        );
        this.shouldScrollToBottom = true;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  createChat(userId: string) {
    this.chatService.getMessagesByUserId(userId).subscribe({
      next: (data: any) => {
        this.getChats();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}
