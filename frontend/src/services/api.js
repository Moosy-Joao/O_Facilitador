const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const getAuthHeaders = () => {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };
};

const handleResponse = async (response) => {
  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: 'Erro de conexão com o servidor' }));
    throw new Error(error.message || `Erro ${response.status}`);
  }
  return response.json();
};

// === AUTH ===
export const authService = {
  login: async (email, password) => {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });
    return handleResponse(response);
  },
  register: async (data) => {
    const response = await fetch(`${API_BASE_URL}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },
};

// === CLIENTS ===
export const clientService = {
  getAll: async () => {
    const response = await fetch(`${API_BASE_URL}/clients`, {
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
  getById: async (id) => {
    const response = await fetch(`${API_BASE_URL}/clients/${id}`, {
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
  create: async (data) => {
    const response = await fetch(`${API_BASE_URL}/clients`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },
  update: async (id, data) => {
    const response = await fetch(`${API_BASE_URL}/clients/${id}`, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },
  toggleActive: async (id) => {
    const response = await fetch(`${API_BASE_URL}/clients/${id}/toggle-active`, {
      method: 'PATCH',
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
};

// === PURCHASES ===
export const purchaseService = {
  getByClient: async (clientId) => {
    const response = await fetch(`${API_BASE_URL}/clients/${clientId}/purchases`, {
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
  create: async (clientId, data) => {
    const response = await fetch(`${API_BASE_URL}/clients/${clientId}/purchases`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },
  reverse: async (clientId, purchaseId) => {
    const response = await fetch(`${API_BASE_URL}/clients/${clientId}/purchases/${purchaseId}/reverse`, {
      method: 'POST',
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
};

// === PAYMENTS ===
export const paymentService = {
  getByClient: async (clientId) => {
    const response = await fetch(`${API_BASE_URL}/clients/${clientId}/payments`, {
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
  create: async (clientId, data) => {
    const response = await fetch(`${API_BASE_URL}/clients/${clientId}/payments`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },
  reverse: async (clientId, paymentId) => {
    const response = await fetch(`${API_BASE_URL}/clients/${clientId}/payments/${paymentId}/reverse`, {
      method: 'POST',
      headers: getAuthHeaders(),
    });
    return handleResponse(response);
  },
};
