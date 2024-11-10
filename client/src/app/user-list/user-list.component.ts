import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';

export interface User {
  name: string;
  email: string;
  role: string;
  createDate: string;
  roleDescription: string;
}

const USER_DATA: User[] = [
  {
    name: 'David Wagner',
    email: 'david_wagner@example.com',
    role: 'Super Admin',
    createDate: '24 Oct, 2015',
    roleDescription: 'Lorem Ipsum',
  },
  {
    name: 'Ina Hogan',
    email: 'windler.warren@runte.net',
    role: 'Admin',
    createDate: '24 Oct, 2015',
    roleDescription: 'Lorem Ipsum',
  },
  // Add more users here
];

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatTableModule], // Use CommonModule
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent {
  displayedColumns: string[] = [
    'name',
    'role',
    'createDate',
    'roleDescription',
    'actions',
  ];
  users = USER_DATA;
}
