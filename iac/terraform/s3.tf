resource "aws_s3_bucket" "villains-images" {
  bucket = "villains-images"
  tags = {
    Name        = "villains-images"
    Environment = "live"
    Application = "villains"
    ManagedBy   = "terraform"
    Owner       = "villains@osbornesupremacy.com"
  }
}

resource "aws_s3_bucket_versioning" "versioning_example" {
  bucket = aws_s3_bucket.villains-images.id
  versioning_configuration {
    status = "Disabled"
  }
}