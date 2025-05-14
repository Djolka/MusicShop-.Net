import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable, throwError } from "rxjs";

export abstract class HttpErrorHandler {
    constructor (private router: Router) {      
    }
    
    protected handleError() {
        return (err: HttpErrorResponse): Observable<never> => {
            if (err.error instanceof ErrorEvent) {
                console.log('An error occured: ',  err.error.message)
            } else { 
                this.router.navigate(['/error', { message: err.message, statusCode: err.status}])
            }
            return throwError('Something bad happend; please try again later')
        }
    }
}