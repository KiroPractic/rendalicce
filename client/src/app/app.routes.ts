import {Routes} from '@angular/router';
import {LoginComponent} from './pages/authentication/login/login.component';
import {HomeComponent} from './pages/home/home.component';
import {RegisterComponent} from './pages/authentication/register/register.component';
import {ProfileComponent} from './pages/profile/profile.component';
import {AboutComponent} from './pages/about/about.component';
import {ContactComponent} from './pages/contact/contact.component';
import {
  CreateOrUpdateServiceProviderComponent
} from './pages/create-or-update-service-provider/create-or-update-service-provider.component';
import {ChatComponent} from './pages/chat/chat.component';
import {
  CreateOrUpdateServiceSeekerComponent
} from "./pages/create-or-update-service-seeker/create-or-update-service-seeker.component";
import {ServiceProvidersListingComponent} from "./pages/service-providers-listing/service-providers-listing.component";
import {ServiceSeekersListingComponent} from "./pages/service-seekers-listing/service-seekers-listing.component";
import {ViewServiceProviderComponent} from "./pages/view-service-provider/view-service-provider.component";
import {ViewServiceSeekerComponent} from "./pages/view-service-seeker/view-service-seeker.component";
import {HowItWorksComponent} from "./pages/how-it-works/how-it-works.component";

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile/:id', component: ProfileComponent },
  { path: 'chat', component: ChatComponent },
  { path: 'chat/:userId', component: ChatComponent },
  { path: 'about', component: AboutComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'service-provider/:state',
    component: CreateOrUpdateServiceProviderComponent,
  },
  {
    path: 'service-provider/:state/:id',
    component: CreateOrUpdateServiceProviderComponent,
  },
  {
    path: 'service-seeker/:state',
    component: CreateOrUpdateServiceSeekerComponent,
  },
  {
    path: 'service-seeker/:state/:id',
    component: CreateOrUpdateServiceSeekerComponent,
  },
  {path: 'service-providers', component: ServiceProvidersListingComponent},
  {path: 'service-seekers', component: ServiceSeekersListingComponent},
  {path: 'service-providers/:id', component: ViewServiceProviderComponent},
  {path: 'service-seekers/:id', component: ViewServiceSeekerComponent},
  {path: 'how-it-works', component: HowItWorksComponent},
  { path: '**', redirectTo: '' },
];
