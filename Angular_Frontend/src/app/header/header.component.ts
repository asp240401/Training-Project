import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PortService } from '../services/port.service';
import { Port } from '../models/Port';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})

/// <summary>
/// Represents the header component handling navigation and disconnection.
/// </summary>
export class HeaderComponent {
  
  /// <summary>
  ///   Constructor injecting necessary services to the component.
  /// </summary>
  /// <param name="router">Router for navigation</param>
  /// <param name="portService">Service for managing ports</param>
  constructor(private router:Router,private portService:PortService){ 
    
  }

  ///<summary>
  /// Navigates to the home page and performs disconnection actions.
  ///</summary>
  goHome()
  {
    localStorage.setItem("connected","no")
    this.portService.disconnect(new Port("",0,0,"","","")).subscribe(data=>{
      this.router.navigate([''])
    })
  }
}
