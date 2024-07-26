import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayContactComponent } from './display-contact.component';
import { HttpClient } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { ContactService } from 'src/app/services/contact.service';
import { Contacts } from 'src/app/models/contact.model';
import { of } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactCountryState } from 'src/app/models/contact-country-state.model';

describe('DisplayContactComponent', () => {
  let component: DisplayContactComponent;
  let fixture: ComponentFixture<DisplayContactComponent>;
  let contactService: jasmine.SpyObj<ContactService>;
  let route: ActivatedRoute;
  const mockContact: ContactCountryState= {
    contactId: 0,
    countryId: 0,
    stateId: 0,
    firstName: '',
    lastName: '',
    contactNumber: 0,
    email: '',
    contactDescription: '',
    profilePic: '',
    gender: '',
    address: '',
    favourite: true,
    imageByte:'',
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0
    },
    country: {
      countryId: 0,
      countryName: ''
    },
  };

  beforeEach(() => {
    const contactServiceSpy = jasmine.createSpyObj('ContactService', ['GetContactWithStateCountryById']);
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule],
      declarations: [DisplayContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ id: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(DisplayContactComponent);
    component = fixture.componentInstance;
    contactService = TestBed.inject(ContactService) as jasmine.SpyObj<ContactService>;
    route = TestBed.inject(ActivatedRoute);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize contactId from route params and load contact details', () => {
    // Arrange
    const mockResponse: ApiResponse<ContactCountryState> = { success: true, data: mockContact, message: '' };
    contactService.GetContactWithStateCountryById.and.returnValue(of(mockResponse));
 
    // Act
    fixture.detectChanges(); // ngOnInit is called here
 
    // Assert
    expect(component.contactId).toBe(1);
    expect(contactService.GetContactWithStateCountryById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });
 
  it('should log "Completed" when contact loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<ContactCountryState> = { success: true, data: mockContact, message: '' };
    contactService.GetContactWithStateCountryById.and.returnValue(of(mockResponse));
    spyOn(console, 'log');
 
    // Act
    component.loadCategoryDetail(1); 
    fixture.detectChanges();
 
    // Assert
    expect(console.log).toHaveBeenCalledWith('Completed');
  });
});
