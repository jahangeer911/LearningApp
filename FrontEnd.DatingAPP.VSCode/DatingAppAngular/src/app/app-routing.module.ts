import { NgModule } from '@angular/core';
import { componentRefresh } from '@angular/core/src/render3/instructions';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [
  {path:'home',component:HomeComponent},
  {path:'messages',component:MessagesComponent,canActivate:[AuthGuard]},
  {path:'lists',component:ListsComponent},
  {path:'members',component:MemberListComponent},
  {path:'**',redirectTo:'home',pathMatch:'full'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
