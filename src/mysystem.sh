docker build -t mysystem .
docker run --name myapi  -d -p 19121:19121 --restart=always -v /var/log:/log mysystem
