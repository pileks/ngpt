import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DragDropQuestionEditorComponent } from './drag-drop-question-editor.component';

describe('DragDropQuestionEditorComponent', () => {
  let component: DragDropQuestionEditorComponent;
  let fixture: ComponentFixture<DragDropQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DragDropQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DragDropQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
