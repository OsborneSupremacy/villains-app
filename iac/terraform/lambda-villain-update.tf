module "lambda-villain-update" {
  source                                            = "./modules/lambda"
  common_tags                                       = local.common_tags
  environment_variables                             = local.common_environment_variables
  gateway_rest_api_id                               = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                               = aws_api_gateway_resource.villain-resource.id
  gateway_http_method                               = "PUT"
  gateway_http_operation_name                       = "UpdateVillain"
  gateway_method_request_parameters                 = {}
  gateway_method_request_model_name                 = "UpdateVillainRequestV2"
  gateway_method_request_model_description          = "A request to create a new villain."
  gateway_method_request_model_schema_file_location = "../../src/Villains.Library/Schema/villain-update-request.schema.json"
  include_404_response                              = true
  good_response_model_name                          = ""
  good_response_model_description                   = ""
  good_response_model_schema_file_location          = ""
  function_description                              = "Update a villain"
  function_memory_size                              = 512
  function_name                                     = "villain-update"
  function_net_class                                = "UpdateVillain"
  deployment_package_filename                       = data.archive_file.lambda_function.output_path
  deployment_package_source_code_hash               = data.archive_file.lambda_function.output_base64sha256
}

resource "aws_iam_role_policy_attachment" "villain-update-exec-role-attachment-dynamodb-full" {
  role       = module.lambda-villain-update.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villain-update-exec-role-attachment-dynamodb-execution" {
  role       = module.lambda-villain-update.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}