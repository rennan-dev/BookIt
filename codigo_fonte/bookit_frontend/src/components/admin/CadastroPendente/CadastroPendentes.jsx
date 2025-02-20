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
        const headers = {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        };

        const response = await fetch("http://localhost:5092/api/User/pendentes", {
          method: "GET",
          headers: headers,
        });

        if (!response.ok) {
          throw new Error(`Erro ${response.status}: ${await response.text()}`);
        }

        const data = await response.json();
        setPendentes(data);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchPendentes();
  }, []);

  if (loading) return <div>Carregando...</div>;
  if (error) return <div>Erro: {error}</div>;

  return (
    <div className="cadastro-container">
      <h1>Cadastros Pendentes</h1>
      <fieldset>
        {pendentes.length > 0 ? (
          <table>
            <thead>
              <tr>
                <th>Nome</th>
                <th>CPF</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {pendentes.map((usuario) => (
                <tr key={usuario.id}>
                  <td>{usuario.nome}</td>
                  <td>{usuario.cpf}</td>
                  <td>
                    <button className="visualizar">Visualizar</button>
                    <button className="aprovar">Aprovado</button>
                    <button className="reprovar">Reprovado</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <p>Não há cadastros pendentes.</p>
        )}
      </fieldset>
    </div>
  );
};

export default CadastroPendentes;