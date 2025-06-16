import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/authContext';
import axios from 'axios';

// 🔑 Componenta Login - gestionează autentificarea
export const LoginForm: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  
  // 📝 Starea formularului
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  // 🔄 Gestionez modificările input-urilor
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    // Șterge eroarea când user-ul începe să tasteze
    if (error) setError('');
  };

  // 📤 Gestionez submit-ul formularului
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      // Apelez backend-ul direct pentru login
      const response = await axios.post('http://localhost:5113/auth/login', {
        email: formData.email,
        password: formData.password
      });

      const { token } = response.data;
      
      // Folosesc AuthContext pentru a salva token-ul și a încărca user data
      await login(token);
      
      // La login reușit, navighez la dashboard
      navigate('/dashboard');
    } catch (err: any) {
      // Afișez mesajul de eroare dacă login-ul eșuează
      console.error('Login error:', err);
      const errorMessage = err.response?.data?.message || 'Login eșuat';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="max-w-md w-full space-y-8 p-8">
        <div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            🏋️ GymFit Login
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            Introdu credențialele pentru a accesa contul
          </p>
        </div>
        
        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          {/* ❌ Afișez mesajul de eroare dacă există */}
          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
              {error}
            </div>
          )}

          <div className="space-y-4">
            {/* 📧 Input pentru email */}
            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                Adresa Email
              </label>
              <input
                id="email"
                name="email"
                type="email"
                required
                value={formData.email}
                onChange={handleChange}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="email@exemplu.com"
                disabled={loading}
              />
            </div>

            {/* 🔒 Input pentru parolă */}
            <div>
              <label htmlFor="password" className="block text-sm font-medium text-gray-700">
                Parola
              </label>
              <input
                id="password"
                name="password"
                type="password"
                required
                value={formData.password}
                onChange={handleChange}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                placeholder="Introdu parola"
                disabled={loading}
              />
            </div>
          </div>

          {/* 🚀 Buton de submit */}
          <div>
            <button
              type="submit"
              disabled={loading}
              className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? 'Se conectează...' : 'Conectează-te'}
            </button>
          </div>

          {/* 🔗 Link la înregistrare */}
          <div className="text-center">
            <p className="text-sm text-gray-600">
              Nu ai cont?{' '}
              <Link 
                to="/register" 
                className="font-medium text-blue-600 hover:text-blue-500"
              >
                Înregistrează-te aici
              </Link>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
}; 