import {Component, inject} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ViewServiceProviderService} from "../view-service-provider/view-service-provider.service";
import {ViewServiceSeekerService} from "./view-service-seeker.service";
import {DatePipe, DecimalPipe, NgForOf, NgIf} from "@angular/common";
import {ProgressSpinnerModule} from "primeng/progressspinner";

@Component({
  selector: 'app-view-service-seeker',
  standalone: true,
  imports: [
    DatePipe,
    DecimalPipe,
    NgForOf,
    NgIf,
    ProgressSpinnerModule
  ],
  templateUrl: './view-service-seeker.component.html',
  styleUrl: './view-service-seeker.component.scss'
})
export class ViewServiceSeekerComponent {
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  service = inject(ViewServiceSeekerService)
  serviceProvider: any;
  isLoading = false;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.isLoading = true;
      this.service.getById(params['id']).subscribe((service: any) => {
        this.serviceProvider = service.serviceProvider;
        this.isLoading = false;
      }, () => {
        this.isLoading = false;
      });
    });
  }
}
