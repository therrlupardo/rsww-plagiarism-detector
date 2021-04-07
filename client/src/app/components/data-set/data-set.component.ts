import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/service/auth.service';
import { SourceObject, SourceService } from 'src/app/service/source.service';

const DB_ERROR = 'Database connection error';


@Component({
  selector: 'app-data-set',
  templateUrl: './data-set.component.html',
  styleUrls: ['./data-set.component.scss']
})
export class DataSetComponent implements OnInit {
  sourcesData: SourceObject[] = [];
  isLoading = true;

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


  logout() {
    this.authService.logout(); 
    this.router.navigate(['/']);
  }
}
