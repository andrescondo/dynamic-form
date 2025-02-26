## 1. descargar imagen de docker
docker pull mcr.microsoft.com/mssql/server

## 2. Iniciar contenedor

``` docker-compose
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server
    container_name: sqlserver
    environment:
      ACCEPT_EULA: 'Y'
      SA_PASSWORD: 'YourPassword@123'
    ports:
      - '1433:1433'
    volumes:
      - sqlserverdata:/var/opt/mssql

volumes:
  sqlserverdata:

```

## 3. Hacer correr la imagen:
**docker-compose up -d**

Nota: si se quiere detener, usar: **docker-compose down**