document.getElementById("loginForm").addEventListener("submit", function (e) {
  e.preventDefault();

  const username = document.getElementById("username").value;
  const password = document.getElementById("password").value;

  // Отправка данных на сервер для проверки
  fetch("/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ username, password }),
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.success) {
        // Если успешная авторизация, перенаправляем на index.html
        window.location.href = "/index.html";
      } else {
        // Показать сообщение об ошибке
        document.getElementById("error").style.display = "block";
      }
    })
    .catch((error) => {
      console.error("Error:", error);
    });
});
