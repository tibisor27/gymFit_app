import axios from 'axios';

const getAuthHeaders = () => {
    const token = localStorage.getItem('token');
    return token ? { Authorization: `Bearer ${token}` } : {};
};

export const trainerService = {
    getAllTrainers: async () => {
        try {
            const response = await axios.get('http://localhost:5113/odata/trainers');
            console.log(response.data);
            return response.data.value;
        } catch (error) {
            console.error('Error fetching trainers:', error);
            throw error;
        }
    },

    deleteTrainer: async (trainerId: number) => {
        try {
            const response = await axios.delete(`http://localhost:5113/odata/trainers(${trainerId})`, {
                headers: getAuthHeaders()
            });
            return response.data;
        } catch (error) {
            console.error('Error deleting trainer:', error);
            throw error;
        }
    },

    updateTrainer: async (trainerId: number, trainerData: any) => {
        try {
            // ✅ Trimit doar câmpurile care pot fi actualizate
            const updateData = {
                ...(trainerData.Experience && { Experience: trainerData.Experience }),
                ...(trainerData.Introduction && { Introduction: trainerData.Introduction })
            };

            console.log('Sending update data:', updateData);

            const response = await axios.patch(`http://localhost:5113/odata/trainers/${trainerId}`, updateData, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            });
            return response.data;
        } catch (error) {
            console.error('Error updating trainer:', error);
            throw error;
        }
    },

    getTrainerById: async (trainerId: number) => {
        try {
            const response = await axios.get(`http://localhost:5113/odata/trainers/${trainerId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json'
                }
            });
            return response.data;
        } catch (error) {
            console.error('Error fetching trainer:', error);
            throw error;
        }
    }
}