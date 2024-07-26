import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirthdaybasedreportComponent } from './birthdaybasedreport.component';

describe('BirthdaybasedreportComponent', () => {
  let component: BirthdaybasedreportComponent;
  let fixture: ComponentFixture<BirthdaybasedreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BirthdaybasedreportComponent]
    });
    fixture = TestBed.createComponent(BirthdaybasedreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
