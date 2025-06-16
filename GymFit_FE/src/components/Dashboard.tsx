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
      <div className="text-white text-xl">Se încarcă...</div>
    </div>
  );

  // ❌ Dacă e eroare
  if (error) return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center">
      <div className="text-red-400 text-xl">Eroare: {error}</div>
    </div>
  );

  // ❌ Dacă nu e user (de exemplu nu e logat)
  if (!user) return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center">
      <div className="text-white text-xl">Nu ești autentificat!</div>
    </div>
  );

  // �� Helper simplu pentru rol
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
                Antrenori
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
        
        {/* User Profile Card */}
        <div className="bg-white rounded-lg border p-6 mb-8">
          <h2 className="text-2xl font-bold text-gray-900 mb-4">
            Profilul Tău
          </h2>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-3">
                Informații Personale
              </h3>
              <div className="space-y-2">
                <div className="flex items-center">
                  <span className="text-2xl mr-3">{roleInfo.icon}</span>
                  <div>
                    <p className="text-sm text-gray-600">Nume</p>
                    <p className="font-medium">{user?.Name}</p>
                  </div>
                </div>
                
                <div className="flex items-center">
                  <span className="text-2xl mr-3">📧</span>
                  <div>
                    <p className="text-sm text-gray-600">Email</p>
                    <p className="font-medium">{user?.Email}</p>
                  </div>
                </div>
                
                <div className="flex items-center">
                  <span className="text-2xl mr-3">📱</span>
                  <div>
                    <p className="text-sm text-gray-600">Telefon</p>
                    <p className="font-medium">{user?.PhoneNumber || 'Nu este setat'}</p>
                  </div>
                </div>
              </div>
            </div>

            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-3">
                Detalii Cont
              </h3>
              <div className="space-y-2">
                <div className="flex items-center">
                  <span className="text-2xl mr-3">🎭</span>
                  <div>
                    <p className="text-sm text-gray-600">Rol</p>
                    <p className={`font-medium ${roleInfo.color}`}>
                      {user?.UserRole}
                    </p>
                  </div>
                </div>
                
                <div className="flex items-center">
                  <span className="text-2xl mr-3">🎂</span>
                  <div>
                    <p className="text-sm text-gray-600">Data Nașterii</p>
                    <p className="font-medium">{user?.DateOfBirth || 'Nu este setată'}</p>
                  </div>
                </div>
                
                <div className="flex items-center">
                  <span className="text-2xl mr-3">🆔</span>
                  <div>
                    <p className="text-sm text-gray-600">ID Utilizator</p>
                    <p className="font-medium">#{user?.Id}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}; 