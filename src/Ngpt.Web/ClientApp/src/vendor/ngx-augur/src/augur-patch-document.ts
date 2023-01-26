export interface AugurPatchItem {
    op: string;
    path: string;
    value: string;
}

export type AugurPatchDocument = AugurPatchItem[];