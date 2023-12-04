/// <summary>
/// Represents the configuration details of a serial port.
/// </summary>
export class Port
{
    PortName:string="COM6"
    BaudRate:number=9600
    DataBits:number=8
    Handshake:string="None"
    StopBits:string="One"
    Parity:string="None"

    /// <summary>
    /// Constructor to initialize the serial port configuration.
    /// </summary>
    /// <param name="ptname">Name of the port</param>
    /// <param name="bdrate">Baud rate</param>
    /// <param name="dtbits">Data bits</param>
    /// <param name="hshake">Handshake setting</param>
    /// <param name="stbits">Stop bits</param>
    /// <param name="par">Parity setting</param>
    constructor(ptname:string,bdrate:number,dtbits:number,hshake:string,stbits:string,par:string)
    {
        this.PortName=ptname
        this.BaudRate=bdrate
        this.DataBits=dtbits
        this.Handshake=hshake
        this.StopBits=stbits
        this.Parity=par
    }
}