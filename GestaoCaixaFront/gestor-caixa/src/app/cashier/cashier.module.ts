import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MaterialModule } from "../app.module";
import { PagesRoutes } from "./cashier.routing";
import { CashierComponent } from "./cashier.component";
import { CurrencyMaskModule } from "ng2-currency-mask";
import { MatNativeDateModule } from "@angular/material/core";

@NgModule({
  imports: [
    CommonModule,
    CurrencyMaskModule,
    RouterModule.forChild(PagesRoutes),
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatNativeDateModule
  ],
  declarations: [CashierComponent],
})
export class CashierModule {}
