# Mingley Dating App — Backend API

## Tech Stack
- .NET 8 Web API — Clean Architecture
- Entity Framework Core 8 (Code First)
- SQL Server (LocalDB for dev)
- JWT Authentication + BCrypt
- Serilog logging
- Swagger UI

## Project Structure
```
src/
├── Mingley.Domain/         # Entities only — no dependencies
│   ├── Common/BaseEntity   # Audit + soft delete
│   └── Entities/           # User, Match, Chat, Message, etc.
├── Mingley.Application/    # DTOs, Interfaces, AutoMapper
├── Mingley.Infrastructure/ # EF Core, Services, Seed Data
└── Mingley.API/            # Controllers, Middleware, Program.cs
```

## Setup & Run

### Prerequisites
- .NET 8 SDK
- SQL Server LocalDB (included with Visual Studio)

### Steps
```powershell
# 1. Clone / extract project
cd MingleyAPI

# 2. Run migrations & seed data
dotnet ef migrations add InitialCreate --project src\Mingley.Infrastructure --startup-project src\Mingley.API
dotnet ef database update --project src\Mingley.Infrastructure --startup-project src\Mingley.API

# 3. Run the API
dotnet run --project src\Mingley.API
```

### API runs at
- API: http://localhost:7001
- Swagger: http://localhost:7001/swagger

---

## Demo Credentials (seeded automatically)
| Email | Password | Gender | Role |
|---|---|---|---|
| admin@mingley.app | Admin@123456 | male | admin |
| priya@demo.com | Admin@123456 | female | user |
| rahul@demo.com | Admin@123456 | male | user |
| arjun@demo.com | Admin@123456 | male (premium) | user |

---

## Complete API Reference

### Auth Endpoints
| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| POST | /v1/auth/register | Register new user | No |
| POST | /v1/auth/verify-otp | Verify OTP after register | No |
| POST | /v1/auth/resend-otp | Resend OTP | No |
| POST | /v1/auth/login | Login with email/phone | No |
| POST | /v1/auth/refresh-token | Get new access token | No |
| POST | /v1/auth/logout | Logout | Yes |
| POST | /v1/auth/forgot-password | Request password reset OTP | No |
| POST | /v1/auth/reset-password | Reset with OTP | No |

### User Endpoints
| Method | Endpoint | Description |
|---|---|---|
| GET | /v1/users/me | Get my full profile |
| PUT | /v1/users/me | Update name/bio/gender/DOB/avatar |
| PUT | /v1/users/me/interests | Update interests array |
| PUT | /v1/users/me/preferences | Update filter preferences |
| PUT | /v1/users/me/location | Update lat/lng/city/country |
| POST | /v1/users/me/images | Add photo URL |
| DELETE | /v1/users/me/images/{id} | Remove photo |
| GET | /v1/users/{id} | View another user's profile |
| POST | /v1/users/{id}/block | Block a user |
| DELETE | /v1/users/{id}/block | Unblock a user |
| GET | /v1/users/blocked | Get my blocked list |

### Discover Endpoints
| Method | Endpoint | Description |
|---|---|---|
| GET | /v1/discover | Get discovery feed (paginated) |
| POST | /v1/discover/swipe | Swipe like/dislike/superlike |
| GET | /v1/matches | Get all matches |
| DELETE | /v1/matches/{matchId} | Unmatch someone |

### Chat Endpoints
| Method | Endpoint | Description |
|---|---|---|
| GET | /v1/chats | Get all chats |
| GET | /v1/chats/{id}/messages | Get messages (paginated) |
| POST | /v1/chats/{id}/messages | Send message (deducts coins) |
| PUT | /v1/chats/{id}/read | Mark as read |
| DELETE | /v1/chats/{id}/messages/{msgId} | Delete message |
| GET | /v1/chats/{id}/quota | Get remaining message quota |

### Wallet Endpoints
| Method | Endpoint | Description |
|---|---|---|
| GET | /v1/wallet/balance | Get coin balance |
| GET | /v1/wallet/packages | Get coin packages |
| GET | /v1/wallet/transactions | Get transaction history |
| POST | /v1/wallet/deposit | Submit UTR deposit request |
| POST | /v1/wallet/withdraw | Submit withdrawal (female only) |

### Subscription Endpoints
| Method | Endpoint | Description |
|---|---|---|
| GET | /v1/subscriptions/plans | Get all plans (Silver/Gold/Platinum) |
| GET | /v1/subscriptions/status | Get my current subscription |
| POST | /v1/subscriptions/subscribe | Subscribe to a plan |
| POST | /v1/subscriptions/{id}/cancel | Cancel subscription |

---

## Response Format
Every endpoint returns:
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Success",
  "data": { ... }
}
```

---

## Coin Economy (matches frontend exactly)
| Action | Cost |
|---|---|
| Male sends message | 10 coins (5 if premium) |
| Female first 3 messages | Free |
| Female after 3 messages | 5 coins each |
| Superlike | 50 coins |
| Video call | 2 coins/second |

---

## Insomnia Setup
1. Open Insomnia
2. Click **Import** → select `Mingley_Insomnia_Collection.json`
3. In the **Base Environment** set `token` after logging in
4. All requests are pre-configured and ready to test

---

## Frontend Integration Notes
- All fields are nullable — frontend never crashes on missing data
- `devOtp` is returned in register response (Development only)
- All GUIDs are returned as strings for JavaScript compatibility
- `isMine` in messages: compare `senderId` with your userId from login
- Coin deduction happens server-side — frontend just shows `newBalance` from response
