import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UploadFileType } from '../../shared/upload-file/upload-file.component';

interface UploadedFile {
  date: string;
  fileName: string;
  status: string;
  isSelected: boolean;
}

@Component({
  selector: 'app-new-analysis',
  templateUrl: './new-analysis.component.html',
  styleUrls: ['./new-analysis.component.scss']
})
export class NewAnalysisComponent implements OnInit {
  uploadedFiles: UploadedFile[] = [];
  selectedRow: UploadedFile | null = null;
  uploadFileType = UploadFileType.ANALYSIS;

  constructor(
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.createMockupData();
  }

  createMockupData() {
    const file1 = {
      date: '01-03-2021',
      fileName: 'test1.py',
      status: 'ready',
      isSelected: false
    } as UploadedFile
    const file2 = {
      date: '25-03-2021',
      fileName: 'test2.py',
      status: 'confirm',
      isSelected: false
    } as UploadedFile
    const file3 = {
      date: '30-03-2021',
      fileName: 'program.py',
      status: 'pending',
      isSelected: false
    } as UploadedFile
    this.uploadedFiles.push(file1, file2, file3);
  }

  toggleRow(row: UploadedFile) {
    if(this.selectedRow === row) {
      this.selectedRow = null;
    }
    else {
      this.selectedRow = row;
    }
  }


  isFileUploadedHandler(message: boolean) {
    if(message) {
      this.toastr.success('File uploaded');
    }
    else {
      this.toastr.error('File uploading error');
    }
  }
}
