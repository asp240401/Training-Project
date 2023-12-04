import { TestBed } from '@angular/core/testing';

import { ManualModeService } from './manual-mode.service';

describe('ManualModeService', () => {
  let service: ManualModeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManualModeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
