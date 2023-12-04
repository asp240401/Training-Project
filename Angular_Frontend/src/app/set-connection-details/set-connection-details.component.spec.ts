import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetConnectionDetailsComponent } from './set-connection-details.component';

describe('SetConnectionDetailsComponent', () => {
  let component: SetConnectionDetailsComponent;
  let fixture: ComponentFixture<SetConnectionDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetConnectionDetailsComponent]
    });
    fixture = TestBed.createComponent(SetConnectionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
