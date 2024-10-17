document
  .getElementById("addCarForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    // Создаем объект FormData для отправки файлов
    const formData = new FormData();

    // Добавляем данные из формы в formData
    formData.append(
      "id",
      Math.floor(Math.random() * (200000 - 100000)) + 100000
    );
    formData.append("brandModel", document.getElementById("brandModel").value);
    formData.append("pts", document.getElementById("pts").value);
    formData.append("sts", document.getElementById("sts").value);
    formData.append("vin", document.getElementById("vin").value);
    formData.append("year", document.getElementById("year").value);
    formData.append(
      "registrationDate",
      document.getElementById("registrationDate").value
    );
    formData.append(
      "plateNumber",
      document.getElementById("plateNumber").value
    );
    formData.append("osago", document.getElementById("osago").value);
    formData.append("kasko", document.getElementById("kasko").value);
    formData.append(
      "techInspection",
      document.getElementById("techInspection").value
    );
    formData.append(
      "taxiLicense",
      document.getElementById("taxiLicense").checked
    );
    formData.append(
      "purchaseOrRent",
      document.getElementById("purchaseOrRent").value
    );
    formData.append("leasing", document.getElementById("leasing").checked);

    // Добавляем фотографии в formData
    const photoFiles = document.getElementById("photos").files;
    for (let i = 0; i < photoFiles.length; i++) {
      formData.append("photos", photoFiles[i]);
    }

    // Отправляем данные на сервер
    fetch("/save-car", {
      method: "POST",
      body: formData, // Отправляем formData
    })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
            alert("Автомобиль успешно добавлен!");
            location.replace("/car");
          document.getElementById("addCarForm").reset();
        } else {
          alert("Ошибка при добавлении автомобиля.");
        }
      })
      .catch((error) => {
        console.error("Ошибка:", error);
        alert("Ошибка при добавлении автомобиля.");
      });
  });
