cd src\Pantokrator.Gateway
docker build -t pantokrator/pantokrator-gateway:latest .

cd src\Pantokrator.Order.Api
docker build -t pantokrator-orderapi:latest .

docker run -d -p 80:5000  -v pantokrator/pantokrator-gateway:latest