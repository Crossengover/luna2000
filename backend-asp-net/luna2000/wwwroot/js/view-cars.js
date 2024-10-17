document.addEventListener("DOMContentLoaded", function () {
  fetch("/data/cars.json")
    .then((response) => response.json())
    .then((cars) => {
      const tableBody = document.querySelector("#carsTable tbody");

      cars.forEach((car) => {
        const row = document.createElement("tr");

        row.innerHTML = `
                     <td>
                        ${car.photos
                          .map(
                            (photo) =>
                              `<img src="/images/${photo}" alt="${car.brandModel}" style="width: 100px; height: auto;">`
                          )
                          .join("")}
                    </td>
                    <td>${car.brandModel}</td>
                    <td>${car.pts}</td>
                    <td>${car.sts}</td>
                    <td>${car.vin}</td>
                    <td>${car.year}</td>
                    <td>${car.registrationDate}</td>
                    <td>${car.plateNumber}</td>
                    <td>${car.osago}</td>
                    <td>${car.kasko}</td>
                    <td>${car.techInspection}</td>
                    <td>${car.taxiLicense ? "Да" : "Нет"}</td>
                    <td>${car.purchaseOrRent}</td>
                    <td>${car.leasing ? "Да" : "Нет"}</td>
                    <td>
                        <button onclick="editCar('${
                          car.id
                        }')">Редактировать</button>
                        <button onclick="deleteCar('${
                          car.id
                        }')">Удалить</button>
                    </td>
                `;

        tableBody.appendChild(row);
      });
    })
    .catch((error) => {
      console.error("Ошибка загрузки данных:", error);
    });
});

function editCar(id) {
  window.location.href = `/car/edit?id=${id}`;
}

function deleteCar(id) {
  if (confirm("Вы уверены, что хотите удалить этот автомобиль?")) {
    fetch(`/car/delete/${id}`, {
      method: "DELETE",
    })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
          alert("Автомобиль успешно удален!");
          window.location.reload(); // Перезагружаем страницу
        } else {
          alert("Ошибка при удалении автомобиля.");
        }
      })
      .catch((error) => {
        console.error("Ошибка:", error);
        alert("Ошибка при удалении автомобиля.");
      });
  }
}
