import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

function AdminDashboard() {

  
  const navigate =
  useNavigate();
  
  useEffect(() => {
    
    const token =
    localStorage.getItem(
        "token"
      );

      if (!token) {
        navigate("/");
    }

  }, []);
  
  // const logout = () => {

  //   localStorage.removeItem(
  //     "token"
  //   );

  //   navigate("/");
  // };
  
  return (
    <>
    <div>AdminDashboard</div>

    {/* <button onClick={logout}>
      Logout
    </button>
     */}
    </>
  );
}

export default AdminDashboard;