import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListeningQuestionPlayerComponent } from './listening-question-player.component';

describe('ListeningQuestionPlayerComponent', () => {
  let component: ListeningQuestionPlayerComponent;
  let fixture: ComponentFixture<ListeningQuestionPlayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListeningQuestionPlayerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListeningQuestionPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
