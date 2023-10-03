import { Component, OnInit, OnDestroy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@app/core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  loginForm: UntypedFormGroup;
  loginError: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: UntypedFormBuilder
  ) { }

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
      domain: ['']
    });
  }

  ngOnDestroy() {
  }

  login() {
    this.loginError = '';

    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.authService
      .login(this.loginForm.getRawValue())
      .subscribe((response) => {
        if (response.isError) {
          this.loginError = response.error;
          return;
        }

        this.router.navigate(['/dashboard']);
      },
      (err: any) => console.log(err));
  }
}
