export class GridRequestModel {
    constructor(init?: Partial<GridRequestModel>) {
        this.sortBy = [];

        Object.assign(this, init);
    }

    page: number;
    pageSize: number;
    search: string;

    sortBy: GridSortModel[];
    filters: GridFilterModel[];
};

export class GridSortModel {
    constructor(init?: Partial<GridSortModel>) {
        Object.assign(this, init);
    }

    column: string;
    direction: string;
}

export class GridFilterModel {
    constructor(init?: Partial<GridFilterModel>) {
        Object.assign(this, init);
    }

    property: string;
    operator: GridFilterOperator;
    value: any;
}

export class GridResult<T> {
    constructor(init?: Partial<GridResult<T>>) {
        this.data = [];

        Object.assign(this, init);
    }

    data: T[];
    metadata: GridMetadata;
    count: number;
};

export class GridMetadata {
    constructor(init?: Partial<GridMetadata>) {
        this.columns = [];
        this.filters = [];

        Object.assign(this, init);
    }

    columns: GridColumnMetadata[];
    filters: GridFilterMetadata[];
};

export class GridColumnMetadata {
    constructor(init?: Partial<GridColumnMetadata>) {
        Object.assign(this, init);
    }

    title: string;
    property: string;
    type: string;
};

export class GridFilterMetadata {
    constructor(init?: Partial<GridFilterMetadata>) {
        Object.assign(this, init);
    }

    title: string;
    property: string;
    type: string;
    isAugurEntity: boolean;
    isEnum: boolean;
    operators: GridFilterOperatorModel[];
};

export class GridFilterOperatorModel {
    constructor(init?: Partial<GridFilterOperatorModel>) {
        Object.assign(this, init);
    }

    title: string;
    operator: GridFilterOperator;
}

export enum GridFilterOperator {
    Equals = 1,
    Greater = 2,
    Less = 3,
    GreaterOrEqual = 4,
    LessOrEqual = 5,
    Contains = 6
}