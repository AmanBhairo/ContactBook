import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatebasedreportComponent } from './statebasedreport.component';

describe('StatebasedreportComponent', () => {
  let component: StatebasedreportComponent;
  let fixture: ComponentFixture<StatebasedreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StatebasedreportComponent]
    });
    fixture = TestBed.createComponent(StatebasedreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
