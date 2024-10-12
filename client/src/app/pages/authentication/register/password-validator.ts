import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) {
      return { required: 'Password is required.' }; // Empty password
    }

    const errors: ValidationErrors = {};

    // Rule for minimum length of 8 characters
    if (value.length < 8) {
      errors['minLength'] = 'Password must be at least 8 characters long.';
    }

    // Rule for at least one digit
    if (!/\d/.test(value)) {
      errors['digit'] = 'Password must contain at least one digit.';
    }

    // Rule for at least one uppercase letter
    if (!/[A-Z]/.test(value)) {
      errors['uppercase'] = 'Password must contain at least one uppercase letter.';
    }

    // Rule for at least one lowercase letter
    if (!/[a-z]/.test(value)) {
      errors['lowercase'] = 'Password must contain at least one lowercase letter.';
    }

    // Rule for at least one special character
    if (!/[\W_]/.test(value)) {
      errors['specialCharacter'] = 'Password must contain at least one special character.';
    }

    // Return errors if any, or null if the password is valid
    return Object.keys(errors).length > 0 ? errors : null;
  };
}
