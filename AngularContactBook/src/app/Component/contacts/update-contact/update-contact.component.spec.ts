import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateContactComponent } from './update-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Router, ActivatedRoute } from '@angular/router';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';
import { UpdateContact } from 'src/app/models/updateContact.model';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { States } from 'src/app/models/state.model';
import { Contacts } from 'src/app/models/contact.model';

describe('UpdateContactComponent', () => {
  let component: UpdateContactComponent;
  let fixture: ComponentFixture<UpdateContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let countryServiceSpy: jasmine.SpyObj<CountryService>;
  let stateServiceSpy: jasmine.SpyObj<StateService>;
  let routerSpy: Router;
  let route: ActivatedRoute;
  const mockContact: Contacts= {
    contactId: 1,
    firstName: 'Test',
    lastName: 'Test',
    profilePic: '',
    email: '',
    contactNumber: '',
    contactDescription:'',
    address: '',
    gender: '',
    favourite: false,
    imageByte:'',
    countryId: 0,
    country:{
      countryId:0,
      countryName:'',
    },
    stateId: 0,
    state:{
      stateId:0,
      stateName:'',
      countryId:0
    }
  }

  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['UpdateContact','GetContactById']);
    countryServiceSpy = jasmine.createSpyObj('CountryService', ['getAllCountries']);
    stateServiceSpy = jasmine.createSpyObj('StateService', ['getStatesByCountryId']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [UpdateContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: CountryService, useValue: countryServiceSpy },
        { provide: StateService, useValue: stateServiceSpy },
        {provide: ActivatedRoute,
          useValue:{
            params:of({id:1})
          }
        }
      ]
    });
    fixture = TestBed.createComponent(UpdateContactComponent);
    component = fixture.componentInstance;
    routerSpy = TestBed.inject(Router);
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  //country load
  it('should load countries on init', () => {
    // Arrange
    const mockResponseContactDetail: ApiResponse<Contacts> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.GetContactById.and.returnValue(of(mockResponseContactDetail));

  const mockCountries: Country[] = [
    { countryId: 1, countryName: 'Category 1'},
    { countryId: 2, countryName: 'Category 2'},
  ];
    const mockResponse: ApiResponse<Country[]> = { success: true, data: mockCountries, message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
 
    // Act
     fixture.detectChanges();

    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(component.countries).toEqual(mockCountries);
  });
 
  it('should handle failed country loading', () => {
    // Arrange
    const mockResponseContactDetail: ApiResponse<Contacts> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.GetContactById.and.returnValue(of(mockResponseContactDetail));
    const mockResponse: ApiResponse<Country[]> = { success: false, data: [], message: 'Failed to fetch countries' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch countries', 'Failed to fetch countries');
  });
 
  it('should handle error during country loading HTTP Error', () => {
    // Arrange
    const mockStates: States[] = [
      { countryId: 1, stateName: 'Category 1', stateId: 2},
      { countryId: 2, stateName: 'Category 2', stateId: 1},
    ];
    const mockStateResponse: ApiResponse<States[]> = { success: true, data: mockStates, message: '' };
    stateServiceSpy.getStatesByCountryId.and.returnValue(of(mockStateResponse));

    const mockResponse: ApiResponse<Contacts> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.GetContactById.and.returnValue(of(mockResponse));

    const mockError = { message: 'Network error' };
    countryServiceSpy.getAllCountries.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching countries: ', mockError);
  });
//state load
  it('should load state from country Id', () => {
    // Arrange
    const mockStates: States[] = [
      { countryId: 1, stateName: 'Category 1', stateId: 2},
      { countryId: 2, stateName: 'Category 2', stateId: 1},
    ];
    const mockResponse: ApiResponse<States[]> = { success: true, data: mockStates, message: '' };
    stateServiceSpy.getStatesByCountryId.and.returnValue(of(mockResponse));
  const countryId = 1;
    // Act
    component.loadStates(countryId) // ngOnInit is called here

    // Assert
    expect(stateServiceSpy.getStatesByCountryId).toHaveBeenCalledWith(countryId);
    expect(component.states).toEqual(mockStates);
  });

  it('should not load state when response is false', () => {
    // Arrange

    const mockResponse: ApiResponse<States[]> = { success: false, data: [], message: 'Failed to fetch states' };
    stateServiceSpy.getStatesByCountryId.and.returnValue(of(mockResponse));
    spyOn(console, 'error');

     const countryId = 1;
    // Act
    component.loadStates(countryId) // ngOnInit is called here

    // Assert
    expect(stateServiceSpy.getStatesByCountryId).toHaveBeenCalledWith(countryId);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch states', 'Failed to fetch states');
  });

  it('should handle error during state loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    stateServiceSpy.getStatesByCountryId.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');
    const countryId = 1;

    // Act
    component.loadStates(countryId) // ngOnInit is called here
 
    // Assert
    expect(stateServiceSpy.getStatesByCountryId).toHaveBeenCalledWith(countryId);
    expect(console.error).toHaveBeenCalledWith('Error fetching states:', mockError);
  });
  //load contact(get)
  it('should initialize contactId from route params and load contact details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contacts> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.GetContactById.and.returnValue(of(mockResponse));
    const mockCountryResponse: ApiResponse<Country[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
    const mockStateResponse: ApiResponse<States[]> = { success: true, data: [],message: '' };
    stateServiceSpy.getStatesByCountryId.and.returnValue(of(mockStateResponse));
    
    //Act
    fixture.detectChanges(); // ngOnInit is called here
    
 
    // Assert
    expect(component.contactId).toBe(1);
    expect(contactServiceSpy.GetContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });
 
  it('should log error message if contact loading fails', () => {
    // Arrange
    const mockResponse: ApiResponse<Contacts> = { success: false, data: mockContact, message: 'Failed to fech contacts:' };
    contactServiceSpy.GetContactById.and.returnValue(of(mockResponse));
    const mockCountryResponse: ApiResponse<Country[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
    spyOn(console, 'error');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fech contacts:', 'Failed to fech contacts:');
  });
 
  it('should alert error message on HTTP error 2', () => {
    // Arrange
    spyOn(console, 'error');
    const mockError = { error: { message: 'Error fetching contacts: ' } };
    contactServiceSpy.GetContactById.and.returnValue(throwError(mockError));
    const mockCountryResponse: ApiResponse<Country[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts: ',mockError);
  });
 
  it('should log "Completed" when contact loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<Contacts> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.GetContactById.and.returnValue(of(mockResponse));
    const mockCountryResponse: ApiResponse<Country[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
    spyOn(console, 'log');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.log).toHaveBeenCalledWith('Completed');
  });


  //onsubmit
  it('should navigate to /contact on successful contact updation', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    contactServiceSpy.UpdateContact.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        countryId: 2,
          stateId: 2,
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          image: '',
          imageByte: "",
          gender: "F",
          favourites: true,
          birthdate: "09-08-2008"
      },
      controls: {
       
        contactId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.contact.stateId = 2; // Ensure this.contact.stateId is set to match form.value.stateId
    component.onSubmit(form);
 
    expect(contactServiceSpy.UpdateContact).toHaveBeenCalledWith(component.contact); // Verify addContact was called with component.contact
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/filteredContact']);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on unsuccessful contact addition', () => {
    spyOn(window, 'alert');
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error adding category' };
    contactServiceSpy.UpdateContact.and.returnValue(of(mockResponse));
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        firstName: 'Test name',
         lastName: 'last name',
         countryId: 2,
           stateId: 2,
           email: "Test@gmail.com",
           contactNumber: "1234567891",
           image: '',
           imageByte: "",
           gender: "F",
           favourites: true,
           birthdate: "09-08-2008"
       },
       controls: {
       
        contactId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.contact.stateId = 2;
    component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on HTTP error 1', () => {
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.UpdateContact.and.returnValue(throwError(mockError));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        contactId:1,
        firstName: 'Test name',
         lastName: 'last name',
         countryId: 2,
           stateId: 2,
           email: "Test@gmail.com",
           contactNumber: "1234567891",
           image: '',
           imageByte: "",
           gender: "F",
           favourites: true,
           birthdate: "09-08-2008"
       },
       controls: {
       
        contactId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.contact.stateId = 2;
    component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
    expect(component.loading).toBe(false);
  });
 
  it('should not call contactService.modifyContact on invalid form submission', () => {
    //Arrange
    const form = <NgForm>{ valid: false };
    //Act
    component.onSubmit(form);
    //Assert
    expect(contactServiceSpy.UpdateContact).not.toHaveBeenCalled();
    expect(component.loading).toBe(false);
  });
});
