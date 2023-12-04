import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepperMotorComponent } from './stepper-motor.component';

describe('StepperMotorComponent', () => {
  let component: StepperMotorComponent;
  let fixture: ComponentFixture<StepperMotorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StepperMotorComponent]
    });
    fixture = TestBed.createComponent(StepperMotorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
