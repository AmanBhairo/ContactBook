import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenderbasedreportComponent } from './genderbasedreport.component';

describe('GenderbasedreportComponent', () => {
  let component: GenderbasedreportComponent;
  let fixture: ComponentFixture<GenderbasedreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GenderbasedreportComponent]
    });
    fixture = TestBed.createComponent(GenderbasedreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
