import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SetConnectionDetailsComponent } from './set-connection-details/set-connection-details.component';
import { DetailsComponent } from './details/details.component';
import { GraphComponent } from './graph/graph.component';
import { connectedGuard } from './guards/connected.guard';
import { settingsResolverResolver } from './resolvers/settings-resolver.resolver';
import { homeGuard } from './guards/home.guard';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

const routes: Routes = [
  {path:'',component:SetConnectionDetailsComponent},
  {path:'details',component:DetailsComponent,canActivate:[connectedGuard]},
  {path:'graph',component:GraphComponent},
  {path:'**',component:PageNotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
