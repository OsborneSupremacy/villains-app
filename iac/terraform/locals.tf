locals {
  common_tags = {
    Environment = "live"
    Application = "villains"
    ManagedBy   = "terraform"
    Owner       = "villains@osbornesupremacy.com"
  }
}