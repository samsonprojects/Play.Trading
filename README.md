# Play.Trading

play around with microservices

## Build docker image

```linux
version="1.0.0"
export GH_OWNER="samsonprojects"
export GH_PAT="[PAT HERE]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t play.trading:$version .
```

## Run the docker image and connect it with the same network as the mongodb and rabbitmq

```linux
docker run -it --rm -p 5006:5006 --name Trading -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq --network src_default play.trading:$version
```
