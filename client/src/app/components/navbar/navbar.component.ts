import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  isNavbarOpen = false;

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
