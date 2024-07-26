import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CountrybasedreportComponent } from './countrybasedreport.component';

describe('CountrybasedreportComponent', () => {
  let component: CountrybasedreportComponent;
  let fixture: ComponentFixture<CountrybasedreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CountrybasedreportComponent]
    });
    fixture = TestBed.createComponent(CountrybasedreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
