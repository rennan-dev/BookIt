import React, { useState, useEffect } from "react";
import "./CadastroPendentes.css";

const CadastroPendentes = () => {
  const [pendentes, setPendentes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPendentes = async () => {
      try {
        const token = localStorage.getItem("token");
        console.log("Token recuperado:", token); 

        const headers = {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        };

        console.log("Enviando requisição com headers:", headers);

        const response = await fetch("http://localhost:5092/api/User/pendentes", {
          method: "GET",
          headers: headers,
        });

        console.log("Status da resposta:", response.status);

        if (!response.ok) {
          const errorText = await response.text();
          console.error("Erro na resposta:", response.status, errorText);
          throw new Error(`Erro ${response.status}: ${errorText}`);
        }

        const data = await response.json();
        console.log("Dados recebidos:", data);

        setPendentes(data);
      } catch (error) {
        console.error("Erro na requisição:", error);
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchPendentes();
  }, []);

  if (loading) {
    return <div>Carregando...</div>;
  }

  if (error) {
    return <div>Erro: {error}</div>;
  }

  return (
    <div className="cadastro-pendentes">
      <h1>Cadastros Pendentes</h1>
      {pendentes.length > 0 ? (
        <ul>
          {pendentes.map((usuario) => (
            <li key={usuario.id}>
              <p>{usuario.nome}</p>
              <p>{usuario.cpf}</p>
              <p>Status: {usuario.status}</p>
            </li>
          ))}
        </ul>
      ) : (
        <p>Não há cadastros pendentes.</p>
      )}
    </div>
  );
};

export default CadastroPendentes;
