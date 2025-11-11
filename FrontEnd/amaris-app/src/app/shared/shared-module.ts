import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Unauthorized } from './unauthorized/unauthorized';



@NgModule({
  declarations: [
    Unauthorized,
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
