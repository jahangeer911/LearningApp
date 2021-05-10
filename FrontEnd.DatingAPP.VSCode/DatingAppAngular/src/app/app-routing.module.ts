import { NgModule } from '@angular/core';
import { componentRefresh } from '@angular/core/src/render3/instructions';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { PrevenetUnSaveChanges } from './prevent-unsavechanges-guard';
import { ListResolver } from './_resolvers/lists-resolver';
import { MemberDetailResolver } from './_resolvers/member-details-resolver';
import { MemberEditResolver } from './_resolvers/member-edit-resolver';
import { MemberListResolver } from './_resolvers/member-listss-resolver';

const routes: Routes = [
  {path:'home',component:HomeComponent},
  {path:'messages',component:MessagesComponent,canActivate:[AuthGuard]},
  {path:'lists',component:ListsComponent,canActivate:[AuthGuard],resolve:{users:ListResolver}},
  {path:'members',component:MemberListComponent,canActivate:[AuthGuard],resolve:{users:MemberListResolver}},
  {path:'members/:id',component:MemberDetailsComponent,canActivate:[AuthGuard],resolve:{user:MemberDetailResolver}},
  {path:'member/edit',component:MemberEditComponent,canActivate:[AuthGuard],resolve:{user:MemberEditResolver},canDeactivate:[PrevenetUnSaveChanges]},
  {path:'**',redirectTo:'home',pathMatch:'full'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
