import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleGapQuestionEditorComponent } from './single-gap-question-editor.component';

describe('SingleGapQuestionEditorComponent', () => {
  let component: SingleGapQuestionEditorComponent;
  let fixture: ComponentFixture<SingleGapQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingleGapQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleGapQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
