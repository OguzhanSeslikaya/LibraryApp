import { Injectable } from '@angular/core';

declare var alertify:any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  message(message:string , messageType : MessageType,position : PositionType,delaySecond : number = 3){
    alertify.set('notifier','position', position);
    alertify.set('notifier','delay', delaySecond);
    alertify[messageType](message);
  }
  confirmBox(message:string,ok:any){
    alertify.confirm(message, () => ok() );
  }
}

export enum MessageType{
  error = "error",
  message = "message",
  notify = "norify",
  success = "success",
  warning = "warning"
}

export enum PositionType{
  topRight="top-right",
  topCenter="top-center",
  topLeft="top-left",
  bottomRight="bottom-right",
  bottomCenter="bottom-center",
  bottomLeft="bottom-left"
}
