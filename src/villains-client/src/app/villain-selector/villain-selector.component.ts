import {Component} from '@angular/core';
import {NgbDropdown, NgbDropdownMenu, NgbDropdownToggle} from "@ng-bootstrap/ng-bootstrap";
import {Villain} from '../models/villain';
import {VillainService} from '../services/villain.service';
import {ImageService} from "../services/image.service";
import {AsyncPipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {VillainDisplayComponent} from "../villain-display/villain-display.component";

@Component({
  selector: 'app-villain-selector',
  standalone: true,
  imports: [
    NgForOf,
    NgOptimizedImage,
    NgIf,
    AsyncPipe,
    NgbDropdown,
    NgbDropdownMenu,
    NgbDropdownToggle,
    FormsModule,
    VillainDisplayComponent
  ],
  templateUrl: './villain-selector.component.html',
  styleUrl: './villain-selector.component.scss'
})
export class VillainSelectorComponent {

  public villains: Villain[];

  public filteredVillains: Villain[];

  public sortField: string = 'insertOn';

  public sortOrder: string = 'default';

  public nameFilter: string = '';

  constructor(
    protected villainService: VillainService,
    protected imageService: ImageService

  ) {
    this.villains = [];
    this.filteredVillains = [];
    this.populateVillains().then(() => {
    });
  }

  private sortVillains() {
    this.filteredVillains.sort((a: Villain, b: Villain) => {
      let comparison = 0;

      if (this.sortField === 'name') {
        // Compare by name
        const nameA = a.name.toUpperCase();
        const nameB = b.name.toUpperCase();

        if (nameA > nameB) {
          comparison = 1;
        } else if (nameA < nameB) {
          comparison = -1;
        }
      } else {
        // Compare by date
        const dateA = new Date(a.insertedOn);
        const dateB = new Date(b.insertedOn);
        comparison = dateA.getTime() - dateB.getTime();
      }

      // Reverse comparison if sortOrder is 'desc'
      if (this.sortOrder === 'desc') {
        comparison *= -1;
      }

      return comparison;
    });
  }

  setSort(field: string, order: string) {
    this.sortField = field;
    this.sortOrder = order;
    this.sortVillains();
  }

  public filterVillains() {
    this.filteredVillains = this.villains
      .filter(villain => villain.name.toLowerCase().includes(this.nameFilter.toLowerCase()));
  }

  private async populateVillains() {
    this.villains = await this.villainService.GetAllAsync();
    this.filteredVillains = this.villains;
    for (const villain of this.villains) {
      await this.imageService.CacheImageAsync(villain.imageName);
    }
  }
}
