import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-genderbasedreport',
  templateUrl: './genderbasedreport.component.html',
  styleUrls: ['./genderbasedreport.component.css']
})
export class GenderbasedreportComponent implements OnInit {
  constructor(
    private reportService:ReportService,
    private router:Router,
    private http: HttpClient
  ) {}

  maleBasedReport :number|null =null;
  femaleBasedReport :number|null =null;

  ngOnInit(): void {
    this.loadReportForMaleDetail();
    this.loadReportForFemaleDetail();
  }

  loadReportForMaleDetail():void{
    this.reportService.genderBasedReport('Male').subscribe({
      next:(response)=>{
        if(response.success){
          this.maleBasedReport = response.data;
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
  loadReportForFemaleDetail():void{
    this.reportService.genderBasedReport('Female').subscribe({
      next:(response)=>{
        if(response.success){
          this.femaleBasedReport = response.data;
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
