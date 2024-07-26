import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contacts } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-favourite-contact',
  templateUrl: './favourite-contact.component.html',
  styleUrls: ['./favourite-contact.component.css']
})
export class FavouriteContactComponent {
  contacts: Contacts[] | undefined;
  contactId:number|undefined;
  loading : boolean = false;
  pageNumber: number = 1;
  pageSize: number = 2;
  totalItems: number = 0;
  totalPages: number = 0;
  Alphabets:string[]= 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');
  letter:string='';
  sortingOrder:string='asc';
  imageSrc:string= 'assets/DefaultImage.jpg';
  activeLetter: string = '';
  constructor(private contactService:ContactService,private router:Router) {
    
  }
  
  ngOnInit(): void {
    this.contactDetailCount();
    // this.loadContacts();
  }

  

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
            this.totalItems--;
            this.totalPages = Math.ceil(this.totalItems / this.pageSize);
            if(this.pageNumber>this.totalPages){
              this.pageNumber=this.totalPages
            }
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
      this.router.navigate(['/contact']);
    }

  

    changePage(pageNumber: number): void {
      this.pageNumber = pageNumber;
      // this.loadContacts();
      this.contactDetail();
    }

    changePageSize(pageSize: number): void {
   
      this.pageSize = pageSize;
      this.pageNumber = 1; // Reset to first page
      this.totalPages = Math.ceil(this.totalItems / this.pageSize); // Recalculate total pages
      // this.loadContacts();
      this.contactDetail();
    }
    contactDetail(){
      if(this.letter=='' || this.letter==null){
        this.loadContacts();
      }
      else{
        this.loadContactByLetter(this.letter);
      }
    }

    loadContacts():void{
      this.loading = true;
      this.contactService.GetPaginatedContactsForFavourite(this.pageNumber, this.pageSize,this.sortingOrder)
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

    loadContactByLetter(letter:string):void{
      this.loading = true;
      //this.loadContactsCountByLetter(letter);
      this.contactService.GetPaginatedContactsByLetterForFavourite(this.pageNumber, this.pageSize,letter,this.sortingOrder)
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
    contactDetailCount(){
      if(this.letter=='' || this.letter==null){
        this.loadContactsCount();
      }
      else{
        this.loadContactsCountByLetter(this.letter);
      }
    }
    loadContactsCount(): void {
      this.contactService.GetTotalContactsForFavourite('').subscribe({
        next: (response: ApiResponse<number>) => {
          if (response.success) {
            console.log(response.data);
            this.totalItems = response.data;
            this.totalPages = Math.ceil(this.totalItems / this.pageSize);
            this.loadContacts();
          } else {
            console.error('Failed to fetch contacts count', response.message);
          }
          this.loading = false;
        },
        error: (error) => {
          console.error('Error fetching contacts count.', error);
          this.loading = false;
        }
      });
    }

    loadContactsCountByLetter(letter:string): void {

      this.contactService.GetTotalContactsForFavourite(letter).subscribe({
        next: (response: ApiResponse<number>) => {
          if (response.success) {
            console.log(response.data);
            this.totalItems = response.data;
            console.log(this.totalItems);
            this.totalPages = Math.ceil(this.totalItems / this.pageSize);
            this.loadContactByLetter(letter);
          } else {
            console.error('Failed to fetch contacts count', response.message);
          }
          this.loading = false;
        },
        error: (error) => {
          // this.totalItems = 0;
          console.error('Error fetching contacts count.', error);
          this.loading = false;
        }
      });
    }

    filterByLetter(letter: string): void {
        this.letter = letter;
        this.pageNumber = 1;
        this.contactDetailCount();
      }

    ShowAllContacts(){
      this.letter='';
      this.contactDetailCount();
      this.loadContacts();
    }

    SortContacts(event:any){
      this.sortingOrder=event.target.value;
      this.contactDetail();
    }

    backToContacts():void{
      this.router.navigate(['/searchContact']);
    }

    setActiveLetter(letter: string) {
      this.activeLetter = letter;
      // Call your filter function or any other logic based on the active letter here
  }
}
