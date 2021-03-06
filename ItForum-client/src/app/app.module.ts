import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {
  MdDialogModule, MdSnackBarModule, MdProgressSpinnerModule, MdInputModule,
  MdAutocompleteModule, MdChipsModule
} from '@angular/material';

import {Ng2PageScrollModule} from 'ng2-page-scroll';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {JwtHelper} from 'angular2-jwt';
import {NgxDatatableModule} from '@swimlane/ngx-datatable';
import {FroalaEditorModule, FroalaViewModule} from 'angular2-froala-wysiwyg';

import {AppRoutingModule} from '../modules/app-routing/app-routing.module';
import {AuthHttpModule} from '../modules/auth-http/auth-http.module';

import {AlertService} from '../services/alert/alert.service';
import {ConstantValuesService} from '../services/constantValues/constant-values.service';
import {RequestService} from '../services/request/request.service';

import {AppComponent} from './app.component';
import {HomeComponent} from './home/home.component';
import {RegisterComponent} from './register/register.component';
import {AlertComponent} from './alert/alert.component';
import {LoginComponent} from './login/login.component';
import {UserService} from '../services/user/user.service';
import {ProfileComponent} from './profile/profile.component';
import {ManageUserComponent} from './manage-user/manage-user.component';
import {ManageTagComponent} from './manage-tag/manage-tag.component';
import {VerifyPostComponent} from './verify-post/verify-post.component';
import {AdminService} from "../services/admin/admin.service";
import {AuthRequestService} from "../services/authRequest/auth-request.service";
import {AddTagComponent} from './add-tag/add-tag.component';
import {TagService} from "../services/tag/tag.service";
import {TagComponent} from './tag/tag.component';
import {TagDetailComponent} from './tag-detail/tag-detail.component';
import {TopicService} from "../services/topic/topic.service";
import {AddPostComponent} from './add-post/add-post.component';
import {ContainerDetailComponent} from './container-detail/container-detail.component';
import {RoleService} from "../services/role/role.service";
import { TopicDetailComponent } from './topic-detail/topic-detail.component';
import { TopicComponent } from './topic/topic.component';
import { PostComponent } from './post/post.component';
import { CommentComponent } from './comment/comment.component';
import {PostService} from "../services/post/post.service";
import { ReplyComponent } from './reply/reply.component';
import {CommentService} from "services/comment/comment.service";


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegisterComponent,
    AlertComponent,
    LoginComponent,
    ProfileComponent,
    TagComponent,
    ManageUserComponent,
    ManageTagComponent,
    VerifyPostComponent,
    AddTagComponent,
    TagDetailComponent,
    AddPostComponent,
    ContainerDetailComponent,
    TopicDetailComponent,
    TopicComponent,
    PostComponent,
    CommentComponent,
    ReplyComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AuthHttpModule,
    HttpModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    ReactiveFormsModule,

    //@angular/material
    BrowserAnimationsModule,
    MdDialogModule,
    MdSnackBarModule,
    MdProgressSpinnerModule,
    MdInputModule,
    MdAutocompleteModule,
    MdChipsModule,

    Ng2PageScrollModule.forRoot(),
    NgxDatatableModule,

    FroalaEditorModule.forRoot(),
    FroalaViewModule.forRoot(),

  ],
  providers: [
    JwtHelper,
    AlertService,
    ConstantValuesService,
    RequestService,
    AuthRequestService,
    UserService,
    AdminService,
    TagService,
    TopicService,
    RoleService,
    PostService,
    CommentService
  ],
  bootstrap: [AppComponent],
  entryComponents: [AlertComponent,ReplyComponent]
})
export class AppModule {
}
