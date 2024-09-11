const express = require("express");
const path = require("path");
const fs = require("fs");
const multer = require("multer");
const app = express();

// Настройка парсера для JSON
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
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
  if (!req.files) {
    return res
      .status(400)
      .json({ success: false, message: "Файлы не загружены." });
  }

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
    taxiLicense: req.body.taxiLicense === "true",
    purchaseOrRent: req.body.purchaseOrRent,
    leasing: req.body.leasing === "true",
    photos: req.files.map((file) => file.filename),
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
// Обработка POST-запроса для добавления водителя
app.post("/save-driver", upload.array("photos", 4), (req, res) => {
  // Проверка и получение данных из формы
  const newDriver = {
    id: req.body.id,
    fio: req.body.fio,
    passport: req.body.passport,
    registration: req.body.registration,
    address: req.body.address,
    driverLicense: req.body.driverLicense,
    parkingAddress: req.body.parkingAddress,
    contacts: req.body.contacts,
    note: req.body.note,
    photos: req.files.map((file) => file.filename), // Обработка загруженных файлов
  };

  // Чтение существующих данных из файла
  fs.readFile("data/drivers.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let drivers = [];
    if (data) {
      drivers = JSON.parse(data);
    }

    // Добавление нового водителя
    drivers.push(newDriver);

    // Запись обновленных данных обратно в файл
    fs.writeFile(
      "data/drivers.json",
      JSON.stringify(drivers, null, 2),
      (err) => {
        if (err) {
          console.error("Ошибка записи файла:", err);
          return res
            .status(500)
            .json({ success: false, message: "Ошибка сервера" });
        }

        // Отправка успешного ответа клиенту
        res.json({ success: true });
      }
    );
  });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Сервер запущен на http://localhost:${PORT}`);
});
