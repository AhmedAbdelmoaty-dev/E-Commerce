resource "aws_route_table" "public_route_table" {
  vpc_id = aws_vpc.E-Commerce_VPC.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.E-Commerce_IGW.id
  }

  tags = {
    Name = "ecommerce-public-rt"
  }
}

resource "aws_route_table_association" "public_route_table_association" {
  for_each = {
    for subnet_name, subnet in var.subnets :
    subnet_name => subnet
    if subnet.type == "public"
  }

  subnet_id      = each.subnet.id
  route_table_id = aws_route_table.public_route_table.id
}


resource "aws_route_table" "private_route_table" {
  vpc_id = aws_vpc.E-Commerce_VPC.id

  route {
    cidr_block     = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.ecommerce-nat.id
  }

  tags = {
    Name = "ecommerce-private-rt"
  }
}

resource "aws_route_table_association" "private_route_table_association" {
  for_each = {
    for subnet_name, subnet in var.subnets :
    subnet_name => subnet
    if subnet.type == "private"
  }

  subnet_id      = each.subnet.id
  route_table_id = aws_route_table.private_route_table.id
}
