import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit, ElementRef, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { lastValueFrom, timer } from "rxjs";
import { AuthService } from "../service/auth.service";
import { UserService } from "../service/user.service";
import { CustomSnackBar } from "../shared/snack-bar";
import { CashierService } from "../service/cashier.service";
import { IOperation } from "../models/operation.interface";
import { CheckBalanceService } from "../service/check-balance.service";

@Component({
  selector: "app-login-cmp",
  standalone: false,
  templateUrl: "./cashier.component.html",
})
export class CashierComponent implements OnInit, OnDestroy {
  public value?: number;
  public totalBalance?: number;
  public lastRefresh?: Date;
  public lastSuccessRefresh?: Date;
  public loop: boolean = false;
  public loading?: boolean;
  constructor(
    private readonly authService: AuthService,
    private readonly userService: UserService,
    private readonly cashierService: CashierService,
    private readonly checkBalanceService: CheckBalanceService,
    private readonly router: Router,
    private readonly snackBar: CustomSnackBar
  ) { }


  ngOnDestroy(): void {
    this.loop = false;
  }

  ngOnInit() {
    this.observableTimer();
    this.loop = true;
  }

  async inputCash() {
    try {
      this.loading = true;
      if (this.value == 0 || this.value == null) {
        this.snackBar.danger('Valor precisa ser maior que 0');
        return;
      }
      const operation: IOperation = {
        isCredito: true,
        valor: this.value!
      }
      var requestObs = this.cashierService.transaction(operation);
      await lastValueFrom(requestObs);
      this.value = 0;
      this.snackBar.success('Entrada realizada');
    }
    catch (error) {
      this.snackBar.danger('Falha na transação');
      console.log("ERROR", error);
    } finally {
      this.loading = false;
    }

  }

  observableTimer() {
    setInterval(async () => {
      if(this.loop){
        this.lastRefresh = new Date();
        try {
          var requestObs = this.checkBalanceService.getBalance();
          const balance = await lastValueFrom(requestObs);
          this.totalBalance = balance.saldo;
          this.lastSuccessRefresh = new Date();
        }
        catch (error) {
          console.log("ERROR", error);
        } 
      }
    }, 5000)
  }


  async outputCash() {

    try {
      this.loading = true;
      if (this.value == 0 || this.value == null) {
        this.snackBar.danger('Valor precisa ser maior que 0');
        return;
      }
      const operation: IOperation = {
        isCredito: false,
        valor: this.value!
      }
      var requestObs = this.cashierService.transaction(operation);
      await lastValueFrom(requestObs);
      this.value = 0;
      this.snackBar.success('Saída realizada');
    }
    catch (error) {
      this.snackBar.danger('Falha na transação');
      console.log("ERROR", error);
    } finally {
      this.loading = false;
    }

  }

  logout() {
    this.authService.logout();
    this.snackBar.primary('Usuário desconectado');
  }
}
