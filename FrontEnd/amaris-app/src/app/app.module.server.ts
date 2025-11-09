import { NgModule } from '@angular/core';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { App } from './app';
import { AppModule } from './app-module';
import { serverRoutes } from './app.routes.server';
import { ServerModule } from '@angular/platform-server';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [AppModule, ServerModule, HttpClientModule],
  providers: [provideServerRendering(withRoutes(serverRoutes))],
  bootstrap: [App],
})
export class AppServerModule {}
