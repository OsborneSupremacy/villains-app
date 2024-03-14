resource "aws_s3_bucket" "villains-images" {
  bucket = "villains-images"
  tags = merge(
    local.common_tags,
    {
      Name = "villains-images"
    }
  )
}

resource "aws_s3_bucket_versioning" "villains-images-versioning" {
  bucket = aws_s3_bucket.villains-images.id
  versioning_configuration {
    status = "Disabled"
  }
}