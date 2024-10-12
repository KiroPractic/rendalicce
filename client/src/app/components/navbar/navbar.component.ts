import {Component, effect, inject, OnInit} from '@angular/core';
import {RouterLink} from "@angular/router";
import {AuthenticationService} from "../../pages/authentication/authentication.service";
import {JwtService} from "../../services/jwt.service";
import {ProfileService} from "../../pages/profile/profile.service";
import {User} from "../../model/user.model";

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  jwtService: JwtService = inject(JwtService);
  profileService: ProfileService = inject(ProfileService);
  authService: AuthenticationService = inject(AuthenticationService);

  user: User | null = null;

  constructor() {
    effect(() => {
      if (!this.jwtService.token()) {
        return;
      }
      if (this.jwtService.token()) {
        this.profileService.getUser().subscribe(user => {console.log(user);this.user = user});
      }
    })
  }
}
