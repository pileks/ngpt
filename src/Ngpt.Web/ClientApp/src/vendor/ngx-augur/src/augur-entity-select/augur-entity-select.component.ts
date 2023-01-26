import { Component, OnInit, Input, EventEmitter, Output, TemplateRef, ContentChild, ViewChild, forwardRef, HostListener, ViewEncapsulation } from '@angular/core';
import { HttpParams, HttpResponse } from '@angular/common/http';
import { catchError, debounceTime, distinctUntilChanged, filter, map, switchMap, tap, finalize } from 'rxjs/operators';
import { NgbTypeahead } from '@ng-bootstrap/ng-bootstrap';
import { Subject, Observable, merge, of, from } from 'rxjs';
import { AugurEntityController } from '../augur-entity-controller';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AugurHttpRequest } from '@augur';

@Component({
    selector: 'augur-entity-select',
    templateUrl: './augur-entity-select.component.html',
    styleUrls: ['./augur-entity-select.component.css'],
    encapsulation: ViewEncapsulation.None,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => AugurEntitySelectComponent),
            multi: true
        }
    ]
})
export class AugurEntitySelectComponent<TEntity> implements ControlValueAccessor, OnInit {

    @ContentChild('option', { static: true }) optionTmpl: TemplateRef<any>;
    @ContentChild('default', { static: true }) defaultTmpl: TemplateRef<any>;
    @ContentChild('noItemSelected', { static: true }) noItemSelectedTmpl: TemplateRef<any>;

    @Input() disabled: boolean;
    @Input() required: boolean;
    @Input() entityApiController: AugurEntityController<TEntity>;
    @Input() listItems: (pageNumber: number, pageSize: number, searchQuery: string, columnFilter: any) => AugurHttpRequest<HttpResponse<any[]>>;
    @Input() identifier: string = 'id';
    @Input() labelFrom: string;
    @Input() label: string = '';
    @Output() onSelect = new EventEmitter();
    @Output() onUnselect = new EventEmitter();
    @Output() on = new EventEmitter();

    modelValue: any;
    @Output() modelChange = new EventEmitter();

    @Input()
    get model() {
        return this.modelValue;
    }
    set model(val) {
        this.modelValue = val;
        this.modelChange.emit(this.model);
        if (this.modelValue === null || this.modelValue === undefined) {
            this.selectedEntityValue = null;
            this.selectedEntityChange.emit(null);
        }
        this.onChange(this.modelValue);
        this.onTouch(this.modelValue);
    }

    selectedEntityValue: any;
    @Output() selectedEntityChange = new EventEmitter();

    @Input()
    get selectedEntity() {
        return this.selectedEntityValue;
    }
    set selectedEntity(val) {
        if (val && typeof val === 'object') {
            this.selectedEntityValue = val;
            this.model = val[this.identifier];

            this.selectedEntityChange.emit(this.selectedEntity);
        } else if (!val && this.selectedEntityValue) {
            this.selectedEntityValue = undefined;
            this.model = undefined;

            this.selectedEntityChange.emit(this.selectedEntity);
        }
    }

    @ViewChild('instance') inputElement: any;
    @ViewChild('instance') instance: NgbTypeahead;
    public focus$ = new Subject<string>();
    public click$ = new Subject<string>();
    public shouldOpenDropdown: boolean;

    public isLoading: boolean;

    private defaultOptionItem: { isDefault: boolean, entity: TEntity };

    search = (text$: Observable<string>) => {
        const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
        const clicksWithClosedPopup$ = this.click$.pipe(filter(() => !this.instance.isPopupOpen()));
        const inputFocus$ = this.focus$;

        if (!this.listItems) {
            return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$).pipe(
                tap(() => this.isLoading = true),
                switchMap((searchText: string) => from(this.entityApiController.list(1, 50, searchText, null).promise)
                    .pipe(catchError((() => { setTimeout(() => this.shouldOpenDropdown = false); return of([]); })),
                        map((resp: HttpResponse<TEntity[]>) => {
                            this.onRequestEnd();

                            if(this.defaultTmpl) {
                                var items = resp.body.map(e => {
                                    return {
                                        isDefault: false,
                                        entity: e
                                    };
                                });

                                return [this.defaultOptionItem].concat(items);
                            }
                            else {
                                return resp.body;
                            }
                        })
                    )
                ),
                finalize(() => {
                    this.onRequestEnd();
                }),
            );
        } else {
            return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$).pipe(
                tap(() => this.isLoading = true),
                switchMap((searchText: string) => from(this.listItems(1, 50, searchText, null).promise)
                    .pipe(catchError((() => { setTimeout(() => this.shouldOpenDropdown = false); return of([]); })),
                        map((resp: HttpResponse<TEntity[]>) => {
                            this.onRequestEnd();
                            
                            if(this.defaultTmpl) {
                                var items = resp.body.map(e => {
                                    return {
                                        isDefault: false,
                                        entity: e
                                    };
                                });

                                this.selectedEntity = this.defaultOptionItem;

                                return [this.defaultOptionItem].concat(items);
                            }
                            else {
                                return resp.body;
                            }
                        })
                    )
                ),
                finalize(() => {
                    this.onRequestEnd();
                }),
            );
        }
    }

    public onFocusOut() {
        this.shouldOpenDropdown = false;
    }

    private onRequestEnd() {
        this.isLoading = false;
    }

    ngOnInit() {
        this.loadEntityFromModel();

        if (this.defaultTmpl) {
            this.defaultOptionItem = {
                isDefault: true,
                entity: null
            };

            setTimeout(() => {
                this.selectedEntity = this.defaultOptionItem;
            });
        }
    }

    loadEntityFromModel() {
        if (this.model) {
            this.isLoading = true;
            this.entityApiController.get(this.model).promise.then(r => {
                this.selectedEntity = r;
                this.isLoading = false;
            });
        }
    }

    public openDropdown($event) {
        this.shouldOpenDropdown = true;

        setTimeout(() => {
            this.click$.next($event.target.value);
            this.focusInput();
        });
    }

    public focusInput() {
        this.inputElement._elementRef.nativeElement.focus();
    }

    public isPopupOpen() {
        return this.instance && this.instance.isPopupOpen();
    }

    public onSelected() {
        this.shouldOpenDropdown = false;

        setTimeout(() => {
            if (this.selectedEntity) {
                this.onSelect.emit(this.selectedEntity);
            }
        });
    }

    public formatInput(val) {
        return '';
    }

    public shouldShowReadOnlySelect() {
        return !this.shouldOpenDropdown && !this.isPopupOpen();
    }

    public removeSelection() {
        this.selectedEntity = null;

        setTimeout(() => {
            if (!this.selectedEntity) {
                this.onUnselect.emit();
            }
        });
    }
    
    onChange: any = () => {};
    onTouch: any = () => {};
    get value() {
        return this.model;
    }
    set value(val) {
        this.model = val;
        this.loadEntityFromModel();
    }

    writeValue(value: any) {
        this.value = value;
    }

    registerOnChange(fn: any) {
        this.onChange = fn;
    }

    registerOnTouched(fn: any) {
        this.onTouch = fn;
    }
}
