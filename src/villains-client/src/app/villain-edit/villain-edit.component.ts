import {Component, OnInit} from '@angular/core';
import {Villain} from "../models/villain";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {VillainService} from "../services/villain.service";
import {ImageService} from "../services/image.service";
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";

@Component({
  selector: 'app-villain-edit',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './villain-edit.component.html',
  styleUrl: './villain-edit.component.scss'
})
export class VillainEditComponent implements OnInit {

  public villainFg: FormGroup;

  villain: Villain | undefined;

  private image: File | undefined;

  constructor(
    private route: ActivatedRoute,
    private villainService: VillainService,
    private imageService: ImageService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.villainFg = this.buildFormGroup();
  }

  private buildFormGroup() {
    return this.fb.group({
      name: [this.villain?.name, [Validators.required]],
      powers: [this.villain?.powers, [Validators.required]],
      buttonText: [this.villain?.buttonText, [Validators.required]],
      saying: [this.villain?.saying, [Validators.required]],
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.villainService.GetAsync(id)
        .then(villain => {
            this.villain = villain;
            this.villainFg = this.buildFormGroup();
            this.imageService.GetImageAsync(villain.imageName)
              .then(imgResponse => {
                if (!imgResponse.exists)
                  return;
                this.villain!.base64Image = imgResponse.imageSrc;
                this.villain!.imageLoaded = true;
              });
          }
        );
    });
  }

  public async onSubmit({ value, valid }: { value: Villain, valid: boolean }) {
    if (!valid) return;

    value.id = this.villain!.id;

    if (this.image != null) {
      console.log('Upload image');
      //let imageUploadResult = await this.imageService.AddAsync(this.image);
      //value.imageName = imageUploadResult.newFileName;
    }
    else
    {
      // if we're not uploading a new image, preserve the exsiting image name
      value.imageName = this.villain!.imageName;
    }

    value.imageName = this.villain!.imageName; // get rid of this once the image upload is working
    await this.villainService.UpdateAsync(value);
    await this.router.navigate(['/']);
  }
}
