import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface AnalysisObject {
  documentId: string;
  lastChangeDate: Date;
  documentName:string;
  operationStatus: string;
}

export interface AnalysisFile {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class AnalysisService {

  constructor(
    private http: HttpClient
  ) { }

  getAnalyzes(): Observable<AnalysisObject[]> {
    return this.http.get<AnalysisObject[]>(`/api/analysis/all`)
  }

  getAnalysis(id: string): Observable<any> {
    return this.http.get(`/api/analysis/${id}`)
  }

  getAnalysisReport(id: string): Observable<Blob> {
    return this.http.get(`/api/analysis/${id}/report`, {responseType: 'blob'})
  }

  getFilesToAnalysis(): Observable<AnalysisFile[]> {
    return this.http.get<AnalysisFile[]>(`/api/documentsToAnalysis/all`)
  }

  uploadAnalysisFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file)
    return this.http.post(`/api/analysis/send`, formData)
  }

  startAnalysis(fileId: string): Observable<any> {
    return this.http.post(`/api/analysis/${fileId}/start`, {})
  }
}
