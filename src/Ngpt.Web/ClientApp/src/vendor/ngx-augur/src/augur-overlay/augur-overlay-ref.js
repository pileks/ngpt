"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AugurOverlayRef = void 0;
var rxjs_1 = require("rxjs");
// R = Response Data Type, T = Data passed to Modal Type
var AugurOverlayRef = /** @class */ (function () {
    function AugurOverlayRef(overlay, content, data // pass data to modal i.e. FormData
    ) {
        var _this = this;
        this.overlay = overlay;
        this.content = content;
        this.data = data;
        this.afterClosed$ = new rxjs_1.Subject();
        overlay.backdropClick().subscribe(function () { return _this._close('backdropClick', null); });
    }
    AugurOverlayRef.prototype.close = function (data) {
        this._close('close', data);
    };
    AugurOverlayRef.prototype._close = function (type, data) {
        this.overlay.dispose();
        this.afterClosed$.next({
            type: type,
            data: data
        });
        this.afterClosed$.complete();
    };
    return AugurOverlayRef;
}());
exports.AugurOverlayRef = AugurOverlayRef;
//# sourceMappingURL=augur-overlay-ref.js.map