import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/service/auth.service';
import { SourceObject, SourceService } from 'src/app/service/source.service';
import { UploadFileType } from '../shared/upload-file/upload-file.component';

const DB_ERROR = 'Database connection error';


@Component({
  selector: 'app-data-set',
  templateUrl: './data-set.component.html',
  styleUrls: ['./data-set.component.scss']
})
export class DataSetComponent implements OnInit {
  sourcesData: SourceObject[] = [];
  isLoading = true;
  uploadFileType = UploadFileType.DATASET

  constructor(
    private sourceService: SourceService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.isLoading = true;
    this.sourceService.getSources().subscribe(
      (sourceObjects) => {
        this.sourcesData = sourceObjects;
        this.isLoading = false;
      },
      (error) => {
        this.toastr.error(DB_ERROR, 'Error');
        this.isLoading = true;
        this.logout();
      }
    )
  }

  isFileUploadedHandler(message: boolean) {
    if(message) {
      this.loadData();
      this.toastr.success('File uploaded');
    }
    else {
      this.toastr.error('File uploading error');
    }
  }

  logout() {
    this.authService.logout(); 
    this.router.navigate(['/']);
  }
}
