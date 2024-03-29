module "lambda-villain-create" {
  source                                            = "./modules/lambda"
  common_tags                                       = local.common_tags
  environment_variables                             = local.common_environment_variables
  gateway_rest_api_id                               = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                               = aws_api_gateway_resource.villain-resource.id
  gateway_http_method                               = "POST"
  gateway_http_operation_name                       = "CreateVillain"
  gateway_method_request_parameters                 = {}
  gateway_method_request_model_name                 = "CreateVillainRequestV2"
  gateway_method_request_model_description          = "A request to create a new villain."
  gateway_method_request_model_schema_file_location = "../../src/Villains.Library/Schema/villain-create-request.schema.json"
  include_404_response                              = false
  good_response_model_name                          = "CreateVillainResponseV2"
  good_response_model_description                   = "A response to a request to create a new villain."
  good_response_model_schema_file_location          = "../../src/Villains.Library/Schema/villain-create-response.schema.json"
  function_description                              = "Create a villain"
  function_memory_size                              = 512
  function_name                                     = "villain-create"
  function_net_class                                = "CreateVillain"
  deployment_package_filename                       = data.archive_file.lambda_function.output_path
  deployment_package_source_code_hash               = data.archive_file.lambda_function.output_base64sha256
}

resource "aws_iam_role_policy_attachment" "villain-create-exec-role-attachment-dynamodb-full" {
  role       = module.lambda-villain-create.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villain-create-exec-role-attachment-dynamodb-execution" {
  role       = module.lambda-villain-create.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}