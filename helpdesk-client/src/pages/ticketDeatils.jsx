import NavBar from "../components/navbar";
import { useState } from "react";
import "../styles/Dashboard.css";
import "../styles/ticketDetails.css";
import {
  FaArrowLeft,
  FaDownload,
  FaFileAlt,
} from "react-icons/fa";
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
        <button
          className="back-btn"
          onClick={() => navigate("/tickets")}
        >
          <FaArrowLeft />
          Back to My Tickets
        </button>

        <div className="ticket-content">
          {/* Ticket Details */}
          <div className="details-card">
            <div className="card-header">
              <h2>Ticket Details</h2>
              <span className="status-badge">Open</span>
            </div>

            <div className="ticket-details-grid">
              <div className="detail-row">
                <span className="detail-label">Ticket ID</span>
                <span className="detail-value">#TKT-2056</span>
              </div>

              <div className="detail-row">
                <span className="detail-label">Title</span>
                <span className="detail-value">
                  VPN not connecting to company network
                </span>
              </div>

              <div className="detail-row">
                <span className="detail-label">Category</span>
                <span className="detail-value">Network</span>
              </div>

              <div className="detail-row">
                <span className="detail-label">Priority</span>
                <span className="detail-value priority-high">
                  High
                </span>
              </div>

              <div className="detail-row">
                <span className="detail-label">CreatedBy</span>
                <span className="detail-value">
                  Sara Johnson
                </span>
              </div>

              <div className="detail-row">
                <span className="detail-label">Created</span>
                <span className="detail-value">
                  May 21, 2024 10:32 AM
                </span>
              </div>
              <div className="detail-row">
                <span className="detail-label">AssignedTo</span>
                <span className="detail-value">
                  John Smith
                </span>
              </div>
            </div>

            
            <div className="ticket-description">
              <h4>Description</h4>

              <p>
                I am unable to connect to the VPN since this
                morning. It shows "Connection failed. Please
                check your network". I have already restarted
                my laptop and internet connection but the issue
                still persists.
              </p>
            </div>

            <div className="attachment-section">
              <h4>Attachment</h4>

              <div className="attachment-box">
                <div className="attachment-file">
                  <FaFileAlt />
                  <span>vpn_error_screenshot.png (2.4 MB)</span>
                </div>

                <FaDownload className="download-btn" />
              </div>
            </div>
          </div>

          {/* Comments */}
          <div className="comments-card">
            <h2>Comments</h2>

            <div className="comments-list">
              <div className="comment">
                <div className="avatar">J</div>

                <div className="comment-body">
                  <div className="comment-header">
                    <strong>John Smith</strong>
                    <span>May 21, 10:35 AM</span>
                  </div>

                  <p>
                    I cannot access the VPN. It keeps showing
                    connection failed even after restarting my
                    laptop.
                  </p>
                </div>
              </div>

              <div className="comment">
                <div className="avatar agent">S</div>

                <div className="comment-body">
                  <div className="comment-header">
                    <strong>Sarah Johnson</strong>
                    <span>May 21, 10:45 AM</span>
                  </div>

                  <p>
                    Thank you for reporting the issue. We are
                    currently reviewing your VPN configuration.
                  </p>
                </div>
              </div>

              <div className="comment">
                <div className="avatar">J</div>

                <div className="comment-body">
                  <div className="comment-header">
                    <strong>John Smith</strong>
                    <span>May 21, 11:10 AM</span>
                  </div>

                  <p>
                    Thank you. Please let me know if you need
                    any additional information.
                  </p>
                </div>
              </div>
            </div>

            <div className="comment-input">
              <textarea
                placeholder="Write a comment..."
              ></textarea>

              <button className="send-btn">
                Add Comment
              </button>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}

export default TicketDetails;