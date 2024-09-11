document.addEventListener("DOMContentLoaded", () => {
  const driverSelect = document.getElementById("driverSelect");
  const carSelect = document.getElementById("carSelect");
  const mainTableBody = document.querySelector("#mainTable tbody");
  const pairForm = document.getElementById("pairForm");
  const saveButton = document.getElementById("saveButton");
  const cancelButton = document.getElementById("cancelButton");
  const pairIdInput = document.getElementById("pairId");
  const writeOffButton = document.getElementById("writeOffButton");

  let pairs = []; // Переменная для хранения данных пар

  // Загрузка данных водителей и машин
  function loadOptions() {
    fetch("/data/drivers.json")
      .then((response) => response.json())
      .then((drivers) => {
        drivers.forEach((driver) => {
          const option = document.createElement("option");
          option.value = driver.id;
          option.textContent = driver.fio;
          driverSelect.appendChild(option);
        });
      });

    fetch("/data/cars.json")
      .then((response) => response.json())
      .then((cars) => {
        cars.forEach((car) => {
          const option = document.createElement("option");
          option.value = car.id;
          option.textContent = `${car.brandModel} (${car.plateNumber})`;
          carSelect.appendChild(option);
        });
      });
  }

  // Загрузка данных из main.json и отображение в таблице
  function loadTableData() {
    fetch("/data/main.json")
      .then((response) => response.json())
      .then((data) => {
        pairs = data; // Сохранение данных в переменную
        mainTableBody.innerHTML = ""; // Очистка таблицы
        pairs.forEach((pair) => {
          const row = document.createElement("tr");
          row.innerHTML = `
              <td>${pair.driverName}</td>
              <td>${pair.carName}</td>
              <td><input type="number" class="balance" value="${pair.balance}" readonly /></td>
              <td><input type="number" class="rent" value="${pair.rent}" readonly /></td>
              <td>
                <button class="edit-button" data-id="${pair.id}">Редактировать</button>
                <button class="delete-button" data-id="${pair.id}">Удалить</button>
              </td>
            `;
          mainTableBody.appendChild(row);
        });

        // Добавление обработчиков для кнопок
        document.querySelectorAll(".edit-button").forEach((button) => {
          button.addEventListener("click", () => {
            const id = button.dataset.id;
            const pair = pairs.find((p) => p.id == id);
            if (pair) {
              driverSelect.value = pair.driverId;
              carSelect.value = pair.carId;
              document.getElementById("balance").value = pair.balance;
              document.getElementById("rent").value = pair.rent;
              pairIdInput.value = id;
              saveButton.textContent = "Сохранить изменения";
            }
          });
        });

        document.querySelectorAll(".delete-button").forEach((button) => {
          button.addEventListener("click", () => {
            const id = button.dataset.id;
            pairs = pairs.filter((p) => p.id != id);
            saveData();
          });
        });
      });
  }

  // Сохранение данных в main.json
  function saveData() {
    fetch("/save-main", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(pairs),
    }).then(() => {
      loadTableData(); // Обновление таблицы
      pairForm.reset(); // Очистка формы
      saveButton.textContent = "Добавить пару";
      pairIdInput.value = "";
    });
  }

  // Обработка отправки формы
  pairForm.addEventListener("submit", (event) => {
    event.preventDefault();

    const driverId = driverSelect.value;
    const carId = carSelect.value;
    const balance = document.getElementById("balance").value;
    const rent = document.getElementById("rent").value;

    const driverName = driverSelect.options[driverSelect.selectedIndex].text;
    const carName = carSelect.options[carSelect.selectedIndex].text;
    const id = pairIdInput.value;

    if (id) {
      // Редактирование существующей пары
      const pair = pairs.find((p) => p.id == id);
      if (pair) {
        pair.driverId = driverId;
        pair.carId = carId;
        pair.driverName = driverName;
        pair.carName = carName;
        pair.balance = balance;
        pair.rent = rent;
      }
    } else {
      // Добавление новой пары
      const newPair = {
        id: Date.now(), // Генерация уникального ID
        driverId,
        carId,
        driverName,
        carName,
        balance,
        rent,
      };
      pairs.push(newPair);
    }

    saveData();
  });

  // Обработка кнопки "Списать аренду"
  writeOffButton.addEventListener("click", () => {
    pairs.forEach((pair) => {
      pair.balance = (pair.balance - pair.rent).toFixed(2); // Обновление баланса
    });
    saveData();
  });

  // Отмена редактирования
  cancelButton.addEventListener("click", () => {
    pairForm.reset();
    saveButton.textContent = "Добавить пару";
    pairIdInput.value = "";
  });

  loadOptions();
  loadTableData();
});
