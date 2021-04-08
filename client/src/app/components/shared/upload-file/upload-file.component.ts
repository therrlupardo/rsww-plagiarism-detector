import { Component, Input, OnInit } from '@angular/core';
import { AnalysisService } from 'src/app/service/analysis.service';
import { SourceService } from 'src/app/service/source.service';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {
  @Input() title: string = ''
  loading: boolean = false;
  file: File | null = null; 

  constructor(private analysisService: AnalysisService) { }

  ngOnInit(): void {}

  onChange(event: any) {
      this.file = event.target.files[0];
  }

  onUpload() {
    this.loading = true;
      this.analysisService.uploadFile(this.file!).subscribe(
        data => {
          console.log('got')
        },
        error => {
          console.log('error');
        } 
      );
  }
}