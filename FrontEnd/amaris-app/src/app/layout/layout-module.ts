import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Navbar } from './navbar/navbar';
import { Sidebar } from './sidebar/sidebar';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    Navbar,
    Sidebar
  ],
  imports: [
    CommonModule,
    RouterModule 
  ],
  exports:[Navbar,Sidebar]
})
export class LayoutModule { }
