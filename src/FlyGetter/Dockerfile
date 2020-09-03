#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
COPY . .

#设置时区
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
#中文字符支持
RUN apt-get clean && apt-get -y update && apt-get install -y locales && locale-gen zh_CN.UTF-8
ENV LANG='zh_CN.UTF-8' LANGUAGE='zh_CN.UTF-8' LC_ALL='zh_CN.UTF-8'
#执行命令
ENTRYPOINT ["dotnet", "FlyGetter.dll"]
