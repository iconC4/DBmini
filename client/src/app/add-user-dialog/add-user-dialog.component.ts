import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { ApiService } from '../services/api.service';

interface Permission {
  permissionName: string;
  isReadable: boolean;
  isWritable: boolean;
  isDeletable: boolean;
}

@Component({
  selector: 'app-add-user-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatTableModule,
    MatSelectModule,
    MatOptionModule, // Import MatOptionModule for <mat-option>
  ],
  templateUrl: './add-user-dialog.component.html',
  styleUrls: ['./add-user-dialog.component.css'],
})
export class AddUserDialogComponent {
  roles = [
    { roleId: '1', roleName: 'Super Admin' },
    { roleId: '2', roleName: 'Admin' },
    { roleId: '3', roleName: 'Employee' },
    { roleId: '4', roleName: 'Lorem Ipsum' },
  ];
  permissions: Permission[] = [
    {
      permissionName: 'Super Admin',
      isReadable: false,
      isWritable: false,
      isDeletable: false,
    },
    {
      permissionName: 'Admin',
      isReadable: false,
      isWritable: false,
      isDeletable: false,
    },
    {
      permissionName: 'Employee',
      isReadable: false,
      isWritable: false,
      isDeletable: false,
    },
    {
      permissionName: 'Lorem Ipsum',
      isReadable: false,
      isWritable: false,
      isDeletable: false,
    },
  ];
  displayedColumns = ['module', 'read', 'write', 'delete'];

  user = {
    userId: '',
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    username: '',
    password: '',
    roleId: '',
    roleName: '',
    permissions: this.permissions,
  };

  constructor(
    public dialogRef: MatDialogRef<AddUserDialogComponent>,
    private apiService: ApiService
  ) {}

  onCancel(): void {
    this.dialogRef.close();
  }

  onRoleSelect(selectedRoleId: string): void {
    const selectedRole = this.roles.find(
      (role) => role.roleId === selectedRoleId
    );
    if (selectedRole) {
      this.user.roleId = selectedRole.roleId;
      this.user.roleName = selectedRole.roleName;
    }
  }
  onAddUser(): void {
    this.apiService.createUser(this.user).subscribe({
      next: (response) => {
        console.log('User added successfully', response);
        this.dialogRef.close(true); // Close dialog and indicate success
      },
      error: (error) => {
        console.error('Error adding user:', error);
        if (error.error && error.error.errors) {
          console.error('Validation errors:', error.error.errors);
        }
      },
    });
  }
}
