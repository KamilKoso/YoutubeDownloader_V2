import { defer } from "rxjs";
export function prepare(callback) {
    return (source) => defer(() => {
        callback();
        return source;
    });
}
//# sourceMappingURL=prepare.js.map