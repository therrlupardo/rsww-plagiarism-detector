FROM node:12-alpine AS nodejs
WORKDIR /app

COPY client/ .

RUN npm install -g @angular/cli &&\
    npm install &&\
    ng build --prod --output-path=./dist/out


FROM nginx:alpine

COPY nginx/ /etc/nginx/
COPY --from=nodejs /app/dist/out/ /usr/share/nginx/html
