import { Component, EventEmitter, OnDestroy, Output } from '@angular/core';
import { ManualModeService } from '../services/manual-mode.service';

@Component({
  selector: 'app-manual',
  templateUrl: './manual.component.html',
  styleUrls: ['./manual.component.scss']
})

/// <summary>
///   Angular Component used to send various manual mode commands and receive status, for different components like LED, Button etc.
/// </summary>
export class ManualComponent{

  // Variable to manage manual mode status
  manual=false

  // Variable for button color
  color='#557C55'

  buttonText="Enter Manual Mode"

  // EventEmitter to emit events to the parent component
  @Output() emitter=new EventEmitter<string>();

  /// <summary>
  ///   Constructor injecting the ManualModeService for manual mode operations.
  /// </summary>
  /// <param name="manualModeService">Service for manual mode operations</param>
  constructor(private manualModeService:ManualModeService)
  {
    
  }
}
