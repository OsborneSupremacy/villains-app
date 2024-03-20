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
  rest_api_id          = aws_api_gateway_rest_api.villains-gateway.id
  resource_id          = aws_api_gateway_resource.image-resource.id
  http_method          = "GET"
  authorization        = "NONE"
  operation_name       = "GetVillainImage"
  request_validator_id = aws_api_gateway_request_validator.villain-image-get-request-validator.id
  request_parameters = {
    "method.request.querystring.imageName" = true
  }
}

resource "aws_api_gateway_request_validator" "villain-image-get-request-validator" {
  name                        = "villain-image-get-request-validator"
  rest_api_id                 = aws_api_gateway_rest_api.villains-gateway.id
  validate_request_body       = false
  validate_request_parameters = true
}

resource "aws_api_gateway_integration" "villain-image-get-integration" {
  rest_api_id             = aws_api_gateway_rest_api.villains-gateway.id
  resource_id             = aws_api_gateway_resource.image-resource.id
  http_method             = aws_api_gateway_method.villain-image-get-method.http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = module.lambda-villain-image-get.lambda_function_invoke_arn
  content_handling        = "CONVERT_TO_TEXT"
}

resource "aws_api_gateway_model" "image-get-response" {
  rest_api_id  = aws_api_gateway_rest_api.villains-gateway.id
  name         = "ImageGetResponse"
  description  = "The response model for the GetVillainImage method."
  content_type = "application/json"
  schema = jsonencode({
    "$schema" : "http://json-schema.org/draft-04/schema#",
    "type" : "object",
    "properties" : {
      "exists" : {
        "type" : "boolean",
        "description" : "Whether or not the image exists."
      },
      "imageSrc" : {
        "type" : "string",
        "description" : "The base64 encoded image file, including the `data:image` prefix (with mime type), so that it can be used as an image src value."
      },
      "fileName" : {
        "type" : "string",
        "description" : "The name of the image file."
      }
    },
    "required" : ["exists", "imageSrc", "fileName"]
    }
  )
}

resource "aws_api_gateway_method_response" "villain_image_get_response" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  resource_id = aws_api_gateway_resource.image-resource.id
  http_method = aws_api_gateway_method.villain-image-get-method.http_method
  status_code = "200"
  response_models = {
    "application/json" = aws_api_gateway_model.image-get-response.name
  }
}

resource "aws_api_gateway_method_response" "villain_image_get_404_response" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  resource_id = aws_api_gateway_resource.image-resource.id
  http_method = aws_api_gateway_method.villain-image-get-method.http_method
  status_code = "404"
}