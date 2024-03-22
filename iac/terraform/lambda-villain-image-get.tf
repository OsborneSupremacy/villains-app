module "lambda-villain-image-get" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  gateway_rest_api_id                 = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                 = aws_api_gateway_resource.image-resource.id
  gateway_http_method                 = "GET"
  gateway_http_operation_name         = "GetVillainImage"
  gateway_method_request_parameters = {
    "method.request.querystring.imageName" = true
  }
  include_404_response                     = true
  good_response_model_name                 = "ImageGetResponse"
  good_response_model_description          = "The response model for the GetVillainImage method."
  good_response_model_schema_file_location = "../../src/Villains.Library/Schema/image-get-response.schema.json"
  function_description                     = "Get a villain's image"
  function_memory_size                     = 2048
  function_name                            = "villain-image-get"
  function_net_class                       = "GetVillainImage"
  deployment_package_filename              = data.archive_file.lambda_function.output_path
  deployment_package_source_code_hash      = data.archive_file.lambda_function.output_base64sha256
}

resource "aws_iam_role_policy_attachment" "villain-image-get-exec-role-attachment-lambda-execute" {
  role       = module.lambda-villain-image-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}