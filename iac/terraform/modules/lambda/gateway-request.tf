resource "aws_api_gateway_request_validator" "request_validator" {
  name                        = "${var.function_name}-request-validator"
  rest_api_id                 = var.gateway_rest_api_id
  validate_request_body       = false // make this dynamic later
  validate_request_parameters = length(var.gateway_method_request_parameters) > 0 ? true : false
}