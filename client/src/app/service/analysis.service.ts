import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface AnalysisObject {
  id: string;
  date: Date;
  fileName:string;
  result: number;
  status: string;
  userId: string;
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

  getAnalysisReport(id: string): Observable<any> {
    return this.http.get(`/api/analysis/${id}/report`)
  }
}
