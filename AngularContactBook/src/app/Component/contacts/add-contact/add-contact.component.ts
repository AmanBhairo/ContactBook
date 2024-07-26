import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddContact } from 'src/app/models/addcontact.model';
import { Country } from 'src/app/models/country.model';
import { States } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit{

  countries : Country[]=[];
  states : States[]=[];

  imageUrl: string | ArrayBuffer | null = null;
  fileToUpload: File | null = null;
  fileName='';

  contact:AddContact={
    countryId: 0,
    stateId: 0,
    firstName: '',
    lastName: '',
    contactNumber: '',
    email: '',
    contactDescription: '',
    profilePic: '',
    gender: '',
    address: '',
    favourite: false,
    imageByte:'',
  }

  loading: boolean =false;

  constructor(
    private contactServicec:ContactService,
    private countryService:CountryService,
    private stateServie:StateService, 
    private router:Router,
    private http: HttpClient) {}

  ngOnInit(): void {
    this.loadCountries();
    this.loadStates(this.contact.countryId);
  }

  onFileSelected(event: any) {
    this.fileToUpload = event.target.files[0];
    if (this.fileToUpload) {
      const reader = new FileReader();
      reader.onload=()=>{
        this.contact.imageByte = (reader.result as string).split(',')[1]; 
      }
      reader.readAsDataURL(this.fileToUpload);
      this.fileName = this.fileToUpload.name;
      this.readImage(this.fileToUpload);
    }
  }

  readImage(file: File) {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imageUrl = e.target.result;
    };
    reader.readAsDataURL(file);
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
    if (this.contact.countryId) {
      this.contact.stateId = null;
      this.states = [];
      this.loadStates(this.contact.countryId);
    } else {
      
    }
  }

  onSubmit(addContactTFForm:NgForm):void{
    if(addContactTFForm.valid){
      this.loading = true;
      console.log(addContactTFForm.value);
      let addContact :AddContact ={
        countryId: addContactTFForm.controls['countryId'].value,
        stateId: addContactTFForm.controls['stateId'].value,
        firstName: addContactTFForm.controls['firstName'].value,
        lastName: addContactTFForm.controls['lastName'].value,
        contactNumber: addContactTFForm.controls['contactNumber'].value,
        email: addContactTFForm.controls['email'].value,
        contactDescription: addContactTFForm.controls['contactDescription'].value,
        profilePic: this.fileName,
        gender: addContactTFForm.controls['gender'].value,
        address: addContactTFForm.controls['address'].value,
        favourite: addContactTFForm.controls['favourite'].value,
        imageByte: this.contact.imageByte,
      };

      this.contactServicec.addContact(addContact).subscribe({
        next:(response)=>{
          if(response.success){
            console.log(addContactTFForm.value);
            this.router.navigate(['/filteredContact']);
          }else{
            alert(response.message);
          }
          this.loading = false;
        },
        error:(err)=>{
          console.log(err)
          console.log(err.error.message);
          this.loading = false;
          alert(err.error.message);
        },
        complete:()=>{
          this.loading = false;
          console.log("completed");
        }
      });
      }
    }
  }

