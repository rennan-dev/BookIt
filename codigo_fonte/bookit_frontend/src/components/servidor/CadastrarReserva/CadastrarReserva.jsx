import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import "./CadastrarReserva.css";

const CadastrarReserva = () => {
  const { data, ambiente } = useParams();
  const [reservas, setReservas] = useState(
    Array(14).fill(null).map(() => ({ nome: "", cpf: "", reservado: false }))
  );
  const [horariosReservados, setHorariosReservados] = useState([]);
  const [selecionarTodos, setSelecionarTodos] = useState(false);

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
      }catch(error) {
        console.error("Erro ao buscar reservas:", error);
      }
    };

    fetchReservas();
  }, [data, ambiente]);

  const handleCheckboxChange = (index) => {
    if(index === 4 || index === 5) return;
    const newReservas = [...reservas];
    newReservas[index].reservado = !newReservas[index].reservado;
    setReservas(newReservas);
  };

  const handleSelecionarTodos = () => {
    setSelecionarTodos(!selecionarTodos);
    const newReservas = reservas.map((reserva, index) => {
      if(!horariosReservados.some((r) => r.horario === horarios[index]) && index !== 4 && index !== 5) {
        return { ...reserva, reservado: !selecionarTodos };
      }
      return reserva;
    });
    setReservas(newReservas);
  };

  const reservarHorarios = async () => {
    const token = localStorage.getItem("token");
    const horariosSelecionados = reservas
      .map((reserva, index) => (reserva.reservado ? horarios[index] : null))
      .filter(Boolean);

    if(horariosSelecionados.length === 0) {
      alert("Selecione pelo menos um horário para reservar!");
      return;
    }

    let dataISO = null;
    try {
      const [ano, mes, dia] = data.split("-");
      const dataFormatada = new Date(`${ano}-${mes.padStart(2, "0")}-${dia.padStart(2, "0")}T00:00:00.000Z`);
      if(isNaN(dataFormatada.getTime())) {
        throw new Error("Falha ao converter a data");
      }
      dataISO = dataFormatada.toISOString();
    }catch(error) {
      console.error("Erro ao converter data:", error);
      alert("Erro ao processar a data. Verifique o formato.");
      return;
    }

    const reservaData = {
      tipo: ambiente,
      dataReserva: dataISO,
      horarios: horariosSelecionados.join(","),
    };

    try {
      const response = await fetch("http://localhost:5092/api/User/reservar", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(reservaData),
      });

      if(!response.ok) {
        throw new Error(`Erro ${response.status}: ${await response.text()}`);
      }

      alert("Reserva criada com sucesso!");
      window.location.reload();
    }catch(error) {
      alert(`Erro ao criar reserva: ${error.message}`);
    }
  };

  return (
    <div className="container-cadastrar-reserva">
      <div className="header-cadastrar-reserva">
        <h1>{ambiente}</h1>
        <span className="data">Data: {data.split("-").reverse().map(num => num.padStart(2, "0")).join(" / ")}</span>
      </div>
      <hr />
      <h2>Selecione o(s) horário(s) desejado(s) para a reserva:</h2>
      <table className="table-cadastrar-reserva">
        <thead>
          <tr>
            <th>
              <input type="checkbox" onChange={handleSelecionarTodos} checked={selecionarTodos} />
            </th>
            <th>Horários</th>
            <th className="col-nome">Nomes</th>
            <th>CPF</th>
          </tr>
        </thead>
        <tbody>
          {horarios.map((hora, index) => {
            const isReservado = horariosReservados.some(
              (reserva) => reserva.horario === hora
            );
            const reserva = horariosReservados.find(
              (reserva) => reserva.horario === hora
            );
            return (
              <tr key={index} className={isReservado ? "reservado" : ""}>
                <td>
                  <input
                    type="checkbox"
                    checked={reservas[index].reservado}
                    onChange={() => handleCheckboxChange(index)}
                    disabled={isReservado || index === 4 || index === 5}
                    style={{ cursor: isReservado ? "not-allowed" : "pointer" }}
                  />
                </td>
                <td>{hora}</td>
                <td>{reserva ? reserva.nome : ""}</td>
                <td>{reserva ? reserva.cpf : ""}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
      <div className="button-container">
        <button className="button-cadastrar-reserva" onClick={reservarHorarios}>
          Reservar
        </button>
      </div>
    </div>
  );
};

export default CadastrarReserva;