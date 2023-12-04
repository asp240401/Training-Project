import { HttpErrorResponse, HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { SensorData } from '../models/SensorData';
import * as signalR from "@microsoft/signalr"
import { SettingsComponent } from '../settings/settings.component';

@Injectable({
  providedIn: 'root'
})

/// <summary>
/// Service handling sensor data retrieval and real-time updates.
/// </summary>
export class SensorDataService {

  baseUrl:string="http://localhost:3000"

  /// <summary>
  ///   Handles HTTP error responses.
  /// </summary>
  httpError(error:HttpErrorResponse){
    let msg=""
    if(error.error instanceof ErrorEvent){
      msg=error.error.message
    }
    else{
      msg=`Error code:${error.status}\nMessage:${error.message}`;
      alert(msg)
    }
    return throwError(msg)
  }

  httpHeader={
    headers:new HttpHeaders({
      'Content-Type':'application/json',
    })
  }

  constructor(private httpClient:HttpClient) 
  { 
    
  }
  
  public data: SensorData[]=[];
  public hubConnection!: signalR.HubConnection;
  
  /// <summary>
  ///   Initializes and starts the signalR connection.
  /// </summary>
  public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('http://localhost:3000/data',{ skipNegotiation: true,transport: signalR.HttpTransportType.WebSockets})
                              .build();
      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
  }
    
  /// <summary>
  ///   Adds a listener for incoming chart data updates.
  /// </summary>
  public addTransferChartDataListener = () => {
      this.hubConnection.on('transferchartdata', (data) => {
        this.data.push(data);
        console.log(data);
      });
  }
}
