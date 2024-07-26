import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ContactReport } from 'src/app/models/contact-record-report.model';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-birthdaybasedreport',
  templateUrl: './birthdaybasedreport.component.html',
  styleUrls: ['./birthdaybasedreport.component.css']
})
export class BirthdaybasedreportComponent {
  monthList: { value: number, name: string }[] = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];
  birthdayMonth:number=0;

  birthdayMonthBasedReport :ContactReport[]=[];

  loading: boolean =false;

  constructor(
    private reportService:ReportService,
    private router:Router,
    private http: HttpClient
  ) {}

  loadReportDetail(month:number):void{
    this.reportService.birthdayMonthBasedReport(month).subscribe({
      next:(response)=>{
        if(response.success){
          this.birthdayMonthBasedReport = response.data;
        }else{
          this.birthdayMonthBasedReport =[]
          console.error('Failed to fech contact report: ',response.message);
        }
      },
      error:(error)=>{
        this.birthdayMonthBasedReport =[];
        console.error('Error fetching contact report: ',error);
      },
      complete:()=>{
        console.log("Completed");
      }
    })
  }

  onMonthChange() {
    if (this.birthdayMonth!=0) {
      this.loadReportDetail(this.birthdayMonth);
    } 
  }

}
