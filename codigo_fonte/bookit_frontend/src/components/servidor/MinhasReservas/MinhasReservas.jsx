import React, { useState, useEffect } from "react";
import "./MinhasReservas.css";

const MinhasReservas = () => {
  const [reservas, setReservas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const token = localStorage.getItem("token"); 

  useEffect(() => {
    const fetchReservas = async () => {
      try {
        const response = await fetch("http://localhost:5092/api/user/reservas", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`, 
          },
        });

        if(!response.ok) {
          throw new Error("Erro ao buscar reservas.");
        }

        const data = await response.json();
        setReservas(data);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchReservas();
  }, [token]);

  const excluirReserva = async (reservaId) => {
    try {
      const response = await fetch(`http://localhost:5092/api/User/reservas/${reservaId}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });
      
      if (response.ok) {
        setReservas(reservas.filter((reserva) => reserva.id !== reservaId));
        alert("Reserva excluída com sucesso!");
      } else {
        console.error(`Falha ao excluir a reserva com ID ${reservaId}. Código de status: ${response.status}`);
        throw new Error("Erro ao excluir reserva.");
      }
    } catch (error) {
      console.error(`Erro ao tentar excluir a reserva com ID ${reservaId}: ${error.message}`);
      alert(error.message);
    }
  };

  return (
    <div className="container-minhas-reservas">
      <h2>Minhas reservas</h2>
      <hr />
      {loading ? (
        <p>Carregando...</p>
      ) : error ? (
        <p>{error}</p>
      ) : (
        <table className="reservas-table">
          <thead>
            <tr>
              <th>Reservas</th>
              <th>Data</th>
              <th>Horários Reservados</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {reservas.map((reserva, index) => {
              const dataFormatada = new Date(reserva.dataReserva).toLocaleDateString();

              // Pegando os horários e convertendo para um array
              const horariosArray = reserva.horarios
                ? reserva.horarios.split(",").map(h => h.trim())
                : [];

              // Juntando todos os horários em uma única string
              const horariosReservados = horariosArray.length > 0 ? horariosArray.join(", ") : "Nenhum horário reservado";

              return (
                <tr key={index}>
                  <td>{reserva.tipo}</td>
                  <td>{dataFormatada}</td>
                  <td>{horariosReservados}</td>
                  <td>
                    <button className="delete-button" onClick={() => excluirReserva(reserva.id)}> ❌ </button>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default MinhasReservas;
