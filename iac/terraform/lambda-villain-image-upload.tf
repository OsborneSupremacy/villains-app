module "lambda-villain-image-upload" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  function_description       = "Upload a villain's image"
  function_handler           = "Villains.Lambda.Image.Upload::Villains.Lambda.Image.Upload.Function::FunctionHandler"
  function_memory_size       = 512
  function_name              = "villain-image-upload"
  function_project_directory = "../../src/Villains.Lambda.Image.Upload/src/Villains.Lambda.Image.Upload"
}

resource "aws_iam_role_policy_attachment" "villain-image-upload-exec-role-attachment-lambda-execute" {
  role       = module.lambda-villain-image-upload.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}