import { Component, OnDestroy } from '@angular/core';
import {MatTabsModule} from '@angular/material/tabs';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { PortService } from '../services/port.service';
import { SensorDataService } from '../services/sensor-data.service';
import { Port } from '../models/Port';
import { ManualModeService } from '../services/manual-mode.service';
import * as signalR from "@microsoft/signalr"

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})

///<summary>
/// Component used to display settings, graph and manual mode components
/// mat-tab is used to display settings page and manual mode page as different tabs
///</summary>
export class DetailsComponent implements OnDestroy{
 
  active=true

  /// <summary>
  ///   Adds a listener to handle error data received.
  ///   Listens for 'transferreplyerror' event and performs redirection to login page
  ///   when an error is received.
  /// </summary>
  public addErrorDataListener = () => {
    this.manualModeService.hubConnection.on('transferreplyerror', (data) => {
      console.log(data);
      localStorage.setItem("connected","no")
      alert("port closed!device may have been removed!!")
      this.router.navigate([''])
    });
  }

  /// <summary>
  /// Constructor injecting necessary services to the component.
  /// </summary>
  /// <param name="router">Router for navigation</param>
  /// <param name="portService">Service to connect/disconnect from port</param>
  /// <param name="sensorDataService">Service for getting sensor data</param>
  /// <param name="manualModeService">Service for manual mode</param>
  constructor(private router:Router,private portService:PortService,private sensorDataService:SensorDataService,private manualModeService:ManualModeService)
  {
    this.manualModeService.startConnection();
    this.addErrorDataListener(); 
  }


  /// <summary>
  /// Handles manual mode events, updating the 'active' status based on the event value.
  /// </summary>
  /// <param name="evt">The event value determining the mode ('active' or otherwise)</param>
  handleManualMode(evt:any)
  {
    if(evt=='active')
    {
      this.active=true
    }
    else
    {
      this.active=false
    }
  }

  /// <summary>
  /// Disconnects the device, stops sensor data service, and navigates to the home page.
  /// </summary>
  disconnect()
  {
    localStorage.setItem("connected","no")
    this.sensorDataService.hubConnection.stop()

    this.portService.disconnect(new Port("",0,0,"","","")).subscribe(data=>{
      this.router.navigate([''])
    })
  }

  /// <summary>
  ///   Handles tab clicks, toggling between manual and auto modes
  ///   and sending commands accordingly to the manual mode service.
  /// </summary>
  /// <param name="tab">The selected tab object</param>
  tabClick(tab:any) {
    console.log(tab);
    if(tab.index==1)
    {
      console.log("manual mode")
      this.active=false
      this.manualModeService.postCommand("SETMAN ON").subscribe()
    }
    else
    {
      console.log("auto mode")
      this.active=true
      this.manualModeService.postCommand("SETMAN OFF").subscribe()
    }
  }

  /// <summary>
  /// Implements the OnDestroy method to handle cleanup when the component is destroyed.
  /// Disconnects from the signalR hub when the component is destroyed.
  /// </summary>
  ngOnDestroy(): void{
    this.manualModeService.hubConnection.stop()
  }
}
