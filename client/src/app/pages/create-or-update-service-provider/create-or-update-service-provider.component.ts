import {Component, inject} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {FloatLabelModule} from "primeng/floatlabel";
import {InputTextModule} from "primeng/inputtext";
import {DropdownModule} from "primeng/dropdown";
import {serviceCategories} from "../../utils/service-categories";
import {InputTextareaModule} from 'primeng/inputtextarea';
import {TextareaModule} from "primeng/textarea";
import {InputNumberModule} from 'primeng/inputnumber';
import {MapInputComponent} from "../../components/map-input/map-input.component";
import {ButtonModule} from "primeng/button";

@Component({
  selector: 'app-create-or-update-service-provider',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FloatLabelModule,
    InputTextModule,
    DropdownModule,
    InputTextareaModule,
    TextareaModule,
    InputNumberModule,
    MapInputComponent,
    ButtonModule
  ],
  templateUrl: './create-or-update-service-provider.component.html',
  styleUrl: './create-or-update-service-provider.component.scss'
})
export class CreateOrUpdateServiceProviderComponent {
  #fb: FormBuilder = inject(FormBuilder);
  createOrUpdateServiceProviderForm: FormGroup;
  serviceCategories = serviceCategories;

  constructor() {
    this.createOrUpdateServiceProviderForm = this.#fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: [null, Validators.required],
      tags: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [null, Validators.required],
      companyName: [''],
      geoLocation: ['', Validators.required]
    });
  }

  createOrUpdate() {


    console.log(this.createOrUpdateServiceProviderForm.get('geoLocation').value);
  }
}
