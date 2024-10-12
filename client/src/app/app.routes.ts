import {Routes} from '@angular/router';
import {LoginComponent} from './pages/authentication/login/login.component';
import {HomeComponent} from './pages/home/home.component';
import {RegisterComponent} from './pages/authentication/register/register.component';
import {
  CreateOrUpdateServiceProviderComponent
} from "./pages/create-or-update-service-provider/create-or-update-service-provider.component";

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'service-provider/:state/:id', component: CreateOrUpdateServiceProviderComponent}
];
