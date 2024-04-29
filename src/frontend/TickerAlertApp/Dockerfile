FROM node:20 as builder

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build-production

FROM nginx:alpine

COPY --from=builder /app/dist/ticker-alert-app/browser /usr/share/nginx/html

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

# Command to run the container
CMD ["nginx", "-g", "daemon off;"]