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

  constructor(
    id: string,
    name: string,
    powers: string,
    imageName: string,
    mimeType: string,
    buttonText: string,
    saying: string
  ) {
    this.id = id;
    this.name = name;
    this.powers = powers;
    this.imageName = imageName;
    this.mimeType = mimeType;
    this.buttonText = buttonText;
    this.saying = saying;
    this.flipped = false;
  }
}
// Path: villains-client/src/app/services/villain.service.ts
