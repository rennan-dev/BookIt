import React from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "./Header.css";
import logo from "../../../assets/images/icet_logo.png";

const Header = () => {
  const navigate = useNavigate();
  const location = useLocation(); 

  const handleLogout = async () => {
    try {
      const token = localStorage.getItem("token");

      const response = await fetch("http://localhost:5092/api/User/logout", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      console.log("Logout status:", response.status);

      localStorage.removeItem("token");
      navigate("/login");
    } catch (error) {
      console.error("Erro ao fazer logout:", error);
    }
  };

  return (
    <header className="header">
      <a href="/servidor" className="logo">
        <img src={logo} alt="Logo ICET" className="logo-img" />
      </a>

      <nav className="navbar" id="navbar">
        <a href="/servidor" className={location.pathname === "/servidor" ? "active" : ""}> Reservas </a>
        <a href="/minhas-reservas" className={location.pathname === "/minhas-reservas" ? "active" : ""}>Minha(s) reserva(s)</a>
        <a href="#logout" onClick={handleLogout}>Sair</a>
      </nav>
    </header>
  );
};

export default Header;