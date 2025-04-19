import { Routes } from "@angular/router";
import { AuthGuard } from "./shared/auth.guard";

export const AppRoutes: Routes = [
    {
        path: "",
        redirectTo: "cashier",
        pathMatch: "full",
    },
    {
        path: "login",
        loadChildren: () =>
            import("./auth/auth.module").then((m) => m.AuthModule)
    },
    {
        path: "cashier",
        canActivate: [AuthGuard],
        loadChildren: () =>
            import("./cashier/cashier.module").then((m) => m.CashierModule)
    }
//   {
//     path: "",
//     children: [
//       {
//         path: "auth",
//         loadChildren: () =>
//           import("./auth/auth.module").then((m) => m.AuthModule),
//       }
//     ],
//   }
];
