import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../services/settings.service';
import { PortService } from '../services/port.service';
import { Port } from '../models/Port';
import { GraphComponent } from '../graph/graph.component';
import { SensorDataService } from '../services/sensor-data.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})

/// <summary>
/// Component which manages user settings such as data acquisition rate and threshold values.
/// Handles form submission, navigation, and interactions with services.
/// </summary>
export class SettingsComponent{

  // FormGroup to handle form controls
  myForm!: FormGroup;

  dar:number=1000; //data acquisition rate in ms
  lowThreshold:number=1
  highThreshold:number=2

  // Flag to manage blinking of alarm
  blink=false

  // Flag for form submission
  submitted=false

  constructor(private router:Router,private fb:FormBuilder,private settingsService:SettingsService,
    private activatedRoute: ActivatedRoute,
    private portService:PortService,
    private sensorDataService:SensorDataService,
    private _snackBar: MatSnackBar)
  {
    this.myForm=this.fb.group({
      'dataAqRate':[],
      'lowThr':[,Validators.required],
      'highThr':[,Validators.required]
    },
    {
      validator:this.MustMatch('lowThr','highThr')
    });

    this.settingsService.getSavedSettings().subscribe(data=>{
      this.dar=data.dataAcquisitionRate
      this.lowThreshold=data.thresholdLow
      this.highThreshold=data.thresholdHigh

      this.myForm.patchValue({
        dataAqRate:this.dar,
        lowThr:this.lowThreshold,
        highThr:this.highThreshold
      })
    })
  }

  /// <summary>
  ///   Handles changes in settings and posts updated settings to the service.
  /// </summary>
  /// <remarks>
  ///   Checks for form validity and ensures lowThreshold < highThreshold.
  ///   Updates local threshold values and sends new settings to the service.
  /// </remarks>
  changeSettings()
  {
    this.submitted=true
    if(this.myForm.invalid)
      return

    if(isNaN(this.myForm.controls['lowThr'].value)||isNaN(this.myForm.controls['highThr'].value))
    {
      alert("invalid thresholds! resetting to defaults")
      this.lowThreshold=1
      this.highThreshold=2
      this.myForm.controls['lowThr'].setValue(1)
      this.myForm.controls['highThr'].setValue(2)
    }
    else
    {
      this.lowThreshold=this.myForm.controls['lowThr'].value
      this.highThreshold=this.myForm.controls['highThr'].value
    }
    
    var command1=this.lowThreshold+" "+this.highThreshold+" "+this.dar
    console.log(command1)
    this._snackBar.open("Settings Saved","Ok",{
      duration: 1500
    });

    this.settingsService.postSettings(command1).subscribe()
  
  }

  /// <summary>
  /// Disconnects from the device using portService and redirects to home page. Also disconnects from the signalR hub.
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
  /// Resets settings to default values.
  /// </summary>
  reset()
  {
    this.dar=1000
    this.lowThreshold=1
    this.highThreshold=2

    this.myForm.controls['dataAqRate'].setValue(this.dar)
    this.myForm.controls['lowThr'].setValue(this.lowThreshold)
    this.myForm.controls['highThr'].setValue(this.highThreshold)

    var command2=this.lowThreshold+" "+this.highThreshold+" "+this.dar
    this.settingsService.postSettings(command2).subscribe()
  }

  /// <summary>
  ///   Sets the blinker state based on the event received ('yes' or 'no').
  /// </summary>
  /// <param name="evt">Event data indicating blinking state.</param>
  setBlinker(evt:any)
  {
    console.log(evt)
    if(evt=="yes")
      this.blink=true
    if(evt=="no")
    {
      console.log("alarm")
      this.blink=false
    }
  }

  /// <summary>
  ///   Logs the updated data acquisition rate value to the console.
  ///   Updates the local 'dar' variable with the new value.
  /// </summary>
  /// <param name="evt">Event containing the updated data acquisition rate value.</param>
  selectDataRate(evt:any)
  {
    console.log("dar changed to:"+evt.target.value)
    this.dar=evt.target.value
  }

  /// <summary>
  ///   Validator to check if lower threshold is less than upper threshold.
  ///   Compares two form controls to ensure that 'control' is less than 'matchingControl'.
  ///   Sets 'mustMatch' error if the condition is not met; otherwise, clears the error.
  /// </summary>
  /// <param name="controlName">The name of the form control.</param>
  /// <param name="matchingControlName">The name of the control to match against.</param>
  /// <returns>A validator function to check control matching.</returns>
  MustMatch(controlName:string,matchingControlName:string){
    return(formGroup:FormGroup)=>{
      const control=formGroup.controls[controlName]
      const matchingControl=formGroup.controls[matchingControlName]

      if(matchingControl.errors && !matchingControl.errors['mustMatch']){
        return;
      }
      if(control.value>=matchingControl.value)
      {
        matchingControl.setErrors({mustMatch:true})
      }
      else
      {
        matchingControl.setErrors(null)
      }
    }
  }

}
