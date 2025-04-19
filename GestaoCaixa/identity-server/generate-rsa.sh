# chave privada
openssl genrsa -out private.key 2048

# chave pública
openssl rsa -in private.key -pubout -out public.key