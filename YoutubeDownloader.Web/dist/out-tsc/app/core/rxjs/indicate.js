import { finalize } from "rxjs/operators";
import { prepare } from "./prepare";
export function indicate(indicator) {
    return (source) => source.pipe(prepare(() => indicator.next(true)), finalize(() => indicator.next(false)));
}
//# sourceMappingURL=indicate.js.map