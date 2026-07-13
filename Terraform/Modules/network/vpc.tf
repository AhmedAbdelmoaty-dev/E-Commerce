resource "aws_vpc" "E-Commerce_VPC" {
  cidr_block = "10.16.0.0/16"

  enable_dns_support = true
  enable_dns_hostnames = true

  tags = {
    Name = "main-vpc"
  }
}

resource "aws_internet_gateway" "igw" {
  vpc_id = aws_vpc.E-Commerce_VPC.id

  tags = {
    Name = "main-igw"
  }
}

resource "aws_eip" "nat_ip" {
  domain = "vpc"

  tags = {
    Name = "nat-eip"
  }
}



resource "aws_nat_gateway" "ecommerce-nat" {
  allocation_id = aws_eip.nat_ip.id
  subnet_id     = aws_subnet.subnets["prod-public-subnet-az1a"].id

  tags = {
    Name = "ecommerce-nat"
  }
}

