import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DragDropQuestion } from '@src/app/entities/drag-drop-question';
import { DragDropQuestionPart } from '@src/app/entities/drag-drop-question-part';
import IResettable from '@src/app/players/resettable-question-player';
import { SortableEvent, SortableOptions } from 'sortablejs';

@Component({
    selector: 'app-drag-drop-question-player',
    templateUrl: './drag-drop-question-player.component.html',
    styleUrls: ['./drag-drop-question-player.component.css']
})
export class DragDropQuestionPlayerComponent implements OnInit, IResettable {

    constructor() { }

    movableSortableOptions: SortableOptions = {
        group: 'answers',
        filter: '.prevent-drag',
        animation: 150,
        swapThreshold: 0.6,
        ghostClass: 'ghost',
        chosenClass: 'chosen',
        dragClass: 'drag'
    }

    answerSortableOptions: SortableOptions = {
        ...this.movableSortableOptions,
        onSort: () => {
            console.log("onMove")
            this.answerChange.emit(this.isAnswerCorrect());
        },
    }

    @Input() question: DragDropQuestion;
    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();
    @Input() instruction: string = '';

    answerQuestionParts: DragDropQuestionPart[];
    movableQuestionParts: DragDropQuestionPart[];

    updateAnswer(): void {
        this.answerChange.emit(this.isAnswerCorrect());
    }

    isAnswerCorrect(): boolean {
        if (this.movableQuestionParts.length > 0) {
            return false;
        }

        for (var i = 1; i < this.answerQuestionParts.length; i++) {
            if (this.answerQuestionParts[i].ordinal < this.answerQuestionParts[i - 1].ordinal) {
                return false;
            }
        }

        return true;
    };

    dropQuestionPart(event: CdkDragDrop<DragDropQuestionPart[]>) {
        if (event.previousContainer === event.container) {
            moveItemInArray(
                event.container.data,
                event.previousIndex,
                event.currentIndex);
        } else {
            transferArrayItem(
                event.previousContainer.data,
                event.container.data,
                event.previousIndex,
                event.currentIndex);
        }

        this.answerChange.emit(this.isAnswerCorrect());
    }

    reset(): void {
        this.initialize();
        this.answerChange.emit(this.isAnswerCorrect());
    }

    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.')
        }

        this.initialize();
    }

    initialize() {
        this.movableQuestionParts = this.question.parts.filter(x => x.isDraggable).sort((a, b) => Math.random() - 0.5);
        this.answerQuestionParts = this.question.parts
            .filter(x => !x.isDraggable)
            .sort((a, b) => a.ordinal - b.ordinal)
            .flatMap((x) =>
                x.text.split(' ')
                    .map((t) => new DragDropQuestionPart({
                        id: x.id,
                        isDraggable: x.isDraggable,
                        ordinal: x.ordinal,
                        question: x.question,
                        questionId: x.questionId,
                        text: t
                    }))
            );
    }
}
