resource "aws_api_gateway_resource" "villain-resource" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  parent_id   = aws_api_gateway_resource.root-resource.id
  path_part   = "villain"
}

module "gateway-options-response-villain" {
  source = "./modules/gateway-options-response"
  gateway_rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  gateway_resource_id = aws_api_gateway_resource.villain-resource.id
}