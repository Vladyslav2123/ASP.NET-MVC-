# ASP.NET MVC User Management System

This is a comprehensive ASP.NET MVC application that provides user management functionality with geolocation features. The application is built using ASP.NET Core MVC and includes various features for managing users, skills, positions, and map markers.

## Features

### User Management
- User authentication and authorization using ASP.NET Identity
- Role-based access control with predefined roles (Admin and User)
- User profile management with image upload capabilities
- Custom user information storage with skills and positions

### Map Integration
- Interactive map functionality using Leaflet.js
- Ability to add and view map markers
- User-specific marker management
- Geolocation features with latitude and longitude support

### Skills and Positions
- Skill management system
- Position/role management
- User-skill association tracking
- Hierarchical organization structure

### Security Features
- Built-in authentication and authorization
- CSRF protection with anti-forgery tokens
- Secure cookie handling
- Custom login/logout paths

## Technical Stack

### Backend
- ASP.NET Core MVC
- Entity Framework Core
- SQLite Database
- ASP.NET Identity

### Frontend
- Bootstrap for responsive design
- JavaScript/jQuery for dynamic interactions
- Leaflet.js for map functionality
- SweetAlert2 for improved user notifications

## Database Schema

The application uses the following main entities:
- Users (AspNetUsers)
- Roles (AspNetRoles)
- UserInfo
- Positions
- Skills
- UserSkills
- MapMarkers
- UserMarkers
- Images

## Setup and Configuration

1. Prerequisites:
   - .NET 6.0 or later
   - SQLite

2. Database Setup:
   ```bash
   dotnet ef database update
   ```

3. Configuration:
   - Update connection string in `appsettings.json` if needed
   - Default connection uses SQLite

4. Running the Application:
   ```bash
   dotnet run
   ```

## Security Configuration

The application uses the following security settings:

```csharp
// Authentication settings
options.SignIn.RequireConfirmedPhoneNumber = false;
options.SignIn.RequireConfirmedEmail = false;
options.SignIn.RequireConfirmedAccount = false;

// Password requirements
options.Password.RequiredLength = 3;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequireDigit = false;
options.Password.RequireLowercase = false;
options.Password.RequireUppercase = false;
```

> Note: These are development settings. For production, it's recommended to strengthen these security requirements.

## API Endpoints

The application includes several API endpoints for:
- User management
- Map marker operations
- Skill management
- Position management

## Project Structure

```
├── Areas/
│   └── Auth/                 # Authentication related views and controllers
├── Infrastructure/
│   ├── DbContextApp/        # Database context and configuration
│   └── Repositories/        # Repository pattern implementation
├── Interfaces/              # Interface definitions
├── Models/                  # Domain models
├── Services/               # Business logic services
└── wwwroot/               # Static files (JS, CSS, etc.)
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project includes Bootstrap under the MIT License. See the [LICENSE](wwwroot/lib/bootstrap/LICENSE) file for details.

## Support

For support, please open an issue in the repository or contact the maintainers.
