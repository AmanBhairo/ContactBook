import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { States } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact-rf',
  templateUrl: './add-contact-rf.component.html',
  styleUrls: ['./add-contact-rf.component.css']
})
export class AddContactRfComponent implements OnInit{

  addContactForm!: FormGroup;
  countries: Country[] = [];
  states: States[] = [];
  imageUrl: string | ArrayBuffer | null = null;
  fileName = '';
  loading = false;
  @ViewChild('imageInput') imageInput!: ElementRef;
  fileSizeExceeded =  false;
  fileFormatInvalid =  false;

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService,
    private countryService: CountryService,
    private stateService: StateService,
    private router: Router
  ) {
    
  }

  ngOnInit(): void {
    this.addContactForm = this.fb.group({
      countryId: [0, Validators.required],
      stateId: [0, Validators.required],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      contactNumber: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      email: ['', [Validators.required, Validators.email]],
      contactDescription: ['', [Validators.required, Validators.minLength(3)]],
      profilePic: [''],
      gender: ['', Validators.required],
      address: ['', [Validators.required, Validators.minLength(3)]],
      favourite: [false],
      imageByte: ['']
    });
    this.loadCountries();
    this.onCountryChange();
  }
  get formcontrols(){
    return this.addContactForm.controls;
  }

  onCountryChange(): void {
    const countryId = this.addContactForm.get('countryId')?.value;
    this.loadStates(countryId);
    this.addContactForm.get('stateId')?.reset();
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const fileType = file.type; // Get the MIME type of the file
      if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg')
         {
          if(file.size > 50 * 1024)
            {
              this.fileSizeExceeded = true; // Set flag to true if file size exceeds the limit
              const inputElement = document.getElementById('imageByte') as HTMLInputElement;
              inputElement.value = '';
              return;
            }
            this.fileFormatInvalid = false;
            this.fileSizeExceeded = false; 

      const reader = new FileReader();
      reader.onload = () => {
        this.addContactForm.patchValue({
          imageByte: (reader.result as string).split(',')[1],
          profilePic: file.name
        });
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(file);
    }
    else {
      this.fileFormatInvalid = true;
      const inputElement = document.getElementById('imageByte') as HTMLInputElement;
      inputElement.value = '';

       
    }

   
  }
}

  readImage(file: File): void {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imageUrl = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  loadCountries(): void {
    this.loading = true;
    this.countryService.getAllCountries().subscribe({
      next: (response: ApiResponse<Country[]>) => {
        if (response.success) {
          this.countries = response.data;
        } else {
          console.error('Failed to fetch countries ', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching countries: ', error);
        this.loading = false;
      }
    });
  }

  loadStates(countryId: number): void {
    this.loading = true;
    this.stateService.getStatesByCountryId(countryId).subscribe({
      next: (response: ApiResponse<States[]>) => {
        if (response.success) {
          this.states = response.data;
        } else {
          console.error('Failed to fetch states ', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching states: ', error);
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.addContactForm.valid) {
      this.loading = true;
      const formData = this.addContactForm.value;
      this.contactService.addContact(formData).subscribe({
        next: (response) => {
          if (response.success) {
            this.router.navigate(['/filteredContact']);
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error: (err) => {
          console.error(err);
          alert(err.error.message);
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        }
      });
    }
  }
}
