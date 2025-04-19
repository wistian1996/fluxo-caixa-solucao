import { Injectable } from "@angular/core";
import {
  HttpInterceptor,
  HttpEvent,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
} from "@angular/common/http";
import { Router } from "@angular/router";
import { filter, switchMap, take, tap } from "rxjs/operators";
import { BehaviorSubject, from, Observable } from "rxjs";
import { throwError, of } from "rxjs";
import { AuthService } from "../service/auth.service";
import { CustomSnackBar } from "./snack-bar";
import { UserService } from "../service/user.service";
import { IErrorResponse } from "../models/error-response.interface";
import { ILoginResponse } from "../models/login-response.interface";


@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  constructor(
    private router: Router,
    private authService: AuthService,
    private snackBar: CustomSnackBar,
    private userService: UserService
  ) { }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this.authService.getLoginResponse()?.accessToken;

    if (token) {
      req = this.addToken(req, token);
    }

    return next.handle(req).pipe(
      tap(
        () => { },
        (err: any) => {
          console.log("ERROR" + err.status);
          if (err.status === 401) {
            this.snackBar.warning(
              "Falha de autenticação, efetue o login para continuar a navegar"
            );
            this.authService.logout();
            if (err instanceof HttpErrorResponse) {
              try {
                const er = err.error as IErrorResponse;
                if (er.errors?.length > 0) {
                  this.snackBar.danger(er.errors[0]);
                  return err.status;
                } else {
                  return 0;
                }
              } catch (e) {
                console.log(e);
                return 0;
              }
            }
          }
          if (err.status === 403) {
            this.snackBar.danger("Sem permissão para realizar esta requisição");
          }
          return throwError(err);
        }
      )
    );
  }

  // private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
  //   if (!this.isRefreshing) {
  //     this.isRefreshing = true;
  //     this.refreshTokenSubject.next(null);

  //     const em = this.userService.getCachedUser()!.email;
  //     const refreshT = this.authService.getLoginResponse().refreshToken;

  //     const refresh: IRefreshToken = {
  //       email: em,
  //       refreshToken: refreshT,
  //     };

  //     return this.authService.refreshToken(refresh).pipe(
  //       switchMap((token: ILoginResponse) => {
  //         this.authService.addLoginResponse(token);
  //         this.isRefreshing = false;
  //         this.refreshTokenSubject.next(token.accessToken);
  //         return next.handle(this.addToken(request, token.accessToken));
  //       })
  //     );
  //   } else {
  //     return this.refreshTokenSubject.pipe(
  //       filter((token) => token != null),
  //       take(1),
  //       switchMap((accessToken) => {
  //         return next.handle(this.addToken(request, accessToken));
  //       })
  //     );
  //   }
  // }

  private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        Authorization: `bearer ${token}`
      },
    });
  }
}
