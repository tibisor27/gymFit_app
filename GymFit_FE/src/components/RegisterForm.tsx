import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../context/authContext';

// 📝 Register Component - handles user registration
export const RegisterForm: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    dateOfBirth: ''
  });
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<{[key: string]: string[]}>({});
  const [generalError, setGeneralError] = useState('');
  const [success, setSuccess] = useState(false);
  
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
    
    // Clear specific field error when user starts typing
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: []
      });
    }
  };
  
  const validateForm = (): boolean => {
    const newErrors: {[key: string]: string[]} = {};
    
    // Name validation
    if (!formData.name.trim()) {
      newErrors.name = ['Name is required'];
    } else if (formData.name.length < 2) {
      newErrors.name = ['Name must be at least 2 characters'];
    } else if (!/^[a-zA-Z\s-']+$/.test(formData.name)) {
      newErrors.name = ['Name can only contain letters, spaces, hyphens and apostrophes'];
    }
    
    // Email validation
    if (!formData.email.trim()) {
      newErrors.email = ['Email is required'];
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = ['Please enter a valid email address'];
    }
    
    // Password validation
    if (!formData.password) {
      newErrors.password = ['Password is required'];
    } else if (formData.password.length < 8) {
      newErrors.password = ['Password must be at least 8 characters long'];
    }
    
    // Confirm password validation
    if (!formData.confirmPassword) {
      newErrors.confirmPassword = ['Please confirm your password'];
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = ['Passwords do not match'];
    }
    
    // Phone validation
    if (!formData.phoneNumber.trim()) {
      newErrors.phoneNumber = ['Phone number is required'];
    } else if (!/^(\+4|0)[0-9]{9}$/.test(formData.phoneNumber)) {
      newErrors.phoneNumber = ['Phone number must be in Romanian format (+40xxxxxxxxx or 07xxxxxxxx)'];
    }
    
    // Date of birth validation
    if (!formData.dateOfBirth) {
      newErrors.dateOfBirth = ['Date of birth is required'];
    } else {
      const birthDate = new Date(formData.dateOfBirth);
      const today = new Date();
      const age = today.getFullYear() - birthDate.getFullYear();
      
      if (age < 16) {
        newErrors.dateOfBirth = ['You must be at least 16 years old'];
      } else if (age > 100) {
        newErrors.dateOfBirth = ['Invalid age'];
      }
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setGeneralError('');
    setSuccess(false);
    
    if (!validateForm()) {
      return;
    }
    
    try {
      setLoading(true);
      
      // Format date for backend (YYYY-MM-DD)
      const registerData = {
        name: formData.name.trim(),
        email: formData.email.trim(),
        password: formData.password,
        phoneNumber: formData.phoneNumber.trim(),
        dateOfBirth: formData.dateOfBirth, // Already in YYYY-MM-DD format from input
        userRole: 1 // Member role
      };
      
      // Call register endpoint
      const response = await axios.post('http://localhost:5113/auth/register', registerData);
      
      setSuccess(true);
      
      // Auto-login after successful registration
      setTimeout(async () => {
        try {
          const loginResponse = await axios.post('http://localhost:5113/auth/login', {
            email: formData.email,
            password: formData.password
          });
          
          const { token } = loginResponse.data;
          await login(token);
          navigate('/dashboard');
        } catch (loginError) {
          // If auto-login fails, redirect to login page
          navigate('/login');
        }
      }, 1500);
      
    } catch (err: any) {
      console.error('Registration error:', err);
      
      if (err.response?.data?.errors) {
        // Handle field-specific errors from backend
        const backendErrors: {[key: string]: string[]} = {};
        
        err.response.data.errors.forEach((error: any) => {
          const fieldName = error.Field.toLowerCase();
          backendErrors[fieldName] = error.Errors;
        });
        
        setErrors(backendErrors);
      } else {
        // Handle general error
        const errorMessage = err.response?.data?.message || 'Registration failed. Please try again.';
        setGeneralError(errorMessage);
      }
    } finally {
      setLoading(false);
    }
  };
  
  if (success) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center p-4">
        <div className="bg-white rounded-2xl shadow-xl p-8 w-full max-w-md text-center">
          <div className="text-6xl mb-4">🎉</div>
          <h1 className="text-2xl font-bold text-gray-900 mb-2">
            Registration Successful!
          </h1>
          <p className="text-gray-600 mb-4">
            Welcome to GymFit! You will be logged in automatically...
          </p>
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mx-auto"></div>
        </div>
      </div>
    );
  }
  
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-indigo-900 flex items-center justify-center p-4">
      <div className="bg-white rounded-2xl shadow-xl p-8 w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            🏋️ Join GymFit
          </h1>
          <p className="text-gray-600">
            Create your account to get started
          </p>
        </div>
        
        <form onSubmit={handleSubmit} className="space-y-4">
          
          {/* General error */}
          {generalError && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
              {generalError}
            </div>
          )}
          
          {/* Name field */}
          <div>
            <label htmlFor="name" className="block text-sm font-medium text-gray-700 mb-1">
              Full Name
            </label>
            <input
              id="name"
              name="name"
              type="text"
              required
              value={formData.name}
              onChange={handleInputChange}
              className={`w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                errors.name ? 'border-red-400' : 'border-gray-300'
              }`}
              placeholder="John Doe"
              disabled={loading}
            />
            {errors.name && (
              <p className="mt-1 text-sm text-red-600">{errors.name[0]}</p>
            )}
          </div>
          
          {/* Email field */}
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-1">
              Email Address
            </label>
            <input
              id="email"
              name="email"
              type="email"
              required
              value={formData.email}
              onChange={handleInputChange}
              className={`w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                errors.email ? 'border-red-400' : 'border-gray-300'
              }`}
              placeholder="john@example.com"
              disabled={loading}
            />
            {errors.email && (
              <p className="mt-1 text-sm text-red-600">{errors.email[0]}</p>
            )}
          </div>
          
          {/* Password field */}
          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
              Password
            </label>
            <input
              id="password"
              name="password"
              type="password"
              required
              value={formData.password}
              onChange={handleInputChange}
              className={`w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                errors.password ? 'border-red-400' : 'border-gray-300'
              }`}
              placeholder="••••••••"
              disabled={loading}
            />
            {errors.password && (
              <p className="mt-1 text-sm text-red-600">{errors.password[0]}</p>
            )}
          </div>
          
          {/* Confirm Password field */}
          <div>
            <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700 mb-1">
              Confirm Password
            </label>
            <input
              id="confirmPassword"
              name="confirmPassword"
              type="password"
              required
              value={formData.confirmPassword}
              onChange={handleInputChange}
              className={`w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                errors.confirmPassword ? 'border-red-400' : 'border-gray-300'
              }`}
              placeholder="••••••••"
              disabled={loading}
            />
            {errors.confirmPassword && (
              <p className="mt-1 text-sm text-red-600">{errors.confirmPassword[0]}</p>
            )}
          </div>
          
          {/* Phone Number field */}
          <div>
            <label htmlFor="phoneNumber" className="block text-sm font-medium text-gray-700 mb-1">
              Phone Number
            </label>
            <input
              id="phoneNumber"
              name="phoneNumber"
              type="tel"
              required
              value={formData.phoneNumber}
              onChange={handleInputChange}
              className={`w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                errors.phoneNumber ? 'border-red-400' : 'border-gray-300'
              }`}
              placeholder="+40756123456 or 0756123456"
              disabled={loading}
            />
            {errors.phoneNumber && (
              <p className="mt-1 text-sm text-red-600">{errors.phoneNumber[0]}</p>
            )}
          </div>
          
          {/* Date of Birth field */}
          <div>
            <label htmlFor="dateOfBirth" className="block text-sm font-medium text-gray-700 mb-1">
              Date of Birth
            </label>
            <input
              id="dateOfBirth"
              name="dateOfBirth"
              type="date"
              required
              value={formData.dateOfBirth}
              onChange={handleInputChange}
              className={`w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                errors.dateOfBirth ? 'border-red-400' : 'border-gray-300'
              }`}
              disabled={loading}
            />
            {errors.dateOfBirth && (
              <p className="mt-1 text-sm text-red-600">{errors.dateOfBirth[0]}</p>
            )}
          </div>
          
          {/* Submit button */}
          <button
            type="submit"
            className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={loading}
          >
            {loading ? 'Creating Account...' : 'Create Account'}
          </button>
        </form>
        
        <div className="mt-6 text-center">
          <p className="text-sm text-gray-600">
            Already have an account?{' '}
            <button
              onClick={() => navigate('/login')}
              className="font-medium text-blue-600 hover:text-blue-500"
              disabled={loading}
            >
              Sign in here
            </button>
          </p>
        </div>
      </div>
    </div>
  );
}; 