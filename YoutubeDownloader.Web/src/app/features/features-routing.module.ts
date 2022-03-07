import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "@app/core/guards/auth.guard";
import { DownloadHistoryComponent } from "./download-history/download-history.component";

const routes: Routes = [
  {
    path: "history",
    component: DownloadHistoryComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FeaturesRoutingModule {}
