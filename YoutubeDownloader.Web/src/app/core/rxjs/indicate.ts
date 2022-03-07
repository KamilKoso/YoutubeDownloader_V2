import { Subject, Observable } from "rxjs";
import { finalize } from "rxjs/operators";
import { prepare } from "./prepare";

export function indicate<T>(indicator: Subject<boolean>): (source: Observable<T>) => Observable<T> {
  return (source: Observable<T>): Observable<T> =>
    source.pipe(
      prepare(() => indicator.next(true)),
      finalize(() => indicator.next(false))
    );
}
