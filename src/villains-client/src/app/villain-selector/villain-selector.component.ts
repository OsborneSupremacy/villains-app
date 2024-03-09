import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {Villain} from '../models/villain';
import {VillainService} from '../services/villain.service';
import {ImageService} from "../services/image.service";
import {AsyncPipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";

@Component({
  selector: 'app-villain-selector',
  standalone: true,
  imports: [
    NgForOf,
    NgOptimizedImage,
    NgIf,
    AsyncPipe
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
    for (const villain of this.villains) {
      const imgResponse = await this.imageService.GetImageAsync(villain.imageName);
      if(!imgResponse.exists)
        continue;
      villain.base64Image = imgResponse.imageSrc
      villain.imageLoaded = true;
    }
  }
}
