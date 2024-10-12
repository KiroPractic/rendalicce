import {Component, inject} from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators,} from '@angular/forms';
import {passwordValidator} from './password-validator';
import {passwordMatchValidator} from './password-match-validator';
import {AuthenticationService} from "../authentication.service";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  #fb: FormBuilder = inject(FormBuilder);
  #authenticationService: AuthenticationService = inject(AuthenticationService);
  registerForm: FormGroup;
  error: string = '';

  constructor() {
    this.registerForm = this.#fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, passwordValidator()]],
      confirmPassword: ['', Validators.required, passwordMatchValidator()],
    });
  }

  register() {
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) return;
    const payload = {
      email: this.registerForm.get('email').value,
      password: this.registerForm.get('password').value,
    };

    this.#authenticationService.register(payload).subscribe(
      (response) => {
        console.log(response);
      }
    );
  }
}
