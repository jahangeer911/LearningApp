import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Photos } from 'src/app/_models/photos';
import { FileUploader } from 'ng2-file-upload'; 
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UsersService } from 'src/app/_services/users.service'; 
declare let alertify:any;
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos:Photos[];
  @Output() getMemberPhotoChanged =new EventEmitter<string>();
  uploader:FileUploader;
  hasBaseDropZoneOver =false;
  hasAnotherDropZoneOver =false;
  response:string;
  baseUrl = environment.apiURL;

  constructor(private authService: AuthService,private userService: UsersService) { 
      
    this.response = '';
  }

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  } 
  setMainPhoto(photo:Photos){
    this.userService.setMainPhoto(this.authService.decodedToken.UserID,photo.id).subscribe(()=>{
      console.log('success');
      this.photos.filter(p=>p.isMain===true)[0].isMain=false;
      photo.isMain=true;
      this.getMemberPhotoChanged.emit(photo.url)
      alertify.success('Main Photo Updated');
    },error=>{
      alertify.error('unable to set main photo');
      console.log('er');
    }); 
  }
  deletePhoto(id:number){
    alertify.confirm('Are you sure you want to delete photo',()=>{
      this.userService.deletePhoto(this.authService.decodedToken.UserID,id).subscribe(()=>{
        this.photos.splice(this.photos.findIndex(p=>p.id===id),1);
        console.log('remove photos array');
        alertify.success('photo deleted');
      },error=>{
        alertify.error('unable to delete photo');
      });
    });

  }
  initializeUploader(){
    this.uploader =new FileUploader({
      url:this.baseUrl+'users/'+this.authService.decodedToken.UserID+'/photos',
      authToken:'Bearer '+localStorage.getItem('token'),
      isHTML5:true,
      allowedFileType:['image'],
      removeAfterUpload:true,
      autoUpload:false,
      maxFileSize:10*1024*1024
    });
    this.uploader.onAfterAddingFile =(file)=>{file.withCredentials=false};
    this.uploader.onSuccessItem=(item,response,status,headers)=>{
      if(response){
        const res :Photos = JSON.parse(response);
        if(res.isMain){
          this.getMemberPhotoChanged.emit(res.url)
        }
        this.photos.push(res);  
        console.log(this.photos);
      }
    }
  }
}


