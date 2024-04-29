FROM node:20 as builder

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build-production

FROM nginx:alpine

COPY --from=builder /app/dist/ticker-alert-app/browser /usr/share/nginx/html

EXPOSE 80

# Command to run the container
CMD ["nginx", "-g", "daemon off;"]