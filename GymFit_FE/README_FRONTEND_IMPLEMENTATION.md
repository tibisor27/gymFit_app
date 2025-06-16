# 🏋️ GymFit Frontend - Clean Authentication Implementation

This is a **beginner-friendly React + TypeScript frontend** that demonstrates how to integrate with a RESTful backend API. The implementation focuses on **authentication flow** and **protected routes**.

## 🎯 Key Backend Endpoints Connected

```typescript
// 🔑 Authentication Endpoints
POST /auth/login          // Login with email/password → returns JWT token
POST /auth/register       // Register new user account
GET /auth/status          // Check backend health

// 👥 Protected User Endpoints  
GET /users               // Get all users (requires authentication)
GET /users/{id}          // Get specific user details
```

## 📁 Project Structure

```
src/
├── types/
│   └── api.ts              // TypeScript interfaces matching backend DTOs
├── services/
│   └── api.ts              // API client with axios, handles HTTP requests
├── contexts/
│   └── AuthContext.tsx     // Global auth state management with React Context
├── components/
│   ├── Login.tsx           // Login form component
│   ├── Register.tsx        // User registration form
│   ├── Dashboard.tsx       // Protected dashboard showing user data
│   └── PrivateRoute.tsx    // Route protection wrapper component
└── App.tsx                 // Main app with routing setup
```

## 🔄 Authentication Flow Explained

### 1. **AuthContext** - The Heart of Authentication
```typescript
// Located: src/contexts/AuthContext.tsx
// Purpose: Manages login state globally across the app

const { user, token, login, register, logout } = useAuth();
```

**What it does:**
- Stores user data and JWT token in localStorage
- Provides login/register/logout functions to all components  
- Automatically adds JWT token to API requests via axios interceptors
- Checks for existing token on app startup (persistent login)

### 2. **Login Component** - User Authentication
```typescript
// Located: src/components/Login.tsx
// Route: /login

await login(email, password);  // Calls backend /auth/login
// On success: saves token + redirects to dashboard
```

**Form fields:**
- Email (required, validated)
- Password (required, min 6 chars)

**Flow:**
1. User submits form
2. Calls `AuthContext.login()` 
3. API request to `POST /auth/login`
4. Backend returns JWT token
5. Token saved to localStorage
6. User redirected to dashboard

### 3. **Register Component** - New User Creation
```typescript
// Located: src/components/Register.tsx  
// Route: /register

await register(userData);  // Calls backend /auth/register
// On success: auto-login + redirect to dashboard
```

**Form fields:**
- Full Name (required, 2-100 chars)
- Email (required, valid format)
- Password (required, min 8 chars)
- Phone Number (required, Romanian format)
- Account Type (Member/Trainer/Admin)
- Date of Birth (required, min 16 years old)

### 4. **PrivateRoute** - Route Protection
```typescript
// Located: src/components/PrivateRoute.tsx
// Purpose: Protects routes that require authentication

<PrivateRoute>
  <Dashboard />  {/* Only shows if user is logged in */}
</PrivateRoute>
```

**Logic:**
- Checks if user exists in AuthContext
- Shows loading spinner while checking auth status
- Redirects to `/login` if no user found
- Renders protected component if authenticated

### 5. **Dashboard** - Protected Content
```typescript
// Located: src/components/Dashboard.tsx
// Route: /dashboard (protected)

const users = await userApi.getAllUsers();  // Requires JWT token
```

**What it displays:**
- Current user profile information
- Total user count from API
- Authentication status
- List of all users (fetched from protected endpoint)

## 🔧 API Service Layer

```typescript
// Located: src/services/api.ts
// Purpose: Centralized API communication with error handling
```

**Features:**
- Axios instance with base URL configuration
- Automatic JWT token injection via interceptors
- Consistent error handling across all requests
- TypeScript interfaces for request/response data

**Example usage:**
```typescript
// Login API call
const response = await authApi.login({ email, password });

// Protected API call (token added automatically)
const users = await userApi.getAllUsers();
```

## 🚀 How to Run

1. **Start the backend** (make sure it's running on `https://localhost:7190`)

2. **Install frontend dependencies:**
```bash
cd GymFit_FE
npm install
```

3. **Start the development server:**
```bash
npm run dev
```

4. **Open your browser** and navigate to `http://localhost:5173`

## 🎓 What Junior Developers Learn

### **State Management:**
- How React Context provides global state without Redux
- Managing authentication state across components
- Persistent login with localStorage

### **API Integration:**
- Making HTTP requests with axios
- Handling authentication tokens
- Error handling and user feedback
- Protected vs public API endpoints

### **Routing & Protection:**
- React Router setup and navigation
- Protecting routes that require authentication
- Redirecting users based on auth status

### **Form Handling:**
- Controlled components with TypeScript
- Form validation and error display
- Async form submission with loading states

### **TypeScript Benefits:**
- Type safety for API requests/responses
- Interface definitions for data structures
- Better development experience with autocomplete

## 🔒 Security Features

1. **JWT Token Storage:** Tokens stored in localStorage (consider httpOnly cookies for production)
2. **Automatic Token Injection:** Axios interceptors add Authorization header
3. **Route Protection:** Unauthenticated users can't access dashboard
4. **Input Validation:** Frontend validation + backend validation
5. **Error Handling:** Secure error messages without exposing sensitive data

## 🎯 Key Learning Points

**For beginners, this implementation shows:**

✅ **How frontend communicates with backend APIs**
✅ **How authentication tokens work in web applications**  
✅ **How to protect routes and manage user sessions**
✅ **How to structure a React app with TypeScript**
✅ **How to handle loading states and error messages**
✅ **How to make the UI responsive and user-friendly**

## 🚧 Next Steps (Production Improvements)

- Implement token refresh mechanism
- Add proper error boundaries for React components  
- Use httpOnly cookies instead of localStorage for tokens
- Add input sanitization and more robust validation
- Implement proper loading skeletons
- Add unit tests for components and API functions
- Set up environment variables for API URLs

---

**This implementation prioritizes clarity and learning over advanced features, making it perfect for understanding the fundamental concepts of frontend-backend integration!** 🎓 