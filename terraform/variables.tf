variable "aws_region" {
  description = "AWS region to deploy resources"
  type        = string
  default     = "us-east-1"
}

variable "aws_profile" {
  description = "AWS CLI profile name to use for authentication"
  type        = string
  default     = "default"
}

# Optional: Uncomment if you prefer direct credentials (not recommended)
# variable "aws_access_key" {
#   description = "AWS Access Key"
#   type        = string
#   sensitive   = true
# }
# 
# variable "aws_secret_key" {
#   description = "AWS Secret Key"
#   type        = string
#   sensitive   = true
# }

variable "project_name" {
  description = "Project name used for resource naming"
  type        = string
  default     = "ielts-platform"
}

variable "environment" {
  description = "Environment name (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "vpc_cidr" {
  description = "CIDR block for VPC"
  type        = string
  default     = "10.0.0.0/16"
}

variable "availability_zones" {
  description = "Availability zones for the region"
  type        = list(string)
  default     = ["us-east-1a", "us-east-1b"]
}

# Database variables
variable "db_name" {
  description = "PostgreSQL database name"
  type        = string
  default     = "ielts_platform_db"
}

variable "db_username" {
  description = "PostgreSQL database username (mock - replace later)"
  type        = string
  default     = "postgres_admin"
  sensitive   = true
}

variable "db_password" {
  description = "PostgreSQL database password (mock - replace later)"
  type        = string
  default     = "CHANGE_ME_STRONG_PASSWORD_123"
  sensitive   = true
}

variable "db_instance_class" {
  description = "RDS instance class"
  type        = string
  default     = "db.t3.micro"
}

# ECS variables
variable "api_container_port" {
  description = "Port for API container"
  type        = number
  default     = 5000
}

variable "web_container_port" {
  description = "Port for Web container"
  type        = number
  default     = 80
}

variable "api_cpu" {
  description = "CPU units for API task"
  type        = number
  default     = 256
}

variable "api_memory" {
  description = "Memory for API task (in MB)"
  type        = number
  default     = 512
}

variable "web_cpu" {
  description = "CPU units for Web task"
  type        = number
  default     = 256
}

variable "web_memory" {
  description = "Memory for Web task (in MB)"
  type        = number
  default     = 512
}

variable "api_desired_count" {
  description = "Desired number of API tasks"
  type        = number
  default     = 1
}

variable "web_desired_count" {
  description = "Desired number of Web tasks"
  type        = number
  default     = 1
}

variable "ecr_image_tag_mutability" {
  description = "Image tag mutability for ECR repositories"
  type        = string
  default     = "MUTABLE"
}

