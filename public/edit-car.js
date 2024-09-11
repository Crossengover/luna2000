document.addEventListener("DOMContentLoaded", function () {
  const urlParams = new URLSearchParams(window.location.search);
  const carId = urlParams.get("id");

  fetch(`/data/cars.json`)
    .then((response) => response.json())
    .then((cars) => {
      const car = cars.find((c) => c.id === carId);
      if (car) {
        document.getElementById("carId").value = car.id;
        document.getElementById("brandModel").value = car.brandModel;
        document.getElementById("pts").value = car.pts;
        document.getElementById("sts").value = car.sts;
        document.getElementById("vin").value = car.vin;
        document.getElementById("year").value = car.year;
        document.getElementById("registrationDate").value =
          car.registrationDate;
        document.getElementById("plateNumber").value = car.plateNumber;
        document.getElementById("osago").value = car.osago;
        document.getElementById("kasko").value = car.kasko;
        document.getElementById("techInspection").value = car.techInspection;
        document.getElementById("taxiLicense").checked = car.taxiLicense;
        document.getElementById("purchaseOrRent").value = car.purchaseOrRent;
        document.getElementById("leasing").checked = car.leasing;

        const existingPhotosDiv = document.getElementById("existingPhotos");
        car.photos.forEach((photo) => {
          const img = document.createElement("img");
          img.src = `/public/images/${photo}`;
          img.style.width = "100px";
          img.style.height = "auto";
          existingPhotosDiv.appendChild(img);
        });
      } else {
        console.error("Автомобиль не найден");
      }
    })
    .catch((error) => console.error("Ошибка загрузки данных:", error));

  function redirectToViewCars() {
    window.location.href = "view-cars.html";
  }

  document
    .getElementById("editCarForm")
    .addEventListener("submit", function (event) {
      event.preventDefault();

      const formData = new FormData();
      formData.append("id", document.getElementById("carId").value);
      formData.append(
        "brandModel",
        document.getElementById("brandModel").value
      );
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

      const photoFiles = document.getElementById("photos").files;
      for (let i = 0; i < photoFiles.length; i++) {
        formData.append("photos", photoFiles[i]);
      }

      fetch(`/edit-car/${document.getElementById("carId").value}`, {
        method: "POST",
        body: formData,
      })
        .then((response) => response.json())
        .then((data) => {
          if (data.success) {
            alert("Информация об автомобиле успешно обновлена!");
            redirectToViewCars(); // Перенаправление после успешного обновления
          } else {
            alert("Ошибка при обновлении информации об автомобиле.");
          }
        })
        .catch((error) =>
          console.error("Ошибка при обновлении информации:", error)
        );
    });
});
