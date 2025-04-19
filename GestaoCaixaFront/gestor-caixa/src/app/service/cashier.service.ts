import { Observable, map } from "rxjs";
import { HttpClient, HttpParams, HttpResponse } from "@angular/common/http";
import { Injectable, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { IUser } from "../models/user.interface";
import { IOperation } from "../models/operation.interface";

@Injectable({
  providedIn: "root",
})
export class CashierService  {
  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) { }


  transaction(operation: IOperation): Observable<IOperation> {
    const url = environment.apiUrl + `/api/v1/lancamentos`;
    return this.http.post<IOperation>(url, operation);
  }

}
