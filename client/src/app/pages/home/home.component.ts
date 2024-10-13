import {Component, inject} from '@angular/core';
import { NgClass } from '@angular/common';
import {Router} from "@angular/router";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NgClass, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  router = inject(Router);
  searchText: string = '';
  categories = [
    { name: 'Građevinarstvo', icon: 'pi-building' },
    { name: 'Informatika', icon: 'pi-desktop' },
    { name: 'Obrazovanje', icon: 'pi-graduation-cap' },
    { name: 'Marketing', icon: 'pi-chart-line' },
    { name: 'Pravo', icon: 'pi-briefcase' },
  ];

  benefits = [
    {
      icon: 'pi-users',
      title: 'Široka mreža stručnjaka',
      description:
        'Pristupite raznovrsnim vještinama i uslugama iz cijele Hrvatske.',
    },
    {
      icon: 'pi-shield',
      title: 'Sigurnost i povjerenje',
      description:
        'Provjereni pružatelji usluga i sustav ocjenjivanja za vaš mir uma.',
    },
    {
      icon: 'pi-star',
      title: 'Kvaliteta usluge',
      description:
        'Visoki standardi i zadovoljstvo korisnika su nam prioritet.',
    },
    {
      icon: 'pi-search',
      title: 'Brzo i jednostavno',
      description: 'Pronađite idealnog pružatelja usluge u nekoliko klikova.',
    },
  ];

  search() {
    this.router.navigate(['/services'], { queryParams: { searchText: this.searchText } });
  }
}
