resource "aws_ecs_service" "ecommerce-fargate-service" {

  name = "ecommerce-fargate-service"

  cluster = aws_ecs_cluster.ecommerce-cluster.id

  desired_count = 2


  launch_type = "Fargate"


  network_configuration {

    subnets = var.ecs_tasks_subnets
    security_groups = [var.ecs_sg_id]

  }


  load_balancer {

    target_group_arn = var.target_group_arn

    container_name = "ecommerce-container"

    container_port = 8080

  }

  lifecycle {

    ignore_changes = [
      task_definition
    ]

  }

}