import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswerPreviewComponent } from './answer-preview.component';

describe('AnswerPreviewComponent', () => {
  let component: AnswerPreviewComponent;
  let fixture: ComponentFixture<AnswerPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnswerPreviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnswerPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
