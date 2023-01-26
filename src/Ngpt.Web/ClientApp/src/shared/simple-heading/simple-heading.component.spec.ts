import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimpleHeadingComponent } from './simple-heading.component';

describe('SimpleHeadingComponent', () => {
  let component: SimpleHeadingComponent;
  let fixture: ComponentFixture<SimpleHeadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimpleHeadingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SimpleHeadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
