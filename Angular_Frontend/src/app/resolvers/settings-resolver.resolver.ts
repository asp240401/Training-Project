import { ResolveFn } from '@angular/router';
import { Setttings } from '../models/Settings';
import { SettingsService } from '../services/settings.service';
import { inject } from '@angular/core';

export const settingsResolverResolver: ResolveFn<Setttings> = (route, state) => {
  
  return inject(SettingsService).getSavedSettings();
};
