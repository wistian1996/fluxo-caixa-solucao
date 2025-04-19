import { Routes } from "@angular/router";
import { LoginComponent } from "./login.component";

export const PagesRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "",
        component: LoginComponent,
      },
    ],
  },
];
