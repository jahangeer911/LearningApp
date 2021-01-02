import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery';
import { Users } from 'src/app/_models/users';
import { UsersService } from 'src/app/_services/users.service';
declare let alertify:any;
@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  user:Users;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private userservice:UsersService,private route:ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data=>{
      this.user=data['user'];   
    });
    this.galleryOptions=[
      {
        width:'500px',
        height:'500px',
        imagePercent:100,
        thumbnailsColumns:4,
        imageAnimation:NgxGalleryAnimation.Slide,
        preview:false
      }
    ];
    this.galleryImages= this.getImages();
    //this.getUserDetails();
  }
  getImages(){
    const imagesUrls=[];
    for(let i = 0;i<this.user.photos.length;i++){
      imagesUrls.push({
        small:this.user.photos[i].url,
        medium:this.user.photos[i].url,
        big:this.user.photos[i].url,
        description:this.user.photos[i].url
      })
    }
    return imagesUrls;
  }
  getUserDetails(){
    this.userservice.getUserbyID(+this.route.snapshot.params['id']).subscribe((returneduser:Users)=>{
      this.user=returneduser;
    },error=>{
      alertify.error('error');
    })
  }

}
