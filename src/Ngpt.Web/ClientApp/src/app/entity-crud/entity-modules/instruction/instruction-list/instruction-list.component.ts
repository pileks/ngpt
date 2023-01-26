import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GridDataSource } from '@augur';
import { InstructionsController } from '@src/app/web-api-controllers/instructions/instructions-controller';
import { GridFilterEntityResolverService } from '@src/app/services/grid-filter-entity-resolver.service';
import { GridFilterEnumResolverService } from '@src/app/services/grid-filter-enum-resolver.service';
import { InstructionGridModel } from '@src/app/web-api-controllers/instructions/models/instruction-grid-model';
import { UseOfLanguageQuestionTypeDefinition } from '@src/app/enums/use-of-language-question-type';

@Component({
    selector: 'app-instruction-list',
    templateUrl: './instruction-list.component.html',
    styleUrls: ['./instruction-list.component.css']
})
export class InstructionListComponent {

    constructor(
        public entityResolver: GridFilterEntityResolverService,
        public enumResolver: GridFilterEnumResolverService,
        private router: Router,
        private instructionsController: InstructionsController
    ) {
    }

    questionTypes = UseOfLanguageQuestionTypeDefinition;

    gridDataSource = new GridDataSource<InstructionGridModel>(
        model => this.instructionsController.grid(model)
    );

    open(item: InstructionGridModel): void {
        this.router.navigate(['/instruction/update', item.id]);
    }

    openInNewTab(item: InstructionGridModel) {
        const url = this.router.serializeUrl(
            this.router.createUrlTree(['/instruction/update', item.id])
        );

        window.open(url, '_blank');
    }

    create() {
        this.router.navigate(['instruction/create']);
    }
}
