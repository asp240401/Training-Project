
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { SensorData } from '../models/SensorData';
import { PortService } from '../services/port.service';
import { SensorDataService } from '../services/sensor-data.service';
import * as signalR from "@microsoft/signalr"
import { DatabaseService } from '../services/database.service';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.scss']
})

///<summary>
///   Component used to render graph showing real-time current sensor values
///</summary>
export class GraphComponent implements OnDestroy{
  
  //Indicates if the graph rendering is currently active
  start=true

  /// <summary>
  ///   Constructor injecting necessary services to the component and initializing connections.
  /// </summary>
  /// <param name="http">HttpClient for HTTP requests</param>
  /// <param name="sensorDataService">Service for sensor data</param>
  /// <param name="dbService">Service for database operations</param>
  constructor(private http : HttpClient,private sensorDataService:SensorDataService,private dbService:DatabaseService) {  

    this.sensorDataService.startConnection();
    this.addTransferChartDataListener();   
  }

  x=0
  
  // Counter for data points in the graph
  i=1

  /// <summary>
  ///   Adds a listener to handle real-time chart data received from the sensor.
  /// </summary>
  public addTransferChartDataListener = () => {
      this.sensorDataService.hubConnection.on('transferreplydata', (data) => {
        if(data.y>this.hthr || data.y<this.lthr)
        {
          data.color="red"
          data.lineColor="red"
          this.emitter.emit("yes")
        }  
        else
        { 
          data.color="green"
          data.lineColor="green"
          this.emitter.emit("no")
        }
        
        if(this.i%(this.interval/1000)==0)
        {
          this.i=1
          data.x=++this.x
          this.dataPoints.push(data);
        }
        else
          this.i++

        if(this.dataPoints.length>10)
          this.dataPoints.shift()
        console.log(data);
        
      });
  }
  
  dataPoints:any[] = [];
  @Input() interval=1000
  @Input() lthr=1
  @Input() hthr=2
  
  @Output() emitter=new EventEmitter<string>();

  timeout:any = null;
  xValue:number = 1;
  yValue:number = 10;
  newDataCount:number = 10;
  chart: any;
 
  chartOptions = {
    backgroundColor: "rgba(255, 255, 255, 0.295)",
    title: {
      text: "Current Values(mA)",
      fontSize:25,
      fontWeight:"normal",
      fontFamily:"tahoma"
    },
    axisX:{
      title: "timeline",
      gridThickness: 2
  },
  axisY:{
    minimum: 0,
    maximum: 5
   },
    data: [{
      type: "line",
      color:"green",
      lineColor:"green",
      dataPoints: this.dataPoints
    }]
  }
 
  /// <summary>
  ///   Gets the instance of the chart and triggers an update upon getting the chart object.
  /// </summary>
  /// <param name="chart">The chart object obtained from the chart component.</param>
  getChartInstance(chart: object) {
    this.chart = chart;

    this.updateData()
  }
 
  /// <summary>
  ///   Implements the OnDestroy method to handle cleanup when the component is destroyed.
  ///   Clears timeout to stop updating the graph.
  /// </summary>
  ngOnDestroy() {
    clearTimeout(this.timeout);
  }
 
  /// <summary>
  /// Renders the chart and starts updating data with specified intervals.
  /// </summary>
  updateData = () => {
    this.addData()
  }
 
  /// <summary>
  /// Initiates the rendering of the chart and sets up the data update cycle.
  /// </summary>
  addData= () =>{
   
    this.chart.render();
    this.timeout = setTimeout(this.updateData,this.interval);
  }

  /// <summary>
  /// Toggles the graph rendering on or off based on button click.
  /// </summary>
  controlGraph()
  {
    if(this.start==true)
    {
      this.start=false
      this.sensorDataService.hubConnection.stop()
    }
    else
    {
      this.start=true
      this.sensorDataService.hubConnection.start()
    }
  }
}
