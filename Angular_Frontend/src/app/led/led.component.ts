import { Component, OnDestroy } from '@angular/core';
import { ManualModeService } from '../services/manual-mode.service';
import * as signalR from "@microsoft/signalr"
import { HttpClient } from '@angular/common/http';
import { Commands } from '../services/Commands';

@Component({
  selector: 'app-led',
  templateUrl: './led.component.html',
  styleUrls: ['./led.component.scss']
})

/// <summary>
///   Represents the LED component managing LED statuses and commands.
/// </summary>
export class LEDComponent{

  // Status variables for different LEDs
  greenStatus=false
  redStatus=false
  yellowStatus=false

  /// <summary>
  ///   Adds a listener to handle LED data received from the manual mode service.
  /// </summary>
  public addLedDataListener = () => {
    this.manualModeService.hubConnection.on('transferreplyled', (data) => {
      console.log(data);
    });
  }

  /// <summary>
  ///   Constructor injecting necessary services to the component.
  /// </summary>
  /// <param name="manualModeService">Service for manual mode operations</param>
  /// <param name="http">HttpClient for HTTP requests</param>
  constructor(private manualModeService:ManualModeService,private http : HttpClient,private commands:Commands){
    this.addLedDataListener();   
  }

  //Command variable for sending LED control commands
  command=""

  /// <summary>
  ///   Handles the click event for the green LED switch.
  /// </summary>
  /// <param name="green">Reference to the green checkbox element</param>
  clickedGreen(green:HTMLInputElement)
  {
    if(green.checked==true)
    {
      this.command=this.commands.green_LED_ON
    }
    else
    {
      this.command=this.commands.green_LED_OFF
    }

    this.manualModeService.postCommand(this.command).subscribe()
  }

  /// <summary>
  ///   Handles the click event for the red LED switch.
  /// </summary>
  /// <param name="red">Reference to the red checkbox element</param>
  clickedRed(red:HTMLInputElement)
  {
    if(red.checked==true)
    {
      this.command=this.commands.red_LED_ON
    }
    else
    {
      this.command=this.commands.red_LED_OFF
    }

    this.manualModeService.postCommand(this.command).subscribe()
  }

  /// <summary>
  ///   Handles the click event for the blue LED switch.
  /// </summary>
  /// <param name="blue">Reference to the blue checkbox element</param>
  clickedBlue(blue:HTMLInputElement)
  {
    if(blue.checked==true)
    {
      this.command=this.commands.blue_LED_ON
    }
    else
    {
      this.command=this.commands.blue_LED_OFF
    }

    this.manualModeService.postCommand(this.command).subscribe()
  }
}
