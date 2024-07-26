import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ContactService } from './contact.service';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contacts } from '../models/contact.model';
import { AddContact } from '../models/addcontact.model';
import { UpdateContact } from '../models/updateContact.model';

describe('ContactService', () => {
  let service: ContactService;
  let httpMock : HttpTestingController;
  const mockApiResponse : ApiResponse<Contacts[]> ={
    success : true,
    data:[
      {
        contactId :1,
        countryId:1,
        stateId:1,
        firstName :'firstName',
        lastName : 'lastName',
        contactNumber:'1122334455',
        email:'email@gmail.com',
        contactDescription:'description',
        profilePic:'profilePic.png',
        gender:'M',
        address:'xyz',
        favourite:true,
        imageByte:"imageByte",
        state:{
          stateId:1,
          stateName:'State1',
          countryId:1
        },
        country:{
          countryId:1,
          countryName:'country1',
        }
      },
      {
        contactId :2,
        countryId:2,
        stateId:2,
        firstName :'firstName',
        lastName : 'lastName',
        contactNumber:'1122334455',
        email:'email@gmail.com',
        contactDescription:'description',
        profilePic:'profilePic.png',
        gender:'M',
        address:'xyz',
        favourite:true,
        imageByte:'imageByte',
        state:{
          stateId:2,
          stateName:'State2',
          countryId:2
        },
        country:{
          countryId:2,
          countryName:'country2',
        }
      }
    ],
    message:''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[ContactService],
    });
    service = TestBed.inject(ContactService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all contacts successfully',()=>{
    //Arrange
    const apiUrl = 'http://localhost:5096/api/Contact/GetAllContacts';

    //Act
    service.getAllContacts().subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle empty contact list',()=>{
    //Arrange
    const apiUrl = 'http://localhost:5096/api/Contact/GetAllContacts';
    const emptyResponse : ApiResponse<Contacts[]> ={
      success : true,
      data:[],
      message:''
    }
    //Act
    service.getAllContacts().subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });
  

  it('should handle Http error gracefully',()=>{
    //Arrange
    const apiUrl = 'http://localhost:5096/api/Contact/GetAllContacts';
    const errorMessage = 'Failed to load contacts';

    //Act
    service.getAllContacts().subscribe(
      ()=> fail('expected an error,not contacts'),
      (error) =>{
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal server error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');

    //Respond with error
    req.flush(errorMessage,{status:500,statusText:'Internal server error'});
  });

  it('should add a contact successfully', () => {
    //Arrange
    const addContact: AddContact = {
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
    }

    const mockSuccessResponse: ApiResponse<string> = {
      success: true,
      data: "",
      message: "Contact saved successfully."
    };

    //Act
    service.addContact(addContact).subscribe(response => {
      //Assert
      expect(response).toBe(mockSuccessResponse);
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/CreateContact');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact addition', () => {
    //Arrange
    const addContact: AddContact = {
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
    }

    const mockErrorResponse: ApiResponse<string> = {
      success: false,
      data: "",
      message: "Contact already exists."
    }

    //Act
    service.addContact(addContact).subscribe(response => {
      //Assert
      expect(response).toBe(mockErrorResponse);
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/CreateContact');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);
  });

  it('should handle http error', () => {
    //Arrange
    const addContact: AddContact = {
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
    }

    const mockHttpError = {
      status: 500,
      statusText: "Internal Server Error"
    }

    //Act
    service.addContact(addContact).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual("Internal Server Error");

      }
    })

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/CreateContact');
    expect(req.request.method).toBe('POST');
    req.flush({}, mockHttpError);
  });

  it('should update a category successfullt',()=>{
    //Arrange
    const updateContact : UpdateContact={
      contactId:1,
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
    };

    const mockSuccessResponse :ApiResponse<string>={
      success:true,
      data:'',
      message:'Category Updated successfully',
    };

    //Act
    service.UpdateContact(updateContact).subscribe(
      response =>{
        //Asert
        expect(response).toEqual(mockSuccessResponse);
      });

      const req = httpMock.expectOne('http://localhost:5096/api/Contact/EditContact');
      expect(req.request.method).toBe('PUT');
      req.flush(mockSuccessResponse);
  });

  it('should handle failed contact Update', () => {
    //Arrange
    const updateContact: UpdateContact = {
      contactId:1,
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
    }

    const mockErrorResponse: ApiResponse<string> = {
      success: false,
      data: "",
      message: "Contact already exists."
    }

    //Act
    service.UpdateContact(updateContact).subscribe(response => {
      //Assert
      expect(response).toBe(mockErrorResponse);
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/EditContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);
  });

  it('should handle http error', () => {
    //Arrange
    const updateContact: UpdateContact = {
      contactId:1,
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
    }

    const mockHttpError = {
      status: 500,
      statusText: "Internal Server Error"
    }

    //Act
    service.UpdateContact(updateContact).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual("Internal Server Error");

      }
    })

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/EditContact');
    expect(req.request.method).toBe('PUT');
    req.flush({}, mockHttpError);
  });

  it('should fetch a contact by id successfully',()=>{
    //Arrange
    const contactId=1;
    const mockSuspenseResponse: ApiResponse<Contacts>={
      success:true,
      data:{
        contactId:1,
      countryId:1,
      stateId:1,
      firstName:'FirstName',
      lastName:'LastName',
      contactNumber:'1234567890',
      email:'xyz@gmail.com',
      contactDescription:'Description',
      profilePic:'Image.png',
      gender:'M',
      address:'city',
      favourite:true,
      imageByte:'ImageByte',
      state: {
        stateId: 1,
        stateName: 'Haryana',
        countryId: 1
      },
      country: {
        countryId: 1,
        countryName: 'India'
      }
      },
      message:''
    };

    //Act
    service.GetContactById(contactId).subscribe(response =>{
      //Assert
      expect(response.success).toBe(true);
      expect(response.data).toEqual(mockSuspenseResponse.data);
      expect(response.data.contactId).toEqual(contactId);
    });
    const req = httpMock.expectOne('http://localhost:5096/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuspenseResponse);
  });

  it('should handle failed contact retrival',()=>{
    //Arrange
    const contactId=1;
    const mockHttpError:ApiResponse<Contacts>={
      success:false,
      data:{}as Contacts,
      message:'N0 record found',
    };

    //Act
    service.GetContactById(contactId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockHttpError);
      expect(response.message).toEqual('N0 record found');
      expect(response.success).toBeFalse();
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockHttpError);
  });

  it('should handle http error while fetching contact by id',()=>{
    //Arrange
    const contactId = 1;
    const mockHttpError ={
      status:500,
      statusText:'Internal Server Error'
    };

    //Act
    service.GetContactById(contactId).subscribe({
      next:()=>fail('should have failed with 500 error'),
      error:(error)=>{
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
  });

  it('should delete a contact by id',()=>{
    //Arrange
    const contactId=1;
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      data:"",
      message:"Contact deleted successfully."
    }

    //act
    service.deleteContactById(contactId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockSuccessResponse);
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/DeleteContact/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed to delete contact',()=>{
    //Arrange
    const contactId=1;
    const mockErrorResponse:ApiResponse<string>={
      success:false,
      data:"",
      message:"Something went wrong, please try after sometimes."
    }

    //act
    service.deleteContactById(contactId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toEqual('Something went wrong, please try after sometimes.');
      expect(response.success).toBeFalse();
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/DeleteContact/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockErrorResponse);

  });

  it('should handle http error while deleting a contact',()=>{
    //Arrange
    const contactId=1;
    const mockHttpError ={
      status:500,
      statusText:'Internal Server Error'
    };

    //act
    service.deleteContactById(contactId).subscribe({
      next:()=>fail('should have failed with 500 error'),
      error:(error)=>{
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    });

    const req = httpMock.expectOne('http://localhost:5096/api/Contact/DeleteContact/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush({},mockHttpError);

  });
});
