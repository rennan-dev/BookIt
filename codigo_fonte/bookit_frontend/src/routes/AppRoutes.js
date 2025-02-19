  import React from "react";
  import { Routes, Route, Navigate } from "react-router-dom";
  import Login from "../views/Login";
  import HomeAdmin from "../views/admin/HomeAdmin";
  import HomeServidor from "../views/servidor/HomeServidor";
  import { useAuth } from "../hooks/useAuth";
  import Cadastro from "../views/Cadastro";
  import CadastrosPendentes from "../views/admin/CadastrosPendentes";

  function AppRoutes() {
    const user = useAuth();

    return (
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/cadastro" element={<Cadastro/>} />

        <Route path="/admin" element={user?.isAdmin ? <HomeAdmin /> : <Navigate to="/login" />} />
        <Route path="/servidor" element={user && !user.isAdmin ? <HomeServidor /> : <Navigate to="/login" />} />
        <Route path="/cadastros-pendentes" element={user?.isAdmin ? <CadastrosPendentes /> : <Navigate to="/login" />} />

        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    );
  }

  export default AppRoutes;
