import {Component, inject} from '@angular/core';
import {ActivatedRoute, RouterLink} from "@angular/router";
import {ViewServiceProviderService} from "./view-service-provider.service";
import {DatePipe, DecimalPipe, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-view-service-provider',
  standalone: true,
  imports: [
    DatePipe,
    NgIf,
    NgForOf,
    DecimalPipe,
    RouterLink
  ],
  templateUrl: './view-service-provider.component.html',
  styleUrl: './view-service-provider.component.scss'
})
export class ViewServiceProviderComponent {
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  service = inject(ViewServiceProviderService)
  serviceProvider: any;

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.service.getById(params['id']).subscribe((service: any) => {
        this.serviceProvider = service.serviceProvider;
      })
    });
  }

  onAddReview() {
    console.log('Add Review');
  }
}
