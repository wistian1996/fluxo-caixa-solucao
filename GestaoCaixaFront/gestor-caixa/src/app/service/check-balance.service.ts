import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { ICheckBalance } from "../models/check-balance.interface";

@Injectable({
  providedIn: "root",
})
export class CheckBalanceService {
  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) { }

  getBalance(): Observable<ICheckBalance> {
    const url = environment.apiUrl + `/api/v1/saldo`;
    return this.http.get<ICheckBalance>(url);
  }

}
