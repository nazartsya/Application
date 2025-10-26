FROM node:22-alpine

WORKDIR /app

COPY event-management-application-client/package*.json ./
RUN npm install -g @angular/cli
RUN npm install

COPY event-management-application-client/ .

EXPOSE 4200

CMD ["ng", "serve", "--host", "0.0.0.0"]
