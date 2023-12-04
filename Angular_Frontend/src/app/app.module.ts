import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BodyComponent } from './body/body.component';
import { HeaderComponent } from './header/header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SetConnectionDetailsComponent } from './set-connection-details/set-connection-details.component';
import {MatSelectModule} from '@angular/material/select';
import { MatFormFieldModule} from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DetailsComponent } from './details/details.component';
import {MatTabsModule} from '@angular/material/tabs';
import { SettingsComponent } from './settings/settings.component';
import { GraphComponent } from './graph/graph.component';
import { ManualComponent } from './manual/manual.component';
import { LEDComponent } from './led/led.component';
import { ButtonComponent } from './button/button.component';
import { EEPROMComponent } from './eeprom/eeprom.component';
import { CurrentComponent } from './current/current.component';
import { StepperMotorComponent } from './stepper-motor/stepper-motor.component';
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatButtonModule} from '@angular/material/button';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatSliderModule} from '@angular/material/slider';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import {MatSnackBarModule} from '@angular/material/snack-bar';

@NgModule({
  declarations: [
    AppComponent,
    BodyComponent,
    HeaderComponent,
    SetConnectionDetailsComponent,
    DetailsComponent,
    SettingsComponent,
    GraphComponent,
    ManualComponent,
    LEDComponent,
    ButtonComponent,
    EEPROMComponent,
    CurrentComponent,
    StepperMotorComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    FormsModule, 
    ReactiveFormsModule,
    HttpClientModule,
    MatTabsModule,
    CanvasJSAngularChartsModule,
    MatTooltipModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatSliderModule,
    MatSnackBarModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
