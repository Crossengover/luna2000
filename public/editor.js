document.addEventListener("DOMContentLoaded", () => {
  const templateSelect = document.getElementById("template");
  const driverSelect = document.getElementById("driver");
  const carSelect = document.getElementById("car");

  // Загружаем шаблоны
  fetch("/templates")
    .then((response) => response.json())
    .then((data) => {
      data.templates.forEach((template) => {
        const option = document.createElement("option");
        option.value = template;
        option.textContent = template;
        templateSelect.appendChild(option);
      });
    });

  // Загружаем водителей
  fetch("/drivers")
    .then((response) => response.json())
    .then((data) => {
      data.forEach((driver) => {
        const option = document.createElement("option");
        option.value = driver.id;
        option.textContent = driver.fio;
        driverSelect.appendChild(option);
      });
    });

  // Загружаем машины
  fetch("/cars")
    .then((response) => response.json())
    .then((data) => {
      data.forEach((car) => {
        const option = document.createElement("option");
        option.value = car.id;
        option.textContent = `${car.brandModel} (${car.plateNumber})`;
        carSelect.appendChild(option);
      });
    });
});
