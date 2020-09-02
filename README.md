# Docker 部署.Net Core

官方文档：https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.0

1. 安装社区办docker：

```shell
yum install docker-ce
sudo docker run hello-world

sudo mkdir -p /etc/docker
sudo vi /etc/docker/daemon.json

sudo systemctl daemon-reload
systemctl restart docker
systemctl start docker.service
systemctl enable docker.service
```

更换为国内镜像源：

```
# 创建或修改 /etc/docker/daemon.json 文件，修改为如下形式
{
    "registry-mirrors" : [
    "https://registry.docker-cn.com",
    "https://docker.mirrors.ustc.edu.cn",
    "http://hub-mirror.c.163.com",
    "https://cr.console.aliyun.com/"
  ]
}
# 重启docker服务使配置生效
$ systemctl restart docker.service
```

2. 创建.Net Core 项目

新建.Net Core 项目，添加Docker支持：

```dockerfile
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
COPY . .
ENV LANG zh_CN.UTF-8
ENV LANGUAGE $LANG
ENV LC_ALL $LANG
ENV TZ=Asia/Shanghai
ENTRYPOINT ["dotnet", "LogGetter.dll"]
```

  dockerfile文件指令说明：

- FROM -指定所创建镜像的基础镜像
- WORKDIR-配置工作目录
- EXPOSE-声明镜像内服务监听的端口 （可以不写，因为我们具体映射的端口可以在运行的时候指定）
- COPY-复制内容到镜像  (. .代表当前目录)
- ENTRYPOINT-启动镜像的默认人口命令

发布项目，拷贝publish文件夹至CentOS，目录：app/publish

3. 部署项目

```shell
cd /app/publish
docker build -t mysystem .
docker run --name myapi  -d -p 19121:19121 --restart=always -v /home/logs:/logs mysystem
```



```
#查看挂掉的容器：
docker ps -a
#查看指定容器的日志：
docker logs b1d05f65856f
#进入docker容器：
docker exec -it containerID /bin/bash
#docker安装vim：
apt-get update
apt-get install vim
```

4. 常用命令：

```shell
docker build -t demotest .    构建 demotest镜像
docker images                      查看当前所有的镜像
docker inspect demotest     查看 运行容器的详情
docker ps                         查看当前运行的容器
docker ps -a                      查看当前所有的容器
docker stop demotest      停止运行demotest容器
docker start demotest     开启运行demotest容器
docker rm demotest     删除demotest容器
docker rmi demotest    删除demotest镜像
docker rm $(docker ps -aq)     删除所有容器
docker rmi $(docker images -q)   删除所有镜像
```

