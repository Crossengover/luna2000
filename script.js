import PizZip from "pizzip";
import Docxtemplater from "docxtemplater";

document.getElementById("generate").addEventListener("click", function () {
  const fileInput = document.getElementById("doc-upload");
  const name = document.getElementById("name").value;
  const email = document.getElementById("email").value;

  if (fileInput.files.length === 0) {
    alert("Пожалуйста, загрузите Word файл");
    return;
  }

  const reader = new FileReader();

  reader.onload = function (event) {
    try {
      const content = event.target.result;
      const zip = new PizZip(content);
      const doc = new Docxtemplater(zip);

      // Данные для замены в документе
      const data = {
        name: name,
        email: email,
      };

      // Устанавливаем данные для замены
      doc.setData(data);

      // Рендерим документ
      doc.render();

      // Генерируем итоговый файл
      const out = doc.getZip().generate({
        type: "blob",
        mimeType:
          "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
      });

      // Создаем ссылку для скачивания документа
      const link = document.createElement("a");
      link.href = URL.createObjectURL(out);
      link.download = "filled_document.docx";
      link.click();
    } catch (error) {
      console.error("Error during file processing:", error);
    }
  };

  // Читаем файл как бинарные данные
  reader.readAsArrayBuffer(fileInput.files[0]);
});
