import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingQuestionPlayerComponent } from './reading-question-player.component';

describe('ReadingQuestionPlayerComponent', () => {
  let component: ReadingQuestionPlayerComponent;
  let fixture: ComponentFixture<ReadingQuestionPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReadingQuestionPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingQuestionPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
