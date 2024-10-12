import { Component } from '@angular/core';
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  socials = [
    { link: '#', icon: 'pi-facebook'},
    { link: '#', icon: 'pi-instagram'},
    { link: '#', icon: 'pi-twitter'},
    { link: '#', icon: 'pi-linkedin'},
    { link: '#', icon: 'pi-youtube'},
  ]
}
