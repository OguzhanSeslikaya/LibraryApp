import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpClientService {

  private url : string = "https://localhost:7227/api"
  constructor(private httpClient:HttpClient) { }

  tokenOlustur(){
    try{
    return (localStorage.getItem("AccessToken")?localStorage.getItem("AccessToken"):"");}
    catch{
    return "";
  }
  }

   linkOlustur(linkDevam:string):string{
    return `${this.url}${linkDevam}`;
  }

  get<T>(linkDevam:string):Observable<T>{
    var yeniUrl = this.linkOlustur(linkDevam);
    return this.httpClient.get<T>(yeniUrl,{headers:{"Authorization":"Bearer " + this.tokenOlustur()}});
  }

  post<T>(linkDevam:string,body:any):Observable<T>{
    var yeniUrl = this.linkOlustur(linkDevam);
    return this.httpClient.post<T>(yeniUrl,body,{headers:{"Authorization":"Bearer " + this.tokenOlustur()}});
  }

  update<T>(linkDevam:string,body:any){
    var yeniUrl = this.linkOlustur(linkDevam);
    return this.httpClient.put(yeniUrl,{body:body,headers:{"Authorization":"Bearer " + this.tokenOlustur()}})
  }

  delete<T>(linkDevam:string,body:any){
    var yeniUrl = this.linkOlustur(linkDevam);
    return this.httpClient.delete(yeniUrl,{body:body,headers:{"Authorization":"Bearer " + this.tokenOlustur()}})
  }
}
