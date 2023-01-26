import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'app-answer-preview',
    templateUrl: './answer-preview.component.html',
    styleUrls: ['./answer-preview.component.css']
})
export class AnswerPreviewComponent implements OnInit {

    constructor() { }

    @Input() correct: boolean;

    ngOnInit(): void {
    }

}
