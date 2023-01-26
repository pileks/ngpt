import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingQuestionEditorComponent } from './reading-question-editor.component';

describe('ReadingQuestionEditorComponent', () => {
  let component: ReadingQuestionEditorComponent;
  let fixture: ComponentFixture<ReadingQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReadingQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
