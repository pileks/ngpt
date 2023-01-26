import { IAugurHttpRequest } from "./i-augur-http-request";

export class AugurHttpRequest<T> implements IAugurHttpRequest {
    constructor(init?: Partial<AugurHttpRequest<T>>) {
      Object.assign(this, init);
    }

    isLoading: boolean;
    hasFinished: boolean;
    hasErrors: boolean;
    isSuccess: boolean;
    onException: () => void;
    promise: Promise<T>;
    data: T;

    then<TResult1 = T, TResult2 = never>(onfulfilled?: ((value: T) => TResult1 | PromiseLike<TResult1>) | undefined | null, onrejected?: ((reason: any) => TResult2 | PromiseLike<TResult2>) | undefined | null): PromiseLike<TResult1 | TResult2> {
        return this.promise.then(onfulfilled, onrejected);
    }
};

;