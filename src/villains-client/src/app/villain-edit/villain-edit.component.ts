import {Component, OnInit} from '@angular/core';
import {Villain} from "../models/villain";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {VillainService} from "../services/villain.service";
import {ImageService} from "../services/image.service";
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";

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

  public villain: Villain | undefined;

  imageFileName: string = '';
  replaceImage: boolean = false;
  base64Image: string = '';

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
      saying: [this.villain?.saying, [Validators.required]]
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.villainService.GetAsync(id)
        .then(villain => {
            this.villain = villain;
            this.villainFg = this.buildFormGroup();
          }
        );
    });
  }

  public getImgSrc(imageName: string): string {
    return this.imageService.GetImageSource(imageName);
  }

  public async onSubmit({value, valid}: { value: Villain, valid: boolean }) {
    if (!valid)
      return;

    value.id = this.villain!.id;

    if (this.replaceImage) {
      const imageUploadResult = await this.imageService
        .AddAsync(this.imageFileName, this.base64Image);
      value.imageName = imageUploadResult.fileName;
    } else {
      value.imageName = this.villain!.imageName;
    }

    await this.villainService.UpdateAsync(value);
    await this.router.navigate(['/']);
  }

  public fileChange(event: any) {

    let fileList: FileList = event.target.files;

    if (fileList.length <= 0) {
      this.replaceImage = false;
      return;
    }

    let reader = new FileReader();
    reader.readAsDataURL(fileList[0]);
    reader.onload = () => {
      this.base64Image = reader.result as string;
      this.replaceImage = true;
      this.imageFileName = fileList[0].name;
    }
  }
}
