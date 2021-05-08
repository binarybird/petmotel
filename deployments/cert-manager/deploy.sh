#helm install \
#  cert-manager jetstack/cert-manager \
#  --namespace cert-manager \
#  --create-namespace \
#  --version v1.3.1 \
kubectl create namespace cert-manager
kubectl apply -f https://github.com/jetstack/cert-manager/releases/download/v1.3.1/cert-manager.yaml
watch kubectl get po,svc -A
kubectl apply -f lets-encrypt-staging-ca-cluster-issuer.yml
kubectl apply -f self-signed-ca-cluster-issuer.yml
kubectl apply -f rabbit-mq-cert.yml
kubectl apply -f https://raw.githubusercontent.com/jetstack/cert-manager-csi/master/deploy/cert-manager-csi-driver.yaml
