import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninComponent } from './Component/auth/signin/signin.component';
import { SignupComponent } from './Component/auth/signup/signup.component';
import { ContactComponent } from './Component/contacts/contact/contact.component';
import { HomeComponent } from './Component/home/home.component';
import { PrivacyComponent } from './Component/privacy/privacy.component';
import { AddContactComponent } from './Component/contacts/add-contact/add-contact.component';
import { UpdateContactComponent } from './Component/contacts/update-contact/update-contact.component';
import { DisplayContactComponent } from './Component/contacts/display-contact/display-contact.component';
import { SignupsuccessComponent } from './Component/auth/signupsuccess/signupsuccess.component';
import { authGuard } from './guards/auth.guard';
import { PaginatedContactComponent } from './Component/contacts/paginated-contact/paginated-contact.component';
import { ForgotPasswordComponent } from './Component/auth/forgot-password/forgot-password.component';
import { UpdatePasswordComponent } from './Component/auth/update-password/update-password.component';
import { PasswordupdateSuccessComponent } from './Component/auth/passwordupdate-success/passwordupdate-success.component';
import { FavouriteContactComponent } from './Component/contacts/favourite-contact/favourite-contact.component';
import { SearchContactComponent } from './Component/contacts/search-contact/search-contact.component';
import { ChangePasswordComponent } from './Component/auth/change-password/change-password.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { UpdateUserComponent } from './Component/auth/update-user/update-user.component';
import { UserDetailComponent } from './Component/auth/user-detail/user-detail.component';
import { FilteredContactComponent } from './Component/contacts/filtered-contact/filtered-contact.component';
import { AddContactRfComponent } from './Component/contacts/add-contact-rf/add-contact-rf.component';
import { UpdateContactRfComponent } from './Component/contacts/update-contact-rf/update-contact-rf.component';
import { BirthdaybasedreportComponent } from './Component/report/birthdaybasedreport/birthdaybasedreport.component';
import { StatebasedreportComponent } from './Component/report/statebasedreport/statebasedreport.component';
import { CountrybasedreportComponent } from './Component/report/countrybasedreport/countrybasedreport.component';
import { GenderbasedreportComponent } from './Component/report/genderbasedreport/genderbasedreport.component';
import { ReportdeciderComponent } from './Component/report/reportdecider/reportdecider.component';


const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home',component:HomeComponent},
  {path:'privacy',component:PrivacyComponent},
  {path:'contact',component:ContactComponent,canActivate:[authGuard]},
  {path:'signup',component:SignupComponent},
  {path:'signin',component:SigninComponent},
  {path:'addContact',component:AddContactComponent,canActivate:[authGuard]},
  {path:'addContactusingrf',component:AddContactRfComponent,canActivate:[authGuard]},
  {path:'updateContact/:id',component:UpdateContactComponent,canActivate:[authGuard]},
  {path:'updateContactusingrf/:id',component:UpdateContactRfComponent,canActivate:[authGuard]},
  {path:'displayContact/:id',component:DisplayContactComponent,canActivate:[authGuard]},
  {path:'signupsuccess',component:SignupsuccessComponent},
  // {path:'paginatedContact',component:PaginatedContactComponent,canActivate:[authGuard]},
  {path:'forgotpassword',component:ForgotPasswordComponent},
  {path:'updateuserauthpassword/:username',component:UpdatePasswordComponent},
  {path:'passwordupdatesuccess',component:PasswordupdateSuccessComponent},
  {path:'favouriteContact',component:FavouriteContactComponent},
  {path:'searchContact',component:SearchContactComponent,canActivate:[authGuard]},
  {path:'changepassword',component:ChangePasswordComponent,canActivate:[authGuard]},
  {path:'updateUser/:id',component:UpdateUserComponent,canActivate:[authGuard]},  
  {path:'userdetail',component:UserDetailComponent,canActivate:[authGuard]},
  {path:'filteredContact',component:FilteredContactComponent,canActivate:[authGuard]},
  {path:'birthdayMonthBasedReport',component:BirthdaybasedreportComponent,canActivate:[authGuard]},
  {path:'stateBasedReport',component:StatebasedreportComponent,canActivate:[authGuard]},
  {path:'countryBasedReport',component:CountrybasedreportComponent,canActivate:[authGuard]},
  {path:'genderBasedReport',component:GenderbasedreportComponent,canActivate:[authGuard]},
  {path:'reportdeicder',component:ReportdeciderComponent,canActivate:[authGuard]},



];

@NgModule({
  imports: [RouterModule.forRoot(routes),NgbDropdownModule],
  
  exports: [RouterModule]
})
export class AppRoutingModule { }
