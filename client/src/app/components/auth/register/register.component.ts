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
  isLoading = false;

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

  private arePasswordsEqual(): boolean {
    return this.registerForm.get('password')?.value === this.registerForm.get('confirmPassword')?.value
  }

  onSubmit(): void {
    if(!this.arePasswordsEqual()) {
      this.registerForm.setErrors({'passwords-not-equal': true});
      return;
    }
    this.isLoading = true;

    this.authService.register(
      this.registerForm.controls['username'].value, 
      this.registerForm.controls['password'].value
      )
      .pipe(first())
      .subscribe(
          data => {
            this.redirectToLogin();
          },
          error => {
            if(error.status === 500) {
              this.isLoading = false;
              this.registerForm.setErrors({'user-exists': true});
            }
            else {
              this.isLoading = false;
              this.registerForm.setErrors({'invalid-credentials': true});
            }
          });
  }

  private redirectToLogin(): void {
    this.redirect = true
    setTimeout(()=>{ 
      this.isLoading = false;
      this.router.navigate(['/login']);
 }, 3000);
  }
}
