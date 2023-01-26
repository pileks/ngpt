export class AugurNavigationMenuItem {
    constructor(init?: Partial<AugurNavigationMenuItem>) {
        Object.assign(this, init);
    }
    text: string;
    iconClass: string;
    faIcon: Array<string> | string;
    isFixedToBottom: boolean;
    showIf: boolean;
    route: string;
    separatorBefore: boolean;
    separatorAfter: boolean;
    requireUserLoggedIn: boolean;
    requireSuperAdminLoggedIn: boolean;
    requireOrgAdminLoggedIn: boolean;
    click: () => any;
    get routerLink(): any {
        return [this.route];
    }
}
