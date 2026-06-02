import {
  BrowserRouter,
  Routes,
  Route
} from "react-router-dom";

import Login from "./pages/Login";
import Dashboard from "./pages/dashboard";
import AdminDashboard from "./pages/adminDashboard";

function App() {
  return (
    <BrowserRouter>

      <Routes>

        <Route
          path="/"
          element={<Login />}
        />

         <Route
          path="/dashboard"
          element={<Dashboard />}
        />

        <Route
          path="/adminDashboard"
          element={<AdminDashboard />}
        />



      </Routes>

    </BrowserRouter>
  );
}

export default App;