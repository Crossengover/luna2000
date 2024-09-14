document.addEventListener("DOMContentLoaded", () => {
  const uploadForm = document.getElementById("uploadForm");
  const fileInput = document.getElementById("fileInput");
  const resultDiv = document.getElementById("result");

  if (!fileInput) {
    console.error('Элемент с id="fileInput" не найден');
    return;
  }

  uploadForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    if (!fileInput.files.length) {
      resultDiv.textContent = "Пожалуйста, выберите файл для загрузки.";
      return;
    }

    const formData = new FormData();
    formData.append("file", fileInput.files[0]);

    try {
      const response = await fetch("/upload", {
        method: "POST",
        body: formData,
      });

      const result = await response.json();
      if (response.ok) {
        resultDiv.textContent = result.message;
      } else {
        resultDiv.textContent = "Ошибка: " + result.message;
      }
    } catch (error) {
      resultDiv.textContent = "Ошибка: " + error.message;
    }
  });
});
