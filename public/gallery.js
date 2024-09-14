document.addEventListener("DOMContentLoaded", () => {
  const gallery = document.getElementById("gallery");

  // Функция для загрузки списка файлов
  async function loadFiles() {
    try {
      const response = await fetch("/files");
      if (!response.ok) throw new Error("Ошибка загрузки файлов");

      const files = await response.json();
      displayFiles(files);
    } catch (error) {
      console.error(error);
      gallery.innerHTML = "Не удалось загрузить файлы.";
    }
  }

  // Функция для отображения файлов
  function displayFiles(files) {
    gallery.innerHTML = "";
    files.forEach((file) => {
      const fileItem = document.createElement("div");
      fileItem.className = "file-item";
      fileItem.innerHTML = `
                <img src="/documents/${file}" class="file-thumbnail" alt="${file}">
                <div class="file-actions">
                    <a href="/documents/${file}" class="download-button" download>Скачать</a>
                    <button class="delete-button" data-file="${file}">Удалить</button>
                </div>
            `;
      gallery.appendChild(fileItem);
    });
  }

  // Функция для удаления файла
  gallery.addEventListener("click", async (event) => {
    if (event.target.classList.contains("delete-button")) {
      const fileName = event.target.getAttribute("data-file");
      if (confirm("Вы уверены, что хотите удалить этот файл?")) {
        try {
          const response = await fetch(`/delete-file/${fileName}`, {
            method: "DELETE",
          });
          if (!response.ok) throw new Error("Ошибка удаления файла");

          alert("Файл удален успешно");
          loadFiles(); // Перезагружаем список файлов
        } catch (error) {
          console.error(error);
          alert("Не удалось удалить файл");
        }
      }
    }
  });

  loadFiles(); // Загружаем файлы при загрузке страницы
});
