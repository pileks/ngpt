import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultipleChoiceQuestionPlayerComponent } from './multiple-choice-question-player.component';

describe('MultipleChoiceQuestionPlayerComponent', () => {
  let component: MultipleChoiceQuestionPlayerComponent;
  let fixture: ComponentFixture<MultipleChoiceQuestionPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultipleChoiceQuestionPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MultipleChoiceQuestionPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
