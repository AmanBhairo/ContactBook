import { TestBed } from '@angular/core/testing';

import { CountryService } from './country.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contacts } from '../models/contact.model';
import { Country } from '../models/country.model';

describe('CountryService', () => {
  let service: CountryService;
  let httpMock : HttpTestingController;
  const mockApiResponse : ApiResponse<Country[]> ={
    success : true,
    data:[
      {
        countryId: 1,
        countryName: 'India',
      },
      {
        countryId: 2,
        countryName: 'USA',
      }
    ],
    message:''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[CountryService],
    });
    service = TestBed.inject(CountryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all countries successfully',()=>{
    //Arrange
    const apiUrl = 'http://localhost:5096/api/Country/GetAllCountry';

    //Act
    service.getAllCountries().subscribe((response)=>{
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
    const apiUrl = 'http://localhost:5096/api/Country/GetAllCountry';
    const emptyResponse : ApiResponse<Contacts[]> ={
      success : true,
      data:[],
      message:''
    }
    //Act
    service.getAllCountries().subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });
  

  it('should handle Http error gracefully while fetching all countries',()=>{
    //Arrange
    const apiUrl = 'http://localhost:5096/api/Country/GetAllCountry';
    const errorMessage = 'Failed to load countries';

    //Act
    service.getAllCountries().subscribe(
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
});
