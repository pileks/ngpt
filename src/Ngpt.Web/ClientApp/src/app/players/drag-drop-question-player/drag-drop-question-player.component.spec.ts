import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DragDropQuestionPlayerComponent } from './drag-drop-question-player.component';

describe('DragDropQuestionPlayerComponent', () => {
  let component: DragDropQuestionPlayerComponent;
  let fixture: ComponentFixture<DragDropQuestionPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DragDropQuestionPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DragDropQuestionPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
