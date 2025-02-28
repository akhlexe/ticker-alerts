FROM node:20 AS builder

WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build-production

FROM nginx:alpine

COPY --from=builder /app/dist/ticker-alert-app /usr/share/nginx/html

EXPOSE 80

# Command to run the container
CMD ["nginx", "-g", "daemon off;"]