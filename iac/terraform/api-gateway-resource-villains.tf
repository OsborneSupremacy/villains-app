resource "aws_api_gateway_resource" "villains-resource" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  parent_id   = aws_api_gateway_resource.root-resource.id
  path_part   = "villains"
}

module "gateway-options-response-villains" {
  source = "./modules/gateway-options-response"
  gateway_rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id = aws_api_gateway_resource.villains-resource.id
}