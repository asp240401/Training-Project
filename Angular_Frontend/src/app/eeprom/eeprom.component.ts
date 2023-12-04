import { Component, OnDestroy } from '@angular/core';
import { ManualModeService } from '../services/manual-mode.service';
import * as signalR from "@microsoft/signalr"
import { Commands } from '../services/Commands';

@Component({
  selector: 'app-eeprom',
  templateUrl: './eeprom.component.html',
  styleUrls: ['./eeprom.component.scss']
})

/// <summary>
/// Represents a component to test EEPROM and display the status of the EEPROM test.
/// </summary>
export class EEPROMComponent{

  // Represents the status of the EEPROM test obtained from the device
  status=""

  /// <summary>
  ///   Adds a listener to handle EEPROM data received.
  ///   Listens for 'transferreplyerm' event and updates the status with received data.
  /// </summary>
  public addErmDataListener = () => {
    this.manualModeService.hubConnection.on('transferreplyerm', (data) => {
      this.status=data
    });
  }

  /// <summary>
  ///   Constructor injecting ManualModeService to the component.
  /// </summary>
  /// <param name="manualModeService">
  ///   An instance of ManualModeService to be injected.
  /// </param>
  constructor(private manualModeService:ManualModeService,private commands:Commands){
    this.addErmDataListener(); 
  }

  /// <summary>
  /// Initiates the ERMTEST command to test EEPROM functionality using ManualModeService
  /// </summary>
  testEEPROM()
  {
    this.manualModeService.postCommand(this.commands.eeprom_test).subscribe()
  }
}
