import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {Villain} from '../models/villain';
import {VillainService} from '../services/villain.service';
import {ImageService} from "../services/image.service";
import {NgForOf, NgIf, NgOptimizedImage} from "@angular/common";

@Component({
  selector: 'app-villain-selector',
  standalone: true,
  imports: [
    NgForOf,
    NgOptimizedImage,
    NgIf
  ],
  templateUrl: './villain-selector.component.html',
  styleUrl: './villain-selector.component.scss'
})
export class VillainSelectorComponent {

  public villains: Villain[];

  public selectedVillain(): Villain | undefined {
    return this.villainService.selectedVillain;
  }

  public select(villain: Villain) {
    this.villainService.selectedVillain = villain;
  }

  public edit(villain: Villain) {
    this.villainService.selectedVillain = villain;
    this.router.navigate(['/', 'Edit']).then(r => {});
  }

  public flip(villain: Villain) {
    villain.flipped = !villain.flipped;
  }

  public async getImageSrc(imageName: string) {
    const response = await this.imageService.GetImageAsync(imageName);
    return response.imageSrc;
  };

  constructor(
    protected villainService: VillainService,
    protected imageService: ImageService,
    private router: Router
  ) {
    this.villains = [];
    this.populateVillains().then(r => {});
  }

  private async populateVillains() {
    this.villains = await this.villainService.GetAllAsync();
  }
}
