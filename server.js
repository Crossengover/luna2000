const express = require("express");
const path = require("path");
const fs = require("fs");
const multer = require("multer");
const app = express();

// Настройка парсера для JSON
app.use(express.json());
app.use(express.urlencoded({ extended: true })); // Для обработки URL-кодированных данных
app.use(express.static(path.join(__dirname, "public")));

// Настройка multer для загрузки файлов
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, path.join(__dirname, "public", "images")); // Папка для сохранения изображений
  },
  filename: (req, file, cb) => {
    cb(null, Date.now() + path.extname(file.originalname)); // Уникальное имя файла
  },
});
const upload = multer({ storage });

// Обработчик для сохранения автомобиля
app.post("/save-car", upload.array("photos", 10), (req, res) => {
  // Проверка наличия файлов
  if (!req.files) {
    return res
      .status(400)
      .json({ success: false, message: "Файлы не загружены." });
  }

  // Формируем объект нового автомобиля
  const newCar = {
    id: req.body.id,
    brandModel: req.body.brandModel,
    pts: req.body.pts,
    sts: req.body.sts,
    vin: req.body.vin,
    year: req.body.year,
    registrationDate: req.body.registrationDate,
    plateNumber: req.body.plateNumber,
    osago: req.body.osago,
    kasko: req.body.kasko,
    techInspection: req.body.techInspection,
    taxiLicense: req.body.taxiLicense === "true", // Преобразование в boolean
    purchaseOrRent: req.body.purchaseOrRent,
    leasing: req.body.leasing === "true", // Преобразование в boolean
    photos: req.files.map((file) => file.filename), // Сохраненные имена файлов
  };

  fs.readFile(
    path.join(__dirname, "data", "cars.json"),
    "utf-8",
    (err, data) => {
      if (err) {
        console.error("Ошибка чтения файла:", err);
        return res
          .status(500)
          .json({ success: false, message: "Ошибка чтения файла." });
      }

      const cars = JSON.parse(data);
      cars.push(newCar);

      fs.writeFile(
        path.join(__dirname, "data", "cars.json"),
        JSON.stringify(cars, null, 2),
        (err) => {
          if (err) {
            console.error("Ошибка записи в файл:", err);
            return res
              .status(500)
              .json({ success: false, message: "Ошибка записи в файл." });
          }

          res.json({ success: true });
        }
      );
    }
  );
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Сервер запущен на http://localhost:${PORT}`);
});
