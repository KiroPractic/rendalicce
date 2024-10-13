import {Component, inject} from '@angular/core';
import {ButtonModule} from "primeng/button";
import {FloatLabelModule} from "primeng/floatlabel";
import {InputNumberModule} from "primeng/inputnumber";
import {InputTextModule} from "primeng/inputtext";
import {MapInputComponent} from "../../components/map-input/map-input.component";
import {ProgressSpinnerModule} from "primeng/progressspinner";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Select} from "primeng/select";
import {TagInputComponent} from "../../components/tag-input/tag-input.component";
import {TextareaModule} from "primeng/textarea";
import {ActivatedRoute, Router} from "@angular/router";
import {GlobalMessageService} from "../../services/global-message.service";
import { serviceCategories } from '../../utils/service-categories';
import {CreateOrUpdateServiceSeekerService} from "./create-or-update-service-seeker.service";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-create-or-update-service-seeker',
  standalone: true,
  imports: [
    ButtonModule,
    FloatLabelModule,
    InputNumberModule,
    InputTextModule,
    MapInputComponent,
    ProgressSpinnerModule,
    ReactiveFormsModule,
    Select,
    TagInputComponent,
    TextareaModule,
    NgClass
  ],
  templateUrl: './create-or-update-service-seeker.component.html',
  styleUrl: './create-or-update-service-seeker.component.scss'
})
export class CreateOrUpdateServiceSeekerComponent {
  private fb: FormBuilder = inject(FormBuilder);
  private route: ActivatedRoute = inject(ActivatedRoute);
  private service: CreateOrUpdateServiceSeekerService = inject(CreateOrUpdateServiceSeekerService);
  private router: Router = inject(Router);
  private globalMessageService: GlobalMessageService = inject(GlobalMessageService);

  state: string;
  createOrUpdateServiceSeekerForm: FormGroup;
  serviceCategories = serviceCategories;

  constructor() {
    this.createOrUpdateServiceSeekerForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: [null, Validators.required],
      tags: [[], Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [null, Validators.required],
      geoLocation: ['', Validators.required],
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

        this.createOrUpdateServiceSeekerForm.patchValue({
          name: serviceProvider.name,
          description: serviceProvider.description,
          category: this.serviceCategories.find((category) => category.value === serviceProvider.category),
          tags: tagsArray,
          email: serviceProvider.email,
          phoneNumber: serviceProvider.phoneNumber,
          geoLocation: [latitude, longitude],
        });
      });
    }
  }

  createOrUpdate() {
    this.createOrUpdateServiceSeekerForm.markAllAsTouched();
    if (this.createOrUpdateServiceSeekerForm.invalid) return;

    const formData = new FormData();

    Object.keys(this.createOrUpdateServiceSeekerForm.value).forEach((key) => {
      let value = this.createOrUpdateServiceSeekerForm.value[key];

      if (key === 'category' && value && value.value) {
        formData.append('Category', value.value);
      } else if (key === 'tags' && Array.isArray(value)) {
        formData.append('Tags', value.join(','));
      } else {
        formData.append(this.capitalizeFirstLetter(key), value);
      }
    });

    if (this.state === 'create') {
      this.service.create(formData).subscribe((response: any) => {
        this.router.navigateByUrl('/service-seeker/edit/' + response.id).then(() => {
          this.globalMessageService.showSuccessMessage({title: '', content: 'Uspješno ste kreirali uslugu'});
        });
      });
    } else {
      const id = this.route.snapshot.paramMap.get('id');
      this.service.update(id, formData).subscribe((response: any) => {
        this.router.navigateByUrl('/service-seeker/edit/' + response.id).then(() => {
          this.globalMessageService.showSuccessMessage({title: '', content: 'Uspješno ste ažurirali uslugu'});
        });
      });
    }
  }

  capitalizeFirstLetter(string: string): string {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }
}
