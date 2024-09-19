document.addEventListener("DOMContentLoaded", () => {
  const historyTableBody = document.querySelector("#historyTable tbody");

  async function fetchAndUpdateHistory() {
    try {
      const response = await fetch("/history");
      const data = await response.json();

      if (response.ok) {
        // Очистить текущие строки таблицы
        historyTableBody.innerHTML = "";

        // Обновить таблицу новыми данными
        data.forEach((record) => {
          const row = document.createElement("tr");

          row.innerHTML = `
              <td>${record.driverName}</td>
              <td>${record.balanceBefore}</td>
              <td>${record.balanceAfter}</td>
              <td>${new Date(record.timestamp).toLocaleString()}</td>
            `;

          historyTableBody.appendChild(row);
        });
      } else {
        console.error("Ошибка загрузки данных: " + data.message);
      }
    } catch (error) {
      console.error("Ошибка: " + error.message);
    }
  }

  // Первоначальная загрузка данных
  fetchAndUpdateHistory();

  // Обновление данных каждые 2 часа (7200000 миллисекунд)
  setInterval(fetchAndUpdateHistory, 7);
});
