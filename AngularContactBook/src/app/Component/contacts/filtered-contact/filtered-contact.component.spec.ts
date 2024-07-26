import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilteredContactComponent } from './filtered-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contact.service';
import { Contacts } from 'src/app/models/contact.model';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('FilteredContactComponent', () => {
  let component: FilteredContactComponent;
  let fixture: ComponentFixture<FilteredContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;
  let router: Router;  

  const mockContacts: Contacts[] = [
    { 
      contactId: 1, 
      firstName: 'Test', 
      lastName:'Test', 
      email: 'Test@gmail.com',
      contactNumber:'1234567890',
      contactDescription:'Description1',
      gender:'M',
      address:'pune',
      favourite:true,
      countryId:1,
      country:{
        countryId:1,
        countryName:'',
      },
      stateId:1,
      state:{
        stateId:1,
        stateName:'State1',
        countryId:1,
      },
      profilePic: '',
      imageByte:''},
    { 
      contactId: 2, 
      firstName: 'Test2', 
      lastName:'Test2', 
      email: 'Test2@gmail.com',
      contactNumber:'1244567890',
      contactDescription:'Description2',
      gender:'F',
      address:'pune',
      favourite:true,
      countryId:1,
      country:{
        countryId:1,
        countryName:''
      },
      stateId:1,
      state:{
        stateId:1,
        countryId:1,
        stateName:'State!'
      },
      profilePic: '',
      imageByte:'' },
    
  ];

  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['getAllContactsCountByLetterSearch','getAllContactsWithPagination','GetPaginatedContactsByLetter','GetPaginatedContactsByLetterSearch','getAllContacts','deleteContactById']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    router = jasmine.createSpyObj('Router', ['navigate']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,FormsModule,RouterTestingModule],
      declarations: [FilteredContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy }
      ],
    });
    fixture = TestBed.createComponent(FilteredContactComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch total contact count without letter without search successfully',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponseall));

    //Act
    component.loadContactsCount();

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();

  })

  it('should log error message when fails to fetch total contact count without letter without search',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockResponse));
    spyOn(console, 'error');

    //Act
    component.loadContactsCount();

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count', 'Failed to fetch contacts count');

  })

  it('should alert error message on HTTP error to fetch total contact count without letter without search',()=>{
    //Arrange
    const mockError = { error: { message: 'Error fetching contacts count.' } };
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Error fetching contacts count.'};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    //Act
    component.loadContactsCount();

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.', mockError);

  })

  it('should calculate total contact count with letter and search is no',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.GetPaginatedContactsByLetter.and.returnValue(of(mockResponseall));

    //Act
    component.letter='t';
    component.needSearch = 'no';
    component.loadContactsCountByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalledWith('t','no');
    expect(contactServiceSpy.GetPaginatedContactsByLetter).toHaveBeenCalled();
    expect(component.totalItems).toEqual(2);
  })

  it('should log error message when fails to fetch total contact count with letter and search is no',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockResponse));
    spyOn(console, 'error');

    //Act
    component.letter='t';
    component.needSearch = 'no';
    component.loadContactsCountByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count', 'Failed to fetch contacts count');
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalledWith('t','no');
    
  })

  it('should alert error message on HTTP error to fetch total contact count with letter and search is no',()=>{
    //Arrange
    const mockError = { error: { message: 'Error fetching contacts count.' } };
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Error fetching contacts count.'};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    //Act
    component.letter='t';
    component.needSearch = 'no';
    component.loadContactsCountByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.', mockError);
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalledWith('t','no');
    
  })

  it('should calculate total contact count with letter and search is yes',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.GetPaginatedContactsByLetter.and.returnValue(of(mockResponseall));

    //Act
    component.letter='t';
    component.needSearch = 'yes';
    component.loadContactsCountByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalledWith('t','yes');
    expect(contactServiceSpy.GetPaginatedContactsByLetter).toHaveBeenCalled();
    expect(component.totalItems).toEqual(2);
  })

  it('should log error message when fails to fetch total contact count with letter and search is yes',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockResponse));
    spyOn(console, 'error');


    //Act
    component.letter='t';
    component.needSearch = 'yes';
    component.loadContactsCountByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalledWith('t','yes');
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count', 'Failed to fetch contacts count');
  })

  it('should alert error message on HTTP error to fetch total contact count with letter and search is yes',()=>{
    //Arrange
    const mockError = { error: { message: 'Error fetching contacts count.' } };
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(throwError(mockError));
    spyOn(console, 'error');


    //Act
    component.letter='t';
    component.needSearch = 'yes';
    component.loadContactsCountByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalled();
    expect(contactServiceSpy.getAllContactsCountByLetterSearch).toHaveBeenCalledWith('t','yes');
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.', mockError);
  })

  it('should load contacts without letter without search successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));

    //Act
    component.loadContacts();

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.sortingOrder);
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should log error message when fails to load contacts without letter without search',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contacts[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));
    spyOn(console, 'error');

    //Act
    component.loadContacts();

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.sortingOrder);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts', 'Failed to fetch contacts');
    expect(component.loading).toBe(false);
  })

  it('should alert error message on HTTP error to load contacts without letter without search',()=>{
    //Arrange
   
    const mockError = { error: { message: 'Error fetching contacts' } };
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    //Act
    component.loadContacts();

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.sortingOrder);
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts', mockError);
    expect(component.loading).toBe(false);
  })

  it('should load contacts with letter and without search successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.GetPaginatedContactsByLetter.and.returnValue(of(mockResponse));

    //Act
    component.letter ='t';
    component.loadContactByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.GetPaginatedContactsByLetter).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.letter,component.sortingOrder);
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should log error message when fails to load contacts with letter and without search',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contacts[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.GetPaginatedContactsByLetter.and.returnValue(of(mockResponse));
    spyOn(console,'error');

    //Act
    component.letter ='t';
    component.loadContactByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.GetPaginatedContactsByLetter).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.letter,component.sortingOrder);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts', 'Failed to fetch contacts');
    expect(component.loading).toBe(false);
  })

  it('should alert error message on HTTP error to load contacts with letter and without search',()=>{
    //Arrange
   
    const mockError = { error: { message: 'Error fetching contacts' } };
    contactServiceSpy.GetPaginatedContactsByLetter.and.returnValue(throwError(mockError));
    spyOn(console,'error');

    //Act
    component.letter ='t';
    component.loadContactByLetter(component.letter);

    //Assert
    expect(contactServiceSpy.GetPaginatedContactsByLetter).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.letter,component.sortingOrder);
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts', mockError);
    expect(component.loading).toBe(false);
  })
  
  it('should load contacts with letter and with search successfully',()=>{
    //Arrange
    const mockContactCountResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockContactCountResponse));
    const mockResponse :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.GetPaginatedContactsByLetterSearch.and.returnValue(of(mockResponse));

    //Act
    component.searchQuery ='Te';
    component.loadFilteredContacts(component.searchQuery);

    //Assert
    expect(contactServiceSpy.GetPaginatedContactsByLetterSearch).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.searchQuery,component.sortingOrder,'yes');
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should log error message when fails to load contacts with letter and with search',()=>{
    //Arrange
    const mockContactCountResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockContactCountResponse));
    const mockResponse :ApiResponse<Contacts[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.GetPaginatedContactsByLetterSearch.and.returnValue(of(mockResponse));
    spyOn(console,'error');

    //Act
    component.searchQuery ='Te';
    component.loadFilteredContacts(component.searchQuery);

    //Assert
    expect(contactServiceSpy.GetPaginatedContactsByLetterSearch).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.searchQuery,component.sortingOrder,'yes');
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts', 'Failed to fetch contacts');
    expect(component.loading).toBe(false);
  })

  it('should alert error message on HTTP error when fails to load contacts with letter and with search',()=>{
    //Arrange
    const mockContactCountResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCountByLetterSearch.and.returnValue(of(mockContactCountResponse));
    const mockError = { error: { message: 'Error fetching contacts.' } };
    contactServiceSpy.GetPaginatedContactsByLetterSearch.and.returnValue(throwError(mockError));
    spyOn(console,'error');

    //Act
    component.searchQuery ='Te';
    component.loadFilteredContacts(component.searchQuery);

    //Assert
    expect(contactServiceSpy.GetPaginatedContactsByLetterSearch).toHaveBeenCalledWith(component.pageNumber,component.pageSize,component.searchQuery,component.sortingOrder,'yes');
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts.', mockError);
    expect(component.loading).toBe(false);
  })

  it('should call confirmDelete and set contactId for deletion', () => {
    // Arrange
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(component, 'deleteContact');
 
    // Act
    component.confirmDelet(1);
 
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure?');
    expect(component.contactId).toBe(1);
    expect(component.deleteContact).toHaveBeenCalled();
  });
 
  it('should not call deleteContact if confirm is cancelled', () => {
    // Arrange
    spyOn(window, 'confirm').and.returnValue(false);
    spyOn(component, 'deleteContact');
 
    // Act
    component.confirmDelet(1);
 
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure?');
    expect(component.deleteContact).not.toHaveBeenCalled();
  });
 
  it('should delete contact and reload contacts', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<string> = { success: true, data: "", message: 'Contact deleted successfully' };
    contactServiceSpy.deleteContactById.and.returnValue(of(mockDeleteResponse));
    spyOn(component, 'loadContacts');
    spyOn(router,'navigate');

 
    // Act
    component.contactId = 1;
    component.deleteContact(component.contactId);
 
    // Assert
    expect(contactServiceSpy.deleteContactById).toHaveBeenCalledWith(1);
    expect(component.loadContacts).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/paginatedContact']);
  });
 
  it('should alert error message if delete contact fails', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<string> = { success: false, data: "", message: 'Failed to delete contact' };
    contactServiceSpy.deleteContactById.and.returnValue(of(mockDeleteResponse));
    spyOn(window, 'alert');
    spyOn(router,'navigate');

 
    // Act
    component.contactId = 1;
    component.deleteContact(component.contactId);
 
    // Assert
    expect(window.alert).toHaveBeenCalledWith('Failed to delete contact');
    expect(router.navigate).toHaveBeenCalledWith(['/paginatedContact']);
  });

  it('should alert error message if delete contact throws error', () => {
    // Arrange
    const mockError = { error: { message: 'Delete error' } };
    contactServiceSpy.deleteContactById.and.returnValue(throwError(() => mockError));
    spyOn(window, 'alert');
    spyOn(router,'navigate');
 
    // Act
    component.contactId = 1;
    component.deleteContact(component.contactId);
 
    // Assert
    expect(window.alert).toHaveBeenCalledWith('Delete error');
    expect(router.navigate).toHaveBeenCalledWith(['/paginatedContact']);
  });
  
  it('should navigate to updateContact when clicked on edit',()=>{
    //Arrange
    spyOn(router,'navigate');

    //Act
    let id:number=1;
    component.updateCategory(id);

    //Assert
    expect(router.navigate).toHaveBeenCalledWith(['/updateContact',1]);
  });

  it('should navigate to displayContact when clicked on display',()=>{
    //Arrange
    spyOn(router,'navigate');

    //Act
    let id:number=1;
    component.displayContact(id);

    //Assert
    expect(router.navigate).toHaveBeenCalledWith(['/displayContact',1]);
  });

  it('should display all the First letter of Contacts as navigation display',()=>{
    //Arrange
    const mockResponse :ApiResponse<Contacts[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContacts.and.returnValue(of(mockResponse));
    spyOn(component,'contactDetailCount');

    //Act
    fixture.detectChanges();

    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
    expect(component.contacts)
    expect(component.Alphabets).toEqual(['T']);
  })

  it('should display all contacts when Onint called for the first time',()=>{
    //Arrange
    
    //Act

    //Assert
  })
  
});
