import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../auth-style.scss']
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  redirect = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required],
        confirmPassword: ['', Validators.required]
    });
  }

  onSubmit() {
    this.authService.register(
      this.registerForm.controls['username'].value, 
      this.registerForm.controls['password'].value
      )
      .pipe(first())
      .subscribe(
          data => {
            console.log(`Hello ${data.login}`);
            this.redirectToLogin();
          },
          error => {
            if(error.status === 500) {
              this.registerForm.setErrors({'user-exists': true});
            }
            else {
              this.registerForm.setErrors({'invalid-credentials': true});
            }
          });
  }

  private redirectToLogin(): void {
    this.redirect = true
    setTimeout(()=>{ 
      this.router.navigate(['/login']);
 }, 3000);
  }
}
