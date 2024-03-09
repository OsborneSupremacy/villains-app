export interface ImageGetResponse {
  // A boolean value indicating whether the image file exists.
  exists: boolean;

  // The base64 encoded image file, including the `data:image` prefix (with mime type),
  // so that it can be used as an image src value.
  imageSrc: string;

  // The name of the image file.
  fileName: string;
}
