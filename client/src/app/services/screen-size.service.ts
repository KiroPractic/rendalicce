import { computed, Injectable, signal, WritableSignal, OnDestroy } from "@angular/core";
import { Subject, tap, Subscription } from "rxjs";

enum ScreenSize {
  Mobile = 1,
  Desktop
}

@Injectable({
  providedIn: 'root'
})
export class ScreenSizeService implements OnDestroy {
  #screenSize: WritableSignal<ScreenSize> = signal<ScreenSize>(undefined);

  isMobile = computed(() => this.#screenSize() === ScreenSize.Mobile);
  isDesktop = computed(() => this.#screenSize() === ScreenSize.Desktop);

  updateScreenSizeRequest$: Subject<void> = new Subject<void>();

  #updateScreenSize$ = this.updateScreenSizeRequest$.pipe(
    tap(() => this.setScreenSize())
  );

  private subscription: Subscription;

  constructor() {
    this.subscription = this.#updateScreenSize$.subscribe();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  private setScreenSize() {
    this.#screenSize.set(window.innerWidth <= 768 ? ScreenSize.Mobile : ScreenSize.Desktop);
  }
}