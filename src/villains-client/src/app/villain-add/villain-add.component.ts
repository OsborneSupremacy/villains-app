import {Component} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Villain} from "../models/villain";
import {Router, RouterLink} from "@angular/router";
import {VillainService} from "../services/villain.service";
import {ImageService} from "../services/image.service";

@Component({
  selector: 'app-villain-add',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './villain-add.component.html',
  styleUrl: './villain-add.component.scss'
})
export class VillainAddComponent {

  public villainFg: FormGroup;

  villain: Villain | undefined;

  imageFileName: string = '';
  addImage: boolean = false;
  base64Image: string = '';

  constructor(
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
      image: [null, [Validators.required]]
    });
  }

  public async onSubmit({value, valid}: { value: Villain, valid: boolean }) {
    if (!valid)
      return;

    const imageUploadResult = await this.imageService
      .AddAsync(this.imageFileName, this.base64Image);
    value.imageName = imageUploadResult.fileName;

    await this.villainService.AddAsync(value);
    await this.router.navigate(['/']);
  }

  public fileChange(event: any) {

    let fileList: FileList = event.target.files;

    if (fileList.length <= 0) {
      this.addImage = false;
      return;
    }

    let reader = new FileReader();
    reader.readAsDataURL(fileList[0]);
    reader.onload = () => {
      this.base64Image = reader.result as string;
      this.addImage = true;
      this.imageFileName = fileList[0].name;
    }
  }
}
