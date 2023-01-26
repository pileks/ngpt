import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingListeningQuestionAnswerPickerComponent } from './reading-listening-question-answer-picker.component';

describe('ReadingListeningQuestionAnswerPickerComponent', () => {
  let component: ReadingListeningQuestionAnswerPickerComponent;
  let fixture: ComponentFixture<ReadingListeningQuestionAnswerPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReadingListeningQuestionAnswerPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingListeningQuestionAnswerPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
