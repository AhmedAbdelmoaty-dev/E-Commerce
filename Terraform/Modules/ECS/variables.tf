variable "ecs_tasks_subnets" {
  type = list(string)
}

variable "target_group_arn" {
  type=string
}

variable "ecs_sg_id" {
  type = string
}
