variable vpc_cidr {
  description = "The CIDR block for the VPC"
  type        = string
  default     = "10.16.0.0/16"
}

variable subnets{
    description = "A list of CIDR blocks for the subnets"
        type = map(object({
        cidr_block = string
        availability_zone = string
        type = string
        tier = string
    }))
}

variable security_groups{
    type= map(object({
        name        = string
        description = string
    }))
}


