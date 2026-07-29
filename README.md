## Part K — Handoff Notes

### Secured Endpoints

| Endpoint | Method | Auth Required | Rule Enforced |
|---|---|---|---|
| `/api/gear/{id}/requests` | GET | JWT (any role) | Must be authenticated |
| `/api/requests/{id}/status` | PATCH | JWT | Caller must be gear owner or Admin |
| `/api/requests/{gearItemId}/maintenance` | PATCH | JWT | Caller must be gear owner or Admin |
| `/api/requests/{gearItemId}/retire` | PATCH | JWT | Admin only — owner attempting this gets 403 |
| `/api/auth/login` | POST | None | Issues JWT with Id, email, role claims |

### 401 vs 403

A frontend should treat these differently:

- **401 Unauthorized** — the request had no token or an invalid/expired token. Redirect the user to the login page and clear any stored credentials.
- **403 Forbidden** — the user is authenticated but does not have permission for this specific resource (e.g. trying to approve a rental on gear they don't own). Do not redirect to login — show an "access denied" message instead, as logging in again will not help.

### Known Limitations

- The idempotency key cache (Part B) is in-memory only — it resets on every restart and is not shared across multiple API instances. A distributed cache (Redis) would be needed for production.
- The overlap pre-flight check in `RentalService` is not atomic with the subsequent insert — two concurrent requests could both pass the check before either commits. The Part E `EXCLUDE` constraint is the hard guarantee at the database level.
- Seed data users have hardcoded bcrypt hashes — the hash was generated for `"password"` but is not re-validated on startup. If the hash is ever wrong, login will silently fail with 401.
- No refresh token flow — JWT tokens expire after 60 minutes with no way to renew without re-logging in.

### Running Locally

```bash
# Start the database
docker run --name gearshare-db -e POSTGRES_PASSWORD=devpass -p 5432:5432 -d postgres:16

# Apply migrations
dotnet ef database update

# Run the API
dotnet run

# Scalar UI
https://localhost:{port}/scalar/v1

# Health check
https://localhost:{port}/health
```

### Test Credentials

| Email | Password | Role |
|---|---|---|
| alice@example.com | password | Member |
| admin@example.com | password | Admin |