import { Observable, map } from "rxjs";
import { HttpClient, HttpParams, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { IUser } from "../models/user.interface";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) { }

  storageUser = "user";

  userProfile(): Observable<IUser> {
    const url = environment.apiUrl + "/user/profile";
    console.log(url);
    return this.http.get<IUser>(url);
  }

  public deleteUserSession() {
    localStorage.removeItem(this.storageUser);
  }

  public addUserSession(response: IUser) {
    localStorage.setItem(this.storageUser, JSON.stringify(response));
  }

  public getCachedUser(): IUser | null {
    try {
      var userString = localStorage.getItem(this.storageUser);
      if(userString == null) return null;
      const user: IUser = JSON.parse(
        userString
      ) as IUser;
      return user;
    } catch (e) {
      return null;
    }
  }

}
