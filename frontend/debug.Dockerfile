FROM node:lts-slim@sha256:557e52a0fcb928ee113df7e1fb5d4f60c1341dbda53f55e3d815ca10807efdce

COPY package*.json .
RUN npm ci

COPY . .

EXPOSE 5173
ENTRYPOINT [ "npm", "run", "dev", "--", "--host" ]