module "network" {
  source = "./network"

}


module "ecs" {
  source = "./ECS"

  ecs_tasks_subnets = module.network.private_subnets_app_ids

  target_group_arn = module.network.target_group_arn

  ecs_sg_id= module.network.ecs_sg
}

module "database" {
  source = "./Database"

  private_subnets_db_ids = module.network.private_subnets_db_ids

  db_sg_id=module.network.db_sg_id
}
