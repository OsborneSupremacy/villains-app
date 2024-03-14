resource "aws_dynamodb_table" "villains" {
  name         = "villains"
  billing_mode = "PAY_PER_REQUEST"
  tags = merge(
    local.common_tags,
    {
      Name = "villains"
    }
  )
  hash_key = "id"
}