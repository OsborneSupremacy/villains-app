module "lambda-villain-get" {
  source                      = "./modules/lambda"
  common_tags                 = local.common_tags
  environment_variables       = local.common_environment_variables
  gateway_rest_api_id         = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id         = aws_api_gateway_resource.villain-resource.id
  gateway_http_method         = "GET"
  gateway_http_operation_name = "GetVillain"
  gateway_method_request_parameters = {
    "method.request.querystring.id" = true
  }
  gateway_method_request_model_name                 = ""
  gateway_method_request_model_description          = ""
  gateway_method_request_model_schema_file_location = ""
  include_404_response                              = true
  good_response_model_name                          = "GetVillainResponseV2"
  good_response_model_description                   = "A response to a successful get villain request."
  good_response_model_schema_file_location          = "../../src/Villains.Library/Schema/villain-get-response.schema.json"
  function_description                              = "Get a villain"
  function_memory_size                              = 512
  function_name                                     = "villain-get"
  function_net_class                                = "GetVillain"
  deployment_package_filename                       = data.archive_file.lambda_function.output_path
  deployment_package_source_code_hash               = data.archive_file.lambda_function.output_base64sha256
}

resource "aws_iam_role_policy_attachment" "villain-get-exec-role-attachment-dynamodb-full" {
  role       = module.lambda-villain-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonDynamoDBFullAccess"
}

resource "aws_iam_role_policy_attachment" "villain-get-exec-role-attachment-dynamodb-execution" {
  role       = module.lambda-villain-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaDynamoDBExecutionRole"
}