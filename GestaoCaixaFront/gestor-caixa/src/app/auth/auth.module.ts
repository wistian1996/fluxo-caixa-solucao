import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { PagesRoutes } from "./auth.routing";
import { MaterialModule } from "../app.module";
import { LoginComponent } from "./login.component";

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(PagesRoutes),
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
  ],
  declarations: [LoginComponent],
})
export class AuthModule {}
