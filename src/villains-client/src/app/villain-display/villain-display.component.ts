import {Component, Input} from '@angular/core';
import {Router} from '@angular/router';
import {Villain} from "../models/villain";
import {ImageService} from "../services/image.service";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-villain-display',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './villain-display.component.html',
  styleUrl: './villain-display.component.scss'
})
export class VillainDisplayComponent {

  @Input() villain!: Villain;

  constructor(
    protected imageService: ImageService,
    private router: Router
  ) {
  }

  public getImgSrc(imageName: string): string {
    return this.imageService.GetImageSource(imageName);
  }

  public edit() {
    this.router.navigate(['/', 'villain', 'edit', this.villain.id]).then(() => {
    });
  }

  public flip() {
    this.villain.flipped = !this.villain.flipped;
  }
}
