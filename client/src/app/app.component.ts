import { Component, HostListener, inject } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { PrimeNGConfig } from 'primeng/api';
import { Aura } from 'primeng/themes/aura';
import { definePreset } from 'primeng/themes';
import { filter } from 'rxjs';
import { ToastModule } from "primeng/toast";
import { ScreenSizeService } from './services/screen-size.service';
import {NavbarComponent} from "./components/navbar/navbar.component";

const CustomPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{indigo.50}',
      100: '{indigo.100}',
      200: '{indigo.200}',
      300: '{indigo.300}',
      400: '{indigo.400}',
      500: '{indigo.500}',
      600: '{indigo.600}',
      700: '{indigo.700}',
      800: '{indigo.800}',
      900: '{indigo.900}',
      950: '{indigo.950}'
    }
  },
});

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ToastModule, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  #config: PrimeNGConfig = inject(PrimeNGConfig);
  #router: Router = inject(Router);

  screenSizeService: ScreenSizeService = inject(ScreenSizeService);

  constructor() {
    this.#config.theme.set({
      preset: CustomPreset,
      options: {
        prefix: 'p',
        darkModeSelector: '',
        cssLayer: false
      }
    })

    this.#router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      window.scrollTo(0, 0);
    });
  }

  ngOnInit() {
    this.screenSizeService.updateScreenSizeRequest$.next();
  }

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.screenSizeService.updateScreenSizeRequest$.next();
  }
}
