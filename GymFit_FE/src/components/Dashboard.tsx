import React, { useEffect } from 'react';
import { useAuth } from '../context/authContext';
import { useNavigate } from 'react-router-dom';

// 🏋️ Dashboard simplu - afișează datele user-ului
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

  // 🔄 Afișez loading în timp ce se încarcă datele
  if (loading) return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center">
      <div className="text-white text-xl">Loading...</div>
    </div>
  );

  // ❌ Dacă e eroare
  if (error) return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center">
      <div className="text-red-400 text-xl">Error: {error}</div>
    </div>
  );

  // ❌ Dacă nu e user (de exemplu nu e logat)
  if (!user) return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center">
      <div className="text-white text-xl">Not authenticated!</div>
    </div>
  );

  // 📊 Helper simplu pentru rol
  const getRoleInfo = (role: string) => {
    switch (role) {
      case 'Admin': return { icon: '👑', color: 'text-purple-600' };
      case 'Member': return { icon: '💪', color: 'text-blue-600' };
      case 'Trainer': return { icon: '🏋️', color: 'text-green-600' };
      default: return { icon: '❓', color: 'text-gray-600' };
    }
  };

  const roleInfo = user ? getRoleInfo(user.UserRole) : { icon: '❓', color: 'text-gray-600' };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex justify-between items-center">
            <div className="flex items-center space-x-4">
              <div className="text-3xl">{roleInfo.icon}</div>
              <h1 className="text-3xl font-bold text-gray-900">
                {user?.UserRole === 'Trainer' ? 'Trainer Dashboard' : 'Member Dashboard'}
              </h1>
            </div>
            
            <div className="flex items-center space-x-4">
              <button
                onClick={() => navigate('/trainers')}
                className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded text-sm transition-colors"
              >
                Trainers
              </button>
              <span className="text-sm text-gray-600">
                <span className="font-semibold">{user.Name}</span>
              </span>
              <button
                onClick={handleLogout}
                className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded text-sm transition-colors"
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
        <div className="bg-white rounded-lg border p-8 mb-8">
          <div className="flex items-center space-x-6">
            <div className={`text-6xl ${roleInfo.color}`}>
              {roleInfo.icon}
            </div>
            <div>
              <h2 className="text-2xl font-bold text-gray-900 mb-2">
                Welcome, {user.Name}!
              </h2>
              <div className="space-y-1">
                <p className="text-gray-600">
                  <span className="font-medium">Email:</span> {user.Email}
                </p>
                <p className="text-gray-600">
                  <span className="font-medium">Role:</span> 
                  <span className={`ml-2 px-3 py-1 rounded-full text-sm font-medium bg-gray-100 ${roleInfo.color}`}>
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
          <div className="bg-white rounded-lg border p-6 hover:shadow-md transition-shadow cursor-pointer">
            <div className="text-3xl mb-4">📅</div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Schedule</h3>
            <p className="text-gray-600 text-sm">View your workout schedule and upcoming sessions</p>
          </div>

          <div className="bg-white rounded-lg border p-6 hover:shadow-md transition-shadow cursor-pointer">
            <div className="text-3xl mb-4">💪</div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Workouts</h3>
            <p className="text-gray-600 text-sm">Browse workout plans and track your progress</p>
          </div>

          <div className="bg-white rounded-lg border p-6 hover:shadow-md transition-shadow cursor-pointer">
            <div className="text-3xl mb-4">👥</div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Community</h3>
            <p className="text-gray-600 text-sm">Connect with other gym members and trainers</p>
          </div>
        </div>
      </div>
    </div>
  );
}; 