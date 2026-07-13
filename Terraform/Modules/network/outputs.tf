output "private_subnets_app_ids" {
  value = [
    for subnet_name, subnet in var.var.subnets :
    aws_subnet.subnets[subnet_name].id
    if subnet.type == "private" && subnet.tier == "app"
  ]
}

output "private_subnets_db_ids" {
  value = [
    for subnet_name, subnet in var.var.subnets :
    aws_subnet.subnets[subnet_name].id
    if subnet.type == "private" && subnet.tier == "db"
  ]
}


output "target_group_arn" {
  value = aws_lb_target_group.ecommerce-tg.arn
}

output "ecs_sg_id" {
  value = aws_security_group.ecommerce-sg["ecommerce-ecs-sg"].id
}

output "db_sg_id" {
  value = aws_security_group.ecommerce-sg["ecommerce-db-sg"].id
}
