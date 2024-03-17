module "lambda-villain-image-get" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  function_description       = "Get a villain's image"
  function_handler           = "Villains.Lambda.Image.Get::Villains.Lambda.Image.Get.Function::FunctionHandler"
  function_memory_size       = 2048
  function_name              = "villain-image-get"
  function_project_directory = "../../src/Villains.Lambda.Image.Get/src/Villains.Lambda.Image.Get"
}

resource "aws_iam_role_policy_attachment" "villain-image-get-exec-role-attachment-lambda-execute" {
  role       = module.lambda-villain-image-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}

resource "aws_api_gateway_method" "villain-image-get-method" {
  rest_api_id   = aws_api_gateway_rest_api.villains-gateway.id
  resource_id   = aws_api_gateway_resource.image-resource.id
  http_method   = "GET"
  authorization = "NONE"
}