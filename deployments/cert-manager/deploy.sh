#helm install \
#  cert-manager jetstack/cert-manager \
#  --namespace cert-manager \
#  --create-namespace \
#  --version v1.3.1 \
kubectl create namespace cert-manager
kubectl apply -f https://github.com/jetstack/cert-manager/releases/download/v1.3.1/cert-manager.yaml
sleep 30
kubectl get pods --namespace cert-manager
kubectl apply -f lets-encrypt-staging-ca-cluster-issuer.yml
