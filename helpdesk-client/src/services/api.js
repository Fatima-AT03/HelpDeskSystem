const API_URL = "http://localhost:5213/api";

export const getDashboard = async () => {
  const token = localStorage.getItem("token");

  const response = await fetch(
    `${API_URL}/dashboard`,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );

  if (!response.ok)
    throw new Error("Failed to load dashboard");

  return response.json();
};

export const getTickets = async () => {
  const token = localStorage.getItem("token");

  const response = await fetch(
    `${API_URL}/tickets`,
    {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );

  if (!response.ok)
    throw new Error("Failed to load tickets");

  return response.json();
};