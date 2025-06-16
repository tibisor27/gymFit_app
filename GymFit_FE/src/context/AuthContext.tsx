import axios from "axios";
import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import { useNavigate } from "react-router-dom";

interface User {
    Id: number;
    Name: string;
    Email: string;
    PhoneNumber: string;
    DateOfBirth: string;
    UserRole: string; // "Admin", "Member", "Trainer"
}

interface AuthContextType {
    user: User | null;
    loading: boolean;
    error: string | null;
    login: (token: string) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);


export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            fetchUser(token);
        } else {
            setLoading(false);
            navigate('/login');
        }
    }, []);

    useEffect(() => {
        const checkToken = async () => {
            const token = localStorage.getItem('token');
            if (token && !user) {
                setUser(null);
                navigate('/login');
            }
        }
        const checkTokenInterval = setInterval(checkToken, 1000);
        return () => clearInterval(checkTokenInterval);
    }, [user, navigate]);


    const fetchUser = async (token: string) => {
        try {
            const response = await axios.get('http://localhost:5113/auth/me', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            setUser(response.data);
            console.log(response.data);
            setError(null);
        } catch (err) {
            console.error('Error fetching user:', err);
            setError('Failed to fetch user data');
            localStorage.removeItem('token');
        } finally {
            setLoading(false);
        }
    };

    const login = async (token: string) => {
        localStorage.setItem('token', token);
        await fetchUser(token);
    };

    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
        navigate('/');
    };

    return (
        <AuthContext.Provider value={{ user, loading, error, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
}; 