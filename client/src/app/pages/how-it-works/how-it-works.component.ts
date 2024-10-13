import { Component } from '@angular/core';
import {AccordionModule} from "primeng/accordion";

@Component({
  selector: 'app-how-it-works',
  standalone: true,
  imports: [AccordionModule],
  templateUrl: './how-it-works.component.html',
  styleUrl: './how-it-works.component.scss'
})
export class HowItWorksComponent {

}
