import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleGapQuestionPlayerComponent } from './single-gap-question-player.component';

describe('SingleGapQuestionPlayerComponent', () => {
  let component: SingleGapQuestionPlayerComponent;
  let fixture: ComponentFixture<SingleGapQuestionPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingleGapQuestionPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleGapQuestionPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
