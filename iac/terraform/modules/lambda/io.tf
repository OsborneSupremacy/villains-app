# inputs

variable "function_name" {
  type = string
}

variable "function_description" {
  type = string
}

variable "function_handler" {
  type = string
}

variable "function_memory_size" {
  type = number
}

variable "function_timeout" {
  type = number
  default = 30
}

variable "function_project_directory" {
  type = string
}

variable "common_tags" {
  type = map(string)
}

variable "environment_variables" {
  type = map(string)
}

locals {
  build_command     = <<EOT
      cd ${var.function_project_directory}
      dotnet publish -o bin/publish -c Release --framework "net8.0" /p:GenerateRuntimeConfigurationFiles=true --runtime linux-arm64 --self-contained false
    EOT
  build_output_path = "${var.function_project_directory}/bin/publish"
  publish_zip_path  = "${var.function_project_directory}/bin/lambda_function.zip"
}

# outputs

output "function_exec_role" {
  value = aws_iam_role.exec-role
}