import { HttpErrorResponse, HttpHeaders,HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/// <summary>
/// This service is responsible for handling database operations 
/// </summary>
export class DatabaseService {

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

  constructor(private httpClient:HttpClient) 
  { 

  }

  /// <summary>
  /// Posts data to the specified endpoint for database storage.
  /// </summary>
  /// <param name="command">The data to be sent for storage.</param>
  /// <returns>An Observable of type string representing the response.</returns>
  postData(command:string):Observable<string>{
    
    return this.httpClient.post<string>(this.baseUrl+'/SensorDatas',JSON.stringify(command),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }
}
