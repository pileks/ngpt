import { HttpResponse } from '@angular/common/http';
import { Component, ContentChild, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { AugurEntityController, AugurHttpRequest } from '@augur';
import { NgbTypeaheadSelectItemEvent } from '@ng-bootstrap/ng-bootstrap';
import { Subject, Observable, merge, from, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError, map, finalize } from 'rxjs/operators';

@Component({
    selector: 'app-entity-select',
    templateUrl: './entity-select.component.html',
    styleUrls: ['./entity-select.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: EntitySelectComponent,
            multi: true
        }
    ]
})
export class EntitySelectComponent<TEntity> implements ControlValueAccessor {
    constructor() { }

    @Input() disabled: boolean | string;
    @Input() required: boolean = false;

    @Input() entityApiController: AugurEntityController<TEntity>;
    @Input() listItems: (pageNumber: number, pageSize: number, searchQuery: string, columnFilter: any) => AugurHttpRequest<HttpResponse<any[]>>;
    @Input() getItem: (id: number) => AugurHttpRequest<any>;

    @Input() identifier: string = 'id';
    @Input() label: string = '';
    @Input() name: string = '';

    get innerId(): string {
        return `${this.name}-inner`;
    }

    @Output() onSelect = new EventEmitter();
    @Output() onUnselect = new EventEmitter();

    value: any;
    localValue: any;

    focus$ = new Subject<string>();
    dropdownOpen: boolean;
    isLoading: boolean;

    @ViewChild('instance') instance: any;

    @ContentChild('option', { static: true }) optionTmpl: TemplateRef<any>;
    @ContentChild('default', { static: true }) defaultTmpl: TemplateRef<any>;
    @ContentChild('noItemSelected', { static: true }) noItemSelectedTmpl: TemplateRef<any>;

    private defaultOptionItem: {
        isDefault: boolean,
        entity: TEntity;
    };

    search = (text$: Observable<string>) => {
        const debouncedText$ = text$.pipe(debounceTime(400), distinctUntilChanged());
        const inputFocus$ = this.focus$;

        if (!this.listItems) {
            return merge(debouncedText$, inputFocus$).pipe(
                tap(() => {
                    this.onRequestStart();
                    this.dropdownOpen = true;
                }),
                switchMap((searchText: string) => from(this.entityApiController.list(1, 50, searchText, null))
                    .pipe(catchError((() => { setTimeout(() => this.dropdownOpen = false); return of([]); })),
                        map((resp: HttpResponse<TEntity[]>) => {
                            this.onRequestEnd();

                            if (this.defaultTmpl) {
                                const items = resp.body.map(e => {
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
            return merge(debouncedText$, inputFocus$).pipe(
                tap(() => {
                    this.onRequestStart();
                    this.dropdownOpen = true;
                }),
                switchMap((searchText: string) => from(this.listItems(1, 50, searchText, null))
                    .pipe(catchError((() => { setTimeout(() => this.dropdownOpen = false); return of([]); })),
                        map((resp: HttpResponse<TEntity[]>) => {
                            this.onRequestEnd();
                            if (this.defaultTmpl) {
                                const items = resp.body.map(e => {
                                    return {
                                        isDefault: false,
                                        entity: e
                                    };
                                });

                                this.localValue = this.defaultOptionItem;

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
    };

    onLabelClick(): void {
        if (!this.instance) {
            this.openDropdown();
        }
    }

    onFocusOut(): void {
        this.dropdownOpen = false;

        this.onTouch();
    }

    openDropdown(): void {
        if (!this.disabled) {
            this.dropdownOpen = true;

            setTimeout(() => {
                this.focusInput();
            });
        }
    }

    onSelected(event: NgbTypeaheadSelectItemEvent): void {
        this.dropdownOpen = false;

        this.localValue = event.item;

        this.updateChanges();

        this.onTouch();
    }

    removeSelection(): void {
        this.localValue = null;

        this.updateChanges();

        this.onTouch();
    }

    formatInput(val: any): string {
        return '';
    }

    private onRequestStart(): void {
        this.isLoading = true;
    }

    private onRequestEnd(): void {
        this.isLoading = false;
    }

    private loadEntityFromModel(): void {
        this.onRequestStart();

        if (this.getItem) {
            this.getItem(this.value).promise.then(r => {
                this.localValue = r;
                this.onRequestEnd();
            });
        } else {
            this.entityApiController.get(this.value).promise.then(r => {
                this.localValue = r;
                this.onRequestEnd();
            });
        }
    }

    private focusInput(): void {
        this.instance._elementRef.nativeElement.focus();
    }

    //ngModel stuff
    updateChanges(): void {
        if (this.localValue) {
            this.value = this.localValue[this.identifier];
            this.onSelect.emit(this.localValue);
        } else {
            this.value = null;
            this.onUnselect.emit();
        }

        this.onChange(this.value);
    }

    writeValue(value: any): void {
        this.value = value;

        if (this.value) {
            this.loadEntityFromModel();
        } else {
            this.localValue = null;
        }

        if (this.defaultTmpl) {
            this.defaultOptionItem = {
                isDefault: true,
                entity: null
            };

            setTimeout(() => {
                this.localValue = this.defaultOptionItem;
            });
        }
    }

    onChange = (value: any) => { };

    onTouch = () => { };

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouch = fn;
    }
}
