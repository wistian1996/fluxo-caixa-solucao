import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ILoginResponse } from "../models/login-response.interface";
import { ILoginRequest } from "../models/login-request.interface";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) { }

  storageToken = "loginResponse";

  public addLoginResponse(response: ILoginResponse) {
    localStorage.setItem(this.storageToken, JSON.stringify(response));
  }

  public getLoginResponse(): ILoginResponse | null {
    try {
      const loginResponse: ILoginResponse = JSON.parse(
        localStorage.getItem(this.storageToken)!
      ) as ILoginResponse;
      return loginResponse;
    } catch (e) {
      return null;
    }
  }

  login(loginRequest: ILoginRequest): Observable<ILoginResponse> {
    const url = `${environment.apiUrl}/identity-server/auth/token`;
    //const url = 'https://run.mocky.io/v3/b4aa832a-68c2-4f2a-8f2a-85bc41a7d6d9';
    return this.http.post<ILoginResponse>(url, loginRequest);
  }

  public async logout() {
    localStorage.removeItem(this.storageToken);
    this.router.navigate(["/login"]);
  }

  public async isAuthenticated(redirect?: boolean): Promise<boolean> {
    const token = this.getLoginResponse();
    const isToken = token?.accessToken != null;

    if (!isToken && redirect) {
      this.logout();
    }
    return Promise.resolve(isToken);
  }

}
