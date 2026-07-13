subnets= {
    prod-public-subnet-az1a={
        cidr_block = "10.16.1.0/24"
        availability_zone = "us-east-1a"
        type = "public"
        tier ="public"
    }

    prod-private-app-subnet-az1a={
        cidr_block = "10.16.2.0/24"
        availability_zone = "us-east-1a"
        type = "private"
        tier = "app"
        }
    
    prod-private-db-subnet-az1a={
        cidr_block = "10.16.3.0/24"
        availability_zone = "us-east-1a"
        type = "private"
        tier ="db"
        }
    
    prod-public-subnet-az1b={
        cidr_block = "10.16.4.0/24"
        availability_zone = "us-east-1b"
        type = "public"
        tier = "public"
    }

    prod-private-app-subnet-az1b={
        cidr_block = "10.16.5.0/24"
        availability_zone = "us-east-1b"
        type = "private"
        tier = "app"
        }

    prod-private-db-subnet-az1b={
        cidr_block = "10.16.6.0/24"
        availability_zone = "us-east-1b"
        type = "private"
        tier = "db"
    }

    prod-public-subnet-az1c={
        cidr_block = "10.16.7.0/24"
        availability_zone = "us-east-1c"
        type = "public"
        tier = "public"
    }

    prod-private-app-subnet-az1c={
        cidr_block = "10.16.8.0/24"
        availability_zone = "us-east-1c"
        type = "private"
        tier = "app"
        }

        prod-private-db-subnet-az1c={
        cidr_block = "10.16.9.0/24"
        availability_zone = "us-east-1c"
        type = "private"
        tier = "db"
        }

}



security_groups = {
    ecommerce-db-sg={
        name        = "ecommerce-db-sg"
        description = "Security group for the database"
    }

    ecommerce-alb-sg={
        name        = "ecommerce-ALB-SG"
        description = "Security group for the ALB"
    }

    ecommerce-ecs-sg={
        name        = "ecommerce-ecs-sg"
        description = "Security group for the ECS tasks"
    }

}
