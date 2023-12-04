import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ManualModeService } from '../services/manual-mode.service';
import { Commands } from '../services/Commands';

@Component({
  selector: 'app-stepper-motor',
  templateUrl: './stepper-motor.component.html',
  styleUrls: ['./stepper-motor.component.scss']
})

/// <summary>
///   Class responsible for managing stepper motor actions.Handles user selections for motor action, direction and angle.
/// </summary>
export class StepperMotorComponent {
  
  // FormGroup for managing form controls
  myForm:FormGroup;

  // Default values for motor action
  motor="Motor 1"
  angle=0
  dir="Clockwise"

  // Boolean flags to track motor actions
  step=true
  rotate=false
  rotating1=false
  rotating2=false

  // Constructor initializing the form controls
  constructor(private fb:FormBuilder,private manualModeService:ManualModeService,private commands:Commands)
  { 
    this.myForm=this.fb.group({
      'motor':[],
      'action':[],
      'rotate':[],
      'angle':[]
    });
  }

  /// <summary>
  /// Toggles between step and rotate actions based on user selection.
  /// </summary>
  /// <param name="evt">Event triggered by the user's action selection.</param>
  actionSelect(evt:any)
  {
    console.log(evt.target.value)
    if(evt.target.value=="Rotate")
    {
      this.rotate=true
      this.step=false
    }
    if(evt.target.value=="Step")
    {
      this.step=true
      this.rotate=false
    }
  }

  /// <summary>
  ///   Handles the direction (clockwise or anti-clockwise) selected from the dropdown. Updates the 'dir' variable with the selected direction.
  /// </summary>
  /// <param name="evt">Event triggered by the user's direction selection.</param>
  dirSelect(evt:any)
  {
    console.log(evt.target.value)
    this.dir=evt.target.value
  }

  /// <summary>
  ///   Handles the change in step angle from the dropdown. Updates the 'angle' variable with the selected angle.
  /// </summary>
  /// <param name="evt">Event triggered by the user's angle selection.</param>
  angleSelect(evt:any)
  {
    console.log(evt.target.value)
    this.angle=evt.target.value
  }

  /// <summary>
  ///   Handles the change in motor selection (motor 1 or 2) from the dropdown. Updates the 'motor' variable with the selected motor.
  /// </summary>
  /// <param name="evt">Event triggered by the user's motor selection.</param>
  motorSelect(evt:any)
  {
    console.log(evt.target.value)
    this.motor=evt.target.value
  }

  //command to be sent to hardware for motor actions
  command=""

  /// <summary>
  /// Executes motor rotation or step based on user selection.
  /// </summary>
  execute()
  {
    console.log("HEllo")
    if(this.rotate==true)
    {
        this.command=this.commands.motor_rotate+" "+(this.dir=="Clockwise"?"RCK":"ACK")+" "+(this.motor=="Motor 1"?"1":"2")
        if(this.motor=="Motor 1")
          this.rotating1=true
        if(this.motor=="Motor 2")
          this.rotating2=true
    }
    else
    {  
      this.command=this.commands.motor_step+this.angle+" "+(this.motor=="Motor 1"?"1":"2")

      if(this.motor=="Motor 1")
          this.rotating1=false
        if(this.motor=="Motor 2")
          this.rotating2=false
    }

    this.manualModeService.postCommand(this.command).subscribe()
  }

  /// <summary>
  /// Stops rotation for Motor 1.
  /// </summary>
  stopRotate1()
  {
    this.rotating1=false
    this.command=this.commands.motor_step+"0"+" "+"1"
    console.log(this.command)
    this.manualModeService.postCommand(this.command).subscribe()
  }

  /// <summary>
  /// Stops rotation for Motor 2.
  /// </summary>
  stopRotate2()
  {
    this.rotating2=false
    this.command=this.commands.motor_step+"0"+" "+"2"
    console.log(this.command)
    this.manualModeService.postCommand(this.command).subscribe()
  }
}
