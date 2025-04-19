import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { IErrorResponse } from "../models/error-response.interface";

@Injectable({
  providedIn: "root",
})
export class CustomSnackBar {
  constructor(private readonly snackBar: MatSnackBar) {}

  primary(title: string) {
    this.snackBar.open(title, "", {
      panelClass: ["primary-snackbar"],
      verticalPosition: "top",
      horizontalPosition: "right",
      duration: 5000,
    });
  }

  success(title: string) {
    this.snackBar.open(title, "", {
      panelClass: ["success-snackbar"],
      verticalPosition: "top",
      horizontalPosition: "right",
      duration: 5000,
    });
  }

  warning(title: string) {
    this.snackBar.open(title, "", {
      panelClass: ["warning-snackbar"],
      verticalPosition: "top",
      horizontalPosition: "right",
      duration: 5000,
    });
  }

  danger(title: string) {
    this.snackBar.open(title, "", {
      panelClass: ["danger-snackbar"],
      verticalPosition: "top",
      horizontalPosition: "right",
      duration: 5000,
    });
  }

  gray(title: string) {
    this.snackBar.open(title, "", {
      panelClass: ["gray-snackbar"],
      verticalPosition: "top",
      horizontalPosition: "right",
      duration: 5000,
    });
  }

  genericErrorStatusMessage(err: HttpErrorResponse): number {
    try {
      switch (err.status) {
        case 404:
          this.danger("Falha na solicitação, erro 404");
          return err.status;
        case 500:
          this.danger("Falha na solicitação, erro 500");
          return err.status;
        case 400:
          try {
            const er = err.error as IErrorResponse;
            if (er.errors?.length > 0) {
              this.primary(er.errors[0]);
              return err.status;
            } else {
              return 0;
            }
          } catch (e) {
            console.log(e);
            return 0;
          }
        default:
          return 0;
      }
    } catch (e) {
      return 0;
    }
  }
}
