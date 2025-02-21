import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import './CadastrarReserva.css';

const CadastrarReserva = () => {
  const { data, ambiente } = useParams(); 
  const [reservas, setReservas] = useState(
    Array(14).fill(null).map(() => ({ nome: '', cpf: '', reservado: false })) 
  );

  const horarios = [
    '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00',
    '16:00', '17:00', '18:00', '19:00', '20:00', '21:00'
  ];

  const handleInputChange = (index, field, value) => {
    const newReservas = [...reservas];
    newReservas[index] = { ...newReservas[index], [field]: value };
    setReservas(newReservas);
  };

  const handleCheckboxChange = (index) => {
    if (index === 4 || index === 5) return; //impede seleção de 12:00 e 13:00

    const newReservas = [...reservas];
    newReservas[index].reservado = !newReservas[index].reservado; 
    setReservas(newReservas);
  };

  return (
    <div className="container-cadastrar-reserva">
      <div className="header-cadastrar-reserva">
        <h1>{ambiente}</h1> {/*exibindo o ambiente dinamicamente */}
        <span className="data">Data: {data.split('-').reverse().join(' / ')}</span>
      </div>
      <hr />
      <h2>Selecione o(s) horário(s) desejado(s) para a reserva:</h2>
      <table className="table-cadastrar-reserva">
        <thead>
          <tr>
            <th className="col-horario">Horários</th>
            <th className="col-nome">Nomes</th>
            <th>CPF</th>
          </tr>
        </thead>
        <tbody>
          {horarios.map((hora, index) => (
            <tr key={index} className={reservas[index].reservado ? 'reservado' : ''}>
              <td>
                <input
                  type="checkbox"
                  checked={reservas[index].reservado}
                  onChange={() => handleCheckboxChange(index)}
                  disabled={index === 4 || index === 5} //desabilita as checkboxes de 12:00 e 13:00
                />
                {hora}
              </td>
              <td>{reservas[index].nome}</td>
              <td>{reservas[index].cpf}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="button-container">
        <button className="button-cadastrar-reserva" onClick={() => alert('Reservas cadastradas!')}>
          Reservar
        </button>
      </div>
    </div>
  );
};

export default CadastrarReserva;