import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AnalysisObject, AnalysisService } from 'src/app/service/analysis.service';
import { AuthService } from 'src/app/service/auth.service';
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
    private authService: AuthService,
    private router: Router,
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
        this.isLoading = false;
        if(error.status !== 500) {
          this.toastr.error(DB_ERROR, 'Error');
          this.isLoading = true;
          this.logout();
        }
      })
  }

  downloadReport(id: string) {
    this.analysisService.getAnalysisReport(id).subscribe(
      (data) => {
        var newBlob = new Blob([data], { type: "application/pdf" });

        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(newBlob);
            return;
        }

        const dataObject = window.URL.createObjectURL(newBlob);

        var link = document.createElement('a');
        link.href = dataObject;
        link.download = "analysis_report.pdf";
        link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

        setTimeout(function () {
            window.URL.revokeObjectURL(dataObject);
            link.remove();
        }, 100);
      },
      (error) => {
        console.log(error)
        this.toastr.error('Report downloading error');
      }
    )
  }

  logout() {
    this.authService.logout(); 
    this.router.navigate(['/']);
  }
}
