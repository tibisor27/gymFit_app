import React, { useEffect } from 'react';
import { useAuth } from '../context/authContext';
import { useNavigate } from 'react-router-dom';

// Dashboard component - displays user data
export const Dashboard: React.FC = () => {
  const { user, logout, loading, error} = useAuth();
  console.log(user);
  const navigate = useNavigate();

  // Redirect to login if no user after loading
  useEffect(() => {
    if (!loading && !user) {
      navigate('/login');
    }
  }, [user, loading, navigate]);

  const handleLogout = () => {
    logout();
    navigate('/login'); // Navigate after logout
  };

  // Show loading state
  if (loading) return (
    <div className="min-h-screen bg-white flex items-center justify-center">
      <div className="text-gray-600 text-xl">Loading...</div>
    </div>
  );

  // Show error state
  if (error) return (
    <div className="min-h-screen bg-white flex items-center justify-center">
      <div className="text-red-600 text-xl">Error: {error}</div>
    </div>
  );

  // Show not authenticated state
  if (!user) return (
    <div className="min-h-screen bg-white flex items-center justify-center">
      <div className="text-gray-600 text-xl">Not authenticated!</div>
    </div>
  );

  // Helper function for role styling
  const getRoleInfo = (role: string) => {
    switch (role) {
      case 'Admin': return { color: 'text-purple-600', bg: 'bg-purple-50' };
      case 'Member': return { color: 'text-blue-600', bg: 'bg-blue-50' };
      case 'Trainer': return { color: 'text-green-600', bg: 'bg-green-50' };
      default: return { color: 'text-gray-600', bg: 'bg-gray-50' };
    }
  };

  const roleInfo = user ? getRoleInfo(user.UserRole) : { color: 'text-gray-600', bg: 'bg-gray-50' };

  return (
    <div className="min-h-screen bg-white">
      {/* Header */}
      <div className="bg-white border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex justify-between items-center">
            <div className="flex items-center space-x-4">
              <h1 className="text-3xl font-bold text-gray-900">
                {user?.UserRole === 'Trainer' ? 'Trainer Dashboard' : 'Dashboard'}
              </h1>
            </div>
            
            <div className="flex items-center space-x-4">
              <button
                onClick={() => navigate('/trainers')}
                className="bg-gray-900 hover:bg-gray-800 text-white px-4 py-2 rounded text-sm transition-colors"
              >
                Trainers
              </button>
              <span className="text-sm text-gray-600">
                <span className="font-semibold">{user.Name}</span>
              </span>
              <button
                onClick={handleLogout}
                className="bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded text-sm transition-colors"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto px-6 py-8">
        {/* User Info Card */}
        <div className="bg-white border border-gray-200 rounded-lg p-8 mb-8">
          <div className="flex items-start space-x-6">
            <div className={`w-16 h-16 rounded-full ${roleInfo.bg} flex items-center justify-center`}>
              <span className={`text-2xl font-bold ${roleInfo.color}`}>
                {user.Name.charAt(0).toUpperCase()}
              </span>
            </div>
            <div>
              <h2 className="text-2xl font-bold text-gray-900 mb-2">
                Welcome, {user.Name}
              </h2>
              <div className="space-y-1">
                <p className="text-gray-600">
                  <span className="font-medium">Email:</span> {user.Email}
                </p>
                <p className="text-gray-600">
                  <span className="font-medium">Role:</span> 
                  <span className={`ml-2 px-3 py-1 rounded-full text-sm font-medium ${roleInfo.bg} ${roleInfo.color}`}>
                    {user.UserRole}
                  </span>
                </p>
                {user.PhoneNumber && (
                  <p className="text-gray-600">
                    <span className="font-medium">Phone:</span> {user.PhoneNumber}
                  </p>
                )}
              </div>
            </div>
          </div>
        </div>

        {/* Quick Actions */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div className="bg-white border border-gray-200 rounded-lg p-6 hover:border-gray-300 transition-colors cursor-pointer">
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Schedule</h3>
            <p className="text-gray-600 text-sm">View your workout schedule and upcoming sessions</p>
          </div>

          <div className="bg-white border border-gray-200 rounded-lg p-6 hover:border-gray-300 transition-colors cursor-pointer">
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Workouts</h3>
            <p className="text-gray-600 text-sm">Browse workout plans and track your progress</p>
          </div>

          <div className="bg-white border border-gray-200 rounded-lg p-6 hover:border-gray-300 transition-colors cursor-pointer">
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Community</h3>
            <p className="text-gray-600 text-sm">Connect with other gym members and trainers</p>
          </div>
        </div>
      </div>
    </div>
  );
}; 