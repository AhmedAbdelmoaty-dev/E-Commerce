resource "aws_subnet" "subnets" {
  vpc_id     = aws_vpc.E-Commerce_VPC.id
  for_each   = var.subnets
  cidr_block = each.value.cidr_block
  availability_zone = each.value.availability_zone
   
    tags = {
        Name = each.key
        Type = each.value.type
    }
}