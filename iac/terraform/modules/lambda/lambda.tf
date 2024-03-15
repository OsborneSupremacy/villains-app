resource "aws_lambda_function" "lambda" {

  function_name    = var.function_name
  description      = var.function_description
  handler          = var.function_handler
  runtime          = "dotnet8"
  architectures    = ["arm64"]
  memory_size      = var.function_memory_size
  filename         = data.archive_file.lambda_function.output_path
  source_code_hash = data.archive_file.lambda_function.output_base64sha256
  role             = aws_iam_role.exec-role.arn
  environment {
    variables = var.environment_variables
  }

  tags = merge(
    var.common_tags,
    {
      Name = var.function_name
    }
  )

  depends_on = [null_resource.build]
}