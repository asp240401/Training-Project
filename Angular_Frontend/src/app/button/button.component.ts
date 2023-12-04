import { Component, OnDestroy } from '@angular/core';
import { ManualModeService } from '../services/manual-mode.service';
import * as signalR from "@microsoft/signalr"
import { Commands } from '../services/Commands';
@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})

/// <summary>
/// Represents a component to get and display the button status of the device.
/// </summary>
export class ButtonComponent{

  // Represents the status of the button
  status=""

  /// <summary>
  ///   Adds a listener to handle button status received.
  /// </summary>
  /// <remarks>
  ///   Listens for 'transferreplybutton' event and updates the status 
  ///   with received data.
  /// </remarks>
  public addButtonDataListener = () => {
      this.manualModeService.hubConnection.on('transferreplybutton', (data) => {
        console.log(data);
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
    this.addButtonDataListener(); 
  }

  /// <summary>
  ///   Method to fetch the button status from the device.
  ///   Initiates a 'GETBUTTON' command using ManualModeService.
  /// </summary>
  getButtonStatus()
  {
    this.manualModeService.postCommand(this.commands.get_button_status).subscribe()
  }
}
