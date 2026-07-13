resource "aws_db_subnet_group" "main" {

  name = "ecommerce-db-subnets"

  subnet_ids =  var.private_subnets_db_ids
  

  tags = {
    Name = "ecommerce-subnet-group"
  }
}

resource "aws_db_instance" "mysql" {

  identifier = "ecommerce-db"

  engine = "mysql"
  engine_version = "8.0"

  instance_class = "db.t3.micro"

  allocated_storage = 20

  db_name = "E-Commerce"

  username = var.db_username
  password = var.db_password

  db_subnet_group_name = aws_db_subnet_group.main.name

  vpc_security_group_ids = [var.db_sg_id]

  publicly_accessible = false

  multi_az = false

  skip_final_snapshot = true

  deletion_protection = false
}