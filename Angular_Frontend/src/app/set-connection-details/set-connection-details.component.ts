import { Component } from '@angular/core';
import {MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortService } from '../services/port.service';
import { Router } from '@angular/router';
import { Port } from '../models/Port';

@Component({
  selector: 'app-set-connection-details',
  templateUrl: './set-connection-details.component.html',
  styleUrls: ['./set-connection-details.component.scss']
})

/// <summary>
///   Component to set connection details for UART communication.
/// </summary>
export class SetConnectionDetailsComponent {

  // Define properties to hold dropdown options and selected values
  portnames:string[]=[]
  portname:string=""
  
  handshakes:string[]=[]
  handshake:string="None"
  
  parities:string[]=[]
  parity:string="None"
  
  stopbits:string[]=[]
  stopbit:string=""

  baudrate:number=115200
  databits:number=8

  myForm: FormGroup;

  //flag representing whether form's submit button is clicked or not
  submitted = false;

  /// <summary>
  ///   Initializes a new instance of the SetConnectionDetailsComponent class.
  /// </summary>
  /// <param name="fb">FormBuilder instance for handling form controls.</param>
  /// <param name="portService">PortService instance for fetching dropdown options and sending data to the server.</param>
  /// <param name="router">Router instance for navigation between routes.</param>
  constructor(private fb:FormBuilder,private portService:PortService,private router:Router)
  {
    this.myForm=this.fb.group({
      'portname':[],
      'handshake':[],
      'baudrate':[],
      'parity':[],
      'databits':[],
      'stopbits':[]
    });

    //Fetch dropdown options from PortService
    this.portService.getPortNames().subscribe(data=>
      {
        this.portnames=data
        console.log(this.portnames)  
  
        this.portService.getHandshakes().subscribe(data=>{
          this.handshakes=data
          this.portService.getParityOptions().subscribe(data=>{
            this.parities=data

            this.portService.getStopBits().subscribe(data=>{
              this.stopbits=data})
          })
        })
      })
  }

  // Getter to access form controls
  get f(){
    return this.myForm.controls;
  }


  /// <summary>
  /// Handles the selection of a port name from the dropdown.
  /// </summary>
  /// <param name="evt">The event containing the selected port name.</param>
  /// <remarks>
  /// Updates the selected port name value based on the user selection.
  /// </remarks>
  selectPortname(evt:any)
  {
    console.log(evt.target.value)
    this.portname=evt.target.value
  }
  // (Other select functions follow the same structure as selectPortname)
  selectParity(evt:any)
  {
    console.log(evt.target.value)
    this.parity=evt.target.value
  }
  selectHandshake(evt:any)
  {
    console.log(evt.target.value)
    this.handshake=evt.target.value
  }
  selectStopbits(evt:any)
  {
    console.log(evt.target.value)
    this.stopbit=evt.target.value
  }
  selectBaudrate(evt:any)
  {
    console.log(evt.target.value)
    this.baudrate=evt.target.value
  }
  selectDatabits(evt:any)
  {
    console.log(evt.target.value)
    this.databits=evt.target.value
  }

  /// <summary>
  /// Handles the form submission and initiates the UART connection.
  /// Redirects to the Details Page
  /// </summary>
  ViewDetails()
  {
    this.submitted=true

    if(localStorage.getItem("connected")=="yes")
    {
      this.router.navigate(['details']);
    }
    else
    {
      var port=new Port(this.portname,parseInt(this.baudrate.toString()),parseInt(this.databits.toString()),this.handshake,this.stopbit,this.parity)
      this.portService.postConnectionParameters(port).subscribe(
        data=>{
          console.log(data.PortName)
          if(data.PortName!=port.PortName)
          {
              localStorage.setItem("connected","no")
          }
          else
          {
            localStorage.setItem("connected","yes")
          }
          
          this.router.navigate(['details']);
        }
      )
    }
  }
}
