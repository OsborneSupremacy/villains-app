provider "aws" {
  region = "us-east-1"
}

terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
    }
  }

  backend "s3" {
    bucket         = "bro-tfstate"
    dynamodb_table = "tf-lock"
    key            = "villains/live"
    region         = "us-east-1"
  }
}