import axios from 'axios';

export const clientsService = {
    getAllClients: async () => {
        try {
            const response = await axios.get('http://localhost:5113/odata/members',
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('token')}`,
                        'Content-Type': 'application/json'
                    }
                }
            );
            console.log('Members response:', response.data);
            return response.data.value;
        } catch (error) {
            console.error('Error fetching members:', error);
            throw error;
        }
    },
}