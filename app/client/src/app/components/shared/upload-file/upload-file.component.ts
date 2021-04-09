import { Component, Input, OnInit } from '@angular/core';
import { AnalysisService } from 'src/app/service/analysis.service';
import { SourceService } from 'src/app/service/source.service';

export enum UploadFileType {
  ANALYSIS,
  DATASET
}

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {
  @Input() type: UploadFileType = UploadFileType.ANALYSIS
  title: string = ''
  loading: boolean = false;
  file: File | null = null; 

  constructor(
    private analysisService: AnalysisService,
    private sourceService: SourceService,
    ) { }

  ngOnInit(): void {
    if(this.type === UploadFileType.ANALYSIS) {
      this.title = 'Upload file to analysis';
    }
    else if(this.type === UploadFileType.DATASET) {
      this.title = 'Upload new Data set';
    }
  }

  onChange(event: any) {
      this.file = event.target.files[0];
  }

  onUpload() {
    if(this.file) {
      this.loading = true;
      if(this.type === UploadFileType.ANALYSIS) {
        this.uploadAnalysis();
      }
      else if(this.type === UploadFileType.DATASET) {
        this.uploadDataSet();
      }
    }
  }

  private uploadAnalysis() {
    this.analysisService.uploadFile(this.file!).subscribe(
      data => {
        this.loading = false;
        console.log('got')
      },
      error => {
        this.loading = false;
        console.log('error');
      } 
    );
  }

  private uploadDataSet() {
    this.sourceService.uploadSource(this.file!).subscribe(
      data => {
        this.loading = false;
        console.log('got')
      },
      error => {
        this.loading = false;
        console.log('error');
      } 
    );
  }
}