import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportdeciderComponent } from './reportdecider.component';

describe('ReportdeciderComponent', () => {
  let component: ReportdeciderComponent;
  let fixture: ComponentFixture<ReportdeciderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReportdeciderComponent]
    });
    fixture = TestBed.createComponent(ReportdeciderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
