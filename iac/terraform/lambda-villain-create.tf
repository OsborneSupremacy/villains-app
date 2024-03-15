module "lambda_function" {
  source = "terraform-aws-modules/lambda/aws"

  function_name                     = "villain-create"
  description                       = "Create a villain"
  handler                           = "Villains.Lambda.Villain.Create::Villains.Lambda.Villain.Create.Function::FunctionHandler"
  runtime                           = "dotnet8"
  architectures                     = ["arm64"]
  memory_size                       = 512
  source_path                       = "../../src/Villains.Lambda.Create/src/Villains.Lambda.Villain.Create"
  create_role                       = false
  lambda_role                       = aws_iam_role.villain-create-exec-role.arn
  logging_log_group                 = "/aws/lambda/villain/create"

  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }

  tags = merge(
    local.common_tags,
    {
      Name = "villain-create"
    }
  )
}

resource "aws_iam_role" "villain-create-exec-role" {
  name = "villain-create-exec-role"

  tags = merge(
    local.common_tags,
    {
      Name = "villain-create-exec-role"
    }
  )

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
        Effect = "Allow"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "villain-create-exec-role-attachment-dynamodb-full" {
  role       = aws_iam_role.villain-create-exec-role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villain-create-exec-role-attachment-dynamodb-execution" {
  role       = aws_iam_role.villain-create-exec-role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}