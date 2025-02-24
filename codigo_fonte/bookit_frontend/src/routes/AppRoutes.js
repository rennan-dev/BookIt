import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import Login from "../views/Login";
import HomeAdmin from "../views/admin/HomeAdmin";
import HomeServidor from "../views/servidor/HomeServidor";
import { useAuth } from "../hooks/useAuth";
import Cadastro from "../views/Cadastro";
import CadastrosPendentes from "../views/admin/CadastrosPendentes";
import UsuarioCadastrado from "../views/admin/UsuariosCadastradosPage";
import ReservasCadastradas from "../views/admin/ReservasCadastradasPage";
import CadastroDeReserva from "../views/servidor/CadastroDeReserva";
import PageMinhasReservas from "../views/servidor/ServidorMinhasReservas";

  function AppRoutes() {
    const user = useAuth();

    return (
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/cadastro" element={<Cadastro/>} />

        <Route path="/admin" element={user?.isAdmin ? <HomeAdmin /> : <Navigate to="/login" />} />
        <Route path="/cadastros-pendentes" element={user?.isAdmin ? <CadastrosPendentes /> : <Navigate to="/login" />} />
        <Route path="/usuarios-cadastrados" element={user?.isAdmin ? <UsuarioCadastrado /> : <Navigate to="/login" />} />
        <Route path="/reservas-cadastradas/:data/:ambiente" element={user?.isAdmin ? <ReservasCadastradas /> : <Navigate to="/login" />} />

        <Route path="/servidor" element={user && !user.isAdmin ? <HomeServidor /> : <Navigate to="/login" />} />
        <Route path="/cadastro-reserva/:data/:ambiente" element={user && !user.isAdmin ? <CadastroDeReserva  /> : <Navigate to="/login" />} />
        <Route path="/minhas-reservas" element={user && !user.isAdmin ? <PageMinhasReservas  /> : <Navigate to="/login" />} />

        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    );
  }

  export default AppRoutes;
