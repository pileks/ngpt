import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListeningQuestionEditorComponent } from './listening-question-editor.component';

describe('ListeningQuestionEditorComponent', () => {
  let component: ListeningQuestionEditorComponent;
  let fixture: ComponentFixture<ListeningQuestionEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListeningQuestionEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListeningQuestionEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
