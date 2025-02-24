import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./ReservasCadastradas.css";

const ReservasCadastradas = () => {
  const { data, ambiente } = useParams();
  const [horariosReservados, setHorariosReservados] = useState([]);

  const horarios = [
    "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00",
    "16:00", "17:00", "18:00", "19:00", "20:00", "21:00"
  ];

  useEffect(() => {
    const fetchReservas = async () => {
      try {
        const response = await fetch(`http://localhost:5092/api/User/reservas/${data}/${ambiente}`);
        if(!response.ok) {
          if(response.status === 404) {
            console.log("Nenhuma reserva encontrada para esse dia e ambiente.");
            return;
          }
          throw new Error(`Erro ao buscar reservas: ${await response.text()}`);
        }

        const dataReservas = await response.json();
        const horariosOcupados = dataReservas.flatMap((reserva) => 
          reserva.horarios.split(",").map((horario) => ({
            horario,
            nome: reserva.usuario.name,
            cpf: reserva.usuario.cpf
          }))
        );

        setHorariosReservados(horariosOcupados);
      } catch (error) {
        console.error("Erro ao buscar reservas:", error);
      }
    };

    fetchReservas();
  }, [data, ambiente]);

  return (
    <div className="container-cadastrar-reserva">
      <div className="header-cadastrar-reserva">
        <h1>{ambiente}</h1>
        <span className="data">
          Data: {data.split("-").reverse().map(num => num.padStart(2, "0")).join(" / ")}
        </span>
      </div>
      <hr />
      <h2>Horário(s) reservado(s):</h2>
      <table className="table-cadastrar-reserva">
        <thead>
          <tr>
            <th>Horários</th>
            <th className="col-nome">Nomes</th>
            <th>CPF</th>
          </tr>
        </thead>
        <tbody>
          {horarios.map((hora, index) => {
            const reserva = horariosReservados.find(reserva => reserva.horario === hora);
            return (
              <tr key={index} className={reserva ? "reservado" : ""}>
                <td>{hora}</td>
                <td>{reserva ? reserva.nome : ""}</td>
                <td>{reserva ? reserva.cpf : ""}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default ReservasCadastradas;