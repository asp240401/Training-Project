import { Injectable } from "@angular/core"

@Injectable({
    providedIn: 'root'
})

export class Commands{
    
    green_LED_ON:string="SETLED ON 1"
    green_LED_OFF:string="SETLED OFF 1"

    red_LED_ON:string="SETLED ON 3"
    red_LED_OFF:string="SETLED OFF 3"

    blue_LED_ON:string="SETLED ON 2"
    blue_LED_OFF:string="SETLED OFF 2"

    eeprom_test:string="ERMTEST"

    get_ADC:string="GETADC"

    motor_rotate:string="SETMOT ROT "
    motor_step:string="SETMOT STP  "

    get_button_status="GETBUTTON"
}