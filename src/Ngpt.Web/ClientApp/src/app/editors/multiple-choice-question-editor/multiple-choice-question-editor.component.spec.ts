import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultipleChoiceQuestionEditorComponent } from './multiple-choice-question-editor.component';

describe('MultipleChoiceQuestionEditorComponent', () => {
  let component: MultipleChoiceQuestionEditorComponent;
  let fixture: ComponentFixture<MultipleChoiceQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultipleChoiceQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MultipleChoiceQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
