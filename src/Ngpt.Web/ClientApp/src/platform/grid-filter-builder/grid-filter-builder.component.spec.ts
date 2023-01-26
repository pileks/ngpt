import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GridFilterBuilderComponent } from './grid-filter-builder.component';

describe('GridFilterBuilderComponent', () => {
  let component: GridFilterBuilderComponent;
  let fixture: ComponentFixture<GridFilterBuilderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GridFilterBuilderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GridFilterBuilderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
