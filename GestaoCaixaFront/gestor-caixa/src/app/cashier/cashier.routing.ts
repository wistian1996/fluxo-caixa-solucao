import { Routes } from "@angular/router";
import { CashierComponent } from "./cashier.component";

export const PagesRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "",
        component: CashierComponent,
      },
    ],
  },
];
