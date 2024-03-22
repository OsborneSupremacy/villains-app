module "lambda-villain-create" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  gateway_rest_api_id        = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id        = aws_api_gateway_resource.image-resource.id
  gateway_http_method        = aws_api_gateway_method.villain-image-get-method.http_method
  function_description       = "Create a villain"
  function_handler           = "Villains.Lambda.Villain.Create::Villains.Lambda.Villain.Create.Function::FunctionHandler"
  function_memory_size       = 512
  function_name              = "villain-create"
  function_project_directory = "../../src/Villains.Lambda.Create/src/Villains.Lambda.Villain.Create"
}

resource "aws_iam_role_policy_attachment" "villain-create-exec-role-attachment-dynamodb-full" {
  role       = module.lambda-villain-create.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villain-create-exec-role-attachment-dynamodb-execution" {
  role       = module.lambda-villain-create.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}