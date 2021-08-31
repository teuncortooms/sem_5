FROM node:14.17.3-alpine3.14 AS builder
COPY ./AngularUI/ ./AngularUI
WORKDIR /AngularUI
RUN npm install
RUN $(npm bin)/ng build

FROM nginx:1.20.1-alpine
COPY --from=builder /AngularUI/dist/AngularUI/ /usr/share/nginx/html
