import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { ApiService } from '../services/api.service';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

export interface ApiResponse<T> {
  status: {
    code: string;
    description: string;
  };
  data: T;
}

export interface User {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  username: string;
  role?: {
    roleId: string;
    roleName: string;
  };
  permissions?: Array<{
    permissionId: string;
    permissionName: string;
    isReadable: boolean;
    isWritable: boolean;
    isDeletable: boolean;
  }>;
}

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    RouterModule,
  ],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  dataSource = new MatTableDataSource<User>([]);
  isLoading = false;
  error: string | null = null;

  displayedColumns: string[] = ['name', 'createDate', 'role', 'actions'];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.error = null;

    this.apiService.getData().subscribe({
      next: (response: ApiResponse<User[]>) => {
        if (response.status.code === '200') {
          this.dataSource.data = response.data;
        } else {
          this.error = response.status.description;
        }
      },
      error: (error) => {
        this.error = 'Failed to load users. Please try again later.';
        console.error('Error:', error);
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }

  deleteUser(userId: string): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.apiService.deleteUser(userId).subscribe({
        next: (response: ApiResponse<any>) => {
          if (response.status.code === '204') {
            this.loadUsers();
          } else {
            this.error = response.status.description;
          }
        },
        error: (error) => {
          this.error = 'Failed to delete user. Please try again later.';
          console.error('Error:', error);
        },
      });
    }
  }
}
