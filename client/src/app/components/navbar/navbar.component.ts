import {Component, inject} from '@angular/core';
import {RouterLink} from "@angular/router";
import {AuthenticationService} from "../../pages/authentication/authentication.service";
import {JwtService} from "../../services/jwt.service";

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
  isNavbarOpen = false;
  jwtService: JwtService = inject(JwtService);
  authService: AuthenticationService = inject(AuthenticationService);

  toggleNavbar() {
    this.isNavbarOpen = !this.isNavbarOpen;
  }

  toggleDropdown(event: Event) {
    const target = event.currentTarget as HTMLElement;
    const dropdown = target.closest('.dropdown');
    dropdown?.classList.toggle('open');

    event.preventDefault();
  }
}
