import axios from 'axios';

export const trainerService = {
    getAllTrainers: async () => {
        try {
            const response = await axios.get('http://localhost:5113/odata/members');
            console.log(response.data);
            return response.data.value;
        } catch (error) {
            console.error('Error fetching trainers:', error);
            throw error;
        }
    },
}