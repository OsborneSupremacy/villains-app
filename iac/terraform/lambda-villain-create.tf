module "lambda" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  function_description       = "Create a villain"
  function_handler           = "Villains.Lambda.Villain.Create::Villains.Lambda.Villain.Create.Function::FunctionHandler"
  function_memory_size       = 512
  function_name              = "villain-create"
  function_project_directory = "../../src/Villains.Lambda.Create/src/Villains.Lambda.Villain.Create"
}