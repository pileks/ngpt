export interface IGridFilterEnumResolver {
    resolve: (enumType: string) => GridFilterEnumDefinition[];
}

export class GridFilterEnumDefinition {
    constructor(init?: Partial<GridFilterEnumDefinition>) {
        Object.assign(this, init);
    }

    value: number;
    title: string;
}