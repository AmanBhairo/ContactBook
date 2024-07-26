import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { CountryService } from 'src/app/services/country.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-countrybasedreport',
  templateUrl: './countrybasedreport.component.html',
  styleUrls: ['./countrybasedreport.component.css']
})
export class CountrybasedreportComponent {
  countries : Country[]=[];
  countryId : number =0;

  loading: boolean =false;
  stateBasedReport :number|null =null;

  constructor(
    private reportService:ReportService,
    private countryService:CountryService,
    private router:Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.loadCountries();
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

  onCountryChange() {
    if (this.countryId) {
      this.loadReportDetail(this.countryId);
    }
  }

  loadReportDetail(country:number):void{
    this.reportService.countryBasedReport(country).subscribe({
      next:(response)=>{
        if(response.success){
          this.stateBasedReport = response.data;
        }else{
          console.error('Failed to fech contact report: ',response.message);
        }
      },
      error:(error)=>{
        console.error('Error fetching contact report: ',error);
      },
      complete:()=>{
        console.log("Completed");
      }
    })
  }
}
