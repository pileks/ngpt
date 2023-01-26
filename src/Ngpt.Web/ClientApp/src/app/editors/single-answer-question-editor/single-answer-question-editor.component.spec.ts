import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleAnswerQuestionEditorComponent } from './single-answer-question-editor.component';

describe('SingleAnswerQuestionEditorComponent', () => {
  let component: SingleAnswerQuestionEditorComponent;
  let fixture: ComponentFixture<SingleAnswerQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingleAnswerQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleAnswerQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
