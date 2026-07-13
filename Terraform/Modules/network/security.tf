resource aws_security_group "ecommerce-sg" {

    for_each = var.security_groups
    name=each.value.name
    description=each.value.description
    vpc_id = aws_vpc.E-Commerce_VPC.id

    tags = {
        Name = each.key
    }
}

resource "aws_security_group_rule" "ecommerce-db-sg-ingress" {
  description       = "Allow inbound traffic on port 3306 from the ECS security group"
  type              = "ingress"
  from_port         = 3306
  to_port           =3306
  protocol          = "tcp"
  cidr_blocks       = [aws_vpc.E-Commerce_VPC.cidr_block]
  security_group_id = aws_security_group.ecommerce-sg["ecommerce-db-sg"].id

  source_security_group_id = aws_security_group.ecommerce-sg["ecommerce-ecs-sg"].id
}

resource "aws_security_group_rule" "ecommerce-ecs-sg-ingress" {
  description       = "Allow inbound traffic on port 8080 from the ALB security group"
  type              = "ingress"
  from_port         = 8080
  to_port           = 8080
  protocol          = "tcp"
  cidr_blocks       = [aws_vpc.E-Commerce_VPC.cidr_block]
  security_group_id = aws_security_group.ecommerce-sg["ecommerce-ecs-sg"].id

  source_security_group_id = aws_security_group.ecommerce-sg["ecommerce-alb-sg"].id
}

resource "aws_security_group_rule" "ecommerce-alb-sg-ingress" {
  description       = "Allow inbound traffic on port 80 from anywhere"
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = aws_security_group.ecommerce-sg["ecommerce-alb-sg"].id

}

resource "aws_security_group_rule" "ecommerce-alb-sg-egress" {
  description       = "Allow all outbound traffic"
  type              = "egress"
  from_port         = 0
  to_port           = 0
  protocol          = "-1"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = aws_security_group.ecommerce-sg["ecommerce-alb-sg"].id
}

