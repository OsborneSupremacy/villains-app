// villain.model.ts

export class Villain {
  id: string;
  name: string;
  powers: string;
  imageName: string;
  mimeType: string;
  buttonText: string;
  saying: string;
  flipped: boolean;
  base64Image: string;
  imageLoaded: boolean;
  insertedOn: Date;

  constructor(
    id: string,
    name: string,
    powers: string,
    imageName: string,
    mimeType: string,
    buttonText: string,
    saying: string,
    insertedOn: Date
  ) {
    this.id = id;
    this.name = name;
    this.powers = powers;
    this.imageName = imageName;
    this.mimeType = mimeType;
    this.buttonText = buttonText;
    this.saying = saying;
    this.flipped = false;
    this.base64Image = ``;
    this.imageLoaded = false;
    this.insertedOn = insertedOn;
  }
}
// Path: villains-client/src/app/services/villain.service.ts
