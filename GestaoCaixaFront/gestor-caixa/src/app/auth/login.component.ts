import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit, ElementRef, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { lastValueFrom } from "rxjs";
import { ILoginRequest } from "../models/login-request.interface";
import { AuthService } from "../service/auth.service";
import { UserService } from "../service/user.service";
import { CustomSnackBar } from "../shared/snack-bar";
import { ILoginResponse } from "../models/login-response.interface";
import { IUser } from "../models/user.interface";

@Component({
  selector: "app-login-cmp",
  standalone: false,
  templateUrl: "./login.component.html",
})
export class LoginComponent implements OnInit {
  public loginRequest?: ILoginRequest;
  public buttonName?: string;
  public loading?: boolean;
  constructor(
    private readonly authService: AuthService,
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly snackBar: CustomSnackBar
  ) { }

  ngOnInit() {
    this.checkLogged();
    this.buttonName = "Login";
    this.loginRequest = {
      comercianteId: ""
    };
  }

  async onSubmit() {
    this.buttonName = "Aguarde...";
    this.loading = true;

     await this.login();

    this.buttonName = "Entrar";
    this.loading = false;
  }

  async login() {
    try {
      const loginObs = this.authService.login(this.loginRequest!);
      const requestResult = await lastValueFrom(loginObs);
      this.onSuccessLogin(requestResult);
      return true;
    } 
    catch (error) {
      if (error instanceof HttpErrorResponse) {
        this.onErrorLogin(error);
      }
      return false;
    }
  }

  onErrorLogin(err: HttpErrorResponse) {
    if (err?.status !== 401 && err?.status !== 403) {
      if (this.snackBar.genericErrorStatusMessage(err) === 0) {
        this.snackBar.danger("Ocorreu um erro desconhecido, tente novamente");
      }
    }
    console.log(err);
  }

  private onSuccessLogin(response: ILoginResponse) {
    console.log("Login request success");
    this.authService.addLoginResponse(response);
    this.router.navigate(["/cashier"]);
  }

  private async checkLogged() {
    if (
      await this.authService.isAuthenticated()
    ) {
      this.router.navigate(["/cashier"]);
    }
  }
}
