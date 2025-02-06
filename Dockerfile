

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5001 
# Adiciona a porta de debug

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConsumerBTGService/ConsumerBTGService.csproj", "ConsumerBTGService/"]
RUN dotnet restore "ConsumerBTGService/ConsumerBTGService.csproj"
COPY . .
WORKDIR "/src/ConsumerBTGService"
RUN dotnet build "ConsumerBTGService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsumerBTGService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerBTGService.dll", "--urls", "http://*:80", "--environment", "Development"]

# ## Use uma imagem base apropriada
# FROM ubuntu:20.04 AS base

# # Defina variáveis de ambiente para MySQL
# ENV MYSQL_ROOT_PASSWORD=btgdbapp
# ENV MYSQL_DATABASE=btg

# # Atualize e instale dependências
# RUN apt-get update && apt-get install -y wget gnupg lsb-release curl


# # Instale MySQL
# #RUN apt-get install -y mysql-server
# RUN apt-get update && apt-get install -y --no-install-recommends mysql-server


# # Copie o script de inicialização do MySQL
# COPY btgdb.sql /docker-entrypoint-initdb.d/btgdb.sql

# # Instale RabbitMQ
# RUN apt-get install -y curl gnupg
# RUN curl -fsSL https://packages.erlang-solutions.com/ubuntu/erlang_solutions.asc | apt-key add -
# RUN echo "deb https://packages.erlang-solutions.com/ubuntu $(lsb_release -cs) contrib" | tee /etc/apt/sources.list.d/erlang.list
# RUN apt-get update
# RUN apt-get install -y erlang
# RUN curl -fsSL https://packagecloud.io/rabbitmq/rabbitmq-server/gpgkey | apt-key add -
# RUN echo "deb https://packagecloud.io/rabbitmq/rabbitmq-server/ubuntu/ $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/rabbitmq-server.list
# RUN apt-get update
# RUN apt-get install -y rabbitmq-server

# # Instale o SDK do .NET
# RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
# RUN dpkg -i packages-microsoft-prod.deb
# RUN apt-get update && apt-get install -y apt-transport-https && apt-get update && apt-get install -y dotnet-sdk-8.0

# # Etapa de build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY ["ConsumerBTGService/ConsumerBTGService.csproj", "ConsumerBTGService/"]
# RUN dotnet restore "ConsumerBTGService/ConsumerBTGService.csproj"
# COPY . .
# WORKDIR "/src/ConsumerBTGService"
# RUN dotnet build "ConsumerBTGService.csproj" -c Release -o /app/build

# # Etapa de publish
# FROM build AS publish
# RUN dotnet publish "ConsumerBTGService.csproj" -c Release -o /app/publish

# # Etapa final
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .

# # Exponha portas necessárias
# EXPOSE 80
# EXPOSE 5001
# EXPOSE 3306
# EXPOSE 5672
# EXPOSE 15673

# # Inicie os serviços MySQL, RabbitMQ e a aplicação .NET
# CMD service mysql start && service rabbitmq-server start && dotnet ConsumerBTGService.dll --urls "http://*:80" --environment "Development"

# # Use uma imagem base apropriada
# FROM ubuntu:20.04 AS base
# # Atualiza os pacotes e instala o MySQL Server corretamente
# RUN apt-get update && apt-get install -y mysql-server
# # Defina variáveis de ambiente para MySQL
# ENV MYSQL_ROOT_PASSWORD=btgdbapp
# ENV MYSQL_DATABASE=btg

# # Atualize e instale dependências
# RUN apt-get update && apt-get install -y wget gnupg lsb-release curl

# # Copie o script de inicialização do MySQL
# COPY btgdb.sql /docker-entrypoint-initdb.d/btgdb.sql

# # Use a imagem RabbitMQ pré-pronta
# FROM rabbitmq:3-management AS rabbitmq




# Instale RabbitMQ
# RUN apt-get install -y curl gnupg
# RUN curl -fsSL https://packages.erlang-solutions.com/ubuntu/erlang_solutions.asc | apt-key add -
# RUN echo "deb https://packages.erlang-solutions.com/ubuntu $(lsb_release -cs) contrib" | tee /etc/apt/sources.list.d/erlang.list
# RUN apt-get update
# RUN apt-get install -y erlang
# RUN curl -fsSL https://packagecloud.io/rabbitmq/rabbitmq-server/gpgkey | apt-key add -
# RUN echo "deb https://packagecloud.io/rabbitmq/rabbitmq-server/ubuntu/ $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/rabbitmq-server.list
# RUN apt-get update
# RUN apt-get install -y rabbitmq-server

# Configure RabbitMQ como um serviço
# RUN ln -s /usr/sbin/rabbitmq-server /etc/init.d/rabbitmq-server

# Etapa de build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY ["ConsumerBTGService/ConsumerBTGService.csproj", "ConsumerBTGService/"]
# RUN dotnet restore "ConsumerBTGService/ConsumerBTGService.csproj"
# COPY . .
# WORKDIR "/src/ConsumerBTGService"
# RUN dotnet build "ConsumerBTGService.csproj" -c Release -o /app/build

# # Etapa de publish
# FROM build AS publish
# RUN dotnet publish "ConsumerBTGService.csproj" -c Release -o /app/publish

# Use a imagem MySQL pré-construída
# FROM mysql:8.0 AS mysql



# # Adicione o script de inicialização
# COPY btgdb.sql /docker-entrypoint-initdb.d/btgdb.sql

# # Etapa final
# FROM ubuntu:20.04 AS final
# COPY --from=mysql / /mysql
# COPY --from=publish /app/publish /app

# # Instale RabbitMQ e configure a inicialização
# RUN apt-get update && apt-get install -y rabbitmq-server
# COPY --from=rabbitmq /etc/rabbitmq/ /etc/rabbitmq/
# COPY --from=rabbitmq /var/lib/rabbitmq/ /var/lib/rabbitmq/



# # Exponha portas necessárias
# EXPOSE 80
# EXPOSE 5001
# EXPOSE 3306
# EXPOSE 5672
# EXPOSE 15673

# # Inicie os serviços MySQL, RabbitMQ e a aplicação .NET
# CMD service mysql start & rabbitmq-server && dotnet /app/ConsumerBTGService.dll --urls "http://*:80" --environment "Development"
