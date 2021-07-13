FROM node:14.17.3-alpine3.14 AS builder
COPY ./ClientApp/ ./ClientApp
WORKDIR /ClientApp
RUN npm install
RUN $(npm bin)/ng build

FROM nginx:1.20.1-alpine
COPY --from=builder /ClientApp/dist/ClientApp/ /usr/share/nginx/html
