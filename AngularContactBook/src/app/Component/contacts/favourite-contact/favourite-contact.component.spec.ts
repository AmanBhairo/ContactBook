import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavouriteContactComponent } from './favourite-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('FavouriteContactComponent', () => {
  let component: FavouriteContactComponent;
  let fixture: ComponentFixture<FavouriteContactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,RouterTestingModule],
      declarations: [FavouriteContactComponent]
    });
    fixture = TestBed.createComponent(FavouriteContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
