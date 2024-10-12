import {Component, inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {FloatLabelModule} from "primeng/floatlabel";
import {InputTextModule} from "primeng/inputtext";
import {serviceCategories} from "../../utils/service-categories";
import {InputTextareaModule} from 'primeng/inputtextarea';
import {TextareaModule} from "primeng/textarea";
import {InputNumberModule} from 'primeng/inputnumber';
import {MapInputComponent} from "../../components/map-input/map-input.component";
import {ButtonModule} from "primeng/button";
import {ActivatedRoute, Router} from "@angular/router";
import {CreateOrUpdateServiceProviderService} from "./create-or-update-service-provider.service";
import {Select} from "primeng/select";
import {NgClass} from "@angular/common";
import {TagInputComponent} from "../../components/tag-input/tag-input.component";
import {paymentTypes} from '../../utils/payment-types';
import {GlobalMessageService} from "../../services/global-message.service";

@Component({
  selector: 'app-create-or-update-service-provider',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FloatLabelModule,
    InputTextModule,
    InputTextareaModule,
    TextareaModule,
    InputNumberModule,
    MapInputComponent,
    ButtonModule,
    Select,
    NgClass,
    TagInputComponent
  ],
  templateUrl: './create-or-update-service-provider.component.html',
  styleUrls: ['./create-or-update-service-provider.component.scss']
})
export class CreateOrUpdateServiceProviderComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private route: ActivatedRoute = inject(ActivatedRoute);
  private service: CreateOrUpdateServiceProviderService = inject(CreateOrUpdateServiceProviderService);
  private router: Router = inject(Router);
  private globalMessageService: GlobalMessageService = inject(GlobalMessageService);

  state: string;
  createOrUpdateServiceProviderForm: FormGroup;
  serviceCategories = serviceCategories;
  paymentTypes = paymentTypes;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  isDragOver: boolean = false;

  constructor() {
    this.createOrUpdateServiceProviderForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: [null, Validators.required],
      tags: [[], Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [null, Validators.required],
      companyName: [''],
      paymentType: [{
        name: 'Plaćanje po satu',
        value: 'Plaćanje po satu'
      }, Validators.required],
      geoLocation: ['', Validators.required],
      price: [null],
    });
  }

  ngOnInit(): void {
    this.state = this.route.snapshot.paramMap.get('state');

    if (this.state === 'edit') {
      const id = this.route.snapshot.paramMap.get('id');
      this.service.getById(id).subscribe((response: any) => {
        const serviceProvider = response.serviceProvider;

        const tagsArray = serviceProvider.tags ? serviceProvider.tags.split(',') : [];
        const [latitude, longitude] = serviceProvider.geolocation.split(',').map(Number);

        this.createOrUpdateServiceProviderForm.patchValue({
          name: serviceProvider.name,
          description: serviceProvider.description,
          category: this.serviceCategories.find((category) => category.value === serviceProvider.category),
          paymentType: this.paymentTypes.find((paymentType) => paymentType.value === serviceProvider.paymentType),
          tags: tagsArray,
          email: serviceProvider.email,
          phoneNumber: serviceProvider.phoneNumber,
          companyName: serviceProvider.companyName,
          geoLocation: [latitude, longitude],
          price: serviceProvider.price
        });

        if (serviceProvider.headerPhotoBase64) {
          this.imagePreview = `data:image/jpeg;base64,${serviceProvider.headerPhotoBase64}`;
        } else {
          this.imagePreview = null;
        }
      });
    }
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.handleFile(file);
    }
  }

  handleFile(file: File): void {
    this.selectedFile = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    reader.readAsDataURL(file);
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;

    const file = event.dataTransfer?.files[0];
    if (file) {
      this.handleFile(file);
    }
  }

  removeImage(): void {
    this.selectedFile = null;
    this.imagePreview = null;
    this.createOrUpdateServiceProviderForm.patchValue({image: null});
  }

  createOrUpdate() {
    this.createOrUpdateServiceProviderForm.markAllAsTouched();
    if (this.createOrUpdateServiceProviderForm.invalid) return;

    const formData = new FormData();

    Object.keys(this.createOrUpdateServiceProviderForm.value).forEach((key) => {
      let value = this.createOrUpdateServiceProviderForm.value[key];

      if (key === 'category' && value && value.value) {
        formData.append('Category', value.value);
      } else if (key === 'paymentType' && value && value.value) {
        formData.append('PaymentType', value.value);
      } else if (key === 'tags' && Array.isArray(value)) {
        formData.append('Tags', value.join(','));
      } else {
        formData.append(this.capitalizeFirstLetter(key), value);
      }
    });

    if (this.selectedFile) {
      formData.append('HeaderPhoto', this.selectedFile);
    }

    if (this.state === 'create') {
      this.service.create(formData).subscribe((response: any) => {
        this.router.navigateByUrl('/service-provider/edit/' + response.id).then(() => {
          this.globalMessageService.showSuccessMessage({title: '', content: 'Uspješno ste kreirali uslugu'});
        });
      });
    } else {
      const id = this.route.snapshot.paramMap.get('id');
      this.service.update(id, formData).subscribe((response: any) => {
        this.router.navigateByUrl('/service-provider/edit/' + response.id).then(() => {
          this.globalMessageService.showSuccessMessage({title: '', content: 'Uspješno ste ažurirali uslugu'});
        });
      });
    }
  }

  capitalizeFirstLetter(string: string): string {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }
}
