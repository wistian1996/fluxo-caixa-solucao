FROM node:23.11.0-alpine3.20 As builder
#FROM node:14.20.1-alpine As builder
WORKDIR /app
COPY package*.json package-lock.json /app/

RUN npm install

COPY ./ /app/

# ARG PROFILE
# ENV PROFILE $PROFILE
# RUN echo "Environment: ${PROFILE}"
# RUN npm run build-${PROFILE} -- --output-path=./dist/out

RUN npm run build -- --configuration production --output-path=./dist/out

FROM nginx:1.23.2-alpine

COPY --from=builder /app/dist/out/browser /usr/share/nginx/html
COPY --from=builder /app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

RUN echo "for mainFileName in /usr/share/nginx/html/main*.js ;\
            do \
              envsubst '\$API_URL \$ENVIRONMENT ' < \$mainFileName > main.tmp ;\
              mv main.tmp \${mainFileName} ;\
            done \
            && nginx -g 'daemon off;'" > run.sh

ENTRYPOINT ["sh", "run.sh"]
