import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/authContext';
import { useNavigate } from 'react-router-dom';
import { clientsService } from '../services/clientsService';
import { trainerService } from '../services/trainerService';

export const AdminDashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [membersCount, setMembersCount] = useState(0);
  const [trainersCount, setTrainersCount] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCounts = async () => {
      try {
        // Fetch members and trainers to display correct numbers
        const [membersData, trainersData] = await Promise.all([
          clientsService.getAllClients(),
          trainerService.getAllTrainers()
        ]);
        
        setMembersCount(membersData.length);
        setTrainersCount(trainersData.length);
      } catch (error) {
        console.error('Error fetching counts:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchCounts();
  }, []);

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
                <p className="text-sm text-gray-600">Total Members</p>
                <p className="text-2xl font-bold text-gray-900">
                  {loading ? '...' : membersCount}
                </p>
              </div>
            </div>
          </div>
          
          <div className="bg-white rounded-lg border p-6">
            <div className="flex items-center">
              <div className="text-3xl mr-4">🏋️</div>
              <div>
                <p className="text-sm text-gray-600">Total Trainers</p>
                <p className="text-2xl font-bold text-gray-900">
                  {loading ? '...' : trainersCount}
                </p>
              </div>
            </div>
          </div>
          
          <div className="bg-white rounded-lg border p-6">
            <div className="flex items-center">
              <div className="text-3xl mr-4">💰</div>
              <div>
                <p className="text-sm text-gray-600">Monthly Revenue</p>
                <p className="text-lg font-medium text-gray-500">Coming Soon</p>
              </div>
            </div>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-2 gap-4 mb-8">
          <button 
            onClick={() => navigate('/admin/trainers')}
            className="bg-blue-600 hover:bg-blue-700 text-white p-4 rounded-lg text-left transition-colors"
          >
            <div className="text-2xl mb-2">🏋️</div>
            <div className="font-semibold">Manage Trainers</div>
            <div className="text-sm opacity-90">View and manage trainers</div>
          </button>
          
          <button 
            onClick={() => navigate('/admin/members')}
            className="bg-green-600 hover:bg-green-700 text-white p-4 rounded-lg text-left transition-colors"
          >
            <div className="text-2xl mb-2">👥</div>
            <div className="font-semibold">Manage Members</div>
            <div className="text-sm opacity-90">Manage gym members</div>
          </button>
          

        </div>

   
      </div>
    </div>
  );
};