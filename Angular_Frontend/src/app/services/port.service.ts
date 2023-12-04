import { HttpErrorResponse, HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError, Observable, catchError } from 'rxjs';
import { SensorData } from '../models/SensorData';
import { Port } from '../models/Port';

@Injectable({
  providedIn: 'root'
})

/// <summary>
/// Service managing UART connection parameters and serial port interactions.
/// Provides methods to retrieve available options for connection settings
/// and perform operations such as connecting and disconnecting from the serial port.
/// </summary>
export class PortService {

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
    }
    return throwError(msg)
  }

  httpHeader={
    headers:new HttpHeaders({
      'Content-Type':'application/json',
      'Response-Type':'text'
    })
  }

  constructor(private httpClient:HttpClient) 
  { 
    
  }

  /// <summary>
  /// Retrieves available port names.
  /// </summary>
  getPortNames():Observable<string[]>{
    return this.httpClient.get<string[]>(this.baseUrl+'/ConnectionParameters/portnames')
    .pipe(
      catchError(this.httpError)
    );
  }

  /// <summary>
  /// Retrieves available handshake options.
  /// </summary>
  getHandshakes():Observable<string[]>{
    return this.httpClient.get<string[]>(this.baseUrl+'/ConnectionParameters/handshakes')
    .pipe(
      catchError(this.httpError)
    );
  }

  /// <summary>
  /// Retrieves available parity options.
  /// </summary>
  getParityOptions():Observable<string[]>{
    return this.httpClient.get<string[]>(this.baseUrl+'/ConnectionParameters/parity')
    .pipe(
      catchError(this.httpError)
    );
  }

  /// <summary>
  /// Retrieves available stop bit options.
  /// </summary>
  getStopBits():Observable<string[]>{
    return this.httpClient.get<string[]>(this.baseUrl+'/ConnectionParameters/stopbit')
    .pipe(
      catchError(this.httpError)
    );
  }

  /// <summary>
  /// Posts connection parameters to establish a serial port connection.
  /// </summary>
  /// <param name="port">The Port object containing connection details.</param>
  /// <returns>An Observable of type Port.</returns>
  postConnectionParameters(port:Port):Observable<Port>{
    console.log(JSON.stringify(port))
    return this.httpClient.post<Port>(this.baseUrl+'/ConnectionParameters',JSON.stringify(port),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }

  /// <summary>
  /// Disconnects from the specified port.
  /// </summary>
  disconnect(port:Port):Observable<Port>{
    return this.httpClient.post<Port>(this.baseUrl+'/ConnectionParameters/disconnect',JSON.stringify(port),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }
}
