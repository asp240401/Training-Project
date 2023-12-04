import { CanActivateFn } from '@angular/router';

/// <summary>
///   This guard function verifies the connection status by checking a localStorage
///   value ('connected'). If the value is 'yes', the route is allowed; otherwise,
///   an alert is shown indicating a failed connection, and the route is not activated.
///   Does not allow user to navigate to the dashboard if connection with the device fails.
/// </summary>
export const connectedGuard: CanActivateFn = (route, state) => {
  var status=localStorage.getItem('connected')
  if(status=="yes")
    return true;

  alert("connection failed")
  return false;
};
