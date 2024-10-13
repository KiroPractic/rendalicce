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
  isRequestingCoins = false;
  isRequestingService = false;
  requestedCoins = 0;
  coinRequestDescription = '';
  serviceCompleted = false;
  requestedServices = [];
  usersServices = [];
  selectedService = '';
  requestedServiceId = '';
  usersServiceId = '';
  requestServiceDescription = '';
  showCompleteServiceButton = false;

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

  toggleCoinRequest() {
    this.isRequestingCoins = !this.isRequestingCoins;
  }

  completeService(transactionId) {
    this.chatService.completeTransaction(transactionId).subscribe({
      next: (data: any) => {
        this.getMessages(this.selectedUser.chatId);
        this.showCompleteServiceButton = false;
        this.isLoading = false;
      }
    });
  }

  toggleServiceRequest() {
    this.isRequestingService = !this.isRequestingService;
    this.profileService.getAccountInformation(this.selectedUser.id).subscribe({
      next: (data: any) => {
        this.requestedServices = data.serviceProviders;
      },
    });
    this.profileService.getAccountInformation(this.user.id).subscribe({
      next: (data: any) => {
        this.usersServices = data.serviceProviders;
      },
    });
  }

  sendCoinRequest() {
    if (this.requestedCoins > 0 && this.coinRequestDescription) {
      const coinMessage = `${this.user?.firstName} je zatražio ${this.requestedCoins} token(a): ${this.coinRequestDescription}`;
      const serviceTransaction = {
        participants: [
          {
            credits: this.requestedCoins,
            serviceProviderId: null,
            userId: this.selectedUser.id,
          },
          {
            credits: null,
            serviceProviderId: null,
            userId: this.user.id,
          },
        ],
      };

      this.sendCoinMessageRequest(
        this.selectedUser.chatId,
        coinMessage,
        serviceTransaction
      );

      this.requestedCoins = 0;
      this.coinRequestDescription = '';
      this.isRequestingCoins = false;
    } else {
      console.error('Invalid coin request');
    }
  }

  sendServiceRequest() {
    if (
      this.requestedServiceId &&
      this.requestServiceDescription &&
      this.usersServiceId
    ) {
      const serviceMessage = `${this.user?.firstName} je zatražio uslugu: ${this.requestServiceDescription}`;
      const serviceTransaction = {
        participants: [
          {
            credits: null,
            serviceProviderId: this.requestedServiceId,
            userId: this.selectedUser.id,
          },
          {
            credits: null,
            serviceProviderId: this.usersServiceId,
            userId: this.user.id,
          },
        ],
      };

      this.sendCoinMessageRequest(
        this.selectedUser.chatId,
        serviceMessage,
        serviceTransaction
      );

      this.requestedServiceId = '';
      this.requestServiceDescription = '';
      this.isRequestingService = false;
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
        this.messages.forEach((message) => {
          if (
            message.serviceTransaction &&
            message.serviceTransaction.completed
          ) {
            this.serviceCompleted = true;
            message.serviceCompleted = true;
          }

          if(message.serviceTransaction && !message.serviceTransaction.completed) {
            this.serviceCompleted = false;
            message.serviceCompleted = false;
            this.showCompleteServiceButton = true;
          }
        });
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
        this.isLoading = false;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  sendCoinMessageRequest(
    chatId: string,
    message: string,
    serviceTransaction: any
  ) {
    this.isLoading = true;
    this.chatService
      .sendCoinMessageRequest(message, chatId, serviceTransaction)
      .subscribe({
        next: (data: any) => {
          this.newMessage = '';
          this.getMessages(chatId);
          this.shouldScrollToBottom = true;
          this.isLoading = false;
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
