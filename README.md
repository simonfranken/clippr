# clippr

clippr is a modern web tool that enables you to share a clipboard across multiple clients and devices in a secure and user-friendly way. You can upload text, small files, and images, which are saved to your user account. Anywhere you are signed in, you can quickly copy any saved content to your local clipboard with just a click.

## Features

- Share clipboard items (text, files, pictures) between devices and browsers
- Content is associated with your user account and available everywhere you sign in
- One-click copy to your local clipboard from any device
- Support for multiple authentication mechanisms (see below)
- Secure backend and API, designed for privacy and data safety

## Technology Stack

clippr is built with modern, robust technologies:

- **Frontend:** Built with Vue.js 3 and TypeScript for a fast, reactive user experience
- **Backend:** Developed in .NET 8, providing a robust and scalable server layer
- **Database:** Uses Entity Framework for database access and management
- **Authentication:**
  - ASP.NET Identity for core authentication
  - Supports authentication via email/password or OIDC providers (e.g., Google, Microsoft, etc.)
  - Issues JWT tokens for secure API access
  - App tokens for future client integrations, with long expiration support

For a more detailed overview of the system architecture and design choices, see the documentation in the respective implementation folders.

## Quick Start

To get started quickly with deploying clippr using Docker Compose, please see the comprehensive [Docker Deployment Guide](./hosting/docker/README.md).

## Repository Structure

- `/frontend` - Vue 3 + TypeScript single-page application
- `/backend` - .NET 8 backend API
- `/identityService` - Identity and authentication microservice
- `/hosting/docker` - All Docker Compose files and deployment-related configuration
- Other folders: Documentation, supporting scripts, and configuration files

## Documentation & Support

- [Docker Deployment Guide](./hosting/docker/README.md)
- Further documentation for each service/component can be found in the respective directories
- Issues and assistance: Please use the GitHub issue tracker for reporting bugs or requesting features

---

**Happy sharing with clippr!**
