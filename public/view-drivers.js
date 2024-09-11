document.addEventListener("DOMContentLoaded", function () {
  fetch("/data/drivers.json")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Ошибка загрузки данных: " + response.statusText);
      }
      return response.json();
    })
    .then((drivers) => {
      const tableBody = document.querySelector("#driversTable tbody");
      tableBody.innerHTML = ""; // Очистка таблицы перед добавлением новых данных

      drivers.forEach((driver) => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${driver.fio}</td>
            <td>${driver.passport}</td>
            <td>${driver.registration}</td>
            <td>${driver.address}</td>
            <td>${driver.driverLicense}</td>
            <td>${driver.parkingAddress}</td>
            <td>${driver.contacts}</td>
            <td>${driver.note}</td>
            <td>
              ${driver.photos
                .map(
                  (photo) =>
                    `<img src="/images/${photo}" style="width: 100px; height: auto;">`
                )
                .join("<br>")}
            </td>
            <td>
              <button onclick="editDriver('${
                driver.id
              }')">Редактировать</button>
              <button onclick="deleteDriver('${driver.id}')">Удалить</button>
            </td>
          `;

        tableBody.appendChild(row);
      });
    })
    .catch((error) => console.error("Ошибка загрузки данных:", error));
});

function editDriver(id) {
  window.location.href = `edit-driver.html?id=${id}`;
}

function deleteDriver(id) {
  if (confirm("Вы уверены, что хотите удалить этого водителя?")) {
    fetch(`/delete-driver/${id}`, { method: "DELETE" })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
          alert("Водитель успешно удален!");
          location.reload(); // Перезагрузить страницу, чтобы обновить список
        } else {
          alert("Ошибка при удалении водителя.");
        }
      })
      .catch((error) => console.error("Ошибка при удалении водителя:", error));
  }
}
