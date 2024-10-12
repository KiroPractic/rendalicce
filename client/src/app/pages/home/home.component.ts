import { Component } from '@angular/core';
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  categories = [
    { name: 'Građevinarstvo', icon: 'pi-graduation-cap'},
    { name: 'Građevinarstvo', icon: 'pi-graduation-cap'},
    { name: 'Građevinarstvo', icon: 'pi-graduation-cap'},
    { name: 'Građevinarstvo', icon: 'pi-graduation-cap'},
    { name: 'Građevinarstvo', icon: 'pi-graduation-cap'},
  ]

  benefits = [
    { icon: 'pi-users', title: "Široka mreža stručnjaka", description: "Pristupite raznovrsnim vještinama i uslugama iz cijele Hrvatske." },
    { icon: 'pi-shield', title: "Sigurnost i povjerenje", description: "Provjereni pružatelji usluga i sustav ocjenjivanja za vaš mir uma." },
    { icon: 'pi-star', title: "Kvaliteta usluge", description: "Visoki standardi i zadovoljstvo korisnika su nam prioritet." },
    { icon: 'pi-search', title: "Brzo i jednostavno", description: "Pronađite idealnog pružatelja usluge u nekoliko klikova." }
  ];
}
