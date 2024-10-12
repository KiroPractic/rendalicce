import {Component, inject} from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators,} from '@angular/forms';
import {JwtService} from "../../../services/jwt.service";
import {Router} from "@angular/router";
import {AuthenticationService} from "../authentication.service";
import {FloatLabelModule} from "primeng/floatlabel";
import {PasswordModule} from "primeng/password";
import {ButtonModule} from "primeng/button";
import {InputTextModule} from "primeng/inputtext";
import {NavbarComponent} from "../../../components/navbar/navbar.component";
import {GlobalMessageService} from "../../../services/global-message.service";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, FloatLabelModule, PasswordModule, ButtonModule, InputTextModule, NavbarComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  #fb: FormBuilder = inject(FormBuilder);
  #authenticationService: AuthenticationService = inject(AuthenticationService);
  #jwtService: JwtService = inject(JwtService);
  #router: Router = inject(Router);
  #globalMessageService: GlobalMessageService = inject(GlobalMessageService);

  loginForm: FormGroup;
  error: string = '';

  constructor() {
    this.loginForm = this.#fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  login() {
    this.loginForm.markAllAsTouched();
    if (this.loginForm.invalid) return;
    const payload = {
      email: this.loginForm.get('email').value,
      password: this.loginForm.get('password').value,
      rememberMe: this.loginForm.get('rememberMe').value
    }

    this.#authenticationService.login(payload).subscribe(
      (response: any) => {
        this.#jwtService.set(response.token);
        this.#router.navigateByUrl("/").then(() => {
          this.#globalMessageService.showInformationMessage({title: '', content: 'Uspješna prijava, dobrodošli'});
        });
      }
    );
  }
}
