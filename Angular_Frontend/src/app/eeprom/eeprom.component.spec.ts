import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EEPROMComponent } from './eeprom.component';

describe('EEPROMComponent', () => {
  let component: EEPROMComponent;
  let fixture: ComponentFixture<EEPROMComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EEPROMComponent]
    });
    fixture = TestBed.createComponent(EEPROMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
