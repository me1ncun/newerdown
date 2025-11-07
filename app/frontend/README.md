# NewerDown Frontend

## Translation Management

This project supports both local and cloud-based translation management via Locize.

Quick start:

- **Local translations**: Set `VITE_TRANSLATION_SOURCE=local` in `.env`
- **Locize**: Set `VITE_TRANSLATION_SOURCE=locize` in `.env`

---

# API Documentation

## Authentication

- **POST `/api/auth/signup`** — register a new user
- **POST `/api/auth/login`** — user login (returns access and refresh tokens)
- **POST `/api/auth/token/refresh`** — refresh tokens (creates new ones and invalidates old ones)
- **POST `/api/auth/change-password`** — change password

## User

- **GET `/api/users/me`** — get current user information
- **PATCH `/api/users/me`** — update user information
- **DELETE `/api/users/me`** — delete user account
- **POST `/api/users/me/upload-photo`** — upload user avatar
- **DELETE `/api/users/me/delete-photo`** — delete user avatar

## Monitor

- **POST `/api/monitors`** — Create a new monitor
- **GET `/api/monitors`** — Get all monitors of the current user
- **GET `/api/monitors/{id}`** — Get a specific monitor by ID
- **PUT `/api/monitors/{id}`** — Update monitor information
- **DELETE `/api/monitors/{id}`** — Delete a monitor by ID
- **POST `/api/monitors/{id}/pause`** — Pause a monitor (`IsActive = false`)
- **POST `/api/monitors/{id}/resume`** — Resume a monitor (`IsActive = true`)
- **POST `/api/monitors/import`** — Import monitors via a CSV file
- **GET `/api/monitors/{id}/export`** — Export a monitor by ID to a CSV file

## Incident and IncidentComment (GraphQL)

- **Query `incidents`** — Get all incidents
- **Query `incident`** — Get incident by ID
- **Mutation** `acknowledgeIncident` — Acknowledge incident by ID
- **Mutation** `commentIncident` — Add comment to incident by ID

Swagger API: https://app-newerdown.azurewebsites.net/swagger/index.html
Тут представлена документація по всіх доступних API-ендпоінтах, з можливістю тестування запитів.
