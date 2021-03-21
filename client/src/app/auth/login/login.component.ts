import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../auth-style.scss']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder
  ) { 
    if (this.authService.currentUserValue.authProps) { 
        this.router.navigate(['/dashboard']);
    }
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
    });
  }

  onSubmit() {    
    this.authService.login(
      this.loginForm.controls['username'].value, 
      this.loginForm.controls['password'].value
      )
      .pipe(first())
      .subscribe(
          data => {
            console.log(`Hello ${this.authService.currentUserValue.username}`);
            this.router.navigate(['/dashboard']);
          },
          error => {
            console.log('login error');
            this.loginForm.setErrors({'invalid-credentials': true});
          });

  }

}
