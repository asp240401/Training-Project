import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { settingsResolverResolver } from './settings-resolver.resolver';

describe('settingsResolverResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => settingsResolverResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
