module "lambda-villain-image-upload" {
  source      = "./modules/lambda"
  common_tags = local.common_tags
  environment_variables = {
    "TABLE_NAME"        = aws_dynamodb_table.villains.name
    "BUCKET_NAME"       = aws_s3_bucket.villains-images.bucket
    "MAX_PAYLOAD_BYTES" = 6291556
  }
  gateway_rest_api_id                               = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id                               = aws_api_gateway_resource.image-resource.id
  gateway_http_method                               = "POST"
  gateway_http_operation_name                       = "UploadImage"
  gateway_method_request_parameters                 = {}
  gateway_method_request_model_name                 = "UploadImageRequestV2"
  gateway_method_request_model_description          = "A request to upload an image. A 2.0 model."
  gateway_method_request_model_schema_file_location = "../../src/Villains.Library/Schema/image-upload-request.schema.json"
  include_404_response                              = false
  good_response_model_name                          = "ImageUploadResponseV2"
  good_response_model_description                   = "The response from an image upload. A 2.0 model."
  good_response_model_schema_file_location          = "../../src/Villains.Library/Schema/image-upload-response.schema.json"
  function_description                              = "Upload a villain's image"
  function_memory_size                              = 512
  function_name                                     = "villain-image-upload"
  function_net_class                                = "UploadImage"
  deployment_package_filename                       = data.archive_file.lambda_function.output_path
  deployment_package_source_code_hash               = data.archive_file.lambda_function.output_base64sha256
}

resource "aws_iam_role_policy_attachment" "villain-image-upload-exec-role-attachment-lambda-execute" {
  role       = module.lambda-villain-image-upload.function_exec_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}