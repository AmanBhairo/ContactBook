import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContactCountryState } from 'src/app/models/contact-country-state.model';
import { Contacts } from 'src/app/models/contact.model';
import { UpdateContact } from 'src/app/models/updateContact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-display-contact',
  templateUrl: './display-contact.component.html',
  styleUrls: ['./display-contact.component.css']
})
export class DisplayContactComponent {
  contactId:number | undefined
  contact:ContactCountryState={
    contactId: 0,
    countryId: 0,
    stateId: 0,
    firstName: '',
    lastName: '',
    contactNumber: 0,
    email: '',
    contactDescription: '',
    profilePic: '',
    gender: '',
    address: '',
    favourite: true,
    imageByte:'',
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0
    },
    country: {
      countryId: 0,
      countryName: ''
    }
  }
  imageSrc:string= '';
  constructor(private contactService:ContactService, private route:ActivatedRoute) {}
  
  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.contactId = params['id'];
      this.loadCategoryDetail(this.contactId);
    });
  }

  loadCategoryDetail(contactId:number | undefined):void{
    this.contactService.GetContactWithStateCountryById(contactId).subscribe({
      next:(response)=>{
        if(response.success){
          this.contact = response.data;
          if (this.contact.profilePic) {
            this.imageSrc = 'assets/' + this.contact.profilePic; 
          }
          else{
            this.imageSrc = 'assets/DefaultImage.jpg'; 
          }
        }else{
          console.error('Failed to fech contact: ',response.message);
        }
      },
      error:(error)=>{
        console.error('Error fetching contact: ',error);
      },
      complete:()=>{
        console.log("Completed");
      }
    })
  }

}
