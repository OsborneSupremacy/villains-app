module "lambda-villain-get" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  gateway_rest_api_id                      = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                      = aws_api_gateway_resource.villain-resource.id
  gateway_http_method                      = "GET"
  gateway_http_operation_name              = ""
  gateway_method_request_validator_id      = ""
  gateway_method_request_parameters        = {}
  include_404_response                     = true
  good_response_model_name                 = ""
  good_response_model_description          = ""
  good_response_model_schema_file_location = ""
  function_description                     = "Get a villain"
  function_handler                         = "Villains.Lambda.Villain.Get::Villains.Lambda.Villain.Get.Function::FunctionHandler"
  function_memory_size                     = 512
  function_name                            = "villain-get"
  function_project_directory               = "../../src/Villains.Lambda.Villain.Get/src/Villains.Lambda.Villain.Get"
}

resource "aws_iam_role_policy_attachment" "villain-get-exec-role-attachment-dynamodb-full" {
  role       = module.lambda-villain-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villain-get-exec-role-attachment-dynamodb-execution" {
  role       = module.lambda-villain-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}