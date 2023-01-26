import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructionUpdateComponent } from './instruction-update.component';

describe('InstructionUpdateComponent', () => {
  let component: InstructionUpdateComponent;
  let fixture: ComponentFixture<InstructionUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InstructionUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InstructionUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
