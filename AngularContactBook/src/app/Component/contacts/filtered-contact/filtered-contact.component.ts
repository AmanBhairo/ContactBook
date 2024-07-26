import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contacts } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-filtered-contact',
  templateUrl: './filtered-contact.component.html',
  styleUrls: ['./filtered-contact.component.css']
})
export class FilteredContactComponent {
  contacts: Contacts[] | undefined;
  contactsForInitialLetter : Contacts[] | undefined;
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
  searchQuery: string = ''; // Added searchQuery property
  totalcontactcount : number =0;
  needSearch: string ='no';
  constructor(private contactService:ContactService,private router:Router) {
    
  }
  
  ngOnInit(): void {
    this.loadAllContact();
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
      this.router.navigate(['/paginatedContact']);
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
      if((this.letter=='' || this.letter==null)&&this.needSearch=="no"){
        this.loadContacts();
      }
      else if( this.searchQuery=='' && this.needSearch=="yes"){
        this.needSearch="no";
        this.loadContacts();

      }
      else if((this.letter!='' || this.searchQuery!='' ) && this.needSearch=="yes"){
        this.filterContacts();
      }
      else{
        this.loadContactByLetter(this.letter);
      }
    }

    loadContacts():void{
      this.loading = true;
      this.contactService.getAllContactsWithPagination(this.pageNumber, this.pageSize,this.sortingOrder)
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
      this.contactService.GetPaginatedContactsByLetter(this.pageNumber, this.pageSize,letter,this.sortingOrder)
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
        this.updateAlphabets();
        this.loadContactsCount();
      }
      else{
        this.updateAlphabets();
        this.loadContactsCountByLetter(this.letter);
      }
    }
      loadContactsCount(): void {
      this.needSearch = 'no';
      this.contactService.getAllContactsCountByLetterSearch('',this.needSearch).subscribe({
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

      this.contactService.getAllContactsCountByLetterSearch(letter,this.needSearch).subscribe({
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
      this.clearSearch();
        this.letter = letter;
        this.pageNumber = 1;
        this.contactDetailCount();
      }

    ShowAllContacts(){
      this.letter='';
      this.contactDetailCount();
      this.loadContacts();
    }

    SortContacts(event:string){
      this.sortingOrder=event;
      this.contactDetail();
    }

    favouriteContacts():void{
      this.router.navigate(['/favouriteContact']);
    }

    setActiveLetter(letter: string) {

      this.activeLetter = letter;
      // Call your filter function or any other logic based on the active letter here
  }

  loadFilteredContacts(letter : string): void {
    this.loading = true;
    let search: string = 'yes';
    this.contactService.getAllContactsCountByLetterSearch(letter,search).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          
          // this.loadContacts();
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


    this.contactService.GetPaginatedContactsByLetterSearch(this.pageNumber,  this.pageSize,letter,this.sortingOrder,search).subscribe({
      next: (response: ApiResponse<Contacts[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contacts = response.data;
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }

  // New method to filter contacts based on search query
  filterContacts() {
    if(this.searchQuery!=''){
      this.activeLetter=''
    }
    // If the search query is empty, load all contacts
    if (!this.searchQuery.trim()) {
      this.contactDetail();
      return;
    }
    this.needSearch ='yes';
     this.loadFilteredContacts(this.searchQuery);
    // Filter contacts based on search query
    this.contacts = this.contacts?.filter(contact => {
      const fullName = `${contact.firstName} ${contact.lastName}`.toLowerCase();
      return fullName.includes(this.searchQuery.toLowerCase());
    });
  }
   // New method to clear search query and load all contacts
   clearSearch() {
    this.searchQuery = '';
    this.contactDetailCount();
    this.contactDetail();
  }

  updateAlphabets() {
    let alphabetsSet = new Set<string>();
    this.loadAllContact();
    if (this.contactsForInitialLetter) {
      this.contactsForInitialLetter.forEach(contactsForInitialLetter => {
        let firstLetter = contactsForInitialLetter.firstName.charAt(0).toUpperCase();
        if (/^[A-Z]$/.test(firstLetter)) {
          alphabetsSet.add(firstLetter);
        }
      });
    }
    this.Alphabets = Array.from(alphabetsSet).sort();
    // console.log('Albhabets button: ',this.Alphabets);
  }

  loadAllContact():void{
    this.loading = true;
    this.contactService.getAllContacts()
    .subscribe(response=> {
      if(response.success){
        this.contactsForInitialLetter = response.data;
        this.updateAlphabets();
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
}
