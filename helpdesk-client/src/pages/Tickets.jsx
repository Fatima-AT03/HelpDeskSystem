import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getTickets } from "../services/api";

function Tickets() {
  const navigate = useNavigate();
  const [tickets, setTickets] = useState([]);

  useEffect(() => {
    loadTickets();

    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/");
    }
  }, []);

  const loadTickets = async () => {
    try {
      const data = await getTickets();
      setTickets(data);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <h2>Tickets</h2>

      <table>
        <thead>
          <tr>
            <th>Ref</th>
            <th>Title</th>
            <th>Status</th>
            <th>Priority</th>
          </tr>
        </thead>

        <tbody>
          {tickets.map((ticket) => (
            <tr key={ticket.id}>
              <td>{ticket.referenceNumber}</td>
              <td>{ticket.title}</td>
              <td>{ticket.statusId}</td>
              <td>{ticket.priorityId}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Tickets;
