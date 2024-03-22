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
  good_response_model_schema_file_location = "../../src/Villains.Lambda.Image.Get/src/Villains.Lambda.Image.Get/schema/image-get-response.schema.json"
  function_description                     = "Get a villain's image"
  function_handler                         = "Villains.Lambda.Image.Get::Villains.Lambda.Image.Get.Function::FunctionHandler"
  function_memory_size                     = 2048
  function_name                            = "villain-image-get"
  function_project_directory               = "../../src/Villains.Lambda.Image.Get/src/Villains.Lambda.Image.Get"
}

resource "aws_iam_role_policy_attachment" "villain-image-get-exec-role-attachment-lambda-execute" {
  role       = module.lambda-villain-image-get.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}

resource "aws_api_gateway_integration" "villain-image-get-integration" {
  rest_api_id             = aws_api_gateway_rest_api.villains-gateway.id
  resource_id             = aws_api_gateway_resource.image-resource.id
  http_method             = "GET"
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = module.lambda-villain-image-get.lambda_function_invoke_arn
  content_handling        = "CONVERT_TO_TEXT"
}