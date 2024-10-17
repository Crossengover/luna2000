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

    function deletePair(id) {
        fetch("/home/deletePair", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(id),
        }).then(() => {
            location.reload();
            loadTableData(); // Обновление таблицы
            pairForm.reset(); // Очистка формы
            saveButton.textContent = "Добавить пару";
            pairIdInput.value = "";
        });
    }

    // Загрузка данных из main.json и отображение в таблице
    function loadTableData() {
        // Добавление обработчиков для кнопок
        document.querySelectorAll(".edit-button").forEach((button) => {
            button.addEventListener("click", () => {
                const id = button.dataset.id;
                const driver = button.dataset.driver;
                const car = button.dataset.car;
                const rent = button.dataset.rent;
                if (id) {
                    document.getElementById("rent").value = rent;
                    document.getElementById("driverSelect").value = driver;
                    document.getElementById("carSelect").value = car;
                    document.getElementById("saveButton").id = "editButton";
                    pairIdInput.value = id;
                    saveButton.textContent = "Сохранить изменения";
                }
            });
        });

        document.querySelectorAll(".delete-button").forEach((button) => {
            button.addEventListener("click", () => {
                const id = button.dataset.id;
                deletePair(id);
            });
        });


        fetch("/data/main.json")
            .then((response) => response.json())
            .then((data) => {

            });
    }

    // Сохранение данных в main.json
    function saveData(newPair) {
        fetch("/home/addRent", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newPair),
        }).then(() => {
            location.reload();
            loadTableData(); // Обновление таблицы
            pairForm.reset(); // Очистка формы
            saveButton.textContent = "Добавить пару";
            pairIdInput.value = "";
        });
    }

    function updateData(newData) {
        fetch("/home/updateRent", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newData),
        }).then(() => {
            location.reload();
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
        const rent = document.getElementById("rent").value;

        const editRentId = document.getElementById("pairId").value;

        if (editRentId) {
            updateData({
                editRentId,
                rent
            })
            return;
        }

        // Добавление новой пары
        const newPair = {
            driverId,
            carId,
            rent
        };

        saveData(newPair);
    });

    function deductRent() {
        fetch("/home/deductRent", {
            method: "GET",
        }).then(() => {
            location.reload();
            loadTableData(); // Обновление таблицы
            pairForm.reset(); // Очистка формы
            saveButton.textContent = "Добавить пару";
            pairIdInput.value = "";
        });
    }

    // Обработка кнопки "Списать аренду"
    writeOffButton.addEventListener("click", () => {
        deductRent();
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
