import { Component } from '@angular/core';

@Component({
  selector: 'app-reportdecider',
  templateUrl: './reportdecider.component.html',
  styleUrls: ['./reportdecider.component.css']
})
export class ReportdeciderComponent {
  selectedComponent:number =1;
  showComponent(num:number){
    this.selectedComponent = num;
  }
}
