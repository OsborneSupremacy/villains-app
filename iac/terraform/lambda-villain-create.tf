locals {
  project_directory = "../../src/Villains.Lambda.Create/src/Villains.Lambda.Villain.Create"
  build_command     = <<EOT
      cd ${local.project_directory}
      dotnet publish -o bin/publish -c Release --framework "net8.0" /p:GenerateRuntimeConfigurationFiles=true --runtime linux-arm64 --self-contained false
    EOT
  build_output_path = "${local.project_directory}/bin/publish"
  publish_zip_path  = "${local.project_directory}/bin/lambda_function.zip"
}

resource "aws_lambda_function" "villain-create" {

  function_name    = "villain-create"
  description      = "Create a villain"
  handler          = "Villains.Lambda.Villain.Create::Villains.Lambda.Villain.Create.Function::FunctionHandler"
  runtime          = "dotnet8"
  architectures    = ["arm64"]
  memory_size      = 512
  filename         = data.archive_file.lambda_function.output_path
  source_code_hash = data.archive_file.lambda_function.output_base64sha256
  role             = aws_iam_role.villain-create-exec-role.arn
  environment {
    variables = {
      "TABLE_NAME"        = aws_dynamodb_table.villains.name
      "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
      "MAX_PAYLOAD_BYTES" = 6291556
    }
  }

  tags = merge(
    local.common_tags,
    {
      Name = "villain-create"
    }
  )

  depends_on = [null_resource.build]
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

resource "null_resource" "build" {
  triggers = {
    timestamp = timestamp()
  }

  provisioner "local-exec" {
    command = local.build_command
  }
}

data "archive_file" "lambda_function" {
  type        = "zip"
  source_dir  = local.build_output_path
  output_path = local.publish_zip_path
  depends_on  = [null_resource.build]
}