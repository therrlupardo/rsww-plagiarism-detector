import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface SourceObject {
  id: string;
  fileName: string;
  status: string;
  date: Date;
  userId: string;
}

@Injectable({
  providedIn: 'root'
})
export class SourceService {

  constructor(
    private http: HttpClient
  ) { }

  getSources(): Observable<SourceObject[]> {
    return this.http.get<SourceObject[]>(`/api/sources/all`)
  }
  
  getSource(id: string): Observable<SourceObject> {
    return this.http.get<SourceObject>(`/api/sources/${id}`)
  }

  uploadSource(file: File): Observable<SourceObject> {
    const formData = new FormData()
    formData.append('file', file)
    return this.http.post<SourceObject>(`/api/sources/create`, formData)
  }
}