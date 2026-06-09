import NavBar from "../components/navbar";
import { useState, useEffect } from "react";
import "../styles/Dashboard.css";
import "../styles/ticketDetails.css";
import { FaArrowLeft, FaClock, FaUser, FaEllipsisH } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

function TicketDetails() {
  const navigate = useNavigate();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className="dashboard-container">
      <NavBar
        isOpen={sidebarOpen}
        toggleSidebar={() => setSidebarOpen(!sidebarOpen)}
      />

      <main className="main-content">
        {/* Header */}
        <div className="ticket-header">
          <div className="ticket-header-left">
            <button className="back-btn" onClick={() => navigate("/tickets")}>
              <FaArrowLeft />
              Back to My Tickets
            </button>

            <div className="ticket-title-row">
              <h2>TKT-2056 VPN not connecting</h2>

              <span className="badge high">High</span>

              <span className="badge progress">In Progress</span>
            </div>
          </div>

          <div className="ticket-actions">
            <button className="secondary-btn">
              More
              <FaEllipsisH />
            </button>

            <button className="primary-btn">Update Status</button>
          </div>
        </div>

        {/* Top Info */}
        <div className="ticket-info-card">
          <div className="info-item">
            <FaUser />
            <div>
              <span>Requester</span>
              <h4>John Smith</h4>
            </div>
          </div>

          <div className="info-item">
            <span>Category</span>
            <h4>Network</h4>
          </div>

          <div className="info-item">
            <FaClock />
            <div>
              <span>Created</span>
              <h4>May 21, 2024 10:32 AM</h4>
            </div>
          </div>

          <div className="info-item">
            <FaClock />
            <div>
              <span>Last Updated</span>
              <h4>May 21, 2024 11:36 AM</h4>
            </div>
          </div>

          <div className="info-item">
            <FaUser />
            <div>
              <span>Assigned To</span>
              <h4>Sarah Johnson</h4>
            </div>
          </div>
        </div>

        {/* Tabs */}
        <div className="tabs">
          <button className="tab active">Details</button>
          <button className="tab">Comments (3)</button>
          <button className="tab">Updates (2)</button>
          <button className="tab">Attachments (1)</button>
        </div>

        {/* Content */}
        <div className="ticket-content">
          {/* Left */}
          <div className="details-card">
            <div className="field">
              <label>Title</label>
              <p>VPN not connecting to company network</p>
            </div>

            <div className="field">
              <label>Description</label>
              <p>
                I am unable to connect to the VPN since this morning. It shows
                "Connection failed, Please check your network". I have already
                restarted my laptop.
              </p>
            </div>

            <div className="details-grid">
              <div>
                <label>Priority</label>
                <span className="badge high">High</span>
              </div>

              <div>
                <label>Status</label>
                <span className="badge progress">In Progress</span>
              </div>

              <div>
                <label>Category</label>
                <p>Network</p>
              </div>

              <div>
                <label>Subcategory</label>
                <p>VPN / Connectivity</p>
              </div>
            </div>
          </div>

          {/* Right */}
          <div className="timeline-card">
            <h3>Activity Timeline</h3>

            <div className="timeline">
              <div className="timeline-item">
                <div className="dot blue"></div>
                <div>
                  <span>May 21, 11:36 AM</span>
                  <p>Sarah Johnson assigned to you</p>
                </div>
              </div>

              <div className="timeline-item">
                <div className="dot blue"></div>
                <div>
                  <span>May 21, 10:45 AM</span>
                  <p>We are checking your VPN configuration</p>
                </div>
              </div>

              <div className="timeline-item">
                <div className="dot white"></div>
                <div>
                  <span>May 21, 10:33 AM</span>
                  <p>Ticket created</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}

export default TicketDetails;
