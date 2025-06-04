# 📚 Book Collab SaaS

A collaborative platform for indie book authors to write, share, and chat in real time. Built with a modern stack using **C#** (ASP.NET Core), **React**, **Redis**, **Firebase**, and **WebSockets**.

---

## 🧰 Tech Stack

| Layer        | Technology            |
|-------------|------------------------|
| Backend      | C# (ASP.NET Core)     |
| Frontend     | React                 |
| Realtime     | WebSockets            |
| Caching      | Redis                 |
| Notifications / Auth | Firebase     |
| Containerization | Docker, Docker Compose |

---

## 🛠️ Features

- ✍️ Share books, chapters, and paragraphs with collaborators.
- 💬 Real-time chat via WebSockets per chapter or project.
- 🔐 Auth and notifications powered by Firebase.
- ⚡ Redis used for session and collaboration caching.
- 🖥️ SaaS architecture ready for deployment.

---

## 🔧 Local Setup

### 1. Prerequisites

Make sure you have the following installed:

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### 2. Run the project

```bash
docker compose up -d
```

### 3. Architecture Workflow

```mermaid
graph TD
    A[Frontend (React)] -->|REST & WebSocket| B[Backend API (ASP.NET Core)]
    B -->|Auth / Messaging| C[Firebase]
    B -->|Cache / PubSub| D[Redis]
    A -->|WebSocket| B
    subgraph Docker Network
        B
        D
    end

