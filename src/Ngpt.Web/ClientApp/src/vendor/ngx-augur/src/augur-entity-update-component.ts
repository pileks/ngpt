import { ActivatedRoute, Router } from '@angular/router';
import { IAugurEntityWithId } from './i-augur-entity-with-id';
import { OnInit, Input, Directive } from '@angular/core';
import { AugurEntityController } from './augur-entity-controller';
import { AugurHttpRequest } from './augur-http-request';

@Directive()
export abstract class EntityUpdateComponent<TEntity extends IAugurEntityWithId> implements OnInit {

    constructor(protected entityName: string, protected route: ActivatedRoute, protected router: Router, protected apiController: AugurEntityController<TEntity>) {
    }

    @Input() id: number;

    @Input() entity: TEntity;

    public onDeleteWarning: string = 'Are you sure you want to delete this record?';

    get isEntityLoading(): boolean {
        return this.request && this.request.isLoading;
    }

    protected request: AugurHttpRequest<TEntity>;

    async ngOnInit() {
        this.id = this.id
            ? this.id
            : this.getEntityIdFromRoute()
                ? Number(this.getEntityIdFromRoute())
                : null;

        await this.loadEntity();
    }

    public getEntityIdFromRoute() {
        return this.route.snapshot.paramMap.get('id');
    }

    public async loadEntity() {
        if (this.id && !this.entity) {
            this.entity = await this.get();

            this.onLoadEntity();
        }
        else if (!this.entity) {
            this.initNewEntity();
        }
    }

    protected async get(): Promise<TEntity> {
        this.request = this.apiController.get(this.id);

        return this.request.promise;
    }

    protected onLoadEntity() {
    }

    protected initNewEntity() {
        this.entity = <TEntity>{};
    }

    public async create() {
        this.request = this.apiController.create(this.entity);

        this.entity = await this.request.promise;

        this.onCreate();
    }

    protected onCreate() {
        this.router.navigate(['/' + this.entityName + '/list']);
    }

    public async update() {
        this.request = this.apiController.update(this.id, this.entity);

        await this.request.promise;

        this.onUpdate();
    }

    protected onUpdate() {
        this.router.navigate(['/' + this.entityName + '/list']);
    }

    protected onDelete() {
        this.router.navigate(['/' + this.entityName + '/list']);
    }

    protected async delete() {
        if (confirm(this.onDeleteWarning)) {
            await this.apiController.delete(this.entity.id).promise;

            this.onDelete();
        }
    }
}
