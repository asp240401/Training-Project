import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LEDComponent } from './led.component';

describe('LEDComponent', () => {
  let component: LEDComponent;
  let fixture: ComponentFixture<LEDComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LEDComponent]
    });
    fixture = TestBed.createComponent(LEDComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
