import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {NgbDropdown, NgbDropdownMenu, NgbDropdownToggle} from "@ng-bootstrap/ng-bootstrap";
import {Villain} from '../models/villain';
import {VillainService} from '../services/villain.service';
import {ImageService} from "../services/image.service";
import {AsyncPipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {FormsModule} from "@angular/forms";

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
    FormsModule
  ],
  templateUrl: './villain-selector.component.html',
  styleUrl: './villain-selector.component.scss'
})
export class VillainSelectorComponent {

  public villains: Villain[];

  public filteredVillains: Villain[];

  public sortOrder: string = 'default';

  public nameFilter: string = '';

  setSortOrder(order: string) {
    this.sortOrder = order;
    this.sortVillains();
  }

  private sortVillains() {
    switch (this.sortOrder) {
      case 'asc':
        this.filteredVillains.sort((a, b) => a.name.localeCompare(b.name));
        break;
      case 'desc':
        this.filteredVillains.sort((a, b) => b.name.localeCompare(a.name));
        break;
      default:
        // sort the villains in the default order
        break;
    }
  }

  public filterVillains() {
    this.filteredVillains = this.villains
      .filter(villain => villain.name.toLowerCase().includes(this.nameFilter.toLowerCase()));
  }

  public edit(villain: Villain) {
    this.router.navigate(['/', 'villain', 'edit', villain.id]).then(() => {
    });
  }

  public flip(villain: Villain) {
    villain.flipped = !villain.flipped;
  }

  constructor(
    protected villainService: VillainService,
    protected imageService: ImageService,
    private router: Router
  ) {
    this.villains = [];
    this.filteredVillains = [];
    this.populateVillains().then(() => {
    });
  }

  public getImgSrc(imageName: string): string {
    return this.imageService.GetImageSource(imageName);
  }

  private async populateVillains() {
    this.villains = await this.villainService.GetAllAsync();
    this.filteredVillains = this.villains;
    for (const villain of this.villains) {
      await this.imageService.CacheImageAsync(villain.imageName);
    }
  }
}
