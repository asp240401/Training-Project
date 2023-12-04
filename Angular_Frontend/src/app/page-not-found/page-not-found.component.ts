import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.scss']
})

//represents the Page-Not-Found page
export class PageNotFoundComponent {

  constructor(private router:Router)
  {
    
  }

  // Method to navigate back home
  home()
  {
    this.router.navigate([''])
  }
}
