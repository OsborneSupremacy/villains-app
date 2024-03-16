resource "aws_api_gateway_rest_api" "villains-gateway" {
  name        = "villains-gateway"
  description = "API Gateway for the Villians App"
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
    "Version": "2012-10-17",
    "Statement": [
      {
        "Effect": "Allow",
        "Principal": {
          "AWS": "*"
        }
        "Action": "execute-api:Invoke",
        "Resource": "arn:aws:execute-api:us-east-1:${data.aws_caller_identity.current.account_id}:*/*/*"
        "Condition": {
          "IpAddress": {
            "aws:SourceIp": data.http.ipify.response_body
          }
        }
      }
    ]
  })
}

resource "aws_api_gateway_deployment" "default" {
  rest_api_id = aws_api_gateway_rest_api.villains-gateway.id
}

/*
resource "aws_iam_role" "cloudwatch-role" {
  name = "villains-gateway-cloudwatch-role"

  tags = merge(
    local.common_tags,
    {
      Name = "villains-gateway-cloudwatch-role"
    }
  )

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Principal = {
          Service = "apigateway.amazonaws.com"
        }
        Effect = "Allow"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "cloudwatch-role-attachment" {
  role       = aws_iam_role.cloudwatch-role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonAPIGatewayPushToCloudWatchLogs"
}
*/