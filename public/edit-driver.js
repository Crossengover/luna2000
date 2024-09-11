document.addEventListener("DOMContentLoaded", function () {
  const urlParams = new URLSearchParams(window.location.search);
  const driverId = urlParams.get("id");

  fetch(`/data/drivers.json`)
    .then((response) => response.json())
    .then((drivers) => {
      const driver = drivers.find((d) => d.id === driverId);
      if (driver) {
        document.getElementById("driverId").value = driver.id;
        document.getElementById("fio").value = driver.fio;
        document.getElementById("passport").value = driver.passport;
        document.getElementById("registration").value = driver.registration;
        document.getElementById("address").value = driver.address;
        document.getElementById("driverLicense").value = driver.driverLicense;
        document.getElementById("parkingAddress").value = driver.parkingAddress;
        document.getElementById("contacts").value = driver.contacts;
        document.getElementById("note").value = driver.note;

        const existingPhotosDiv = document.getElementById("existingPhotos");
        driver.photos.forEach((photo) => {
          const img = document.createElement("img");
          img.src = `/images/${photo}`;
          img.style.width = "100px";
          img.style.height = "auto";
          existingPhotosDiv.appendChild(img);
        });
      } else {
        console.error("Водитель не найден");
      }
    })
    .catch((error) => console.error("Ошибка загрузки данных:", error));

  document
    .getElementById("editDriverForm")
    .addEventListener("submit", function (event) {
      event.preventDefault();

      const formData = new FormData();
      formData.append("id", document.getElementById("driverId").value);
      formData.append("fio", document.getElementById("fio").value);
      formData.append("passport", document.getElementById("passport").value);
      formData.append(
        "registration",
        document.getElementById("registration").value
      );
      formData.append("address", document.getElementById("address").value);
      formData.append(
        "driverLicense",
        document.getElementById("driverLicense").value
      );
      formData.append(
        "parkingAddress",
        document.getElementById("parkingAddress").value
      );
      formData.append("contacts", document.getElementById("contacts").value);
      formData.append("note", document.getElementById("note").value);

      const photoFiles = document.getElementById("photos").files;
      for (let i = 0; i < photoFiles.length; i++) {
        formData.append("photos", photoFiles[i]);
      }

      fetch(`/edit-driver/${document.getElementById("driverId").value}`, {
        method: "POST",
        body: formData,
      })
        .then((response) => response.json())
        .then((data) => {
          if (data.success) {
            alert("Информация о водителе успешно обновлена!");
            window.location.href = "view-drivers.html"; // Перенаправление на страницу со списком водителей
          } else {
            alert("Ошибка при обновлении информации о водителе.");
          }
        })
        .catch((error) =>
          console.error("Ошибка при обновлении информации:", error)
        );
    });
});
