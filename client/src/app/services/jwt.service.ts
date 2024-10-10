import { Injectable, Signal, signal, WritableSignal } from "@angular/core";
import { jwtDecode } from "jwt-decode";
import { Token } from "../model/token.model";

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  #rawToken: WritableSignal<string> = signal(null);
  #token: WritableSignal<Token> = signal<Token>(null);

  readonly rawToken: Signal<string> = this.#rawToken.asReadonly();
  readonly token: Signal<Token> = this.#token.asReadonly();

  constructor() {
    this.#token.set(this.get());
    this.#rawToken.set(localStorage.getItem('token'));
  }

  private get(): Token | null {
    if (localStorage.getItem('token'))
      return jwtDecode<Token>(localStorage.getItem('token'));
    return null;
  }

  set(token: string): void {
    localStorage.setItem('token', token);
    this.#token.set(this.get());
    this.#rawToken.set(token);
  }

  remove(): void {
    localStorage.removeItem('token');
    this.#token.set(null);
    this.#rawToken.set(null);
  }

  isTokenExpired(): boolean {
    const token = this.#token();
    if (!token || !token.exp) {
      return true;
    }
    const currentTime = Math.floor(Date.now() / 1000);
    return token.exp < currentTime;
  }
}
