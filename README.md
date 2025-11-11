# Task Management API

A complete .NET 8 Web API for managing tasks with SQLite database and Entity Framework Core.

## Features

- **Complete CRUD Operations**: Create, Read, Update, Delete tasks
- **Task Status Management**: NOT_STARTED, IN_PROGRESS, COMPLETED
- **Database**: SQLite with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI integration
- **CORS Support**: Configured for frontend integration
- **Input Validation**: Comprehensive validation and error handling
- **Health Check**: System health monitoring endpoint

## Quick Start

### Prerequisites

- .NET 8.0 SDK
- SQLite (automatically managed)

### Running the API

```bash
dotnet run
```

The API will be available at:
- **Server**: http://localhost:5234
- **Swagger UI**: http://localhost:5234
- **Health Check**: http://localhost:5234/health

### Database Setup

The database is automatically created and seeded with sample data on first run.

## API Endpoints

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/tasks` | Get all tasks (optional status filter) |
| GET | `/tasks?status=IN_PROGRESS` | Filter tasks by status |
| POST | `/tasks` | Create a new task |
| PUT | `/tasks/{id}` | Update an existing task |
| DELETE | `/tasks/{id}` | Delete a task |

### System

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Health check endpoint |

## Task Model

```json
{
  "id": 1,
  "title": "Task title (required, max 120 chars)",
  "description": "Task description (optional)",
  "status": "NOT_STARTED", // NOT_STARTED, IN_PROGRESS, COMPLETED
  "createdAt": "2025-11-11T06:28:06.375795Z",
  "updatedAt": "2025-11-11T06:28:06.375795Z"
}
```

## API Usage Examples

### Get All Tasks
```bash
curl http://localhost:5234/tasks
```

### Get Tasks by Status
```bash
curl "http://localhost:5234/tasks?status=IN_PROGRESS"
```

### Create New Task
```bash
curl -X POST http://localhost:5234/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "New Task",
    "description": "Task description",
    "status": "NOT_STARTED"
  }'
```

### Update Task
```bash
curl -X PUT http://localhost:5234/tasks/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Updated Task Title",
    "status": "IN_PROGRESS"
  }'
```

### Delete Task
```bash
curl -X DELETE http://localhost:5234/tasks/1
```

### Health Check
```bash
curl http://localhost:5234/health
```

## Project Structure

```
TaskApi/
├── Models/          # Data models
│   ├── TaskItem.cs
│   └── TaskStatus.cs
├── Data/            # Database context and seeding
│   ├── TaskDbContext.cs
│   └── DatabaseSeeder.cs
├── DTOs/            # Data Transfer Objects
│   └── TaskDtos.cs
├── Endpoints/       # API endpoint definitions
│   └── TasksEndpoints.cs
├── Program.cs       # Application entry point
└── .gitignore       # Git ignore rules
```

## Configuration

### CORS
The API is configured to allow requests from:
- http://localhost:3000 (React frontend)
- http://127.0.0.1:3000

### Database
- **Type**: SQLite
- **File**: `tasks.db` (auto-created)
- **Migrations**: Auto-applied on startup

## Sample Data

The database is automatically seeded with 6 sample tasks:
- 2 × NOT_STARTED tasks
- 2 × IN_PROGRESS tasks  
- 2 × COMPLETED tasks

## Development

### Build
```bash
dotnet build
```

### Run in Development
```bash
dotnet run
```

### Clean Build Files
Build artifacts are automatically ignored by `.gitignore`.

## Technology Stack

- **.NET 8 Web API**: Modern web framework
- **Entity Framework Core**: ORM for database operations
- **SQLite**: Lightweight database
- **Swagger/OpenAPI**: API documentation
- **Minimal APIs**: Clean and efficient endpoint definitions