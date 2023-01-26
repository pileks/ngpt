import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleAnswerQuestionPlayerComponent } from './single-answer-question-player.component';

describe('SingleAnswerQuestionPlayerComponent', () => {
  let component: SingleAnswerQuestionPlayerComponent;
  let fixture: ComponentFixture<SingleAnswerQuestionPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingleAnswerQuestionPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleAnswerQuestionPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
