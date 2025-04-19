import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from "@angular/router";
import { AuthService } from "../service/auth.service";
import { CustomSnackBar } from "./snack-bar";
import { UserService } from "../service/user.service";


@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly snackBar: CustomSnackBar,
    private readonly userService: UserService
  ) {}

  public async canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> {
    return this.authService
      .isAuthenticated()
      .then(async (authenticated) => {
        console.log("Auth? " + authenticated);
        if (authenticated) {
          return this.loggedRedirect(state);
        } else {
          this.router.navigate(["/login"]);
          return false;
        }
      })
      .catch((error) => {
        this.router.navigate(["/login"]);
        return false;
      });
  }

  private isLoginPage(url) {
    return /\/login/.test(url);
  }

  private loggedRedirect(state) {
    if (this.isLoginPage(state.url)) {
      const { redirectUrl } = state.root.queryParams;
      this.router.navigate([redirectUrl || "/"]);
    }
    return true;
  }

  private notLoggedRedirect(state) {
    const returnUrl = state.root.queryParams.redirectUrl;

    if (this.isLoginPage(state.url)) {
      return true;
    }
    if (returnUrl) {
      this.router.navigate(["/login"], { queryParams: { returnUrl } });
    } else {
      this.router.navigate(["/login"]);
    }
    this.snackBar.warning("Sessão expirada");
    return false;
  }
}
