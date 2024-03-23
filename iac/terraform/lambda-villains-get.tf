module "lambda-villains-get" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  gateway_rest_api_id                               = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                               = aws_api_gateway_resource.villains-resource.id
  gateway_http_method                               = "GET"
  gateway_http_operation_name                       = "GetVillains"
  gateway_method_request_parameters                 = {}
  gateway_method_request_model_name                 = ""
  gateway_method_request_model_description          = ""
  gateway_method_request_model_schema_file_location = ""
  include_404_response                              = false
  good_response_model_name                          = "GetVillainsResponseV2"
  good_response_model_description                   = "A collection of villains. A 2.0 model."
  good_response_model_schema_file_location          = "../../src/Villains.Library/Schema/villains-get-response.schema.json"
  function_description                              = "Get villains"
  function_memory_size                              = 512
  function_name                                     = "villains-get"
  function_net_class                                = "GetVillains"
  deployment_package_filename                       = data.archive_file.lambda_function.output_path
  deployment_package_source_code_hash               = data.archive_file.lambda_function.output_base64sha256
}

resource "aws_iam_role_policy_attachment" "villains-get-exec-role-attachment-dynamodb-full" {
  role       = module.lambda-villains-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villains-get-exec-role-attachment-dynamodb-execution" {
  role       = module.lambda-villains-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}