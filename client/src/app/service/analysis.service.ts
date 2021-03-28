import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AnalysisService {

  constructor(
    private http: HttpClient,
    private authService: AuthService,
  ) { }

  getAnalyzes() {
      this.http.get(`/api/analysis/all`)
        .subscribe((response) => {
          console.log(response);
        })
    
  }
}
