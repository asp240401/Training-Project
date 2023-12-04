import { Component, OnDestroy } from '@angular/core';
import { ManualModeService } from '../services/manual-mode.service';
import * as signalR from "@microsoft/signalr"
import { Commands } from '../services/Commands';

@Component({
  selector: 'app-current',
  templateUrl: './current.component.html',
  styleUrls: ['./current.component.scss']
})

/// <summary>
///   Represents a component used to get and display current and ADC data.
/// </summary>
export class CurrentComponent{

   // Represents the current value
  current=""
  // Represents the ADC value
  adc=""

  /// <summary>
  ///   Adds a listener to handle ADC data received.
  ///   Listens for 'transferadcdata' event and updates the current and ADC values.
  /// </summary>
  public addAdcDataListener = () => {
    this.manualModeService.hubConnection.on('transferadcdata', (data) => {
      console.log(data);
      var words=data.split(' ')
      this.current=words[1]
      this.adc=words[2]
    });
  }

  /// <summary>
  ///   Constructor injecting ManualModeService to the component.
  /// </summary>
  /// <param name="manualModeService">
  ///   An instance of ManualModeService to be injected.
  /// </param>
  constructor(private manualModeService:ManualModeService,private commands:Commands){
    this.addAdcDataListener(); 
  }

  /// <summary>
  ///   Method to read ADC data from the device.
  ///   Initiates a 'GETADC' command using ManualModeService.
  /// </summary>
  readAdc()
  {
    this.manualModeService.postCommand(this.commands.get_ADC).subscribe()
  }
}
