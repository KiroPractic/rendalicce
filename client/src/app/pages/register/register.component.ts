import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { passwordValidator } from './password-validator';
import { passwordMatchValidator } from './password-match-validator';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  registerForm: FormGroup;
  error: string = '';

  constructor(private fb: FormBuilder, private router: Router) {
    this.registerForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', [Validators.required, passwordValidator()]],
      confirmPassword: ['', Validators.required, passwordMatchValidator()],
    });
  }

  register() {}
}
