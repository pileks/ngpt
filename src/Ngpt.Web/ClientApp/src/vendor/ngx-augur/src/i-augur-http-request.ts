export interface IAugurHttpRequest extends PromiseLike<any> {
    isLoading: boolean;
    hasFinished: boolean;
    hasErrors: boolean;
    isSuccess: boolean;
    onException: () => void;
    promise: any;
}
