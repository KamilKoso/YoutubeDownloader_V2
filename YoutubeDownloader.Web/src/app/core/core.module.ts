import { CommonModule } from "@angular/common";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ToastrModule } from "ngx-toastr";
import { DirectivesModule } from "./directives/directives.module";
import { ErrorHandlingInterceptor } from "./interceptors/error-handler.interceptor";

import { HttpAuthInterceptor } from "./interceptors/http-auth-interceptor";
import { HttpRequestCancellingInterceptor } from "./interceptors/http-request-cancelling-interceptor";
import { BlobErrorHttpInterceptor } from "./interceptors/htttp-blob-error-interceptor";

@NgModule({
  imports: [CommonModule, HttpClientModule, ToastrModule.forRoot({ progressBar: true }), DirectivesModule],
  declarations: [],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlingInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BlobErrorHttpInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpAuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpRequestCancellingInterceptor,
      multi: true,
    },
  ],
})
export class CoreModule {}
