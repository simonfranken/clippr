# Setup

## NGINX + SSL

### Generate a self-signed a cerificate

```sh
cd .docker/
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout ./nginx/certs/selfsigned.key \
  -out ./nginx/certs/selfsigned.crt \
  -subj "/CN=clippr_reverse_proxy"
cp nginx/certs/selfsigned.crt ../backend/.
```
