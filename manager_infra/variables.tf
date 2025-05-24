variable "yc_token" {
  description = "IAM-токен для доступа к Yandex Cloud"
  type        = string
}

variable "yc_cloud_id" {
  description = "ID облака"
  type        = string
}

variable "yc_folder_id" {
  description = "ID каталога (folder)"
  type        = string
}

variable "pg_password" {
  description = "Пароль администратора PostgreSQL"
  type        = string
  sensitive   = true
}
