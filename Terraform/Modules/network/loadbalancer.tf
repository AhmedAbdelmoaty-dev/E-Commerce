resource "aws_lb" "ecommerce-alb" {
  name               = "ecommerce-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.ecommerce-sg["ecommerce-alb-sg"].id]
  subnets            = [for subnet_name, subnet in aws_subnet.subnets :
                           subnet.id 
                           if subnet.type == "public"
                       ]
  enable_deletion_protection = false

  tags = {
    Name = "ecommerce-alb"
  }
}


resource "aws_lb_target_group" "ecommerce-tg" {
  name     = "ecommerce-tg"
  port     = 8080
  protocol = "HTTP"
  vpc_id   = aws_vpc.E-Commerce_VPC.id
  target_type = "ip"    

  health_check {
    path                = "/health"
    interval            = 30
    healthy_threshold   = 3
    unhealthy_threshold = 3
  }
}

resource "aws_lb_target_group_attachment" "test" {
  target_group_arn = aws_lb_target_group.ecommerce-tg.arn
  target_id        = aws_instance.test.id // ecs instance ids
  port             = 8080
}

resource "aws_lb_listener" "front_end" {
  load_balancer_arn = aws_lb.ecommerce-alb.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.ecommerce-tg.arn
  }
}