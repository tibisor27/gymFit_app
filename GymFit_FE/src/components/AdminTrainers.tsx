import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { trainerService } from '../services/trainerService';

interface Trainer {
  Id: number;
  TrainerName: string;
  TrainerEmail: string;
  TrainerPhoneNumber: string;
  TrainerPhone: string;
  Experience: string;
  Introduction: string;
}

export const AdminTrainers: React.FC = () => {
  const [trainers, setTrainers] = useState<Trainer[]>([]);
  const [loading, setLoading] = useState(true);
  const [editingTrainer, setEditingTrainer] = useState<Trainer | null>(null);
  const [showEditModal, setShowEditModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchTrainers();
  }, []);

  const fetchTrainers = async () => {
    try {
      setLoading(true);
      const trainersData = await trainerService.getAllTrainers();
      setTrainers(trainersData);
    } catch (error) {
      console.error('Error fetching trainers:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (trainerId: number) => {
          if (window.confirm('Are you sure you want to delete this trainer?')) {
      try {
        await trainerService.deleteTrainer(trainerId);
        setTrainers(trainers.filter(trainer => trainer.Id !== trainerId));
                  alert('Trainer deleted successfully!');
      } catch (error) {
        console.error('Error deleting trainer:', error);
                  alert('Error deleting trainer!');
      }
    }
  };

  const handleEdit = (trainer: Trainer) => {
    setEditingTrainer({ ...trainer });
    setShowEditModal(true);
  };

  const handleSaveEdit = async () => {
    if (!editingTrainer) return;

    // ✅ Validare frontend
    if (editingTrainer.Experience && editingTrainer.Experience.length < 5) {
              alert('Experience must have at least 5 characters!');
      return;
    }

    if (editingTrainer.Introduction && editingTrainer.Introduction.length < 10) {
              alert('Introduction must have at least 10 characters!');
      return;
    }

    try {
      await trainerService.updateTrainer(editingTrainer.Id, editingTrainer);
      await fetchTrainers(); // Refresh list
      setShowEditModal(false);
      setEditingTrainer(null);
              alert('Trainer updated successfully!');
    } catch (error) {
      console.error('Error updating trainer:', error);
              alert('Error updating trainer!');
    }
  };

  const handleInputChange = (field: keyof Trainer, value: string) => {
    if (editingTrainer) {
      setEditingTrainer({
        ...editingTrainer,
        [field]: value
      });
    }
  };

  if (loading) {
          return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center">
          <div className="text-gray-600 text-lg">Loading...</div>
        </div>
      );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-3xl font-bold text-gray-900 mb-2">
                👑 Manage Trainers
              </h1>
              <p className="text-gray-600">
                                  {trainers.length} registered trainers
              </p>
            </div>
            <button
              onClick={() => navigate('/dashboard')}
              className="bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded text-sm transition-colors"
            >
              ← Back to Dashboard
            </button>
          </div>
        </div>
      </div>

      {/* Trainers Table */}
      <div className="max-w-7xl mx-auto px-6 py-8">
        {trainers.length === 0 ? (
          <div className="text-center py-16">
                          <p className="text-gray-500 text-lg">No registered trainers</p>
          </div>
        ) : (
          <div className="bg-white rounded-lg border overflow-hidden">
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Trainer
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Contact
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Experience
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Actions
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {trainers.map((trainer) => (
                    <tr key={trainer.Id} className="hover:bg-gray-50">
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="flex items-center">
                          <div className="w-10 h-10 bg-blue-500 rounded-full flex items-center justify-center text-white font-semibold">
                            {trainer.TrainerName?.charAt(0) || '?'}
                          </div>
                          <div className="ml-4">
                            <div className="text-sm font-medium text-gray-900">
                              {trainer.TrainerName}
                            </div>
                            <div className="text-sm text-gray-500">
                              ID: {trainer.Id}
                            </div>
                          </div>
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="text-sm text-gray-900">{trainer.TrainerEmail}</div>
                        <div className="text-sm text-gray-500">{trainer.TrainerPhoneNumber}</div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className="inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800">
                          {trainer.Experience || 'N/A'}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                        <div className="flex space-x-2">
                          <button
                            onClick={() => handleEdit(trainer)}
                            className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded text-xs transition-colors"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => handleDelete(trainer.Id)}
                            className="bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded text-xs transition-colors"
                          >
                            Delete
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>

      {/* Edit Modal */}
      {showEditModal && editingTrainer && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 w-full max-w-md mx-4">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">
              Edit Trainer
            </h3>
            
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Name
                </label>
                <input
                  type="text"
                  value={editingTrainer.TrainerName}
                  onChange={(e) => handleInputChange('TrainerName', e.target.value)}
                  className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Email
                </label>
                <input
                  type="email"
                  value={editingTrainer.TrainerEmail}
                  onChange={(e) => handleInputChange('TrainerEmail', e.target.value)}
                  className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Phone
                </label>
                <input
                  type="text"
                  value={editingTrainer.TrainerPhoneNumber}
                  onChange={(e) => handleInputChange('TrainerPhoneNumber', e.target.value)}
                  className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Experience
                </label>
                <input
                  type="text"
                  value={editingTrainer.Experience}
                  onChange={(e) => handleInputChange('Experience', e.target.value)}
                  className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Introduction
                </label>
                <textarea
                  value={editingTrainer.Introduction}
                  onChange={(e) => handleInputChange('Introduction', e.target.value)}
                  rows={3}
                  className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
            </div>
            
            <div className="flex justify-end space-x-3 mt-6">
              <button
                onClick={() => {
                  setShowEditModal(false);
                  setEditingTrainer(null);
                }}
                className="px-4 py-2 text-gray-700 border border-gray-300 rounded hover:bg-gray-50 transition-colors"
              >
                Cancel
              </button>
              <button
                onClick={handleSaveEdit}
                className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors"
              >
                Save
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}; 