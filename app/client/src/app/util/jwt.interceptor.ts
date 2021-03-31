import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../service/auth.service';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let currentUser = this.authService.currentUserValue;
        if (currentUser && currentUser.authProps) {
            request = request.clone({
                setHeaders: {
                    authorization: `Bearer ${currentUser.authProps.accessToken}`
                }
            });
        }

        return next.handle(request);
    }
}