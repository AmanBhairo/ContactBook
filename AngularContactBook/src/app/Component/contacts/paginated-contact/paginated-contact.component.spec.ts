import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginatedContactComponent } from './paginated-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

describe('PaginatedContactComponent', () => {
  let component: PaginatedContactComponent;
  let fixture: ComponentFixture<PaginatedContactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule],
      declarations: [PaginatedContactComponent]
    });
    fixture = TestBed.createComponent(PaginatedContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
