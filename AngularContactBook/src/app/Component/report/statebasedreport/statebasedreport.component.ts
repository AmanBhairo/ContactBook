import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactReport } from 'src/app/models/contact-record-report.model';
import { Country } from 'src/app/models/country.model';
import { States } from 'src/app/models/state.model';
import { CountryService } from 'src/app/services/country.service';
import { ReportService } from 'src/app/services/report.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-statebasedreport',
  templateUrl: './statebasedreport.component.html',
  styleUrls: ['./statebasedreport.component.css']
})
export class StatebasedreportComponent {
  countries : Country[]=[];
  states : States[]=[];
  countryId : number =0;
  stateId : number = 0;

  loading: boolean =false;
  stateBasedReport :ContactReport[]=[];

  constructor(
    private reportService:ReportService,
    private countryService:CountryService,
    private stateServie:StateService, 
    private router:Router,
    private http: HttpClient) {}

  ngOnInit(): void {
    this.loadCountries();
    this.loadStates(this.countryId);
  }

  loadCountries():void{
    this.loading = true;
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<Country[]>)=>{
        if(response.success){
          this.countries = response.data;
        }else{
          console.error('Failed to fetch countries',response.message);
        }
        this.loading = false;
      },
      error:(error)=>{
        console.error('Error fetching countries: ',error);
        this.loading = false;
      }
    });
  }

  loadStates(countryId:number):void{
    this.loading = true;
    this.stateServie.getStatesByCountryId(countryId).subscribe({
      next:(response: ApiResponse<States[]>)=>{
        if(response.success){
          this.states = response.data;
        }else{
          console.error('Failed to fetch states',response.message);
        }
        this.loading = false;
      },
      error:(error)=>{
        console.error('Error fetching states: ',error);
        this.loading = false;
      }
    });
  }

  onCountryChange() {
    if (this.countryId) {
      this.stateId = 0;
      this.states = [];
      this.loadStates(this.countryId);
    } else {
      
    }
  }

  onMonthChange() {
    if (this.stateId!=0) {
      this.loadReportDetail(this.stateId);
    } 
  }

  loadReportDetail(state:number):void{
    this.reportService.stateBasedReport(state).subscribe({
      next:(response)=>{
        if(response.success){
          this.stateBasedReport = response.data;
        }else{
          this.stateBasedReport =[]
          console.error('Failed to fech contact report: ',response.message);
        }
      },
      error:(error)=>{
        this.stateBasedReport =[];
        console.error('Error fetching contact report: ',error);
      },
      complete:()=>{
        console.log("Completed");
      }
    })
  }
}
