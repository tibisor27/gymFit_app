import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { trainerService } from "../services/trainerService";

export const Trainers = () => {
    const [trainers, setTrainers] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchTrainers = async () => {
            try {
                setLoading(true);
                const trainers = await trainerService.getAllTrainers();
                console.log("trainers", trainers);
                setTrainers(trainers);
            } catch (error) {
                console.error("Error fetching trainers:", error);
            } finally {
                setLoading(false);
            }
        }
        fetchTrainers();
    }, []);

    if (loading) {
        return (
            <div className="min-h-screen bg-white flex items-center justify-center">
                <div className="text-gray-600 text-lg">Loading...</div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-white">
            {/* Header */}
            <div className="bg-white border-b border-gray-200">
                <div className="max-w-6xl mx-auto px-6 py-8">
                    <div className="flex items-center justify-between">
                        <div>
                            <h1 className="text-3xl font-bold text-gray-900 mb-2">
                                Trainers
                            </h1>
                            <p className="text-gray-600">
                                {trainers.length} available trainers
                            </p>
                        </div>
                        <button
                            onClick={() => navigate('/')}
                            className="bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded text-sm transition-colors"
                        >
                            ← Back
                        </button>
                    </div>
                </div>
            </div>

            {/* Trainers List */}
            <div className="max-w-6xl mx-auto px-6 py-8">
                {trainers.length === 0 ? (
                    <div className="text-center py-16">
                        <p className="text-gray-500 text-lg">
                            No registered trainers
                        </p>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {trainers.map((trainer: any) => (
                            <div 
                                key={trainer.Id}
                                className="bg-white border border-gray-200 rounded-lg p-6 hover:border-gray-300 transition-colors"
                            >
                                {/* Avatar */}
                                <div className="flex justify-center mb-4">
                                    <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center text-xl font-semibold text-gray-600">
                                        {trainer.TrainerName?.charAt(0) || '?'}
                                    </div>
                                </div>

                                {/* Info */}
                                <div className="text-center">
                                    <h3 className="text-lg font-semibold text-gray-900 mb-1">
                                        {trainer.TrainerName}
                                    </h3>
                                    
                                    <span className="inline-block bg-gray-50 text-gray-700 px-3 py-1 rounded-full text-sm mb-4">
                                        {trainer.Experience}
                                    </span>

                                    {/* Contact Info */}
                                    <div className="space-y-2 text-sm text-gray-600 mb-4">
                                        <div>{trainer.TrainerEmail}</div>
                                        <div>{trainer.TrainerPhoneNumber}</div>
                                        <div>
                                            {trainer.TrainerPhone}
                                        </div>
                                        <div>
                                            {trainer.Introduction}
                                        </div>
                                    </div>

                                    {/* Action Buttons */}
                                    <div className="space-y-2">
                                        <button className="w-full bg-gray-900 hover:bg-gray-800 text-white py-2 px-4 rounded text-sm transition-colors">
                                            Schedule
                                        </button>
                                        <button className="w-full border border-gray-300 hover:bg-gray-50 text-gray-700 py-2 px-4 rounded text-sm transition-colors">
                                            Contact
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}