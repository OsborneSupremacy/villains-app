module "lambda-villain-image-upload" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  gateway_rest_api_id                      = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                      = aws_api_gateway_resource.image-resource.id
  gateway_http_method                      = "POST"
  gateway_http_operation_name              = ""
  gateway_method_request_validator_id      = ""
  gateway_method_request_parameters        = {}
  include_404_response                     = false
  good_response_model_name                 = ""
  good_response_model_description          = ""
  good_response_model_schema_file_location = ""
  function_description                     = "Upload a villain's image"
  function_handler                         = "Villains.Lambda.Image.Upload::Villains.Lambda.Image.Upload.Function::FunctionHandler"
  function_memory_size                     = 512
  function_name                            = "villain-image-upload"
  function_project_directory               = "../../src/Villains.Lambda.Image.Upload/src/Villains.Lambda.Image.Upload"
}

resource "aws_iam_role_policy_attachment" "villain-image-upload-exec-role-attachment-lambda-execute" {
  role       = module.lambda-villain-image-upload.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}