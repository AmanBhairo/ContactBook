import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordupdateSuccessComponent } from './passwordupdate-success.component';

describe('PasswordupdateSuccessComponent', () => {
  let component: PasswordupdateSuccessComponent;
  let fixture: ComponentFixture<PasswordupdateSuccessComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PasswordupdateSuccessComponent]
    });
    fixture = TestBed.createComponent(PasswordupdateSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
