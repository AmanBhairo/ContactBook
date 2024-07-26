import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { States } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-update-contact-rf',
  templateUrl: './update-contact-rf.component.html',
  styleUrls: ['./update-contact-rf.component.css']
})
export class UpdateContactRfComponent implements OnInit {
  @ViewChild('imageInput') imageInput!: ElementRef;

  updateContactForm!: FormGroup; // Define FormGroup for reactive form
  contactId: number | undefined;
  countryId: number = 0;
  countries: Country[] = [];
  states: States[] = [];

  imageUrl: string | ArrayBuffer | null = null;
  fileToUpload: File | null = null;
  fileName = '';
  tempProfilePic = '';
  fileSizeExceeded = false;
  fileFormatInvalid = false;

  loading = false;

  constructor(
    private formBuilder: FormBuilder, // Inject FormBuilder for reactive form
    private contactService: ContactService,
    private countryService: CountryService,
    private stateService: StateService,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.updateContactForm = this.formBuilder.group({
      contactId: [0],
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
    this.route.params.subscribe((params) => {
      this.contactId = params['id'];

    });

    this.loadContactDetail(this.contactId);
    this.loadCountries();
    // this.countryId = this.updateContactForm.get('countryId')?.value;
    // console.log("Countryid from ngoninit :"+this.countryId);
    // this.loadStates(this.countryId);
    this.fetchStateByCountry();

  }


  fetchStateByCountry(): void {
    this.updateContactForm.get('countryId')?.valueChanges.subscribe((countryId: number) => {
      if (countryId !== 0) {
        this.states = [];
        this.updateContactForm.get('stateId')?.setValue(null); // Reset the state control's value to null

        this.loading = true;
        this.stateService.getStatesByCountryId(countryId).subscribe({
          next: (response: ApiResponse<States[]>) => {
            if (response.success) {
              this.states = response.data;
            } else {
              console.error('Failed to fetch states', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Failed to fetch states', error);
            this.loading = false;
          }
        });
      }
    });
  }



  // Convenience getters for easy access to form controls
  get f() {
    return this.updateContactForm.controls;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const fileType = file.type; // Get the MIME type of the file
      if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg') {
        if (file.size > 50 * 1024) {
          this.fileSizeExceeded = true; // Set flag to true if file size exceeds the limit
          const inputElement = document.getElementById('imageByte') as HTMLInputElement;
          inputElement.value = '';
          return;
        }
        this.fileFormatInvalid = false;
        this.fileSizeExceeded = false;

        const reader = new FileReader();
        reader.onload = () => {
          this.updateContactForm.patchValue({
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

  loadContactDetail(contactId: number | undefined): void {
    this.contactService.GetContactById(contactId).subscribe({
      next: (response) => {
        if (response.success) {
          this.updateContactForm.patchValue({
            contactId: response.data.contactId,
            countryId: response.data.countryId,
            stateId: response.data.stateId,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            address: response.data.address,
            contactNumber: response.data.contactNumber,
            contactDescription: response.data.contactDescription,
            email: response.data.email,
            gender: response.data.gender,
            favourite: response.data.favourite,
            profilePic: response.data.profilePic,
            imageByte: response.data.imageByte,
          });

          this.loadStates(response.data.countryId);
          if (response.data.imageByte) {

            this.imageUrl = 'data:image/jpeg;base64,' + response.data.imageByte;
          }
        } else {
          console.error('Failed to fetch contacts: ', response.message);
        }
      },
      error: (error) => {
        console.error('Error fetching contacts: ', error);
      }
    });
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

  onCountryChange() {
    const countryId = this.updateContactForm.value.countryId;
    this.updateContactForm.patchValue({ stateId: null });
    this.loadStates(countryId);
  }

  removeFile() {
    this.updateContactForm.patchValue({
      imageByte: '',
      imageUrl: null,
      image: null,
      profilePic: ''
    });
    this.imageUrl = null; // Also reset imageUrl to remove the displayed image
    const inputElement = document.getElementById('imageByte') as HTMLInputElement;
    inputElement.value = '';

  }

  onSubmit(): void {
    if (this.updateContactForm.invalid) {
      return;
    }

    this.loading = true;
    const formData = this.updateContactForm.value;

    this.contactService.UpdateContact(formData).subscribe({
      next: (response) => {
        if (response.success) {
          console.log('Contact updated successfully:', response.data);
          this.router.navigate(['/filteredContact']);
        } else {
          alert(response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error updating contact:', err);
        alert(err.error.message);
        this.loading = false;
      }
    });
  }
}
