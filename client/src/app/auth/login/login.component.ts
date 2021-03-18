import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
      //this.submitted = true;

      // stop here if form is invalid
      // if (this.loginForm.invalid) {
      //     return;
      // }

      // this.loading = true;
      this.authService.login('admin', 'admin')
        .pipe(first())
        .subscribe(
            data => {
              console.log('hello');
            },
            error => {
              console.log('error');
            });
  }

}
