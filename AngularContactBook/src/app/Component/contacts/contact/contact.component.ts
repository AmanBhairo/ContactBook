import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Contacts } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit{
  contacts: Contacts[] | undefined;
  contactId:number|undefined;
  loading : boolean = false;
  imageSrc:string= 'assets/DefaultImage.jpg';
  constructor(private contactService:ContactService,private router:Router) {
    
  }
  
  ngOnInit(): void {
    this.loadContacts();
  }

  loadContacts():void{
    this.loading = true;
    this.contactService.getAllContacts()
    .subscribe(response=> {
      if(response.success){
        this.contacts = response.data;
      }
      else{
        console.error('Failed to fetch contacts', response.message);
      }
      this.loading = false;
    },error =>{
      console.error('Error fetching contacts',error);
      this.loading = false;
    }
    )}

    updateCategory(id:number):void{
      this.router.navigate(['/updateContact',id]);
    }

    displayContact(id:number):void{
      this.router.navigate(['/displayContact',id]);
    }

    confirmDelet(id:number):void{
      if(confirm('Are you sure?')){
        this.contactId = id;
        this.deleteContact(this.contactId);
      }
    }
  
    deleteContact(id:number):void{
      this.contactService.deleteContactById(id).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/paginatedContact']);
            this.loadContacts();
          }else{
            alert(response.message);
          }
        },
        error:(err)=>{
          alert(err.error.message);
        },
        complete:()=>{
          console.log('completed');
        }
      })
      this.router.navigate(['/paginatedContact']);
    }
}
