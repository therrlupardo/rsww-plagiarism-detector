import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AnalysisObject, AnalysisService } from 'src/app/service/analysis.service';
import { SourceService } from 'src/app/service/source.service';
const DB_ERROR = 'Database connection error';

@Component({
  selector: 'app-analyzes-list',
  templateUrl: './analyzes-list.component.html',
  styleUrls: ['./analyzes-list.component.scss']
})
export class AnalyzesListComponent implements OnInit {
  analysisData: AnalysisObject[] = [];
  isLoading = false;

  constructor(
    private analysisService: AnalysisService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.analysisService.getAnalyzes().subscribe(
      (analysisObject) => {
        this.analysisData = analysisObject;
        this.isLoading = false;
      },
      (error) => {
        this.toastr.error(DB_ERROR, 'Error');
        this.isLoading = true;
      })
  }

  downloadReport(id: string) {
    // this.analysisService.getAnalysisReport(id)
    //   .subscribe(item => {
    //       console.log(item)
    //     }
    //   )
  }
}
