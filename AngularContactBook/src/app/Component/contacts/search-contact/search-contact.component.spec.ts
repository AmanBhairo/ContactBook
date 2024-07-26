import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchContactComponent } from './search-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

describe('SearchContactComponent', () => {
  let component: SearchContactComponent;
  let fixture: ComponentFixture<SearchContactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [SearchContactComponent]
    });
    fixture = TestBed.createComponent(SearchContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
