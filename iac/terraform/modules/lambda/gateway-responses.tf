resource "aws_api_gateway_method_response" "get_404_response" {
  rest_api_id = var.gateway_rest_api_id
  resource_id = var.gateway_resource_id
  http_method = var.gateway_http_method
  status_code = "404"
  count       = var.include_404_response ? 1 : 0
}