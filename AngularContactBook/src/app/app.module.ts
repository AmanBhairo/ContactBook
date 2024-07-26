import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http"; 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SigninComponent } from './Component/auth/signin/signin.component';
import { SignupComponent } from './Component/auth/signup/signup.component';
import { ContactComponent } from './Component/contacts/contact/contact.component';
import { HomeComponent } from './Component/home/home.component';
import { PrivacyComponent } from './Component/privacy/privacy.component';
import { AddContactComponent } from './Component/contacts/add-contact/add-contact.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UpdateContactComponent } from './Component/contacts/update-contact/update-contact.component';
import { DisplayContactComponent } from './Component/contacts/display-contact/display-contact.component';
import { SignupsuccessComponent } from './Component/auth/signupsuccess/signupsuccess.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { PaginatedContactComponent } from './Component/contacts/paginated-contact/paginated-contact.component';
import { ForgotPasswordComponent } from './Component/auth/forgot-password/forgot-password.component';
import { UpdatePasswordComponent } from './Component/auth/update-password/update-password.component';
import { PasswordupdateSuccessComponent } from './Component/auth/passwordupdate-success/passwordupdate-success.component';
import { FavouriteContactComponent } from './Component/contacts/favourite-contact/favourite-contact.component';
import { SearchContactComponent } from './Component/contacts/search-contact/search-contact.component';
import { ChangePasswordComponent } from './Component/auth/change-password/change-password.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavbarComponent } from './Component/navbar/navbar.component';
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


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PrivacyComponent,
    ContactComponent,
    SignupComponent,
    SigninComponent,
    AddContactComponent,
    UpdateContactComponent,
    DisplayContactComponent,
    SignupsuccessComponent,
    PaginatedContactComponent,
    ForgotPasswordComponent,
    UpdatePasswordComponent,
    PasswordupdateSuccessComponent,
    FavouriteContactComponent,
    SearchContactComponent,
    ChangePasswordComponent,
    NavbarComponent,
    UpdateUserComponent,
    UserDetailComponent,
    FilteredContactComponent,
    AddContactRfComponent,
    UpdateContactRfComponent,
    BirthdaybasedreportComponent,
    StatebasedreportComponent,
    CountrybasedreportComponent,
    GenderbasedreportComponent,
    ReportdeciderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule
  ],
  providers: [AuthService,{provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
