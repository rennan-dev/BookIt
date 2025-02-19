import React from "react";
import { useNavigate } from "react-router-dom";
import "./Header.css";
import logo from "../../../assets/images/icet_logo.png";

const Header = () => {
  const navigate = useNavigate();

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

      // Remove o token e redireciona para a página de login
      localStorage.removeItem("token");
      navigate("/login");
    } catch (error) {
      console.error("Erro ao fazer logout:", error);
    }
  };

  return (
    <header className="header">
      <a href="#home" className="logo">
        <img src={logo} alt="Logo ICET" className="logo-img" />
      </a>

      <nav className="navbar" id="navbar">
        <a href="#reservas">Reservas</a>
        <a href="#cadastros-pendentes">Cadastros Pendentes</a>
        <a href="#busca-usuario">Buscar Usuário</a>
        <a href="#logout" onClick={handleLogout}>Sair</a>
      </nav>
    </header>
  );
};

export default Header;