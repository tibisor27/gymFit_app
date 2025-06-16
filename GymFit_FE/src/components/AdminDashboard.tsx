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
    <div className="min-h-screen bg-white">
      {/* Header */}
      <div className="bg-white border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex justify-between items-center">
            <div className="flex items-center space-x-4">
              <h1 className="text-3xl font-bold text-gray-900">Admin Dashboard</h1>
            </div>
            
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-600">
                Admin: <span className="font-semibold">{user?.Name}</span>
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
        
        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <div className="bg-white border border-gray-200 rounded-lg p-6">
            <div className="flex items-center">
              <div className="w-12 h-12 bg-blue-50 rounded-lg flex items-center justify-center mr-4">
                <div className="w-6 h-6 bg-blue-600 rounded"></div>
              </div>
              <div>
                <p className="text-sm text-gray-600">Total Members</p>
                <p className="text-2xl font-bold text-gray-900">
                  {loading ? '...' : membersCount}
                </p>
              </div>
            </div>
          </div>
          
          <div className="bg-white border border-gray-200 rounded-lg p-6">
            <div className="flex items-center">
              <div className="w-12 h-12 bg-green-50 rounded-lg flex items-center justify-center mr-4">
                <div className="w-6 h-6 bg-green-600 rounded"></div>
              </div>
              <div>
                <p className="text-sm text-gray-600">Total Trainers</p>
                <p className="text-2xl font-bold text-gray-900">
                  {loading ? '...' : trainersCount}
                </p>
              </div>
            </div>
          </div>
          
          <div className="bg-white border border-gray-200 rounded-lg p-6">
            <div className="flex items-center">
              <div className="w-12 h-12 bg-gray-50 rounded-lg flex items-center justify-center mr-4">
                <div className="w-6 h-6 bg-gray-600 rounded"></div>
              </div>
              <div>
                <p className="text-sm text-gray-600">Monthly Revenue</p>
                <p className="text-lg font-medium text-gray-500">Coming Soon</p>
              </div>
            </div>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
          <button 
            onClick={() => navigate('/admin/trainers')}
            className="bg-white border border-gray-200 hover:border-gray-300 p-6 rounded-lg text-left transition-colors"
          >
            <div className="font-semibold text-gray-900 mb-2">Manage Trainers</div>
            <div className="text-sm text-gray-600">View and manage trainers</div>
          </button>
          
          <button 
            onClick={() => navigate('/admin/members')}
            className="bg-white border border-gray-200 hover:border-gray-300 p-6 rounded-lg text-left transition-colors"
          >
            <div className="font-semibold text-gray-900 mb-2">Manage Members</div>
            <div className="text-sm text-gray-600">Manage gym members</div>
          </button>
        </div>

      </div>
    </div>
  );
};