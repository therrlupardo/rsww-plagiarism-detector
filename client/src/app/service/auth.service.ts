import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface User {
  id: number;
  username: string;
  password: string;
  token?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
    const user: string | null = localStorage.getItem('currentUser');
    if(user) {
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(user));
      this.currentUser = this.currentUserSubject.asObservable();
    }
    else {
      this.currentUserSubject = new BehaviorSubject<User>({} as User)
      this.currentUser = this.currentUserSubject.asObservable();
    }
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  // tslint:disable-next-line: typedef
  login(username: string, password: string) {
    let params = new HttpParams();
    params = params.append('login', username);
    params = params.append('password', password);

    return this.http.post<any>(`${environment.apiUrl}/api/identity/login?login=${username}&password=${password}`, params)
        .pipe(map(user => {
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
            return user;
        }));
  }

  // tslint:disable-next-line: typedef
  logout() {
      // remove user from local storage to log user out
      localStorage.removeItem('currentUser');
      this.currentUserSubject.next({} as User);
  }
}
