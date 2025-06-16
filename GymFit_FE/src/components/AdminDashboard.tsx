import React from 'react';
import { useAuth } from '../context/authContext';
import { useNavigate } from 'react-router-dom';

export const AdminDashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex justify-between items-center">
            <div className="flex items-center space-x-4">
              <div className="text-3xl">👑</div>
              <h1 className="text-3xl font-bold text-gray-900">Admin Dashboard</h1>
            </div>
            
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-600">
                Admin: <span className="font-semibold">{user?.Name}</span>
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
        
        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <div className="bg-white rounded-lg border p-6">
            <div className="flex items-center">
              <div className="text-3xl mr-4">👥</div>
              <div>
                <p className="text-sm text-gray-600">Total Membri</p>
                <p className="text-2xl font-bold text-gray-900">245</p>
              </div>
            </div>
          </div>
          
          <div className="bg-white rounded-lg border p-6">
            <div className="flex items-center">
              <div className="text-3xl mr-4">🏋️</div>
              <div>
                <p className="text-sm text-gray-600">Total Antrenori</p>
                <p className="text-2xl font-bold text-gray-900">12</p>
              </div>
            </div>
          </div>
          
          <div className="bg-white rounded-lg border p-6">
            <div className="flex items-center">
              <div className="text-3xl mr-4">💰</div>
              <div>
                <p className="text-sm text-gray-600">Venituri Luna</p>
                <p className="text-2xl font-bold text-gray-900">15,430 RON</p>
              </div>
            </div>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
          <button 
            onClick={() => navigate('/admin/trainers')}
            className="bg-blue-600 hover:bg-blue-700 text-white p-4 rounded-lg text-left transition-colors"
          >
            <div className="text-2xl mb-2">🏋️</div>
            <div className="font-semibold">Gestionează Antrenori</div>
            <div className="text-sm opacity-90">Vezi și administrează antrenorii</div>
          </button>
          
          <button className="bg-green-600 hover:bg-green-700 text-white p-4 rounded-lg text-left transition-colors">
            <div className="text-2xl mb-2">👥</div>
            <div className="font-semibold">Gestionează Membri</div>
            <div className="text-sm opacity-90">Administrează membrii gym-ului</div>
          </button>
          
          <button className="bg-purple-600 hover:bg-purple-700 text-white p-4 rounded-lg text-left transition-colors">
            <div className="text-2xl mb-2">📊</div>
            <div className="font-semibold">Rapoarte</div>
            <div className="text-sm opacity-90">Vezi statistici și rapoarte</div>
          </button>
          
          <button className="bg-orange-600 hover:bg-orange-700 text-white p-4 rounded-lg text-left transition-colors">
            <div className="text-2xl mb-2">⚙️</div>
            <div className="font-semibold">Setări</div>
            <div className="text-sm opacity-90">Configurări sistem</div>
          </button>
        </div>

        {/* Recent Activity */}
        <div className="bg-white rounded-lg border">
          <div className="p-6 border-b">
            <h2 className="text-xl font-semibold text-gray-900">Activitate Recentă</h2>
          </div>
          <div className="p-6">
            <div className="space-y-4">
              <div className="flex items-center space-x-4">
                <div className="w-2 h-2 bg-green-500 rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm text-gray-900">Membru nou înregistrat: <span className="font-semibold">Ana Maria</span></p>
                  <p className="text-xs text-gray-500">Acum 5 minute</p>
                </div>
              </div>
              
              <div className="flex items-center space-x-4">
                <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm text-gray-900">Antrenor nou adăugat: <span className="font-semibold">Mihai Popescu</span></p>
                  <p className="text-xs text-gray-500">Acum 1 oră</p>
                </div>
              </div>
              
              <div className="flex items-center space-x-4">
                <div className="w-2 h-2 bg-yellow-500 rounded-full"></div>
                <div className="flex-1">
                  <p className="text-sm text-gray-900">Sesiune programată: <span className="font-semibold">Yoga - Sala 2</span></p>
                  <p className="text-xs text-gray-500">Acum 2 ore</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};