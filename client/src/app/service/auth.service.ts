import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface User {
  username: string;
  token?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;

  constructor(private http: HttpClient) {
    const token: string | null = localStorage.getItem('userToken');
    const username: string | null = localStorage.getItem('username');
    if(token) {
      const user = {
        username: username,
        token: token
      } as User
      this.currentUserSubject = new BehaviorSubject<User>(user);
    }
    else {
      this.currentUserSubject = new BehaviorSubject<User>({} as User)
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
        .pipe(map(token => {
            localStorage.setItem('userToken', JSON.stringify(token));
            localStorage.setItem('username', JSON.stringify(username));
            const user = {
              username: username,
              token: token
            } as User
            this.currentUserSubject.next(user);
            return token;
        }));
  }

  // tslint:disable-next-line: typedef
  logout() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('username');
    this.currentUserSubject.next({} as User);
  }
}
