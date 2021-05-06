kubectl apply -f "https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml"
sleep 30
kubectl apply -f tls-cluster.yml
sleep 30
username="$(kubectl get secret -n petmotel petmotel-mq-default-user -o jsonpath='{.data.username}' | base64 --decode)"
echo "username: $username"
password="$(kubectl get secret -n petmotel petmotel-mq-default-user -o jsonpath='{.data.password}' | base64 --decode)"
echo "password: $password"
service="$(kubectl get service -n petmotel petmotel-mq -o jsonpath='{.spec.clusterIP}')"
echo "ip: $service"
minikube service -n petmotel petmotel-mq
