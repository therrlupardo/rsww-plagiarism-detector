import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SourceObject, SourceService } from 'src/app/service/source.service';



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
        this.isLoading = true;
      }
    )
  }
}
