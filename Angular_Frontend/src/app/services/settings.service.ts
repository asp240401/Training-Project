import { HttpErrorResponse, HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Setttings } from '../models/Settings';
import * as signalR from "@microsoft/signalr"
import { SensorData } from '../models/SensorData';

@Injectable({
  providedIn: 'root'
})

/// <summary>
/// Service handling settings related operations.
/// Used for getting and setting the configuration settings values such as threshold and data acquisition rate
/// </summary>
export class SettingsService {

  baseUrl:string="http://localhost:3000"

  /// <summary>
  /// Handles HTTP error responses.
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

  /// <summary>
  /// Posts settings data to the server.
  /// </summary>
  /// <param name="command">The settings command to post.</param>
  /// <returns>An Observable of type string.</returns>
  postSettings(command:string):Observable<string>{
    console.log(JSON.stringify(command))
    return this.httpClient.post<string>(this.baseUrl+'/Settings',JSON.stringify(command),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }

  /// <summary>
  /// Retrieves saved settings from the server.
  /// </summary>
  /// <returns>An Observable of type Settings.</returns>
  getSavedSettings():Observable<Setttings>{
    return this.httpClient.get<Setttings>(this.baseUrl+'/Settings')
    .pipe(
      catchError(this.httpError)
    );
  }
}
