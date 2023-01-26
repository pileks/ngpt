import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { Instruction } from '@src/app/entities/instruction';
import { UseOfLanguageQuestionTypeDefinition } from '@src/app/enums/use-of-language-question-type';
import { InstructionsController } from '@src/app/web-api-controllers/instructions/instructions-controller';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';

@Component({
    selector: 'app-instruction-update',
    templateUrl: './instruction-update.component.html',
    styleUrls: ['./instruction-update.component.css']
})
export class InstructionUpdateComponent extends EntityUpdateComponent<Instruction> {

    constructor(
        route: ActivatedRoute,
        router: Router,
        instructionsController: InstructionsController,
        public languagesController: LanguagesController) {

        super('instruction', route, router, instructionsController);
    }

    listTypes = Object.values(UseOfLanguageQuestionTypeDefinition);

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    cancel() {
        this.router.navigate(['instruction/list']);
    }
}
