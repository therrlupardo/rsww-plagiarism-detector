import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AnalysisFile, AnalysisService } from 'src/app/service/analysis.service';
import { AuthService } from 'src/app/service/auth.service';
import { UploadFileType } from '../../shared/upload-file/upload-file.component';

@Component({
  selector: 'app-new-analysis',
  templateUrl: './new-analysis.component.html',
  styleUrls: ['./new-analysis.component.scss']
})
export class NewAnalysisComponent implements OnInit {
  uploadedFiles: AnalysisFile[] = [];
  selectedRow: AnalysisFile | null = null;
  uploadFileType = UploadFileType.ANALYSIS;
  isLoading = false;
  startButtonDisabled = true;

  constructor(
    private toastr: ToastrService,
    private analysisService: AnalysisService,
    private authService: AuthService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.isLoading = true;
    this.analysisService.getFilesToAnalysis().subscribe(
      (analysisFiles) => {
        this.uploadedFiles = analysisFiles;
        this.isLoading = false;
      },
      (error) => {
        this.isLoading = false;
        if(error.status !== 500) {
          this.toastr.error('Database connection error', 'Error');
          this.isLoading = true;
          this.logout();
        }
      })
  }

  startAnalysis() {
    this.startButtonDisabled = true;
    this.analysisService.startAnalysis(this.selectedRow?.id!).subscribe(
      (taskId) => {
        console.log(taskId);
      },
      (error) => {
        this.toastr.error('Analysis error', 'Error');
        this.startButtonDisabled = false;
      }
    )
  }

  toggleRow(row: AnalysisFile) {
    if(this.selectedRow === row) {
      this.selectedRow = null;
      this.startButtonDisabled = true;
    }
    else {
      this.selectedRow = row;
      this.startButtonDisabled = false;
    }
  }


  isFileUploadedHandler(message: boolean) {
    if(message) {
      this.toastr.success('File uploaded');
      this.loadData();
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
