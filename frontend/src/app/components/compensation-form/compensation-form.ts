import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Compensation } from '../../services/api';

@Component({
  selector: 'app-compensation-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './compensation-form.html'
})
export class CompensationFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  public dialogRef = inject(MatDialogRef<CompensationFormComponent>);
  
  public data: { employeeId: string, compensation: Compensation | null } = inject(MAT_DIALOG_DATA);

  form!: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      employeeId: [this.data.employeeId],
      taxNumber: [this.data.compensation?.taxNumber || '', Validators.required],
      bankAccountNumber: [this.data.compensation?.bankAccountNumber || '', Validators.required],
      grossBaseSalary: [this.data.compensation?.grossBaseSalary || '', [Validators.required, Validators.min(0)]],
      currency: [this.data.compensation?.currency || 'HUF', Validators.required]
    });
  }

  save() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }
}