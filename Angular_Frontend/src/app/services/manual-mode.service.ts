import { HttpErrorResponse, HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Port } from '../models/Port';
import * as signalR from "@microsoft/signalr"

@Injectable({
  providedIn: 'root'
})

/// <summary>
/// This service facilitates sending manual mode commands to the connected device
/// through the C# backend. Additionally, it establishes
/// a SignalR hub connection to receive and handle responses from the device.
/// </summary>
export class ManualModeService {

  baseUrl:string="http://localhost:3000"

  /// <summary>
  /// Handles HTTP error responses for the service.
  /// </summary>
  /// <param name="error">The HttpErrorResponse object.</param>
  /// <returns>A string representing the error message.</returns>
  httpError(error:HttpErrorResponse){
    let msg=""
    if(error.error instanceof ErrorEvent){
      msg=error.error.message
    }
    else{
      msg=`Error code:${error.status}\nMessage:${error.message} `;
      if(error.status==400)
      alert('could not write command to serial port! check connection')
    }
    return throwError(msg)
  }

  httpHeader={
    headers:new HttpHeaders({
      'Content-Type':'application/json',
    })
  }

  public hubConnection!: signalR.HubConnection;
  
  /// <summary>
  /// Initializes and starts the SignalR hub connection for listening to device responses.
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

  constructor(private httpClient:HttpClient) 
  { 
    
  }

  /// <summary>
  /// Posts a command to the backend for manual mode operation.
  /// </summary>
  /// <param name="command">The command to be sent in manual mode.</param>
  /// <returns>An Observable of type string.</returns>
  postCommand(command:string):Observable<string>{
    
    return this.httpClient.post<string>(this.baseUrl+'/ManualMode',JSON.stringify(command),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }
}
