import { TestBed } from '@angular/core/testing';

import { StateService } from './state.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { States } from '../models/state.model';

describe('StateService', () => {
  let service: StateService;
  let httpMock : HttpTestingController;
  const mockApiResponse : ApiResponse<States[]> ={
    success : true,
    data:[
      {
        stateId: 1,
        stateName: 'Haryana',
        countryId: 1,
      },
      {
        stateId: 2,
        stateName: 'Gujarat',
        countryId: 1,
      }
    ],
    message:''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[StateService],
    });
    service = TestBed.inject(StateService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all states by country id successfully',()=>{
    //Arrange
    const countryId =1;
    const apiUrl = 'http://localhost:5096/api/State/GetStateByCountryId/'+countryId;

    //Act
    service.getStatesByCountryId(countryId).subscribe((response)=>{
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle empty state list while fetching states by country id',()=>{
    //Arrange
    const countryId =1;
    const apiUrl = 'http://localhost:5096/api/State/GetStateByCountryId/'+countryId;
    const emptyResponse : ApiResponse<States[]> ={
      success : true,
      data:[],
      message:''
    }
    //Act
    service.getStatesByCountryId(countryId).subscribe((response)=>{
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
    const countryId =1;
    const apiUrl = 'http://localhost:5096/api/State/GetStateByCountryId/'+countryId;
    const errorMessage = 'Failed to load countries';

    //Act
    service.getStatesByCountryId(countryId).subscribe(
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
