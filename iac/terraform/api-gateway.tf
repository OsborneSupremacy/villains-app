resource "aws_api_gateway_rest_api" "villains-gateway" {
  name        = "villains-gateway"
  description = "API Gateway for the Villains App"
  endpoint_configuration {
    types = ["REGIONAL"]
  }
  tags = merge(
    local.common_tags,
    {
      Name = "villains-gateway"
    }
  )
  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Effect" : "Allow",
        "Principal" : {
          "AWS" : "*"
        }
        "Action" : "execute-api:Invoke",
        "Resource" : "arn:aws:execute-api:us-east-1:${data.aws_caller_identity.current.account_id}:*/*/*"
        "Condition" : {
          "IpAddress" : {
            "aws:SourceIp" : data.http.ipify.response_body
          }
        }
      }
    ]
  })
}

resource "aws_api_gateway_deployment" "default" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
}

resource "aws_api_gateway_stage" "live-stage" {
  stage_name    = "live"
  rest_api_id   = aws_api_gateway_rest_api.villains-gateway.id
  deployment_id = aws_api_gateway_deployment.default.id
}

resource "aws_api_gateway_resource" "root-resource" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
  parent_id   = ""
  path_part   = ""
}