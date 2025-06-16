import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { Dashboard } from './components/Dashboard';
import { AdminDashboard } from './components/AdminDashboard';
import { AdminTrainers } from './components/AdminTrainers';
import { AdminMembers } from './components/AdminMembers';
import { LoginForm } from './components/LoginForm';
import { AuthProvider, useAuth } from './context/authContext';
import { Trainers } from './components/Trainers';

// Private Route Component
const PrivateRoute = ({ children }: { children: React.ReactNode }) => {
  const { user, loading } = useAuth();
  
  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-gray-600 text-lg">Loading...</div>
      </div>
    );
  }
  
  if (!user) {
    return <Navigate to="/login" replace />;
  }
  
  return <>{children}</>;
};

// Dashboard Router Component - shows correct dashboard based on user role
const DashboardRouter = () => {
  const { user } = useAuth();
  
  if (user?.UserRole === 'Admin') {
    return <AdminDashboard />;
  }
  
  return <Dashboard />;
};

// App Routes Component
function AppRoutes() {
  const { user, loading } = useAuth();

  // Show loading while checking authentication
  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-gray-600 text-lg">Loading...</div>
      </div>
    );
  }

  return (
    <Routes>
      <Route path="/login" element={<LoginForm />} />
      <Route 
        path="/" 
        element={
          user ? <Navigate to="/dashboard" replace /> : <Navigate to="/login" replace />
        } 
      />
      <Route 
        path="/dashboard" 
        element={
          <PrivateRoute>
            <DashboardRouter />
          </PrivateRoute>
        } 
      />
      <Route 
        path="/trainers" 
        element={
          <Trainers />
        } 
      />
      <Route 
        path="/admin/trainers" 
        element={
          <PrivateRoute>
            <AdminTrainers />
          </PrivateRoute>
        } 
      />
      <Route 
        path="/admin/members" 
        element={
          <PrivateRoute>
            <AdminMembers />
          </PrivateRoute>
        } 
      />
    </Routes>
  );
}

function App() {
  return (
    <Router>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </Router>
  );
}

export default App;
